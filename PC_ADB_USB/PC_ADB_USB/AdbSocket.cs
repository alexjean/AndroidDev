using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing.Imaging;

namespace PC_ADB_USB
{
    public partial class AdbSocket : Form
    {
        public AdbSocket()
        {
            InitializeComponent();
        }

        public delegate void OneStringDelegate(string msg);
        void Message(string msg)
        {
            if (listView1.InvokeRequired)
            {
                this.Invoke(new OneStringDelegate(this.Message), new object[] { msg });
                return;
            }
            listView1.Items.Add(msg);
            Application.DoEvents();
        }

        void ExecuteAdb(string args)
        {
            int lineCount = 0;
            Message("------ adb " + args+" ------");
            Process proc = new Process();
            ProcessStartInfo info = proc.StartInfo;
            info.FileName=@"D:\Android\sdk\platform-tools\adb";
            info.Arguments=args;
            info.ErrorDialog=false;
            info.CreateNoWindow=true;
            info.WindowStyle=ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            info.RedirectStandardOutput=true;
            info.RedirectStandardError=true;
            proc.Start();
            proc.WaitForExit();
            StreamReader SR = proc.StandardOutput;
            while (!SR.EndOfStream)  Message(lineCount++.ToString()+": "+SR.ReadLine());
            SR = proc.StandardError;
            while (!SR.EndOfStream)  Message(SR.ReadLine());
            proc.Close();
        }

        private void btnAdbBlackMapping_Click(object sender, EventArgs e)
        {
//            ExecuteAdb("shell am broadcast -a NotifyServiceStop");
            ExecuteAdb("-s ee5dcfec forward tcp:12580 tcp:10086");
//            ExecuteAdb("shell am broadcast -a NotifyServiceStart");
        }

        private void btnAdbWhiteMapping_Click(object sender, EventArgs e)
        {
            ExecuteAdb("-s d0c9fe14 forward tcp:12580 tcp:10086");
        }

        class OneEye
        {
            public TcpClient mTcpClient;
            public bool mRemainConnect = false;
            public bool mAlreadExit = false;
            public NetworkStream mStream = null;
            public PictureBox pbxOriginal;
            public PictureBox pbxGray;
            public IPEndPoint endPoint;
            public Bitmap imgGray;
            public Bitmap img;
            public image<double> imgDouble;

            public void takePicture()
            {
                byte[] takePicture = new byte[3] { 0xD5, 0xaa, 0x96 };
                if (mStream != null)
                    mStream.Write(takePicture, 0, 3);
            }
            public void Disconnect()
            {
                mRemainConnect = false;
                for (int i = 0; i < 300; i++)
                {
                    if (mAlreadExit) break;
                    Thread.Sleep(10);
                }
                try
                {
                    if (mTcpClient != null)
                        mTcpClient.Close();
                }
                catch { }
            }
        }

        OneEye mLeftEye, mRightEye;
        private void Run(object obj)
        {
            OneEye eye = (OneEye)obj;
            DateTime t1 = new DateTime(1999,1,1);
            const int bufSize = 8192000;
            byte[] buffer = new byte[bufSize];
            int len = 0, l = 0;
            string msg = "";
            eye.mRemainConnect = true;
            eye.mAlreadExit = false;
            bool waitHeader=true;
            int expectLen=0;
            while (eye.mRemainConnect)
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(10);
                        if (!eye.mStream.DataAvailable)
                        {
                            if (eye.mTcpClient.Client.Connected) continue;
                            eye.mRemainConnect = false;
                            Message("Socket Connect broken!");
                            break;
                        }
                        if (waitHeader) t1=DateTime.Now;       // 開始計時
                        l = eye.mStream.Read(buffer, len, bufSize - len);
                        if (l == 0) continue;
                        msg += " " + l.ToString();
                        len += l;
                        if (waitHeader)
                        {
                            if (HeaderFound(buffer, ref len, out expectLen))
                                waitHeader = false;
                        }
                        if (expectLen>0 && len >= expectLen) break;
                        i = 0;   // 剛讀一筆要等1秒沒資料才換下圖
                    }
                    if (!eye.mRemainConnect) break;  // while
                    if (len == 0) continue;
                    MemoryStream memStream;
                    double kb;
                    if (len > expectLen)
                    {
                        memStream = new MemoryStream(expectLen);
                        memStream.Write(buffer, 0, expectLen);
                        for (int i = expectLen; i < len; i++) buffer[i - expectLen] = buffer[i];
                        kb = Math.Round((double)expectLen / 1024, 1);
                    }
                    else
                    {
                        memStream = new MemoryStream(len);
                        memStream.Write(buffer, 0, len);
                        kb = Math.Round((double)len / 1024, 1);
                    }


