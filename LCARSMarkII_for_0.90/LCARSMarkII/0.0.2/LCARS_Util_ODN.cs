using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_ShipSystem_Type
    {
        public string name { get; set; }
        public string type { get; set; }
        public Vessel vessel { get; set; }
        public bool disabled { get; set; }

        public bool damaged { get; set; }
        public bool damagable { get; set; }
        public float integrity { get; set; }

        public string powerSystem_takerType { get; set; }
        public string powerSystem_takerSubType { get; set; }
        public float powerSystem_L1_usage { get; set; } // Level1: allways power draw
        public float powerSystem_L2_usage { get; set; } // Level2: on action power draw
        public float powerSystem_L3_usage { get; set; } // Level3: something else additionally
        public float powerSystem_consumption_current { get; set; }
        public float powerSystem_consumption_total { get; set; }

        public ILCARSPlugin plugin_instance { get; set; }
        public System.Type plugin_type { get; set; }
        
        
        public bool isNominal
        {
            get
            {
                return (disabled || damaged) ? false : true;
            }
        }
        public bool isDamaged
        {
            get
            {
                return damaged;
            }
        }
        public bool isDisabled
        {
            get
            {
                return disabled;
            }
        }
        public void inflictDamage(float damage)
        {
            integrity -= damage;
            integrity = (integrity<0) ? 0 : integrity;
            if (integrity < 1)
            {
                if (!damaged)
                {
                    LCARS_Message_Type mT = new LCARS_Message_Type();
                    mT.receiver_vessel = vessel;
                    mT.sender = "Engineering";
                    mT.sender_id_line = "Engineering reports:";
                    mT.receiver_type = "Station";
                    mT.receiver_code = "Bridge";
                    mT.priority = 3;
                    mT.setTitle("System went offline");
                    mT.setMessage("We have a damaged system: The " + name + " just blew up Sir!");
                    mT.Queue();

                    try
                    {
                        LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog(mT.title, "Engineering Reports: <" + mT.message + ">");
                    }
                    catch { }
                }
                vessel.LCARS().lRepairTeams.lastRepairedSystem = "";
                damaged = true;

            }
        }
        public void repairDamage(float repair)
        {
            integrity += repair;
            integrity = (integrity > 100) ? 100 : integrity;
            if (integrity == 100)
            {
                damaged = false;
            }
        }

        public bool show_in_MODNJ { get; set; }
    }
    public class LCARS_ShipStatus_Type
    {
        // defaults
        private string default_LCARS_Plugin_Path = "LCARSMarkII/";
        private float default_LCARSVessel_EC_Generator_default_rate = 670000f;
        private float default_LCARS_FullImpulse_modifier = 7f;
        private float default_LCARS_UseReserves_modifier = 3f;
        private float default_MainComputerDefaultPowerUsage = 1250f;
        private float default_MainImpulseDefaultPowerUsage = 100f;
        private float default_CoreOverHeating_PowerRate = 6700000f;
        private float default_MaxPowerGeneratorRate = 20000000f;
        // defaults
        




        public bool LCARS_FullImpulse { get; set; }
        public bool LCARS_UseReserves { get; set; }
        public float current_LCARS_thrust_x { get; set; }
        public float current_LCARS_thrust_y { get; set; }
        public float current_LCARS_thrust_z { get; set; }

        public string LCARS_Plugin_Path { get{ return default_LCARS_Plugin_Path; } }
        public bool LCARS_hasPower { get; set; }
        public bool disabled { get; set; }
        public float LCARS_VesselDryMass { get; set; }
        public float LCARS_VesselWetMass { get; set; }
        public float LCARS_VesselResourceMass { get; set; }
        public float AveragePartTemperature { get; set; }
        public float AveragePartTemperatureMax { get; set; }
        public float LCARS_Heat_percentage { get; set; }

        public Dictionary<string, PartModule> LCARS_VesselPartModules { get; set; }
        public Dictionary<int, ModuleGenerator> LCARS_VesselResourceGenerators { get; set; }
        public ModuleGenerator.GeneratorResource LCARSVessel_EC_Generator { get; set; }
        public float LCARSVessel_EC_Generator_default_rate { get { return default_LCARSVessel_EC_Generator_default_rate; } }
        

        public bool LCARS_FullHalt { get; set; }
        public bool LCARS_MakeSlowToSave { get; set; }
        public bool LCARS_AccelerationLock { get; set; }
        public bool LCARS_ProgradeStabilizer { get; set; }
        public bool LCARS_SlowDown { get; set; }
        public bool LCARS_InertiaDamper { get; set; }
        public bool LCARS_HoldSpeed { get; set; }
        public double LCARS_HoldSpeed_value { get; set; }
        public bool LCARS_HoldHeight { get; set; }
        public double LCARS_HoldHeight_value { get; set; }
        public float LCARS_FullImpulse_modifier { get { return default_LCARS_FullImpulse_modifier; } }
        public float LCARS_UseReserves_modifier { get { return default_LCARS_UseReserves_modifier; } }
        public float current_total_charge { get; set; }
        public float current_total_thrust { get; set; }


        //public Dictionary<string, LCARS_PowerTaker_Type> LCARS_PowerTakers  { get; set; }
        internal float MainComputerDefaultPowerUsage { get { return default_MainComputerDefaultPowerUsage; } }
        internal float MainImpulseDefaultPowerUsage { get { return default_MainImpulseDefaultPowerUsage; } }
        public float CoreOverHeating_PowerRate { get { return default_CoreOverHeating_PowerRate; } }
        public float MaxPowerGeneratorRate { get { return default_MaxPowerGeneratorRate; } }
        public float LCARS_ImpulseDrive_Part_add_heat { get; set; }

        public List<string> NCI_missionOptions_DeactivatedSystems { get; set; }
        public List<string> NCI_missionOptions_DamagedSystems { get; set; }


        public float numpadcontroll_thrust { get; set; }

        public float current_LCARS_thrust_performance { get; set; }

        public float current_LCARS_SFI_performance { get; set; }

        public float current_LCARS_SFI_force { get; set; }

        public Rect TransporterSystem_ShipToEVA_screen_rect { get; set; }

        public bool numpadcontroll_enabled { get; set; }
    }
    public class LCARS_ArtefactInventory_Type
    {
        public string idcode { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public string icon { set; get; }
        public string isDamagable { set; get; }
        public string integrity { set; get; }
        public string usage_amount { set; get; }
        public string usage_times { set; get; }
        public string powerconsumption { set; get; }

    }

    public class LCARS_ScanReport_Type
    {
        public List<LCARS_ScanReportItem_Type> list { set; get; }

        // find specific vessel-object
        public List<string> retrieve_actionpoints_from_NCIObject(Guid guid)
        {
            List<string> return_List = new List<string>();
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                if (R.guid == guid)
                {
                    if (R.NCIactionpoints!=null)
                    {
                        return_List = R.NCIactionpoints;
                    }
                }
            }
            return return_List;
        }
        // find specific vessel-object
        public bool verify_required_Scan_distance_for_NCI(Guid guid, string idcode, float distance, string scan_level)
        {
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                if (R.guid == guid && R.idcode == idcode && R.distance <= distance && R.scan_level == scan_level)
                {
                    return true;
                }
            }
            return false;
        }
        // find specific vessel-object
        public bool verify_required_Scan_distance_for_NCI(Guid guid, string idcode, string scan_level)
        {
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                if (R.guid == guid && R.idcode == idcode && R.scan_level == scan_level)
                {
                    return true;
                }
            }
            return false;
        }
        // find specific vessel-object
        public bool verify_required_Scan_distance_for_NCI(Guid guid, string idcode, float distance)
        {
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                UnityEngine.Debug.Log("### LCARS_ScanReport_Type verify_required_Scan_distance_for_NCI R.guid" + R.guid + " R.idcode=" + R.idcode + " R.distance=" + R.distance + " checkCondition scan testing");
                if (R.guid == guid && R.idcode == idcode && R.distance <= distance)
                {
                    UnityEngine.Debug.Log("### LCARS_ScanReport_Type verify_required_Scan_distance_for_NCI distance" + distance + " R.distance=" + R.distance + " R.idcode=" + R.idcode + " checkCondition scan true");
                    return true;
                }
            }
            UnityEngine.Debug.Log("### LCARS_ScanReport_Type verify_required_Scan_distance_for_NCI distance" + distance +" idcode=" + idcode + " checkCondition scan false ");
            return false;
        }
        // find specific scan with distance
        public bool verify_required_Scan_distance_for_NCI(string target_type, string scan_level, string idcode, float distance)
        {
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                if (R.target_type == target_type && R.scan_level == scan_level && R.idcode == idcode && R.distance <= distance)
                {
                    return true;
                }
            }
            return false;
        }
        // find specific scan without distance
        public bool verify_required_Scan_distance_for_NCI(string target_type, string scan_level, string idcode)
        {
            foreach (LCARS_ScanReportItem_Type R in this.list)
            {
                if (R.target_type == target_type && R.scan_level == scan_level && R.idcode == idcode)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class LCARS_EngineType
    {
        public string id { get; set; }

        public string name { get; set; }

        public float max_weight { get; set; }

        public float ImpulseDefaultPowerUsageFactor { get; set; }
    }

    public class LCARS_ScanReportItem_Type
    {
        public string target_type { set; get; }
        public string idcode { set; get; }
        public Guid guid { set; get; }
        public string time { set; get; }

        public string scan_level { set; get; }
        public string description { set; get; }
        public float distance { set; get; }

        public string body_name { set; get; }
        public string body_biom_map { set; get; }
        public string body_surface_map { set; get; }

        public GameObject gameobject { set; get; }

        public List<string> NCIactionpoints { set; get; }

    }
    public class LCARS_ODN_Util
    {
        public LCARS_ShipStatus_Type ShipStatus = null;
        public LCARS_ScanReport_Type ScanReport = null;
        public Dictionary<string, LCARS_ShipSystem_Type> ShipSystems = null;
        public List<LCARS_CommunicationQueue_Type> CommunicationQueue = null;
        public Dictionary<string, LCARS_ArtefactInventory_Type> ArtefactInventory = null;

        public GameObject IFD_object_of_interest = null;
        public float IFD_object_distance { get; set; }
        public bool IFD_show_results_as_screenmessage { get; set; }
        public bool IFD_show_results_as_consolemessage { get; set; }
        public LCARS_ArtefactInventory_Type NucleonicAnalyzer_artefact_of_interest = null;
        public LCARS_ArtefactInventory_Type PteroplasticScrambler_artefact_of_interest = null;
        public Dictionary<string, LCARS_EngineType> ImpulseEngineTypes = null;
        

        Vessel thisVessel;
        public LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {

            thisVessel = vessel;
            LCARS = thisVessel.LCARS();

                        
        }
        public void initSystems()
        {

            if (ShipStatus == null)
            {
                ShipStatus = new LCARS_ShipStatus_Type();
            }
            if (ScanReport == null)
            {
                ScanReport = new LCARS_ScanReport_Type();
                ScanReport.list = new List<LCARS_ScanReportItem_Type>();
            }
            if (ShipSystems == null)
            {
                ShipSystems = new Dictionary<string, LCARS_ShipSystem_Type>();
            }
            if (CommunicationQueue == null)
            {
                CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();
            }
            if (ArtefactInventory == null)
            {
                ArtefactInventory = new Dictionary<string, LCARS_ArtefactInventory_Type>();
            }
            if (ImpulseEngineTypes == null)
            {
                ImpulseEngineTypes = new Dictionary<string, LCARS_EngineType>();
                LCARS_EngineType eT = null;
                eT = new LCARS_EngineType();
                eT.id = "1";
                eT.name = "Impulse Core Mark I";
                eT.max_weight = 20f;
                eT.ImpulseDefaultPowerUsageFactor = 0.8f;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "2";
                eT.name = "Impulse Core Mark II";
                eT.max_weight = 60f;
                eT.ImpulseDefaultPowerUsageFactor = 0.9f;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "3";
                eT.name = "Impulse Core Mark III";
                eT.max_weight = 120f;
                eT.ImpulseDefaultPowerUsageFactor = 1.0f;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "4";
                eT.name = "Impulse Core Mark IV";
                eT.max_weight = 250f;
                eT.ImpulseDefaultPowerUsageFactor = 1.1F;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "5";
                eT.name = "Impulse Core Mark V";
                eT.max_weight = 520f;
                eT.ImpulseDefaultPowerUsageFactor = 1.2F;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "6";
                eT.name = "Impulse Core Mark VI";
                eT.max_weight = 1040f;
                eT.ImpulseDefaultPowerUsageFactor = 1.3F;
                ImpulseEngineTypes.Add(eT.id, eT);

                eT = null;
                eT = new LCARS_EngineType();
                eT.id = "7";
                eT.name = "Impulse Core Mark VII";
                eT.max_weight = 1500f;
                eT.ImpulseDefaultPowerUsageFactor = 1.4F;
                ImpulseEngineTypes.Add(eT.id, eT);

            }

            /*
            // test
            LCARS_ArtefactInventory_Type foo = new LCARS_ArtefactInventory_Type();
            foo.idcode = "DumboTheGreat";
            foo.name = "DumboTheGreat";
            foo.description = "It's a flying elephant";
            ArtefactInventory.Add("DumboTheGreat", foo);
            // test
            */
            List<string> sys_list_Bridge = new List<string>();
                sys_list_Bridge.Add("LCARS-ODN");
                sys_list_Bridge.Add("FormationFlight");
                foreach (string key in sys_list_Bridge)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "BridgeSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;

                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Bridge";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;
                    
                        ShipSystems.Add(key, ShipSystem);
                    }
                }
                ShipSystems["LCARS-ODN"].show_in_MODNJ = false;

            List<string> sys_list_Helm = new List<string>();
                sys_list_Helm.Add("AccelerationLock");
                sys_list_Helm.Add("ProgradeStabilizer");
                sys_list_Helm.Add("InertiaDamper");
                sys_list_Helm.Add("HoldSpeed");
                sys_list_Helm.Add("HoldHeight");
                sys_list_Helm.Add("SlowDown");
                sys_list_Helm.Add("FullHalt");
                sys_list_Helm.Add("MakeSlowToSave");
                foreach (string key in sys_list_Helm)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "HelmSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Helm";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;
                    
                        ShipSystems.Add(key, ShipSystem);
                    }
                }
                ShipSystems["MakeSlowToSave"].disabled = true;
                ShipSystems["MakeSlowToSave"].show_in_MODNJ = false;

            List<string> sys_list_Engineering = new List<string>();
                sys_list_Engineering.Add("PropulsionMatrix");
                sys_list_Engineering.Add("HoverForce");
                sys_list_Engineering.Add("FullImpulse");
                sys_list_Engineering.Add("UseReserves");
                foreach (string key in sys_list_Engineering)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "EngineeringSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Engineering";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;

                        ShipSystems.Add(key, ShipSystem);
                    }
                }
                ShipSystems["PropulsionMatrix"].powerSystem_L1_usage = LCARS.lODN.ShipStatus.MainImpulseDefaultPowerUsage;
                ShipSystems["PropulsionMatrix"].powerSystem_L2_usage = LCARS.lODN.ShipStatus.MainImpulseDefaultPowerUsage * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier * 2;
                ShipSystems["PropulsionMatrix"].powerSystem_L3_usage = LCARS.lODN.ShipStatus.MainImpulseDefaultPowerUsage * LCARS.lODN.ShipStatus.LCARS_FullImpulse_modifier * LCARS.lODN.ShipStatus.LCARS_UseReserves_modifier * 3;


                List<string> sys_list_Tactical = new List<string>();
                //sys_list_Tactical.Add("UniversalTranslator");
                foreach (string key in sys_list_Tactical)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "TacticalSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Tactical";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;

                        ShipSystems.Add(key, ShipSystem);
                    }
                }


                List<string> sys_list_Science = new List<string>();
                //sys_list_Science.Add("UniversalTranslator");
                foreach (string key in sys_list_Science)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "ScienceSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Science";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;
                        
                        ShipSystems.Add(key, ShipSystem);
                    }
                }

            List<string> sys_list_Communication = new List<string>();
                sys_list_Communication.Add("UniversalTranslator");
                foreach (string key in sys_list_Communication)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "CommunicationSystems";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "MainSystem";
                        ShipSystem.powerSystem_takerSubType = "Communication";
                        ShipSystem.powerSystem_L1_usage = 10f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;
                        
                        ShipSystems.Add(key, ShipSystem);
                    }
                }
                ShipSystems["UniversalTranslator"].powerSystem_L2_usage = 80f;

            List<string> sys_list_RepairTeam = new List<string>();
                sys_list_RepairTeam.Add("RepairTeam_Rockney");
                sys_list_RepairTeam.Add("RepairTeam_Scott");
                sys_list_RepairTeam.Add("RepairTeam_LaForge");

                foreach (string key in sys_list_RepairTeam)
                {
                    if (!ShipSystems.ContainsKey(key))
                    {
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = key;
                        ShipSystem.type = "RepairTeams";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = false;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "Crew";
                        ShipSystem.powerSystem_takerSubType = "Engineering";
                        ShipSystem.powerSystem_L1_usage = 100f;
                        ShipSystem.powerSystem_L2_usage = 0f;
                        ShipSystem.powerSystem_L3_usage = 0f;
                        
                        ShipSystems.Add(key, ShipSystem);
                    }
                }

        }

        double current_geeForce = 0;
        double previous_geeForce = 1;
        internal void MonitorShipSystems()
        {

            /*
             Apply Damage on too high G
             */
            int random_value = 0;
            current_geeForce = thisVessel.geeForce;
            double diff = current_geeForce - previous_geeForce;
            diff *= (diff > 0) ? 1 : -1;
            if (diff > 10)
            {
                int i = 0;
                foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
                {
                    random_value = new System.Random().Next(1, 20);

                    if (random_value<18)
                    { continue; }
                    
                    if (pair.Value.isDamaged)
                    { continue; }

                    try
                    {
                        if (pair.Value.plugin_instance!=null)
                        {
                            if (!pair.Value.plugin_instance.subsystemIsDamagable)
                            { continue; }
                        }
                    }
                    catch (Exception ex) { }

                    i++;
                    if (i > diff)
                    {
                        pair.Value.inflictDamage((float)(diff * 0.05));
                        i = 0;
                    }
                }
            }
            previous_geeForce = current_geeForce;
            /*
             Apply Damage on too high G
             */

            /*
             Disable a Systems if NCI want's it
             */
            if (LCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems != null)
            {
                foreach (string sysName in LCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems)
                {
                    if (LCARS.lODN.ShipSystems.ContainsKey(sysName))
                    {
                        LCARS.lODN.ShipSystems[sysName].disabled = true;
                    }
                }
                LCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Clear();
            }
            /*
             Keep Systems Disabled if NCI want's it
             */


            /*
             Damage a System (once) if NCI want's it
             */
            if (LCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems!=null)
            {
                foreach (string sysName in LCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems)
                {
                    if (LCARS.lODN.ShipSystems.ContainsKey(sysName))
                    {
                        LCARS.lODN.ShipSystems[sysName].inflictDamage(100f);
                    }
                }
                LCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems.Clear();
            }
            /*
             Damage a Systems if NCI want's it
             */


        }


        /* DATA: serialization and compression * /
        public string CommunicationQueueSerialize()
        {
            //byte[] bytes = LCARS_ShipStatus_TypeToByteArray(LCARS.lODN.ShipStatus);
            MemoryStream mem = new MemoryStream();
            IFormatter binf = new BinaryFormatter();
            binf.Serialize(mem, LCARS.lODN.CommunicationQueue);
            string blob = Convert.ToBase64String(CLZF2.Compress(mem.ToArray()));
            return blob.Replace("/", "-").Replace("=", "_");

        }
        public void CommunicationQueueDeserialize(string blob)
        {
            try
            {
                blob = blob.Replace("-", "/").Replace("_", "=");
                byte[] bytes = Convert.FromBase64String(blob);
                bytes = CLZF2.Decompress(bytes);
                MemoryStream mem = new MemoryStream(bytes, false);
                BinaryFormatter binf = new BinaryFormatter();
                LCARS.lODN.CommunicationQueue = (List<LCARS_CommunicationQueue_Type>)binf.Deserialize(mem);
            }
            catch (Exception e)
            {
                LCARS.lODN.CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();
                throw e;
            }
        }



        /*


        // Convert an Object to a byte array
        private byte[] LCARS_ShipStatus_TypeToByteArray(Dictionary<string, LCARS_ShipSystem_Type> obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        // Convert a byte array to an Object
        private Dictionary<string, LCARS_ShipSystem_Type> ByteArrayToLCARS_ShipSystem_Type(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Dictionary<string, LCARS_ShipSystem_Type> obj = (Dictionary<string, LCARS_ShipSystem_Type>)binForm.Deserialize(memStream);
            return obj;
        }

        /* DATA: serialization and compression * /
        public string ShipSystemsSerialize()
        {
            //byte[] bytes = LCARS_ShipStatus_TypeToByteArray(LCARS.lODN.ShipSystems);
            MemoryStream mem = new MemoryStream();
            IFormatter binf = new BinaryFormatter();
            binf.Serialize(mem, LCARS.lODN.ShipSystems);
            string blob = Convert.ToBase64String(CLZF2.Compress(mem.ToArray()));
            return blob.Replace("/", "-").Replace("=", "_");

        }
        public void ShipSystemsDeserialize(string blob)
        {
            try
            {
                blob = blob.Replace("-", "/").Replace("_", "=");
                byte[] bytes = Convert.FromBase64String(blob);
                bytes = CLZF2.Decompress(bytes);
                MemoryStream mem = new MemoryStream(bytes, false);
                BinaryFormatter binf = new BinaryFormatter();
                LCARS.lODN.ShipSystems = (Dictionary<string, LCARS_ShipSystem_Type>)binf.Deserialize(mem);
            }
            catch (Exception e)
            {
                LCARS.lODN.ShipSystems = new Dictionary<string, LCARS_ShipSystem_Type>();
                throw e;
            }
        }
 */










        public float distance_threshold { get; set; }
    }
}
