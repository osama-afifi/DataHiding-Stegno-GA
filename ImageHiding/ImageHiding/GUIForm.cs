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
               "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select a file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }
        private string SaveImageFileDialog(string initialDirectory = "C:\\")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter =
               "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Save a text file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            if (InputMessageBox.Text == "")
            {
                DialogResult dlgRes = MessageBox.Show("The Message Content is Empty", "Empty Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string SecretMessage = InputMessageBox.Text;
            string CoverImageDir = FileImageDirectoryBox.Text;
          //  BeforePictureBox.Image = Image.FromFile(CoverImageDir);
            Encrypt newEncrypt = new Encrypt(SecretMessage, CoverImageDir);
            string hashOutput = newEncrypt.Run();
            newEncrypt.SaveStegoImage(SaveImageFileDialog());
            EncryptionHash.Text = hashOutput;
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            string HashInput = DecryptionHash.Text;
            Decrypt newDecrypt = new Decrypt(HashInput, SelectImagetoDecryptBox.Text);
            string resultMessage = newDecrypt.getSecretMessage();
            OutputMessageBox.Text = resultMessage;
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

        private void EncryptionHash_TextChanged(object sender, EventArgs e)
        {

        }

        private void InputMessageBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            EncryptionHash.Copy();
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            DecryptionHash.Paste();
        }

        
 
     

 

    }
}
