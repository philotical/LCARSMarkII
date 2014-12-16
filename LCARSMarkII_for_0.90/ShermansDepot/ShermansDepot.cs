using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class ShermansDepot : PartModule
    {

        string SystemManagementWindow = null;

        Part ShermanDepotPart = null;

        Transform kerbal = null;
        Transform NCIMechanicalDoorAccessPoint = null;
        Transform NCIComputerConsolePoint = null;
        Transform NCIForceFieldHiddenAccessPoint = null;
        Transform NCIHabitatSectionConsolePoint = null;

        bool ReactorIsRunning = true;
        bool ShieldEmmitterIsRunning = true;
        bool TransportScramblerIsRunning = true;
        bool FroceFieldsIsLocked = false;
        bool FroceFieldsIsLocked_backup = true;
        bool AirlocksIsLocked = false;
        bool Deck1IsLocked = true;
        bool Deck2IsLocked = false;
        bool Deck3IsLocked = false;
        bool Deck4IsLocked = true;
        bool Deck5IsLocked = true;



        /*
         * NCI can send settings with this
         * The OptionID is what you have set in the NCI_Object.cfg file for this object
         * the values of the fields "doors" and "GUIWindows" will be recognized by NCI Missions
         * It's simple, either this part plugin shows and uses it's own settings for doors and windows, 
         * OR NCI overwrites locks doors and replaces windows with own conversations.
        actionpoints = Deck1_Bridge_001,Deck1_Bridge_002,Deck1_Bridge_003,Deck1_004,Deck1_005,Deck1_006,Deck1_007,Deck2_001,Deck2_002,Deck2_003,Deck2_004,Deck2_005,Deck2_006,Deck4_001,Deck4_002,Deck4_003,Deck4_004,Deck4_005,Deck4_006,Deck4_007,Deck4_008,Deck4_009,Deck4_010,Deck4_011,Deck4_012,Deck5_001,Deck5_002,Deck5_003,Deck5_004,Deck5_005,Deck5_ReactorCore
	    doors = DeckPassageDoor1,DeckPassageDoor2,DeckPassageDoor3,DeckPassageDoor4,DeckPassageDoor5,DoorNode1,DoorNode2,DoorNode3,DoorNode4,DoorNode5,DoorNode6
	    GUIWindows = 
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
                    case "ForceField": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ForceField=" + FroceFieldsIsLocked);
                        FroceFieldsIsLocked = isDisabled;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ForceField=" + FroceFieldsIsLocked);
                        break;

                    case "Airlocks": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  AirlocksIsLocked=" + AirlocksIsLocked);
                        AirlocksIsLocked = isDisabled;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  AirlocksIsLocked=" + AirlocksIsLocked);
                        break;

                    case "DeckPassageDoor1": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck1IsLocked=" + Deck1IsLocked);
                        Deck1IsLocked = isDisabled;
                        DeckPassageDoor1.collider.enabled = Deck1IsLocked;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck1IsLocked=" + Deck1IsLocked);
                        break;

                    case "DeckPassageDoor2": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck2IsLocked=" + Deck2IsLocked);
                        Deck2IsLocked = isDisabled;
                        DeckPassageDoor2.collider.enabled = Deck2IsLocked;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck2IsLocked=" + Deck2IsLocked);
                        break;

                    case "DeckPassageDoor3": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck3IsLocked=" + Deck3IsLocked);
                        Deck3IsLocked = isDisabled;
                        DeckPassageDoor3.collider.enabled = Deck3IsLocked;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck3IsLocked=" + Deck3IsLocked);
                       break;

                    case "DeckPassageDoor4": // it's a door, if true, the door is locked as defined in mission cfg
                       UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck4IsLocked=" + Deck4IsLocked);
                        Deck4IsLocked = isDisabled;
                        DeckPassageDoor4.collider.enabled = Deck4IsLocked;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck4IsLocked=" + Deck4IsLocked);
                        break;

                    case "DeckPassageDoor5": // it's a door, if true, the door is locked as defined in mission cfg
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck5IsLocked=" + Deck5IsLocked);
                        Deck5IsLocked = isDisabled;
                        DeckPassageDoor5.collider.enabled = Deck5IsLocked;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  Deck5IsLocked=" + Deck5IsLocked);
                        break;

                    case "SPECIAL_ReactorCoreIsRunning": // turns off the light and changes some string values
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ReactorIsRunning=" + ReactorIsRunning);
                        ReactorIsRunning = !isDisabled;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ReactorIsRunning=" + ReactorIsRunning);
                        break;

                    case "SPECIAL_ShieldEmmitters": // changes the max heat of the part and makes it (in-)destructable
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ShieldEmmitterIsRunning=" + ShieldEmmitterIsRunning);
                        ShieldEmmitterIsRunning = !isDisabled;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  ShieldEmmitterIsRunning=" + ShieldEmmitterIsRunning);
                        break;

                    case "SPECIAL_TransportScrambler": // changes the max heat of the part and makes it (in-)destructable
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  TransportScramblerIsRunning=" + TransportScramblerIsRunning);
                        TransportScramblerIsRunning = !isDisabled;
                        UnityEngine.Debug.Log("ShermansDepot NCIRemote_Generic_EnableDisable:  TransportScramblerIsRunning=" + TransportScramblerIsRunning);
                        break;

                    /*
                     GUIWINDOWS
                     */
                }
            }
        }
        
        
        
        int doorState = 1;

        void runDoorAnimations()
        {
            if (doorState == 1)
            {
                switch (DoorID)
                {
                    case "1":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock1_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door1 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock1_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door1 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door1").gameObject);
                        }
                        break;
                    case "2":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock2_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door2 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock2_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door2 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door2").gameObject);
                        }
                        break;
                    case "3":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock3_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door3 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock3_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door3 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door3").gameObject);
                        }
                        break;
                    case "4":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock4_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door4 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock4_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door4 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door4").gameObject);
                        }
                        break;
                    case "5":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock5_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door5 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock5_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door5 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_largeDoor", ShermanDepotPart.FindModelTransform("Door5").gameObject);
                        }
                        break;
                    case "6":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock6_o"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door6 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock6_o");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door6 end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door6").gameObject);
                        }
                        break;
                }
            }
            if (doorState == 2)
            {
                switch (DoorID)
                {
                    case "1":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock1_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door1 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock1_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door1c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door1").gameObject);
                        }
                        break;
                    case "2":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock2_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door2 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock2_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door2c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door2").gameObject);
                        }
                        break;
                    case "3":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock3_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door3 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock3_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door3c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door3").gameObject);
                        }
                        break;
                    case "4":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock4_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door4 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock4_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door4c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door4").gameObject);
                        }
                        break;
                    case "5":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock5_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door5 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock5_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door5c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_largeDoor", ShermanDepotPart.FindModelTransform("Door5").gameObject);
                        }
                        break;
                    case "6":
                        foreach (Animation animation in ShermanDepotPart.FindModelAnimators("Airlock6_c"))
                        {
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door6 begin ");
                            animation.Rewind();
                            animation.PlayQueued("Airlock6_c");
                            //UnityEngine.Debug.Log("ShermansDepot runAnimations:  Door6c end ");
                            //LCARSRef.lAudio.play("ShermansDepot_smallDoor", ShermanDepotPart.FindModelTransform("Door6").gameObject);
                        }
                        break;
                }
            }

        }






        bool runOnce = false;
        bool runOnceTwo = false;
        bool openDoor = false;
        float dist = 0f;
        bool ReactorIsRunning_backup = true;
        bool NCI_LCARSShipSystems_are_damaged=false;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 1.0f;
        float closestDistance = 0;
        string DoorID = "0";
        Transform DeckPassageDoor1 = null;
        Transform DeckPassageDoor2 = null;
        Transform DeckPassageDoor3 = null;
        Transform DeckPassageDoor4 = null;
        Transform DeckPassageDoor5 = null;
        Transform DoorNode1 = null;
        Transform DoorNode2 = null;
        Transform DoorNode3 = null;
        Transform DoorNode4 = null;
        Transform DoorNode5 = null;
        Transform DoorNode6 = null;
        void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null || this.vessel.loaded==false)
            { return; }

            if(!runOnce)
            {
                UnityEngine.Debug.Log("ShermansDepot FixedUpdate:  close door fix failed start animation ");
                doorState = 2;
                runDoorAnimations();
                doorState = 0;

                runOnce = true;
            }

            /*if (ShermanDepotPart == null)
            {
                foreach(Part p in this.vessel.parts)
                {
                    if (p.name == "ShermansDepot")
                    {
                        ShermanDepotPart = p;
                    }
                }
            }
            */

            //this.vessel.transform.LookAt(this.vessel.mainBody.transform.position - this.vessel.transform.position, this.vessel.mainBody.transform.up);
            /*
            Vector3 relativePos = this.vessel.mainBody.transform.up;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            this.vessel.transform.rotation = rotation;
            */
            /*
            var pos = this.vessel.mainBody.transform.position; // get the target position...
            pos.y = 0f; // but at this object's height
            //this.vessel.transform.LookAt(pos); // then look at it
            this.vessel.transform.forward = this.vessel.transform.rotation * pos;
            //this.vessel.transform.position.y = this.vessel.mainBody.transform.position.y;
            */

            //this.vessel.transform.LookAt(this.vessel.GetObtVelocity().normalized, this.vessel.mainBody.transform.up);

            /*
            Vector3 tmp = Vector3.Cross(this.vessel.mainBody.transform.up, this.vessel.mainBody.transform.forward);
            this.vessel.transform.LookAt(Vector3.Cross(tmp, this.vessel.GetObtVelocity()).normalized, this.vessel.mainBody.transform.up);
            */

            this.vessel.transform.rotation = Quaternion.LookRotation(Vector3d.left, Vector3d.up);// *Quaternion.AngleAxis(-0f, Vector3d.forward);

            ShermanDepotPart = this.vessel.rootPart;

            ShermanDepotPart.SetHighlight(false, true); // turn off the vessel highlight while in EVA or IVA

            if ((Time.time - lastFixedUpdate) > logInterval)
            {
                lastFixedUpdate = Time.time;



                
                
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ShieldEmmitterIsRunning begin ");
                if (ShieldEmmitterIsRunning)
                {
                    ShermanDepotPart.crashTolerance = 6000000f;
                    ShermanDepotPart.maxTemp = 31000000f;
                }
                else
                {
                    ShermanDepotPart.crashTolerance = 60f;
                    ShermanDepotPart.maxTemp = 3100f;
                }
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ShieldEmmitterIsRunning end ");
                /*
                */


                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ReactorIsRunning begin ");
                if (ReactorIsRunning != ReactorIsRunning_backup)
                {
                    if (ReactorIsRunning)
                    {
                        ShermanDepotPart.FindModelTransform("Lamp001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp001_001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp002").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp002_001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp003").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp003_001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp004").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp004_001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp005").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp005_001").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp006").light.enabled = true;
                        ShermanDepotPart.FindModelTransform("Lamp006_001").light.enabled = true;
                    }
                    else
                    {
                        ShermanDepotPart.FindModelTransform("Lamp001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp001_001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp002").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp002_001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp003").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp003_001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp004").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp004_001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp005").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp005_001").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp006").light.enabled = false;
                        ShermanDepotPart.FindModelTransform("Lamp006_001").light.enabled = false;
                    }
                    ReactorIsRunning_backup = ReactorIsRunning;
                }
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ReactorIsRunning end ");
                /*
                */

                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: disable LCARS begin ");
                if (ReactorIsRunning)
                {
                    /* disable LCARS Systems here*/
                    #region LCARS Connect
                    // ******************************************************
                    // Connect to LCARS.ODN and interfere with ship systems:
                    try
                    {

                        if (FlightGlobals.ActiveVessel.LCARS() != null)
                        {
                            //Debug.Log("ShermansDepot FixedUpdate FlightGlobals.ActiveVessel.LCARS() found ");
                            dist = Vector3.Distance(FlightGlobals.ActiveVessel.transform.position, this.vessel.CoM);
                            //UnityEngine.Debug.Log("ShermansDepot FixedUpdate:  NCIForceFieldHiddenAccessPoint:  dist=" + dist);
                            if (dist < 1000f)
                            {
                                //Debug.Log("ShermansDepot FixedUpdate dist < 5000f ");

                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems = new List<string>(); }
                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems = new List<string>(); }
                                if (!FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Contains("Transporter Systems"))
                                {
                                    FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Add("Transporter Systems");
                                }
                            }
                        }
                    }
                    catch
                    {
                        //Debug.Log("ShermansDepot FixedUpdate LCARS.ODN not found ");
                    }
                    // Connect to LCARS.ODN and interfere with ship systems:
                    // ******************************************************
                    #endregion
                }
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: disable LCARS end ");


                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: FroceFieldsIsLocked begin ");

                if (FroceFieldsIsLocked != FroceFieldsIsLocked_backup)
                {
                    
                    ShermanDepotPart.FindModelTransform("ForceField").collider.enabled = FroceFieldsIsLocked;
                    if (FroceFieldsIsLocked)
                    {
                        UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ForceField locked ");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("ShermansDepot FixedUpdate: ForceField unlocked ");
                    }
                    FroceFieldsIsLocked_backup = FroceFieldsIsLocked;
                }
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: FroceFieldsIsLocked end ");
                /*
                 */ 
