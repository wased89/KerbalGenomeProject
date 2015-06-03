using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalGenomeProject
{
    public struct KBKerbal
    {
        public bool isInfertile;
        public bool isBabby;
        public double age;
        public Kerbal mother;
        public Kerbal father;
        public Genome genome;
        public KBKerbal getDefaultKBKerbal(KBKerbal kbKerbal)
        {
            isInfertile = false;
            isBabby = false;
            age = 0;
            mother = null;
            father = null;
            genome = null;
            return kbKerbal;
        }
    }
    
}
