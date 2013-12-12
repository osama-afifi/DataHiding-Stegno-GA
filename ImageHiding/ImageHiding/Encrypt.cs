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
        Encrypt(string secretMessage, string coverImageDirectory)
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

            coverImage.UnlockImage();
            coverImageBitmap.Dispose(); // Clear Image from Memory
        
        }

    }
}
