using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Random = UnityEngine.Random;

namespace KerbalGenomeProject
{
    class BreedingFunctions
    {
        public static List<Chromosome> modChromosomeList = new List<Chromosome>();
        
        //allows mods to add their own chromosomes
        //returns true if the add was successful, false if it's already in the list
        public static bool addModChromosome(Chromosome modChromosome)
        {
            if(!modChromosomeList.Contains(modChromosome) && modChromosome.genes != null && (modChromosome.name != "" || modChromosome.name != null) && modChromosome.value != null)
            {
                modChromosomeList.Add(modChromosome);
                return true;
            }
            else
            {
                Debug.Log("[KGP]: ERROR ON ADDING MOD GENOME TO KERBAL, MAKE SURE IT ISNT NULL");
                return false;
            }
        }

        //used for when a new kerbal spawns that isn't a baby
        public static Genome createNewGenome(Kerbal k)
        {
            Genome g = new Genome();
            g.chromosomes = createDefaultChromosomes(k);
            foreach(Chromosome modChromosome in modChromosomeList)
            {
                Debug.Log("[KGP]: Mod chromosomes: " + modChromosomeList.Count);
                if(modChromosome.genes != null && (modChromosome.name != "" || modChromosome.name != null) && modChromosome.value != null)
                {
                    Debug.Log("[KGP]: Adding chromosome");
                    g.chromosomes.Add(modChromosome);
                }
                else
                {
                    Debug.Log("[KGP]: ERROR ON ADDING MOD CHROMOSOME TO KERBAL, MAKE SURE IT ISNT NULL");
                }
                
            }
            return g;
        }
        //creates the default set of chromosomes to be put into the genome
        public static List<Chromosome> createDefaultChromosomes(Kerbal k)
        {
            List<Chromosome> chromosomes = new List<Chromosome>();
            
            //create the stupidity chromosome
            Chromosome stupidity = new Chromosome();
            stupidity.name = "stupidity";
            stupidity.genes = createGenes(5, stupidity.name, false, k.stupidity, 0.25f);
            stupidity.value = averageGeneValue(stupidity.genes);
            chromosomes.Add(stupidity);

            //create the courage chromosome
            Chromosome courage = new Chromosome();
            courage.name = "courage";
            courage.genes = createGenes(5, courage.name, false, k.courage, 0.25f);
            courage.value = averageGeneValue(courage.genes);
            chromosomes.Add(courage);

            Chromosome height = new Chromosome();
            height.name = "height";
            height.genes = createGenes(5, height.name, false, k.avatarSize, 0.25f);
            height.value = averageGeneValue(height.genes);
            chromosomes.Add(height);

            return chromosomes;
        }
        //Amount = amount of genes that make up the chromosome
        //name is the chromosome name, ie. Stupidity / courage. Usually describes it's effect. also will add a number for gene specifics
        //isRecessive should be left false if the gene/s aren't necessarily dominant/recessive
        //value is the main value to go off of, most likely the parent's value for their chromosome
        //tolerance is the gene tolerance, so that variation can be had through this.
        //there is a random.range for the end value, it goes (value - tolerance) * 100 so note that your tolerance is a percentage.
        public static List<Gene> createGenes(float count, String name, bool isRecessive, float value, float tolerance)
        {
            List<Gene> genes = new List<Gene>();
            for (int i = 0; i < count; i++)
            {
                Gene gene = new Gene();
                gene.name = name + i;
                gene.isRecessive = isRecessive;
                float value1 = Random.Range((value - tolerance) * 100, (value + tolerance) * 100)/100f;
                genes.Add(gene);
            }
            return genes;
        }
        //Averages the genes values
        public static float averageGeneValue(List<Gene> genes)
        {
            int count = genes.Count;
            float value = 0;
            foreach(Gene gene in genes)
            {
                value += gene.value; //add value to total
            }
            return value /= count; //average the values and return
        }
        //Create a Genome from two parents for the offspring
        public static Genome fuseGenomes(Genome father, Genome mother)
        {
            Genome g = new Genome();
            List<Chromosome> list = new List<Chromosome>();
            list = father.chromosomes;
            List<Chromosome> childGenome = new List<Chromosome>();
            for (int i = 0; i < list.Count; i++ )
            {
                
                int choose = Random.Range(0, 1);
                if (choose == 0)//father's chromosome
                {
                    childGenome.Add(father.chromosomes[i]);
                }
                else //mother
                {
                    childGenome.Add(mother.chromosomes[i]);
                }
            }
            g.chromosomes = childGenome;
            return g;
        }
    }
}
