namespace MyLinkToken
{
    partial class FormQRCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQRCode));
            this.lbAddress = new System.Windows.Forms.Label();
            this.qrCodeAddress = new Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeImgControl();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeAddress)).BeginInit();
            this.SuspendLayout();
            // 
            // lbAddress
            // 
            this.lbAddress.Font = new System.Drawing.Font("宋体", 11F);
            this.lbAddress.Location = new System.Drawing.Point(12, 14);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(360, 15);
            this.lbAddress.TabIndex = 4;
            this.lbAddress.Text = "0";
            this.lbAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // qrCodeAddress
            // 
            this.qrCodeAddress.ErrorCorrectLevel = Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M;
            this.qrCodeAddress.Image = ((System.Drawing.Image)(resources.GetObject("qrCodeAddress.Image")));
            this.qrCodeAddress.Location = new System.Drawing.Point(96, 46);
            this.qrCodeAddress.Name = "qrCodeAddress";
            this.qrCodeAddress.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Two;
            this.qrCodeAddress.Size = new System.Drawing.Size(200, 200);
            this.qrCodeAddress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.qrCodeAddress.TabIndex = 6;
            this.qrCodeAddress.TabStop = false;
            // 
            // FormQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.qrCodeAddress);
            this.Controls.Add(this.lbAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQRCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormQRCode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeAddress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbAddress;
        private Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeImgControl qrCodeAddress;
    }
}