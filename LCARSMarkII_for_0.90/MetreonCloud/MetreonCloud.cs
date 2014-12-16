using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class MetreonCloud : PartModule
    {


        bool ShieldEmmitter_IsRunning = true;



        /*
         * NCI can send settings with this
         * The OptionID is what you have set in the NCI_Object.cfg file for this object
         * the values of the fields "doors" and "GUIWindows" will be recognized by NCI Missions
         * It's simple, either this part plugin shows and uses it's own settings for doors and windows, 
         * OR NCI overwrites locks doors and replaces windows with own conversations.
        actionpoints = NCIComputerConsolePoint,NCIForceFieldHiddenAccessPoint,NCIHabitatSectionConsolePoint,NCIMechanicalDoorAccessPoint
        doors = ForceField,MechanicalDoor
        GUIWindows = ComputerConsole,ForceFieldHiddenAccess,HabitatSectionConsole,MechanicalDoorAccess
         */
        public void NCIRemote_Generic_EnableDisable(Dictionary<string,bool> sendOptions) // this functioncall MUST be identical in all NCI parts
        {
            foreach (KeyValuePair<string, bool> opts in sendOptions)
            {
                string OptionID = opts.Key;
                bool isDisabled = opts.Value;

                switch (OptionID)
                {
                    /*
                    DOORS
                    */

                    case "ShieldEmmitter": // it's a door, if true, the door is locked as defined in mission cfg
                        ShieldEmmitter_IsRunning = isDisabled;
                        break;

                    /*
                     GUIWINDOWS
                     */
                }
            }
        }
        
        
        



        bool runOnce = false;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        private float logInterval = 1.0f;
        Part MetreonCloudPart = null;
        void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null || this.vessel.loaded == false)
            { return; }

            /*if (MemoryAlphaPart == null)
            {
                foreach (Part p in this.vessel.parts)
                {
                    if (p.name == "MemoryAlpha")
                    {
                        MemoryAlphaPart = p;
                    }
                }
            }*/

            this.vessel.transform.rotation = Quaternion.LookRotation(Vector3d.left, Vector3d.up);

            MetreonCloudPart = this.vessel.rootPart;

            if (!runOnce)
            {
                MetreonCloudPart.CrewCapacity = 385;
                runOnce = true;
            }

            if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
            {
            }
                MetreonCloudPart.SetHighlight(false, true); // turn off the vessel highlight while in EVA
            
            
            if ((Time.time - lastFixedUpdate) > logInterval)
            {



                if (ShieldEmmitter_IsRunning)
                {
                    MetreonCloudPart.crashTolerance = 6000000000f;
                    MetreonCloudPart.maxTemp = 31000000000f;
                }
                else
                {
                    MetreonCloudPart.crashTolerance = 60f;
                    MetreonCloudPart.maxTemp = 3100f;
                }



                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: begin ");


                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: 7 ");
            }
        }
    }
}
