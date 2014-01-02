using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageHiding.GeneticAlgorithm
{

    class GeneticAlgorithm
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
        private ArrayList population;
        private KeyValuePair<int, int>[] genesDomain;
        #endregion

        public delegate double EvaluationDelegate(params int[] values);
        public EvaluationDelegate fitnessFunction;
        public KeyValuePair<int, int>[] GenesDomain
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


        public GeneticAlgorithm(int populationSize, int chromosomeLength, int numberOfGenerations, double crossoverRate, double mutationRate, EvaluationDelegate fitnessFunction, int elitismFactor = 0, SelectionMode Selection = SelectionMode.RouletteWheel , bool PrintLogMode = false)
        {
            this.populationSize = populationSize;
            this.chromosomeLength = chromosomeLength;
            this.numberOfGenerations = numberOfGenerations;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.fitnessFunction = fitnessFunction;
            this.elitismFactor = elitismFactor;
            this.PrintLogMode = PrintLogMode;
            genesDomain = new KeyValuePair<int, int>[chromosomeLength];
        }

        public Organism Run() // Warning you should assign an Fitness Function and genes Domain before running
        {
            createInitialPopulation();
            for (int curGeneration = 1; curGeneration <= numberOfGenerations; ++curGeneration)
            {
                rankPopulation();
                population.Sort();
                matePopulation();
            }
            Organism Best = (Organism)population[0]; // return the best Organism as Optimal Solution
            return Best;
        }

        #region population handlers
        private void createInitialPopulation()
        {
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
            ArrayList newPopulation = new ArrayList();
            // Save the Elites
            ArrayList Elites = new ArrayList();
            for (int i = 0; i < elitismFactor; i++)
                Elites.Add((Organism)population[i]);
            for (int i = 0; i < populationSize; i++)
            {
                Organism parent1 = (Organism)population[ParentSelection()];
                Organism parent2 = (Organism)population[ParentSelection()];
                Organism child1 = new Organism();
                Organism child2 = new Organism();                   
                if (GARandomGenerator.NextDouble() < crossoverRate)
                    crossover(ref parent1, ref parent2, out child1, out child2);
                mutate(ref child1);
                mutate(ref child2);
                newPopulation.Add((Organism)child1);
                newPopulation.Add((Organism)child2);
            }
            for (int i = 0; i < elitismFactor && i<populationSize; i++)
                newPopulation[i] = (Organism)Elites[i];
            population.Clear();
            population = new ArrayList(newPopulation);
        }
        #endregion

        #region Genetic Operators

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
                    mutatedOrganism.chromosome[i] = (mutatedOrganism.chromosome[i] + GARandomGenerator.Next(genesDomain[i].Key, genesDomain[i].Value)) / 2;
        }
        #endregion

        #   region Selection Algorithms
        private int RouletteWheel()
        { return -1; }
        private int Tournament()
        { return -1; }
        private int RewardBased()
        { return -1; }
        #   endregion

    }
}
