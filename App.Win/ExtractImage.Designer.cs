namespace App.Win
{
    partial class ExtractImage
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
            this.btnConvertImages = new System.Windows.Forms.Button();
            this.ofdConvertToImage = new System.Windows.Forms.OpenFileDialog();
            this.lbResult = new System.Windows.Forms.Label();
            this.btnAddLogo = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddWhiteBackground = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConvertImages
            // 
            this.btnConvertImages.Location = new System.Drawing.Point(55, 293);
            this.btnConvertImages.Name = "btnConvertImages";
            this.btnConvertImages.Size = new System.Drawing.Size(136, 33);
            this.btnConvertImages.TabIndex = 0;
            this.btnConvertImages.Text = "Convert to Images";
            this.btnConvertImages.UseVisualStyleBackColor = true;
            this.btnConvertImages.Click += new System.EventHandler(this.btnConvertImages_Click);
            // 
            // ofdConvertToImage
            // 
            this.ofdConvertToImage.FileName = "ofdConvertToImage";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(69, 64);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(0, 13);
            this.lbResult.TabIndex = 1;
            // 
            // btnAddLogo
            // 
            this.btnAddLogo.Location = new System.Drawing.Point(211, 293);
            this.btnAddLogo.Name = "btnAddLogo";
            this.btnAddLogo.Size = new System.Drawing.Size(124, 33);
            this.btnAddLogo.TabIndex = 2;
            this.btnAddLogo.Text = "Add logo";
            this.btnAddLogo.UseVisualStyleBackColor = true;
            this.btnAddLogo.Click += new System.EventHandler(this.btnAddLogo_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(112, 13);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(209, 20);
            this.txtOutputFolder.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Output folder :";
            // 
            // btnAddWhiteBackground
            // 
            this.btnAddWhiteBackground.Location = new System.Drawing.Point(361, 293);
            this.btnAddWhiteBackground.Name = "btnAddWhiteBackground";
            this.btnAddWhiteBackground.Size = new System.Drawing.Size(136, 33);
            this.btnAddWhiteBackground.TabIndex = 5;
            this.btnAddWhiteBackground.Text = "Add white background";
            this.btnAddWhiteBackground.UseVisualStyleBackColor = true;
            this.btnAddWhiteBackground.Click += new System.EventHandler(this.btnAddWhiteBackground_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(111, 45);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(210, 20);
            this.txtInput.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Input folder :";
            // 
            // ExtractImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 338);
            this.Controls.Add(this.btnAddWhiteBackground);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.btnAddLogo);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.btnConvertImages);
            this.Name = "ExtractImage";
            this.Text = "Extract images and add watermark for PDF file";
            this.Load += new System.EventHandler(this.ExtractImage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConvertImages;
        private System.Windows.Forms.OpenFileDialog ofdConvertToImage;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Button btnAddLogo;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddWhiteBackground;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label2;
    }
}

