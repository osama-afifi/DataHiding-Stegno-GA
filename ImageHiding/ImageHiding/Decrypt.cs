using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BitmapProcessing;

namespace ImageHiding
{
    class Decrypt
    {
        private string secretMessage;
        private Bitmap stegoImageBitmap;
        private FastBitmap stegoImage;

         public Decrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            stegoImageBitmap = new Bitmap(coverImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            stegoImage = new FastBitmap(stegoImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking

        }


    }
}
