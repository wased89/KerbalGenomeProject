using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KerbalGenomeProject
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class HeadMaster : MonoBehaviour
    {
        public static ProtoCrewMember.KerbalType Doctor = new ProtoCrewMember.KerbalType();
        public static ProtoCrewMember.KerbalType Baby = new ProtoCrewMember.KerbalType();
        public static Dictionary<KBKerbal, Kerbal> kerbalMap = new Dictionary<KBKerbal, Kerbal>();
        public static Dictionary<Kerbal, KBKerbal> kbKerbalMap = new Dictionary<Kerbal, KBKerbal>();

        private Rect MainGUI = new Rect(100,100,100,100);


        int mainGUID;

        bool showMainGUI = true;

        void Awake()
        {

            mainGUID = Guid.NewGuid().GetHashCode();

            DontDestroyOnLoad(this);
            
            //add in default genomes to any kerbals that dont have them
            for (int i = 0; i < HighLogic.CurrentGame.CrewRoster.Count; i++ )
            {
                ProtoCrewMember pcm = HighLogic.CurrentGame.CrewRoster[i];
                
                if(!kerbalMap.Values.Contains(pcm.KerbalRef))
                {
                    KBKerbal kbKerbal = new KBKerbal();
                    kbKerbal = kbKerbal.getDefaultKBKerbal(kbKerbal);
                    kbKerbal.genome = BreedingFunctions.createNewGenome(pcm.KerbalRef);
                    kbKerbal.isBabby = false;
                    kbKerbal.father = null;
                    kbKerbal.mother = null;
                    kbKerbal.isInfertile = false;
                    kbKerbal.age = 0;
                    
                }
            }
        }
        void FixedUpdate()
        {

        }
        void OnGUI()
        {
            if(showMainGUI)
            {
                MainGUI = GUI.Window(mainGUID, MainGUI,MainGUIWindow, "Kerbals~");
            }
        }
        void MainGUIWindow(int windowID)
        {
            foreach(Kerbal kerbal in kerbalMap.Values)
            {
                GUILayout.Button(kerbal.name);
            }
        }
    }
}
