using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Helm_Station
    {
        private GUIStyle Helm_BackGroundLayoutStyle = null;
        private GUIStyle SliderStyle;
        private GUIStyle toggle_style = null;
        private GUIStyle caption_style = null;
        private GUIStyle TextField_style = null;

        
        Rect ProgradeStabilizer_togglePosition = new Rect(7, 42, 153, 20);
        Rect SlowDown_togglePosition = new Rect(7, 61, 153, 21);
        Rect SlowToSave_togglePosition = new Rect(160, 23, 151, 20);
        Rect Inertia_togglePosition = new Rect(160, 42, 151, 20);
        Rect FullHalt_togglePosition = new Rect(160, 61, 151, 21);
        Rect AccellerationLock_togglePosition = new Rect(160, 88, 151, 20);
        Rect HoldSpeed_togglePosition = new Rect(7, 107, 153, 20);
        Rect HoldHeight_togglePosition = new Rect(7, 127, 153, 21);
        
        Rect HoldSpeed_valuePosition = new Rect(168, 110, 80, 16);
        Rect HoldHeight_valuePosition = new Rect(168, 130, 80, 16);

        Rect mainslider_buttonPosition = new Rect(35, 160, 250, 250);
        Rect forward_buttonPosition = new Rect(7, 160, 23, 250);
        Rect side_buttonPosition = new Rect(35, 411, 250, 23);
        Rect up_buttonPosition = new Rect(288, 160, 23, 250);
        float deadzone_pixel = 10;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float Slider_current_minmax = 125;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }

        Dictionary<string, bool> audioLibraryControllerList = new Dictionary<string, bool>();
        /*
        private void AudioWrapper(string key, bool value, string soundFile, string key_2 = "")
        {
            try
            {
                string key2 = "";
                key2 = (key_2 == "") ? key : key_2;
                if (!audioLibraryControllerList.ContainsKey(key2)) { audioLibraryControllerList.Add(key2, !value); }
                if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key2])
                {
                    UnityEngine.Debug.Log("### LCARS_Helm_Station AudioWrapper value=" + value + "  key=" + key + "  key2=" + key2 + " LCARS.lODN.ShipSystems[key2].disabled=" + LCARS.lODN.ShipSystems[key2].disabled + " LCARS.lODN.ShipSystems[key2].damaged=" + LCARS.lODN.ShipSystems[key2].damaged);
                    
                    if (LCARS.lODN.ShipSystems[key2].disabled)
                    { LCARS.lAudio.play("LCARS_computer_error", LCARS.thisVessel); return; }

                    if (LCARS.lODN.ShipSystems[key2].damaged)
                    { LCARS.lAudio.play("LCARS_computer_error2", LCARS.thisVessel); return; }

                    if (value)
                    { LCARS.lAudio.play(soundFile, LCARS.thisVessel); }
                    else { LCARS.lAudio.play(soundFile, LCARS.thisVessel); }
                    
                    audioLibraryControllerList[key2] = value;
                }
                key_2 = "";
                key2 = "";
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.Log("### LCARS_Helm_Station AudioWrapper <"+key+"> error ex=" + ex);
            }
        }
        */
        string key = "";
        bool value = false;
        string soundfile_on = "LCARS_helmBeeb";
        string soundfile_off = "LCARS_helmBeeb";
        string soundfile_error1 = "LCARS_computer_error";
        string soundfile_error2 = "LCARS_computer_error2";
        public void getGUI()
        {


            if (TextField_style == null)
            {
                TextField_style = new GUIStyle(GUI.skin.textField);
                TextField_style.normal.textColor = Color.yellow;
                TextField_style.active.textColor = Color.yellow;
                TextField_style.padding = new RectOffset(0, -2, 0, 0) ;
                TextField_style.fontSize = 10;
            }

            if (Helm_BackGroundLayoutStyle == null)
            {
                Helm_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Helm_BackGroundLayoutStyle.alignment = TextAnchor.MiddleCenter;
                Helm_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
                Helm_BackGroundLayoutStyle.padding = new RectOffset(0, 0, 0, 0);
                Helm_BackGroundLayoutStyle.fixedWidth = 320;
                Helm_BackGroundLayoutStyle.fixedHeight = 443;
                Helm_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Background", false);
                Helm_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Background", false);
                Helm_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Background", false);
                Helm_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Background", false);
            }
            if (SliderStyle == null)
            {
                SliderStyle = new GUIStyle();
                SliderStyle.alignment = TextAnchor.MiddleCenter;
                SliderStyle.padding = new RectOffset(0, 0, 0, 0);
                SliderStyle.margin = new RectOffset(0, 0, 0, 0);
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
            if (caption_style == null)
            {
                caption_style = new GUIStyle();
                //caption_style.alignment = TextAnchor.MiddleCenter;
                //caption_style.padding = new RectOffset(0, 0, 0, 0);
                caption_style.margin = new RectOffset(20, 0, -4, 0);
                caption_style.fontSize = 12;
                caption_style.fixedHeight = 15;
                //caption_style.imagePosition = ImagePosition.ImageOnly;
                caption_style.normal.textColor = Color.white;
                caption_style.active.textColor = Color.white;
                //caption_style.fontStyle = FontStyle.Italic;
            }

            GUILayout.BeginVertical(Helm_BackGroundLayoutStyle, GUILayout.Width(320), GUILayout.Height(443));
            GUILayout.Label("");

            GUIContent content = new GUIContent("");
            toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 74;
            toggle_style.fixedHeight = 11;

            if (GUI.Button(new Rect(264, 5, 50, 8), content, toggle_style))
            {
                LCARS.lWindows.setWindowState("Helm", false);
            }


            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Prograde_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Prograde", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["ProgradeStabilizer"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Prograde_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["ProgradeStabilizer"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Prograde_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 153;
            toggle_style.fixedHeight = 20;
            LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer = GUI.Toggle(ProgradeStabilizer_togglePosition, LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer, "", toggle_style);
            key = "ProgradeStabilizer";
            value = LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, Camera.main.transform); }
                else { LCARS.lAudio.play(soundfile_off, Camera.main.transform); }
            }
            audioLibraryControllerList[key] = value;

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_SlowDown) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowDown_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowDown", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["SlowDown"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowDown_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["SlowDown"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowDown_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 153;
            toggle_style.fixedHeight = 21;
            LCARS.lODN.ShipStatus.LCARS_SlowDown = false;
            if (GUI.RepeatButton(SlowDown_togglePosition, "", toggle_style))
            {
                LCARS.lODN.ShipStatus.LCARS_SlowDown = true;
                if (!audioLibraryControllerList.ContainsKey("SlowDown")) { audioLibraryControllerList.Add("SlowDown", false); }
                if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList["SlowDown"])
                {
                    if (LCARS.lODN.ShipSystems["SlowDown"].disabled)
                    { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                    if (LCARS.lODN.ShipSystems["SlowDown"].damaged)
                    { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                    LCARS.lAudio.play(soundfile_on, Camera.main.transform);
                }
            }
            else 
            {
                audioLibraryControllerList["SlowDown"] = false;
            }

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowSave_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowSave", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["MakeSlowToSave"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowSave_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["MakeSlowToSave"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_SlowSave_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 151;
            toggle_style.fixedHeight = 20;
            LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave = GUI.Toggle(SlowToSave_togglePosition, LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave, "", toggle_style);
            key = "MakeSlowToSave";
            value = LCARS.lODN.ShipStatus.LCARS_MakeSlowToSave;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, Camera.main.transform); }
                else { LCARS.lAudio.play(soundfile_off, Camera.main.transform); }
            }
            audioLibraryControllerList[key] = value;

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_InertiaDamper) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Inertia_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Inertia", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["InertiaDamper"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Inertia_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["InertiaDamper"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_Inertia_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 151;
            toggle_style.fixedHeight = 20;
            LCARS.lODN.ShipStatus.LCARS_InertiaDamper = GUI.Toggle(Inertia_togglePosition, LCARS.lODN.ShipStatus.LCARS_InertiaDamper, "", toggle_style);
            key = "InertiaDamper";
            value = LCARS.lODN.ShipStatus.LCARS_InertiaDamper;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, Camera.main.transform); }
                else { LCARS.lAudio.play(soundfile_off, Camera.main.transform); }
            }
            audioLibraryControllerList[key] = value;

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_FullHalt) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_FullHalt_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_FullHalt", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["FullHalt"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_FullHalt_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["FullHalt"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_FullHalt_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 151;
            toggle_style.fixedHeight = 21;
            LCARS.lODN.ShipStatus.LCARS_FullHalt = GUI.Toggle(FullHalt_togglePosition, LCARS.lODN.ShipStatus.LCARS_FullHalt, "", toggle_style);
            if (LCARS.lODN.ShipStatus.LCARS_FullHalt)
            {
                LCARS.lODN.ShipStatus.LCARS_AccelerationLock = false;
                LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer = false;
                LCARS.lODN.ShipStatus.LCARS_HoldHeight = false;
                LCARS.lODN.ShipStatus.LCARS_HoldSpeed = false;
            }
            key = "FullHalt";
            value = LCARS.lODN.ShipStatus.LCARS_FullHalt;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, Camera.main.transform); }
                else { LCARS.lAudio.play(soundfile_off, Camera.main.transform); }
            }
            audioLibraryControllerList[key] = value;

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_AccelerationLock) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_LockAccelleration_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_LockAccelleration", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["AccelerationLock"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_LockAccelleration_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["AccelerationLock"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_LockAccelleration_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 151;
            toggle_style.fixedHeight = 20;
            LCARS.lODN.ShipStatus.LCARS_AccelerationLock = GUI.Toggle(AccellerationLock_togglePosition, LCARS.lODN.ShipStatus.LCARS_AccelerationLock, "", toggle_style);
            key = "AccelerationLock";
            value = LCARS.lODN.ShipStatus.LCARS_AccelerationLock;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, Camera.main.transform); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, Camera.main.transform); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, Camera.main.transform); }
                else { LCARS.lAudio.play(soundfile_off, Camera.main.transform); }
            }
            audioLibraryControllerList[key] = value;


            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_HoldSpeed) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldSpeed_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldSpeed", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["HoldSpeed"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldSpeed_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["HoldSpeed"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldSpeed_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 153;
            toggle_style.fixedHeight = 20;
            if(GUI.Button(HoldSpeed_togglePosition, "", toggle_style))
            {
                LCARS.lODN.ShipStatus.LCARS_HoldSpeed = !LCARS.lODN.ShipStatus.LCARS_HoldSpeed;
                key = "HoldSpeed";
                value = LCARS.lODN.ShipStatus.LCARS_HoldSpeed;
                if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
                if (value != audioLibraryControllerList[key])
                {
                    if (LCARS.lODN.ShipSystems[key].disabled)
                    { LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); return; }

                    if (LCARS.lODN.ShipSystems[key].damaged)
                    { LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); return; }

                    if (value)
                    { LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                    else { LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                }
                audioLibraryControllerList[key] = value;
            }
            if (!LCARS.lODN.ShipStatus.LCARS_HoldSpeed)
            {
                LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value = thisVessel.horizontalSrfSpeed;
            }
            else
            {
                LCARS.lODN.ShipStatus.LCARS_AccelerationLock = false;
                toggle_style.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
                toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
                if (GUI.RepeatButton(new Rect(163, 109, 17, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value -= 10; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(181, 109, 13, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value -= 5; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(196, 109, 10, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value -= 1; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                GUI.Label(new Rect(208, 111, 48, 12), " "+Math.Round(LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value,2) + "m/s", TextField_style);
                if (GUI.RepeatButton(new Rect(259, 109, 10, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value += 1; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(270, 109, 13, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value += 5; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(286, 109, 17, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldSpeed_value += 10; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
            }


            //TextField_style.fontSize = 14;
            //LCARS.LCARS_HoldSpeed_value = Double.Parse(GUI.TextField(HoldSpeed_valuePosition, Math.Round(LCARS.LCARS_HoldSpeed_value, 2).ToString(), 45, TextField_style));

            toggle_style.active.background = (LCARS.lODN.ShipStatus.LCARS_HoldHeight) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldHeight_active", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldHeight", false);
            toggle_style.active.background = (LCARS.lODN.ShipSystems["HoldHeight"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldHeight_disabled", false) : toggle_style.active.background;
            toggle_style.active.background = (LCARS.lODN.ShipSystems["HoldHeight"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_HoldHeight_damaged", false) : toggle_style.active.background;
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 153;
            toggle_style.fixedHeight = 21;
            LCARS.lODN.ShipStatus.LCARS_HoldHeight = GUI.Toggle(HoldHeight_togglePosition, LCARS.lODN.ShipStatus.LCARS_HoldHeight, "", toggle_style);
            if (!LCARS.lODN.ShipStatus.LCARS_HoldHeight)
            {
                Vector3 CoM = thisVessel.findWorldCenterOfMass();
                double altitudeASL = thisVessel.mainBody.GetAltitude(CoM);
                LCARS.lODN.ShipStatus.LCARS_HoldHeight_value = altitudeASL;
            }
            else 
            {
                LCARS.lODN.ShipStatus.LCARS_ProgradeStabilizer = false;
                LCARS.lODN.ShipStatus.LCARS_AccelerationLock = false;
                toggle_style.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
                toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
                if (GUI.RepeatButton(new Rect(163, 128, 17, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value -= 10; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(181, 128, 13, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value -= 5; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(196, 128, 10, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value -= 1; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                GUI.Label(new Rect(208, 130, 48, 12), " " + Math.Round(LCARS.lODN.ShipStatus.LCARS_HoldHeight_value, 2) + "m", TextField_style);
                if (GUI.RepeatButton(new Rect(259, 128, 10, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value += 1; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(270, 128, 13, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value += 5; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
                if (GUI.RepeatButton(new Rect(286, 128, 17, 16), "", toggle_style)) { LCARS.lODN.ShipStatus.LCARS_HoldHeight_value += 10; LCARS.lAudio.play("LCARS_keyPress", FlightGlobals.ActiveVessel); }
            }
            key = "HoldHeight";
            value = LCARS.lODN.ShipStatus.LCARS_HoldHeight;
            if (!audioLibraryControllerList.ContainsKey(key)) { audioLibraryControllerList.Add(key, !value); }
            if (Input.GetMouseButtonUp(0) && value != audioLibraryControllerList[key])
            {
                if (LCARS.lODN.ShipSystems[key].disabled)
                { LCARS.lAudio.play(soundfile_error1, FlightGlobals.ActiveVessel); return; }

                if (LCARS.lODN.ShipSystems[key].damaged)
                { LCARS.lAudio.play(soundfile_error2, FlightGlobals.ActiveVessel); return; }

                if (value)
                { LCARS.lAudio.play(soundfile_on, FlightGlobals.ActiveVessel); }
                else { LCARS.lAudio.play(soundfile_off, FlightGlobals.ActiveVessel); }
            }
            audioLibraryControllerList[key] = value;

            //TextField_style.fontSize = 14;
            //LCARS.LCARS_HoldHeight_value = Double.Parse(GUI.TextField(HoldHeight_valuePosition, Math.Round(LCARS.LCARS_HoldHeight_value, 2).ToString(), 45, TextField_style));



            
            //x = getMousePosition().x - LCARS.lWindows.LCARSWindows["Helm"].position.x - mainslider_buttonPosition.x - 125;
            //y = getMousePosition().y - (Screen.height - LCARS.lWindows.LCARSWindows["Helm"].position.y - mainslider_buttonPosition.y - 125);


            /*
                if (Event.current.type == EventType.Repaint)
                {
                    if(mainslider_buttonPosition.Contains(Event.current.mousePosition))
                    {
                        GUILayout.Label("mainslider Mouse over!");
                    }
                    if (forward_buttonPosition.Contains(Event.current.mousePosition))
                    {
                        GUILayout.Label("forward_button Mouse over!");
                    }
                    if (side_buttonPosition.Contains(Event.current.mousePosition))
                    {
                        GUILayout.Label("side_button Mouse over!");
                    }
                    if (up_buttonPosition.Contains(Event.current.mousePosition))
                    {
                        GUILayout.Label("up_button Mouse over!");
                    }
                }
            */



            
            
            bool button_down = false;
            SliderStyle.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_mainslider_nominal", false);
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_mainslider_disabled", false) : SliderStyle.active.background;
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_mainslider_damaged", false) : SliderStyle.active.background;
            SliderStyle.normal.background = SliderStyle.active.background;
            SliderStyle.fixedWidth = 250;
            SliderStyle.fixedHeight = 250;
            if (GUI.RepeatButton(mainslider_buttonPosition, "", SliderStyle))
            {
                LCARS.LCARS_thrust_x = clamp_value(get_mainslider_x(), Slider_current_minmax, deadzone_pixel);
                LCARS.LCARS_thrust_y = clamp_value(get_mainslider_y(), Slider_current_minmax, deadzone_pixel);
                LCARS.backup_LCARS_thrust_x = LCARS.LCARS_thrust_x;
                LCARS.backup_LCARS_thrust_y = LCARS.LCARS_thrust_y;
                button_down = true;
                //AudioWrapper("thrust_xy", true, "LCARS_keyPress","PropulsionMatrix");

            }
            SliderStyle.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_yslider_nominal", false);
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_yslider_disabled", false) : SliderStyle.active.background;
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_yslider_damaged", false) : SliderStyle.active.background;
            SliderStyle.normal.background = SliderStyle.active.background;
            SliderStyle.fixedWidth = 24;
            SliderStyle.fixedHeight = 250;
            if (GUI.RepeatButton(forward_buttonPosition, "", SliderStyle))
            {
                LCARS.LCARS_thrust_y = clamp_value(get_subslider_y(), Slider_current_minmax, deadzone_pixel);
                LCARS.backup_LCARS_thrust_y = LCARS.LCARS_thrust_y;
                button_down = true;
                //AudioWrapper("thrust_y", true, "LCARS_keyPress", "PropulsionMatrix");
            }
            SliderStyle.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_xslider_nominal", false);
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_xslider_disabled", false) : SliderStyle.active.background;
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_xslider_damaged", false) : SliderStyle.active.background;
            SliderStyle.normal.background = SliderStyle.active.background;
            SliderStyle.fixedWidth = 250;
            SliderStyle.fixedHeight = 24;
            if (GUI.RepeatButton(side_buttonPosition, "", SliderStyle))
            {
                LCARS.LCARS_thrust_x = clamp_value(get_subslider_x(), Slider_current_minmax, deadzone_pixel);
                LCARS.backup_LCARS_thrust_x = LCARS.LCARS_thrust_x;
                button_down = true;
                //AudioWrapper("thrust_x", true, "LCARS_keyPress", "PropulsionMatrix");
            }
            SliderStyle.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_zslider_nominal", false);
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].disabled) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_zslider_disabled", false) : SliderStyle.active.background;
            SliderStyle.active.background = (LCARS.lODN.ShipSystems["PropulsionMatrix"].damaged) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Helm_Button_zslider_damaged", false) : SliderStyle.active.background;
            SliderStyle.normal.background = SliderStyle.active.background;
            SliderStyle.fixedWidth = 24;
            SliderStyle.fixedHeight = 250;
            if (GUI.RepeatButton(up_buttonPosition, "", SliderStyle))
            {
                LCARS.LCARS_thrust_z = clamp_value(get_subslider_z(), Slider_current_minmax, deadzone_pixel);
                LCARS.backup_LCARS_thrust_z = LCARS.LCARS_thrust_z;
                button_down = true;
                //AudioWrapper("thrust_z", true, "LCARS_keyPress", "PropulsionMatrix");
            }
            if (!button_down)
            {
                LCARS.LCARS_thrust_x = 0f;
                LCARS.LCARS_thrust_y = 0f;
                LCARS.LCARS_thrust_z = 0f;
                audioLibraryControllerList["thrust_xy"] = false;
                audioLibraryControllerList["thrust_y"] = false;
                audioLibraryControllerList["thrust_x"] = false;
                audioLibraryControllerList["thrust_z"] = false;
            }
            else 
            {
            }

            //GUI.Label(new Rect(230, 164, 80, 15), "x: " + clamp_value(get_mainslider_x(), Slider_current_minmax, deadzone_pixel) + " / " + LCARS.current_LCARS_thrust_x, caption_style);
            //GUI.Label(new Rect(230, 180, 80, 15), "y: " + clamp_value(get_mainslider_y(), Slider_current_minmax, deadzone_pixel) + " / " + LCARS.current_LCARS_thrust_y, caption_style);
            //GUI.Label(new Rect(230, 196, 80, 15), "z: " + clamp_value(get_subslider_z(), Slider_current_minmax, deadzone_pixel) + " / " + LCARS.current_LCARS_thrust_z, caption_style);

            GUI.Label(new Rect(230, 164, 80, 15), "x: " + LCARS.lODN.ShipStatus.current_LCARS_thrust_x + "Kp", caption_style);
            GUI.Label(new Rect(230, 180, 80, 15), "y: " + LCARS.lODN.ShipStatus.current_LCARS_thrust_y + "Kp", caption_style);
            GUI.Label(new Rect(230, 196, 80, 15), "z: " + LCARS.lODN.ShipStatus.current_LCARS_thrust_z + "Kp", caption_style);

            GUILayout.EndVertical();
        
        }

        Vector3 getMousePosition()
        {
            return UnityEngine.Input.mousePosition;
        }
        float get_mainslider_x()
        {
            return getMousePosition().x - LCARS.lWindows.LCARSWindows["Helm"].position.x - mainslider_buttonPosition.x - (mainslider_buttonPosition.width/2);
        }

        float get_mainslider_y()
        {
            return getMousePosition().y - (Screen.height - LCARS.lWindows.LCARSWindows["Helm"].position.y - mainslider_buttonPosition.y - (mainslider_buttonPosition.height / 2));
        }

        float get_subslider_x()
        {
            return getMousePosition().x - LCARS.lWindows.LCARSWindows["Helm"].position.x - side_buttonPosition.x - (side_buttonPosition.width / 2);
        }

        float get_subslider_y()
        {
            return getMousePosition().y - (Screen.height - LCARS.lWindows.LCARSWindows["Helm"].position.y - forward_buttonPosition.y - (forward_buttonPosition.height / 2));
        }
        float get_subslider_z()
        {
            return getMousePosition().y - (Screen.height - LCARS.lWindows.LCARSWindows["Helm"].position.y - up_buttonPosition.y - (up_buttonPosition.height / 2));
        }
        float clamp_value(float value, float minmax, float deadzone)
        {
            value = (value >= minmax) ? minmax : value;
            value = (value <= -minmax) ? -minmax : value;

            value = (deadzone >= value && value >= 1) ? 0 : value;
            value = (-1 >= value && value >= -deadzone) ? 0 : value;

            value = (-deadzone >= value) ? value + deadzone : value;
            value = (deadzone <= value) ? value - deadzone : value;
            return value;
        }


    }
}
