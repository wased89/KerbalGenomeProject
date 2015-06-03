using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalGenomeProject
{
    public struct Chromosome
    {
        //this is where the Genes will be held to make the container that is the chromosome
        public string name;
        public List<Gene> genes;
        public float value;
    }
}
