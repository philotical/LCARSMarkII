using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Tactical_Station
    {
        private GUIStyle Tactical_BackGroundLayoutStyle = null;
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
            if (Tactical_BackGroundLayoutStyle == null)
            {
                Tactical_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Tactical_BackGroundLayoutStyle.alignment = TextAnchor.MiddleCenter;
                Tactical_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
                Tactical_BackGroundLayoutStyle.padding = new RectOffset(20, 40, 0, 0);
                Tactical_BackGroundLayoutStyle.fixedWidth = 485;
                Tactical_BackGroundLayoutStyle.fixedHeight = 301;
                Tactical_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Tactical_Background", false);
                Tactical_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Tactical_Background", false);
                Tactical_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Tactical_Background", false);
                Tactical_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Tactical_Background", false);
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

            GUILayout.BeginVertical(Tactical_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(301));
            GUILayout.Label("");

            GUIContent content = new GUIContent("");
            toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 74;
            toggle_style.fixedHeight = 11;

            if (GUI.Button(new Rect(402, 10, 74, 11), content, toggle_style))
            {
                LCARS.lWindows.setWindowState("Tactical", false);
            }



            GUILayout.EndVertical();
        }

    }
}
