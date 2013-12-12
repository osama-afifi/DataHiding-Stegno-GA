using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHiding
{
    public partial class GUIForm : Form
    {
        public GUIForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private string SelectImageFile(string initialDirectory = "C:\\")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select a text file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            string SecretMessage = InputMessageBox.Text;
            string CoverImageDir = FileImageDirectoryBox.Text;


        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ChooseImageButton_Click(object sender, EventArgs e)
        {
            string imagepath = SelectImageFile();
            FileImageDirectoryBox.Text = imagepath;
        }

        private void SelectImageToDecryptButton_Click(object sender, EventArgs e)
        {
            string imagepath = SelectImageFile();
            SelectImagetoDecryptBox.Text = imagepath;
            
        }

        private void SelectImagetoDecryptBox_TextChanged(object sender, EventArgs e)
        {
      
        }

        private void DecryptionHash_TextChanged(object sender, EventArgs e)
        {

        }

 
     

 

    }
}
