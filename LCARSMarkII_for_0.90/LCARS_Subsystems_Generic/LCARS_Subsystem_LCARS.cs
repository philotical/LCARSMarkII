using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_LCARS : ILCARSPlugin
    {
        public string subsystemName { get{return "Library Computer Access System";}}
        public string subsystemDescription {get{return "It's an Information Database";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
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

        public string[] selStrings = new string[] { "Home", "Scan Results" };
        private int selGridInt = 0;
        public void getGUI()
        {

            selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 4);

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();


            switch (selGridInt)
            {
                case 0:
                    GUILayout.Label("Library Computer Access And Retrieval System");
                    GUILayout.Label("Status: Online");
                    GUILayout.Label("Clearance Level: Valid");
                    GUILayout.Label("Access Mode: Limited Access Granted");
                    break;

                case 1:
                    GUILayout.Label("Other: ");
                    ScanResultsGUI();
                    break;

            }




            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

        GUIStyle scrollview_style;
        Vector2 ScrollPosition;
        LCARS_ScanReportItem_Type ScanReportItem = null;
        Texture2D BiomeTexture = null;
        bool IFD_error = false;
        private void ScanResultsGUI()
        {
            if (scrollview_style == null)
            {
                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 250;
            }
            if (ScanReportItem != null)
            {
                if (GUILayout.Button("Back")) { ScanReportItem = null; }
                scrollview_style.fixedHeight = 300;
                GUILayout.BeginVertical(scrollview_style);
                ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

                try
                {
                    GUILayout.Label("Object ID: " + ScanReportItem.idcode);
                }
                catch { }
                try
                {
                    GUILayout.Label("Guid: " + ScanReportItem.guid);
                }
                catch { }
                try
                {
                    GUILayout.Label("description: " + ScanReportItem.description);
                    GUILayout.Label("distance: " + ScanReportItem.distance);
                }
                catch { }

                if (ScanReportItem.NCIactionpoints!=null)
                {
                    if (IFD_error)
                    {
                        GUILayout.Label("Error: IFD was not able to identify this GameObject!");
                        GUILayout.Label("Maybe the object is not loaded.");
                    }
                    GUILayout.Label("NCIactionpoints: ");
                    foreach (string s in ScanReportItem.NCIactionpoints)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(s);
                        if (GUILayout.Button("Send to IFD")) 
                        {
                            IFD_error = false;
                            foreach(Vessel v in FlightGlobals.Vessels)
                            {
                                if (ScanReportItem.guid == v.id)
                                {
                                    try
                                    {
                                        GameObject tmp = v.rootPart.FindModelTransform(s).gameObject;
                                        thisVessel.LCARS().lODN.IFD_object_of_interest = tmp;
                                    }
                                    catch { IFD_error = true; }
                                    continue;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                switch (ScanReportItem.target_type)
                {
                    case "ship":
                        break;

                    case "body":
                        switch (ScanReportItem.scan_level)
                        {
                            case "biome":
                                GUILayout.Label("Biome Scan Results");
                                GUILayout.BeginVertical();
                                //CelestialBody.BiomeMap.Map;
                                BiomeTexture = GameDatabase.Instance.GetTexture(ScanReportItem.body_biom_map, false);
                                GUIContent content = new GUIContent(BiomeTexture, "");
                                GUILayout.Box(content, GUILayout.Height(260), GUILayout.Width(260));
                                GUILayout.EndVertical();
                                break;

                            case "surface":
                                GUILayout.Label("surface Scan Results - TODO");
                                break;

                            case "structure":
                                GUILayout.Label("structure Scan Results - TODO");

                                if (GUILayout.Button("Send to IFD"))
                                {
                                    thisVessel.LCARS().lODN.IFD_object_of_interest = ScanReportItem.gameobject;
                                }

                                break;

                        }
                        break;
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Time" );
                    GUILayout.Label("type" );
                    GUILayout.Label("level" );
                    GUILayout.Label("idcode" );
                GUILayout.EndHorizontal();

                foreach (LCARS_ScanReportItem_Type R in thisVessel.LCARS().lODN.ScanReport.list)
                {
                    GUILayout.BeginHorizontal();
                        GUILayout.Label(R.time);
                        GUILayout.Label(R.target_type);
                        GUILayout.Label(R.scan_level);
                        GUILayout.Label(R.idcode);
                        if (GUILayout.Button("Details")) { ScanReportItem = R; }
                    GUILayout.EndHorizontal();

                }
            }

        }

    } 
    
}
