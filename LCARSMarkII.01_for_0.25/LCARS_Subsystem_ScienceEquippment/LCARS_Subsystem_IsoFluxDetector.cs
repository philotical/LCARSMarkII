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

        GUIStyle scrollview_style1;
        Vector2 ScrollPosition1;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 5.0f;
        public void getGUI()
        {
            if ((Time.time - lastFixedUpdate) > logInterval)
            {
                lastFixedUpdate = Time.time;

            }


            if (scrollview_style1 == null)
            {
                scrollview_style1 = new GUIStyle();
                scrollview_style1.fixedHeight = 255;
            }

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();
            GUILayout.Label("ToDo: " + thisVessel.vesselName);


            GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(445));
            ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);



            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            //PQSCity










            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
