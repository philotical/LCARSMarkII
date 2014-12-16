using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LCARSMarkII;
using System.Globalization;

/*
MODULE
{
    name = LCARS_ImpulseDrive
    RESOURCE
    {
        name = ElectricCharge
        rate = 0.1
    }
}
MODULE
{
    name = LCARS_ShuttleBay
    volume = 450
}
MODULE
{
    name = LCARS_CargoBay
    maxTonnage = 200
}
 MODULE
{
    name = LCARS_CloakingDevice
}
 MODULE
{
    name = LCARS_StructuralIntegrityField
}
MODULE
{
    name = LCARS_FuelTransfer
}
MODULE
{
    name = LCARS_CrewQuartier
}
MODULE
{
    name = LCARS_TransporterSystem
}
MODULE
{
    name = LCARS_WeaponSystems
}
MODULE
{
    name = LCARS_PhotonTorpedo
}
MODULE
{
    name = LCARS_TractorBeam
}
MODULE
{
    name = LCARS_SensorArray
}
    
 */



//[LOG 19:26:59.356] [Orbit Targeter]: Target is Sphere39 (Sphere39)



//            lAudio.play("LCARS_ImpulseOff", FlightGlobals.ActiveVessel);


/*
  mat = new Material("Shader \"Lines/Colored Blended\" {" +
        "SubShader { Pass { " +
        " Blend SrcAlpha OneMinusSrcAlpha " +
        " ZWrite Off Cull Off Fog { Mode Off } " +
        " BindChannels {" +
        " Bind \"vertex\", vertex Bind \"color\", color }" +
        "} } }");
mat.hideFlags = HideFlags.HideAndDontSave;
mat.shader.hideFlags = HideFlags.HideAndDontSave;

 */
namespace LCARSMarkII
{
    public class LCARSMarkII : PartModule
    {
        public LCARS_Windows_Util lWindows;
        public LCARS_Util_FlightControll lFlightControll;
        public LCARS_GravityTools grav;
        public LCARS_Util_Toolbar lToolbar;
        public LCARS_Subsystem_Util lSubSys;
        public LCARS_PowerSystem_Util lPowSys;
        public LCARS_ODN_Util lODN;
        public LCARS_RepairTeam_Crew lRepairTeams;
        public LCARS_Communication_Util lComm;
        public LCARS_CrewQuartier lCQ;
        public LCARS_AudioLibrary lAudio;

        public LCARS_Bridge_Station lStationBridge;
        public LCARS_Helm_Station lStationHelm;
        public LCARS_Engineering_Station lStationEng;
        public LCARS_Communication_Station lStationComm;
        public LCARS_Science_Station lStationSci;
        public LCARS_Tactical_Station lStationTac;


        public LCARSMarkII LCARS;
        ConfigNode onLoadNode = null;

        public float EngineConstant = 100.0f;
        public float LCARS_thrust_x = 0f;
        public float LCARS_thrust_y = 0f;
        public float LCARS_thrust_z = 0f;

        public float backup_LCARS_thrust_x = 0f;
        public float backup_LCARS_thrust_y = 0f;
        public float backup_LCARS_thrust_z = 0f;

        [KSPField(isPersistant = true)]
        public string engine_type;

        [KSPField(isPersistant = true)]
        public int CrewQuartiers_MaxCrewSpace;
        [KSPField(isPersistant = true)]
        public int CrewQuartiers_UsedSpace;
        [KSPField(isPersistant = true)]
        public string CrewQuartiers_SeatGameObject_List;


        [KSPField(isPersistant = true)]
        public bool LCARS_FullHalt;
        [KSPField(isPersistant = true)]
        public bool LCARS_MakeSlowToSave;
        [KSPField(isPersistant = true)]
        public bool LCARS_FullImpulse;
        [KSPField(isPersistant = true)]
        public bool LCARS_UseReserves;
        [KSPField(isPersistant = true)]
        public bool LCARS_AccelerationLock;
        [KSPField(isPersistant = true)]
        public bool LCARS_ProgradeStabilizer;
        [KSPField(isPersistant = true)]
        public bool LCARS_SlowDown;
        [KSPField(isPersistant = true)]
        public bool LCARS_InertiaDamper;
        [KSPField(isPersistant = true)]
        public bool LCARS_HoldSpeed;
        [KSPField(isPersistant = true)]
        public float LCARS_HoldSpeed_value;
        [KSPField(isPersistant = true)]
        public bool LCARS_HoldHeight;
        [KSPField(isPersistant = true)]
        public float LCARS_HoldHeight_value;
        [KSPField(isPersistant = true)]
        public string LCARS_ShipSystems_data;
        //[KSPField(isPersistant = true)]
        //public string CommunicationQueue;


        [KSPField(isPersistant = true)]
        public Rect LCARS_Bridge_windowPosition;
        [KSPField(isPersistant = true)]
        public Rect LCARS_Helm_windowPosition ;
        [KSPField(isPersistant = true)]
        public Rect LCARS_Engineering_windowPosition ;
        [KSPField(isPersistant = true)]
        public Rect LCARS_Science_windowPosition ;
        [KSPField(isPersistant = true)]
        public Rect LCARS_Communication_windowPosition ;
        [KSPField(isPersistant = true)]
        public Rect LCARS_Tactical_windowPosition ;

