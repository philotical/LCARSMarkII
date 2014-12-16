using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Engineering_Station
    {
        private GUIStyle Engineering_BackGroundLayoutStyle = null;
        private GUIStyle toggle_style = null;

        
        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }
        public void getGUI()
        {
            if (Engineering_BackGroundLayoutStyle == null)
            {
                Engineering_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Engineering_BackGroundLayoutStyle.alignment = TextAnchor.MiddleCenter;
                Engineering_BackGroundLayoutStyle.padding = new RectOffset(0, 0, 0, 0);
                Engineering_BackGroundLayoutStyle.fixedWidth = 485;
                //Engineering_BackGroundLayoutStyle.fixedHeight = 443;

            }
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                toggle_style.alignment = TextAnchor.MiddleCenter;
                toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(0, 0, 0, 0);
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
            }

            GUIContent content = new GUIContent("");
            toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 74;
            toggle_style.fixedHeight = 11;

            if (GUI.Button(new Rect(402, 10, 74, 11), content, toggle_style))
            {
                LCARS.lWindows.setWindowState("Engineering", false);
            }

            Engineering_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Engineering_TopBackground", false);
            Engineering_BackGroundLayoutStyle.onNormal.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.onHover.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
            GUILayout.BeginVertical(Engineering_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(48));
            GUILayout.Label("");


            GUILayout.EndVertical();

            Engineering_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Engineering_MiddleBackground", false);
            Engineering_BackGroundLayoutStyle.onNormal.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.onHover.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            Engineering_BackGroundLayoutStyle.padding = new RectOffset(11, 0, 0, 0);
            GUILayout.BeginVertical(Engineering_BackGroundLayoutStyle, GUILayout.Width(485));


            if (LCARS.lODN.ShipSystems["FullImpulse"].isNominal && LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal)
            {
                LCARS.lODN.ShipStatus.LCARS_FullImpulse = GUILayout.Toggle(LCARS.lODN.ShipStatus.LCARS_FullImpulse, "Full Impulse");

                if (LCARS.lODN.ShipSystems["UseReserves"].isNominal)
                {
                    if (LCARS.lODN.ShipStatus.LCARS_FullImpulse)
                    {
                        LCARS.lODN.ShipStatus.LCARS_UseReserves = GUILayout.Toggle(LCARS.lODN.ShipStatus.LCARS_UseReserves, "Use Reserves");
                    }
                }
            }


                LCARS.lSubSys.getGUI("Engineering");
            GUILayout.EndVertical();




            Engineering_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Engineering_BottomBackground", false);
            Engineering_BackGroundLayoutStyle.onNormal.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.onHover.background = Engineering_BackGroundLayoutStyle.normal.background;
            Engineering_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            GUILayout.BeginVertical(Engineering_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(21));
            GUILayout.Label("", GUILayout.Width(485), GUILayout.Height(10));
            GUILayout.EndVertical();
        }
    }
}
