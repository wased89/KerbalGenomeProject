using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KerbalGenomeProject
{
    public class Genome
    {
        //mitochondrial dna is passed down the female line
        //autosomal dna is blended
        //Y chromosome is passed down the male line
        //genes make up chromosomes, make up genome
        //potential genes:
        //disease susceptibility
        //resistence to medicine
        //stupidity
        //courage
        //height
        //eye colour
        //hair colour


        //have a chromosome pair represent one thing to modify, and each chromosome contains n amount of variance genes
        //which have influence on the total effect

        public List<Chromosome> chromosomes = new List<Chromosome>();
    }
}
