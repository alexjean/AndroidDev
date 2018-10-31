using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PC_ADB_USB
{
    class Vision
    {
        static public Bitmap XorTwo(Image left,Image right)
        {
            if (left == null || right == null) return null;
            Bitmap imgLeft = (Bitmap)left;
            Bitmap imgRight = (Bitmap)right;
            int h1 = imgLeft.Size.Height;
            int w1 = imgLeft.Size.Width;
            int h2 = imgRight.Size.Height;
            int w2 = imgRight.Size.Width;
            int h = (h1 < h2) ? h1 : h2;
            int w = (w1 < w2) ? w1 : w2;
            try
            {
                Bitmap xorTwo = new Bitmap( w, h,PixelFormat.Format24bppRgb);
                BitmapData dataLeft  = imgLeft.LockBits (new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData dataRight = imgRight.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData dataOut   = xorTwo.LockBits  (new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* pLeft  = (byte*)(dataLeft.Scan0.ToPointer());
                    byte* pRight = (byte*)(dataRight.Scan0.ToPointer());
                    byte* pOut   = (byte*)(dataOut.Scan0.ToPointer());

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            pOut[0] = (byte)(pLeft[0] ^ pRight[0]);
                            pOut[1] = (byte)(pLeft[1] ^ pRight[1]);
                            pOut[2] = (byte)(pLeft[2] ^ pRight[2]);
                            pLeft  += 3;
                            pRight += 3;
                            pOut   += 3;
                        }
                        pLeft  += dataLeft.Stride  - w * 3;
                        pRight += dataRight.Stride - w * 3;
                        pOut   += dataOut.Stride   - w * 3;
                    }
                }
                xorTwo.UnlockBits(dataOut);
                imgLeft.UnlockBits(dataLeft);
                imgRight.UnlockBits(dataRight);
                return xorTwo;
            }
            catch{ return null; }
        }
    }
}