        [KSPField(isPersistant = true)]
        public bool LCARS_Bridge_windowState;
        [KSPField(isPersistant = true)]
        public bool LCARS_Helm_windowState ;
        [KSPField(isPersistant = true)]
        public bool LCARS_Engineering_windowState ;
        [KSPField(isPersistant = true)]
        public bool LCARS_Science_windowState ;
        [KSPField(isPersistant = true)]
        public bool LCARS_Communication_windowState ;
        [KSPField(isPersistant = true)]
        public bool LCARS_Tactical_windowState ;



        //internal float MainComputerDefaultPowerUsage = 1250f;
        //internal float MainImpulseDefaultPowerUsage = 850f;



        public Vessel thisVessel = null;
        public Vessel thisHostVessel = null;

        [KSPField(isPersistant = true)]
        public bool WindowState { get; set; }

        public bool WindowState_bypass { get; set; }

        private bool isReady() 
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
            {
                return false;
            }
            if (this.vessel != FlightGlobals.ActiveVessel)
            {
                return false;
            }
            if (thisVessel == null)
            {
                thisVessel = this.vessel;
                thisHostVessel = thisVessel;

            }
            return true;
        }
        internal void setWindowState(bool state)
        {

            //UnityEngine.Debug.Log("### NCI setWindowState");
            WindowState = state;
            LCARS.lWindows.setWindowState("Bridge", state);
        }
        internal bool getWindowState()
        {
            //UnityEngine.Debug.Log("### NCI getWindowState");
            return WindowState;
        }

