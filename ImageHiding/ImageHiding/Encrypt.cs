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
            coverImage.UnlockImage();
            coverImageBitmap.Dispose(); // Clear Image from Memory

        }

        static string hashFunction(int a, int b, int c, int d, int l)
        {
            List<int> Nums = new List<int>();
            Nums.Add(a); Nums.Add(b); Nums.Add(c); Nums.Add(d); Nums.Add(l);
            string hashed = "";
            for (int j = 0; j < 5; j++)
            {
                long Pow = (long)Math.Pow(2.0, 32.0);
                long i = Nums[j] * 2654435761;
                long div = i / Pow;
                long rem = i % Pow;

                hashed += div;
                hashed += '-';
                hashed += rem;
                hashed += '-';
            }
            return hashed;
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
