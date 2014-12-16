using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_ShipInfo : ILCARSPlugin
    {
        public string subsystemName { get{return "Ship Info";}}
        public string subsystemDescription {get{return "Display Stats about this vessel";}}
        public string subsystemStation { get { return "Bridge"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return true; } }
        public bool subsystemPanelState { get; set; } // has to be false at start

        

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        bool show_ship_data = true;
        bool show_crew_data = false;
        bool show_engine_data = false;
        bool show_hull_data = false;
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
                GUILayout.BeginVertical();
                GUILayout.Label("Name: " + thisVessel.vesselName);
                GUILayout.Label(" ");

                show_ship_data = GUILayout.Toggle(show_ship_data, "Ship");
                if (show_ship_data)
                {
                    GUILayout.Label("Mass: " + Math.Round(thisVessel.LCARSVessel_WetMass(), 2) + " t");
                    GUILayout.Label("   Dry: " + Math.Round(thisVessel.LCARSVessel_DryMass(), 2) + " t");
                    GUILayout.Label("   Resources: " + Math.Round(thisVessel.LCARSVessel_ResourceMass(), 2) + " t");
                    GUILayout.Label("Parts: " + thisVessel.parts.Count);
                    GUILayout.Label("Crash Tolerance (Rootpart): " + thisVessel.rootPart.crashTolerance + "m/s");
                    GUILayout.Label("Temp Tolerance (Rootpart): " + thisVessel.rootPart.maxTemp + "°");
                    GUILayout.Label(" ");
                }
                show_crew_data = GUILayout.Toggle(show_crew_data, "Crew");
                if (show_crew_data)
                {
                    GUILayout.Label("Crew compliment: " + thisVessel.GetCrewCount() + "/" + thisVessel.GetCrewCapacity());
                    GUILayout.Label(" ");
                }
                show_engine_data = GUILayout.Toggle(show_engine_data, "Engine");
                if(show_engine_data)
                {
                    if (LCARS.lODN.ShipStatus.numpadcontroll_enabled)
                    {
                        if(GUILayout.Button("Disable Numpad Controll"))
                        {
                            LCARS.lODN.ShipStatus.numpadcontroll_enabled = false;
                        }
                        GUILayout.Label("Up/Down:  9/3");
                        GUILayout.Label("Forward/Back:  8/2");
                        GUILayout.Label("Left/Right:  4/6");
                        GUILayout.Label("Toggle FullHalt:  5");
                        GUILayout.Label("Toggle ProgradeStabilizer:  7");
                        GUILayout.Label("Toggle HoldSpeed:  1");
                        GUILayout.Label("Toggle HoldHeight:  0");
                        GUILayout.Label("How much Force for the Numpad? " + LCARS.lODN.ShipStatus.numpadcontroll_thrust);
                        LCARS.lODN.ShipStatus.numpadcontroll_thrust = GUILayout.HorizontalSlider((float)LCARS.lODN.ShipStatus.numpadcontroll_thrust, 1, 100);
                    }
                    else 
                    {
                        if(GUILayout.Button("Enable Numpad Controll"))
                        {
                            LCARS.lODN.ShipStatus.numpadcontroll_enabled = true;
                        }
                    }
                    GUILayout.Label("Engine Type: " + LCARS.lODN.ImpulseEngineTypes[LCARS.engine_type].name);
                    GUILayout.Label("max_weight: " + LCARS.lODN.ImpulseEngineTypes[LCARS.engine_type].max_weight);
                    GUILayout.Label("PowerUsageFactor: " + LCARS.lODN.ImpulseEngineTypes[LCARS.engine_type].ImpulseDefaultPowerUsageFactor);
                    GUILayout.Label("PowerUsage: " + LCARS.lODN.ShipStatus.current_total_charge+"MW");
                    GUILayout.Label("Current Engine Performance: " + LCARS.lODN.ShipStatus.current_total_charge+"%");
                    GUILayout.Label("Engine Efficiency: " + LCARS.EngineConstant+"%");
                    if (LCARS.EngineConstant<100.0f)
                    {
                        GUILayout.Label("**************");
                        GUILayout.Label("Warning:");
                        GUILayout.Label("Your ship does not meet the engine specifications.");
                        GUILayout.Label("Ship's performance is reduced due to overweight! ");
                        GUILayout.Label("**************");
                    }
                    GUILayout.Label(" ");
                }
                show_hull_data = GUILayout.Toggle(show_hull_data, "Hull");
                if (show_hull_data)
                {
                    GUILayout.Label("Hull Intergity: " + Math.Round(thisVessel.LCARSVessel_HullIntegrity_percentage(), 2) + "% ");
                    GUILayout.Label("Current SFI Force: " + LCARS.lODN.ShipStatus.current_LCARS_SFI_force + "");
                    GUILayout.Label("Current SFI Performance: " + (100*LCARS.lODN.ShipStatus.current_LCARS_SFI_performance) + "%");
                    GUILayout.Label("Crash Tolerance (Rootpart): " + thisVessel.rootPart.crashTolerance+"m/s");
                    GUILayout.Label("Temp Tolerance (Rootpart): " + thisVessel.rootPart.maxTemp + "°");
                    GUILayout.Label("  Avg. PartTemperature" + Math.Round(thisVessel.LCARSVessel_AveragePartTemperature(), 2) + "° of " + Math.Round(thisVessel.LCARSVessel_AveragePartTemperatureMax(), 2) + "° max =" + Math.Round(thisVessel.LCARSVessel_Heat_percentage(), 2) + "% ");
                    GUILayout.Label(" ");
                }
                GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
