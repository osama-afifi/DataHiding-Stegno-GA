using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BitmapProcessing;

namespace ImageHiding
{
    public class Encrypt
    {
        private string secretMessage;
        private Bitmap coverImageBitmap;
        private FastBitmap coverImage;
        public Encrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            coverImageBitmap = new Bitmap(coverImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            coverImage = new FastBitmap(coverImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking

        }

        public void Run()
        {
            coverImage.LockImage(); // Lock Cover Image for Accessing

            // partitionMessage();
            // generateRandomSequence();
            // HashRecurrence();
            //ReplacePixels();

            ///////////////////////////////////////////////////
            //  3shan tst3ml el pixel get w set shofo da 3shan 2na mnzl class lil bitmap m5soos 2sr3 mn el default f lasm t2ro da
            // http://www.vcskicks.com/fast-image-processing2.php
            // lw 5lsna el klam da n3ml class el Decrypt w yb2a 5lsna mn el project mn 8er optimization
            // ya ret n5ls da 2nhrda
            ////////////////////////////////////////
            int hoda;
            coverImage.UnlockImage();
            coverImageBitmap.Dispose(); // Clear Image from Memory
        
        }

    }
}
