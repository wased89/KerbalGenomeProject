using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KerbalGenomeProject;

namespace KerbalGenomeProject
{
    public struct Gene
    {
        public string name;
        public bool isRecessive;
        public float value; //the value or weight of the Gene, eg. height = value, eye colour = 0,1,2 for green blue brown
    }
}
