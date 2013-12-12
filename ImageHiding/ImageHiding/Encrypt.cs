using System;
using System.Collections;
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

        public void Run(int NumberOfLSB = 4)
        {
            BitArray MessageBitString = new BitArray(partitionMessage(secretMessage));
            int x0 = 0, a = 1, b = 1, c = 1;// x0 , xi+1 = a(xi+1)^b +c passed by reference
            generateSequence(ref x0, ref a, ref b, ref c);
            // HashRecurrence();
            // ReplacePixels();

            ///////////////////////////////////////////////////
            //  3shan tst3ml el pixel get w set shofo da 3shan 2na mnzl class lil bitmap m5soos 2sr3 mn el default f lasm t2ro da
            // http://www.vcskicks.com/fast-image-processing2.php
            // lw 5lsna el klam da n3ml class el Decrypt w yb2a 5lsna mn el project mn 8er optimization
            // ya ret n5ls da 2nhrda
            ////////////////////////////////////////



            coverImage.LockImage();
            // image editing here 
            coverImage.UnlockImage();
            coverImageBitmap.Dispose(); // Clear Image from Memory

        }

        static string hashRecurrence(int a, int b, int c, int d, int l)
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
        BitArray partitionMessage(string secretMessage)
        {
            int len = secretMessage.Length;
            BitArray bitString = new BitArray(len * 8 + 1);
            //byte[] ByteString  = new byte[len * 8 + 1] ;
            for (int i = 0; i < len; i++)
                for (int bit = 0; bit < 8; bit++)
                    bitString.Set(i * 8 + bit, (secretMessage[i] & (1 << (7 - bit))) == 1);

            return bitString;
        }

        void generateSequence(ref int x0, ref int a, ref int b, ref int c)
        {
            // here we will implement the optimal seq.
            // for now x0 = 0 , xi+1 = (1xi^1+1)%(m*n)
        }

    }
}
