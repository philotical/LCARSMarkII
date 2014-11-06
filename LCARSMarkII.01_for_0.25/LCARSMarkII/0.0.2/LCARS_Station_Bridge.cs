using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Bridge_Station
    {
/*
        Bridge:
        - Vessel Selector
        - Ship Info
        - Command pannel
        - Formation mode
        - NCI mission data
*/

        private GUIStyle Bridge_BackGroundLayoutStyle = null;
        private GUIStyle toggle_style = null;
        private bool show_debug_options = false;
        public int selGridInt = 5;
        public string[] selStrings = new string[] { "RV+", "T+", "N+", "MT", "P", "NONE", "RV-", "T-", "N-", "MT-", "R" };

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }

        Dictionary<string, bool> backupbool_windowstate = null;

        public void getGUI()
        {
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                toggle_style.alignment = TextAnchor.MiddleCenter;
                toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(0, 0, 0, 0);
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
            }
            if (Bridge_BackGroundLayoutStyle == null)
            {
                Bridge_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Bridge_BackGroundLayoutStyle.alignment = TextAnchor.MiddleCenter;
                Bridge_BackGroundLayoutStyle.padding = new RectOffset(20, 40, 0, 0);
                Bridge_BackGroundLayoutStyle.fixedWidth = 485;
                //Bridge_BackGroundLayoutStyle.fixedHeight = 443;

            }
            if (backupbool_windowstate==null)
            {
                backupbool_windowstate = new Dictionary<string, bool>();
                backupbool_windowstate.Add("Helm", false);
                backupbool_windowstate.Add("Engineering", false);
                backupbool_windowstate.Add("Tactical", false);
                backupbool_windowstate.Add("Science", false);
                backupbool_windowstate.Add("Communication", false);
            }
            Bridge_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_TopBackground", false);
            Bridge_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_TopBackground", false);
            Bridge_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_TopBackground", false);
            Bridge_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
            GUILayout.BeginVertical(Bridge_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(57));

            //GUILayout.Label("Window Controll: ");
            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("");
            toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 74;
            toggle_style.fixedHeight = 11;

            if (GUI.Button(new Rect(402, 10, 74, 11), content, toggle_style))
            { 
                //LCARS.setWindowState(false);
                //LCARS.lToolbar.stockToolbarBtnHide();
                LCARS.lWindows.setWindowState("Bridge", false);

            }

            toggle_style.active.background = (LCARS.lWindows.LCARSWindows["Helm"].state) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/active_button_HelmWindow", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/button_HelmWindow", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 47;
            toggle_style.fixedHeight = 27;
            LCARS.lWindows.LCARSWindows["Helm"].state = GUI.Toggle(new Rect(44, 27, 47, 27), LCARS.lWindows.LCARSWindows["Helm"].state, "", toggle_style);
            if (Input.GetMouseButtonUp(0) && backupbool_windowstate["Helm"] != LCARS.lWindows.LCARSWindows["Helm"].state)
            {
                //LCARS.lWindows.setWindowState("Helm", backupbool_windowstate["Helm"]);
                //LCARS.lWindows.LCARSWindows["Helm"].state = backupbool_windowstate["Helm"];
                //if (LCARS.lWindows.LCARSWindows["Helm"].state)
                //{ LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); }
                //else { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                backupbool_windowstate["Helm"] = LCARS.lWindows.LCARSWindows["Helm"].state;
            }

            toggle_style.active.background = (LCARS.lWindows.LCARSWindows["Engineering"].state) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/active_button_EngineeringWindow", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/button_EngineeringWindow", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 72;
            toggle_style.fixedHeight = 27;
            LCARS.lWindows.LCARSWindows["Engineering"].state = GUI.Toggle(new Rect(92, 27, 72, 27), LCARS.lWindows.LCARSWindows["Engineering"].state, "", toggle_style);
            if (Input.GetMouseButtonUp(0) && backupbool_windowstate["Engineering"] != LCARS.lWindows.LCARSWindows["Engineering"].state)
            {
                //LCARS.lWindows.setWindowState("Engineering", backupbool_windowstate["Engineering"]);
                //LCARS.lWindows.LCARSWindows["Engineering"].state = backupbool_windowstate["Engineering"];
                //if (LCARS.lWindows.LCARSWindows["Engineering"].state)
                //{ LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); }
                //else { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                backupbool_windowstate["Engineering"] = LCARS.lWindows.LCARSWindows["Engineering"].state;
            }

            toggle_style.active.background = (LCARS.lWindows.LCARSWindows["Tactical"].state) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/active_button_TacticalWindow", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/button_TacticalWindow", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 62;
            toggle_style.fixedHeight = 27;
            LCARS.lWindows.LCARSWindows["Tactical"].state = GUI.Toggle(new Rect(165, 27, 62, 27), LCARS.lWindows.LCARSWindows["Tactical"].state, "", toggle_style);
            if (Input.GetMouseButtonUp(0) && backupbool_windowstate["Tactical"] != LCARS.lWindows.LCARSWindows["Tactical"].state)
            {
                //LCARS.lWindows.setWindowState("Tactical", backupbool_windowstate["Tactical"]);
                //LCARS.lWindows.LCARSWindows["Tactical"].state = backupbool_windowstate["Tactical"];
                //if (LCARS.lWindows.LCARSWindows["Tactical"].state)
                //{ LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); }
                //else { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                backupbool_windowstate["Tactical"] = LCARS.lWindows.LCARSWindows["Tactical"].state;
            }

            toggle_style.active.background = (LCARS.lWindows.LCARSWindows["Science"].state) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/active_button_ScienceWindow", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/button_ScienceWindow", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 67;
            toggle_style.fixedHeight = 27;
            LCARS.lWindows.LCARSWindows["Science"].state = GUI.Toggle(new Rect(228, 27, 67, 27), LCARS.lWindows.LCARSWindows["Science"].state, "", toggle_style);
            if (Input.GetMouseButtonUp(0) && backupbool_windowstate["Science"] != LCARS.lWindows.LCARSWindows["Science"].state)
            {
                //LCARS.lWindows.setWindowState("Science", backupbool_windowstate["Science"]);
                //LCARS.lWindows.LCARSWindows["Science"].state = backupbool_windowstate["Science"];
                //if (LCARS.lWindows.LCARSWindows["Science"].state)
                //{ LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); }
                //else { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                backupbool_windowstate["Science"] = LCARS.lWindows.LCARSWindows["Science"].state;
            }

            toggle_style.active.background = (LCARS.lWindows.LCARSWindows["Communication"].state) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/active_button_CommunicationWindow", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/button_CommunicationWindow", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 89;
            toggle_style.fixedHeight = 27;
            LCARS.lWindows.LCARSWindows["Communication"].state = GUI.Toggle(new Rect(296, 27, 89, 27), LCARS.lWindows.LCARSWindows["Communication"].state, "", toggle_style);
            if (Input.GetMouseButtonUp(0) && backupbool_windowstate["Communication"] != LCARS.lWindows.LCARSWindows["Communication"].state)
            {
                //LCARS.lWindows.setWindowState("Communication", backupbool_windowstate["Communication"]);
                //LCARS.lWindows.LCARSWindows["Communication"].state = backupbool_windowstate["Communication"];
                //if (LCARS.lWindows.LCARSWindows["Communication"].state)
                //{ LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); }
                //else { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                backupbool_windowstate["Communication"] = LCARS.lWindows.LCARSWindows["Communication"].state;
            }

            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();

            Bridge_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_MiddleBackground", false);
            Bridge_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_MiddleBackground", false);
            Bridge_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_MiddleBackground", false);
            Bridge_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_MiddleBackground", false);
            Bridge_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            Bridge_BackGroundLayoutStyle.padding = new RectOffset(11, 0, 0, 0);
            GUILayout.BeginVertical(Bridge_BackGroundLayoutStyle, GUILayout.Width(485));

            if (LCARS.lWindows.LCARSWindows["Helm"].state)
            {
                GUILayout.BeginHorizontal();
                selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 6);
                    
                if(selGridInt!=5)
                {
                    switch(selGridInt)
                    {
                        case 0:
                            LCARS.grav.ASAS(thisVessel,Orient.RelativeVelocity);
                            break;
                        case 1:
                            LCARS.grav.ASAS(thisVessel,Orient.RelativeVelocityAway);
                            break;
                        case 2:
                            LCARS.grav.ASAS(thisVessel,Orient.Target);
                            break;
                        case 3:
                            LCARS.grav.ASAS(thisVessel,Orient.TargetAway);
                            break;
                        case 4:
                            LCARS.grav.ASAS(thisVessel,Orient.Normal);
                            break;
                        case 6:
                            LCARS.grav.ASAS(thisVessel,Orient.AntiNormal);
                            break;
                        case 7:
                            LCARS.grav.ASAS(thisVessel,Orient.MatchTarget);
                            break;
                        case 8:
                            LCARS.grav.ASAS(thisVessel,Orient.MatchTargetAway);
                            break;
                        case 9:
                            LCARS.grav.ASAS(thisVessel,Orient.Prograde);
                            break;
                        case 10:
                            LCARS.grav.ASAS(thisVessel,Orient.Retrograde);
                            break;
                    }
                }

                GUILayout.EndHorizontal();
            }



                show_debug_options = GUILayout.Toggle(show_debug_options,"show debug menu");
                if (show_debug_options)
                {

                    GUILayout.Label("Debug: Send Test Messages");
                    if (GUILayout.Button("send plain"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test plain";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 1;
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Queue();
                    }
                    if (GUILayout.Button("send encrypted Level0"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test Level0";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 1;
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Encrypt(0);
                        mT.Queue();
                    }
                    if (GUILayout.Button("send encrypted Level1"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test Level1";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 1;
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Encrypt(1);
                        mT.Queue();
                    }
                    if (GUILayout.Button("send encrypted Level2"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test Level2";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 1;
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Encrypt(2);
                        mT.Queue();
                    }
                    if (GUILayout.Button("send encrypted Level2b"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test Level2b";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 5;
                        mT.decryption_artefact = "DumboTheGreat";
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Encrypt(2);
                        mT.Queue();
                    }
                    if (GUILayout.Button("send encrypted Level2c"))
                    {
                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.receiver_vessel = LCARS.vessel;
                        mT.sender = "Captain";
                        mT.sender_id_line = " Captain wanted to test Level2b";
                        mT.receiver_type = "Station";
                        mT.receiver_code = "Engineering";
                        mT.priority = 5;
                        mT.decryption_artefact = "DumboTheSmall";
                        mT.setTitle("The Captain wrote a title");
                        mT.setMessage("For the world is hollow and I have touched the sky");
                        mT.Encrypt(2);
                        mT.Queue();
                    }

                    GUILayout.Label("Debug: Damage/Repair Tests: ");
                    GUILayout.Label("HoverForce Intergity=" + LCARS.lODN.ShipSystems["HoverForce"].integrity + "%  isNominal=" + LCARS.lODN.ShipSystems["HoverForce"].isNominal);
                    if (GUILayout.Button("inflictDamage"))
                    {
                        LCARS.lODN.ShipSystems["HoverForce"].inflictDamage(20.9f);
                    }
                    if (GUILayout.Button("repairDamage (obsolete)"))
                    {
                        LCARS.lODN.ShipSystems["HoverForce"].repairDamage(50.123f);
                    }
                    GUILayout.Label("PropulsionMatrix Intergity=" + LCARS.lODN.ShipSystems["PropulsionMatrix"].integrity + "%  isNominal=" + LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal);
                    if (GUILayout.Button("inflictDamage"))
                    {
                        LCARS.lODN.ShipSystems["PropulsionMatrix"].inflictDamage(20.9f);
                    }
                    if (GUILayout.Button("repairDamage (obsolete)"))
                    {
                        LCARS.lODN.ShipSystems["PropulsionMatrix"].repairDamage(50.123f);
                    }
                    GUILayout.Label("InertiaDamper Intergity=" + LCARS.lODN.ShipSystems["InertiaDamper"].integrity + "%  isNominal=" + LCARS.lODN.ShipSystems["InertiaDamper"].isNominal);
                    if (GUILayout.Button("inflictDamage"))
                    {
                        LCARS.lODN.ShipSystems["InertiaDamper"].inflictDamage(20.9f);
                    }
                    if (GUILayout.Button("repairDamage (obsolete)"))
                    {
                        LCARS.lODN.ShipSystems["InertiaDamper"].repairDamage(50.123f);
                    }
                    /*
                    */

                }
                LCARS.lSubSys.getGUI("Bridge");
            GUILayout.EndVertical();


            Bridge_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_BottomBackground", false);
            Bridge_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_BottomBackground", false);
            Bridge_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_BottomBackground", false);
            Bridge_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Bridge_BottomBackground", false);
            Bridge_BackGroundLayoutStyle.margin = new RectOffset(0, 0, 0, 0);
            GUILayout.BeginVertical(Bridge_BackGroundLayoutStyle, GUILayout.Width(485), GUILayout.Height(19));
            GUILayout.Label("", GUILayout.Width(485), GUILayout.Height(10));
            GUILayout.EndVertical();
        }

    }
}