                    ShowImageAndTime(t1, memStream,kb,msg,eye);
                    len = 0;
                    msg = "";
                    waitHeader = true;
                    expectLen = 0;
                }
                catch (Exception ex) { Message(ex.Message); }
            }
            eye.mAlreadExit = true;
        }

        // 找到 "Jpg"而且檢查Length是對的,共8個byte
        // 如果找到, return true並調整buffer及len切掉Header,傳回expectImageLength
        private bool HeaderFound(byte[] buffer, ref int len,out int expectImageLength)
        {
            expectImageLength = 0;
            int JPos=0;
            for (; JPos < len; JPos++)
            {
                if (buffer[JPos] != (byte)'J') continue;
                if ((JPos +8) > len) goto Unfinish;
                if (buffer[JPos+1] != (byte)'p') continue;
                if (buffer[JPos+2] != (byte)'g') continue;
                int xor = 88;
                for (int i = 0; i < 7; i++) xor ^= buffer[i+JPos];
                if ((byte)xor != buffer[JPos + 7]) continue;
                expectImageLength = buffer[6 + JPos];
                for(int i=5;i>=3;i--)
                {
                    int j=buffer[i+JPos];
                    expectImageLength *= 256;
                    expectImageLength +=j ;
                }
                JPos += 8;
                len = len - JPos;
                for (int i = 0; i < len; i++) buffer[i] = buffer[i + JPos];
                return true;
            }
        Unfinish:
            len = len - JPos;
            for (int i = 0; i < len; i++) buffer[i] = buffer[i + JPos];
            return false;
        }

        private void ShowImageAndTime(DateTime t1, MemoryStream imageStream,double kb, string msg,OneEye eye)
        {
            double span = Math.Round((DateTime.Now - t1).TotalSeconds, 2);
            Message(span.ToString() + " sec:" + msg + "=" + kb.ToString() + "k");
            try
            {
                Bitmap img =(Bitmap)Bitmap.FromStream(imageStream);
                eye.img = img;
                eye.pbxOriginal.Image = img;
                Message("Image " + kb.ToString() + "k Width=" + img.Size.Width.ToString() + " Height=" + img.Size.Height.ToString());
                toGray(img,eye);
            }
            catch (Exception ex)
            {
                Message("不是有效的圖形:" + ex.Message);
            }
        }

        private void toGray(Bitmap img,OneEye eye)
        {
            int h = img.Size.Height;
            int w = img.Size.Width;
            try
            {
                Bitmap gray = new Bitmap( w, h,PixelFormat.Format24bppRgb);
                eye.imgDouble = image<double>.newImage((uint)w, (uint)h);
                
                //Color c;
                byte gr;
                BitmapData dataIn = img.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData dataOut = gray.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* pIn = (byte*)(dataIn.Scan0.ToPointer());
                    byte* pOut = (byte*)(dataOut.Scan0.ToPointer());
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            //c = img.GetPixel(x , y);
                            //gr = (byte)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
                            //gray.SetPixel(x, y, Color.FromArgb(gr, gr, gr));
                            gr = (byte)(pIn[0] * 0.299 + pIn[1] * 0.587 + pIn[2] * 0.114);
                            eye.imgDouble.data[x, y] = gr;
                            pOut[0] = pOut[1] = pOut[2] = gr;
                            pIn  += 3;
                            pOut += 3;
                        }
                        pIn += dataIn.Stride - w * 3;
                        pOut += dataOut.Stride - w * 3;
                    }
                }
                gray.UnlockBits(dataOut);
                img.UnlockBits(dataIn);
                eye.imgGray = gray;
                eye.pbxGray.Image = gray;
            }
            catch (Exception ex) { Message("Make Gray:"+ex.Message); }
        }

        private void btnConnectLeft_Click(object sender, EventArgs e)
        {
            if (mLeftEye == null)
            {
                mLeftEye = new OneEye();
                mLeftEye.pbxOriginal = pictureBoxLeft;
                mLeftEye.pbxGray = pictureBoxGrayLeft;
                mLeftEye.endPoint = new IPEndPoint(IPAddress.Parse("192.168.31.133"), 10086);
            }
            RunTcpThread(mLeftEye);
        }

        private void btnConnectRight_Click(object sender, EventArgs e)
        {
            if (mRightEye == null)
            {
                mRightEye = new OneEye();
                mRightEye.pbxOriginal = pictureBoxRight;
                mRightEye.pbxGray = pictureBoxGrayRight;
                mRightEye.endPoint = new IPEndPoint(IPAddress.Parse("192.168.31.61"), 10086);
            }
            RunTcpThread(mRightEye);
        }


        void RunTcpThread(OneEye eye)
        {
            if (eye.mRemainConnect)
            {
                Message("己連線... 請先斷線!");
                return;
            }
            
            eye.mStream = null;
            eye.mTcpClient = new TcpClient();
            try
            {
                eye.mTcpClient.Connect(eye.endPoint);
                //mTcpClient.Connect(IPAddress.Parse("127.0.0.1"), 12580);
            }
            catch (Exception ex)
            {
                Message("連線失敗:" + ex.Message);
                return;
            }
            if (eye.mTcpClient != null)
                eye.mStream = eye.mTcpClient.GetStream();
            else
                eye.mStream = null;
            if (eye.mTcpClient != null && eye.mStream != null)
            {
                Message("Connect "+eye.mTcpClient.Client.RemoteEndPoint.ToString()+" Success!");
                eye.mRemainConnect = true;
                ParameterizedThreadStart parameterRun = new ParameterizedThreadStart(Run);
                Thread thread = new Thread(parameterRun);
                thread.Start(eye);
            }
            else
                Message("Connect Fail!");
        }

        private void btnDisconnectLeft_Click(object sender, EventArgs e)
        {
            if (mLeftEye == null) Message("左眼未啟始！");
            else
            {
                mLeftEye.Disconnect();
                Message("Disconnect Left!");
            }
        }

        private void btnDisconnectRight_Click(object sender, EventArgs e)
        {
            if (mRightEye == null) Message("右眼未啟始！");
            else
            {
                mRightEye.Disconnect();
                Message("Disconnect Right!");
            }

        }


        private void btnTakePictureLeft_Click(object sender, EventArgs e)
        {
            if (mLeftEye == null) Message("左眼未啟始！");
            else mLeftEye.takePicture();
        }

        private void btnTakePictureRight_Click(object sender, EventArgs e)
        {
            if (mRightEye == null) Message("右眼未啟始！");
            else mRightEye.takePicture();
        }

        private void btnTake2Picture_Click(object sender, EventArgs e)
        {
            if (mLeftEye == null)  { Message("左眼未啟始！"); return; }
            if (mRightEye == null) { Message("右眼未啟始！"); return; }
            mLeftEye.takePicture();
            mRightEye.takePicture();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void btnXorTwo_Click(object sender, EventArgs e)
        {
            if (mLeftEye == null || mRightEye == null)
            {
                Message("左右眼必需都啟始!");
                return;
            }
            Bitmap img=Vision.XorTwo(mLeftEye.imgGray, mRightEye.imgGray);
            pictureBoxBoth.Image = img;
        }

        private void btnGaussianRight_Click(object sender, EventArgs e)
        {
            Bitmap bitmap=DoGaussian(mRightEye);
            pictureBoxGaussianRight.Image = bitmap;
            Message("Right Gaussion Width=" + bitmap.Size.Width.ToString() + " Height=" + bitmap.Size.Height.ToString());
        }

        private Bitmap DoGaussian(OneEye eye)
        {
            LSD.doubleTupleList outList = LSD.doubleTupleList.newTupleList(7);
            image<double> modgrad;
            List<coord> list_p;
            // angle tolerance 
            double quant = 2.0;       // Bound to the quantization error on the gradient norm.    
            double ang_th = 22.5;     // Gradient angle tolerance in degrees.          
            int n_bins = 1024;        // Number of bins in pseudo-ordering of gradient modulus.
            double prec = Math.PI * ang_th / 180.0;
            double p = ang_th / 180.0;
            double rho = quant / Math.Sin(prec);
            //LSD.image<Double> scaled_image=LSD.gaussian_sampler(eye.imgDouble, 0.8, 0.6);
            image<Double> scaled_image = eye.imgDouble;
            image<Double> angles = LSD.ll_angle(scaled_image, rho, (uint)n_bins, out modgrad, out list_p);
            image<double> img = angles;
            int w = (int)img.width;
            int h = (int)img.height;
            //double normal = double.Epsilon;
            //for (int y = 0; y < h; y++)
            //    for (int x = 0; x < w; x++)
            //    {
            //        if (img.data[x, y] == LSD.NOTDEF) continue;
            //        if (normal < Math.Abs(img.data[x, y])) normal = Math.Abs(img.data[x, y]);
            //    }
            //normal /= 255;
            try
            {
                Bitmap gaussian = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                BitmapData dataOut = gaussian.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* pOut = (byte*)(dataOut.Scan0.ToPointer());
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            byte gr ;
                            if (img.data[x, y] == LSD.NOTDEF) gr = 0;
                            else gr = 200;
                            pOut[0] = pOut[1] = pOut[2] = (byte)gr;
                            pOut += 3;
                        }
                        pOut += dataOut.Stride - w * 3;
                    }
                }
                gaussian.UnlockBits(dataOut);
                gaussian.Save("gaussian.bmp");
                return gaussian;
            }
            catch (Exception ex) { Message("Make Gaussian:" + ex.Message); }
            return null;
        }

        private void btnLSD_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = DoLSD(mRightEye);
            pictureBoxGaussianRight.Image = bitmap;
            Message("Right LSD Width=" + bitmap.Size.Width.ToString() + " Height=" + bitmap.Size.Height.ToString());

        }

        private Bitmap DoLSD(OneEye eye)
        {
            image<int> img=new image<int>();   // 隨更new,只要不是null, 在LSD裏會重建
            LSD.doubleTupleList tupleList = LSD.lsd_scale_region(eye.imgDouble,0.8, ref img);
            int w = (int)img.width;
            int h = (int)img.height;
            try
            {
                Bitmap lineSegment = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                BitmapData dataOut = lineSegment.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* pOut = (byte*)(dataOut.Scan0.ToPointer());
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            byte gr = (img.data[x, y] == 0) ? (byte)0 : (byte)200;
                            pOut[0] = pOut[1] = pOut[2] = gr;
                            pOut += 3;
                        }
                        pOut += dataOut.Stride - w * 3;
                    }
                }
                lineSegment.UnlockBits(dataOut);
                lineSegment.Save("LSD.bmp");
                return lineSegment;
            }
            catch (Exception ex) { Message("Make LSD:" + ex.Message); }
            return null;
        }

    }
}
