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

        public string Run(int NumberOfLSB = 4)
        {
            BitArray MessageBitString = new BitArray(PartitionMessage(secretMessage));
            string outputHash = "";
            int x0 = 0, a = 1, b = 1, c = 1;// x0 , xi+1 = a(xi+1)^b +c passed by reference
            GenerateSequence(ref x0, ref a, ref b, ref c);
            ReplacePixels(ref coverImage);
            outputHash = HashRecurrence(x0, a, b, c, secretMessage.Length);
            return outputHash;
        }

        private string HashRecurrence(int a, int b, int c, int d, int l)
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
        private BitArray PartitionMessage(string secretMessage)
        {
            int len = secretMessage.Length;
            BitArray bitString = new BitArray(len * 8 + 1);
            for (int i = 0; i < len; i++)
                for (int bit = 0; bit < 8; bit++)
                    bitString.Set(i * 8 + bit, (secretMessage[i] & (1 << (7 - bit))) == 1);

            return bitString;
        }

        private void GenerateSequence(ref int x0, ref int a, ref int b, ref int c)
        {
            // here we will implement the optimal seq.
            // for now x0 = 0 , xi+1 = (1xi^1+1)%(m*n)
        }

        private Bitmap ReplacePixels(ref FastBitmap originalImage)
        {
            Bitmap stegoImage = new Bitmap(
            coverImage.LockImage();
            
            coverImage.UnlockImage();
            coverImageBitmap.Dispose(); // Clear Image from Memory
            
        }

    }
}