        /*
        */
        public override void OnLoad(ConfigNode node)
        {
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
            {
                return;
            }
            //UnityEngine.Debug.Log("### LCARSMarkII OnLoad node=" + node);
            

            try
            {
                CrewQuartiers_MaxCrewSpace = LCARS_Utilities.ToInt(node.GetValue("CrewQuartiers_MaxCrewSpace"));
                CrewQuartiers_UsedSpace = LCARS_Utilities.ToInt(node.GetValue("CrewQuartiers_UsedSpace"));
                CrewQuartiers_SeatGameObject_List = node.GetValue("CrewQuartiers_SeatGameObject_List");

                LCARS_FullHalt = (node.GetValue("LCARS_FullHalt") == "True") ? true : false;
                LCARS_MakeSlowToSave = (node.GetValue("LCARS_MakeSlowToSave") == "True") ? true : false; 
                LCARS_FullImpulse = (node.GetValue("LCARS_FullImpulse") == "True") ? true : false; 
                LCARS_UseReserves = (node.GetValue("LCARS_UseReserves") == "True") ? true : false; 
                
                LCARS_AccelerationLock = (node.GetValue("LCARS_AccelerationLock") == "True") ? true : false; 
                LCARS_ProgradeStabilizer = (node.GetValue("LCARS_ProgradeStabilizer") == "True") ? true : false; 
                LCARS_SlowDown = (node.GetValue("LCARS_SlowDown") == "True") ? true : false; 
                LCARS_InertiaDamper = (node.GetValue("LCARS_InertiaDamper") == "True") ? true : false; 
                LCARS_HoldSpeed = (node.GetValue("LCARS_HoldSpeed") == "True") ? true : false; 
                LCARS_HoldSpeed_value = LCARS_Utilities.ToFloat(node.GetValue("LCARS_HoldSpeed_value")); 
                LCARS_HoldHeight = (node.GetValue("LCARS_HoldHeight") == "True") ? true : false; 
                LCARS_HoldHeight_value = LCARS_Utilities.ToFloat(node.GetValue("LCARS_HoldHeight_value"));


                LCARS_Engineering_windowPosition = ConvertToRect(node.GetValue("LCARS_Engineering_windowPosition"));
                LCARS_Helm_windowPosition = ConvertToRect(node.GetValue("LCARS_Helm_windowPosition"));
                LCARS_Bridge_windowPosition = ConvertToRect(node.GetValue("LCARS_Bridge_windowPosition"));
                LCARS_Science_windowPosition = ConvertToRect(node.GetValue("LCARS_Science_windowPosition"));
                LCARS_Communication_windowPosition = ConvertToRect(node.GetValue("LCARS_Communication_windowPosition"));
                LCARS_Tactical_windowPosition = ConvertToRect(node.GetValue("LCARS_Tactical_windowPosition"));

                LCARS_Engineering_windowState = (node.GetValue("LCARS_Engineering_windowState") == "True") ? true : false;
                LCARS_Helm_windowState = (node.GetValue("LCARS_Helm_windowState") == "True") ? true : false;
                LCARS_Bridge_windowState = (node.GetValue("LCARS_Bridge_windowState") == "True") ? true : false;
                LCARS_Science_windowState = (node.GetValue("LCARS_Science_windowState") == "True") ? true : false;
                LCARS_Communication_windowState = (node.GetValue("LCARS_Communication_windowState") == "True") ? true : false;
                LCARS_Tactical_windowState = (node.GetValue("LCARS_Tactical_windowState") == "True") ? true : false;
                //UnityEngine.Debug.Log("### LCARSMarkII OnLoad done node=" + node);

                /*
                part.activate = ;
                part.ActivatesEvenIfDisconnected = ;
                part.active = ;
                part.drawStats = ;
                part.enabled = ;
                part.Fields = ;
                part.force_activate = ;
                part.LoadModule = ;
                part.partInfo = ;
                part.partInfo.moduleInfo = ;
                part.partInfo.moduleInfos = ;
                part.partInfo.partPrefab = ;
                part.partInfo.partPrefab.Fields = ;
            */
            }
            catch (Exception ex) { UnityEngine.Debug.Log("### LCARSMarkII OnLoad failed ex=" + ex); UnityEngine.Debug.Log("### LCARSMarkII OnLoad failed node=" + node); }
            /*
            try
            {
                //LCARS.lODN.CommunicationQueueDeserialize(node.GetValue("CommunicationQueue"));
            }
            catch (Exception ex) { onLoadNode = node; UnityEngine.Debug.Log("### LCARSMarkII OnLoad CommunicationQueueDeserialize failed ex=" + ex); UnityEngine.Debug.Log("### LCARSMarkII OnLoad failed node=" + node); }
            */
            try
            {
                //LCARS.lODN.ShipSystemsDeserialize(node.GetValue("ShipSystems"));
                LCARS_ShipSystems_data = "";
                LCARS_ShipSystems_data = node.GetValue("LCARS_ShipSystems_data");
                string[] tmp1 = LCARS_ShipSystems_data.Split(new Char[] { '|' });
                foreach (string tmp2 in tmp1)
                {
                    if (tmp2 == "" || tmp2 == null) { continue; }
                    string[] tmp3 = tmp2.Split(new Char[] { ',' });
                    LCARS.lODN.ShipSystems[tmp3[0]].disabled = (tmp3[1] == "True") ? true : false;
                    LCARS.lODN.ShipSystems[tmp3[0]].damaged = (tmp3[2] == "True") ? true : false;
                    LCARS.lODN.ShipSystems[tmp3[0]].integrity = LCARS_Utilities.ToFloat(tmp3[3]);
                    LCARS.lODN.ShipSystems[tmp3[0]].powerSystem_consumption_total = LCARS_Utilities.ToFloat(tmp3[4]);
                }
            }
            catch (Exception ex) { onLoadNode = node; UnityEngine.Debug.Log("### LCARSMarkII OnLoad LCARS_ShipSystems_data failed ex=" + ex); }
            base.OnLoad(node);
        }
        public Rect ConvertToRect(string value)
        {
            string[] args = ConvertToArgs(value);
            return new Rect(ToFloat(args[0]), ToFloat(args[1]), ToFloat(args[2]), ToFloat(args[3]));
        }
        public string[] ConvertToArgs(string value)
        {
            string[] args_tmp = value.Split(',');
            string[] args = new string[4];
            for (int i = 0; i < args_tmp.Length; i++)
            {
                string[] args2 = args_tmp[i].Split(':');
                //UnityEngine.Debug.Log("### LCARSMarkII ConvertToArgs args_tmp[i]=" + args_tmp[i] + " - args2[1]=" + args2[1]);
                args[i] = args2[1].Trim();
            }
            return args;
        }
        public static float ToFloat(string s)
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools ToFloat");
            float number;
            //bool result = float.TryParse(s, out number);
            bool result = float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            if (result)
            {
                //Console.WriteLine("Converted '{0}' to {1}.", s, number);
                return number;
            }
            else
            {
                //if (s == null) s = "";
                //Console.WriteLine("Attempted conversion of '{0}' failed.", s);
                return 0;
            }
        }
        public override void OnSave(ConfigNode node)
        {
            LCARS = this;
            if (HighLogic.LoadedSceneIsEditor || this.vessel == null || LCARS.lODN == null)
            {
                return;
            }
            UnityEngine.Debug.Log("### LCARSMarkII OnSave");
            try
            {
                node.SetValue("LCARS_FullHalt", LCARS.lODN.ShipStatus.LCARS_FullHalt.ToString());

                node.SetValue("LCARS_MakeSlowToSave", LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave.ToString());
                node.SetValue("LCARS_FullImpulse", LCARS.lODN.ShipStatus.LCARS_FullImpulse.ToString());
                node.SetValue("LCARS_UseReserves", LCARS.lODN.ShipStatus.LCARS_UseReserves.ToString());


                node.SetValue("LCARS_AccelerationLock", LCARS.lODN.ShipStatus.LCARS_AccelerationLock.ToString());
                node.SetValue("LCARS_ProgradeStabilizer", LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer.ToString());
                node.SetValue("LCARS_SlowDown", LCARS.lODN.ShipStatus.LCARS_SlowDown.ToString());
                node.SetValue("LCARS_InertiaDamper", LCARS.lODN.ShipStatus.LCARS_InertiaDamper.ToString());
                node.SetValue("LCARS_HoldSpeed", LCARS.lODN.ShipStatus.LCARS_HoldSpeed.ToString());
                node.SetValue("LCARS_HoldSpeed_value", LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value.ToString());
                node.SetValue("LCARS_HoldHeight", LCARS.lODN.ShipStatus.LCARS_HoldHeight.ToString());
                node.SetValue("LCARS_HoldHeight_value", LCARS.lODN.ShipStatus.LCARS_HoldHeight_value.ToString());

                
                node.SetValue("LCARS_Engineering_windowPosition", LCARS.lWindows.LCARSWindows["Engineering"].position.ToString());
                node.SetValue("LCARS_Helm_windowPosition", LCARS.lWindows.LCARSWindows["Helm"].position.ToString());
                node.SetValue("LCARS_Bridge_windowPosition", LCARS.lWindows.LCARSWindows["Bridge"].position.ToString());
                node.SetValue("LCARS_Science_windowPosition", LCARS.lWindows.LCARSWindows["Science"].position.ToString());
                node.SetValue("LCARS_Communication_windowPosition", LCARS.lWindows.LCARSWindows["Communication"].position.ToString());
                node.SetValue("LCARS_Tactical_windowPosition", LCARS.lWindows.LCARSWindows["Tactical"].position.ToString());

                node.SetValue("LCARS_Engineering_windowState", LCARS.lWindows.LCARSWindows["Engineering"].state.ToString());
                node.SetValue("LCARS_Helm_windowState", LCARS.lWindows.LCARSWindows["Helm"].state.ToString());
                node.SetValue("LCARS_Bridge_windowState", LCARS.lWindows.LCARSWindows["Bridge"].state.ToString());
                node.SetValue("LCARS_Science_windowState", LCARS.lWindows.LCARSWindows["Science"].state.ToString());
                node.SetValue("LCARS_Communication_windowState", LCARS.lWindows.LCARSWindows["Communication"].state.ToString());
                node.SetValue("LCARS_Tactical_windowState", LCARS.lWindows.LCARSWindows["Tactical"].state.ToString());
                //UnityEngine.Debug.Log("### LCARSMarkII OnSave done node=" + node);

                //node.SetValue("ShipStatus", LCARS.lODN.ShipStatusSerialize());
                //node.SetValue("ShipSystems", LCARS.lODN.ShipSystemsSerialize());

                /*
                */
            }
            catch (Exception ex) { UnityEngine.Debug.Log("### LCARSMarkII OnSave failed ex=" + ex); UnityEngine.Debug.Log("### LCARSMarkII OnSave failed node=" + node); }

            /*
            try
            {
                //node.SetValue("CommunicationQueue", LCARS.lODN.CommunicationQueueSerialize()); 
            }
            catch (Exception ex) { UnityEngine.Debug.Log("### LCARSMarkII OnSave CommunicationQueueSerialize failed ex=" + ex); UnityEngine.Debug.Log("### LCARSMarkII OnSave failed node=" + node); }
*/

            try
            {
                LCARS_ShipSystems_data = "";
                foreach(KeyValuePair<string,LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
                {
                    LCARS_ShipSystem_Type s = pair.Value;
                    string tmp = pair.Key + "," + s.disabled + "," + s.damaged + "," + s.integrity + "," + s.powerSystem_consumption_total + "|";
                    LCARS_ShipSystems_data = LCARS_ShipSystems_data + tmp;
                }
                node.SetValue("LCARS_ShipSystems_data", LCARS_ShipSystems_data);
            }
            catch (Exception ex) { UnityEngine.Debug.Log("### LCARSMarkII OnSave LCARS_ShipSystems_data failed ex=" + ex); UnityEngine.Debug.Log("### LCARSMarkII OnSave LCARS_ShipSystems_data failed node=" + node); }

            base.OnSave(node);
        }
        public override void OnStart(StartState state)
        {
            LCARS = this;
            UnityEngine.Debug.Log("### LCARSMarkII OnStart");

            if (!isReady())
            { return;  }

            WindowState = true;

            GameEvents.onVesselChange.Add(onVesselChange);


            base.OnStart(state);
        }
        public void SetVessel(Vessel vessel)
        {
            UnityEngine.Debug.Log("### LCARSMarkII SetVessel");


            thisVessel = vessel;
            //LCARS = this;
            lWindows.SetVessel(thisVessel);
            lToolbar.SetVessel(thisVessel);
            lFlightControll.SetVessel(thisVessel);
            lSubSys.SetVessel(thisVessel);
            lStationBridge.SetVessel(thisVessel);
            lStationHelm.SetVessel(thisVessel);
            lStationEng.SetVessel(thisVessel);
            lStationComm.SetVessel(thisVessel);
            lStationSci.SetVessel(thisVessel);
            lStationTac.SetVessel(thisVessel);
        }
        public void onVesselChange(Vessel newVessel)
        {
            if (newVessel.vesselType == VesselType.EVA)
            {

            }
            else 
            {
                SetVessel(newVessel);
            }
        }


        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 0.5f;
        public bool ArtefactInventory_from_SFS_Backup_done = false;
        public void initObjects()
        {



            //UnityEngine.Debug.Log("### LCARSMarkII initObjects");
            LCARS = this;


            if (lODN == null)
            {
                lODN = new LCARS_ODN_Util();
                lODN.SetVessel(thisVessel);
                lODN.initSystems();
                ScreenMessages.PostScreenMessage(
                   "<color=#ff9900ff>LCARS: Initializing ODN Network.</color>",
                  10f, ScreenMessageStyle.UPPER_CENTER
                );
                LCARS.lODN.ShipStatus.numpadcontroll_thrust = 20;
                LCARS.lODN.ShipStatus.numpadcontroll_enabled = false;
            }
            if (lPowSys == null)
            {
                lPowSys = new LCARS_PowerSystem_Util();
                lPowSys.SetVessel(thisVessel);
            }
            if (lWindows == null)
            {
                lWindows = new LCARS_Windows_Util();
                lWindows.SetVessel(thisVessel);
                lWindows.initWindows();
                setWindowState(true);

                try
                {
                    if ( LCARS_Engineering_windowPosition.x < 0.01f)
                    {
                        LCARS_Bridge_windowPosition =  new Rect(20, 20, 485, 50);
                        LCARS_Helm_windowPosition = new Rect(20, 20, 320, 443);
                        LCARS_Engineering_windowPosition = new Rect(20, 20, 485, 50);
                        LCARS_Science_windowPosition = new Rect(20, 20, 485, 50);
                        LCARS_Communication_windowPosition = new Rect(20, 20, 459, 600);
                        LCARS_Tactical_windowPosition = new Rect(20, 20, 485, 301);
                        LCARS_Bridge_windowState = true;
                        LCARS_Helm_windowState = false;
                        LCARS_Engineering_windowState = false;
                        LCARS_Science_windowState = false;
                        LCARS_Communication_windowState = false;
                        LCARS_Tactical_windowState = false;
                    }

                    LCARS.lWindows.LCARSWindows["Engineering"].position = LCARS_Engineering_windowPosition;
                    LCARS.lWindows.LCARSWindows["Helm"].position = LCARS_Helm_windowPosition;
                    LCARS.lWindows.LCARSWindows["Bridge"].position = LCARS_Bridge_windowPosition;
                    LCARS.lWindows.LCARSWindows["Science"].position = LCARS_Science_windowPosition;
                    LCARS.lWindows.LCARSWindows["Communication"].position = LCARS_Communication_windowPosition;
                    LCARS.lWindows.LCARSWindows["Tactical"].position = LCARS_Tactical_windowPosition;

                    LCARS.lWindows.setWindowState("Engineering", LCARS_Engineering_windowState);
                    LCARS.lWindows.setWindowState("Helm", LCARS_Helm_windowState);
                    LCARS.lWindows.setWindowState("Bridge", LCARS_Bridge_windowState);
                    LCARS.lWindows.setWindowState("Science", LCARS_Science_windowState);
                    LCARS.lWindows.setWindowState("Communication", LCARS_Communication_windowState);
                    LCARS.lWindows.setWindowState("Tactical", LCARS_Tactical_windowState);
                
                
                }
                catch { }
            }
            if (lSubSys == null)
            {
                lSubSys = new LCARS_Subsystem_Util();
                lSubSys.SetVessel(thisVessel);
            }
            if (grav == null)
            {
                grav = new LCARS_GravityTools();
                //LCARS_Util_Toolbar.Instance.SetVessel(thisVessel);
            }
            if (lToolbar == null)
            {
                lToolbar = FindObjectOfType<LCARS_Util_Toolbar>();// new LCARS_Util_Toolbar();
                lToolbar.SetVessel(thisVessel);
            }
            if (lFlightControll == null)
            {
                lFlightControll = LCARS_Util_FlightControll.Instance;
                lFlightControll.SetVessel(thisVessel);
            }
            if(lStationBridge == null)
            {
                lStationBridge = new LCARS_Bridge_Station();
                lStationBridge.SetVessel(thisVessel);
            }
            if (lStationHelm == null)
            {
                lStationHelm = new LCARS_Helm_Station();
                lStationHelm.SetVessel(thisVessel);
            }
            if (lStationEng == null)
            {
                lStationEng = new LCARS_Engineering_Station();
                lStationEng.SetVessel(thisVessel);
            }
            if (lStationComm == null)
            {
                lStationComm = new LCARS_Communication_Station();
                lStationComm.SetVessel(thisVessel);
            }
            if (lStationSci == null)
            {
                lStationSci = new LCARS_Science_Station();
                lStationSci.SetVessel(thisVessel);
            }
            if (lStationTac == null)
            {
                lStationTac = new LCARS_Tactical_Station();
                lStationTac.SetVessel(thisVessel);
            }
            if (lRepairTeams == null)
            {
                lRepairTeams = new LCARS_RepairTeam_Crew();
                lRepairTeams.SetVessel(thisVessel);
            }
            if (lComm == null)
            {
                lComm = new LCARS_Communication_Util();
                lComm.SetVessel(thisVessel);
            }
            if (lCQ == null)
            {
                lCQ = new LCARS_CrewQuartier();
                lCQ.setVessel(thisVessel);
                lCQ.addCrewSpace();
                lCQ.addHatch();
            }
            if (lAudio == null)
            {
                lAudio = new LCARS_AudioLibrary();
                lAudio.init(lODN.ShipStatus.LCARS_Plugin_Path + "sounds/");
            }




            if (onLoadNode != null && LCARS.lODN.ShipSystems!=null)
            {
                UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate ShipSystems from sfs begin ");
                LCARS_ShipSystems_data = "";
                LCARS_ShipSystems_data = onLoadNode.GetValue("LCARS_ShipSystems_data");
                string[] tmp1 = LCARS_ShipSystems_data.Split(new Char[] { '|' });
                foreach (string tmp2 in tmp1)
                {
                    if (tmp2 == "" || tmp2 == null) { continue; }
                    string[] tmp3 = tmp2.Split(new Char[] { ',' });
                    try
                    {
                                LCARS.lODN.ShipSystems[tmp3[0]].disabled = (tmp3[1] == "True") ? true : false;
                                LCARS.lODN.ShipSystems[tmp3[0]].damaged = (tmp3[2] == "True") ? true : false;
                                LCARS.lODN.ShipSystems[tmp3[0]].integrity = LCARS_Utilities.ToFloat(tmp3[3]);
                                LCARS.lODN.ShipSystems[tmp3[0]].powerSystem_consumption_total = LCARS_Utilities.ToFloat(tmp3[4]);
                    }
                    catch 
                    { 
                        UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate ShipSystems failed for key <"+tmp3[0]+"> ");
                    }
                }
                onLoadNode = null;
                UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate ShipSystems from sfs done");
            }

            try
            {
                if (LCARS.lODN.ArtefactInventory != null && LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup != null && !ArtefactInventory_from_SFS_Backup_done)
                {
                    UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate ArtefactInventory from sfs begin thisVessel.id=" + thisVessel.id + " ArtefactInventory_String_SFS_Backup=" + LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup);
                    LCARSNCI_Bridge.Instance.setShipInventoryFromPersistentFile(LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup, thisVessel);
                    //LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup = null;
                }
                else
                {
                    //UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate ArtefactInventory from sfs can't begin now ");
                }
                ArtefactInventory_from_SFS_Backup_done = true;
            }
            catch(Exception ex) { UnityEngine.Debug.Log("### LCARSMarkII initObjects LCARSNCI_Bridge.Instance.setShipInventoryFromPersistentFile failed ex="+ex); }

            try
            {
                if (LCARS.lODN.CommunicationQueue != null && LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup != null)
                {
                    UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate CommunicationQueue from sfs begin ");
                    LCARSNCI_Bridge.Instance.setCommunicationQueueFromPersistentFile(LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup);
                    LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup = null;
                    UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate CommunicationQueue from sfs end ");
                }
                else
                {
                    //UnityEngine.Debug.Log("### LCARSMarkII initObjects reinstate CommunicationQueue from sfs can't begin now ");
                }
            }
            catch { LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup = null; /*UnityEngine.Debug.Log("### LCARSMarkII initObjects LCARSNCI_Bridge.Instance.setCommunicationQueueFromPersistentFile failed ");*/ }



            // routine clean up
            if ((Time.time - lastFixedUpdate) > logInterval)
            {

                EngineConstant = (100 - ((thisVessel.LCARSVessel_WetMass() - LCARS.lODN.ImpulseEngineTypes[engine_type].max_weight) / (thisVessel.LCARSVessel_WetMass() / 100)));
                EngineConstant = (EngineConstant > 100) ? 100.0f : EngineConstant ;
                UnityEngine.Debug.Log("### LCARSMarkII logInterval EngineConstant=" + EngineConstant);

                // window sizes
                Rect tmp_position;
                lastFixedUpdate = Time.time;
                tmp_position = lWindows.LCARSWindows["Engineering"].position;
                tmp_position.height = 50f;
                lWindows.LCARSWindows["Engineering"].position = tmp_position;

                tmp_position = lWindows.LCARSWindows["Bridge"].position;
                tmp_position.height = 50f;
                lWindows.LCARSWindows["Bridge"].position = tmp_position;

                tmp_position = lWindows.LCARSWindows["Science"].position;
                tmp_position.height = 50f;
                lWindows.LCARSWindows["Science"].position = tmp_position;

                // current power usage
                foreach(KeyValuePair<string, LCARS_ShipSystem_Type> pair in lODN.ShipSystems)
                {
                    LCARS.lODN.ShipSystems[pair.Key].powerSystem_consumption_current = 0f;
                }



            }

            
        }

        public void OnGUI()
        {

            if (!isReady())
            { return; }
            /*
            */
            if (lWindows == null)
            {
                return;
            }
            lWindows.DrawWindows();
        }


        void FixedUpdate()
        {
            if (this.vessel != FlightGlobals.ActiveVessel)
            {
                return;
            }
            /*
            */
            if (thisVessel.isLCARSVessel())
            {
                initObjects();
            }
            else 
            {
                return;
            }

            /*
            */

            /*
             * Propulsion Code
             */
            if (LCARS.lODN.ShipStatus.LCARS_AccelerationLock && LCARS.lODN.ShipSystems["AccelerationLock"].isNominal && LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal)
            {
                LCARS_thrust_x = backup_LCARS_thrust_x;
                LCARS_thrust_y = backup_LCARS_thrust_y;
                LCARS_thrust_z = backup_LCARS_thrust_z;

            }

            if (LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal)
            {
                LCARS.lODN.ShipStatus.current_LCARS_thrust_x = LCARS_thrust_x;
                LCARS.lODN.ShipStatus.current_LCARS_thrust_y = LCARS_thrust_y;
                LCARS.lODN.ShipStatus.current_LCARS_thrust_z = LCARS_thrust_z;

                LCARS.lODN.ShipStatus.current_total_thrust = LCARS.lODN.ShipStatus.current_LCARS_thrust_x + LCARS.lODN.ShipStatus.current_LCARS_thrust_y + LCARS.lODN.ShipStatus.current_LCARS_thrust_z;
                LCARS.lODN.ShipStatus.current_total_charge = LCARS.lODN.ShipStatus.current_total_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L1_usage;

                if (LCARS.lODN.ShipStatus.LCARS_FullImpulse)
                {
                    if (LCARS.lODN.ShipSystems["FullImpulse"].isNominal && LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal)
                    {
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_x = LCARS.lODN.ShipStatus.current_LCARS_thrust_x * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier;
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_y = LCARS.lODN.ShipStatus.current_LCARS_thrust_y * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier;
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_z = LCARS.lODN.ShipStatus.current_LCARS_thrust_z * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier;
                        LCARS.lODN.ShipStatus.current_total_charge = LCARS.lODN.ShipStatus.current_total_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L2_usage;
                    }
                }
                else 
                {
                    LCARS.lODN.ShipStatus.LCARS_UseReserves = false;
                }
                if (LCARS.lODN.ShipStatus.LCARS_UseReserves)
                {
                    if (LCARS.lODN.ShipSystems["UseReserves"].isNominal && LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal)
                    {
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_x *= LCARS.lODN.ShipStatus.LCARS_UseReserves_modifier;
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_y *= LCARS.lODN.ShipStatus.LCARS_UseReserves_modifier;
                        LCARS.lODN.ShipStatus.current_LCARS_thrust_z *= LCARS.lODN.ShipStatus.LCARS_UseReserves_modifier;
                        LCARS.lODN.ShipStatus.current_total_charge = LCARS.lODN.ShipStatus.current_total_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage;
                    }
                }

                ////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////

                // perform frame based modifications on ships systems
                lODN.MonitorShipSystems();

                // perform repairs
                lRepairTeams.GetToWork();

                // main computer LCARS power usage
                lPowSys.draw(lODN.ShipSystems["LCARS-ODN"].name, lODN.ShipSystems["LCARS-ODN"].powerSystem_L1_usage);

                // scienceEquippment supplementary stuff
                if (LCARS.lODN.ShipSystems.ContainsKey("Iso Flux Detector"))
                {
                    if (LCARS.lODN.IFD_object_distance>0)
                    {
                        lPowSys.draw(lODN.ShipSystems["Iso Flux Detector"].name, LCARS.lODN.ShipSystems["Iso Flux Detector"].powerSystem_L2_usage);
                    }
                    if (LCARS.lODN.IFD_show_results_as_screenmessage && LCARS.lODN.ShipSystems["Iso Flux Detector"].isNominal)
                    {
                        ScreenMessages.PostScreenMessage(
                           "<color=#ff9900ff>(LCARS) IFD Result: " + LCARS.lODN.IFD_object_distance + " meter</color>",
                          Time.deltaTime, ScreenMessageStyle.UPPER_CENTER
                        );
                        lPowSys.draw(lODN.ShipSystems["Iso Flux Detector"].name, LCARS.lODN.ShipSystems["Iso Flux Detector"].powerSystem_L3_usage);
                    }
                }


                // main EPS/propulsion power usage
                float performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, LCARS.lODN.ShipStatus.current_total_charge * LCARS.lODN.ImpulseEngineTypes[LCARS.engine_type].ImpulseDefaultPowerUsageFactor);
                LCARS.lODN.ShipStatus.current_LCARS_thrust_performance = performance;

                // Structural Integrity Field power usage
                LCARS.lODN.ShipStatus.current_LCARS_SFI_performance = lPowSys.draw(lODN.ShipSystems["Structural Integrity Field"].name, (LCARS.lODN.ShipStatus.current_LCARS_SFI_force - 1) * LCARS.lODN.ShipSystems["Structural Integrity Field"].powerSystem_L2_usage);

                ////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////

                
    

                if (LCARS.lODN.ShipStatus.current_LCARS_thrust_x != 0f)
                {
                    grav.AddNewForce_left_right((LCARS.lODN.ShipStatus.current_LCARS_thrust_x * performance)/100*EngineConstant, thisVessel);
                }
                if (LCARS.lODN.ShipStatus.current_LCARS_thrust_y != 0f)
                {
                    grav.AddNewForce_ffwd_back((LCARS.lODN.ShipStatus.current_LCARS_thrust_y * performance) / 100 * EngineConstant, thisVessel);
                }
                if (LCARS.lODN.ShipStatus.current_LCARS_thrust_z != 0f)
                {
                    grav.AddNewForce_up_down((LCARS.lODN.ShipStatus.current_LCARS_thrust_z * performance)/100*EngineConstant, thisVessel);
                }


                if (LCARS.lODN.ShipStatus.numpadcontroll_enabled)
                {
                    float np_thrust = LCARS.lODN.ShipStatus.numpadcontroll_thrust;
                    np_thrust = (LCARS.lODN.ShipStatus.LCARS_FullImpulse) ? np_thrust * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier : np_thrust;
                    np_thrust = (LCARS.lODN.ShipStatus.LCARS_UseReserves) ? np_thrust * LCARS.lODN.ShipStatus.LCARS_UseReserves_modifier : np_thrust;

                    if (Input.GetKey(KeyCode.Keypad8))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_ffwd_back((np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }
                    if (Input.GetKey(KeyCode.Keypad2))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_ffwd_back((-np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }

                    if (Input.GetKey(KeyCode.Keypad4))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_left_right((-np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }
                    if (Input.GetKey(KeyCode.Keypad6))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_left_right((np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }

                    if (Input.GetKey(KeyCode.Keypad9))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_up_down((np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }
                    if (Input.GetKey(KeyCode.Keypad3))
                    {
                        performance = lPowSys.draw(lODN.ShipSystems["PropulsionMatrix"].name, np_thrust * LCARS.lODN.ShipSystems["PropulsionMatrix"].powerSystem_L3_usage);
                        grav.AddNewForce_up_down((-np_thrust * performance) / 100 * EngineConstant, thisVessel);
                    }

                    if (Input.GetKey(KeyCode.Keypad5))
                    {
                        grav.FullHalt(thisVessel);
                    }

                    if (Input.GetKey(KeyCode.Keypad7))
                    {
                        LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer = !LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer;
                    }

                    if (Input.GetKey(KeyCode.Keypad1))
                    {
                        LCARS.lODN.ShipStatus.LCARS_HoldSpeed = !LCARS.lODN.ShipStatus.LCARS_HoldSpeed;
                    }
                    if (Input.GetKey(KeyCode.Keypad0))
                    {
                        LCARS.lODN.ShipStatus.LCARS_HoldHeight = !LCARS.lODN.ShipStatus.LCARS_HoldHeight;
                    }
                }
            }




            if (LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer && LCARS.lODN.ShipSystems["ProgradeStabilizer"].isNominal)
            {
                grav.PilotMode(thisVessel);
            }
            if (LCARS.lODN.ShipStatus.LCARS_SlowDown && LCARS.lODN.ShipSystems["FullHalt"].isNominal)
            {
                grav.FullHalt(thisVessel);
            }
            /*if (LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave && LCARS.lODN.ShipSystems["MakeSlowToSave"].isNominal)
            {
                // TO DO!!
            }*/
            if (LCARS.lODN.ShipStatus.LCARS_InertiaDamper && LCARS.lODN.ShipSystems["InertiaDamper"].isNominal)
            {
                grav.CancelMoI(thisVessel);
            }
            if (LCARS.lODN.ShipStatus.LCARS_FullHalt && LCARS.lODN.ShipSystems["FullHalt"].isNominal)
            {
                grav.FullHalt(thisVessel);
            }
            if (LCARS.lODN.ShipStatus.LCARS_HoldSpeed && LCARS.lODN.ShipSystems["HoldSpeed"].isNominal)
            {
                grav.HoldSpeed(thisVessel, LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value);
            }
            if (LCARS.lODN.ShipStatus.LCARS_HoldHeight && LCARS.lODN.ShipSystems["HoldHeight"].isNominal)
            {
                grav.HoldHeight(thisVessel, LCARS.lODN.ShipStatus.LCARS_HoldHeight_value);
            }
            /*
             * Propulsion Code
             */


            //UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate pre heat");
            if (LCARS.lODN.ShipStatus.LCARSVessel_EC_Generator==null)
            {
                LCARS.lODN.ShipStatus.LCARSVessel_EC_Generator = thisVessel.LCARSVessel_EC_Generator();
            }
            ModuleGenerator.GeneratorResource LCARSVessel_EC_Generator = LCARS.lODN.ShipStatus.LCARSVessel_EC_Generator;
            if (LCARSVessel_EC_Generator.rate > LCARS.lODN.ShipStatus.CoreOverHeating_PowerRate)
            {
                /*
                float heat_percentage = LCARSVessel_EC_Generator.rate / (LCARS.lODN.ShipStatus.MaxPowerGeneratorRate / 100);
                UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate heat_percentage=" + heat_percentage);
                float heat_percentage2 = LCARS.lODN.ShipStatus.CoreOverHeating_PowerRate / (LCARS.lODN.ShipStatus.MaxPowerGeneratorRate / 100);
                UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate heat_percentage2=" + heat_percentage2);
                LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat = (heat_percentage - heat_percentage2) / 100;
                */
                LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat = Mathf.Clamp((LCARSVessel_EC_Generator.rate - LCARS.lODN.ShipStatus.CoreOverHeating_PowerRate), 0, LCARSVessel_EC_Generator.rate) / (LCARS.lODN.ShipStatus.MaxPowerGeneratorRate / 100) / 2 / 100;
                //UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat=" + LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat);
                thisVessel.LCARSVessel_ImpulseDrive_Part().temperature += LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat;
                //UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate thisVessel.LCARS_ImpulseDrive_Part().temperature=" + thisVessel.LCARSVessel_ImpulseDrive_Part().temperature);
            }
            //UnityEngine.Debug.Log("### LCARSMarkII FixedUpdate post heat");
            /*
            */

        }



    }
}

