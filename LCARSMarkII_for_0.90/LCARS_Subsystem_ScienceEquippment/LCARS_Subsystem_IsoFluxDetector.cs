using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{


    public class LCARS_Subsystem_IsoFluxDetector : ILCARSPlugin
    {
        public string subsystemName { get { return "Iso Flux Detector"; } }
        public string subsystemDescription {get{return "Can be used to localize ActionPoints";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 100f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 20f; } } // Power draw for additional consumtion - is added to running
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

        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 0.5f;

        Vessel tVessel = null;
        GameObject tGo = null;
        LCARS_ScanReportItem_Type ScanReportItem = null;
        //float dist = 0f;
        float default_distance_threshold = 5000f;
        //public float distance_threshold = 0.0f;

        public void getGUI()
        {
            if (thisVessel.LCARS().lODN.distance_threshold == 0.0f)
            {
                try 
                {
                    string tmp = LCARSNCI_Bridge.Instance.GetEquippment_Setting("IsoFluxDetector", "distance_threshold");
                    thisVessel.LCARS().lODN.distance_threshold = (tmp != "ERROR") ? LCARS_Utilities.ToFloat(tmp) : default_distance_threshold;
                }
                catch 
                {
                    thisVessel.LCARS().lODN.distance_threshold = default_distance_threshold;
                }
            }

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Reset this system"))
            {
                thisVessel.LCARS().lODN.IFD_object_of_interest = null;
                LCARS.lODN.IFD_show_results_as_screenmessage = false;
                LCARS.lODN.IFD_show_results_as_consolemessage = false;
            }

            //PQSCity

            /*
            ScanReport_Item.NCIactionpoints
            ScanReport_Item.target_type = "ship";
            ScanReport_Item.idcode = SelectedTargetShip.vesselName;
            ScanReport_Item.guid = SelectedTargetShip.id;
            ScanReport_Item.time = DateTime.Now.ToString();
            ScanReport_Item.scan_level = "3";
            */

            if(thisVessel.LCARS().lODN.IFD_object_of_interest != null)
            {
                GUILayout.Label("IFD-Target: " + thisVessel.LCARS().lODN.IFD_object_of_interest.name);
                GUILayout.Label("distance_threshold: " + thisVessel.LCARS().lODN.distance_threshold);




                // this happens now in LCARS_Util_FlightControll
                /*
                if ((Time.time - lastFixedUpdate) > logInterval)
                {
                    dist = Vector3.Distance(thisVessel.CoM  ,   thisVessel.LCARS().lODN.IFD_object_of_interest.transform.position);
                    LCARS.lODN.IFD_object_distance = dist;
                    lastFixedUpdate = Time.time;
                }
                */





                if (thisVessel.LCARS().lODN.distance_threshold > LCARS.lODN.IFD_object_distance)
                {
                    GUILayout.Label("We've detected an isometric fluctuation related to the object's signature ");
                    GUILayout.Label("The flux is at a distance of " + LCARS.lODN.IFD_object_distance + " meters.");
                    GUILayout.Label("We are not able to triangulate the direction.");

                    LCARS.lODN.IFD_show_results_as_screenmessage = GUILayout.Toggle(LCARS.lODN.IFD_show_results_as_screenmessage, "Send to Screen");
                    LCARS.lODN.IFD_show_results_as_consolemessage = GUILayout.Toggle(LCARS.lODN.IFD_show_results_as_consolemessage, "Send to Console");
                }
                else 
                {
                    GUILayout.Label("Distance: Object out of range.. ");
                }
            }
            else
            {
                GUILayout.Label("The IFD has no target to work with!");
                LCARS.lODN.IFD_show_results_as_screenmessage = false;
                LCARS.lODN.IFD_show_results_as_consolemessage = false;
            }




            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
