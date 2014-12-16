﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{





    public class LCARS_NCIMC
    {
        public LCARS_NCI NCI;

        public bool NCIMC_WindowState = false;
        private Rect NCIMC_windowPosition = new Rect(120, 120, 550, 650);
        private static System.Random rnd = new System.Random();
        private int NCIMC_windowID = rnd.Next();

        
        internal void setWindowState(bool state)
        {
            //UnityEngine.Debug.Log("### NCIMC setWindowState");
            NCIMC_WindowState = state;
        }
        internal bool getWindowState()
        {
            //UnityEngine.Debug.Log("### NCIMC getWindowState");
            return NCIMC_WindowState;
        }

        internal void OnGUI()
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            { return; }

            if (NCI == null)
            {
                NCI = LCARS_NCI.Instance;
                LCARS_NCI.Instance.init();

            }
            if (LCARS_NCI.Instance.Data.Naturals_List.Count < 1)
            {
                LCARS_NCI.Instance.Data.Naturals_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (LCARS_NCI.Instance.Data.Mines_List.Count < 1)
            {
                LCARS_NCI.Instance.Data.Mines_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (LCARS_NCI.Instance.Data.Stations_List.Count < 1)
            {
                LCARS_NCI.Instance.Data.Stations_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (LCARS_NCI.Instance.Data.CelestialBody_List.Count < 1)
            {
                LCARS_NCI.Instance.Data.CelestialBody_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            
            LCARS_NCI.Instance.Data.Gather_Object_Lists();

            //UnityEngine.Debug.Log("### NCIMC OnGUI begin NCIMC_WindowState = " + NCIMC_WindowState);

            if (!NCIMC_WindowState)
            { return; }

            if (HighLogic.LoadedSceneIsEditor)
            { return; }
            //UnityEngine.Debug.Log("### NCIMC OnGUI 1 ");



            NCIMC_windowPosition = LCARS_Utilities.ClampToScreen(GUILayout.Window(NCIMC_windowID, NCIMC_windowPosition, NCIMC_Window, ""));
            //UnityEngine.Debug.Log("### NCIMC OnGUI end ");

        }

        int GUI_Mode = 0;
        private GUIStyle style_TextField;
        private GUIStyle style2_TextField;
        private GUIStyle style3_TextField;

        private void NCIMC_Window(int windowID)
        {
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 1 ");
            if (HighLogic.LoadedSceneIsEditor)
                return;

            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 2 ");
            if (!NCIMC_WindowState)
                return;
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 3 ");

            if (style_TextField == null)
            {
                style_TextField = new GUIStyle(GUI.skin.textField);
                style_TextField.fixedWidth = 200;
            }
            if (style2_TextField == null)
            {
                style2_TextField = new GUIStyle(GUI.skin.textField);
                style2_TextField.fixedWidth = 100;
            }
            if (style3_TextField == null)
            {
                style3_TextField = new GUIStyle(GUI.skin.textField);
                style3_TextField.fixedWidth = 150;
            }

            GUILayout.BeginVertical();
            GUILayout.Label("NCI Mission Creator");

            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 4 ");

                if (GUI_Mode != 0)
                {
                    //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 5 ");
                    if (GUILayout.Button("Back"))
                    {
                        GUI_Mode = 0;
                    }
                    //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 6 ");
                    switch (GUI_Mode)
                    {
                        case 1:
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 7 ");
                            GUILayout.Label(":: List Existing Missions ::");
                            listMissionMainGUI();
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 8 ");
                            break;
                        case 2:
                            GUILayout.Label(":: Mission Editor GUI ::");
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 9 ");
                            createMissionMainGUI();
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 10 ");
                            break;
                    }
                }
                else
                {
                    //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 11 ");
                    if (GUILayout.Button("Mission List"))
                    {
                        GUI_Mode = 1;
                    }
                    //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 12 ");

                    if (GUILayout.Button("Mission GUI"))
                    {
                        GUI_Mode = 2;
                    }
                    //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 13 ");
                }
                //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 14 ");
            GUILayout.EndVertical();
                //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 15 ");

            GUI.DragWindow();
            //UnityEngine.Debug.Log("### NCIMC NaturalCauseInc_Window 16 ");
        }

#region VARs

        LCARS_NCI_Mission mission;
        string group_selected="";
        Dictionary<string, LCARS_NCI_Object> current_list;

        string newArtefact_idcode="";
        string newArtefact_name="";
        string newArtefact_description="";
        string newArtefact_icon="";
        bool newArtefact_isDamagable=false;
        string newArtefact_integrity="";
        //string newArtefact_usage_mode="";
        string newArtefact_usage_amount="";
        string newArtefact_powerconsumption="";

        string newPersonality_idcode = "";
        string newPersonality_name = "";
        string newPersonality_description = "";
        string newPersonality_portrait = "";

        private bool formtoggle_Mission_Assets = true;
        private bool formtoggle_core_data = false;
        private bool formtoggle_requirements = false;
        private bool formtoggle_equippment = false;
        private bool formtoggle_artefacts = false;
        private bool formtoggle_ObjectOptions = false;
        private bool formtoggle_SealDeal = false;
        private bool formtoggle_Personalities = false;
        private bool formtoggle_Mission_Story = false;
        private int ObjectOptions_selected = 0;

        private bool step_formtoggle_options = false;
        private bool step_formtoggle_conversation = false;
        private bool step_formtoggle_messages = false;
        private bool step_formtoggle_jobs = false;

        private bool message_formtoggle_options = false;
        private bool message_formtoggle_contents = false;
        private bool message_formtoggle_responses = false;
        int message_condition_distance_to = 0;

        private string Artefact_add_mode = "";
        private string Personality_add_mode = "";
        //private string story_edit_mode = "";
        private int story_edit_current_step = 0;
        private int story_edit_current_speech = 0;
        private int story_edit_current_message = 0;
        bool LCARS_Systems_NotificationText_needed = false;

        private bool add_equippment = false;

        GUIStyle scrollview_style1;
        GUIStyle scrollview_style2;
        Vector2 ScrollPosition1;
        Vector2 ScrollPosition2;
        Vector2 ScrollPosition3;
        //Vector2 ScrollPosition4;

        Texture2D icon_tex = null;
        string Personality_default_icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_noPic";
        string Artefact_default_icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_noPic";

        bool reset_mission = false;

        LCARS_NCI_Mission_Step Step = null;
#endregion


        private void createMissionMainGUI()
        {


#region GUI
            
            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI begin ");

            //LCARS_NCI_Eggs NCI_Egg_Array = new LCARS_NCI_Eggs();
            //NCI_Egg_Array.init();
            //LCARS_NCI_Personalities NCI_Personality_Array = new LCARS_NCI_Personalities();
            //NCI_Personality_Array.init();

            if (scrollview_style1 == null)
            {
                scrollview_style1 = new GUIStyle();
                scrollview_style1.fixedHeight = 440;
            }
            if (scrollview_style2 == null)
            {
                scrollview_style2 = new GUIStyle();
                scrollview_style2.fixedHeight = 250;
            }


            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 1 ");


            if (mission == null)
            {
                if (GUILayout.Button("Create a new Mission Object"))
                {
                    if (mission == null)
                    {
                        mission = new LCARS_NCI_Mission();

                        // assets
                        //mission.story_type = "";
                        mission.title = "Enter here your Title";
                        mission.filename = "validMissionFileName";
                        mission.description = "";
                        mission.skill_level = "Medium";
                        mission.creator = "";
                        mission.url = "";
                        mission.requirements = new LCARS_NCI_Mission_Requirement_Manager();
                        mission.requirements.list = new Dictionary<int, LCARS_NCI_Mission_Requirement>();
                        mission.artefacts = new LCARS_NCI_Mission_Artefacts();
                        mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
                        /*mission.sealDeal = new LCARS_NCI_Mission_SealDeal();
                        mission.sealDeal.SealDeal_messageID = "";
                        mission.sealDeal.CloseDeal_messageID = "";
                        mission.sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
                        mission.sealDeal.Gain.cash = "0";
                        mission.sealDeal.Gain.reputation = "0";
                        mission.sealDeal.Gain.science = "0";
                        mission.sealDeal.Gain.objects = new List<string>();*/
                        mission.personalities = new LCARS_NCI_Mission_Personalities();
                        mission.personalities.list = new List<LCARS_NCI_Personality>();
                        mission.equippment = new LCARS_NCI_Mission_Equippment_List();
                        mission.equippment.list = new Dictionary<int, LCARS_NCI_Equippment>();
                        mission.artefacts = new LCARS_NCI_Mission_Artefacts();
                        mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();

                        // story
                        mission.steps = new Dictionary<int, LCARS_NCI_Mission_Step>();

                    }
                }

            }
            else 
            {

#region The Mission

                if (mission.requirements == null)
                {
                    mission.requirements = new LCARS_NCI_Mission_Requirement_Manager();
                }
                if (mission.requirements.list == null)
                {
                    mission.requirements.list = new Dictionary<int, LCARS_NCI_Mission_Requirement>();
                }
                /*
                if (mission.sealDeal == null)
                {
                    mission.sealDeal = new LCARS_NCI_Mission_SealDeal();
                }
                if (mission.sealDeal.Gain == null)
                {
                    mission.sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
                    mission.sealDeal.Gain.cash = "0";
                    mission.sealDeal.Gain.reputation = "0";
                    mission.sealDeal.Gain.science = "0";
                }
                if (mission.sealDeal.Gain.objects == null)
                {
                    mission.sealDeal.Gain.objects = new List<string>();
                }
                */
                if (mission.artefacts == null)
                {
                    mission.artefacts = new LCARS_NCI_Mission_Artefacts();
                }
                if (mission.artefacts.list == null)
                {
                    mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
                }

                if (mission.personalities == null)
                {
                    mission.personalities = new LCARS_NCI_Mission_Personalities();
                }
                if (mission.personalities.list == null)
                {
                    mission.personalities.list = new List<LCARS_NCI_Personality>();
                }

                if (mission.equippment == null)
                {
                    mission.equippment = new LCARS_NCI_Mission_Equippment_List();
                }
                if (mission.equippment.list == null)
                {
                    mission.equippment.list = new Dictionary<int, LCARS_NCI_Equippment>();
                }

                if (mission.artefacts == null)
                {
                    mission.artefacts = new LCARS_NCI_Mission_Artefacts();
                }
                if (mission.artefacts.list == null)
                {
                    mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
                }


                //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 2 ");

#region The Mission Nav

                GUILayout.Label("Please complete the Assets and write your story!");
                GUILayout.BeginHorizontal(GUILayout.Width(420));
                    GUILayout.Label("Display: ");
                    if (GUILayout.Button("Mission Assets"))
                    {
                        formtoggle_Mission_Assets = true;
                        formtoggle_Mission_Story = false;
                    }
                    if (GUILayout.Button("Mission Story"))
                    {
                        formtoggle_Mission_Assets = false;
                        formtoggle_Mission_Story = true;
                    }
                GUILayout.EndHorizontal();
#endregion

                if (formtoggle_Mission_Assets)
                {
#region Mission_Assets
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre GUITop ");

#region Mission_Assets GUI Top
                    GUILayout.Label("********************************************************************");
                    GUILayout.Label("Mission Assets:  Please fill out all Asset-Sections below as needed!");
                        GUILayout.Label("Asset-Sections:");
                    GUILayout.BeginHorizontal();
                    if (formtoggle_core_data == false && formtoggle_requirements == false && formtoggle_equippment == false && formtoggle_artefacts == false && formtoggle_ObjectOptions == false && formtoggle_SealDeal == false && formtoggle_Personalities == false)
                    {
                        if (GUILayout.Button("CoreData"))
                        {
                            formtoggle_core_data = true;
                            formtoggle_requirements = false;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = false;
                        }
                        if (GUILayout.Button("Objects"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = true;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = false;
                        }
                        if (GUILayout.Button("Personalities"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = false;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = true;
                        }
                        if (GUILayout.Button("Artefacts"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = false;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = true;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = false;
                        }
                        if (GUILayout.Button("Equippment"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = false;
                            formtoggle_equippment = true;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = false;
                        }
                        if (GUILayout.Button("ObjectOptions"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = false;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = true;
                            formtoggle_SealDeal = false;
                            formtoggle_Personalities = false;
                        }
                        /*if (GUILayout.Button("SealDeal"))
                        {
                            formtoggle_core_data = false;
                            formtoggle_requirements = false;
                            formtoggle_equippment = false;
                            formtoggle_artefacts = false;
                            formtoggle_ObjectOptions = false;
                            formtoggle_SealDeal = true;
                            formtoggle_Personalities = false;
                        }*/
                    }
                    else 
                    {
                        if (formtoggle_core_data)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section Data:");
                            if (GUILayout.Button("Back")) { formtoggle_core_data = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_requirements)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section Requirements:");
                            if (GUILayout.Button("Back")) { formtoggle_requirements = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_Personalities)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section NCI-Personalities:");
                            if (GUILayout.Button("Back")) { formtoggle_Personalities = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_equippment)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section NCI-Equippment:");
                            if (GUILayout.Button("Back")) { formtoggle_equippment = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_artefacts)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section NCI-Artefacts:");
                            if (GUILayout.Button("Back")) { formtoggle_artefacts = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_ObjectOptions)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section Object Options:");
                            if (GUILayout.Button("Back")) { formtoggle_ObjectOptions = false; }
                            GUILayout.EndHorizontal();
                        }
                        if (formtoggle_SealDeal)
                        {
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Section SealDeal:  What can the player gain:");
                            if (GUILayout.Button("Back")) { formtoggle_SealDeal = false; }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("********************************************************************");


#endregion
                    scrollview_style1.fixedHeight = 440;
                    scrollview_style1.fixedWidth = 530;
                    GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(530));
                    ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre requirements ");

#region core data

                    if (formtoggle_core_data)
                    {

                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI core_data 1 ");
                       /*
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Define the Story Type:");
                        if (mission.story_type == "")
                        {
                            if (GUILayout.Button("Automatic Background Story ")) { mission.story_type = "AutomaticBackgroundStory"; }
                            if (GUILayout.Button("User Started Story ")) { mission.story_type = "UserStartedStory"; }
                        }
                        else
                        {
                            GUILayout.Label(mission.story_type);
                            if (GUILayout.Button("change")) { mission.story_type = ""; }
                        }
                        GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI core_data 2 ");
                        */

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Title:", GUILayout.Width(200));
                            mission.title = GUILayout.TextField(mission.title, 70, style_TextField);
                        GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI core_data 3 ");

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("filename:", GUILayout.Width(100));
                            mission.filename = GUILayout.TextField(mission.filename, 70, style_TextField);
                            GUILayout.Label(".mission_cfg");
                        GUILayout.EndHorizontal();
                            GUILayout.Label("filename rules: no spaces, no dashes, no underscores, no special chars, just make a valid unix style filename with a minimum length of 6 chars", GUILayout.Width(420));

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Description:", GUILayout.Width(200));
                            mission.description = GUILayout.TextArea(mission.description, 500, GUILayout.Width(200), GUILayout.Height(100));
                            GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 3 ");

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Skill Level:", GUILayout.Width(200));
                            if (mission.skill_level == "")
                            {
                                if (GUILayout.Button("None at all ")) { mission.skill_level = "None at all"; }
                                if (GUILayout.Button("Easy ")) { mission.skill_level = "Easy"; }
                                if (GUILayout.Button("Medium ")) { mission.skill_level = "Medium"; }
                                if (GUILayout.Button("Hard ")) { mission.skill_level = "Hard"; }
                                if (GUILayout.Button("Insane ")) { mission.skill_level = "Insane"; }
                                if (GUILayout.Button("Manley ")) { mission.skill_level = "Manley"; }
                            }
                            else
                            {
                                GUILayout.Label(mission.skill_level);
                                if (GUILayout.Button("change")) { mission.skill_level = ""; }
                            }
                            GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 3 ");

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Creator:", GUILayout.Width(200));
                            mission.creator = GUILayout.TextField(mission.creator, 70, style_TextField);
                        GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 4 ");

                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Forum url:", GUILayout.Width(200));
                            mission.url = GUILayout.TextField(mission.url, 1000, style_TextField);
                        GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 5 ");
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI core_data 4 ");
                    }
#endregion

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre requirements ");

#region Requirements


                 if (formtoggle_requirements)
                 {
                     UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 1 ");
                     //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 6 ");
            
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 7 ");

                    GUILayout.Label("Select NCI-Objects:");
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 10 ");

                    UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 2 ");
                    if (group_selected.Length < 1)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Please select a NCI-Objects Group:");
                        if (GUILayout.Button("Naturals" + LCARS_NCI.Instance.Data.Naturals_List.Count)) { group_selected = "naturals"; current_list = LCARS_NCI.Instance.Data.Naturals_List; }
                        if (GUILayout.Button("Stations" + LCARS_NCI.Instance.Data.Stations_List.Count)) { group_selected = "stations"; current_list = LCARS_NCI.Instance.Data.Stations_List; }
                        if (GUILayout.Button("Bodys " + LCARS_NCI.Instance.Data.CelestialBody_List.Count)) { group_selected = "CelestialBodys"; current_list = LCARS_NCI.Instance.Data.CelestialBody_List; }
                        if (GUILayout.Button("Hostiles" + LCARS_NCI.Instance.Data.Mines_List.Count)) { group_selected = "mines"; current_list = LCARS_NCI.Instance.Data.Mines_List; }
                        
                        GUILayout.EndHorizontal();
                        UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 3 ");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 4 ");
                        if (GUILayout.Button("Back")) { group_selected = ""; current_list = null; }
                        scrollview_style2.fixedHeight = 250;
                        GUILayout.BeginVertical(scrollview_style2);
                        ScrollPosition3 = GUILayout.BeginScrollView(ScrollPosition3);
                        foreach (KeyValuePair<string, LCARS_NCI_Object> pair in current_list)
                        {

                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 5 ");

                            GUILayout.BeginHorizontal();
                                GUILayout.BeginVertical();
                                    string objID = pair.Key;
                                    string partname = pair.Value.partname;
                                    if (GUILayout.Button(partname))
                                    {
                                        UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 6 ");
                                        mission.addRequirement(pair.Value);
                                        group_selected = "";
                                        UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 7 ");
                                    }
                                GUILayout.EndVertical();

                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                        UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 8 ");

                    }
                    UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 9 ");
                    UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 10 ");
                    GUILayout.Label("Required NCI-Objects Selected:");
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                        GUILayout.Label("*************************************************");
                        if (mission.requirements.list.Count==0)
                        {
                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 11 ");
                            GUILayout.Label("No NCI-Objects Selected");
                        }
                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                        {
                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 12 ");
                            LCARS_NCI_Mission_Requirement R = pair.Value;
                            GUILayout.BeginHorizontal();
                                GUILayout.Label(R.name, GUILayout.Width(100));
                                GUILayout.Label(R.description, GUILayout.Width(300));
                                if (GUILayout.Button("X")) { 
                                    mission.requirements.list.Remove(pair.Key);
                                    Dictionary<int, LCARS_NCI_Mission_Requirement> tmp = new Dictionary<int, LCARS_NCI_Mission_Requirement>();
                                    foreach (LCARS_NCI_Mission_Requirement tmpR in mission.requirements.list.Values)
                                    {
                                        tmpR.id = tmp.Count + 1;
                                        tmp.Add((tmp.Count + 1), tmpR);
                                    }
                                    mission.requirements.list = tmp;
                                }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 13 ");

                            bool isCelestialBody = false;
                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 14 ");
                            try
                            {
                                UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 15 ");
                                isCelestialBody = mission.requirements.list[R.id].isCelestialBody;
                            }
                            catch
                            {
                                UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 16 ");
                            }
                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 17 ");
                            if (isCelestialBody)
                            {
                                UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 18 ");
                                GUILayout.Label("Locations: " + mission.requirements.list[R.id].Locations.Count);
                            }
                            else
                            {
                                UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 19 ");
                                try
                                {
                                    GUILayout.Label("Action Points: " + mission.requirements.list[R.id].actionpoints.Count);
                                    GUILayout.Label("Doors: " + mission.requirements.list[R.id].doors.Count);
                                    //GUILayout.Label("GUIWindows: " + mission.requirements.list[R.id].GUIWindows.Count);
                                }
                                catch { UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 20 "); }
                            }
                            GUILayout.EndHorizontal();


                            UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 21 ");
                        }
                        GUILayout.Label("*************************************************");
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 22 ");

                 }


#endregion

                 //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre personalities ");

#region Personalities

                if (formtoggle_Personalities)
                {
                     //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 12 ");
                    switch (Personality_add_mode)
                    {
                        case "existing":
                            if (GUILayout.Button("Back"))
                            {
                                Personality_add_mode = "";
                            }
                            scrollview_style2.fixedHeight = 250;
                            GUILayout.BeginVertical(scrollview_style2);
                            ScrollPosition2 = GUILayout.BeginScrollView(ScrollPosition2);
                            foreach (KeyValuePair<string, LCARS_NCI_Personality> pair in LCARS_NCI.Instance.Data.Personality_List)
                            {
                                LCARS_NCI_Personality P = pair.Value;
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(P.name, GUILayout.Width(100));
                                GUILayout.Label(P.description, GUILayout.Width(300));
                                if (GUILayout.Button("Select")) { mission.personalities.list.Add(P); }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndScrollView();
                            GUILayout.EndVertical();
                            break;

                        case "new":
                            if (GUILayout.Button("Back"))
                            {
                                Personality_add_mode = "";
                            }
                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    GUILayout.Label("idcode", GUILayout.Width(100));
                                    GUILayout.Label("name", GUILayout.Width(100));
                                    GUILayout.Label("description", GUILayout.Width(100));
                                    GUILayout.Label("portrait", GUILayout.Width(100));
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    style2_TextField.fixedWidth = 100;
                                    newPersonality_idcode = GUILayout.TextField(newPersonality_idcode, 200, style2_TextField);
                                    newPersonality_name = GUILayout.TextField(newPersonality_name, 200, style2_TextField);
                                    newPersonality_description = GUILayout.TextField(newPersonality_description, 200, style2_TextField);
                                    newPersonality_portrait = GUILayout.TextField(newPersonality_portrait, 200, style2_TextField);
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(440));
                                if (GUILayout.Button("add Personality") && newPersonality_idcode!="")
                                    {
                                        LCARS_NCI_Personality P = new LCARS_NCI_Personality();
                                        P.idcode = newPersonality_idcode;
                                        P.name = newPersonality_name;
                                        P.description = newPersonality_description;
                                        P.portrait = newPersonality_portrait;
                                        mission.personalities.list.Add(P);
                                        LCARS_NCI.Instance.Data.Personalities.list.Add(P.idcode, P);
                                        newPersonality_idcode = "";
                                        newPersonality_name = "";
                                        newPersonality_description = "";
                                        newPersonality_portrait = "";
                                    }
                                GUILayout.EndHorizontal();
                            break;

                        default:
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                            GUILayout.Label("Add: ");
                            if (GUILayout.Button("Existing"))
                            {
                                Personality_add_mode = "existing";
                            }
                            if (GUILayout.Button("New"))
                            {
                                Personality_add_mode = "new";
                            }
                            GUILayout.EndHorizontal();
                            break;
                    }

                    GUILayout.BeginVertical();
                    GUILayout.Label("*************************************************");
                    if (mission.personalities.list.Count == 0)
                    {
                        GUILayout.Label("No NCI-Personalities Defined:");
                    }
                    foreach (LCARS_NCI_Personality P in mission.personalities.list)
                    {
                        bool use_default_portait = false;
                        try
                        {
                            icon_tex = GameDatabase.Instance.GetTexture(P.portrait, false);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_Personalities loaded icon_tex P.portrait=" + P.portrait);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_Personalities   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.Log("### NCIMC formtoggle_Personalities skipping icon_tex P.portrait=" + P.portrait + " - file not found ex=" + ex);
                            use_default_portait = true;
                        }
                        if (P.portrait == "")
                        {
                            use_default_portait = true;
                        }
                        if(use_default_portait)
                        {
                            icon_tex = GameDatabase.Instance.GetTexture(Personality_default_icon, false);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_Personalities loaded icon_tex Personality_default_icon=" + Personality_default_icon);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_Personalities   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                            GUILayout.Label("Warning: This Personality has no Picture!");
                        }
                        GUILayout.BeginHorizontal();
                        GUILayout.Button(icon_tex, GUILayout.Height(55), GUILayout.Width(50));
                        GUILayout.Label(P.name, GUILayout.Width(100));
                        GUILayout.Label(P.description, GUILayout.Width(250));
                        if (GUILayout.Button("X")) { mission.personalities.list.Remove(P); }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Label("*************************************************");
                    GUILayout.EndVertical();
                }
#endregion

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre Artefacts ");


#region Equippment
                    if (formtoggle_equippment)
                    {
                        if (add_equippment)
                        {
                            if (GUILayout.Button("Back"))
                            {
                                add_equippment = false;
                            }
                            scrollview_style2.fixedHeight = 250;
                            GUILayout.BeginVertical(scrollview_style2);
                            ScrollPosition2 = GUILayout.BeginScrollView(ScrollPosition2);
                            foreach (KeyValuePair<string, LCARS_NCI_Equippment> pair in LCARS_NCI.Instance.Data.Equippment_List)
                            {
                                LCARS_NCI_Equippment invItem = pair.Value;
                                //if (mission.equippment.list.ContainsKey(invItem.idcode))
                                //{continue;}

                                GUILayout.BeginHorizontal();
                                GUILayout.Label(invItem.name, GUILayout.Width(100));
                                GUILayout.Label(invItem.description, GUILayout.Width(300));
                                if (GUILayout.Button("Select")) 
                                {
                                    LCARS_NCI_Equippment P;
                                    P = new LCARS_NCI_Equippment();
                                    P.name = invItem.idcode;
                                    P.description = invItem.idcode;
                                    P.idcode = invItem.idcode;
                                    P.icon = invItem.idcode;
                                    mission.addEquippment(P);
                                    //mission.equippment.list.Add(mission.equippment.list.Count+1, invItem); 
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndScrollView();
                            GUILayout.EndVertical();

                        }
                        else 
                        {
                            if (GUILayout.Button("Add NCI Equippment")) { add_equippment = true; }

                        }

                        GUILayout.BeginVertical();

                        GUILayout.Label("********************************************************************");
                        if (mission.equippment.list.Count == 0)
                        {
                            GUILayout.Label("No NCI-Equippment Selected");
                        }
                        foreach (KeyValuePair<int,LCARS_NCI_Equippment> pair in mission.equippment.list)
                        {
                            LCARS_NCI_Equippment equ = pair.Value;
                            icon_tex = GameDatabase.Instance.GetTexture(equ.icon, false);
                            GUILayout.BeginHorizontal();
                            GUILayout.Button(icon_tex, GUILayout.Height(50), GUILayout.Width(50));
                            GUILayout.Label(equ.name, GUILayout.Width(100));
                            GUILayout.Label(equ.description, GUILayout.Width(250));
                            if (GUILayout.Button("X")) { mission.equippment.list.Remove(pair.Key); }   
                            GUILayout.EndHorizontal();
                            switch (equ.idcode)
                            {
                                case "BussardCollector":
                                        GUILayout.Label("Note: if you add two of these, each must have a different resource");
                                    GUILayout.Label("Options:");
                                    GUILayout.BeginHorizontal();
                                        if (equ.egg == "")
                                        {
                                            GUILayout.Label("Select Location Egg:");
                                            GUILayout.BeginVertical();
                                            GUILayout.Label("*****************************");
                                            if (mission.requirements.list.Count == 0)
                                            {
                                                GUILayout.Label("Please create the Mission Assets first");
                                            }
                                            foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair2 in mission.requirements.list)
                                            {
                                                LCARS_NCI_Mission_Requirement mE = pair2.Value;
                                                //GUILayout.Label("Part " + mE.name + " contains:");
                                                foreach (string eggstring in mE.actionpoints)
                                                {
                                                    if (GUILayout.Button(eggstring + " at " + mE.name))
                                                    {
                                                        equ.part = mE.part_idcode;
                                                        equ.egg = eggstring;
                                                    }
                                                }
                                            }
                                            GUILayout.Label("*****************************");
                                            GUILayout.EndVertical();
                                        }
                                        else
                                        {
                                            GUILayout.Label(equ.egg + " at " + equ.part);
                                            if (GUILayout.Button("change")) { equ.egg = ""; }
                                        }
                                    GUILayout.EndHorizontal();

                                    if (equ.resource == null) { equ.resource = "WarpPlasma"; }
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("Resource:");
                                        GUILayout.BeginVertical();
                                            equ.resource = GUILayout.TextField(equ.resource, 200, style2_TextField);
                                            GUILayout.Label("Enter a valid KSP/ORS resource name");
                                        GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    if (equ.distance_threshhold == null) { equ.distance_threshhold = ""; }
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("Distance Threshold:");
                                        GUILayout.BeginVertical();
                                        equ.distance_threshhold = GUILayout.TextField(equ.distance_threshhold, 200, style2_TextField);
                                            GUILayout.Label("Starts working only below this distance");
                                        GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    break;

                                case "IsoFluxDetector":
                                    GUILayout.Label("Options:");
                                    /*GUILayout.BeginHorizontal();
                                        if (equ.egg == "")
                                        {
                                            GUILayout.Label("Select Egg to locate:");
                                            GUILayout.BeginVertical();
                                            GUILayout.Label("*****************************");
                                            if (mission.requirements.list.Count == 0)
                                            {
                                                GUILayout.Label("Please create the Mission Assets first");
                                            }
                                            foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair2 in mission.requirements.list)
                                            {
                                                LCARS_NCI_Mission_Requirement mE = pair2.Value;
                                                //GUILayout.Label("Part " + mE.name + " contains:");
                                                foreach (string eggstring in mE.actionpoints)
                                                {
                                                    if (GUILayout.Button(eggstring + " at " + mE.name))
                                                    {
                                                        equ.part = mE.part_idcode;
                                                        equ.egg = eggstring;
                                                    }
                                                }
                                            }
                                            GUILayout.Label("*****************************");
                                            GUILayout.EndVertical();
                                        }
                                        else
                                        {
                                            GUILayout.Label(equ.egg + " at " + equ.part);
                                            if (GUILayout.Button("change")) { equ.egg = ""; }
                                        }
                                    GUILayout.EndHorizontal();
                                    */
                                    if (equ.distance_threshhold == null) { equ.distance_threshhold = ""; }
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("Note: if you add two of these, the second will be ignored");
                                        GUILayout.Label("Distance Threshold:");
                                        GUILayout.BeginVertical();
                                        equ.distance_threshhold = GUILayout.TextField(equ.distance_threshhold, 200, style2_TextField);
                                            GUILayout.Label("Starts working only below this distance");
                                        GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    break;

                                case "NucleonicAnalyzer":
                                        GUILayout.Label("Note: if you add two of these, each must have a different Artefact");
                                        GUILayout.Label("Options");
                                        GUILayout.BeginHorizontal();
                                        if (equ.artefact1 == "")
                                        {
                                            if (mission.artefacts.list.Count == 0)
                                            {
                                                GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                            }
                                            else
                                            {
                                                GUILayout.BeginVertical();
                                                GUILayout.Label("Select Artefact to be Analyzed");
                                                foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                                {
                                                    if (GUILayout.Button(invItem.name))
                                                    {
                                                        equ.artefact1 = invItem.idcode;
                                                    }
                                                }
                                                GUILayout.EndVertical();
                                            }
                                        }
                                        else
                                        {
                                            GUILayout.Label("Target Artefact: " + equ.artefact1);
                                            if (GUILayout.Button("change")) { equ.artefact1 = ""; }
                                        }
                                        GUILayout.EndHorizontal();
                                        if (equ.textline == null) { equ.textline = ""; }
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("Result Text:");
                                        GUILayout.BeginVertical();
                                            equ.textline = GUILayout.TextField(equ.textline, 200, style2_TextField);
                                            GUILayout.Label("What you type here will be the result of the anlysis");
                                        GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                    break;

                                case "PteroplasticScrambler":
                                    GUILayout.Label("Note: if you add two of these, each must have a different Artefacts");
                                    GUILayout.Label("Options");
                                    GUILayout.BeginHorizontal();
                                    if (equ.artefact1 == "")
                                    {
                                        if (mission.artefacts.list.Count == 0)
                                        {
                                            GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                        }
                                        else
                                        {
                                            GUILayout.BeginVertical();
                                            GUILayout.Label("Select Artefact to be Scrambled");
                                            foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                            {
                                                if (GUILayout.Button(invItem.name))
                                                {
                                                    equ.artefact1 = invItem.idcode;
                                                }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                    }
                                    else
                                    {
                                        GUILayout.Label("Input Artefact: " + equ.artefact1);
                                        if (GUILayout.Button("change")) { equ.artefact1 = ""; }
                                    }
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    if (equ.artefact2 == "")
                                    {
                                        if (mission.artefacts.list.Count == 0)
                                        {
                                            GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                        }
                                        else
                                        {
                                            GUILayout.BeginVertical();
                                            GUILayout.Label("Select Artefact to be Delivered");
                                            foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                            {
                                                if (GUILayout.Button(invItem.name))
                                                {
                                                    equ.artefact2 = invItem.idcode;
                                                }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                    }
                                    else
                                    {
                                        GUILayout.Label("Output Artefact: " + equ.artefact2);
                                        if (GUILayout.Button("change")) { equ.artefact2 = ""; }
                                    }
                                    GUILayout.EndHorizontal();

                                    break;

                            }
                            GUILayout.Label("******************************");

                        }
                        GUILayout.Label("********************************************************************");
                        GUILayout.EndVertical();


                    }
#endregion


#region Artefacts

                if (formtoggle_artefacts)
                {
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI Artefacts begin ");
                    switch(Artefact_add_mode)
                    {
                        case "existing":
                            if (GUILayout.Button("Back"))
                            {
                                Artefact_add_mode = "";
                            }
                             scrollview_style2.fixedHeight = 250;
                             GUILayout.BeginVertical(scrollview_style2);
                             ScrollPosition2 = GUILayout.BeginScrollView(ScrollPosition2);
                             foreach (KeyValuePair<string, LCARS_NCI_InventoryItem_Type> pair in LCARS_NCI.Instance.Data.Artefact_List)
                            {
                                LCARS_NCI_InventoryItem_Type invItem = pair.Value;
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(invItem.name, GUILayout.Width(100));
                                GUILayout.Label(invItem.description, GUILayout.Width(300));
                                if (GUILayout.Button("Select")) { mission.artefacts.list.Add(invItem); }
                                GUILayout.EndHorizontal();
                            }
                             GUILayout.EndScrollView();
                             GUILayout.EndVertical();
                            break;

                        case"new":
                            if (GUILayout.Button("Back"))
                            {
                                Artefact_add_mode = "";
                            }

                                GUILayout.Label("Create Artefact:");
                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    GUILayout.Label("idcode", GUILayout.Width(100));
                                    GUILayout.Label("name", GUILayout.Width(100));
                                    GUILayout.Label("description", GUILayout.Width(100));
                                    GUILayout.Label("icon", GUILayout.Width(100));
                                    GUILayout.Label("isDamagable", GUILayout.Width(100));
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    style2_TextField.fixedWidth = 100;
                                    newArtefact_idcode = GUILayout.TextField(newArtefact_idcode, 200, style2_TextField);
                                    newArtefact_name = GUILayout.TextField(newArtefact_name, 200, style2_TextField);
                                    newArtefact_description = GUILayout.TextField(newArtefact_description, 200, style2_TextField);
                                    newArtefact_icon = GUILayout.TextField(newArtefact_icon, 200, style2_TextField);
                                    newArtefact_isDamagable = GUILayout.Toggle(newArtefact_isDamagable, "true");
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    GUILayout.Label("usage_amount (-1=unlimited)", GUILayout.Width(150));
                                    GUILayout.Label("powerconsumption", GUILayout.Width(150));
                                    if (newArtefact_isDamagable)
                                    {
                                        GUILayout.Label("integrity", GUILayout.Width(150));
                                    }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(480));
                                    newArtefact_usage_amount = GUILayout.TextField(newArtefact_usage_amount, 200, style3_TextField);
                                    newArtefact_powerconsumption = GUILayout.TextField(newArtefact_powerconsumption, 200, style3_TextField);
                                    if (newArtefact_isDamagable)
                                    {
                                        newArtefact_integrity = GUILayout.TextField(newArtefact_integrity, 200, style3_TextField);
                                    }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(440));
                                    if (GUILayout.Button("add Artefact") && newArtefact_idcode != "")
                                    {
                                        LCARS_NCI_InventoryItem_Type i = new LCARS_NCI_InventoryItem_Type();
                                        i.idcode =  newArtefact_idcode;
                                        i.name = newArtefact_name;
                                        i.description  =  newArtefact_description;
                                        i.icon  =  newArtefact_icon;
                                        i.isDamagable  =  newArtefact_isDamagable.ToString();
                                        i.integrity =  newArtefact_integrity;
                                        i.usage_amount  =  newArtefact_usage_amount;
                                        i.usage_times  =  "0";
                                        i.powerconsumption  =  newArtefact_powerconsumption;
                                        mission.artefacts.list.Add(i);
                                        LCARS_NCI.Instance.Data.Artefact_List.Add(i.idcode, i);
                                        newArtefact_idcode = "";
                                        newArtefact_name = "";
                                        newArtefact_description = "";
                                        newArtefact_icon = "";
                                        newArtefact_isDamagable = false;
                                        newArtefact_integrity = "";
                                        //newArtefact_usage_mode = "";
                                        newArtefact_usage_amount = "";
                                        newArtefact_powerconsumption = "";
                                    }
                                GUILayout.EndHorizontal();
                            break;

                        default:
                            GUILayout.BeginHorizontal(GUILayout.Width(420));
                                GUILayout.Label("Add: ");
                                if (GUILayout.Button("Existing"))
                                {
                                    Artefact_add_mode = "existing";
                                }
                                if (GUILayout.Button("New"))
                                {
                                    Artefact_add_mode = "new";
                                }
                            GUILayout.EndHorizontal();
                            break;
                    }
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 12 ");

                    GUILayout.BeginVertical();
                    GUILayout.Label("********************************************************************");
                    if (mission.artefacts.list.Count == 0)
                    {
                        GUILayout.Label("No NCI-Artefacts Selected");
                    }
                    foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                    {
                        bool use_default_portait = false;
                        try
                        {
                            icon_tex = GameDatabase.Instance.GetTexture(invItem.icon, false);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_artefacts loaded icon_tex invItem.icon=" + invItem.icon);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_artefacts   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.Log("### NCIMC formtoggle_artefacts skipping icon_tex invItem.icon=" + invItem.icon + " - file not found ex=" + ex);
                            use_default_portait = true;
                        }
                        if (invItem.icon == "")
                        {
                            use_default_portait = true;
                        }
                        if (use_default_portait)
                        {
                            icon_tex = GameDatabase.Instance.GetTexture(Artefact_default_icon, false);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_artefacts loaded icon_tex Artefact_default_icon=" + Artefact_default_icon);
                            //UnityEngine.Debug.Log("### NCIMC formtoggle_artefacts   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                            GUILayout.Label("Warning: This Artefact has no Picture!");
                        }
                        GUILayout.BeginHorizontal();
                        GUILayout.Button(icon_tex, GUILayout.Height(50), GUILayout.Width(50));
                        GUILayout.Label(invItem.name, GUILayout.Width(100));
                        GUILayout.Label(invItem.description, GUILayout.Width(250));
                        if (GUILayout.Button("X")) { mission.artefacts.list.Remove(invItem); }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Label("********************************************************************");
                    GUILayout.EndVertical();
                }

#endregion

                //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre seal deal ");

#region ObjectOptions

                if (formtoggle_ObjectOptions)
                {
                    GUILayout.Label("Required NCI-Objects Selected:");
                    if (ObjectOptions_selected > 0)
                    {
                        LCARS_NCI_Mission_Requirement R = mission.requirements.list[ObjectOptions_selected];
                        GUILayout.BeginVertical();
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(R.name, GUILayout.Width(200));
                            if (GUILayout.Button("Back"))
                            {
                                ObjectOptions_selected = 0;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                                GUILayout.BeginHorizontal();
                                    GUILayout.Label("Doors/ForceFields/Specials:");
                                GUILayout.EndHorizontal();
                            foreach (string eM in R.doors)
                            {
                                GUILayout.BeginHorizontal();
                                    GUILayout.Label(" ", GUILayout.Width(25));
                                    GUILayout.Label(eM);
                                GUILayout.EndHorizontal();
                                if (mission.steps.Count == 0)
                                {
                                    GUILayout.Label("No Steps available");
                                }
                                else 
                                {
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label(" ", GUILayout.Width(50));
                                        GUILayout.Label("Step ID", GUILayout.Width(50));
                                        GUILayout.Label("OptionID", GUILayout.Width(250));
                                    GUILayout.EndHorizontal();
                                
                                }
                                foreach (int stepID in mission.steps.Keys)
                                {
                                    if (mission.steps[stepID].PartOptions==null)
                                    {
                                        mission.steps[stepID].PartOptions = new Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type>();
                                    }
                                    if (mission.steps[stepID].PartOptions.ContainsKey(eM + "_" + R.id))
                                    {
                                        GUILayout.BeginHorizontal();
                                            GUILayout.Label(" ", GUILayout.Width(50));
                                            GUILayout.Label("Step " + stepID);
                                            GUILayout.Label("Status: " + ((mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled) ? "Locked" : "Unlocked"), GUILayout.Width(200));
                                        if (mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled)
                                        {
                                            if (GUILayout.Button("Unlock"))
                                            {
                                                mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (GUILayout.Button("Lock"))
                                            {
                                                mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled = true;
                                            }
                                        }
                                        if (GUILayout.Button("X"))
                                        {
                                            mission.steps[stepID].PartOptions.Remove(eM + "_" + R.id);
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                    else 
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(" ", GUILayout.Width(50));
                                        GUILayout.Label("Step " + stepID + " Status: none", GUILayout.Width(200));
                                        if (GUILayout.Button("Add Option"))
                                        {
                                            LCARS_NCI_Step_Remote_PartOptions_Type tmp = new LCARS_NCI_Step_Remote_PartOptions_Type();
                                            tmp.requirementID = R.id;
                                            tmp.partmodulename = R.partmodulename;
                                            tmp.part_idcode = R.part_idcode;
                                            tmp.OptionID = eM;
                                            tmp.isDisabled = false;
                                            mission.steps[stepID].PartOptions.Add(eM + "_" + R.id,tmp);
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                }
                            }
                            GUILayout.EndVertical();
                        /*
                                GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("GUIWindows:");
                                    GUILayout.EndHorizontal();
                                foreach (string eM in R.GUIWindows)
                                {
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label(" ", GUILayout.Width(25));
                                    GUILayout.Label(eM);
                                    GUILayout.EndHorizontal();
                                    if (mission.steps.Count == 0)
                                    {
                                        GUILayout.Label("No Steps available");
                                    }
                                    else
                                    {
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label(" ", GUILayout.Width(50));
                                        GUILayout.Label("Step ID", GUILayout.Width(50));
                                        GUILayout.Label("OptionID", GUILayout.Width(250));
                                        GUILayout.EndHorizontal();

                                    }
                                    foreach (int stepID in mission.steps.Keys)
                                    {
                                        if (mission.steps[stepID].PartOptions == null)
                                        {
                                            mission.steps[stepID].PartOptions = new Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type>();
                                        }
                                        if (mission.steps[stepID].PartOptions.ContainsKey(eM + "_" + R.id))
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label(" ", GUILayout.Width(50));
                                            GUILayout.Label("Step " + stepID);
                                            GUILayout.Label("Status: " + ((mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled) ? "Override" : "Allow"), GUILayout.Width(200));
                                            if (mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled)
                                            {
                                                if (GUILayout.Button("Allow"))
                                                {
                                                    mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled = false;
                                                }
                                            }
                                            else
                                            {
                                                if (GUILayout.Button("Override"))
                                                {
                                                    mission.steps[stepID].PartOptions[eM + "_" + R.id].isDisabled = true;
                                                }
                                            }
                                            if (GUILayout.Button("X"))
                                            {
                                                mission.steps[stepID].PartOptions.Remove(eM + "_" + R.id);
                                            }
                                            GUILayout.EndHorizontal();
                                        }
                                        else
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label(" ", GUILayout.Width(50));
                                            GUILayout.Label("Step " + stepID + " Status: none", GUILayout.Width(200));
                                            if (GUILayout.Button("Add Option"))
                                            {
                                                LCARS_NCI_Step_Remote_PartOptions_Type tmp = new LCARS_NCI_Step_Remote_PartOptions_Type();
                                                tmp.requirementID = R.id;
                                                tmp.part_idcode = R.part_idcode;
                                                tmp.OptionID = eM;
                                                tmp.isDisabled = false;
                                                mission.steps[stepID].PartOptions.Add(eM + "_" + R.id, tmp);
                                            }
                                            GUILayout.EndHorizontal();
                                        }

                                    }
                                }
                                GUILayout.EndVertical();
                        */
                        GUILayout.EndVertical();

                    }
                    else
                    { 
                        if (mission.requirements.list.Count == 0)
                        {
                            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 11 ");
                            GUILayout.Label("No NCI-Objects Selected");
                        }

                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                        {
                            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI requirements 12 ");
                            LCARS_NCI_Mission_Requirement R = pair.Value;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(R.name, GUILayout.Width(200));
                            if(GUILayout.Button("Edit Options"))
                            {
                                ObjectOptions_selected = pair.Key;
                            }
                            GUILayout.EndHorizontal();
                        
                        }
                    }
                }

#endregion

#region SealDeal
/*
                if (formtoggle_SealDeal)
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Science:", GUILayout.Width(120));
                        GUILayout.Label("Cash:", GUILayout.Width(120));
                        GUILayout.Label("Reputation:", GUILayout.Width(120));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        style2_TextField.fixedWidth = 120;
                        mission.sealDeal.Gain.cash = GUILayout.TextField(mission.sealDeal.Gain.cash, 70, style2_TextField);
                        mission.sealDeal.Gain.science = GUILayout.TextField(mission.sealDeal.Gain.science, 70, style2_TextField);
                        mission.sealDeal.Gain.reputation = GUILayout.TextField(mission.sealDeal.Gain.reputation, 70, style2_TextField);
                    GUILayout.EndHorizontal();

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 1 ");
                    GUILayout.Label("Artefacts:");
                    GUILayout.BeginVertical();
                    GUILayout.Label("********************************************************************");
                    if (mission.personalities.list.Count == 0)
                    {
                        GUILayout.Label("No NCI-Artefacts available, complete the assets!");
                    }
                    foreach (LCARS_NCI_InventoryItem_Type e in mission.artefacts.list)
                    {
                        if (GUILayout.Button(e.name)) { mission.addArtefactGain(e); }
                    }
                    GUILayout.Label("********************************************************************");
                    GUILayout.EndVertical();
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI 8 ");

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2 ");
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2a ");

                    GUILayout.BeginVertical();
                    GUILayout.Label("********************************************************************");
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2b ");
                    if (mission.sealDeal.Gain.objects.Count == 0)
                    {
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2c ");
                        GUILayout.Label("No NCI-Artefacts Selected");
                    }
                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2d ");
                    foreach (string s in mission.sealDeal.Gain.objects)
                    {
                        if(s=="")
                        { continue; }
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2e ");
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2f s=" + s);
                        LCARS_NCI_InventoryItem_Type e = mission.artefacts.getArtefactByName(s);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(e.name, GUILayout.Width(100));
                        GUILayout.Label(e.description, GUILayout.Width(300));
                        if (GUILayout.Button("X")) { mission.sealDeal.Gain.objects.Remove(e.idcode); }
                        GUILayout.EndHorizontal();
                        //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 2g ");
                    }
                    GUILayout.Label("********************************************************************");
                    GUILayout.EndVertical();

                    //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI seal deal 3 ");

                }
*/
#endregion
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
#endregion

                }
                //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI pre Mission_Story ");
                if (formtoggle_Mission_Story)
                {

#region Mission Story
                    GUILayout.Label("Mission Story: ");


                        //GUILayout.Label("ToDo ");


#region List Steps
                                GUILayout.BeginHorizontal();
                                    GUILayout.Label("You have " + mission.steps.Count+ " Steps defined.  ");
                                    if (GUILayout.Button("Add new Step"))
                                    {
                                        LCARS_NCI_Mission_Step ns = new LCARS_NCI_Mission_Step();
                                        ns.id = mission.steps.Count + 1;
                                        ns.locationtype = "";
                                        ns.location_part = "";
                                        ns.location_egg = "";
                                        ns.location_distance = "";
                                        //ns.sealdeal_textID = 0;
                                        //ns.conversation_trigger_distance = "1";
                                        ns.next_step = "";
                                        ns.Messages = new LCARS_NCI_Mission_Messages();
                                        ns.Messages.MessageList = new Dictionary<int, LCARS_NCI_Mission_Message>();
                                        ns.stepStart_messageID_email = "none";
                                        ns.stepStart_messageID_console = "none";
                                        ns.stepStart_messageID_screen = "none";
                                        ns.stepEnd_messageID_email = "none";
                                        ns.stepEnd_messageID_console = "none";
                                        ns.stepEnd_messageID_screen = "none";
                                        mission.steps.Add(ns.id, ns);
                                        story_edit_current_step = ns.id;
                                        story_edit_current_speech = 0;
                                    }
                                GUILayout.EndHorizontal();

                                if (mission.steps.Count > 0)
                                {
                                    story_edit_current_step = (story_edit_current_step > 0) ? story_edit_current_step : 1;
                                    Step = mission.steps[story_edit_current_step];


                                    GUILayout.BeginHorizontal();
#region Step Top
                                    GUILayout.BeginVertical(GUILayout.Width(120));
                                    if (story_edit_current_step > 1)
                                    {
                                        if (GUILayout.Button("<< Step " + (story_edit_current_step - 1)))
                                        { story_edit_current_step--; story_edit_current_speech = 0; }
                                    }
                                    else { GUILayout.Label(""); }
                                    GUILayout.EndVertical();
                                    GUILayout.BeginVertical(GUILayout.Width(250));
                                    GUILayout.Label("                      Step " + Step.id);
                                    GUILayout.EndVertical();
                                    GUILayout.BeginVertical(GUILayout.Width(120));
                                    if (story_edit_current_step < mission.steps.Count)
                                    {
                                        if (GUILayout.Button("Step " + (story_edit_current_step + 1) + " >>"))
                                        { story_edit_current_step++; story_edit_current_speech = 0; }
                                    }
                                    else { GUILayout.Label(""); }
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();

                                    GUILayout.Label("********************************************************************");
                                    GUILayout.Label("Step Settings:  Please fill out all Step-Sections below as needed!");
                                    GUILayout.BeginHorizontal();
                                    if (step_formtoggle_options == false && step_formtoggle_conversation == false && step_formtoggle_jobs == false && step_formtoggle_messages == false)
                                    {
                                        GUILayout.Label("Step-Sections:");
                                        if (GUILayout.Button("Options"))
                                        {
                                            step_formtoggle_options = true;
                                            step_formtoggle_conversation = false;
                                            step_formtoggle_messages = false;
                                            step_formtoggle_jobs = false;
                                        }
                                        if (GUILayout.Button("Conversation"))
                                        {
                                            step_formtoggle_options = false;
                                            step_formtoggle_conversation = true;
                                            step_formtoggle_messages = false;
                                            step_formtoggle_jobs = false;
                                        }
                                        if (GUILayout.Button("Messages"))
                                        {
                                            step_formtoggle_options = false;
                                            step_formtoggle_conversation = false;
                                            step_formtoggle_messages = true;
                                            step_formtoggle_jobs = false;
                                        }
                                        if (GUILayout.Button("Jobs"))
                                        {
                                            step_formtoggle_options = false;
                                            step_formtoggle_conversation = false;
                                            step_formtoggle_messages = false;
                                            step_formtoggle_jobs = true;
                                        }
                                    }
                                    else
                                    {
                                        if (step_formtoggle_options)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Step Options:");
                                            if (GUILayout.Button("Back")) { step_formtoggle_options = false; }
                                            GUILayout.EndHorizontal();
                                        }
                                        if (step_formtoggle_conversation)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Step Conversation:");
                                            if (GUILayout.Button("Back")) { step_formtoggle_conversation = false; }
                                            GUILayout.EndHorizontal();
                                        }
                                        if (step_formtoggle_messages)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Step Messages:");
                                            if (GUILayout.Button("Back")) { step_formtoggle_messages = false; }
                                            GUILayout.EndHorizontal();
                                        }
                                        if (step_formtoggle_jobs)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Step Jobs:");
                                            if (GUILayout.Button("Back")) { step_formtoggle_jobs = false; }
                                            GUILayout.EndHorizontal();
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                    GUILayout.Label("********************************************************************");
                                    #endregion

                                }
                    scrollview_style1.fixedHeight = 440;
                    scrollview_style1.fixedWidth = 530;
                    GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(530));
                    ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

                        if (mission.steps.Count > 0)
                        {
                            story_edit_current_step = (story_edit_current_step > 0) ? story_edit_current_step : 1;
                            Step = mission.steps[story_edit_current_step];

                            if (step_formtoggle_options)
                            {
#region Step Options
                                //UnityEngine.Debug.Log("### NCIMC Step Options begin ");
                                        
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Delete this Step: (no undo)");
                                if (GUILayout.Button("Delete")) 
                                { 
                                    mission.steps.Remove(story_edit_current_step); 
                                    story_edit_current_step = 0;
                                    Dictionary<int, LCARS_NCI_Mission_Step> tmp = new Dictionary<int, LCARS_NCI_Mission_Step>();
                                    foreach(LCARS_NCI_Mission_Step S in mission.steps.Values)
                                    {
                                        S.id = tmp.Count + 1;
                                        tmp.Add((tmp.Count + 1),S);
                                    }
                                    mission.steps = tmp;
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.Label("Be Aware: if you delete a Step, the Step IDs will change, all references might have changed!");
                                GUILayout.Label("********************************************************************");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("LocationType:");
                                if (Step.locationtype == "")
                                {
                                    //if (GUILayout.Button("Egg")) { Step.locationtype = "egg"; }
                                    //if (GUILayout.Button("Part")) { Step.locationtype = "part"; }
                                    if (GUILayout.Button("DistanceLower")) { Step.locationtype = "distanceLower"; }
                                    if (GUILayout.Button("DistanceGreater")) { Step.locationtype = "distanceGreater"; }
                                }
                                else
                                {
                                    GUILayout.Label(Step.locationtype);
                                    if (GUILayout.Button("change")) { Step.locationtype = ""; }
                                }
                                GUILayout.EndHorizontal();

                                //UnityEngine.Debug.Log("### NCIMC Step Options 1 ");
                                if (Step.locationtype != "")
                                {
#region Locationtype
                                    switch (Step.locationtype)
                                    {
                                        case "distanceLower":
                                        case "distanceGreater":
                                            GUILayout.BeginVertical();
                                            GUILayout.Label("Select Distance and Part:");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Distance:");
                                            Step.location_distance = GUILayout.TextField(Step.location_distance, 70);
                                            GUILayout.Label("Meter");
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Relative to Requirement:");
                                            if (Step.location_part == "" && Step.location_body == "")
                                            {
                                                GUILayout.BeginVertical();
                                                GUILayout.Label("*****************************");
                                                if (mission.requirements.list.Count == 0)
                                                {
                                                    GUILayout.Label("Please create the Mission Assets first");
                                                }
                                                foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                {
                                                    LCARS_NCI_Mission_Requirement mE = pair.Value;
                                                    if (mE.isCelestialBody)
                                                    {
                                                        if (GUILayout.Button(mE.name + " (CoM)"))
                                                        {
                                                            Step.requirementPart_id = mE.id;
                                                            Step.location_body = mE.name;
                                                            Step.location_part = "";
                                                            Step.location_PQSCity = "";
                                                            Step.location_egg = "";
                                                        }
                                                        foreach (KeyValuePair<string, List<string>> pair2 in mE.Locations)
                                                        {
                                                            string location = pair2.Key;
                                                            GUILayout.Label(mE.name+"-Location: " + location);
                                                            List<string> actionpoints = pair2.Value;
                                                            foreach (string s in actionpoints)
                                                            {
                                                                if (GUILayout.Button(s + " at " + location + " on " + mE.name))
                                                                {
                                                                    Step.requirementPart_id = mE.id;
                                                                    Step.location_body = mE.name;
                                                                    Step.location_part = "";
                                                                    Step.location_PQSCity = location;
                                                                    Step.location_egg = s;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {

                                                        //GUILayout.Label("Part " + mE.name + " contains:");
                                                        if (GUILayout.Button(mE.name+" (CoM)"))
                                                        {
                                                            Step.requirementPart_id = mE.id;
                                                            Step.location_part = mE.part_idcode;
                                                            Step.location_egg = "";
                                                            Step.location_body = "";
                                                            Step.location_PQSCity = "";
                                                        }
                                                        foreach (string eggstring in mE.actionpoints)
                                                        {
                                                            if (GUILayout.Button(eggstring + " at " + mE.name))
                                                            {
                                                                Step.requirementPart_id = mE.id;
                                                                Step.location_part = mE.part_idcode;
                                                                Step.location_egg = eggstring;
                                                                Step.location_body = "";
                                                                Step.location_PQSCity = "";
                                                            }
                                                        }
                                                    }
                                                }
                                                GUILayout.Label("*****************************");
                                                GUILayout.EndVertical();
                                            }
                                            else
                                            {

                                                if (Step.location_body != "")
                                                {
                                                    if (Step.location_egg != "")
                                                    {
                                                        GUILayout.Label(Step.location_egg + " at " + Step.location_PQSCity + " on " + Step.location_body);
                                                    }
                                                    else
                                                    {
                                                        GUILayout.Label(Step.location_body);
                                                    }
                                                    if (GUILayout.Button("change")) { Step.location_body = ""; }
                                                }
                                                else
                                                {
                                                    if (Step.location_egg != "")
                                                    {
                                                            GUILayout.Label(Step.location_egg + " at " + Step.location_part);
                                                    }
                                                    else
                                                    {
                                                        GUILayout.Label(Step.location_part);
                                                    }
                                                    if (GUILayout.Button("change")) { Step.location_part = ""; }
                                                }
                                            }
                                            GUILayout.EndHorizontal();
                                            GUILayout.EndVertical();
                                            break;
                                    }
#endregion
                                }
                                /*
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("GUI Button:");
                                GUILayout.BeginVertical();
                                if (Step.GUIButton == null)
                                {
                                    if (GUILayout.Button("Add GUI Button"))
                                    {
                                        //UnityEngine.Debug.Log("### NCIMC Add GUI Button 1 ");
                                        Step.GUIButton = new LCARS_NCI_Mission_Step_GUIButton();
                                        Step.GUIButton.buttontext1 = "";
                                        Step.GUIButton.textID = 0;
                                        //UnityEngine.Debug.Log("### NCIMC Add GUI Button 2 ");
                                    }
                                }
                                else
                                {
                                    //UnityEngine.Debug.Log("### NCIMC Add GUI Button 3 ");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Button Text:");
                                    Step.GUIButton.buttontext1 = GUILayout.TextField(Step.GUIButton.buttontext1, 70);
                                    //UnityEngine.Debug.Log("### NCIMC Add GUI Button 4 ");
                                    if (GUILayout.Button("X"))
                                    {
                                        Step.GUIButton = null;
                                    }
                                    GUILayout.EndHorizontal();
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("GoTo Text ID:");
                                    if (Step.GUIButton.textID == 0)
                                    {
                                        //UnityEngine.Debug.Log("### NCIMC Add GUI Button 5 ");
                                        if (mission.steps[story_edit_current_step].Conversation == null)
                                        {
                                            GUILayout.Label("Please create the Step Conversation first");
                                            //UnityEngine.Debug.Log("### NCIMC Add GUI Button 6 ");
                                        }
                                        else
                                        {
                                            foreach (LCARS_NCI_Mission_Conversation_Speech cS in mission.steps[story_edit_current_step].Conversation.SpeechList.Values)
                                            {
                                                if (GUILayout.Button("Speech " + cS.textID)) { Step.GUIButton.textID = cS.textID; }
                                                //UnityEngine.Debug.Log("### NCIMC Add GUI Button 7 ");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //UnityEngine.Debug.Log("### NCIMC Add GUI Button 8 ");

                                        GUILayout.Label(Step.GUIButton.textID.ToString());
                                        if (GUILayout.Button("change")) { Step.GUIButton.textID = 0; }
                                        //UnityEngine.Debug.Log("### NCIMC Add GUI Button 9 ");

                                    }
                                    GUILayout.EndHorizontal();
                                }
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                                */
                                //UnityEngine.Debug.Log("### NCIMC Step Options 2 ");
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send eMail Message at StepStart:");
                                if (Step.stepStart_messageID_email == "" && Step.stepStart_messageID_email != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepStart_messageID_email = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "eMail") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepStart_messageID_email = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepStart_messageID_email);
                                    if (GUILayout.Button("change")) { Step.stepStart_messageID_email = ""; }
                                }
                                GUILayout.EndHorizontal();
                                //UnityEngine.Debug.Log("### NCIMC Step Options 3 ");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send Console Message at StepStart:");
                                if (Step.stepStart_messageID_console == "" && Step.stepStart_messageID_console != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepStart_messageID_console = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "console") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepStart_messageID_console = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepStart_messageID_console);
                                    if (GUILayout.Button("change")) { Step.stepStart_messageID_console = ""; }
                                }
                                GUILayout.EndHorizontal();
                                //UnityEngine.Debug.Log("### NCIMC Step Options 3 ");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send Screen Message at StepStart:");
                                if (Step.stepStart_messageID_screen == "" && Step.stepStart_messageID_screen != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepStart_messageID_screen = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "screen") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepStart_messageID_screen = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepStart_messageID_screen);
                                    if (GUILayout.Button("change")) { Step.stepStart_messageID_screen = ""; }
                                }
                                GUILayout.EndHorizontal();
                                //UnityEngine.Debug.Log("### NCIMC Step Options 3 ");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send eMail Message at StepEnd:");
                                if (Step.stepEnd_messageID_email == "" && Step.stepEnd_messageID_email != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepEnd_messageID_email = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "eMail") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepEnd_messageID_email = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepEnd_messageID_email);
                                    if (GUILayout.Button("change")) { Step.stepEnd_messageID_email = ""; }
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send Console Message at StepEnd:");
                                if (Step.stepEnd_messageID_console == "" && Step.stepEnd_messageID_console != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepEnd_messageID_console = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "console") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepEnd_messageID_console = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepEnd_messageID_console);
                                    if (GUILayout.Button("change")) { Step.stepEnd_messageID_console = ""; }
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Send Screen Message at StepEnd:");
                                if (Step.stepEnd_messageID_screen == "" && Step.stepEnd_messageID_screen != "none")
                                {
                                    GUILayout.BeginVertical();
                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                    if (GUILayout.Button("none")) { Step.stepEnd_messageID_screen = "none"; }
                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                    {
                                        if (mM.messageType != "screen") { continue; }
                                        if (GUILayout.Button("Message " + mM.messageID)) { Step.stepEnd_messageID_screen = mM.messageID.ToString(); }
                                    }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.stepEnd_messageID_screen);
                                    if (GUILayout.Button("change")) { Step.stepEnd_messageID_screen = ""; }
                                }
                                GUILayout.EndHorizontal();

                                //UnityEngine.Debug.Log("### NCIMC Step Options 4 ");

                                GUILayout.BeginHorizontal();
                                GUILayout.Label("Next Step:");
                                if (Step.next_step == "")
                                {
                                    GUILayout.BeginVertical();
                                    foreach (LCARS_NCI_Mission_Step mS in mission.steps.Values)
                                    {
                                        if (mS.id == Step.id) { continue; }
                                        if (GUILayout.Button("Step " + mS.id)) { Step.next_step = mS.id.ToString(); }
                                    }
                                    if (GUILayout.Button("MissionEnd")) { Step.next_step = "missionEnd"; }
                                    GUILayout.EndVertical();
                                }
                                else
                                {
                                    GUILayout.Label(Step.next_step);
                                    if (GUILayout.Button("change")) { Step.next_step = ""; }
                                }
                                GUILayout.EndHorizontal();
                                UnityEngine.Debug.Log("### NCIMC Step Options 5 ");


                if (Step.LCARS_Systems_Options == null) 
                {
                    Step.LCARS_Systems_NotificationText = "";
                    Step.LCARS_Systems_Options = new List<LCARS_ShipSystems_Options>(); 
                    LCARS_ShipSystems_Options SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "PropulsionMatrix";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "SensorArray";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "CommunicationsArray";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "ShieldMatrix";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "PhaserArray";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "TorpedoLauncher";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "Transporter Systems";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);

                    SSO = new LCARS_ShipSystems_Options();
                    SSO.system_name = "Tractor Beam";
                    SSO.damage_this = false;
                    SSO.disable_this = false;
                    Step.LCARS_Systems_Options.Add(SSO);
                }


                                GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("LCARS Damage Systems:");
                                        GUILayout.Label("You can damage LCARS systems on a <per step> basis, what you select here, will break immediately if this step starts and the player must repair it. Write a message line to tell the user the reason for the damage.");
                                    GUILayout.EndHorizontal();
                                    GUILayout.BeginHorizontal();
                                        GUILayout.Label("LCARS Disable Systems:");
                                        GUILayout.Label("You can disable LCARS systems on a <per step> basis, what you disable here, will stop working immediately if this step starts and the player cannot reactivate it until next step. Write a message line to tell the user the reason for the malfunctions.");
                                    GUILayout.EndHorizontal();
                                GUILayout.EndVertical();
                                if (LCARS_Systems_NotificationText_needed)
                                {
                                GUILayout.BeginVertical();
                                GUILayout.Label("Notification Text - Let the player know, why those Systems failed?:");
                                Step.LCARS_Systems_NotificationText = GUILayout.TextArea(Step.LCARS_Systems_NotificationText, 500, GUILayout.Width(300), GUILayout.Height(50));
                                GUILayout.EndVertical();
                                }


                                GUILayout.BeginVertical();
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("ShipSystem Name", GUILayout.Width(150));
                                    GUILayout.Label("Disabled", GUILayout.Width(170));
                                    GUILayout.Label("Damaged");
                                    GUILayout.EndHorizontal();
                                    LCARS_Systems_NotificationText_needed = false;
                                    foreach(LCARS_ShipSystems_Options ssO in Step.LCARS_Systems_Options)
                                    {
                                        GUILayout.BeginHorizontal();
                                            GUILayout.Label(ssO.system_name, GUILayout.Width(150));
                                            if (GUILayout.Button(ssO.disable_this.ToString()))
                                            {
                                                ssO.disable_this = !ssO.disable_this;
                                            }
                                            if (GUILayout.Button(ssO.damage_this.ToString()))
                                            {
                                                ssO.damage_this = !ssO.damage_this;
                                            }
                                        GUILayout.EndHorizontal();
                                        if (ssO.disable_this || ssO.damage_this)
                                        {
                                            LCARS_Systems_NotificationText_needed = true;
                                        }
                                    }
                                GUILayout.EndVertical();
                                UnityEngine.Debug.Log("### NCIMC Step Options 6 ");

#endregion
                            }
                            if (step_formtoggle_conversation)
                            {
#region Step Conversation
                                //UnityEngine.Debug.Log("### NCIMC - Step Conversation  With whom do you want to talk start ");
                                if (mission.steps[story_edit_current_step].Conversation == null)
                                {
                                    mission.steps[story_edit_current_step].Conversation = new LCARS_NCI_Mission_Conversation();
                                    mission.steps[story_edit_current_step].Conversation.personality = "";
                                    mission.steps[story_edit_current_step].Conversation.SpeechList = new Dictionary<int,LCARS_NCI_Mission_Conversation_Speech>();
                                }
                                if(mission.steps[story_edit_current_step].Conversation.personality=="")
                                {
#region Step Conversation Init
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation  With whom do you want to talk start ");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("With whom do you want to talk?:");

                                        if (mission.personalities.list.Count == 0)
                                        {
                                            GUILayout.Label("Please complete the Mission-Assets first");
                                        }
                                        else
                                        {
                                            GUILayout.BeginVertical();
                                            foreach (LCARS_NCI_Personality P in mission.personalities.list)
                                            {
                                                if (GUILayout.Button(P.name)) { mission.steps[story_edit_current_step].Conversation.personality = P.idcode; }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                            
                                    GUILayout.EndHorizontal();
#endregion
                                }
                                else
                                {
#region Step Conversation nav
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation  Conversation nav start ");
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation Talking to begin");
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("Talking to " + LCARS_NCI.Instance.Data.Personalities.list[mission.steps[story_edit_current_step].Conversation.personality].name);
                                    if (GUILayout.Button("change")) { mission.steps[story_edit_current_step].Conversation.personality = ""; }
                                    GUILayout.EndHorizontal();
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Label("You have " + mission.steps[story_edit_current_step].Conversation.SpeechList.Count + " Speeches defined.  ");
                                        if (GUILayout.Button("Add new Speech"))
                                        {
                                            LCARS_NCI_Mission_Conversation_Speech nCS = new LCARS_NCI_Mission_Conversation_Speech();
                                            nCS.textID = mission.steps[story_edit_current_step].Conversation.SpeechList.Count+1;
                                            nCS.text = "";
                                            nCS.reward_artefact = "";
                                            nCS.reward_cash = "";
                                            nCS.reward_reputation = "";
                                            nCS.reward_science = "";
                                            nCS.speechStart_messageID_email = "none";
                                            nCS.speechStart_messageID_console = "none";
                                            nCS.speechStart_messageID_screen = "none";
                                            nCS.ResponsList = new List<LCARS_NCI_Mission_Speech_Respons>();
                                            mission.steps[story_edit_current_step].Conversation.SpeechList.Add(nCS.textID, nCS);
                                            story_edit_current_speech = nCS.textID;
                                        }
                                    GUILayout.EndHorizontal();
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation Talking to end");
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation story_edit_current_step=" + story_edit_current_step);
                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation story_edit_current_speech=" + story_edit_current_speech);
#endregion

                                    //UnityEngine.Debug.Log("### NCIMC - Step Conversation  SpeechList start ");

                                    if (mission.steps[story_edit_current_step].Conversation.SpeechList.Count > 0)
                                    {


#region Step Conversation Speech
                                        story_edit_current_speech = (story_edit_current_speech > 0) ? story_edit_current_speech : 1;
                                        UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector begin");
                                        UnityEngine.Debug.Log("### NCIMC - Step Conversation story_edit_current_step=" + story_edit_current_step);
                                        UnityEngine.Debug.Log("### NCIMC - Step Conversation story_edit_current_speech=" + story_edit_current_speech);
                                        LCARS_NCI_Mission_Conversation_Speech Speech = mission.steps[story_edit_current_step].Conversation.SpeechList[story_edit_current_speech];
                                        GUILayout.BeginHorizontal();
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector 1 ");

                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation  SpeechNav start ");
                                        GUILayout.BeginVertical(GUILayout.Width(120));
                                        if (story_edit_current_speech > 1)
                                        {
                                            if (GUILayout.Button("<< Speech " + (story_edit_current_speech - 1)))
                                            { story_edit_current_speech--; }
                                        }
                                        else { GUILayout.Label(""); }
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector 2 ");
                                        GUILayout.EndVertical();
                                        GUILayout.BeginVertical(GUILayout.Width(250));
                                        GUILayout.Label("                      Speech " + Speech.textID);
                                        GUILayout.EndVertical();
                                        GUILayout.BeginVertical(GUILayout.Width(120));
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector 3 ");
                                        if (story_edit_current_speech < mission.steps[story_edit_current_step].Conversation.SpeechList.Count)
                                        {
                                            if (GUILayout.Button("Speech " + (story_edit_current_speech + 1) + " >>"))
                                            { story_edit_current_speech++; }
                                        }
                                        else { GUILayout.Label(""); }
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector 4 ");
                                        GUILayout.EndVertical();
                                        GUILayout.EndHorizontal();
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Speech Selector end ");


                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Delete this Speech start ");
                                        GUILayout.Label("********************************************************************");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Delete this Speech (no undo):");
                                        if (GUILayout.Button("Delete"))
                                        { 
                                            mission.steps[story_edit_current_step].Conversation.SpeechList.Remove(story_edit_current_speech);
                                            story_edit_current_speech = 0;
                                            Dictionary<int, LCARS_NCI_Mission_Conversation_Speech> tmp = new Dictionary<int, LCARS_NCI_Mission_Conversation_Speech>();
                                            foreach (LCARS_NCI_Mission_Conversation_Speech S in mission.steps[story_edit_current_step].Conversation.SpeechList.Values)
                                            {
                                                S.textID = tmp.Count + 1;
                                                tmp.Add((tmp.Count + 1), S);
                                            }
                                            mission.steps[story_edit_current_step].Conversation.SpeechList = tmp;
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.Label("Be Aware: if you delete a speech, the speech IDs will change, all references might have changed!");
                                        GUILayout.Label("********************************************************************");

                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Text start ");

                                        if (Speech.text == null)
                                        {
                                            Speech.text = "";
                                        }
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Text (" + Speech.text.Length+ "/500):");
                                        Speech.text = GUILayout.TextArea(Speech.text, 500, GUILayout.Width(200), GUILayout.Height(100));
                                        GUILayout.EndHorizontal();

                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Reward the User now begin ");
                                        GUILayout.BeginHorizontal();
                                        Speech.reward_player = GUILayout.Toggle(Speech.reward_player, "Reward the User now?:");
                                        GUILayout.EndHorizontal();

                                        if (Speech.reward_player)
                                        {
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Cash:");
                                            Speech.reward_cash = GUILayout.TextField(Speech.reward_cash, 50);
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Science:");
                                            Speech.reward_science = GUILayout.TextField(Speech.reward_science, 50);
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Reputation:");
                                            Speech.reward_reputation = GUILayout.TextField(Speech.reward_reputation, 50);
                                            GUILayout.EndHorizontal();

                                            if (Speech.reward_artefact == "")
                                            {
                                                if (mission.artefacts.list.Count == 0)
                                                {
                                                    GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                                }
                                                else
                                                {
                                                    GUILayout.BeginVertical();
                                                    GUILayout.Label("Select Artefact to give");
                                                    GUILayout.Label("*******************************");
                                                    foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                                    {
                                                        if (GUILayout.Button(invItem.name))
                                                        {
                                                            Speech.reward_artefact = invItem.idcode;
                                                        }
                                                    }
                                                    GUILayout.Label("*******************************");
                                                    GUILayout.EndVertical();
                                                }
                                            }
                                            else
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Player receives the Artefact: " + Speech.reward_artefact);
                                                if (GUILayout.Button("change")) { Speech.reward_artefact = ""; }
                                                GUILayout.EndHorizontal();
                                            }
                                        }
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Reward the User now end ");


                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Send Message at SpeechStart begin ");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Send eMail Message at SpeechStart:");
                                        if (Speech.speechStart_messageID_email == "")
                                        {
                                            GUILayout.BeginVertical();
                                            if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                            if (GUILayout.Button("none")) { Speech.speechStart_messageID_email = "none"; }
                                            foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                            {
                                                if (mM.messageType != "eMail") { continue; }
                                                if (GUILayout.Button("Message " + mM.messageID)) { Speech.speechStart_messageID_email = mM.messageID.ToString(); }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                        else
                                        {
                                            GUILayout.Label(Speech.speechStart_messageID_email);
                                            if (GUILayout.Button("change")) { Speech.speechStart_messageID_email = ""; }
                                        }
                                        GUILayout.EndHorizontal();
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Send Message at SpeechStart end ");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Send Console Message at SpeechStart:");
                                        if (Speech.speechStart_messageID_console == "")
                                        {
                                            GUILayout.BeginVertical();
                                            if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                            if (GUILayout.Button("none")) { Speech.speechStart_messageID_console = "none"; }
                                            foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                            {
                                                if (mM.messageType != "console") { continue; }
                                                if (GUILayout.Button("Message " + mM.messageID)) { Speech.speechStart_messageID_console = mM.messageID.ToString(); }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                        else
                                        {
                                            GUILayout.Label(Speech.speechStart_messageID_console);
                                            if (GUILayout.Button("change")) { Speech.speechStart_messageID_console = ""; }
                                        }
                                        GUILayout.EndHorizontal();
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Send Message at SpeechStart end ");

                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Send Screen Message at SpeechStart:");
                                        if (Speech.speechStart_messageID_screen == "")
                                        {
                                            GUILayout.BeginVertical();
                                            if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                            if (GUILayout.Button("none")) { Speech.speechStart_messageID_screen = "none"; }
                                            foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                            {
                                                if(mM.messageType!="screen"){continue;}
                                                if (GUILayout.Button("Message " + mM.messageID)) { Speech.speechStart_messageID_screen = mM.messageID.ToString(); }
                                            }
                                            GUILayout.EndVertical();
                                        }
                                        else
                                        {
                                            GUILayout.Label(Speech.speechStart_messageID_screen);
                                            if (GUILayout.Button("change")) { Speech.speechStart_messageID_screen = ""; }
                                        }
                                        GUILayout.EndHorizontal();
                                        //UnityEngine.Debug.Log("### NCIMC - Step Conversation Send Message at SpeechStart end ");

#endregion


#region Step Conversation Response Options
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Response Options:");
                                            if (GUILayout.Button("Add new Response"))
                                            {
                                                LCARS_NCI_Mission_Speech_Respons nR = new LCARS_NCI_Mission_Speech_Respons();
                                                nR.responsText = "";
                                                nR.responsEvent = "";
                                                nR.responsTextID = "";
                                                nR.responsCashAmount = "";
                                                nR.responsArtefact = "";
                                                //nR.isStepEnd = false;
                                                Speech.ResponsList.Add(nR);
                                            }
                                        GUILayout.EndHorizontal();

                                        GUILayout.BeginVertical();
                                            if (Speech.ResponsList.Count == 0)
                                            {
                                                GUILayout.Label("You have not defined any response!");
                                            }
                                            else
                                            {
                                                GUILayout.BeginVertical();
                                                foreach (LCARS_NCI_Mission_Speech_Respons R in Speech.ResponsList)
                                                {
                                                    GUILayout.Label("*******************************");
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Respons:");
                                                    if (GUILayout.Button("Delete this Response")) { Speech.ResponsList.Remove(R); }
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                        GUILayout.Label("Text (" + R.responsText.Length + "/100):");
                                                        R.responsText = GUILayout.TextField(R.responsText, 100, style2_TextField);
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    if (R.responsEvent != "")
                                                    {
                                                        GUILayout.Label("Response Event " + R.responsEvent);
                                                        if (GUILayout.Button("change")) { R.responsEvent = ""; }
                                                    }
                                                    else
                                                    {
                                                        GUILayout.BeginVertical();
                                                        GUILayout.Label("***************");
                                                        GUILayout.Label("Select Response Event:");
                                                        GUILayout.BeginHorizontal();
                                                        if (GUILayout.Button("GoTo Speech")) { R.responsEvent = "textID"; }
                                                        //if (GUILayout.Button("Sign Contract")) { R.responsEvent = "sealdeal"; }
                                                        if (GUILayout.Button("End Conversation")) { R.responsEvent = "closeWindow"; }
                                                        if (GUILayout.Button("Give Artefact")) { R.responsEvent = "giveArtefact"; }
                                                        if (GUILayout.Button("Give Cash")) { R.responsEvent = "giveCash"; }
                                                        GUILayout.EndHorizontal();
                                                        GUILayout.Label("**************");
                                                        GUILayout.EndVertical();
                                                    }
                                                    GUILayout.EndHorizontal();

                                                    if (R.responsEvent == "giveArtefact")
                                                    {
                                                        GUILayout.BeginHorizontal();
                                                        if (R.responsArtefact == "")
                                                        {
                                                            if (LCARS_NCI.Instance.Data.Artefact_List.Count == 0)
                                                            {
                                                                GUILayout.Label("You have no Artefacts to select! Complete the Mission Assets.");
                                                            }
                                                            else
                                                            {
                                                                GUILayout.BeginVertical();
                                                                GUILayout.Label("***************");
                                                                GUILayout.Label("Select the Artefact the Player has to give away!");
                                                                foreach (KeyValuePair<string, LCARS_NCI_InventoryItem_Type> pair in NCI.Data.Artefact_List)
                                                                {
                                                                    LCARS_NCI_InventoryItem_Type invItem = pair.Value;
                                                                    if (GUILayout.Button(invItem.name))
                                                                    {
                                                                        R.responsArtefact = invItem.idcode;
                                                                    }
                                                                }
                                                                GUILayout.Label("***************");
                                                                GUILayout.EndVertical();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            GUILayout.Label("The Player gives away the Artefact: " + R.responsArtefact);
                                                            if (GUILayout.Button("change")) { R.responsArtefact = ""; }
                                                        }
                                                        GUILayout.EndHorizontal();
                                                    }
                                                    if (R.responsEvent == "closeWindow")
                                                    {
                                                        if (R.responsStepID == "")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Stay in Step " + Step.id + ":");
                                                                if (GUILayout.Button("closeWindow ")) { R.responsStepID = Step.id.ToString(); }
                                                            GUILayout.EndHorizontal();
                                                            GUILayout.Label("This will END the current conversation and start the first Job of Step " + Step.id + "!");
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Or go to Step:");
                                                            GUILayout.BeginVertical();
                                                            if ((mission.steps.Count - 1) == 0) { GUILayout.Label("No other Steps defined!"); }
                                                            foreach (LCARS_NCI_Mission_Step mS in mission.steps.Values)
                                                            {
                                                                if (mS.id == Step.id) { continue; }
                                                                if (GUILayout.Button("Step " + mS.id)) { R.responsStepID = mS.id.ToString(); }
                                                            }
                                                            if (GUILayout.Button("MissionEnd")) { R.responsStepID = "missionEnd"; }
                                                            GUILayout.EndVertical();
                                                            GUILayout.EndHorizontal();
                                                            GUILayout.Label("This will END the current Step (and conversation) and move on to the selected Step and start the first Job there! Use with care for continuity.");
                                                        }
                                                        else
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label(R.responsStepID);
                                                            if (GUILayout.Button("change")) { R.responsStepID = ""; }
                                                            GUILayout.EndHorizontal();
                                                        }
                                                    }
                                                    if (R.responsEvent == "giveCash")
                                                    {
                                                        GUILayout.BeginHorizontal(GUILayout.Width(420));
                                                        GUILayout.Label("Cash Amount:", GUILayout.Width(200));
                                                        R.responsCashAmount = GUILayout.TextField(R.responsCashAmount, 30, style_TextField);
                                                        GUILayout.EndHorizontal();
                                                    }
                                                    if (R.responsEvent == "textID" || R.responsEvent == "giveArtefact" || R.responsEvent == "sealdeal" || R.responsEvent == "giveCash")
                                                    {
                                                        GUILayout.Label("Select the Next Speech to go to!");
                                                        GUILayout.BeginHorizontal();
                                                            if (R.responsTextID == "")
                                                            {
                                                                if (mission.steps[story_edit_current_step].Conversation.SpeechList.Count < 2)
                                                                {
                                                                    GUILayout.Label("You have no Speeches to select! Add some.");
                                                                }
                                                                else
                                                                { 
                                                                    GUILayout.BeginVertical();
                                                                    foreach (LCARS_NCI_Mission_Conversation_Speech oSp in mission.steps[story_edit_current_step].Conversation.SpeechList.Values)
                                                                    {
                                                                        if (oSp.textID != Speech.textID)
                                                                        {
                                                                            if (GUILayout.Button("Speech " + oSp.textID))
                                                                            {
                                                                                R.responsTextID = oSp.textID.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                    GUILayout.EndVertical();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label("We go to Speech: "+R.responsTextID);
                                                                if (GUILayout.Button("change")) { R.responsTextID = ""; }
                                                            }
                                                        GUILayout.EndHorizontal();
                                                    }

                                                    GUILayout.BeginHorizontal();
                                                        //R.isStepEnd = GUILayout.Toggle(R.isStepEnd, "Is this the end of Step " + story_edit_current_step + "?");
                                                    GUILayout.EndHorizontal();

                                                }
                                                GUILayout.EndVertical();
                                            }
                                        GUILayout.EndVertical();
#endregion



                                    }

                                }

#endregion
                            }
                            if (step_formtoggle_messages)
                            {
#region Step Messages

#region Step Messages Top

                                //UnityEngine.Debug.Log("### NCIMC - Step Messages begin ");
                                if (mission.steps[story_edit_current_step].Messages == null)
                                {
                                    mission.steps[story_edit_current_step].Messages = new LCARS_NCI_Mission_Messages();
                                    mission.steps[story_edit_current_step].Messages.MessageList = new Dictionary<int, LCARS_NCI_Mission_Message>();
                                }
                                        
                                //UnityEngine.Debug.Log("### NCIMC - Step Messages 1");
                                        
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("You have " + mission.steps[story_edit_current_step].Messages.MessageList.Count + " Messages defined.  ");
                                if (GUILayout.Button("Add new Message"))
                                {
                                    LCARS_NCI_Mission_Message Message = new LCARS_NCI_Mission_Message();
                                    Message.messageID = mission.steps[story_edit_current_step].Messages.MessageList.Count + 1;
                                    Message.message  = "";
                                    Message.title  = "";
                                    Message.sender  = "";
                                    Message.sender_id_line  = ""; // like subject
                                    Message.priority = 3;
                                    //Message.reply_code = "";
                                    Message.Encrypted = false;
                                    Message.encryptionMode  = ""; // normal, broken, unbreakable, specialItem
                                    Message.decryption_artefact = ""; // special Item
                                    //Message.reply_sent  = false;
                                    Message.reply_options = new List<LCARS_NCI_Mission_Message_Reply>();

                                    Message.messageType = "eMail"; // eMail,console,screen
                                    Message.loopMessage = false; // only for console and screen
                                    
                                    //Message.loopCondition = ""; // end of job, end of step,
                                    //Message.loopCondition_id = 0; // jobID or stepID
                                    //Message.conditions = new LCARS_NCI_Mission_Message_Conditions();
                                    //Message.conditions.condition = "none";
                                    //Message.conditions.artefact_idcode = "";
                                    //Message.conditions.part_idcode = "";
                                    //Message.conditions.egg_idcode = "";
                                    //Message.conditions.distance = "";
                                    //Message.conditions.jobID = "";
                                    //Message.conditions.stepID = "";

                                    mission.steps[story_edit_current_step].Messages.MessageList.Add((mission.steps[story_edit_current_step].Messages.MessageList.Count + 1), Message);
                                }
                                GUILayout.EndHorizontal();

                                //UnityEngine.Debug.Log("### NCIMC - Step Messages 2");

#endregion



                                if (mission.steps[story_edit_current_step].Messages.MessageList.Count > 0)
                                {

                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages 3");
                                    if (story_edit_current_message > 0)
                                    {

#region Step Messages Options Nav
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("Message: " + story_edit_current_message + "");
                                        if (GUILayout.Button("Back")) { story_edit_current_message = 0; }
                                        GUILayout.EndHorizontal();
                                        GUILayout.Label("*********************************************");

                                        LCARS_NCI_Mission_Message editM = mission.steps[story_edit_current_step].Messages.MessageList[story_edit_current_message];

                                        GUILayout.BeginHorizontal();
                                        if (message_formtoggle_options == false && message_formtoggle_contents == false && message_formtoggle_responses == false)
                                        {
                                            GUILayout.Label("Message-Sections:");
                                            if (GUILayout.Button("Options & Conditions"))
                                            {
                                                message_formtoggle_options = true;
                                                message_formtoggle_contents = false;
                                                message_formtoggle_responses = false;
                                            }
                                            if (GUILayout.Button("Message Content"))
                                            {
                                                message_formtoggle_options = false;
                                                message_formtoggle_contents = true;
                                                message_formtoggle_responses = false;
                                            }
                                            if (GUILayout.Button("Responses"))
                                            {
                                                message_formtoggle_options = false;
                                                message_formtoggle_contents = false;
                                                message_formtoggle_responses = true;
                                            }
                                        }
                                        else
                                        {
                                            if (message_formtoggle_options)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Message Options / Conditions:");
                                                if (GUILayout.Button("Back")) { message_formtoggle_options = false; }
                                                GUILayout.EndHorizontal();
                                            }
                                            if (message_formtoggle_contents)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Message Content:");
                                                if (GUILayout.Button("Back")) { message_formtoggle_contents = false; }
                                                GUILayout.EndHorizontal();
                                            }
                                            if (message_formtoggle_responses)
                                            {
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Message Responses:");
                                                if (GUILayout.Button("Back")) { message_formtoggle_responses = false; }
                                                GUILayout.EndHorizontal();
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.Label("********************************************************************");

#endregion


                                        if (message_formtoggle_options)
                                        {
#region Message Options
                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages options 1 ");

                                            GUILayout.BeginHorizontal();
                                            if (editM.messageType == "")
                                            {
                                                GUILayout.BeginVertical();
                                                GUILayout.Label("Select Type");
                                                if (GUILayout.Button("LCARS-eMail")) { editM.messageType = "eMail"; }
                                                if (GUILayout.Button("NCI-Console")) { editM.messageType = "console"; }
                                                if (GUILayout.Button("KSP-ScreenMessage")) { editM.messageType = "screen"; }
                                                GUILayout.EndVertical();
                                            }
                                            else
                                            {
                                                GUILayout.Label("Type: " + editM.messageType);
                                                if (GUILayout.Button("change")) { editM.messageType = ""; }
                                            }
                                            GUILayout.EndHorizontal();

                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages options 2 ");

                                            if (editM.messageType == "screen" || editM.messageType == "console")
                                            {
                                                GUILayout.BeginHorizontal();
                                                editM.loopMessage = GUILayout.Toggle(editM.loopMessage,"This is a loop message?");
                                                GUILayout.EndHorizontal();
                                            }

#region obsolete
/*
                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages options 3 ");

                                            if (editM.loopMessage)
                                            {
                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 4 ");

                                                GUILayout.BeginHorizontal();
                                                if (editM.loopCondition == "")
                                                {
                                                    GUILayout.BeginVertical();
                                                    GUILayout.Label("Loop Condition");
                                                    if (GUILayout.Button("End of Job")) { editM.loopCondition = "end_of_job"; }
                                                    if (GUILayout.Button("End of Step")) { editM.loopCondition = "end_of_step"; }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label("Loop Condition: " + editM.loopCondition);
                                                    if (GUILayout.Button("change")) { editM.loopCondition = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                            }
                                            else 
                                            {
                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 5 ");

                                                GUILayout.BeginHorizontal();
                                                if (editM.conditions.condition == "")
                                                {
                                                    GUILayout.BeginVertical();
                                                    GUILayout.Label("Send Condition");
                                                    if (GUILayout.Button("None")) { editM.conditions.condition = "none"; }
                                                    if (GUILayout.Button("distanceLower")) { editM.conditions.condition = "distanceLower"; }
                                                    if (GUILayout.Button("distanceGreater")) { editM.conditions.condition = "distanceGreater"; }
                                                    if (GUILayout.Button("objectScanned")) { editM.conditions.condition = "objectScanned"; }
                                                    if (GUILayout.Button("objectDestroyed")) { editM.conditions.condition = "objectDestroyed"; }
                                                    if (GUILayout.Button("artefactCollected")) { editM.conditions.condition = "artefactCollected"; }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label("Send Condition: " + editM.conditions.condition);
                                                    if (GUILayout.Button("change")) { editM.conditions.condition = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 6 ");

                                                if (editM.conditions.condition == "distanceLower" || editM.conditions.condition == "distanceGreater")
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Distance: ");
                                                    editM.conditions.distance = GUILayout.TextField(editM.conditions.distance, 100);
                                                    GUILayout.EndHorizontal();
                                                    string[] message_condition_distance_to_list = new string[] { "Object", "Egg" };
                                                    message_condition_distance_to = GUILayout.SelectionGrid(message_condition_distance_to, message_condition_distance_to_list,2);

                                                }

                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 7 ");

                                                if ((editM.conditions.condition == "distanceLower" || editM.conditions.condition == "distanceGreater") && message_condition_distance_to == 1)
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    if (editM.conditions.egg_idcode == "")
                                                    {
                                                    GUILayout.Label("Select Target Egg:");
                                                        if (mission.requirements.list.Count == 0)
                                                        {
                                                            GUILayout.Label("Please create the Mission Assets first");
                                                        }
                                                        GUILayout.BeginVertical();
                                                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                        {
                                                            LCARS_NCI_Mission_Requirement mE = pair.Value;
                                                            //GUILayout.Label("Part " + mE.name + " contains:");
                                                            foreach (string eggstring in mE.actionpoints)
                                                            {
                                                                if (GUILayout.Button(eggstring + " at " + mE.name))
                                                                {
                                                                    editM.conditions.part_idcode = mE.part_idcode;
                                                                    editM.conditions.egg_idcode = eggstring;
                                                                }
                                                            }
                                                        }
                                                        GUILayout.EndVertical();
                                                    }
                                                    else
                                                    {
                                                        GUILayout.Label("Target Egg: " + editM.conditions.egg_idcode + " at " + editM.conditions.part_idcode);
                                                        if (GUILayout.Button("change")) { editM.conditions.egg_idcode = ""; }
                                                    }
                                                    GUILayout.EndHorizontal();
                                                }

                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 8 ");

                                                if (editM.conditions.condition == "objectScanned" || editM.conditions.condition == "objectDestroyed" || ((editM.conditions.condition == "distanceLower" || editM.conditions.condition == "distanceGreater") && message_condition_distance_to == 0))
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    if (editM.conditions.part_idcode == "")
                                                    {
                                                    GUILayout.Label("Target Object:");
                                                        if (mission.requirements.list.Count == 0)
                                                        {
                                                            GUILayout.Label("Please create the Mission Assets first");
                                                        }
                                                        GUILayout.BeginVertical();
                                                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                        {
                                                            LCARS_NCI_Mission_Requirement mE = pair.Value;
                                                            if (GUILayout.Button(mE.name))
                                                            {
                                                                editM.conditions.part_idcode = mE.part_idcode;
                                                            }
                                                        }
                                                        GUILayout.EndVertical();
                                                    }
                                                    else
                                                    {
                                                        GUILayout.Label("Target Object: " + editM.conditions.part_idcode);
                                                        if (GUILayout.Button("change")) { editM.conditions.part_idcode = ""; }
                                                    }
                                                    GUILayout.EndHorizontal();
                                                }

                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 9 ");

                                                if (editM.conditions.condition == "artefactCollected")
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    if (editM.conditions.artefact_idcode == "")
                                                    {
                                                        if (mission.artefacts.list.Count == 0)
                                                        {
                                                            GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                                        }
                                                        else
                                                        {
                                                            GUILayout.BeginVertical();
                                                            GUILayout.Label("Select Artefact");
                                                            foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                                            {
                                                                if (GUILayout.Button(invItem.name))
                                                                {
                                                                    editM.conditions.artefact_idcode = invItem.idcode;
                                                                }
                                                            }
                                                            GUILayout.EndVertical();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        GUILayout.Label("Artefact: " + editM.conditions.artefact_idcode);
                                                        if (GUILayout.Button("change")) { editM.conditions.artefact_idcode = ""; }
                                                    }
                                                    GUILayout.EndHorizontal();
                                                }
                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages options 10 ");
                                            }
*/
#endregion 



                                            GUILayout.Label("********************************************************************");
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Delete this Message (no undo):");
                                            if (GUILayout.Button("Delete"))
                                            {
                                                mission.steps[story_edit_current_step].Messages.MessageList.Remove(story_edit_current_message);
                                                story_edit_current_message = 0;
                                                Dictionary<int, LCARS_NCI_Mission_Message> tmp = new Dictionary<int, LCARS_NCI_Mission_Message>();
                                                foreach (LCARS_NCI_Mission_Message S in mission.steps[story_edit_current_step].Messages.MessageList.Values)
                                                {
                                                    S.messageID = tmp.Count + 1;
                                                    tmp.Add((tmp.Count + 1), S);
                                                }
                                                mission.steps[story_edit_current_step].Messages.MessageList = tmp;
                                            }
                                            GUILayout.EndHorizontal();
                                            GUILayout.Label("Be Aware: if you delete a message, the message IDs will change, all references might have changed!");
                                            GUILayout.Label("********************************************************************");


#endregion
                                        }


                                        if (message_formtoggle_contents)
                                        {

#region Message Contents
                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 1 ");

                                            if (editM.messageType == "eMail")
                                            {
                                                if (editM.sender == "")
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Who shall send this message?:");
                                                    if (mission.personalities.list.Count == 0)
                                                    {
                                                        GUILayout.Label("You have no Personalities to select! Complete the Assets.");
                                                    }
                                                    else
                                                    {
                                                        GUILayout.BeginVertical();
                                                        GUILayout.Label("Select Personality");
                                                        foreach (LCARS_NCI_Personality P in mission.personalities.list)
                                                        {
                                                            if (GUILayout.Button(P.name)) { editM.sender = P.idcode; }
                                                        }
                                                        GUILayout.EndVertical();
                                                    }
                                                    GUILayout.EndHorizontal();
                                                }
                                                else
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Personality: " + editM.sender);
                                                    if (GUILayout.Button("change")) { editM.sender = ""; }
                                                    GUILayout.EndHorizontal();
                                                }
                                            }

                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 ");

                                            if (editM.sender != "" || editM.messageType != "eMail")
                                            {

                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 1");

                                                if (editM.messageType == "eMail")
                                                {
                                                    if (editM.sender != "")
                                                    {
                                                        editM.sender_id_line = (editM.sender_id_line != "") ? editM.sender_id_line : mission.personalities.getPersonalityByName(editM.sender).name + " reports";
                                                    }
                                                            
                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 2");

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Subject Line: ");
                                                    editM.sender_id_line = GUILayout.TextField(editM.sender_id_line, 100);
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Title: ");
                                                    editM.title = GUILayout.TextField(editM.title, 250);
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Text: ");
                                                    editM.message = GUILayout.TextArea(editM.message, 500, GUILayout.Width(200), GUILayout.Height(100));
                                                    GUILayout.EndHorizontal();

                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 3");

                                                }
                                                else 
                                                {

                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 4");

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Message Line: ");
                                                    GUILayout.BeginVertical();
                                                        editM.title = GUILayout.TextField(editM.title, 500);
                                                        GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Available Keywords:");
                                                            if (GUILayout.Button("[DISTANCE]"))
                                                            {
                                                                editM.title = editM.title + " [DISTANCE] ";
                                                            }
                                                            GUILayout.EndHorizontal();
                                                    GUILayout.EndVertical();
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Optional) Select Target Egg for [DISTANCE] calculation:");
                                                    if (editM.NCI_egg == "")
                                                    {
                                                        if (mission.requirements.list.Count == 0)
                                                        {
                                                            GUILayout.Label("Please create the Mission Assets first");
                                                        }
                                                        GUILayout.BeginVertical();
                                                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                        {
                                                            LCARS_NCI_Mission_Requirement mR = pair.Value;
                                                            if (mR.isCelestialBody)
                                                            {
                                                                foreach (KeyValuePair<string, List<string>> pair2 in mR.Locations)
                                                                {
                                                                    string location = pair2.Key;
                                                                    List<string> actionpoints = pair2.Value;
                                                                    foreach (string s in actionpoints)
                                                                    {
                                                                        if (GUILayout.Button(s + " at " + location + " on " + mR.name))
                                                                        {
                                                                            editM.requirement_id = mR.id;
                                                                            editM.NCI_body = mR.part_idcode;
                                                                            editM.NCI_PQSCity = location;
                                                                            editM.NCI_object = "";
                                                                            editM.NCI_egg = s;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {

                                                                //GUILayout.Label("Part " + mR.name + " contains:");
                                                                foreach (string eggstring in mR.actionpoints)
                                                                {
                                                                    if (GUILayout.Button(eggstring + " at " + mR.name))
                                                                    {
                                                                        editM.NCI_body = "";
                                                                        editM.NCI_PQSCity = "";
                                                                        editM.NCI_object = mR.part_idcode;
                                                                        editM.NCI_egg = eggstring;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        GUILayout.EndVertical();
                                                    }
                                                    else
                                                    {
                                                        if (editM.NCI_object != "")
                                                        {
                                                            GUILayout.Label("Target Egg: " + editM.NCI_egg + " at " + editM.NCI_object);
                                                            if (GUILayout.Button("change")) { editM.NCI_egg = ""; }
                                                        }
                                                        else
                                                        {
                                                            GUILayout.Label("Target Egg: " + editM.NCI_egg + " at " + editM.NCI_PQSCity + " on " + editM.NCI_body);
                                                            if (GUILayout.Button("change")) { editM.NCI_egg = ""; }
                                                        }
                                                    }
                                                    GUILayout.EndHorizontal();


                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 5");

                                                }

                                                if (editM.messageType == "eMail")
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    if (editM.priority == 0)
                                                    {

                                                        //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 6");

                                                        GUILayout.Label("Select Priority");
                                                        GUILayout.BeginVertical();
                                                        if (GUILayout.Button("1")) { editM.priority = 1; }
                                                        if (GUILayout.Button("2")) { editM.priority = 2; }
                                                        if (GUILayout.Button("3")) { editM.priority = 3; }
                                                        if (GUILayout.Button("4")) { editM.priority = 4; }
                                                        if (GUILayout.Button("5")) { editM.priority = 5; }
                                                        GUILayout.EndVertical();

                                                        //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 7");

                                                    }
                                                    else
                                                    {

                                                        //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 8");

                                                        GUILayout.Label("Priority: " + editM.priority);
                                                        if (GUILayout.Button("change")) { editM.priority = 0; }

                                                        //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 2 9");

                                                    }
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    editM.Encrypted = GUILayout.Toggle(editM.Encrypted, "Send Encrypted?");
                                                    GUILayout.EndHorizontal();

                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 3 ");


                                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 4 ");

                                                    if (editM.Encrypted)
                                                    {
                                                        GUILayout.BeginHorizontal();
                                                        if (editM.encryptionMode == "")
                                                        {
                                                            GUILayout.BeginVertical();
                                                            GUILayout.Label("Encryption Mode");
                                                            if (GUILayout.Button("Normal")) { editM.encryptionMode = "normal"; }
                                                            if (GUILayout.Button("Broken")) { editM.encryptionMode = "broken"; }
                                                            if (GUILayout.Button("Unbreakable")) { editM.encryptionMode = "unbreakable"; }
                                                            if (GUILayout.Button("SpecialItem")) { editM.encryptionMode = "specialItem"; }
                                                            GUILayout.EndVertical();
                                                        }
                                                        else
                                                        {
                                                            GUILayout.Label("Encryption Mode: " + editM.encryptionMode);
                                                            if (GUILayout.Button("change")) { editM.encryptionMode = ""; }
                                                        }
                                                        GUILayout.EndHorizontal();


                                                        //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 5 ");

                                                        if (editM.encryptionMode == "specialItem")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            if (editM.decryption_artefact == "")
                                                            {
                                                                if (mission.artefacts.list.Count == 0)
                                                                {
                                                                    GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                                                }
                                                                else
                                                                {
                                                                    GUILayout.BeginVertical();
                                                                    GUILayout.Label("Select Decrypt-Artefact");
                                                                    GUILayout.Label("****************************************");
                                                                    if (mission.artefacts.list.Count==0)
                                                                    {
                                                                        GUILayout.Label("Bo NCI-Artefact available - Complete Assets!");
                                                                    }
                                                                    foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                                                    {
                                                                        if (GUILayout.Button(invItem.name))
                                                                        {
                                                                            editM.decryption_artefact = invItem.idcode;
                                                                        }
                                                                    }
                                                                    GUILayout.Label("****************************************");
                                                                    GUILayout.EndVertical();
                                                                }

                                                                //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 6 ");

                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label("Decrypt-Artefact: " + editM.decryption_artefact);
                                                                if (GUILayout.Button("change")) { editM.decryption_artefact = ""; }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                        }
                                                    }
                                                }
                                            }

                                            //UnityEngine.Debug.Log("### NCIMC - Step Messages contents 7 ");

#endregion
                                        }

                                        if (message_formtoggle_responses)
                                        {
#region Message Responses

                                            if (editM.messageType == "eMail")
                                            {
                                                if (GUILayout.Button("Add new Reply"))
                                                {
                                                    LCARS_NCI_Mission_Message_Reply reply = new LCARS_NCI_Mission_Message_Reply();
                                                    reply.replyCode = "none"; // goto: none, message, step, job
                                                    reply.buttonText = "";
                                                    reply.replyID = ""; // message, step, job
                                                    editM.reply_options.Add(reply);
                                                }
                                                if (editM.reply_options.Count == 0)
                                                {
                                                    GUILayout.Label("No ReplyOptions defined");
                                                }
                                                foreach(LCARS_NCI_Mission_Message_Reply mR in editM.reply_options)
                                                {
                                                    GUILayout.Label("*************************************************");

                                                    if (mR.buttonText==null)
                                                    {
                                                        mR.buttonText = "";
                                                    }

                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("Button Text: ");
                                                    mR.buttonText = GUILayout.TextField(mR.buttonText, 100);
                                                    GUILayout.EndHorizontal();

                                                    GUILayout.BeginHorizontal();
                                                    if (mR.replyCode != "")
                                                    {
                                                        GUILayout.Label("Reply Event " + mR.replyCode);
                                                        if (GUILayout.Button("change")) { mR.replyCode = ""; }
                                                    }
                                                    else
                                                    {
                                                        GUILayout.BeginVertical();
                                                        GUILayout.Label("***************");
                                                        GUILayout.Label("Select Reply Event:");
                                                        GUILayout.BeginHorizontal();
                                                        if (GUILayout.Button("None")) { mR.replyCode = "none"; }
                                                        if (GUILayout.Button("Message")) { mR.replyCode = "message"; }
                                                        if (GUILayout.Button("Step")) { mR.replyCode = "step"; }
                                                        if (GUILayout.Button("Job")) { mR.replyCode = "job"; }
                                                        GUILayout.EndHorizontal();
                                                        GUILayout.Label("**************");
                                                        GUILayout.EndVertical();
                                                    }
                                                    GUILayout.EndHorizontal();

                                                    switch (mR.replyCode)
                                                    {
                                                        case"message":
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Send Message:");
                                                            if (mR.replyID == "")
                                                            {
                                                                GUILayout.BeginVertical();
                                                                if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No other messages defined!"); }
                                                                foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                                {
                                                                    if (mM == editM) { continue; }
                                                                    if (GUILayout.Button("Message " + mM.messageID)) { mR.replyID = mM.messageID.ToString(); }
                                                                }
                                                                GUILayout.EndVertical();
                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label("Message " + mR.replyID);
                                                                if (GUILayout.Button("change")) { mR.replyID = ""; }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                                GUILayout.Label("The player will receive the selected Message. Works best with Type eMail");
                                                            break;

                                                        case"step":
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Go to Step:");
                                                            if (mR.replyID == "")
                                                            {
                                                                GUILayout.BeginVertical();
                                                                if ((mission.steps.Count - 1) == 0) { GUILayout.Label("No other Steps defined!"); }
                                                                foreach (LCARS_NCI_Mission_Step mS in mission.steps.Values)
                                                                {
                                                                    if (mS.id == Step.id) { continue; }
                                                                    if (GUILayout.Button("Step " + mS.id)) { mR.replyID = mS.id.ToString(); }
                                                                }
                                                                if (GUILayout.Button("MissionEnd")) { mR.replyID = "missionEnd"; }
                                                                GUILayout.EndVertical();
                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label(mR.replyID);
                                                                if (GUILayout.Button("change")) { mR.replyID = ""; }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                                GUILayout.Label("This will END the current Step and move on to the selected Step! Use with care for continuity.");
                                                            break;

                                                        case"job":
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Go to Job:");
                                                            if (mR.replyID == "")
                                                            {
                                                                if (mission.steps[story_edit_current_step].Jobs == null)
                                                                {
                                                                    mission.steps[story_edit_current_step].Jobs = new LCARS_NCI_Mission_Step_Jobs();
                                                                    mission.steps[story_edit_current_step].Jobs.jobList = new Dictionary<int, LCARS_NCI_Mission_Job>();
                                                                }
                                                                GUILayout.BeginVertical();
                                                                if ((mission.steps[story_edit_current_step].Jobs.jobList.Count - 1) == 0) { GUILayout.Label("No Jobs defined!"); }
                                                                foreach (LCARS_NCI_Mission_Job J in mission.steps[story_edit_current_step].Jobs.jobList.Values)
                                                                {
                                                                    if (GUILayout.Button("Step " + story_edit_current_step + " Job " + J.jobID)) { mR.replyID = J.jobID.ToString(); }
                                                                }
                                                                GUILayout.EndVertical();
                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label(mR.replyID);
                                                                if (GUILayout.Button("change")) { mR.replyID = ""; }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                            GUILayout.Label("This will END the active Job and move on to the selected Job! Use with care for continuity.");
                                                            break;

                                                    }

                                                }

                                            }
                                            else
                                            {
                                                GUILayout.Label("Only 'Type: eMail' can send a response");
                                            }
                                            /*
    public List<LCARS_NCI_Mission_Message_Reply> reply_options { get; set; }
    public void addReply(LCARS_NCI_Mission_Message_Reply r)
    public string buttonText { set; get; }
    public string replyCode { set; get; } // goto: message, step, job
    public string messageID { set; get; }
                                        */
#endregion
                                        }







                                    }
                                    else
                                    {
#region Message List
                                        foreach (KeyValuePair<int, LCARS_NCI_Mission_Message> pair in mission.steps[story_edit_current_step].Messages.MessageList)
                                        {
                                            LCARS_NCI_Mission_Message M = pair.Value;

                                            GUILayout.BeginVertical();
                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("MessageID: " + M.messageID);
                                            GUILayout.Label("Type: " + M.messageType);
                                            if (GUILayout.Button("Edit")) { story_edit_current_message = M.messageID; }
                                            GUILayout.EndHorizontal();

                                            GUILayout.BeginHorizontal();
                                            GUILayout.Label("Title: " + M.title);
                                            GUILayout.Label("Sender: " + M.sender);
                                            GUILayout.EndHorizontal();
                                            GUILayout.Label("********************");
                                            GUILayout.EndVertical();

                                        }
#endregion
                                    }
                                    //UnityEngine.Debug.Log("### NCIMC - Step Messages 4");

                                }
                                else 
                                {
                                    GUILayout.Label("No messages to list..");
                                }

#endregion
                            }
                            if (step_formtoggle_jobs)
                            {

#region Step Jobs
                                        if (mission.steps[story_edit_current_step].Jobs == null)
                                        {
                                            mission.steps[story_edit_current_step].Jobs = new LCARS_NCI_Mission_Step_Jobs();
                                            mission.steps[story_edit_current_step].Jobs.jobList = new Dictionary<int,LCARS_NCI_Mission_Job>();
                                        }
                                        GUILayout.Label("Job 1 starts when this Step starts. Job 2 starts when Job 1 is finished, you can add unlimited jobs per step");
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("You have " + mission.steps[story_edit_current_step].Jobs.jobList.Count + " Jobs defined.  ");
                                        if (GUILayout.Button("Add new Job"))
                                        {
                                            LCARS_NCI_Mission_Job Job = new LCARS_NCI_Mission_Job();
                                            Job.jobID = mission.steps[story_edit_current_step].Jobs.jobList.Count + 1;
                                            Job.isStepEnd  = false;
                                            Job.jobtype = ""; // distance, destroy, scan, collect
                                            Job.jobStart_messageID_email = "none";
                                            Job.jobStart_messageID_console = "none";
                                            Job.jobStart_messageID_screen = "none";
                                            Job.jobEnd_messageID_email = "none";
                                            Job.jobEnd_messageID_console = "none";
                                            Job.jobEnd_messageID_screen = "none";
                                            Job.target = ""; // object (distance, destroy, scan), egg (distance, scan), artefact (collect)
                                            Job.distance = ""; // how close is required meter
                                            Job.NCI_object = ""; // what object idcode
                                            Job.NCI_egg  = ""; // what artefact idcode
                                            Job.NCI_artefact = ""; // what artefact idcode
                                            mission.steps[story_edit_current_step].Jobs.jobList.Add(Job.jobID,Job);
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.Label("********************************************************************");

                                        if (mission.steps[story_edit_current_step].Jobs.jobList.Count > 0)
                                        {
                                            int jCounter = 0;
                                            foreach (LCARS_NCI_Mission_Job J in mission.steps[story_edit_current_step].Jobs.jobList.Values)
                                            {
                                                jCounter++;
                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Delete Job " + jCounter + "(no undo): ");
                                                if (GUILayout.Button("Delete"))
                                                {
                                                    mission.steps[story_edit_current_step].Jobs.jobList.Remove(J.jobID);

                                                    Dictionary<int, LCARS_NCI_Mission_Job> tmp = new Dictionary<int, LCARS_NCI_Mission_Job>();
                                                    foreach (LCARS_NCI_Mission_Job backupJ in mission.steps[story_edit_current_step].Jobs.jobList.Values)
                                                    {
                                                        backupJ.jobID = tmp.Count + 1;
                                                        tmp.Add((tmp.Count + 1), backupJ);
                                                    }
                                                    mission.steps[story_edit_current_step].Jobs.jobList = tmp;
                                                }
                                                GUILayout.EndHorizontal();

                                                if (J.target == "")
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("(physical) Job Target:");
                                                    if (GUILayout.Button("None")) 
                                                    { J.target = "none";     J.jobtype = ""; J.NCI_artefact = ""; J.distance = ""; J.NCI_egg = ""; J.NCI_object = ""; }
                                                    if (GUILayout.Button("Object")) 
                                                    { J.target = "object";   J.jobtype = ""; J.NCI_artefact = ""; J.distance = ""; J.NCI_egg = ""; J.NCI_object = ""; }
                                                    if (GUILayout.Button("Egg")) 
                                                    { J.target = "egg";      J.jobtype = ""; J.NCI_artefact = ""; J.distance = ""; J.NCI_egg = ""; J.NCI_object = ""; }
                                                    if (GUILayout.Button("Artefact")) 
                                                    { J.target = "artefact"; J.jobtype = ""; J.NCI_artefact = ""; J.distance = ""; J.NCI_egg = ""; J.NCI_object = ""; }
                                                    GUILayout.EndHorizontal();
                                                }
                                                else
                                                {
                                                    GUILayout.BeginHorizontal();
                                                    GUILayout.Label("(physical) Job Target:");
                                                    GUILayout.Label(J.target);
                                                    if (GUILayout.Button("change")) { J.target = ""; }
                                                    GUILayout.EndHorizontal();

                                                    if (J.jobtype == "")
                                                    {
                                                        GUILayout.Label("What shall be done with it?:");
                                                        GUILayout.BeginHorizontal();
                                                        if (J.target == "none")
                                                        {
                                                            if (GUILayout.Button("UserInput")) { J.jobtype = "awaitingUserInput"; }
                                                        }
                                                        if (J.target == "object" || J.target == "egg")
                                                        {
                                                            if (GUILayout.Button("DistanceLower")) { J.jobtype = "distanceLower"; }
                                                            if (GUILayout.Button("DistanceGreater")) { J.jobtype = "distanceGreater"; }
                                                        }
                                                        if (J.target == "object")
                                                        {
                                                            if (GUILayout.Button("Destroy")) { J.jobtype = "destroy"; }
                                                        }
                                                        if (J.target == "object")
                                                        {
                                                            if (GUILayout.Button("Scan")) { J.jobtype = "scan"; }
                                                        }
                                                        if (J.target == "artefact")
                                                        {
                                                            if (GUILayout.Button("Collect")) { J.jobtype = "collect"; }
                                                            if (GUILayout.Button("Drop")) { J.jobtype = "drop"; }
                                                        }
                                                        GUILayout.EndHorizontal();
                                                    }
                                                    else
                                                    {
                                                        GUILayout.BeginHorizontal();
                                                        GUILayout.Label("Job Type:");
                                                        GUILayout.Label(J.jobtype);
                                                        if (GUILayout.Button("change")) { J.jobtype = ""; }
                                                        GUILayout.EndHorizontal();

                                                        if (J.jobtype == "awaitingUserInput")
                                                        {
                                                            GUILayout.Label("This Job will end the automatic process and wait for some kind of predefined UserInput.");
                                                            GUILayout.Label("Please use only if you know what you are doing. ");
                                                            GUILayout.Label("If used wrong, it will prevent the mission from ending. ");
                                                        }

                                                        if (J.jobtype == "distanceLower" || J.jobtype == "distanceGreater" || J.jobtype == "scan")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Required Distance:");
                                                            J.distance = GUILayout.TextField(J.distance, 70);
                                                            GUILayout.Label(" Meter");
                                                            GUILayout.EndHorizontal();

                                                        }

                                                        if (J.target == "artefact")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            if (J.NCI_artefact == "")
                                                            {
                                                                if (mission.artefacts.list.Count == 0)
                                                                {
                                                                    GUILayout.Label("You have no Artefacts to select! Complete the Assets.");
                                                                }
                                                                else
                                                                {
                                                                    GUILayout.BeginVertical();
                                                                    GUILayout.Label("Select Target Artefact");
                                                                    foreach (LCARS_NCI_InventoryItem_Type invItem in mission.artefacts.list)
                                                                    {
                                                                        if (GUILayout.Button(invItem.name))
                                                                        {
                                                                            J.NCI_artefact = invItem.idcode;
                                                                        }
                                                                    }
                                                                    GUILayout.EndVertical();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                GUILayout.Label("Target Artefact: " + J.NCI_artefact);
                                                                if (GUILayout.Button("change")) { J.NCI_artefact = ""; }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                        }
                                                        if (J.target == "egg")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Select Target Egg:");
                                                            if (J.NCI_egg == "")
                                                            {
                                                                if (mission.requirements.list.Count == 0)
                                                                {
                                                                    GUILayout.Label("Please create the Mission Assets first");
                                                                }
                                                                GUILayout.BeginVertical();
                                                                foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                                {
                                                                    LCARS_NCI_Mission_Requirement mE = pair.Value;
                                                                    if (mE.isCelestialBody)
                                                                    {
                                                                        foreach (KeyValuePair<string, List<string>> pair2 in mE.Locations)
                                                                        {
                                                                            string location = pair2.Key;
                                                                            List<string> actionpoints = pair2.Value;
                                                                            foreach (string s in actionpoints)
                                                                            {
                                                                                if (GUILayout.Button(s + " at " + location + " on " + mE.name))
                                                                                {
                                                                                    J.requirement_id = pair.Key;
                                                                                    J.NCI_PQSCity = location;
                                                                                    J.NCI_body = mE.name;
                                                                                    J.NCI_object = "";
                                                                                    J.NCI_egg = s;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //GUILayout.Label("Part " + mE.name + " contains:");
                                                                        foreach (string eggstring in mE.actionpoints)
                                                                        {
                                                                            if (GUILayout.Button(eggstring + " at " + mE.name))
                                                                            {
                                                                                J.requirement_id = pair.Key;
                                                                                J.NCI_PQSCity = "";
                                                                                J.NCI_body = "";
                                                                                J.NCI_object = mE.part_idcode;
                                                                                J.NCI_egg = eggstring;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                GUILayout.EndVertical();
                                                            }
                                                            else
                                                            {
                                                                if (J.NCI_object != "")
                                                                {
                                                                    GUILayout.Label("Target Egg: " + J.NCI_egg + " at " + J.NCI_object);
                                                                    if (GUILayout.Button("change")) { J.NCI_egg = ""; }
                                                                }
                                                                else 
                                                                {
                                                                    GUILayout.Label("Target Egg: " + J.NCI_egg + " at " + J.NCI_PQSCity + " on " + J.NCI_body);
                                                                    if (GUILayout.Button("change")) { J.NCI_egg = ""; }
                                                                }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                        }
                                                        if (J.target == "object")
                                                        {
                                                            GUILayout.BeginHorizontal();
                                                            GUILayout.Label("Select Target Part:");
                                                            if (J.NCI_object == "")
                                                            {
                                                                if (mission.requirements.list.Count == 0)
                                                                {
                                                                    GUILayout.Label("Please create the Mission Assets first");
                                                                }
                                                                GUILayout.BeginVertical();
                                                                foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in mission.requirements.list)
                                                                {
                                                                    LCARS_NCI_Mission_Requirement mE = pair.Value;
                                                                    if (GUILayout.Button(mE.name))
                                                                    {
                                                                        if(mE.isCelestialBody)
                                                                        {
                                                                            J.NCI_body = mE.bodyname;
                                                                            J.NCI_object = "";
                                                                            J.NCI_PQSCity = "";
                                                                            J.NCI_egg = "";
                                                                        }
                                                                        else
                                                                        {
                                                                            J.NCI_body = "";
                                                                            J.NCI_PQSCity = "";
                                                                            J.NCI_egg = "";
                                                                            J.NCI_object = mE.part_idcode;
                                                                        }
                                                                    }
                                                                }
                                                                GUILayout.EndVertical();
                                                            }
                                                            else
                                                            {
                                                                if (J.NCI_body!="")
                                                                {
                                                                    GUILayout.Label("Target Object: " + J.NCI_body);
                                                                    if (GUILayout.Button("change")) { J.NCI_body = ""; }
                                                                }
                                                                else
                                                                {
                                                                    GUILayout.Label("Target Object: " + J.NCI_object);
                                                                    if (GUILayout.Button("change")) { J.NCI_object = ""; }
                                                                }
                                                            }
                                                            GUILayout.EndHorizontal();
                                                        }
                                                    }
                                                }



                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send eMail Message at JobStart:");
                                                if (J.jobStart_messageID_email == "" && J.jobStart_messageID_email != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobStart_messageID_email = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "eMail") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobStart_messageID_email = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobStart_messageID_email);
                                                    if (GUILayout.Button("change")) { J.jobStart_messageID_email = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send Console Message at JobStart:");
                                                if (J.jobStart_messageID_console == "" && J.jobStart_messageID_console != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobStart_messageID_console = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "console") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobStart_messageID_console = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobStart_messageID_console);
                                                    if (GUILayout.Button("change")) { J.jobStart_messageID_console = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send Screen Message at JobStart:");
                                                if (J.jobStart_messageID_screen == "" && J.jobStart_messageID_screen != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobStart_messageID_screen = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "screen") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobStart_messageID_screen = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobStart_messageID_screen);
                                                    if (GUILayout.Button("change")) { J.jobStart_messageID_screen = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send eMail Message at JobEnd:");
                                                if (J.jobEnd_messageID_email == "" && J.jobEnd_messageID_email != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobEnd_messageID_email = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "eMail") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobEnd_messageID_email = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobEnd_messageID_email);
                                                    if (GUILayout.Button("change")) { J.jobEnd_messageID_email = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send Console Message at JobEnd:");
                                                if (J.jobEnd_messageID_console == "" && J.jobEnd_messageID_console != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobEnd_messageID_console = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "console") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobEnd_messageID_console = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobEnd_messageID_console);
                                                    if (GUILayout.Button("change")) { J.jobEnd_messageID_console = ""; }
                                                }
                                                GUILayout.EndHorizontal();

                                                GUILayout.BeginHorizontal();
                                                GUILayout.Label("Send Screen Message at JobEnd:");
                                                if (J.jobEnd_messageID_screen == "" && J.jobEnd_messageID_screen != "none")
                                                {
                                                    GUILayout.BeginVertical();
                                                    if (Step.Messages.MessageList.Count == 0) { GUILayout.Label("No messages defined!"); }
                                                    if (GUILayout.Button("none")) { J.jobEnd_messageID_screen = "none"; }
                                                    foreach (LCARS_NCI_Mission_Message mM in Step.Messages.MessageList.Values)
                                                    {
                                                        if (mM.messageType != "screen") { continue; }
                                                        if (GUILayout.Button("Message " + mM.messageID)) { J.jobEnd_messageID_screen = mM.messageID.ToString(); }
                                                    }
                                                    GUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    GUILayout.Label(J.jobEnd_messageID_screen);
                                                    if (GUILayout.Button("change")) { J.jobEnd_messageID_screen = ""; }
                                                }
                                                GUILayout.EndHorizontal();


                                                GUILayout.BeginHorizontal();
                                                    J.isStepEnd = GUILayout.Toggle(J.isStepEnd, "Is this the end of this Step?");
                                                GUILayout.EndHorizontal();
                                                GUILayout.Label("******************************");
                                            }
                                        }
                                        else 
                                        {
                                            GUILayout.Label("No Jobs to list");
                                        }

#endregion
                            }
                        }


                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
#endregion

#endregion

                }




#region The Mission Save
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save Mission (does overwrite)"))
                {
                    //UnityEngine.Debug.Log("### NCIMC Save Mission clicked ");
                    /*
                    ConfigNode NC = new ConfigNode("NATURALCAUSEINC");
                    ConfigNode NCISTORY = new ConfigNode("NCISTORY");
                    NC.AddNode(NCISTORY);
                    ConfigNode cnTemp = ConfigNode.CreateConfigFromObject(mission, NC);
                    //UnityEngine.Debug.Log("### NCIMC cnTemp=" + cnTemp);
                    */
                    /*
                                    var dump = ObjectDumper.Dump(mission);
                                    //UnityEngine.Debug.Log("### NCIMC dump=" + dump);
                    */
                    SaveMission();
                    //UnityEngine.Debug.Log("### NCIMC SaveMission done ");

                }
                if (GUILayout.Button("Reset Editor"))
                {
                    reset_mission = true;
                }
                GUILayout.EndHorizontal();
                if (reset_mission)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("!!CONFIRM RESET!!"))
                    {
                        mission = null;
                        reset_mission = false;
                    }
                    if (GUILayout.Button("cancel"))
                    {
                        reset_mission = false;
                    }
                    GUILayout.EndHorizontal();
                }
#endregion

            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI done ");

#endregion
            }
#endregion
            //UnityEngine.Debug.Log("### NCIMC createMissionMainGUI done ");

        }


        void SaveMission()
        {
            mission.save();
        }

        


        LCARS_NCI_Mission LoadMission(string loadfileName)
        {
            return mission.load(loadfileName);
        }

        

        
        private void listMissionMainGUI()
        {
            string[] filePaths = Directory.GetFiles(@"" + KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions/", "*.mission_cfg");

            GUILayout.BeginVertical();
            foreach (string s in filePaths)
            {
                string fileName;
                fileName = s.Replace(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions/", "");
                fileName = fileName.Replace(".mission_cfg", "");
                if (GUILayout.Button(fileName))
                {
                    if(mission==null)
                    {
                        mission = new LCARS_NCI_Mission();
                    }
                    UnityEngine.Debug.Log("### NCIMC LoadMissionButton clicked fileName=" + fileName);
                    mission = LoadMission(fileName);
                    GUI_Mode = 2;
                }
            }
            GUILayout.EndVertical();

        }



    }


    /*
     // http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object
     */
    public class ObjectDumper
{
    private int _level;
    private readonly int _indentSize;
    private readonly StringBuilder _stringBuilder;
    private readonly List<int> _hashListOfFoundElements;

    private ObjectDumper(int indentSize)
    {
        _indentSize = indentSize;
        _stringBuilder = new StringBuilder();
        _hashListOfFoundElements = new List<int>();
    }

    public static string Dump(object element)
    {
        return Dump(element, 2);
    }

    public static string Dump(object element, int indentSize)
    {
        var instance = new ObjectDumper(indentSize);
        return instance.DumpElement(element);
    }

    private string DumpElement(object element)
    {
        if (element == null || element is ValueType || element is string)
        {
                Write(FormatValue(element));
        }
        else
        {
            var objectType = element.GetType();
            if (!typeof(IEnumerable).IsAssignableFrom(objectType))
            {
                Write("{0}", objectType.Name + "{");
                _hashListOfFoundElements.Add(element.GetHashCode());
                _level++;
            }

            var enumerableElement = element as IEnumerable;
            if (enumerableElement != null)
            {
                foreach (object item in enumerableElement)
                {
                    if (item is IEnumerable && !(item is string))
                    {
                        _level++;
                        DumpElement(item);
                        _level--;
                    }
                    else
                    {
                        if (!AlreadyTouched(item))
                            DumpElement(item);
                        else
                            Write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
                    }
                }
            }
            else
            {
                MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                foreach (var memberInfo in members)
                {
                    var fieldInfo = memberInfo as FieldInfo;
                    var propertyInfo = memberInfo as PropertyInfo;

                    if (fieldInfo == null && propertyInfo == null)
                        continue;

                    var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
                    object value = fieldInfo != null
                                       ? fieldInfo.GetValue(element)
                                       : propertyInfo.GetValue(element, null);

                    if (type.IsValueType || type == typeof(string))
                    {
                        Write("{0} = {1}", memberInfo.Name, FormatValue(value));
                    }
                    else
                    {
                        var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                        Write("{0}  {1}", memberInfo.Name, "{ ");

                        var alreadyTouched = !isEnumerable && AlreadyTouched(value);
                        _level++;
                        if (!alreadyTouched)
                            DumpElement(value);
                        else
                            Write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
                        _level--;
                        Write("{0}", "}");
                    }
                }
            }

            if (!typeof(IEnumerable).IsAssignableFrom(objectType))
            {
                Write("{0}", "}");
                _level--;
            }
        }

        return _stringBuilder.ToString();
    }

    private bool AlreadyTouched(object value)
    {
        if (value == null)
            return false;

        var hash = value.GetHashCode();
        for (var i = 0; i < _hashListOfFoundElements.Count; i++)
        {
            if (_hashListOfFoundElements[i] == hash)
                return true;
        }
        return false;
    }

    private void Write(string value, params object[] args)
    {
        var space = new string(' ', _level * _indentSize);

        if (args != null)
            value = string.Format(value, args);

        _stringBuilder.AppendLine(space + value);
    }

    private string FormatValue(object o)
    {
        if (o == null)
            return ("null");

        if (o is DateTime)
            return (((DateTime)o).ToShortDateString());

        if (o is string)
            return string.Format("{0}", o);

        if (o is char && (char)o == '\0') 
            return string.Empty; 

        if (o is ValueType)
            return (o.ToString());

        if (o is IEnumerable)
            return ("...");

        return ("{ }");
    }
}}