/*
*/
                UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoors begin ");
                if (DeckPassageDoor1 == null)
                {
                    DeckPassageDoor1 = ShermanDepotPart.FindModelTransform("DeckPassageForceField001");
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoor1 found ");
                }
                if (DeckPassageDoor2 == null)
                {
                    DeckPassageDoor2 = ShermanDepotPart.FindModelTransform("DeckPassageForceField002");
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoor2 found ");
                }
                if (DeckPassageDoor3 == null)
                {
                    DeckPassageDoor3 = ShermanDepotPart.FindModelTransform("DeckPassageForceField003");
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoor3 found ");
                }
                if (DeckPassageDoor4 == null)
                {
                    DeckPassageDoor4 = ShermanDepotPart.FindModelTransform("DeckPassageForceField004");
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoor4 found ");
                }
                if (DeckPassageDoor5 == null)
                {
                    DeckPassageDoor5 = ShermanDepotPart.FindModelTransform("DeckPassageForceField005");
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoor5 found ");
                }
                UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoors half ways ");


                if (!runOnceTwo && DeckPassageDoor1 != null && DeckPassageDoor2 != null && DeckPassageDoor3 != null && DeckPassageDoor4 != null && DeckPassageDoor5 != null)
                {
                    UnityEngine.Debug.Log("ShermansDepot FixedUpdate runOnceTwo:  begin ");

                    DeckPassageDoor1.collider.enabled = Deck1IsLocked;
                    DeckPassageDoor2.collider.enabled = Deck2IsLocked;
                    DeckPassageDoor3.collider.enabled = Deck3IsLocked;
                    DeckPassageDoor4.collider.enabled = Deck4IsLocked;
                    DeckPassageDoor5.collider.enabled = Deck5IsLocked;

                    runOnceTwo = true;
                }

                UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoors Status: 1=" + Deck1IsLocked + " 2=" + Deck2IsLocked + " 3=" + Deck3IsLocked + " 4=" + Deck4IsLocked + " 5=" + Deck5IsLocked + " ");
                UnityEngine.Debug.Log("ShermansDepot FixedUpdate: DeckPassageDoors: end ");


                if (DoorNode1 == null)
                {
                    DoorNode1 = ShermanDepotPart.FindModelTransform("Airlock1");
                }
                if (DoorNode2 == null)
                {
                    DoorNode2 = ShermanDepotPart.FindModelTransform("Airlock2");
                }
                if (DoorNode3 == null)
                {
                    DoorNode3 = ShermanDepotPart.FindModelTransform("Airlock3");
                }
                if (DoorNode4 == null)
                {
                    DoorNode4 = ShermanDepotPart.FindModelTransform("Airlock4");
                }
                if (DoorNode5 == null)
                {
                    DoorNode5 = ShermanDepotPart.FindModelTransform("Airlock5");
                }
                if (DoorNode6 == null)
                {
                    DoorNode6 = ShermanDepotPart.FindModelTransform("Airlock6");
                }

                if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
                {
                    //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: VesselType.EVA ");

                    kerbal = FlightGlobals.ActiveVessel.rootPart.transform;

                    //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: 2 ");
	    //doors = DeckPassageDoor1,DeckPassageDoor2,DeckPassageDoor3,DeckPassageDoor4,DeckPassageDoor5,DoorNode1,DoorNode2,DoorNode3,DoorNode4,DoorNode5,DoorNode6

                    //DoorID = "0";
                    openDoor = false;
                    
                    /*
                    closestDistance = (doorState == 0) ? 1000 : closestDistance;
                    dist = Vector3.Distance(DeckPassageDoor1.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor1 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !Deck1IsLocked;
                        DoorID = "7";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor1  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DeckPassageDoor2.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor2 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !Deck2IsLocked;
                        DoorID = "7";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor2  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DeckPassageDoor3.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor3 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !Deck3IsLocked;
                        DoorID = "7";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor3 openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DeckPassageDoor4.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor4 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !Deck4IsLocked;
                        DoorID = "7";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor4  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DeckPassageDoor5.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor5 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !Deck5IsLocked;
                        DoorID = "7";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DeckPassageDoor5  openDoor=" + openDoor);
                    }
                    */

                    dist = Vector3.Distance(DoorNode1.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode1 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "1";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode1  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DoorNode2.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode2 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "2";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode2  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DoorNode3.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode3 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "3";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode3  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DoorNode4.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode4 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "4";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode4  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DoorNode5.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode5 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "5";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode5  openDoor=" + openDoor);
                    }

                    dist = Vector3.Distance(DoorNode6.position, kerbal.position);
                    closestDistance = (dist < closestDistance) ? dist : closestDistance;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode6 dist=" + dist);
                    if (dist < 1.0f)
                    {
                        openDoor = !AirlocksIsLocked;
                        DoorID = "6";
                        UnityEngine.Debug.Log("ShermansDepot Update:  DoorNode6  openDoor=" + openDoor);
                    }
                    //UnityEngine.Debug.Log("ShermansDepot Update:  4 ");

                    if (doorState == 0)
                    {
                        //UnityEngine.Debug.Log("ShermansDepot Update:  doorState=0 ");
                        if (openDoor)
                        {
                            UnityEngine.Debug.Log("ShermansDepot Update:  openDoor ");
                            doorState = 1;
                            runDoorAnimations();
                        }
                    }
                    else
                    {
                        if (doorState == 1)
                        {
                            //UnityEngine.Debug.Log("ShermansDepot Update:  doorState=1 ");
                            if (openDoor)
                            {
                            }
                            else
                            {
                                UnityEngine.Debug.Log("ShermansDepot Update:  close door ");
                                doorState = 2;
                                runDoorAnimations();
                            }
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("ShermansDepot Update:  doorState=2 ");
                            doorState = 0;
                            DoorID = "0";
                        }
                    }

                    openDoor = false;
                    //UnityEngine.Debug.Log("ShermansDepot Update:  5 ");

                }
                //UnityEngine.Debug.Log("ShermansDepot FixedUpdate: 7 ");
            }
        }
    }
}
