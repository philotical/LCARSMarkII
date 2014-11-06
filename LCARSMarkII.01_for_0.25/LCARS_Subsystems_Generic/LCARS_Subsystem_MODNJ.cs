
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_MODNJ : ILCARSPlugin
    {
        public string subsystemName { get { return "Main ODN Junction"; } }
        public string subsystemDescription { get { return "The heart of the ship's systems"; } }
        public string subsystemStation { get { return "Engineering"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start

        private GUIStyle scrollview_style;
        private Vector2 MODNJ_ScrollPosition1;
        private Vector2 MODNJ_ScrollPosition2;
        private Vector2 MODNJ_ScrollPosition3;
        private Vector2 MODNJ_ScrollPosition4;
        private Vector2 MODNJ_ScrollPosition5;
        private Vector2 MODNJ_ScrollPosition6;
        private GUIStyle toggle_style = null;


        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_MODNJ SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_MODNJ SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }


        public void getGUI()
        {
            if (scrollview_style == null)
            {
                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 200;
            }
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                toggle_style.alignment = TextAnchor.MiddleCenter;
                toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(0, 0, 0, 0);
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
                toggle_style.fixedWidth = 74;
                toggle_style.fixedHeight = 20;
            }

            //UnityEngine.Debug.Log("LCARS_Subsystem_MODNJ getGUI begin ");
            GUILayout.Label("LCARS_Subsystem_MODNJ: ");


            GUILayout.BeginVertical(scrollview_style);
            MODNJ_ScrollPosition1 = GUILayout.BeginScrollView(MODNJ_ScrollPosition1);
            int oddEvenCount = 1;
            bool sectionIsOpen = false;
            foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
            {

                if (!pair.Value.show_in_MODNJ)
                { continue; }




                toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/ODNSwitch_nominal", false);
                toggle_style.active.background = (pair.Value.isDisabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/ODNSwitch_disabled", false) : toggle_style.active.background;
                toggle_style.active.background = (pair.Value.isDamaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/ODNSwitch_damaged", false) : toggle_style.active.background;
                toggle_style.normal.background = toggle_style.active.background;

                string status = "Nominal";
                status = (pair.Value.isDisabled) ? "Disabled" : status;
                status = (pair.Value.isDamaged) ? "Damaged" : status;
                if (oddEvenCount % 2 != 0)
                { // odd
                    GUILayout.BeginHorizontal();
                    sectionIsOpen = true;
                }
                else
                {// even
                }


                    GUILayout.BeginVertical(GUILayout.Width(215));
                        
                        GUILayout.BeginHorizontal();
                            LCARS.lODN.ShipSystems[pair.Value.name].disabled = GUILayout.Toggle(LCARS.lODN.ShipSystems[pair.Value.name].disabled, "", toggle_style);
                            GUILayout.Label(pair.Value.name);
                            //GUILayout.Label("Integrity: " + pair.Value.integrity+"%");
                            //GUILayout.Label("Status: " + status);
                        GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    if (oddEvenCount % 2 != 0)
                    { // odd
                    }
                    else
                    {// even
                        GUILayout.EndHorizontal();
                        sectionIsOpen = false;
                    }
                    oddEvenCount++;


            }
            if (sectionIsOpen)
            { 
                GUILayout.EndHorizontal();
                sectionIsOpen = false;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            
            //UnityEngine.Debug.Log("LCARS_Subsystem_MODNJ getGUI done ");
        }

    }

}
