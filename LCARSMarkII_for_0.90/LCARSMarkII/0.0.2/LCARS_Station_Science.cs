using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Science_Station
    {
        private GUIStyle Science_BackGroundLayoutStyle = null;
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
            if (Science_BackGroundLayoutStyle == null)
            {
                Science_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Science_BackGroundLayoutStyle.alignment = TextAnchor.MiddleCenter;
                Science_BackGroundLayoutStyle.padding = new RectOffset(0, 0, 0, 0);
                Science_BackGroundLayoutStyle.fixedWidth = 485;
                //Science_BackGroundLayoutStyle.fixedHeight = 443;

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
                LCARS.lWindows.setWindowState("Science", false);
            }
            Science_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Science_TopBackground", false);
            Science_BackGroundLayoutStyle.onNormal.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.onHover.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
            GUILayout.BeginVertical(Science_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(48));
            GUILayout.Label("");


            GUILayout.EndVertical();

            Science_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Science_MiddleBackground", false);
            Science_BackGroundLayoutStyle.onNormal.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.onHover.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            Science_BackGroundLayoutStyle.padding = new RectOffset(11, 0, 0, 0);
            GUILayout.BeginVertical(Science_BackGroundLayoutStyle, GUILayout.Width(485));


            /*
             * 
             * BiomScanner, Biom Browser
             * Equippment 
             *      Bussard Collector - gathers resources - what and where can be defined by the story creator
                    Iso Flux Detector - finds Easteregg if prepared to do so - what and where can be defined by the story creator
                    Nucleonic Analyzer - if you feed in an artefact, it can gather some information about it - what can be defined by the story creator
                    Pteroplastic Scrambler - if you feed one artefact, it will produce an other - the story creator defines all parameters. 
             * They all come as subsystem and are downloaded with NCI
             * 
             * Sensor Array
             * Detect: ships, ship types, bodys in SOI, 
             * After Scan, access to gameobjects, can be used with equippment or to target (maybe).
             *  
             * Scann levels:
             *      - if Ship:
             *          - standard scann
             *          - tactical
             *          - internal
             *      - if body
             *          - Global surface Scann (maybe with mapsat)
             *          - Biomscann
             *          - Global Annomalies scan
             * 
             * Astrometrics:
             *  Flight data like apoasis,periapsis, time to etc..
             * 
             * Library Computer Access Panel to see 
             *  scanned objects and it's features (saved in savegame folder), 
             *  all visited Bioms and planet data
             *  if possible annomalies per body (only after scann)
             * 
             * 
             */


            LCARS.lSubSys.getGUI("Science");



            GUILayout.EndVertical();




            Science_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Science_BottomBackground", false);
            Science_BackGroundLayoutStyle.onNormal.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.onHover.background = Science_BackGroundLayoutStyle.normal.background;
            Science_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            GUILayout.BeginVertical(Science_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(21));
            GUILayout.Label("", GUILayout.Width(485), GUILayout.Height(10));
            GUILayout.EndVertical();
        }

    }
}
