namespace ImageHiding
{
    partial class Form1
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
            this.BeforePictureBox = new System.Windows.Forms.PictureBox();
            this.AfterPictureBox = new System.Windows.Forms.PictureBox();
            this.EncryptButton = new System.Windows.Forms.Button();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.InputMessageBox = new System.Windows.Forms.TextBox();
            this.FileImageDirectoryBox = new System.Windows.Forms.TextBox();
            this.ChooseImageButton = new System.Windows.Forms.Button();
            this.OutputMessageBox = new System.Windows.Forms.RichTextBox();
            this.SelectImagetoDecryptBox = new System.Windows.Forms.TextBox();
            this.SelectImageToDecryptButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EncryptionHash = new System.Windows.Forms.TextBox();
            this.DecryptionHash = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CopyButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.PasteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BeforePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AfterPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BeforePictureBox
            // 
            this.BeforePictureBox.Location = new System.Drawing.Point(381, 12);
            this.BeforePictureBox.Name = "BeforePictureBox";
            this.BeforePictureBox.Size = new System.Drawing.Size(170, 170);
            this.BeforePictureBox.TabIndex = 0;
            this.BeforePictureBox.TabStop = false;
            this.BeforePictureBox.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // AfterPictureBox
            // 
            this.AfterPictureBox.Location = new System.Drawing.Point(574, 12);
            this.AfterPictureBox.Name = "AfterPictureBox";
            this.AfterPictureBox.Size = new System.Drawing.Size(170, 170);
            this.AfterPictureBox.TabIndex = 1;
            this.AfterPictureBox.TabStop = false;
            this.AfterPictureBox.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // EncryptButton
            // 
            this.EncryptButton.Location = new System.Drawing.Point(274, 154);
            this.EncryptButton.Name = "EncryptButton";
            this.EncryptButton.Size = new System.Drawing.Size(75, 23);
            this.EncryptButton.TabIndex = 2;
            this.EncryptButton.Text = "Encrypt";
            this.EncryptButton.UseVisualStyleBackColor = true;
            this.EncryptButton.Click += new System.EventHandler(this.EncryptButton_Click);
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(274, 424);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(75, 23);
            this.DecryptButton.TabIndex = 3;
            this.DecryptButton.Text = "Decrypt";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // InputMessageBox
            // 
            this.InputMessageBox.Location = new System.Drawing.Point(12, 24);
            this.InputMessageBox.Multiline = true;
            this.InputMessageBox.Name = "InputMessageBox";
            this.InputMessageBox.Size = new System.Drawing.Size(337, 124);
            this.InputMessageBox.TabIndex = 4;
            // 
            // FileImageDirectoryBox
            // 
            this.FileImageDirectoryBox.Location = new System.Drawing.Point(12, 156);
            this.FileImageDirectoryBox.Name = "FileImageDirectoryBox";
            this.FileImageDirectoryBox.Size = new System.Drawing.Size(212, 20);
            this.FileImageDirectoryBox.TabIndex = 5;
            // 
            // ChooseImageButton
            // 
            this.ChooseImageButton.Location = new System.Drawing.Point(230, 154);
            this.ChooseImageButton.Name = "ChooseImageButton";
            this.ChooseImageButton.Size = new System.Drawing.Size(25, 23);
            this.ChooseImageButton.TabIndex = 6;
            this.ChooseImageButton.Text = "...";
            this.ChooseImageButton.UseVisualStyleBackColor = true;
            this.ChooseImageButton.Click += new System.EventHandler(this.ChooseImageButton_Click);
            // 
            // OutputMessageBox
            // 
            this.OutputMessageBox.Location = new System.Drawing.Point(12, 251);
            this.OutputMessageBox.Name = "OutputMessageBox";
            this.OutputMessageBox.Size = new System.Drawing.Size(337, 141);
            this.OutputMessageBox.TabIndex = 7;
            this.OutputMessageBox.Text = "";
            // 
            // SelectImagetoDecryptBox
            // 
            this.SelectImagetoDecryptBox.Location = new System.Drawing.Point(12, 424);
            this.SelectImagetoDecryptBox.Name = "SelectImagetoDecryptBox";
            this.SelectImagetoDecryptBox.Size = new System.Drawing.Size(212, 20);
            this.SelectImagetoDecryptBox.TabIndex = 8;
            this.SelectImagetoDecryptBox.TextChanged += new System.EventHandler(this.SelectImagetoDecryptBox_TextChanged);
            // 
            // SelectImageToDecryptButton
            // 
            this.SelectImageToDecryptButton.Location = new System.Drawing.Point(230, 424);
            this.SelectImageToDecryptButton.Name = "SelectImageToDecryptButton";
            this.SelectImageToDecryptButton.Size = new System.Drawing.Size(25, 23);
            this.SelectImageToDecryptButton.TabIndex = 9;
            this.SelectImageToDecryptButton.Text = "...";
            this.SelectImageToDecryptButton.UseVisualStyleBackColor = true;
            this.SelectImageToDecryptButton.Click += new System.EventHandler(this.SelectImageToDecryptButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Encrypt Secret Message into an Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Decrypt Secret Message from an Image";
            // 
            // EncryptionHash
            // 
            this.EncryptionHash.Location = new System.Drawing.Point(86, 185);
            this.EncryptionHash.Name = "EncryptionHash";
            this.EncryptionHash.Size = new System.Drawing.Size(181, 20);
            this.EncryptionHash.TabIndex = 12;
            // 
            // DecryptionHash
            // 
            this.DecryptionHash.Location = new System.Drawing.Point(104, 398);
            this.DecryptionHash.Name = "DecryptionHash";
            this.DecryptionHash.Size = new System.Drawing.Size(163, 20);
            this.DecryptionHash.TabIndex = 13;
            this.DecryptionHash.TextChanged += new System.EventHandler(this.DecryptionHash_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Output Hash";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(273, 183);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(40, 23);
            this.CopyButton.TabIndex = 15;
            this.CopyButton.Text = "Copy!";
            this.CopyButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 401);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Decryption Hash";
            // 
            // PasteButton
            // 
            this.PasteButton.Location = new System.Drawing.Point(274, 395);
            this.PasteButton.Name = "PasteButton";
            this.PasteButton.Size = new System.Drawing.Size(48, 23);
            this.PasteButton.TabIndex = 18;
            this.PasteButton.Text = "Paste!";
            this.PasteButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 456);
            this.Controls.Add(this.PasteButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DecryptionHash);
            this.Controls.Add(this.EncryptionHash);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectImageToDecryptButton);
            this.Controls.Add(this.SelectImagetoDecryptBox);
            this.Controls.Add(this.OutputMessageBox);
            this.Controls.Add(this.ChooseImageButton);
            this.Controls.Add(this.FileImageDirectoryBox);
            this.Controls.Add(this.InputMessageBox);
            this.Controls.Add(this.DecryptButton);
            this.Controls.Add(this.EncryptButton);
            this.Controls.Add(this.AfterPictureBox);
            this.Controls.Add(this.BeforePictureBox);
            this.Name = "Form1";
            this.Text = "Image Data Hiding using Genetic Algorithms";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BeforePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AfterPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox BeforePictureBox;
        private System.Windows.Forms.PictureBox AfterPictureBox;
        private System.Windows.Forms.Button EncryptButton;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.TextBox InputMessageBox;
        private System.Windows.Forms.TextBox FileImageDirectoryBox;
        private System.Windows.Forms.Button ChooseImageButton;
        private System.Windows.Forms.RichTextBox OutputMessageBox;
        private System.Windows.Forms.TextBox SelectImagetoDecryptBox;
        private System.Windows.Forms.Button SelectImageToDecryptButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EncryptionHash;
        private System.Windows.Forms.TextBox DecryptionHash;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button PasteButton;

    }
}

