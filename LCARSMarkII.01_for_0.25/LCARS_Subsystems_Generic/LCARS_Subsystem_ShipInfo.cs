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

        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
                GUILayout.BeginVertical();
                GUILayout.Label("Name: " + thisVessel.vesselName);
                GUILayout.Label("Hull Intergity: " + Math.Round(thisVessel.LCARSVessel_HullIntegrity_percentage(), 2) + "% ");
                GUILayout.Label("  Avg. PartTemperature" + Math.Round(thisVessel.LCARSVessel_AveragePartTemperature(), 2) + "° of "+Math.Round(thisVessel.LCARSVessel_AveragePartTemperatureMax(), 2) + "° max ="+Math.Round(thisVessel.LCARSVessel_Heat_percentage(), 2) + "% ");
                GUILayout.Label("Mass: " + Math.Round(thisVessel.LCARSVessel_WetMass(), 2) + " t");
                GUILayout.Label("   Dry: " + Math.Round(thisVessel.LCARSVessel_DryMass(), 2) + " t");
                GUILayout.Label("   Resources: " + Math.Round(thisVessel.LCARSVessel_ResourceMass(), 2) + " t");
                GUILayout.Label("Parts: " + thisVessel.parts.Count);
                GUILayout.Label("Crew compliment: " + thisVessel.GetCrewCount()+"/"+thisVessel.GetCrewCapacity());
                GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
