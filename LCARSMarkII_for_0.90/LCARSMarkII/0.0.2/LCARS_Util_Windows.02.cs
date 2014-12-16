using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARSWindowType
    {
        public string name { set; get; }
        public string title { set; get; }
        public bool state { set; get; }
        public bool damagable { set; get; }
        public Rect position { set; get; }
        public int windowID { set; get; }
        public GUI.WindowFunction wf { set; get; }
    }
    public class LCARS_Windows_Util
    {

        public Dictionary<string, LCARSWindowType> LCARSWindows = new Dictionary<string, LCARSWindowType>();

        Dictionary<string, GUI.WindowFunction> lwt;
        private static System.Random rnd = new System.Random();

        private GUIStyle myStyle = null;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        internal void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }
        internal void initWindows()
        {
            if (lwt == null)
            {
                lwt = new Dictionary<string, GUI.WindowFunction>();
                lwt.Add("Bridge", Bridge_win);
                lwt.Add("Helm", Helm_win);
                lwt.Add("Engineering", Engineering_win);
                lwt.Add("Communication", Communication_win);
                lwt.Add("Science", Science_win);
                lwt.Add("Tactical", Tactical_win);


                foreach (string key in lwt.Keys)
                {
                    LCARSWindowType LWT = new LCARSWindowType();
                    LWT.name = key;
                    LWT.title = "";
                    LWT.damagable = false;
                    switch (key)
                    {
                        case "Bridge":
                            LWT.state =  LCARS.LCARS_Bridge_windowState;
                            LWT.position =  LCARS.LCARS_Bridge_windowPosition ;
                            LWT.damagable = false;
                            break;
                        case "Helm":
                            LWT.state =  LCARS.LCARS_Helm_windowState ;
                            LWT.position = LCARS.LCARS_Helm_windowPosition;
                            LWT.damagable = true;
                            break;
                        case "Engineering":
                            LWT.state =  LCARS.LCARS_Engineering_windowState ;
                            LWT.position = LCARS.LCARS_Engineering_windowPosition;
                            LWT.damagable = false;
                            break;
                        case "Tactical":
                            LWT.state =  LCARS.LCARS_Tactical_windowState ;
                            LWT.position = LCARS.LCARS_Tactical_windowPosition;
                            LWT.damagable = true;
                            break;
                        case "Science":
                            LWT.state =  LCARS.LCARS_Science_windowState ;
                            LWT.position = LCARS.LCARS_Science_windowPosition;
                            LWT.damagable = true;
                            break;
                        case "Communication":
                            LWT.state =  LCARS.LCARS_Communication_windowState;
                            LWT.position = LCARS.LCARS_Communication_windowPosition;
                            LWT.damagable = true;
                            break;
                        default:
                            LWT.state = false;
                            LWT.position = new Rect(20, 20, 320, 50);
                            LWT.damagable = false;
                            break;
                    }
                    LWT.windowID = rnd.Next();
                    LWT.wf = lwt[key];
                    LCARS.lWindows.LCARSWindows.Add(LWT.name, LWT);

                    LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                    ShipSystem.name = key;
                    ShipSystem.vessel = thisVessel;
                    ShipSystem.disabled = false;
                    ShipSystem.damaged = false;
                    ShipSystem.damagable = LWT.damagable;
                    ShipSystem.integrity = 100;
                    ShipSystem.powerSystem_consumption_current = 0f;
                    ShipSystem.powerSystem_consumption_total = 0f;
                    ShipSystem.powerSystem_takerType = "MainSystem";
                    ShipSystem.powerSystem_takerSubType = key;
                    ShipSystem.powerSystem_L1_usage = 10f;
                    ShipSystem.powerSystem_L2_usage = 0f;
                    ShipSystem.powerSystem_L3_usage = 0f;
                    LCARS.lODN.ShipSystems.Add(key, ShipSystem);
                    //LCARS.lPowSys.setPowerTaker("Station: " + key, "MainSystem", key, 10, 0, 0);
                }
                LCARS.lODN.ShipSystems["Bridge"].damagable = false;
                LCARS.lODN.ShipSystems["Bridge"].show_in_MODNJ = false;
                LCARS.lODN.ShipSystems["Engineering"].damagable = false;
                LCARS.lODN.ShipSystems["Engineering"].show_in_MODNJ = false;
                LCARS.lODN.ShipSystems["Communication"].powerSystem_L2_usage = 40f;
            }

        }

        internal void setWindowState(string WindowKey, bool state)
        {
            UnityEngine.Debug.Log("LCARS_Windows_Util setWindowState WindowKey=" + WindowKey + " state=" + state);
            if (LCARS.lWindows.LCARSWindows.ContainsKey(WindowKey))
            {

                if(state)
                {
                    try
                    {
                        LCARS.lAudio.play("LCARS_SubsystemOpen", Camera.main.transform);
                        UnityEngine.Debug.Log("LCARS_Windows_Util lAudio.play WindowKey=" + WindowKey + " state=" + state);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        LCARS.lAudio.play("LCARS_SubsystemClose", Camera.main.transform);
                        UnityEngine.Debug.Log("LCARS_Windows_Util lAudio.play WindowKey=" + WindowKey + " state=" + state);
                    }
                    catch { }
                }

                LCARS.lWindows.LCARSWindows[WindowKey].state = state;
            }
        }

        internal void DrawWindows()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT || LCARS == null)
            { return; }



            if (!LCARS.WindowState && !LCARS.WindowState_bypass)
            { return; }

            if(myStyle==null)
            {
            myStyle = new GUIStyle();
            myStyle.margin = new RectOffset(0, 0, 0, 0);
            myStyle.padding = new RectOffset(0, 0, 0, 0);
            myStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            myStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            myStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            myStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            }




            foreach (string key in LCARS.lWindows.LCARSWindows.Keys)
            {
                if (!LCARS.lODN.ShipSystems[key].isNominal)
                {
                    continue;
                }
                if (LCARS.lWindows.LCARSWindows[key].state)
                {
                    LCARS.lWindows.LCARSWindows[key].position = LCARS_Utilities.ClampToScreen(GUILayout.Window(LCARS.lWindows.LCARSWindows[key].windowID, LCARS.lWindows.LCARSWindows[key].position, LCARS.lWindows.LCARSWindows[key].wf, LCARS.lWindows.LCARSWindows[key].title, myStyle));
                }
                else 
                {
                    LCARS.lODN.ShipSystems[key].powerSystem_consumption_current = 0f;
                }
            }
        }

        private void Bridge_win(int id)
        {
            LCARS.lPowSys.draw("Bridge", LCARS.lODN.ShipSystems["Bridge"].powerSystem_L1_usage);
            LCARS.lStationBridge.getGUI();
            GUI.DragWindow();
        }

        private void Helm_win(int id)
        {
            LCARS.lPowSys.draw("Helm", LCARS.lODN.ShipSystems["Helm"].powerSystem_L1_usage);
            LCARS.lStationHelm.getGUI();
            GUI.DragWindow();
        }

        private void Engineering_win(int id)
        {
            LCARS.lPowSys.draw("Engineering", LCARS.lODN.ShipSystems["Engineering"].powerSystem_L1_usage);
            LCARS.lStationEng.getGUI();
            GUI.DragWindow();
        }

        private void Tactical_win(int id)
        {
            LCARS.lPowSys.draw("Tactical", LCARS.lODN.ShipSystems["Tactical"].powerSystem_L1_usage);
            LCARS.lStationTac.getGUI();
            GUI.DragWindow();
        }

        private void Science_win(int id)
        {
            LCARS.lPowSys.draw("Science", LCARS.lODN.ShipSystems["Science"].powerSystem_L1_usage);
            LCARS.lStationSci.getGUI();
            GUI.DragWindow();
        }

        private void Communication_win(int id)
        {
            LCARS.lPowSys.draw("Communication", LCARS.lODN.ShipSystems["Communication"].powerSystem_L1_usage);
            LCARS.lStationComm.getGUI();
            GUI.DragWindow();
        }


    }
}
