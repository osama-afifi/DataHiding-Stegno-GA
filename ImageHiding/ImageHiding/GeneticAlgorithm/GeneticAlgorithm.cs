using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageHiding.GA
{

    public class GeneticAlgorithm
    {

        public enum SelectionMode
        {
            RouletteWheel,
            Tournament,
            RewardBased
        };

        #region private members
        SelectionMode Selection;
        private static Random GARandomGenerator = new Random();
        private int populationSize;
        private int numberOfGenerations;
        private double crossoverRate;
        private double mutationRate;
        private int chromosomeLength;
        private int elitismFactor;
        private List<Organism> population;
        private BoundPair []genesDomain;
        private double fitnessSum;
        private double[] cummulativeFitness;
        #endregion

        public delegate double EvaluationDelegate(params int[] values);
        public EvaluationDelegate fitnessFunction;
        public BoundPair[] GenesDomain
        {
            set
            {
                genesDomain = value;
            }

            get
            {
                return genesDomain;
            }
        }
        public EvaluationDelegate FitnessFunction
        {
            get
            {
                return fitnessFunction;
            }
            set
            {
                fitnessFunction = value;
            }
        }
        public bool PrintLogMode;


        public GeneticAlgorithm(int populationSize, int chromosomeLength, int numberOfGenerations, double crossoverRate, double mutationRate, EvaluationDelegate fitnessFunction, BoundPair[] genesDomain ,  int elitismFactor = 0, SelectionMode Selection = SelectionMode.RouletteWheel, bool PrintLogMode = false)
        {
            this.populationSize = populationSize;
            this.chromosomeLength = chromosomeLength;
            this.numberOfGenerations = numberOfGenerations;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.fitnessFunction = fitnessFunction;
            this.genesDomain = genesDomain;
            this.elitismFactor = elitismFactor;
            this.PrintLogMode = PrintLogMode;
       
        }

        public Organism Run() // Warning you should assign an Fitness Function and genes Domain before running
        {
            createInitialPopulation();
            for (int curGeneration = 1; curGeneration <= numberOfGenerations; ++curGeneration)
            {
                rankPopulation();
                population.Sort();
                matePopulation();
                double f = population[0].fitnessValue;
                f.ToString();
            }
            Organism Best = (Organism)population[populationSize-1]; // return the best Organism as Optimal Solution         
            return Best;
        }

        #region Population Handlers

        private void createInitialPopulation()
        {
            population = new List<Organism>();
            population.Clear();
            for (int i = 0; i < populationSize; i++)
            {
                Organism genome = new Organism(chromosomeLength, ref genesDomain);
                population.Add((Organism)genome);
            }
        }
        private void rankPopulation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                Organism genome = (Organism)population[i];
                genome.fitnessValue = FitnessFunction(genome.chromosome);
            }
            //population.Sort();
        }
        private void matePopulation()
        {
            List<Organism> newPopulation = new List<Organism>();
            // Save the Elites
            List<Organism> Elites = new List<Organism>();
            for (int i = 0; i < elitismFactor; i++)
                Elites.Add((Organism)population[i]);

            SelectionPreprocess();
            int[] freq = new int[populationSize]; // for test purposes;
            for (int i = 0; i < populationSize; i++)
            {
                int parent1Index = ParentSelection();
                int parent2Index = ParentSelection();
                freq[parent1Index]++; //
                freq[parent2Index]++; //
                Organism parent1 = population[parent1Index];
                Organism parent2 = population[parent2Index];
                Organism child1 = new Organism(chromosomeLength);
                Organism child2 = new Organism(chromosomeLength);
                if (GARandomGenerator.NextDouble() < crossoverRate)
                    crossover(ref parent1, ref parent2, out child1, out child2);
                mutate(ref child1);
                mutate(ref child2);
                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }
            for (int i = 0; i < elitismFactor && i < populationSize; i++)
                newPopulation[i] = Elites[i];
            population.Clear();
            population = new List<Organism>(newPopulation);
        }
        #endregion

        #region Genetic Operators


        private void crossover(ref Organism parent1, ref Organism parent2, out Organism child1, out Organism child2)
        {
            int pivot = GARandomGenerator.Next(0, chromosomeLength - 1);
            for (int i = pivot; i < chromosomeLength; i++)
            {
                int temp = parent1.chromosome[i];
                parent1.chromosome[i] = parent2.chromosome[i];
                parent2.chromosome[i] = temp;
            }
            child1 = parent1;
            child2 = parent2;
        }

        private void mutate(ref Organism mutatedOrganism)
        {
            for (int i = 0; i < chromosomeLength; i++)
                if (GARandomGenerator.NextDouble() < mutationRate)
                    mutatedOrganism.chromosome[i] = (mutatedOrganism.chromosome[i] + GARandomGenerator.Next(genesDomain[i].LowerBound, genesDomain[i].UpperBound)) / 2;
        }
        #endregion

        #region Selection Preprocess

        private void SelectionPreprocess()
        {
            switch (Selection)
            {
                case SelectionMode.RouletteWheel:
                    RouletteWheelPreprocess();
                    break;
                case SelectionMode.Tournament:
                    TournamentPreprocess();
                    break;
                case SelectionMode.RewardBased:
                    RewardBasedPreprocess();
                    break;
            }
        }

        void RouletteWheelPreprocess()
        {

            fitnessSum = 0;
            //for (int i = 0; i < populationSize; i++)
            //    fitnessSum += ((Organism)population[i]).fitnessValue;
            cummulativeFitness = new double[populationSize];
            cummulativeFitness[0] = population[0].fitnessValue;
            for (int i = 1; i < populationSize; i++)
                cummulativeFitness[i] = cummulativeFitness[i - 1] + population[i].fitnessValue;
            for (int i = 1; i < populationSize; i++)
                cummulativeFitness[i] = cummulativeFitness[i - 1] + cummulativeFitness[i];
            fitnessSum = cummulativeFitness[populationSize-1];
            ////probability of each organism from the population according to its fitness value
            //for (int i = 0; i < populationSize; i++)
            //    genomeProbability[i] = ((Organism)population[i]).fitnessValue / fitnessSum;

            ////cumulative probability of each oragnism
            //for (int i = 0; i < populationSize; i++)
            //    if (i == 0)
            //        cummulativeProbability[i] = genomeProbability[i];
            //    else
            //        cummulativeProbability[i] = cummulativeProbability[i - 1] + genomeProbability[i];
        }

        void TournamentPreprocess()
        { }

        void RewardBasedPreprocess()
        { }

        #endregion

        #   region Selection Algorithms

        private int ParentSelection()
        {
            int parentIndex = 0;
            switch (Selection)
            {
                case SelectionMode.RouletteWheel:
                    parentIndex = RouletteWheel();
                    break;
                case SelectionMode.Tournament:
                    parentIndex = Tournament();
                    break;
                case SelectionMode.RewardBased:
                    parentIndex = RewardBased();
                    break;
            }
            return parentIndex;
        }

        private int RouletteWheel()
        {
            double accumulatedFreq = GARandomGenerator.NextDouble() * fitnessSum;
            //binary search to find the random selected oragnism
            int start = 0;
            int end = populationSize - 1;
            int ret = 0;
            int prev = -1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                if (Math.Abs(prev - mid) < 1e-3) break;
                if (accumulatedFreq <= cummulativeFitness[mid])
                {
                    ret =  mid;
                    end = mid - 1;
                }
                else if (accumulatedFreq < cummulativeFitness[mid])
                    start = mid + 1;
                prev = mid;
            }
            return ret;
        }
        private int Tournament()
        { return -1; }
        private int RewardBased()
        { return -1; }
        #   endregion

    }
}

# region Auxillary BoundPair Class
public class BoundPair
{
    int lowerBound , upperBound;

    public BoundPair()
    { }
    public BoundPair(int lowerBound, int upperBound)
    {
        this.lowerBound = lowerBound;
        this.upperBound = upperBound;
    }
    public int LowerBound
    {
        set
        {
            lowerBound = value;
        }
        get
        {
            return lowerBound;
        }
    }
    public int UpperBound
    {
        set
        {
            upperBound = value;
        }
        get
        {
            return upperBound;
        }
    }
}

#endregion