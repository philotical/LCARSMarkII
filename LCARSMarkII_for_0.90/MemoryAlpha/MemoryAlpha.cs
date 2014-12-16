using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class MemoryAlpha : PartModule
    {

        string SystemManagementWindow = null;
        Transform kerbal = null;

        Transform AsteroidTransform = null;

        Transform ForceFieldAccess1 = null;
        Transform ForceFieldAccess2 = null;
        Transform ForceFieldAccess3 = null;

        bool UserHasAccess1 = false;
        bool UserHasAccess2 = false;
        bool UserHasAccess3 = false;

        bool ForceFieldAccess1_IsLocked = true;
        bool ForceFieldAccess1_IsLocked_backup = false;
        bool ForceFieldAccess2_IsLocked = true;
        bool ForceFieldAccess2_IsLocked_backup = false;
        bool ForceFieldAccess3_IsLocked = true;
        bool ForceFieldAccess3_IsLocked_backup = false;

        Transform ActionPoint_Dome1_Shelve1 = null;
        Transform ActionPoint_Dome1_Shelve2 = null;
        Transform ActionPoint_Dome1_Shelve3 = null;
        Transform ActionPoint_Dome1_Observation = null;

        Transform ActionPoint_Dome2_Shelve1 = null;
        Transform ActionPoint_Dome2_Shelve2 = null;
        Transform ActionPoint_Dome2_Shelve3 = null;
        Transform ActionPoint_Dome2_Observation = null;

        Transform ActionPoint_Dome3_Shelve1 = null;
        Transform ActionPoint_Dome3_Shelve2 = null;
        Transform ActionPoint_Dome3_Shelve3 = null;
        Transform ActionPoint_Dome3_Observation = null;

        Transform ActionPoint_Outside1 = null;
        Transform ActionPoint_Outside2 = null;
        Transform ActionPoint_Outside3 = null;

        bool ShieldEmmitter_IsRunning = true;
        bool TransportScrambler_IsRunning = true;



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

                    case "TransportScrambler": // turns off the light and changes some string values
                        TransportScrambler_IsRunning = !isDisabled;
                        break;

                    /*
                     GUIWINDOWS
                     */
                }
            }
        }
        
        
        
        int doorState = 1;
        
        [KSPEvent(guiActive = false)]
        void runDoorAnimations()
        {
            if (doorState == 1)
            {
                UnityEngine.Debug.Log("MemoryAlpha runAnimations:  Door1 doorState == 1 ");
                foreach (Animation animation in MemoryAlphaPart.FindModelAnimators("MechanicalDoor1"))
                {
                    animation.Rewind();
                    animation.PlayQueued("MechanicalDoor1");
                }
            }
            if (doorState == 2)
            {
                UnityEngine.Debug.Log("MemoryAlpha runAnimations:  Door1 doorState == 2 ");
                foreach (Animation animation in MemoryAlphaPart.FindModelAnimators("MechanicalDoor2"))
                {
                    animation.Rewind();
                    animation.PlayQueued("MechanicalDoor2");
                }
            }
        }



        private Rect WindowPosition = new Rect(120, 120, 380, 230);
        private int WindowID = new System.Random().Next();
        private void OnGUI()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
                return;

            if (SystemManagementWindow == null)
                return;


            
            switch (SystemManagementWindow)
            {
                case "MemoryAlpha_AccessControll_Level1":
                    UnityEngine.Debug.Log("MemoryAlpha OnGUI: SystemManagementWindow=" + SystemManagementWindow);
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, MemoryAlpha_AccessControll_Level1_GUI, "");
                    break;

                case "MemoryAlpha_AccessControll_Level2":
                    UnityEngine.Debug.Log("MemoryAlpha OnGUI: SystemManagementWindow=" + SystemManagementWindow);
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, MemoryAlpha_AccessControll_Level2_GUI, "");
                    break;

                case "MemoryAlpha_AccessControll_Level3":
                    UnityEngine.Debug.Log("MemoryAlpha OnGUI: SystemManagementWindow=" + SystemManagementWindow);
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, MemoryAlpha_AccessControll_Level3_GUI, "");
                    break;

            }
            //UnityEngine.Debug.Log("MemoryAlpha OnGUI: no override defined for " + SystemManagementWindow);



        }
        int MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
        void MemoryAlpha_AccessControll_Level1_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));

            if (MemoryAlpha_AccessControll_Level1_GUI_menu_mode==0)
            {
                GUILayout.Label(" .:: Welcome To Memory Alpha ::. ", GUILayout.Width(280));
                GUILayout.Label("You can only pass with a Valid Access Code");
                GUILayout.Label("If you do not have one, click on SignUp");
                GUILayout.Label(" *********************************** ", GUILayout.Width(280));

                if (GUILayout.Button("Identification"))
                {
                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 1;
                }
                if (GUILayout.Button("SignUp"))
                {
                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 2;
                }
                if (GUILayout.Button("Info Panel"))
                {
                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 3;
                }
            }
            else
            {
                if (GUILayout.Button("Back"))
                {
                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                }
                switch (MemoryAlpha_AccessControll_Level1_GUI_menu_mode)
                {
                    case 1:
                        GUILayout.Label(" .:: Please show me your Access Code ::. ", GUILayout.Width(280));
                            if (UserHasAccess1)
                            {
                                if (GUILayout.Button("Show Access 1"))
                                {
                                    ForceFieldAccess1_IsLocked = false;
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("never mind.."))
                                {
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                            }
                        break;

                    case 2:
                        GUILayout.Label(" .:: We do not have you in our Scientists Database! ::. ", GUILayout.Width(280));
                        GUILayout.Label("What is your field of research?");
                        GUILayout.Label(" *********************************** ", GUILayout.Width(280));
                                if (GUILayout.Button("Pizza"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1",true,true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Warp Fields"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Impulse Drives"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("'Splosions"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Comics"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Jazz"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Vulcan Philosophy"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Movies"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                                if (GUILayout.Button("Literature"))
                                {
                                    LCARSNCI_Bridge.Instance.giveArtefactToPlayer("MemoryAlphaAccessCode1", true, true);
                                    MemoryAlpha_AccessControll_Level1_GUI_menu_mode = 0;
                                }
                        break;

                    case 3:
                        GUILayout.Label(" .:: We protect your knowledge! ::. ", GUILayout.Width(280));
                        GUILayout.Label("This facility is protected by:");
                        GUILayout.Label(" *********************************** ", GUILayout.Width(280));

                        if (ShieldEmmitter_IsRunning)
                        {
                            GUILayout.Label("- ShieldGenerator, some torpedoes do not threaten us");
                        }
                        else 
                        {
                            GUILayout.Label("!!WARNING!! SHIELDEMITTER IS OFFLINE - DANGER, DANGER!!");
                        }
                        if (TransportScrambler_IsRunning)
                        {
                            GUILayout.Label("- TransporterScrambler, no one reaches a dome without permission");
                        }
                        else 
                        {
                            GUILayout.Label("!!WARNING!! TRANSPORTSCRAMBLER IS OFFLINE - DANGER, DANGER!!");
                        }
                        break;


                }
            }


            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        int MemoryAlpha_AccessControll_Level2_GUI_menu_mode = 0;
        void MemoryAlpha_AccessControll_Level2_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));

            GUILayout.Label(" .:: Please show me your Level 2 Access Code ::. ", GUILayout.Width(280));
            if (UserHasAccess2)
            {
                if (GUILayout.Button("Show Access 2"))
                {
                    ForceFieldAccess2_IsLocked = false;
                }
            }
            else
            {
                if (GUILayout.Button("never mind.."))
                {
                    SystemManagementWindow = "";
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        int MemoryAlpha_AccessControll_Level3_GUI_menu_mode = 0;
        void MemoryAlpha_AccessControll_Level3_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));

            GUILayout.Label(" .:: Please show me your Level 3 Access Code ::. ", GUILayout.Width(280));
            if (UserHasAccess3)
            {
                if (GUILayout.Button("Show Access 3"))
                {
                    ForceFieldAccess3_IsLocked = false;
                }
            }
            else
            {
                if (GUILayout.Button("never mind.."))
                {
                    SystemManagementWindow = "";
                }
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }



        bool runOnce = false;
        bool openDoor = false;
        float dist = 0f;
        bool NCI_LCARSShipSystems_are_damaged=false;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 1.0f;
        Part MemoryAlphaPart = null;
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

            MemoryAlphaPart = this.vessel.rootPart;

            if (!runOnce)
            {
            }

            if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
            {
                //MemoryAlphaPart.SetHighlight(false); // turn off the vessel highlight while in EVA
                MemoryAlphaPart.SetHighlight(false, true);
            }

            if ((Time.time - lastFixedUpdate) > logInterval)
            {
                lastFixedUpdate = Time.time;
                try
                {
                    UserHasAccess1 = LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact("MemoryAlphaAccessCode1");
                    UserHasAccess2 = LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact("MemoryAlphaAccessCode2");
                    UserHasAccess3 = LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact("MemoryAlphaAccessCode3");
                    UnityEngine.Debug.Log("MemoryAlpha OnGUI: LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact UserHasAccess1=" + UserHasAccess1 + " UserHasAccess2=" + UserHasAccess2 + " UserHasAccess3=" + UserHasAccess3);
                }
                catch
                {
                    UnityEngine.Debug.Log("MemoryAlpha OnGUI: LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact failed ");
                }


                if (ShieldEmmitter_IsRunning)
                {
                    MemoryAlphaPart.crashTolerance = 6000000000f;
                    MemoryAlphaPart.maxTemp = 31000000000f;
                }
                else
                {
                    MemoryAlphaPart.crashTolerance = 60f;
                    MemoryAlphaPart.maxTemp = 3100f;
                }

                if (TransportScrambler_IsRunning)
                {
                    /* disable LCARS Systems here*/
                    #region LCARS Connect
                    // ******************************************************
                    // Connect to LCARS.ODN and interfere with ship systems:
                    try
                    {

                        if (FlightGlobals.ActiveVessel.LCARS() != null)
                        {
                            //Debug.Log("MemoryAlpha FixedUpdate FlightGlobals.ActiveVessel.LCARS() found ");
                            dist = Vector3.Distance(FlightGlobals.ActiveVessel.transform.position, this.vessel.CoM);
                            //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate:  NCIForceFieldHiddenAccessPoint:  dist=" + dist);
                            if (dist < 1000f)
                            {
                                //Debug.Log("MemoryAlpha FixedUpdate dist < 5000f ");

                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems = new List<string>(); }
                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems = new List<string>(); }
                                if (!FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Contains("Transporter Systems"))
                                {
                                    FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Add("Transporter Systems");
                                }
                                /*
                                if (!NCI_LCARSShipSystems_are_damaged)
                                {
                                    if (!LCARS_NCI.Instance.getLCARS_of_ActiveVessel().lODN.ShipStatus.NCI_missionOptions_DamagedSystems.Contains("Sensor Array"))
                                    {
                                        LCARS_NCI.Instance.getLCARS_of_ActiveVessel().lODN.ShipStatus.NCI_missionOptions_DamagedSystems.Add("Sensor Array");
                                        NCI_LCARSShipSystems_are_damaged = true;
                                    }
                                }
                                */
                            }
                        }
                    }
                    catch
                    {
                        //Debug.Log("MemoryAlpha FixedUpdate LCARS.ODN not found ");
                    }
                    // Connect to LCARS.ODN and interfere with ship systems:
                    // ******************************************************
                    #endregion
                }


                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: begin ");

                SystemManagementWindow = null;

                if (ForceFieldAccess1_IsLocked != ForceFieldAccess1_IsLocked_backup)
                {
                    if (ForceFieldAccess1_IsLocked)
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess1").collider.enabled = true;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess1 locked ");
                    }
                    else
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess1").collider.enabled = false;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess1 unlocked ");
                    }
                    ForceFieldAccess1_IsLocked_backup = ForceFieldAccess1_IsLocked;
                }
                if (ForceFieldAccess2_IsLocked != ForceFieldAccess2_IsLocked_backup)
                {
                    if (ForceFieldAccess2_IsLocked)
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess2").collider.enabled = true;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess2 locked ");
                    }
                    else
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess2").collider.enabled = false;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess2 unlocked ");
                    }
                    ForceFieldAccess2_IsLocked_backup = ForceFieldAccess2_IsLocked;
                }
                if (ForceFieldAccess3_IsLocked != ForceFieldAccess3_IsLocked_backup)
                {
                    if (ForceFieldAccess3_IsLocked)
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess2").collider.enabled = true;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess3 locked ");
                    }
                    else
                    {
                        MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess2").collider.enabled = false;
                        UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: ForceFieldDomeAccess3 unlocked ");
                    }
                    ForceFieldAccess3_IsLocked_backup = ForceFieldAccess3_IsLocked;
                }




                if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
                {
                    //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: VesselType.EVA ");

                    kerbal = FlightGlobals.ActiveVessel.rootPart.transform;

                    ForceFieldAccess1 = MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess1");
                    ForceFieldAccess2 = MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess2");
                    ForceFieldAccess3 = MemoryAlphaPart.FindModelTransform("ForceFieldDomeAccess3");



                    //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: 2 ");

                    SystemManagementWindow = "";

                    dist = Vector3.Distance(ForceFieldAccess1.position, kerbal.position);
                    //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: door locked NCIMechanicalDoorAccessPoint vs. " + kerbal.name + ":  dist=" + dist);
                    if (dist < 5f)
                    {
                        SystemManagementWindow = "MemoryAlpha_AccessControll_Level1";
                    }
                    else
                    {
                        //SystemManagementWindow = SystemManagementWindow;
                    }

                    dist = Vector3.Distance(ForceFieldAccess2.position, kerbal.position);
                    //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate:  NCIComputerConsolePoint vs. " + kerbal.name + ":  dist=" + dist);
                    if (dist < 1f)
                    {
                        SystemManagementWindow = "MemoryAlpha_AccessControll_Level2";
                    }
                    else
                    {
                        //SystemManagementWindow = SystemManagementWindow;
                    }

                    dist = Vector3.Distance(ForceFieldAccess3.position, kerbal.position);
                        //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate:  NCIHabitatSectionConsolePoint vs. " + kerbal.name + ":  dist=" + dist);
                    if (dist < 1f)
                    {
                        SystemManagementWindow = "MemoryAlpha_AccessControll_Level3";
                    }
                    else
                    {
                        //SystemManagementWindow = SystemManagementWindow;
                    }
                    //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: 6 ");

                }
                //UnityEngine.Debug.Log("MemoryAlpha FixedUpdate: 7 ");
            }
        }
    }
}
