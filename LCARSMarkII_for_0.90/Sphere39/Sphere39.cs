using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class Sphere39 : PartModule
    {

        string SystemManagementWindow = null;
        Transform kerbal = null;
        Transform NCIMechanicalDoorAccessPoint = null;
        Transform NCIComputerConsolePoint = null;
        Transform NCIForceFieldHiddenAccessPoint = null;
        Transform NCIHabitatSectionConsolePoint = null;

        bool ReactorIsRunning = true;
        bool ShieldEmmitterIsRunning = true;



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
        bool MechanicalDoor_isLocked = true;
        bool ForceField_isLocked = true;
        bool ForceField_isLocked_backup = false;
        bool Override_ComputerConsole = false;
        bool Override_ForceFieldHiddenAccess = false;
        bool Override_HabitatSectionConsole = false;
        bool Override_MechanicalDoorAccess = false;
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
                        ForceField_isLocked = isDisabled;
                        break;

                    case "MechanicalDoor": // it's a door, if true, the door is locked as defined in mission cfg
                        MechanicalDoor_isLocked = isDisabled;
                        break;

                    case "SPECIAL_ReactorCoreIsRunning": // turns off the light and changes some string values
                        ReactorIsRunning = !isDisabled;
                        break;

                    case "SPECIAL_ShieldEmmitters": // changes the max heat of the part and makes it (in-)destructable
                        ShieldEmmitterIsRunning = !isDisabled;
                        break;

                    /*
                     GUIWINDOWS
                     */
                    case "ComputerConsole": // it's a GUIWindow, if true, the default window is omitted as defined in mission cfg
                        Override_ComputerConsole = isDisabled;
                        break;

                    case "ForceFieldHiddenAccess": // it's a GUIWindow, if true, the default window is omitted as defined in mission cfg
                        Override_ForceFieldHiddenAccess = isDisabled;
                        break;

                    case "HabitatSectionConsole": // it's a GUIWindow, if true, the default window is omitted as defined in mission cfg
                        Override_HabitatSectionConsole = isDisabled;
                        break;

                    case "MechanicalDoorAccess": // it's a GUIWindow, if true, the default window is omitted as defined in mission cfg
                        Override_MechanicalDoorAccess = isDisabled;
                        break;
                }
            }
        }
        
        
        
        int doorState = 1;
        
        [KSPEvent(guiActive = false)]
        void runDoorAnimations()
        {
            if (doorState == 1)
            {
                UnityEngine.Debug.Log("Spere39 runAnimations:  Door1 doorState == 1 ");
                foreach (Animation animation in Spere39Part.FindModelAnimators("MechanicalDoor1"))
                {
                    animation.Rewind();
                    animation.PlayQueued("MechanicalDoor1");
                }
            }
            if (doorState == 2)
            {
                UnityEngine.Debug.Log("Spere39 runAnimations:  Door1 doorState == 2 ");
                foreach (Animation animation in Spere39Part.FindModelAnimators("MechanicalDoor2"))
                {
                    animation.Rewind();
                    animation.PlayQueued("MechanicalDoor2");
                }
            }
        }



        private Rect WindowPosition = new Rect(120, 120, 380, 230);
        private int WindowID = new System.Random().Next();
        string GravimetricReactorForce = "100";
        string GravimetricRadiation = "978440";
        private void OnGUI()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
                return;

            if (SystemManagementWindow == null)
                return;

            UnityEngine.Debug.Log("Spere39 OnGUI: SystemManagementWindow=" + SystemManagementWindow);
            switch (SystemManagementWindow)
            {
                case "NCIMechanicalDoorAccessPoint":
                    if (Override_MechanicalDoorAccess) { return; }
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, NCIMechanicalDoorWindow_GUI, "");
                    break;

                case "NCIComputerConsolePoint":
                    if (Override_ComputerConsole) { return; }
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, NCIComputerConsoleWindow_GUI, "");
                    break;

                case "NCIForceFieldHiddenAccessPoint":
                    if (Override_ForceFieldHiddenAccess) { return; }
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, NCIForceFieldHiddenAccessWindow_GUI, "");
                    break;

                case "NCIHabitatSectionConsolePoint":
                    if (Override_HabitatSectionConsole) { return; }
                    WindowPosition = GUILayout.Window(WindowID, WindowPosition, NCIHabitatSectionConsoleWindow_GUI, "");
                    break;
            }
            UnityEngine.Debug.Log("Spere39 OnGUI: no override defined for " + SystemManagementWindow);



        }
        void NCIMechanicalDoorWindow_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label(" .:: Transdimensional Door Lock ::. ", GUILayout.Width(280));

            if (MechanicalDoor_isLocked)
            {
                if (GUILayout.Button("Unlock the Door"))
                {
                    MechanicalDoor_isLocked = false;
                    //Override_MechanicalDoorAccess = true;
                }
            }
            else
            {
                if (GUILayout.Button("Lock the Door"))
                {
                    MechanicalDoor_isLocked = true;
                    //Override_MechanicalDoorAccess = false;
                }
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        void NCIComputerConsoleWindow_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label(" .:: Transdimensional Joke Reactor ::. ", GUILayout.Width(280));
            GUILayout.Label("You've made it, take a Nerd-Joke as reward:");
            GUILayout.Label("The Barkeep said: <We ain't gonna serve Faster-Than-Light-Neutrinos here!>. Shortly after that, the door opened and a Faster-Than-Light-Neutrino entered the bar.");
            GUILayout.Label(" *********************************** ", GUILayout.Width(280));

            GUILayout.Label(" .:: Transdimensional Gravimetric Reactor ::. ", GUILayout.Width(280));

            if (ReactorIsRunning)
            {
                if (GUILayout.Button("Reactor Shut Down"))
                {
                    ReactorIsRunning = false;
                }
            }
            else
            {
                if (GUILayout.Button("Reactor Set Nominal"))
                {
                    ReactorIsRunning = true;
                }
            }
            if (ShieldEmmitterIsRunning)
            {
                if (GUILayout.Button("ShieldEmmitter Shut Down"))
                {
                    ShieldEmmitterIsRunning = false;
                }
            }
            else
            {
                if (GUILayout.Button("ShieldEmmitter Set Nominal"))
                {
                    ShieldEmmitterIsRunning = true;
                }
            }
            GUILayout.Label("Transdimensional Operation Status", GUILayout.Width(280));

            GUILayout.Label("Gravimetric Reactor Force: " + GravimetricReactorForce + "%", GUILayout.Width(280));
            GUILayout.Label("Gravimetric Radiation: " + GravimetricRadiation + " Rad.", GUILayout.Width(280));
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        void NCIForceFieldHiddenAccessWindow_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label(" .:: Transdimensional ForceField ::. ", GUILayout.Width(280));

            GUILayout.Label("This seems to be a holographic force field! It's locked and solid.", GUILayout.Width(280));
            GUILayout.Label("We need to find an other entrance!.", GUILayout.Width(280));

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        int NCIHabitatSectionConsoleWindow_GUI_menu_mode = 0;
        void NCIHabitatSectionConsoleWindow_GUI(int ScienceManagementWindowID) //the default window
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label(" .:: Transdimensional Computer Panel ::. ", GUILayout.Width(280));

            switch (NCIHabitatSectionConsoleWindow_GUI_menu_mode)
            {
                case 0:
                        if (GUILayout.Button("Operation Status"))
                        {
                            NCIHabitatSectionConsoleWindow_GUI_menu_mode = 1;
                        }
                        if (GUILayout.Button("Security Grid"))
                        {
                            NCIHabitatSectionConsoleWindow_GUI_menu_mode = 2;
                        }
                    break;

                case 1:
                    if (GUILayout.Button("Back"))
                    {
                        NCIHabitatSectionConsoleWindow_GUI_menu_mode = 0;
                    }
                    GUILayout.Label("****************************");
                    GUILayout.Label("Transdimensional Operation Status", GUILayout.Width(280));

                    GUILayout.Label("Gravimetric Reactor Force: "+GravimetricReactorForce+"%", GUILayout.Width(280));
                    GUILayout.Label("Gravimetric Radiation: "+GravimetricRadiation+" Rad.", GUILayout.Width(280));
                    GUILayout.Label("Gravimetric Radiation can affect Systems:  ", GUILayout.Width(280));
                    GUILayout.Label("- Transporter Systems  ", GUILayout.Width(280));
                    GUILayout.Label("- Scanner Arrays  ", GUILayout.Width(280));
                    break;

                case 2:
                    if (GUILayout.Button("Back"))
                    {
                        NCIHabitatSectionConsoleWindow_GUI_menu_mode = 0;
                    }
                    GUILayout.Label("****************************");
                    GUILayout.Label("Transdimensional Security Grid", GUILayout.Width(280));
                    if (MechanicalDoor_isLocked)
                    {
                        if (GUILayout.Button("Unlock the Door"))
                        {
                            MechanicalDoor_isLocked = false;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Lock the Door"))
                        {
                            MechanicalDoor_isLocked = true;
                        }
                    }

                    if (ForceField_isLocked)
                    {
                        if (GUILayout.Button("Unlock the ForceField"))
                        {
                            ForceField_isLocked = false;
                            Override_ForceFieldHiddenAccess = true;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Lock the ForceField"))
                        {
                            ForceField_isLocked = true;
                            Override_ForceFieldHiddenAccess = false;
                        }
                    }
                    break;
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }



        bool runOnce = false;
        bool openDoor = false;
        float dist = 0f;
        bool ReactorIsRunning_backup = true;
        bool NCI_LCARSShipSystems_are_damaged=false;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 1.0f;
        Part Spere39Part = null;
        void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null || this.vessel.loaded == false)
            { return; }

            /*if (Spere39Part == null)
            {
                foreach (Part p in this.vessel.parts)
                {
                    if (p.name == "Spere39")
                    {
                        Spere39Part = p;
                    }
                }
            }*/
            this.vessel.transform.rotation = Quaternion.LookRotation(Vector3d.left, Vector3d.up);
            Spere39Part = this.vessel.rootPart;

            if (!runOnce)
            {
                UnityEngine.Debug.Log("Spere39 FixedUpdate:  close door fix failed start animation ");
                doorState = 2;
                runDoorAnimations();
                doorState = 0;

                runOnce = true;
            }
            if ((Time.time - lastFixedUpdate) > logInterval)
            {


                if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
                {
                    Spere39Part.SetHighlight(false, true); // turn off the vessel highlight while in EVA
                }

                if (ShieldEmmitterIsRunning)
                {
                    Spere39Part.crashTolerance = 6000000f;
                    Spere39Part.maxTemp = 31000000f;
                }
                else
                {
                    Spere39Part.crashTolerance = 60f;
                    Spere39Part.maxTemp = 3100f;
                }

                if (ReactorIsRunning != ReactorIsRunning_backup)
                {
                    if (ReactorIsRunning)
                    {
                        GravimetricReactorForce = "100";
                        GravimetricRadiation = "978440";
                        Spere39Part.FindModelTransform("MainReactorLight").light.enabled = true;
                    }
                    else
                    {
                        GravimetricReactorForce = "0";
                        GravimetricRadiation = "0";
                        Spere39Part.FindModelTransform("MainReactorLight").light.enabled = false;
                        /* enable LCARS Systems here*/
                    }
                    ReactorIsRunning_backup = ReactorIsRunning;
                }
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
                            Debug.Log("Spere39 FixedUpdate FlightGlobals.ActiveVessel.LCARS() found ");
                            dist = Vector3.Distance(FlightGlobals.ActiveVessel.transform.position, this.vessel.CoM);
                            //UnityEngine.Debug.Log("Spere39 FixedUpdate:  NCIForceFieldHiddenAccessPoint:  dist=" + dist);
                            if (dist < 5000f)
                            {
                                Debug.Log("Spere39 FixedUpdate dist < 5000f ");

                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems = new List<string>(); }
                                if (FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems == null) { FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DamagedSystems = new List<string>(); }
                                if (!FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Contains("Sensor Array"))
                                {
                                    FlightGlobals.ActiveVessel.LCARS().lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Add("Sensor Array");
                                }
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
                        //Debug.Log("Spere39 FixedUpdate LCARS.ODN not found ");
                    }
                    // Connect to LCARS.ODN and interfere with ship systems:
                    // ******************************************************
                    #endregion
                }


                //UnityEngine.Debug.Log("Spere39 FixedUpdate: begin ");

                SystemManagementWindow = null;

                if (ForceField_isLocked != ForceField_isLocked_backup)
                {
                    if (ForceField_isLocked)
                    {
                        Spere39Part.FindModelTransform("ForceField").collider.enabled = true;
                        UnityEngine.Debug.Log("Spere39 FixedUpdate: ForceField locked ");
                    }
                    else
                    {
                        Spere39Part.FindModelTransform("ForceField").collider.enabled = false;
                        UnityEngine.Debug.Log("Spere39 FixedUpdate: ForceField unlocked ");
                    }
                    ForceField_isLocked_backup = ForceField_isLocked;
                }

                //UnityEngine.Debug.Log("Spere39 FixedUpdate: 1 ");
                if (!Override_ForceFieldHiddenAccess)
                {
                    kerbal = FlightGlobals.ActiveVessel.transform;
                    NCIForceFieldHiddenAccessPoint = Spere39Part.FindModelTransform("NCIForceFieldHiddenAccessPoint");
                    dist = Vector3.Distance(NCIForceFieldHiddenAccessPoint.position, kerbal.position);
                    //UnityEngine.Debug.Log("Spere39 FixedUpdate:  NCIForceFieldHiddenAccessPoint:  dist=" + dist);
                    if (dist < 5f)
                    {
                        SystemManagementWindow = "NCIForceFieldHiddenAccessPoint";
                    }
                    else
                    {
                        //SystemManagementWindow = SystemManagementWindow;
                    }
                }
                //UnityEngine.Debug.Log("Spere39 FixedUpdate: 1b ");


                if (FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
                {
                    //UnityEngine.Debug.Log("Spere39 FixedUpdate: VesselType.EVA ");

                    kerbal = FlightGlobals.ActiveVessel.rootPart.transform;
                    NCIMechanicalDoorAccessPoint = Spere39Part.FindModelTransform("NCIMechanicalDoorAccessPoint");
                    NCIComputerConsolePoint = Spere39Part.FindModelTransform("NCIComputerConsolePoint");
                    NCIHabitatSectionConsolePoint = Spere39Part.FindModelTransform("NCIHabitatSectionConsolePoint");

                    //UnityEngine.Debug.Log("Spere39 FixedUpdate: 2 ");

                    if (!MechanicalDoor_isLocked)
                    {
                        dist = Vector3.Distance(NCIMechanicalDoorAccessPoint.position, kerbal.position);
                        //UnityEngine.Debug.Log("Spere39 FixedUpdate: door unlocked NCIMechanicalDoorAccessPoint vs. " + kerbal.name + ":  dist=" + dist);
                        if (dist < 2.5f)
                        {
                            openDoor = true;
                            //UnityEngine.Debug.Log("Spere39 FixedUpdate:  Door2 ");
                        }
                        if (doorState == 0)
                        {
                            //UnityEngine.Debug.Log("Spere39 FixedUpdate:  doorState=0 ");
                            if (openDoor)
                            {
                                UnityEngine.Debug.Log("Spere39 FixedUpdate:  openDoor ");
                                doorState = 1;
                                runDoorAnimations();
                            }
                        }
                        else
                        {
                            if (doorState == 1)
                            {
                                //UnityEngine.Debug.Log("Spere39 FixedUpdate:  doorState=1 ");
                                if (openDoor)
                                {
                                }
                                else
                                {
                                    UnityEngine.Debug.Log("Spere39 FixedUpdate:  close door ");
                                    doorState = 2;
                                    runDoorAnimations();
                                }
                            }
                            else
                            {
                                //UnityEngine.Debug.Log("Spere39 FixedUpdate:  doorState=2 ");
                                doorState = 0;
                            }
                        }

                        openDoor = false;
                        /*
                         */
                    }
                    else
                    {
                    }
                    if (!Override_MechanicalDoorAccess)
                    {
                        dist = Vector3.Distance(NCIMechanicalDoorAccessPoint.position, kerbal.position);
                        //UnityEngine.Debug.Log("Spere39 FixedUpdate: door locked NCIMechanicalDoorAccessPoint vs. " + kerbal.name + ":  dist=" + dist);
                        if (dist < 5f)
                        {
                            SystemManagementWindow = "NCIMechanicalDoorAccessPoint";
                        }
                        else
                        {
                            //SystemManagementWindow = SystemManagementWindow;
                        }
                    }
                    //UnityEngine.Debug.Log("Spere39 FixedUpdate: 4 ");
                    if (!Override_ComputerConsole)
                    {
                        dist = Vector3.Distance(NCIComputerConsolePoint.position, kerbal.position);
                        //UnityEngine.Debug.Log("Spere39 FixedUpdate:  NCIComputerConsolePoint vs. " + kerbal.name + ":  dist=" + dist);
                        if (dist < 1f)
                        {
                            SystemManagementWindow = "NCIComputerConsolePoint";
                        }
                        else
                        {
                            //SystemManagementWindow = SystemManagementWindow;
                        }
                    }
                    //UnityEngine.Debug.Log("Spere39 FixedUpdate: 5 ");
                    if (!Override_HabitatSectionConsole)
                    {
                        dist = Vector3.Distance(NCIHabitatSectionConsolePoint.position, kerbal.position);
                        //UnityEngine.Debug.Log("Spere39 FixedUpdate:  NCIHabitatSectionConsolePoint vs. " + kerbal.name + ":  dist=" + dist);
                        if (dist < 1f)
                        {
                            SystemManagementWindow = "NCIHabitatSectionConsolePoint";
                        }
                        else
                        {
                            //SystemManagementWindow = SystemManagementWindow;
                        }
                    }
                    //UnityEngine.Debug.Log("Spere39 FixedUpdate: 6 ");

                }
                //UnityEngine.Debug.Log("Spere39 FixedUpdate: 7 ");
            }
        }
    }
}
