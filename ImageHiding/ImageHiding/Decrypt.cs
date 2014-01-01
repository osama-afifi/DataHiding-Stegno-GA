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
      //  private FastBitmap stegoImage;
        private int numOfLSB;
        private int messageLength;
        //     private byte[] MessageBitString;
        public Decrypt(string HashInput, string stegoImageDirectory)
        {
            this.HashInput = HashInput;
            stegoImageBitmap = new Bitmap(stegoImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
        //    stegoImage = new FastBitmap(stegoImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking
            numOfLSB = 4;
        }

        public string getSecretMessage()
        {
            List<int> parameters = decrypt_hash(HashInput);
            messageLength = parameters.Last();
            //collectBytes(parameters);
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

        //private void collectBytes(List<int> para)
        //{
        //    int byteArrayLength = messageLength * (7 + numOfLSB) / numOfLSB;
        //    MessageBitString  = new byte[byteArrayLength + 10];
        //    int x0 = para[0], a = para[1], b = para[2], c = para[3];
        //    int n = stegoImageBitmap.Height;
        //    int m = stegoImageBitmap.Width;
        //    stegoImage.LockImage();
        //    int MOD = m * n;
        //    int index = x0;
        //    for (int i = 0; i < byteArrayLength; i++)
        //    {
        //        int x = index / n;
        //        int y = index % m;
        //        Color TargetPixelColor = stegoImage.GetPixel(x, y);
        //        MessageBitString[i] = getLSB((byte)TargetPixelColor.ToArgb(), numOfLSB);
        //        index = (((a % MOD * (int)powerMod(ref index, b, ref MOD)) % MOD) + c % MOD) % MOD;
        //    }
        //    //return MessageBitString;
        //}

        private string getMessage(List<int> para)
         {
             string decryptedMessage = "";
             int lsbSwitch = 0;
             int newByte = 0;

                 int x0 = para[0], a = para[1], b = para[2], c = para[3];
                 int n = stegoImageBitmap.Height;
                 int m = stegoImageBitmap.Width;
            //     stegoImage.LockImage();
                 int MOD = m * n;
                 int index = x0;

             for (int i = 0; i < 2 * messageLength; i++)
             {
                 if (lsbSwitch == 0)
                  newByte = 0;

                 int x = index / m;
                 int y = index % n;
                 Color TargetPixelColor = stegoImageBitmap.GetPixel(x, y);
                 int colorARGB = TargetPixelColor.ToArgb();
                 //newByte |=  (getLSB(colorARGB, numOfLSB)) <<(lsbSwitch*numOfLSB);
                 if (lsbSwitch == 1)
                 {
                  //   newByte <<= (numOfLSB);
                     byte MSB = (byte)(getLSB(colorARGB>> (numOfLSB), numOfLSB) );
                     newByte |= (MSB << numOfLSB);
                     decryptedMessage += (char)newByte;
                 }
                 else newByte = getLSB((byte)TargetPixelColor.ToArgb(), numOfLSB);

                 index = (((a % MOD * (int)powerMod(ref index, b, ref MOD) % MOD) % MOD) + c % MOD) % MOD;
                 lsbSwitch ^= 1;
             }
          //   stegoImage.UnlockImage();
             stegoImageBitmap.Dispose();
             return decryptedMessage;
         }

        //private string getMessage(byte[] byteArray)
        //{
        //    string ret = "";
        //    int byteParts = messageLength * (7 + numOfLSB) / numOfLSB;
        //    int byteSize = (7 + numOfLSB) / numOfLSB;
        //    byte temp = 0;
        //    //for (int i = 0; i < messageLength; i++)
        //    //{
        //    //    for (int j = 0; j < 8; j++)
        //    //        temp |= (byte)(byteArray[i*byteSize  ] & (1 << j));

        //    //}
        //    for (int i = 0; i < byteParts; i += byteSize)
        //    {
        //        temp = 0;
        //        for (int k = 0; k < byteSize; k++)
        //        {
        //            for (int j = 0; j < numOfLSB; j++)
        //                temp |= (byte)( 1<<( (byteArray[i + k] & (1 << j)) * (k * numOfLSB )) ); //+j
        //        }
        //        ret += (char)temp;
        //    }
        //        return ret;
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
                long Ret = powerMod(ref Number, Power - 1, ref MOD);
                Ret *= Ret;
                return Ret % MOD;
            }

        }
    }
}
