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

         static List<int> decrypt_hash(string ss)
         {
             List<int> param = new List<int>();
             List<long> dec = new List<long>();
             while (ss != "")
             {
                 int idx = ss.IndexOf('-');
                 string num = ss.Substring(0, idx);
                 ss = ss.Substring(idx + 1);
                 long numb = Convert.ToInt64(num);
                 dec.Add(numb);
             }
             long Pow = (long)Math.Pow(2.0, 32.0);
             for (int i = 0; i < 10; i += 2)
             {
                 long num = dec[i] * Pow;
                 num += dec[i + 1];
                 num = num / 2654435761;
                 param.Add((int)num);
             }
             return param;
         }
    }
}
