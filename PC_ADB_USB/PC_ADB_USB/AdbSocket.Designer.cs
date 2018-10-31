namespace PC_ADB_USB
{
    partial class AdbSocket
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAdbMapping = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnConnectLeft = new System.Windows.Forms.Button();
            this.pictureBoxLeft = new System.Windows.Forms.PictureBox();
            this.btnDisconnectLeft = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLeft = new System.Windows.Forms.TabPage();
            this.tabPageBlackWhite = new System.Windows.Forms.TabPage();
            this.pictureBoxGrayLeft = new System.Windows.Forms.PictureBox();
            this.btnTakePictureLeft = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBoxRight = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBoxGrayRight = new System.Windows.Forms.PictureBox();
            this.GaussianTabPage = new System.Windows.Forms.TabPage();
            this.pictureBoxGaussianRight = new System.Windows.Forms.PictureBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pictureBoxBoth = new System.Windows.Forms.PictureBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.btnTakePictureRight = new System.Windows.Forms.Button();
            this.btnDisconnectRight = new System.Windows.Forms.Button();
            this.btnConnectRight = new System.Windows.Forms.Button();
            this.btnAdbWhiteMapping = new System.Windows.Forms.Button();
            this.btnTake2Picture = new System.Windows.Forms.Button();
            this.btnXorTwo = new System.Windows.Forms.Button();
            this.btnGaussianRight = new System.Windows.Forms.Button();
            this.btnLSD = new System.Windows.Forms.Button();
            this.tabPageRegionGrow = new System.Windows.Forms.TabPage();
            this.pictureBoxRegionGrow = new System.Windows.Forms.PictureBox();
            this.btnRegion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeft)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageLeft.SuspendLayout();
            this.tabPageBlackWhite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrayLeft)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrayRight)).BeginInit();
            this.GaussianTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGaussianRight)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoth)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.tabPageRegionGrow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRegionGrow)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdbMapping
            // 
            this.btnAdbMapping.Location = new System.Drawing.Point(1050, 396);
            this.btnAdbMapping.Name = "btnAdbMapping";
            this.btnAdbMapping.Size = new System.Drawing.Size(96, 42);
            this.btnAdbMapping.TabIndex = 0;
            this.btnAdbMapping.Text = "ADB black mapping";
            this.btnAdbMapping.UseVisualStyleBackColor = true;
            this.btnAdbMapping.Click += new System.EventHandler(this.btnAdbBlackMapping_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(646, 501);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(534, 363);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // btnConnectLeft
            // 
            this.btnConnectLeft.Location = new System.Drawing.Point(2, 396);
            this.btnConnectLeft.Name = "btnConnectLeft";
            this.btnConnectLeft.Size = new System.Drawing.Size(96, 42);
            this.btnConnectLeft.TabIndex = 2;
            this.btnConnectLeft.Text = "Connect Left";
            this.btnConnectLeft.UseVisualStyleBackColor = true;
            this.btnConnectLeft.Click += new System.EventHandler(this.btnConnectLeft_Click);
            // 
            // pictureBoxLeft
            // 
            this.pictureBoxLeft.BackColor = System.Drawing.Color.Azure;
            this.pictureBoxLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLeft.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxLeft.Name = "pictureBoxLeft";
            this.pictureBoxLeft.Size = new System.Drawing.Size(528, 352);
            this.pictureBoxLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLeft.TabIndex = 3;
            this.pictureBoxLeft.TabStop = false;
            // 
            // btnDisconnectLeft
            // 
            this.btnDisconnectLeft.Location = new System.Drawing.Point(206, 396);
            this.btnDisconnectLeft.Name = "btnDisconnectLeft";
            this.btnDisconnectLeft.Size = new System.Drawing.Size(96, 42);
            this.btnDisconnectLeft.TabIndex = 4;
            this.btnDisconnectLeft.Text = "Disconn";
            this.btnDisconnectLeft.UseVisualStyleBackColor = true;
            this.btnDisconnectLeft.Click += new System.EventHandler(this.btnDisconnectLeft_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(646, 453);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(96, 42);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageLeft);
            this.tabControl1.Controls.Add(this.tabPageBlackWhite);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 388);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPageLeft
            // 
            this.tabPageLeft.Controls.Add(this.pictureBoxLeft);
            this.tabPageLeft.Location = new System.Drawing.Point(4, 26);
            this.tabPageLeft.Name = "tabPageLeft";
            this.tabPageLeft.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLeft.Size = new System.Drawing.Size(534, 358);
            this.tabPageLeft.TabIndex = 2;
            this.tabPageLeft.Text = "左眼";
            this.tabPageLeft.UseVisualStyleBackColor = true;
            // 
            // tabPageBlackWhite
            // 
            this.tabPageBlackWhite.Controls.Add(this.pictureBoxGrayLeft);
            this.tabPageBlackWhite.Location = new System.Drawing.Point(4, 26);
            this.tabPageBlackWhite.Name = "tabPageBlackWhite";
            this.tabPageBlackWhite.Size = new System.Drawing.Size(534, 358);
            this.tabPageBlackWhite.TabIndex = 1;
            this.tabPageBlackWhite.Text = "黑白";
            this.tabPageBlackWhite.UseVisualStyleBackColor = true;
            // 
            // pictureBoxGrayLeft
            // 
            this.pictureBoxGrayLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxGrayLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxGrayLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxGrayLeft.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxGrayLeft.Name = "pictureBoxGrayLeft";
            this.pictureBoxGrayLeft.Size = new System.Drawing.Size(534, 358);
            this.pictureBoxGrayLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGrayLeft.TabIndex = 0;
            this.pictureBoxGrayLeft.TabStop = false;
            // 
            // btnTakePictureLeft
            // 
            this.btnTakePictureLeft.Location = new System.Drawing.Point(104, 396);
            this.btnTakePictureLeft.Name = "btnTakePictureLeft";
            this.btnTakePictureLeft.Size = new System.Drawing.Size(96, 42);
            this.btnTakePictureLeft.TabIndex = 7;
            this.btnTakePictureLeft.Text = "TakePicture";
            this.btnTakePictureLeft.UseVisualStyleBackColor = true;
            this.btnTakePictureLeft.Click += new System.EventHandler(this.btnTakePictureLeft_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.GaussianTabPage);
            this.tabControl2.Controls.Add(this.tabPageRegionGrow);
            this.tabControl2.Location = new System.Drawing.Point(642, 2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(542, 388);
            this.tabControl2.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBoxRight);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(534, 358);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "右眼";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureBoxRight
            // 
            this.pictureBoxRight.BackColor = System.Drawing.Color.Azure;
            this.pictureBoxRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRight.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxRight.Name = "pictureBoxRight";
            this.pictureBoxRight.Size = new System.Drawing.Size(528, 352);
            this.pictureBoxRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRight.TabIndex = 3;
            this.pictureBoxRight.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBoxGrayRight);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(534, 358);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "黑白";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBoxGrayRight
            // 
            this.pictureBoxGrayRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxGrayRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxGrayRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxGrayRight.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxGrayRight.Name = "pictureBoxGrayRight";
            this.pictureBoxGrayRight.Size = new System.Drawing.Size(534, 358);
            this.pictureBoxGrayRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGrayRight.TabIndex = 0;
            this.pictureBoxGrayRight.TabStop = false;
            // 
            // GaussianTabPage
            // 
            this.GaussianTabPage.Controls.Add(this.pictureBoxGaussianRight);
            this.GaussianTabPage.Location = new System.Drawing.Point(4, 26);
            this.GaussianTabPage.Name = "GaussianTabPage";
            this.GaussianTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GaussianTabPage.Size = new System.Drawing.Size(534, 358);
            this.GaussianTabPage.TabIndex = 3;
            this.GaussianTabPage.Text = "Gaussian";
            this.GaussianTabPage.UseVisualStyleBackColor = true;
            // 
            // pictureBoxGaussianRight
            // 
            this.pictureBoxGaussianRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxGaussianRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxGaussianRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxGaussianRight.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxGaussianRight.Name = "pictureBoxGaussianRight";
            this.pictureBoxGaussianRight.Size = new System.Drawing.Size(528, 352);
            this.pictureBoxGaussianRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGaussianRight.TabIndex = 1;
            this.pictureBoxGaussianRight.TabStop = false;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Location = new System.Drawing.Point(2, 476);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(542, 388);
            this.tabControl3.TabIndex = 9;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pictureBoxBoth);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(534, 358);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "BothEye";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pictureBoxBoth
            // 
            this.pictureBoxBoth.BackColor = System.Drawing.Color.Azure;
            this.pictureBoxBoth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxBoth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxBoth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxBoth.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxBoth.Name = "pictureBoxBoth";
            this.pictureBoxBoth.Size = new System.Drawing.Size(528, 352);
            this.pictureBoxBoth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxBoth.TabIndex = 3;
            this.pictureBoxBoth.TabStop = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.pictureBox5);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(534, 358);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "黑白";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox5.Location = new System.Drawing.Point(0, 0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(534, 358);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // btnTakePictureRight
            // 
            this.btnTakePictureRight.Location = new System.Drawing.Point(744, 396);
            this.btnTakePictureRight.Name = "btnTakePictureRight";
            this.btnTakePictureRight.Size = new System.Drawing.Size(96, 42);
            this.btnTakePictureRight.TabIndex = 14;
            this.btnTakePictureRight.Text = "TakePicture";
            this.btnTakePictureRight.UseVisualStyleBackColor = true;
            this.btnTakePictureRight.Click += new System.EventHandler(this.btnTakePictureRight_Click);
            // 
            // btnDisconnectRight
            // 
            this.btnDisconnectRight.Location = new System.Drawing.Point(948, 396);
            this.btnDisconnectRight.Name = "btnDisconnectRight";
            this.btnDisconnectRight.Size = new System.Drawing.Size(96, 42);
            this.btnDisconnectRight.TabIndex = 12;
            this.btnDisconnectRight.Text = "Disconn";
            this.btnDisconnectRight.UseVisualStyleBackColor = true;
            this.btnDisconnectRight.Click += new System.EventHandler(this.btnDisconnectRight_Click);
            // 
            // btnConnectRight
            // 
            this.btnConnectRight.Location = new System.Drawing.Point(642, 396);
            this.btnConnectRight.Name = "btnConnectRight";
            this.btnConnectRight.Size = new System.Drawing.Size(96, 42);
            this.btnConnectRight.TabIndex = 11;
            this.btnConnectRight.Text = "Connect Right";
            this.btnConnectRight.UseVisualStyleBackColor = true;
            this.btnConnectRight.Click += new System.EventHandler(this.btnConnectRight_Click);
            // 
            // btnAdbWhiteMapping
            // 
            this.btnAdbWhiteMapping.Location = new System.Drawing.Point(410, 396);
            this.btnAdbWhiteMapping.Name = "btnAdbWhiteMapping";
            this.btnAdbWhiteMapping.Size = new System.Drawing.Size(96, 42);
            this.btnAdbWhiteMapping.TabIndex = 10;
            this.btnAdbWhiteMapping.Text = "ADB white mapping";
            this.btnAdbWhiteMapping.UseVisualStyleBackColor = true;
            this.btnAdbWhiteMapping.Click += new System.EventHandler(this.btnAdbWhiteMapping_Click);
            // 
            // btnTake2Picture
            // 
            this.btnTake2Picture.Location = new System.Drawing.Point(550, 161);
            this.btnTake2Picture.Name = "btnTake2Picture";
            this.btnTake2Picture.Size = new System.Drawing.Size(77, 42);
            this.btnTake2Picture.TabIndex = 15;
            this.btnTake2Picture.Text = "Take2 Picture";
            this.btnTake2Picture.UseVisualStyleBackColor = true;
            this.btnTake2Picture.Click += new System.EventHandler(this.btnTake2Picture_Click);
            // 
            // btnXorTwo
            // 
            this.btnXorTwo.Location = new System.Drawing.Point(550, 572);
            this.btnXorTwo.Name = "btnXorTwo";
            this.btnXorTwo.Size = new System.Drawing.Size(81, 42);
            this.btnXorTwo.TabIndex = 16;
            this.btnXorTwo.Text = "Xor";
            this.btnXorTwo.UseVisualStyleBackColor = true;
            this.btnXorTwo.Click += new System.EventHandler(this.btnXorTwo_Click);
            // 
            // btnGaussianRight
            // 
            this.btnGaussianRight.Location = new System.Drawing.Point(846, 396);
            this.btnGaussianRight.Name = "btnGaussianRight";
            this.btnGaussianRight.Size = new System.Drawing.Size(96, 42);
            this.btnGaussianRight.TabIndex = 17;
            this.btnGaussianRight.Text = "微分";
            this.btnGaussianRight.UseVisualStyleBackColor = true;
            this.btnGaussianRight.Click += new System.EventHandler(this.btnGaussianRight_Click);
            // 
            // btnLSD
            // 
            this.btnLSD.Location = new System.Drawing.Point(846, 453);
            this.btnLSD.Name = "btnLSD";
            this.btnLSD.Size = new System.Drawing.Size(96, 42);
            this.btnLSD.TabIndex = 18;
            this.btnLSD.Text = "LSD";
            this.btnLSD.UseVisualStyleBackColor = true;
            this.btnLSD.Click += new System.EventHandler(this.btnLSD_Click);
            // 
            // tabPageRegionGrow
            // 
            this.tabPageRegionGrow.Controls.Add(this.pictureBoxRegionGrow);
            this.tabPageRegionGrow.Location = new System.Drawing.Point(4, 26);
            this.tabPageRegionGrow.Name = "tabPageRegionGrow";
            this.tabPageRegionGrow.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegionGrow.Size = new System.Drawing.Size(534, 358);
            this.tabPageRegionGrow.TabIndex = 4;
            this.tabPageRegionGrow.Text = "RegionGrow";
            this.tabPageRegionGrow.UseVisualStyleBackColor = true;
            // 
            // pictureBoxRegionGrow
            // 
            this.pictureBoxRegionGrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxRegionGrow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxRegionGrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRegionGrow.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxRegionGrow.Name = "pictureBoxRegionGrow";
            this.pictureBoxRegionGrow.Size = new System.Drawing.Size(528, 352);
            this.pictureBoxRegionGrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRegionGrow.TabIndex = 2;
            this.pictureBoxRegionGrow.TabStop = false;
            // 
            // btnRegion
            // 
            this.btnRegion.Location = new System.Drawing.Point(948, 453);
            this.btnRegion.Name = "btnRegion";
            this.btnRegion.Size = new System.Drawing.Size(96, 42);
            this.btnRegion.TabIndex = 19;
            this.btnRegion.Text = "Region";
            this.btnRegion.UseVisualStyleBackColor = true;
            // 
            // AdbSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 865);
            this.Controls.Add(this.btnRegion);
            this.Controls.Add(this.btnLSD);
            this.Controls.Add(this.btnGaussianRight);
            this.Controls.Add(this.btnXorTwo);
            this.Controls.Add(this.btnTake2Picture);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnTakePictureRight);
            this.Controls.Add(this.btnDisconnectRight);
            this.Controls.Add(this.btnConnectRight);
            this.Controls.Add(this.btnAdbWhiteMapping);
            this.Controls.Add(this.tabControl3);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.btnTakePictureLeft);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDisconnectLeft);
            this.Controls.Add(this.btnConnectLeft);
            this.Controls.Add(this.btnAdbMapping);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AdbSocket";
            this.Text = "Connect Android by ADB";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeft)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageLeft.ResumeLayout(false);
            this.tabPageBlackWhite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrayLeft)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGrayRight)).EndInit();
            this.GaussianTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGaussianRight)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoth)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.tabPageRegionGrow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRegionGrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdbMapping;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnConnectLeft;
        private System.Windows.Forms.PictureBox pictureBoxLeft;
        private System.Windows.Forms.Button btnDisconnectLeft;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageBlackWhite;
        private System.Windows.Forms.PictureBox pictureBoxGrayLeft;
        private System.Windows.Forms.Button btnTakePictureLeft;
        private System.Windows.Forms.TabPage tabPageLeft;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBoxRight;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBoxGrayRight;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pictureBoxBoth;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button btnTakePictureRight;
        private System.Windows.Forms.Button btnDisconnectRight;
        private System.Windows.Forms.Button btnConnectRight;
        private System.Windows.Forms.Button btnAdbWhiteMapping;
        private System.Windows.Forms.Button btnTake2Picture;
        private System.Windows.Forms.Button btnXorTwo;
        private System.Windows.Forms.TabPage GaussianTabPage;
        private System.Windows.Forms.PictureBox pictureBoxGaussianRight;
        private System.Windows.Forms.Button btnGaussianRight;
        private System.Windows.Forms.Button btnLSD;
        private System.Windows.Forms.TabPage tabPageRegionGrow;
        private System.Windows.Forms.PictureBox pictureBoxRegionGrow;
        private System.Windows.Forms.Button btnRegion;
    }
}

