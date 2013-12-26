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
	//menna
        private string secretMessage;
        private Bitmap coverImageBitmap;
        private FastBitmap coverImage;
        private Bitmap stegoImageBitmap;
       private FastBitmap stegoImage;
        //    private byte[] MessageBitString;
        private int NumberOfLSB;
        private int BitPartitions;

        public Encrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            coverImageBitmap = new Bitmap(coverImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            coverImage = new FastBitmap(coverImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking
            NumberOfLSB = 4;
            BitPartitions = (secretMessage.Length + NumberOfLSB - 1) / NumberOfLSB;
        }

        public string Run()
        {
            // MessageBitString = PartitionMessage(secretMessage);
            string outputHash = "";
            int x0 = 0, a = 1, b = 1, c = 1; // x0 , xi+1 = a(xi+1)^b +c passed by reference
            GenerateSequence(ref x0, ref a, ref b, ref c);
            ReplacePixels(x0, a, b, c);
            outputHash = HashRecurrence(x0, a, b, c, secretMessage.Length);
            return outputHash;
        }

        public void SaveStegoImage(string stegoDir)
        {
            if (stegoDir == null) return;
            
            stegoImageBitmap.Save(@stegoDir);
            stegoImageBitmap.Dispose();
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

        private void GenerateSequence(ref int x0, ref int a, ref int b, ref int c)
        {
            // here we will implement the optimal seq.
            // for now x0 = 0 , xi+1 = (1xi^1+1)%(m*n)
        }


        private void ReplacePixels(int x0, int a, int b, int c)
        {
            stegoImageBitmap = new Bitmap(coverImageBitmap);
           stegoImage = new FastBitmap(stegoImageBitmap);
            int n = stegoImageBitmap.Height;
            int m = stegoImageBitmap.Width;
           stegoImage.LockImage();
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    stegoImage.SetPixel(i, j, Color.Black);
          stegoImage.UnlockImage();
           stegoImageBitmap.Dispose();

        }

        //private void ReplacePixels(int x0, int a, int b, int c)
        //{
        //    stegoImageBitmap = new Bitmap(coverImageBitmap);
        //    stegoImage = new FastBitmap(stegoImageBitmap);
        //    int n = stegoImageBitmap.Height;
        //    int m = stegoImageBitmap.Width;
        //    stegoImage.LockImage();
        //    int MOD = m * n;
        //    int index = x0;
        //    int lsbSwitch = 0;
        //    for (int i = 0; i < 2*secretMessage.Length; i++)
        //    {
        //               int x = index / n;
        //               int y = index % m;
        //               byte newLSB = (byte)(((1 << NumberOfLSB) - 1) << (lsbSwitch * NumberOfLSB) & secretMessage[i/2]);
        //               newLSB = (lsbSwitch==1) ? (byte)(lsbSwitch >>= NumberOfLSB) : newLSB;
        //               Color OldColor = stegoImage.GetPixel(x, y);
        //               byte newARGB = (byte)clearKBits(OldColor.ToArgb(), NumberOfLSB);
        //               newARGB |= newLSB;
        //               stegoImage.SetPixel(x, y, Color.FromArgb(newARGB));
        //               index = (((a % MOD * (int)powerMod(ref index, b, ref MOD)%MOD) % MOD) + c % MOD) % MOD;
        //               lsbSwitch ^= 1;
        //    }
        //    stegoImage.UnlockImage();

        //}


        private long powerMod(ref int Number, int Power, ref int MOD)
        {
            if (Power == 0) return 1;
            if (Power % 2 == 1)
                return (powerMod(ref Number, Power - 1, ref MOD) % MOD * (long)Number % MOD) % MOD;
            else
            {
                long Ret = powerMod(ref Number, Power - 1, ref MOD);
                Ret *= Ret;
                return Ret % MOD;
            }

        }

        private int clearKBits(int original, int k)
        {
            int ret = original;
            ret = ((ret & (~((1 << (k + 1)) - 1)))); // clear k bits
            //   ret |= ((int)pat & ((1 << (NumberOfLSB + 1)) - 1)); // set the k bits
            return ret;
        }

    }
}
