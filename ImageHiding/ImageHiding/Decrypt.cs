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
        private string HashInput;
        private Bitmap stegoImageBitmap;
       // private FastBitmap stegoImage;
        private int numOfLSB;
        private int messageLength;
        private int imageWidth;
        private int imageHeight;
        //private byte[] MessageBitString;
        public Decrypt(string HashInput, string stegoImageDirectory)
        {
            this.HashInput = HashInput;
            stegoImageBitmap = new Bitmap(stegoImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            //stegoImage = new FastBitmap(stegoImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking
            numOfLSB = 4;
            imageHeight = stegoImageBitmap.Height;
            imageWidth = stegoImageBitmap.Width;
        }

        public string getSecretMessage()
        {
            List<int> parameters = decrypt_hash(HashInput);
            messageLength = parameters.Last();
            string secretMessage = getMessage(parameters);
            return secretMessage;
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

        private string getMessage(List<int> para)
        {
            string decryptedMessage = "";
            int lsbSwitch = 0;
            int newByte = 0;
            HashSet<int> visitedPixels = new HashSet<int>();
            int x0 = para[0], a = para[1], b = para[2], c = para[3];
          //  stegoImage.LockImage();
            int MOD = imageWidth * imageHeight;
            int index = x0;

            for (int i = 0; i < 2 * messageLength; i++)
            {
                if (lsbSwitch == 0)
                    newByte = 0;

                int x = index / imageWidth;
                int y = index % imageHeight;
                Color TargetPixelColor = stegoImageBitmap.GetPixel(y, x);
                int colorARGB = TargetPixelColor.ToArgb();
                //newByte |=  (getLSB(colorARGB, numOfLSB)) <<(lsbSwitch*numOfLSB);
                if (lsbSwitch == 1)
                {
                    //   newByte <<= (numOfLSB);
                    // byte MSB = (byte)(getLSB(colorARGB >> (numOfLSB), numOfLSB));
                    newByte |= ((getLSB((byte)colorARGB, numOfLSB)) << numOfLSB);
                    decryptedMessage += (char)newByte;
                }
                else newByte = getLSB((byte)colorARGB, numOfLSB);

                visitedPixels.Add(index);
                index = (((a % MOD * (int)powerMod(ref index, b, ref MOD) % MOD) % MOD) + c % MOD) % MOD;
                while (visitedPixels.Contains(index))
                {
                    index++;
                    index %= MOD;
                }

                lsbSwitch ^= 1;
            }
            //stegoImage.UnlockImage();
           // stegoImageBitmap.Dispose();
            return decryptedMessage;
        }

        //private string getMessage(List<int> para)
        //{
        //    string decryptedMessage = "";
        //    int lsbSwitch = 0;
        //    int newByte = 0;
        //    HashSet<int> visitedPixels = new HashSet<int>();
        //    int x0 = para[0], a = para[1], b = para[2], c = para[3];

        //    stegoImage.LockImage();
        //    int MOD = imageWidth * imageHeight;
        //    int index = x0;

        //    for (int i = 0; i < 2 * messageLength; i++)
        //    {
        //        if (lsbSwitch == 0)
        //            newByte = 0;

        //        int x = index / imageWidth;
        //        int y = index % imageHeight;
        //        Color TargetPixelColor = stegoImage.GetPixel(y, x);
        //        int colorARGB = TargetPixelColor.ToArgb();
        //        //newByte |=  (getLSB(colorARGB, numOfLSB)) <<(lsbSwitch*numOfLSB);
        //        if (lsbSwitch == 1)
        //        {
        //            //   newByte <<= (numOfLSB);
        //            byte MSB = (byte)(getLSB(colorARGB >> (numOfLSB), numOfLSB));
        //            newByte |= (MSB << numOfLSB);
        //            decryptedMessage += (char)newByte;
        //        }
        //        else newByte = getLSB((byte)TargetPixelColor.ToArgb(), numOfLSB);
        //        visitedPixels.Add(index);
        //        index = (((a % MOD * (int)powerMod(ref index, b, ref MOD) % MOD) % MOD) + c % MOD) % MOD;
        //        while (visitedPixels.Contains(index))
        //        {
        //            index++;
        //            index %= MOD;
        //        }
        //        lsbSwitch ^= 1;
        //    }
        //    stegoImage.UnlockImage();
        //    stegoImageBitmap.Dispose();
        //    return decryptedMessage;
        //}

        private byte getLSB(int org, int k)
        {
            byte b = 0;
            for (int i = 0; i < k; i++)
                b |= (byte)(org & (1 << i));
            return b;
        }
        private long powerMod(ref int Number, int Power, ref int MOD)
        {
            if (Power == 0) return 1;
            if (Power % 2 == 1)
                return (powerMod(ref Number, Power - 1, ref MOD) % MOD * (long)Number % MOD) % MOD;
            else
            {
                long Ret = powerMod(ref Number, Power/2, ref MOD);
                Ret *= Ret;
                return Ret % MOD;
            }
        }
    }
}
