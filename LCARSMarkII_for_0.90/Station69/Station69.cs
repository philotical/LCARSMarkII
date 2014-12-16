using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class Station69 : PartModule
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
        public void NCIRemote_Generic_EnableDisable(Dictionary<string, bool> sendOptions) // this functioncall MUST be identical in all NCI parts
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
        Part Station69Part = null;
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
            Station69Part = this.vessel.rootPart;
            Station69Part.SetHighlight(false, true); // turn off the vessel highlight

            if (!runOnce)
            {
                UnityEngine.Debug.Log("Station69 FixedUpdate: runOnce begin ");



                /*
                string path = KSPUtil.ApplicationRootPath + "GameData/";
                string loadfileName = "INTERNALTes.cfg";
                string INTERNALTestCFG_path = path + "/" + loadfileName;
                ConfigNode rootN = new ConfigNode("root");
                ConfigNode internalN = new ConfigNode("INTERNAL");
                internalN.AddValue("name  ", "MetreonCloudInternal");

                ConfigNode seat = new ConfigNode("MODULE");
                seat.AddValue("name ", "InternalSeat");
                seat.AddValue("seatTransformName ", "seat");
                seat.AddValue("allowCrewHelmet", false);
                internalN.AddNode(seat);
                for (int i = 1; i < 385; i++)
                {
                    seat = new ConfigNode("MODULE");
                    seat.AddValue("name ", "InternalSeat");
                    seat.AddValue("seatTransformName ", "seat_" + i.ToString("D3"));
                    seat.AddValue("allowCrewHelmet", false);
                    internalN.AddNode(seat);
                }

                rootN.AddNode(internalN);
                rootN.Save(INTERNALTestCFG_path);
                */
                runOnce = true;
                    UnityEngine.Debug.Log("Station69 FixedUpdate: runOnce done ");
            }
            if ((Time.time - lastFixedUpdate) > logInterval)
            {


                if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
                {
                    Station69Part.SetHighlight(false, true); // turn off the vessel highlight while in EVA
                }

                if (ShieldEmmitter_IsRunning)
                {
                    Station69Part.crashTolerance = 6000000000f;
                    Station69Part.maxTemp = 31000000000f;
                }
                else
                {
                    Station69Part.crashTolerance = 60f;
                    Station69Part.maxTemp = 3100f;
                }



                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: begin ");


                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: 7 ");
            }
        }
    }
}
