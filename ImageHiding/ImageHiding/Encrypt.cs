using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Drawing;
using BitmapProcessing;
using ImageHiding.GA;

namespace ImageHiding
{
    public class Encrypt
    {
        private string secretMessage;
        private Bitmap coverImageBitmap;
        private FastBitmap coverImage;
        private Bitmap stegoImageBitmap;
        BackgroundWorker encryptWorker;
        DoWorkEventArgs encryptEvent;
        //   private FastBitmap stegoImage;
        private  int imageWidth;
        private  int imageHeight;
        private int NumberOfLSB;
        public string OutputHash;
        
        public Encrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            coverImageBitmap = new Bitmap(coverImageDirectory);
            stegoImageBitmap = new Bitmap(coverImageBitmap);
            coverImage = new FastBitmap(coverImageBitmap);
            NumberOfLSB = 4;
            imageHeight = coverImageBitmap.Height;
            imageWidth = coverImageBitmap.Width;
        }

        public void Run(BackgroundWorker sender, DoWorkEventArgs e)
        {
            this.encryptWorker = sender;
            this.encryptEvent = e;
            int x0 = 0, a = 1, b = 1, c = 1; // x0 , xi+1 = a(xi)^b +c passed by reference
            coverImage.LockImage();
            GenerateSequence(ref x0, ref a, ref b, ref c); 
            ReplacePixels(x0, a, b, c);
            coverImage.UnlockImage();
            OutputHash = HashRecurrence(x0, a, b, c, secretMessage.Length);
        }

        public void SaveStegoImage(string stegoDir)
        {
            if (stegoDir == null) return;
            stegoImageBitmap.Save(@stegoDir);
            stegoImageBitmap.Dispose();
            coverImageBitmap.Dispose();
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

        private void GenerateSequence(ref int x0, ref int a, ref int b, ref int c) // returns also the Optmal PSNR value
        {
            // Defining genes Domains
            BoundPair[] parameterDomain = new BoundPair[4];

            //Domain of x0
            parameterDomain[0] = new BoundPair(0, imageWidth * imageHeight);
            //Domain of a
            parameterDomain[1] = new BoundPair(1, imageWidth * imageHeight);
            //Domain of b
            parameterDomain[2] = new BoundPair(1, imageWidth * imageHeight);
            //Domain of c
            parameterDomain[3] = new BoundPair(1, imageWidth * imageHeight);

            GeneticAlgorithm GA = new GeneticAlgorithm(
                30, // Population Size
                4,   // Chromosome Length
                100, // Number of Generations
                0.85, // Crossover Ratio 
                0.25, // Mutation Ratio
                new GeneticAlgorithm.EvaluationDelegate(PSNR), // Fitness Function
                parameterDomain, // Domain of each Parameter
                1,  // Elitism Factor
                GeneticAlgorithm.SelectionMode.RouletteWheel  //Selection Method
                );

            ImageHiding.GA.Organism OptimalSequence = GA.Run(encryptWorker,encryptEvent); // Run the Genetic Algorithm

            //Get the Best Result
            x0 = OptimalSequence.chromosome[0];
            a = OptimalSequence.chromosome[1];
            b = OptimalSequence.chromosome[2];
            c = OptimalSequence.chromosome[3];
            double trivial = PSNR(0, 1, 1, 1);
            double opt = PSNR(OptimalSequence.chromosome);
            int x = 0;
          
        }

        private void ReplacePixels(int x0, int a, int b, int c)
        {
            //stegoImageBitmap = new Bitmap(coverImageBitmap);
            //stegoImage = new FastBitmap(stegoImageBitmap);
            HashSet<int> visitedPixels = new HashSet<int>();
            //stegoImage.LockImage();
            int MOD = imageWidth * imageHeight;
            int index = x0;
            int lsbSwitch = 0;
            for (int i = 0; i < 2 * secretMessage.Length; i++)
            {
                int y = index / imageWidth;
                int x = index % imageWidth;
                byte newLSB = (byte)((((1 << NumberOfLSB) - 1) << (lsbSwitch * NumberOfLSB)) & secretMessage[i / 2]);
                newLSB >>= (lsbSwitch * NumberOfLSB); //
                Color OldColor = stegoImageBitmap.GetPixel(x, y);
                int newARGB = clearKBits(OldColor.ToArgb(), NumberOfLSB);
                newARGB |= newLSB;
                stegoImageBitmap.SetPixel(x, y, Color.FromArgb(newARGB));
                visitedPixels.Add(index);
                index = (((a % MOD * (int)powerMod(ref index, b, ref MOD) % MOD) % MOD) + c % MOD) % MOD;
                index += MOD;
                index %= MOD;
                while (visitedPixels.Contains(index))
                {
                    index++;
                    index %= MOD;
                }

                lsbSwitch ^= 1;
            }
            //stegoImage.UnlockImage();

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

        private int clearKBits(int original, int k)
        {
            int ret = original;
            ret = ((ret & (~((1 << (k + 1)) - 1)))); // clear k bits
            return ret;
        }
        private int replaceKLSBBits(int original, int k , int replaced)
        {
            int ret = clearKBits(original, k);
            replaced &= ((1 << k) - 1); // mask original
            ret |= replaced;
            return ret;
        }

        private double MSE(int x0, int a, int b, int c)
        {
            // optimize by taking used pixel only
            int MUL = imageWidth * imageHeight;
            HashSet<int> visitedPixels = new HashSet<int>();
            double mse = 0.0;
            // coverImage = new FastBitmap(coverImageBitmap);  
            // coverImage.LockImage();
            
            int index = x0;
            int lsbSwitch = 0;
            for (int i = 0; i < 2 * secretMessage.Length; i++)
            {
                int y = index / imageWidth;
                int x = index % imageWidth;
               // int coverPixel = coverImage.GetPixel(x, y).ToArgb();
                Color coverColor = coverImage.GetPixel(x, y);
                int stegoPixel = replaceKLSBBits(coverColor.ToArgb(), NumberOfLSB, secretMessage[i / 2] >> (NumberOfLSB * lsbSwitch));
                Color stegoColor = Color.FromArgb(stegoPixel);

                mse += (double)((coverColor.R - stegoColor.R) * (coverColor.R - stegoColor.R));
                mse += (double)((coverColor.G - stegoColor.G) * (coverColor.G - stegoColor.G));
                mse += (double)((coverColor.B - stegoColor.B) * (coverColor.B - stegoColor.B));
                visitedPixels.Add(index);
                index = (((a % MUL * (int)powerMod(ref index, b, ref MUL) % MUL) % MUL) + c % MUL) % MUL;
                index += MUL;
                index %= MUL;
                while (visitedPixels.Contains(index))
                {
                    index++;
                    index %= MUL;
                }
                lsbSwitch ^= 1;
            }
            //coverImage.UnlockImage();
            //coverImageBitmap.Dispose();
            mse /= (double)MUL;
            return mse/3.0;
        }
        private double PSNR(params int[] values)
        {
            int x0 = values[0];
            int a = values[1];
            int b = values[2];
            int c = values[3];
            //return 1.0/MSE(x0, a, b, c);
            double psnr = 20 * Math.Log10((1 << 8) - 1) - 10 * Math.Log10(MSE(x0, a, b, c));
            return psnr;
        }

    }
}
