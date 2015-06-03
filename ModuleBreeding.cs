using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

using Random = UnityEngine.Random;

namespace KerbalGenomeProject
{
    class ModuleBreeding : PartModule
    {

        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Breeding: ", guiFormat = "F2", isPersistant = true)]
        public string breedingString = "";

        int kerbalCount;
        bool enoughCrew = false;
        bool canBreed = false;
        bool hasMale = false;
        bool hasFemale = false;
        bool isBreeding = false;

        ProtoCrewMember female;
        ProtoCrewMember male;

        Vessel houseVessel;
        Part housePart;

        double timeOfConception;
        double timeOfBirth = -1;

        private delegate bool AddCrewMemberToRosterDelegate(ProtoCrewMember pcm);

        private AddCrewMemberToRosterDelegate AddCrewMemberToRoster;

        public override void OnAwake()
        {
            
            DontDestroyOnLoad(this);
            base.OnAwake();
        }

        public override void OnStart(PartModule.StartState state)
        {
            Debug.Log("OnStart!");
            Vessel v = base.vessel;
            houseVessel = v;
            housePart = base.part;
            Debug.Log("Getting crew count");
            kerbalCount = base.part.CrewCapacity;
            if (kerbalCount <= 1)
            {
                enoughCrew = false;
            }
            Debug.Log("Foreaching crew");
            

            foreach (ProtoCrewMember pcm in housePart.protoModuleCrew)
            {
                if (pcm.gender == ProtoCrewMember.Gender.Male)
                {
                    Debug.Log("Found male");
                    hasMale = true;
                    male = pcm;
                }
                else if (pcm.gender == ProtoCrewMember.Gender.Female)
                {
                    Debug.Log("Found female");
                    hasFemale = true;
                    female = pcm;
                }
                else
                {
                    Debug.Log("No suitable kerbal found???");
                }
            }
            if (hasFemale && hasMale && housePart.CrewCapacity >= 3)
            {
                Debug.Log("Can breed!");
                canBreed = true;
            }
            Debug.Log("Start has finished");
            base.OnStart(state);
        }

        [KSPEvent(active = true, guiActive = true, guiActiveEditor = false, guiName = "Start Breeding")]
        public void StartBreeding()
        {
            Debug.Log("Breed check");
            if(!isBreeding && canBreed)
            {
                StartBreedingProcess();
            }
            
        }
        public override void OnUpdate()
        {
            
            if (Planetarium.GetUniversalTime() >= timeOfBirth && timeOfBirth != -1)
            {
                Birth();

                Debug.Log("Ending the breeding process");
                isBreeding = false;
                breedingString = "False";
                GameEvents.onVesselChange.Fire(houseVessel);
                Debug.Log("Breeding process ended");
                timeOfBirth = -1;
            }
            base.OnUpdate();
        }
        public override void OnFixedUpdate()
        {
            Debug.Log("Getting crew count");
            kerbalCount = base.part.CrewCapacity;
            if (kerbalCount <= 1)
            {
                enoughCrew = false;
            }
            Debug.Log("Foreaching crew");

            foreach (ProtoCrewMember pcm in housePart.protoModuleCrew)
            {
                Debug.Log(pcm.name);
                if (pcm.gender == ProtoCrewMember.Gender.Male)
                {
                    Debug.Log("Found male");
                    hasMale = true;
                    male = pcm;
                }
                else if (pcm.gender == ProtoCrewMember.Gender.Female)
                {
                    Debug.Log("Found female");
                    hasFemale = true;
                    female = pcm;
                }
                else
                {
                    Debug.Log("No suitable kerbal found???");
                }
            }
            if (hasFemale && hasMale && housePart.CrewCapacity >= 3)
            {
                Debug.Log("Can breed!");
                canBreed = true;
            }
            
            base.OnFixedUpdate();
        }

        public void StartBreedingProcess()
        {
            Debug.Log("Starting the breeding process");
            isBreeding = true;
            breedingString = "True";
            Breed();
            
        }
        public void Breed()
        {

            Random rnd = new Random();
            double BakeTime = 86400;
            timeOfConception = Planetarium.GetUniversalTime();
            timeOfBirth = timeOfConception + BakeTime;
            breedingString = "Making the babby";
        }

        public static void test()
        {
            KBKerbal kb = new KBKerbal();
            ProtoCrewMember pcm;
            Kerbal k = new Kerbal();
            if(k.protoCrewMember.type == HeadMaster.Doctor)
            {
            }
        }

        public void Birth()
        {
            ProtoCrewMember babby = new ProtoCrewMember(HeadMaster.Baby);
            babby.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;


            MethodInfo addMemberToCrewRosterMethod = typeof(KerbalRoster).GetMethod("AddCrewMember", BindingFlags.NonPublic | BindingFlags.Instance);
            AddCrewMemberToRoster = (AddCrewMemberToRosterDelegate)Delegate.CreateDelegate(typeof(AddCrewMemberToRosterDelegate), HighLogic.CurrentGame.CrewRoster, addMemberToCrewRosterMethod);
            //babby.name = "Babby Kerman";
            babby.name = HighLogic.CurrentGame.CrewRoster.GetNewKerbal().name;
            Debug.Log("[KGP]: Baby name: " + babby.name);
            if (AddCrewMemberToRoster == null)
            {
                throw new Exception("Failed to load AddCrewMember delegate!");
            }
            if(!HighLogic.CurrentGame.CrewRoster.Exists(babby.name))
            {
                if(houseVessel.GetCrewCount() < houseVessel.GetCrewCapacity())
                {
                    housePart.AddCrewmember(babby);
                    housePart.RegisterCrew();
                    //KerbalRoster.SetExperienceTrait(babby);
                    babby.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
                    AddCrewMemberToRoster(babby);
                }
                
            }
            else
            {
                if (houseVessel.GetCrewCount() < houseVessel.GetCrewCapacity())
                {
                    babby.rosterStatus = ProtoCrewMember.RosterStatus.Assigned;
                    housePart.AddCrewmember(babby);
                    housePart.RegisterCrew();

                    KBKerbal kbKerbal = new KBKerbal();
                    
                    //set KBKerbal values
                    kbKerbal.isBabby = true;
                    kbKerbal.father = male.KerbalRef;
                    kbKerbal.mother = female.KerbalRef;
                    kbKerbal.genome = BreedingFunctions.fuseGenomes(HeadMaster.kbKerbalMap[kbKerbal.father].genome, HeadMaster.kbKerbalMap[kbKerbal.mother].genome);
                    kbKerbal.age = 0;
                    kbKerbal.isInfertile = true;

                    //add KBKerbal ref and kerbal ref to maps
                    HeadMaster.kerbalMap.Add(kbKerbal, babby.KerbalRef);
                    HeadMaster.kbKerbalMap.Add(babby.KerbalRef, kbKerbal);

                
                }
                
            }

            //AddCrewMemberToRoster(babby);
            houseVessel.SpawnCrew();

            
        }
    }
}
