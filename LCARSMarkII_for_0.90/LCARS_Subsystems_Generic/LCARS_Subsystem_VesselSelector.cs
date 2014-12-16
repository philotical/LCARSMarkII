
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_VesselSelector : ILCARSPlugin
    {
        public string subsystemName { get { return "Target Selector"; } }
        public string subsystemDescription { get { return "Select wich LCARS Vessel you want to controll"; } }
        public string subsystemStation { get { return "Bridge"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start



        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        private Vector2 _scrollPosition = new Vector2(0, 0);
        public void getGUI()
        {



            //UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI begin ");


            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(300), GUILayout.Height(300));

            // Generate and sort an array of vessels by distance.
            LCARS.grav.vesselList = new List<Vessel>(FlightGlobals.Vessels);
            var vdc = new VesselDistanceComparer();
            vdc.OriginVessel = thisVessel;
            
            LCARS.grav.vesselList.Sort(vdc);

            for (int i = 0; i < LCARS.grav.vesselList.Count; i++)
            {
                // Skip ourselves.
                if (LCARS.grav.vesselList[i] == thisVessel)
                    continue;

                //if (LCARS.grav.vesselList[i].LandedOrSplashed)
                    //continue;

                // Skip stuff around other worlds.
                if (thisVessel.orbit.referenceBody != LCARS.grav.vesselList[i].orbit.referenceBody)
                    continue;

                // Calculate the distance.
                float d = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                if (GUILayout.Button((d / 1000).ToString("F1") + "km " + LCARS.grav.vesselList[i].vesselName, GUILayout.ExpandWidth(true)))
                {
                    //Mode = UIMode.SELECTED;
                    LCARS.grav.selectedVesselInstanceId = LCARS.grav.vesselList[i].GetInstanceID();
                    LCARS.grav.selectedVesselIndex = FlightGlobals.Vessels.IndexOf(LCARS.grav.vesselList[i]);
                    FlightGlobals.fetch.SetVesselTarget(LCARS.grav.vesselList[i]);
                }
            }

            GUILayout.EndScrollView();



            /*


            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI begin ");
            GUILayout.BeginHorizontal();
                GUILayout.Label("Host Vessel: " + FlightGlobals.ActiveVessel.vesselName);
                if (thisVessel.id != FlightGlobals.ActiveVessel.id)
                {
                    if (GUILayout.Button("return to host"))
                    {
                        UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 4 ");
                        changeLinkUpVessel(FlightGlobals.ActiveVessel);
                        UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector changeLinkUpVessel clicked v.vesselName=" + FlightGlobals.ActiveVessel.vesselName);
                    }
                }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("LCARS LinkUp Vessel: " + thisVessel.vesselName);
            GUILayout.EndHorizontal();

            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 1 ");

            GUILayout.Label("Select LinkUp Vessel: ");
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 2 ");
                if (v.isLCARSVessel() && thisVessel.id != v.id)
                {
                    UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 3 ");
                    if (GUILayout.Button(v.vesselName))
                    {
                        UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 4 ");
                        changeLinkUpVessel(v);
                        UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector changeLinkUpVessel clicked v.vesselName=" + v.vesselName);
                    }
                    UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 5 ");

                }
                UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 6 ");
            }
            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI 7 ");
            
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI done ");
        */
            //UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector getGUI done ");
        }

        private void changeLinkUpVessel(Vessel v)
        {
            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector changeLinkUpVessel begin ");
            LCARS.SendMessage("SetVessel",v);
            UnityEngine.Debug.Log("LCARS_Subsystem_VesselSelector changeLinkUpVessel done ");
        }

    }

}
