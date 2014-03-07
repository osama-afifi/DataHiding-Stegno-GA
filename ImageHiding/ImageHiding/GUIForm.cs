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

            string SecretMessage = InputMessageBox.Text;
            string CoverImageDir = FileImageDirectoryBox.Text;
            BeforePictureBox.Image = resizeImage(Image.FromFile(CoverImageDir), new Size(BeforePictureBox.Height, BeforePictureBox.Width));
            Encrypt newEncrypt = new Encrypt(SecretMessage, CoverImageDir);
            worker.RunWorkerAsync(newEncrypt);


        
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

        private string SaveImageFileDialog(string initialDirectory = "C:\\Users\\Osama\\Desktop\\")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter =
               "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Save your Encrypted file";
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
        }


        // Background Work
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker encryptWorker;
            encryptWorker = (System.ComponentModel.BackgroundWorker)sender;

            // Get the Words object and call the main method.
            Encrypt encryptor = (Encrypt)e.Argument;
            encryptor.Run(encryptWorker, e);
            e.Result = encryptor;
            if (e.Cancel || encryptWorker.CancellationPending)
                return;
           
            
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
             progressBar1.Value = e.ProgressPercentage;
             PercLabel.Text = e.ProgressPercentage.ToString() + "%";
            state curState = (state)e.UserState;
            ProgressLabel.Text = curState.currentGeneration + " out of " + curState.numOfGenerations + " Generations.";
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Encryption canceled.");
            else
            {
           
                string stegoImageDir = SaveImageFileDialog();
                Encrypt encryptor = (Encrypt)e.Result;
                EncryptionHash.Text = encryptor.OutputHash;
                encryptor.SaveStegoImage(stegoImageDir);
                AfterPictureBox.Image = resizeImage(Image.FromFile(stegoImageDir), new Size(AfterPictureBox.Height, AfterPictureBox.Width));
                MessageBox.Show("Encryption Done Succesfully.");
            }
            clearProgress();
        }

        private void clearProgress()
        {
            PercLabel.Text = "%";
            ProgressLabel.Text = "";
            progressBar1.Value = 0;
        }
        private void CancelEncrypt_Click(object sender, EventArgs e)
        {
            clearProgress();
            worker.CancelAsync();
        }



        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    // This event handler is called when the background thread finishes. 
        //    // This method runs on the main thread. 
        //    if (e.Error != null)
        //        MessageBox.Show("Error: " + e.Error.Message);
        //    else if (e.Cancelled)
        //        MessageBox.Show("Word counting canceled.");
        //    else
        //        MessageBox.Show("Finished counting words.");
        //}

        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    // This method runs on the main thread.
        //    Words.CurrentState state =
        //        (Words.CurrentState)e.UserState;
        //    this.LinesCounted.Text = state.LinesCounted.ToString();
        //    this.WordsCounted.Text = state.WordsMatched.ToString();
        //}


        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    // This event handler is where the actual work is done. 
        //    // This method runs on the background thread. 

        //    // Get the BackgroundWorker object that raised this event.
        //    System.ComponentModel.BackgroundWorker worker;
        //    worker = (System.ComponentModel.BackgroundWorker)sender;

        //    // Get the Words object and call the main method.
        //    Words WC = (Words)e.Argument;
        //    WC.CountWords(worker, e);
        //}

        //private void StartThread()
        //{
         
        //    Words WC = new Words();
        //    WC.CompareString = this.CompareString.Text;
        //    WC.SourceFile = this.SourceFile.Text;

        //    // Start the asynchronous operation.
        //    backgroundWorker1.RunWorkerAsync(WC);
        //}

       



    }
}
