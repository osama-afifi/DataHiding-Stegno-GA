using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ImageHiding;

namespace ImageHiding.GA
{

    public class GeneticAlgorithm
    {

        public enum SelectionMode
        {
            RouletteWheel,
            Tournament
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
        private BoundPair[] genesDomain;
        private double fitnessSum;
        private double[] cummulativeFitness;
        private int tourSize;
        List<int> tournamentItems;
        private bool[] taken;
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


        public GeneticAlgorithm(int populationSize, int chromosomeLength, int numberOfGenerations, double crossoverRate, double mutationRate, EvaluationDelegate fitnessFunction, BoundPair[] genesDomain, int elitismFactor = 0, SelectionMode Selection = SelectionMode.RouletteWheel, bool PrintLogMode = false)
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

        public Organism Run()
        {
            createInitialPopulation();
            for (int curGeneration = 1; curGeneration <= numberOfGenerations; ++curGeneration)
            {
                rankPopulation();
                population.Sort();
                matePopulation();
            }
            rankPopulation();
            population.Sort();
            Organism Best = (Organism)population[0]; // return the best Organism as Optimal Solution         
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
                Elites.Add(population[i]);          

            SelectionPreprocess();
            int[] freq = new int[populationSize];
            for (int i = 0; i < populationSize/2; i++)
            {
                int p1 = ParentSelection();
                int p2 = ParentSelection();
                freq[p1]++;
                freq[p2]++;
                Organism parent1 = (Organism)population[p1];
                Organism parent2 = (Organism)population[p2];
                Organism child1 = new Organism(chromosomeLength);
                Organism child2 = new Organism(chromosomeLength);
                if (GARandomGenerator.NextDouble() < crossoverRate)
                    crossover(parent1, parent2, out child1, out child2);
                else
                    copyChromosome(parent1, parent2, out child1, out child2);              
              
                mutate(ref child1);
                mutate(ref child2);
                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }
            for (int i = 0; i < elitismFactor && i < populationSize; i++)
                newPopulation[i] = (Organism)Elites[i];
            population.Clear();
            population = new List<Organism>(newPopulation);
        }
        #endregion

        #region Genetic Operators

        private void copyChromosome(Organism parent1, Organism parent2, out Organism child1, out Organism child2)
        {
            child1 = new Organism(chromosomeLength);
            child2 = new Organism(chromosomeLength);
            for (int i = 0; i < chromosomeLength; i++)
            {
                child1.chromosome[i] = parent1.chromosome[i];
                child2.chromosome[i] = parent2.chromosome[i];
            }
        }
        private void copyChromosome(Organism parent, out Organism child)
        {
            child = new Organism(chromosomeLength);
            for (int i = 0; i < chromosomeLength; i++)
                child.chromosome[i] = parent.chromosome[i];          
        }

        private void crossover(Organism parent1, Organism parent2, out Organism child1, out Organism child2)
        {
            child1 = new Organism(chromosomeLength);
            child2 = new Organism(chromosomeLength);
            int pivot = GARandomGenerator.Next(0, chromosomeLength - 1);
            for (int i = 0; i < chromosomeLength; i++)
            {
                if (i <= pivot)
                {
                    child1.chromosome[i] = parent1.chromosome[i];
                    child2.chromosome[i] = parent2.chromosome[i];
                }
                else
                {
                    child1.chromosome[i] = parent2.chromosome[i];
                    child2.chromosome[i] = parent1.chromosome[i];
                }
            }
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
                    TournamentPreprocess(0.1);
                    break;
               
            }
        }

        void RouletteWheelPreprocess()
        {
            cummulativeFitness = new double[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                if (i == 0)
                    cummulativeFitness[i] = population[i].fitnessValue;
                else
                    cummulativeFitness[i] = cummulativeFitness[i - 1] + population[i].fitnessValue;
            }
            //for (int i = 1; i < populationSize; i++)
            //    cummulativeFitness[i] = cummulativeFitness[i - 1] + cummulativeFitness[i];
            fitnessSum = cummulativeFitness[populationSize - 1];
            
        }

        void TournamentPreprocess(double tPercentage)
        {
            tourSize = (int) tPercentage * populationSize;
            tournamentItems.Clear();
        }

       

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
               
            }
            return parentIndex;
        }

        private int RouletteWheel()
        {
            double num =  GARandomGenerator.NextDouble() * fitnessSum;
            //binary search to find the random selected oragnism
            int start = 0;
            int end = populationSize - 1;
            int mid = (start + end) / 2;
            while (start <= end)
            {
                mid = (start + end) / 2;
                //the value is compared with the previous cumulative probability and the cumulative probability of the current item
                //to handle the case if there is no previous item to compare with I set the previous value to 0
                double intervalStart;
                double intervalEnd = cummulativeFitness[mid];
                if (mid == 0)
                    intervalStart = 0.0;
                else
                    intervalStart = cummulativeFitness[mid - 1];

                if (num >= intervalStart && num <= cummulativeFitness[mid])
                {
                    return mid;
                }
                else if (num > cummulativeFitness[mid])
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            return mid;
        }
        private int Tournament()
        {
            int takenCount = 0;
            int toTake;
            double maxFitness = 0;
            int chosenItem = 0;
            Random rnd = new Random();
            tournamentItems.Clear();
            //the default value of bool array is false
            taken = new bool[populationSize];
            //choose the tournament randomly
            while (takenCount < tourSize)
            {
                toTake = rnd.Next(populationSize);
                if (taken[toTake])
                    continue;
                else
                {
                    ++takenCount;
                    taken[toTake] = true;
                    tournamentItems.Add(toTake);
                }
            }
            foreach (int i in tournamentItems)
            {
                if (population[i].fitnessValue > maxFitness)
                {
                    chosenItem = i;
                    maxFitness = population[i].fitnessValue;
                }
            }
            return chosenItem;
        }
        
        #   endregion

    }
}

# region Auxillary BoundPair Class
public class BoundPair
{
    int lowerBound, upperBound;

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