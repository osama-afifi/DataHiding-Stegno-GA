using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int imageWidth;
        private int imageHeight;
        private FastBitmap stegoImage;
        private int NumberOfLSB;
        private int BitPartitions;
      
        public Encrypt(string secretMessage, string coverImageDirectory)
        {
            this.secretMessage = secretMessage;
            coverImageBitmap = new Bitmap(coverImageDirectory); // Creating Ordinary Bitmap to Load Image from Path 
            coverImage = new FastBitmap(coverImageBitmap);  // FastBitmap is a costum created unsafe bitmap which allow faster access by manual locking/unlocking
            NumberOfLSB = 4;
            imageHeight = coverImageBitmap.Height;
            imageWidth = coverImageBitmap.Width;
            BitPartitions = (secretMessage.Length + NumberOfLSB - 1) / NumberOfLSB;
        }

        public string Run()
        {
            string outputHash = "";
            int x0 = 0, a = 1, b = 1, c = 1; // x0 , xi+1 = a(xi+1)^b +c passed by reference
            //GenerateSequence(ref x0, ref a, ref b, ref c);
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
            // for now x0 = 0 , xi+1 = (1xi^1+1)%(m*n)

            // Defining genes Domains
            BoundPair []parameterDomain = new BoundPair[4];

            //Domain of x0
            parameterDomain[0].LowerBound = 0 ;
            parameterDomain[0].LowerBound = imageHeight*imageWidth;

            //Domain of a
            parameterDomain[1].LowerBound = 0;
            parameterDomain[1].LowerBound = (imageHeight*imageWidth)/secretMessage.Length;

            //Domain of b
            parameterDomain[2].LowerBound = 1 ;
            parameterDomain[2].LowerBound = (imageHeight*imageWidth)/secretMessage.Length;

            //Domain of c
            parameterDomain[3].LowerBound = 0 ;
            parameterDomain[3].LowerBound = imageHeight*imageWidth;


            GeneticAlgorithm GA = new GeneticAlgorithm(
                100, // population size
                4,   // chromosome Length
                3000, // Number of Generations
                0.80, // Crossover Ratio 
                0.05, // Mutation Ratio
                new GeneticAlgorithm.EvaluationDelegate(PSNR), // Fitness Function
                parameterDomain, // domain of each Parameter
                3,  // Elitism Factor
                GeneticAlgorithm.SelectionMode.RouletteWheel  //Selection Method
                );

            ImageHiding.GA.Organism OptimalSequence = GA.Run(); // Run the Genetic Algorithm

            //Get the Best Result
            x0 = OptimalSequence.chromosome[0];
            a = OptimalSequence.chromosome[1];
            b = OptimalSequence.chromosome[2];
            c = OptimalSequence.chromosome[3];
        }

        private void ReplacePixels(int x0, int a, int b, int c)
        {
            stegoImageBitmap = new Bitmap(coverImageBitmap);
            stegoImage = new FastBitmap(stegoImageBitmap);
            HashSet<int> visitedPixels = new HashSet<int>();
            stegoImage.LockImage();
            int MOD = imageWidth * imageHeight;
            int index = x0;
            int lsbSwitch = 0;
            for (int i = 0; i < 2 * secretMessage.Length; i++)
            {
                int x = index / imageWidth;
                int y = index % imageHeight;
                byte newLSB = (byte)((((1 << NumberOfLSB) - 1) << (lsbSwitch * NumberOfLSB)) & secretMessage[i / 2]);
                Color OldColor = stegoImage.GetPixel(y, x);
                byte newARGB = (byte)clearKBits(OldColor.ToArgb(), NumberOfLSB);
                newARGB |= newLSB;
                stegoImage.SetPixel(y, x, Color.FromArgb(newARGB));
                visitedPixels.Add(index);
                index = (((a % MOD * (int)powerMod(ref index, b, ref MOD) % MOD) % MOD) + c % MOD) % MOD;
                while (visitedPixels.Contains(index)) 
                {
                    index++;
                    index %= MOD;
                }
                visitedPixels.Add(index);
                lsbSwitch ^= 1;
            }
            stegoImage.UnlockImage();

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

        private int clearKBits(int original, int k)
        {
            int ret = original;
            ret = ((ret & (~((1 << (k + 1)) - 1)))); // clear k bits
            //   ret |= ((int)pat & ((1 << (NumberOfLSB + 1)) - 1)); // set the k bits
            return ret;
        }

        private double MSE(int x0, int a, int b, int c)
        {
            // optimize by taking used pixel only
            int MUL = imageWidth * imageHeight;
            HashSet<int> visitedPixels = new HashSet<int>();
            double mse = 0.0;
            // stegoImage = new FastBitmap(stegoImageBitmap);
            stegoImage.LockImage();
            coverImage.LockImage();
            int index = x0;
            for (int i = 0; i < 2 * secretMessage.Length; i++)
            {
                int x = index / imageWidth;
                int y = index % imageHeight;
                int stegoPixel = stegoImage.GetPixel(y, x).ToArgb();
                int coverPixel = coverImage.GetPixel(y, x).ToArgb();
                mse += (double)((stegoPixel - coverPixel) * (stegoPixel - coverPixel)) / (double)MUL;
                visitedPixels.Add(index);
                index = (((a % MUL * (int)powerMod(ref index, b, ref MUL) % MUL) % MUL) + c % MUL) % MUL;
                while (visitedPixels.Contains(index))
                {
                    index++;
                    index %= MUL;
                }
                visitedPixels.Add(index);
            }
            stegoImage.UnlockImage();
            coverImage.UnlockImage();
            stegoImageBitmap.Dispose();
            coverImageBitmap.Dispose();
            return mse;
        }
        private double PSNR(params int []values)
        {
            int x0 = values[0];
            int a  = values[1];
            int b  = values[2];
            int c  = values[3];
            return 20 * Math.Log10(1 << 24) - 10 * Math.Log10(MSE(x0, a, b, c));
        }

    }
}


// user shouldn't replace the stego with the cover image
// 