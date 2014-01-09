using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageHiding.GA
{
    
    public class Organism : IComparable<Organism> , IEquatable<Organism>
    {
       // public static int chromosomeLength;
        static Random RandomGenerator = new Random();
        public int[] chromosome;
        public double fitnessValue;

        public Organism(int chromosomeLength) 
        {
            fitnessValue = 0;
            chromosome = new int[chromosomeLength];
        }
        public Organism(int chromosomeLength, ref BoundPair[] genesDomain)
        {
            fitnessValue = 0;
            chromosome = new int[chromosomeLength];
            for (int i = 0; i < chromosomeLength; i++)
                chromosome[i] = RandomGenerator.Next(genesDomain[i].LowerBound , genesDomain[i].UpperBound); // set the bounds of the gene
        }
        


        # region IEquatable
        public bool Equals(Organism other)
        {
            if (other == null)
                return false;
            if (this.fitnessValue == other.fitnessValue)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Organism p = obj as Organism;
            if (p == null)
                return false;
            else
                return Equals(p);
        }

        public static bool operator ==(Organism p1, Organism p2)
        {
            if ((object)p1 == null || ((object)p2) == null)
                return object.Equals(p1, p2);

            return p1.Equals(p2);
        }

        public static bool operator !=(Organism p1, Organism p2)
        {
            if (p1 == null || p2 == null)
                return !object.Equals(p1, p2);

            return !(p1.Equals(p2));
        }

        # endregion

        # region IComparable
        public int CompareTo(Organism x)
        {
            if (this.fitnessValue > x.fitnessValue)
                return -1;
            else if (this.fitnessValue < x.fitnessValue)
                return 1;
            else
                return 0;
        }

        public static bool operator <(Organism p1, Organism p2)
        {
            return p1.CompareTo(p2) < 0;
        }

        public static bool operator >(Organism p1, Organism p2)
        {
            return p1.CompareTo(p2) > 0;
        }
        # endregion
    }
}
