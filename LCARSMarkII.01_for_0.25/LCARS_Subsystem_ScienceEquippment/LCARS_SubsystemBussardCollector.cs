using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_SubsystemBussardCollector : ILCARSPlugin
    {
        public string subsystemName { get { return "Bussard Collector"; } }
        public string subsystemDescription {get{return "Gather Resources from Space";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 80f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 100f; } } // Power draw for additional consumtion - is added to running
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
            GUILayout.Label("ToDo: " + thisVessel.vesselName);
            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
