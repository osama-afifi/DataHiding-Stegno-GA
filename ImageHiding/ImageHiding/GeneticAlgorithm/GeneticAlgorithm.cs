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
        public int populationSize;
        public int numberOfGenerations;
        public double crossoverRate;
        public double mutationRate;
        public int elitismFactor;
        public bool printMode;
        public delegate double EvaluationDelegate(params int[] values);        
        EvaluationDelegate fitnessFunction;
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
        static Random GARandomGenerator = new Random();

        public GeneticAlgorithm(int populationSize, int numberOfGenerations, double crossoverRate, double mutationRate, EvaluationDelegate fitnessFunction, int elitismFactor = 0)
        {
            this.populationSize = populationSize;
            this.numberOfGenerations = numberOfGenerations;
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.elitismFactor = elitismFactor;
            this.fitnessFunction = fitnessFunction;
        }

        public void runSimulation()
        { 
            //create initial organsims
            //rank organisms
            //generate new population
            // select two different organism using a Roulette Selection
            // do crossover || mutation || elitism
            // repeat till satisfied
            // return best
        }


    }
}
