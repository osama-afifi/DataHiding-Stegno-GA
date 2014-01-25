using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ImageHiding
{
    public partial class GUIForm : Form
    {
        private string hashMessage;

        public GUIForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private string SelectImageFile(string initialDirectory = "C:\\Users\\Osama\\Desktop\\")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select a file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }

        private string SaveImageFileDialog(string initialDirectory = "C:\\Users\\Osama\\Desktop\\")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter =
               "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Save your Encrypted file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            int total = 100; //some number (this is your variable to change)!!

            for (int i = 0; i <= total; i++) //some number (total)
            {
                System.Threading.Thread.Sleep(100);
                int percents = (i * 100) / total;
                backGroundWorker.ReportProgress(percents, i);
                //2 arguments:
                //1. procenteges (from 0 t0 100) - i do a calcumation 
                //2. some current value!
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            //progressBar.Increment(e.ProgressPercentage);
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //do the code when bgv completes its work
            progressBar.Visible.Equals(false);
        }

        public static Image resizeImage(Image img, Size size)
        {
            return (Image)(new Bitmap(img, size));
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            if (InputMessageBox.Text == "")
            {
                DialogResult dlgRes = MessageBox.Show("The Message Content is Empty", "Empty Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (FileImageDirectoryBox.Text == "")
            {
                DialogResult dlgRes = MessageBox.Show("Please Insert a Valid Image", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            backGroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWork);
            backGroundWorker.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
            backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            backGroundWorker.WorkerReportsProgress = true;
            backGroundWorker.RunWorkerAsync();

            string SecretMessage = InputMessageBox.Text;
            string CoverImageDir = FileImageDirectoryBox.Text;
            BeforePictureBox.Image = resizeImage(Image.FromFile(CoverImageDir), new Size(BeforePictureBox.Height, BeforePictureBox.Width));
            Encrypt newEncrypt = new Encrypt(SecretMessage, CoverImageDir);
            string hashOutput = newEncrypt.Run();
            string stegoImag = SaveImageFileDialog();
            newEncrypt.SaveStegoImage(stegoImag);
            AfterPictureBox.Image = resizeImage(Image.FromFile(stegoImag), new Size(AfterPictureBox.Height, AfterPictureBox.Width));
            EncryptionHash.Text = hashOutput;
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            if (DecryptionHash.Text == "")
            {
                DialogResult dlgRes = MessageBox.Show("Please Insert a Valid Hash Key", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (SelectImagetoDecryptBox.Text == "")
            {
                DialogResult dlgRes = MessageBox.Show("Please Insert a Valid Image", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
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
            this.hashMessage = EncryptionHash.Text;
            //EncryptionHash.Copy();
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            DecryptionHash.Text = this.hashMessage;
            // DecryptionHash.Paste();
        }

        private void GUIForm_Load(object sender, EventArgs e)
        {

        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }









    }
}
