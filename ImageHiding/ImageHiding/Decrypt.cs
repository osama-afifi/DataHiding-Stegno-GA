using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageHiding
{
    class Decrypt
    {

         public Decrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            coverImageBitmap = new Bitmap(coverImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            coverImage = new FastBitmap(coverImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking

        }


    }
}
