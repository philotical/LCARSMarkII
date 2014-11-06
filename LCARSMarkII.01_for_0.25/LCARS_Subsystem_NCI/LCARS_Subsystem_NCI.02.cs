﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_NCI : ILCARSPlugin
    {
        public string subsystemName { get { return "NCI Connection"; } }
        public string subsystemDescription { get { return "Uplink with the NCI system"; } }
        public string subsystemStation { get { return "Bridge"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 0f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start



        public string[] selStrings;
        public int selGridInt = 0;



        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  SetVessel done ");
        }
        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {
            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI customCallback key=" + key + " value1=" + value1 + " value2=" + value2 + " value3=" + value3 + " value4=" + value4 + " value5=" + value5);
            /*
            switch (key)
            {
                    //"SendMessageReply", qT.Message_Object.id.ToString(), R.replyCode, R.replyID
                case"SendMessageReply":
                    UnityEngine.Debug.Log("### LCARS_Subsystem_NCI SendMessageReply   value1=" + value1 + " value2=" + value2 + " value3=" + value3);
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.processMessageReply(value1,value2,value3);
                    break;
            }
            */
        }
        private float originalLoadDistance = 2500f;
        private float originalUnLoadDistance = 2250f;
        public void SetLoadDistance(float loadDistance = 0f, float unloadDistance = 0f)
        {
            loadDistance = (loadDistance == 0f) ? originalLoadDistance : loadDistance;
            unloadDistance = (unloadDistance == 0f) ? originalUnLoadDistance : unloadDistance;
            Vessel.loadDistance = loadDistance;
            Vessel.unloadDistance = unloadDistance;
        }
        string Inventory_Artefact_showDetails = "";
        GUIStyle scrollview_style1;
        Vector2 ScrollPosition1;
        private float LoopingConsoleMessage_lastUpdate = 0.0f;
        private float LoopingConsoleMessage_Interval = 0.5f;
        private List<string> looplist_compiled;
        public void getGUI()
        {

            if (scrollview_style1 == null)
            {
                scrollview_style1 = new GUIStyle();
                scrollview_style1.fixedHeight = 255;
            }


            UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI begin ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("LCARS_Subsystem_NCI: ");
            GUILayout.EndHorizontal();

            selStrings = new string[] { "Mission", "Console", "Log", "Universe", "Inventory", "Options" };
            GUILayout.BeginHorizontal();
            selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 3);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(445));
            ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

                switch (selGridInt)
                {
                    case 0: //Mission
                        if (LCARS_NCI.Instance.Data.vessel != thisVessel)
                        {
                            GUILayout.Label("This is NOT your mission vessel!!");
                            if (GUILayout.Button("Set this Vessel as your Mission Vessel"))
                            {
                                LCARS_NCI.Instance.Data.vessel = thisVessel;
                            }
                        }
                        else
                        {
                            GUILayout.Label("This is your mission vessel.");
                        }
                        //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 2 ");

                        if (LCARS_NCI.Instance.Data.kerbal == "" || LCARS_NCI.Instance.Data.kerbal == null)
                        {
                            GUILayout.Label("There is no mission Kerbal defined");
                        }
                        else
                        {
                            GUILayout.Label(LCARS_NCI.Instance.Data.kerbal+" is your mission Kerbal");
                        }
                        bool MissionToDisplay = false;
                        try
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                            {
                                GUILayout.Label("RunningMission title: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
                                GUILayout.Label("Description: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.description);
                                MissionToDisplay = true;
                            }
                        }
                        catch  { MissionToDisplay = false; GUILayout.Label("Currently, there is no running Mission.");} // + ex); }
                        if (MissionToDisplay)
                        {
                            try
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                                {
                                    //GUILayout.Label("RunningMission missionStep.id: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id);
                                    GUILayout.Label("RunningMission current_step: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep());
                                }
                            }
                            catch { GUILayout.Label("Currently, there is no running Step."); } // + ex); }
                            try
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                                {
                                    //GUILayout.Label("RunningMission missionJob.jobID: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID);
                                    GUILayout.Label("RunningMission current_job: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob());
                                }
                            }
                            catch { GUILayout.Label("Currently, there is no running Job."); } // + ex); }
                            try
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                                {
                                    GUILayout.Label("Task: " +
                                        LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Jobs(LCARS_NCI.Instance.Data.MissionArchive.RunningMission)
                                        /*
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " than " +
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance + " to " +
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object
                                        */
                                        );
                                }
                            }
                            catch { GUILayout.Label("Currently, there is no Task."); } // + ex); }
                            try
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep != null)
                                {
                                    GUILayout.Label("RunningMission check Step Condition: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.checkCondition());
                                }
                            }
                            catch { GUILayout.Label("Currently, there is no Step Condition."); } // + ex); }
                        }
                        break;
                    case 1://Console
                        GUILayout.Label("Console: ToDo: ");
                        if (LCARS_NCI.Instance.Data.NCIConsole == null)
                        {
                            LCARS_NCI.Instance.Data.NCIConsole = new NCIConsole();
                        }
                        if (LCARS_NCI.Instance.Data.NCIConsole.list == null)
                        {
                            LCARS_NCI.Instance.Data.NCIConsole.list = new List<string>();
                        }
                        if (LCARS_NCI.Instance.Data.NCIConsole.looplist == null)
                        {
                            LCARS_NCI.Instance.Data.NCIConsole.looplist = new List<LoopingConsoleMessage>();
                        }
                        try
                        {
                            if ((Time.time - LoopingConsoleMessage_lastUpdate) > LoopingConsoleMessage_Interval)
                            {
                                looplist_compiled = new List<string>();
                                UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist LoopingConsoleMessage_lastUpdate - it's time ");
                                LoopingConsoleMessage_lastUpdate = Time.time;

                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID > 0)
                                {
                                    foreach (LoopingConsoleMessage L in LCARS_NCI.Instance.Data.NCIConsole.looplist)
                                    {
                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist loop running L.messageID=" + L.messageID);
                                        if (L.step < LCARS_NCI_Mission_Archive_Tools.get_current_stepID() || L.job < LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID)
                                        {
                                            LCARS_NCI.Instance.Data.NCIConsole.looplist.Remove(L);
                                            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist message removed ");
                                        }
                                        else
                                        {
                                            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist attempt to find L.messageID=" + L.messageID + " L.step=" + L.step + " L.job=" + L.job);
                                            string text = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[L.step].Messages.MessageList[L.messageID].title;
                                            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist text=" + text);
                                            if (text.Contains("[DISTANCE]"))
                                            {
                                                UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist text contains [DISTANCE]  text=" + text);
                                                int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object);
                                                float distance = 0f;
                                                UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist  mR index=" + index);
                                                if (index > 0)
                                                {
                                                    L.contextPosition = Vector3.zero;
                                                    UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist L.contextPosition=null");
                                                    LCARS_NCI_Mission_Requirement mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];
                                                    UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist mR.name=" + mR.name);

                                                    if (mR.assigned_sfs_object_Guid.ToString() != "00000000-0000-0000-0000-000000000000" && L.contextPosition == Vector3.zero)
                                                    {
                                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist mR.assigned_sfs_object_Guid=" + mR.assigned_sfs_object_Guid);
                                                        foreach (Vessel v in FlightGlobals.Vessels)
                                                        {
                                                            if (v.id == mR.assigned_sfs_object_Guid)
                                                            {
                                                                UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist mR.assigned_sfs_object_Guid found v.name=" + v.name);
                                                                L.contextTransform = v.transform;
                                                                L.contextPosition = v.CoM;
                                                                L.vessel = v;
                                                            }
                                                        }
                                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist set L.contextPosition again ");
                                                        L.contextPosition = (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg == "") ? L.contextPosition : L.vessel.rootPart.FindModelTransform(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg).position;
                                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist mR.assigned_sfs_object_Guid found missionJob.NCI_egg=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);
                                                    }
                                                    if (L.contextPosition != Vector3.zero)
                                                    {
                                                        distance = Vector3.Distance(FlightGlobals.ActiveVessel.CoM, L.contextPosition);
                                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist mR.assigned_sfs_object_Guid found distance=" + distance);
                                                        text = text.Replace("[DISTANCE]", distance + " Meter");
                                                    }


                                                }
                                            }
                                            looplist_compiled.Add(text);
                                        }
                                        UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist loop end L.messageID=" + L.messageID);
                                    }
                                    UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist done ");
                                }
                            }
                            foreach (string s in looplist_compiled)
                            {
                                GUILayout.Label(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.looplist failed ex=" + ex);
                        }
                        try
                        {
                            foreach (string s in LCARS_NCI.Instance.Data.NCIConsole.list)
                            {
                                GUILayout.Label(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI NCIConsole.list failed ex=" + ex);
                        }

                        break;
                    case 2://Log
                        GUILayout.Label("Log: ToDo: ");
                        break;
                    case 3://Universe
                        


                        try
                        {
                            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList != null)
                            {
                                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Count + " Objects in the sfs_ActiveObjectsList");
                            }
                        }
                        catch  { }
                        UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 5 ");

                        try
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions != null)
                            {
                                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions.Count + " Objects in the playable_backgroundMissions");
                            }
                        }
                        catch { }
                        //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 6 ");

                        try
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions != null)
                            {
                                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions.Count + " Objects in the playable_UserMissions");
                            }
                        }catch{}
                        //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 7 ");
                        if (LCARS_NCI.Instance.Data.Naturals_List != null)
                        {
                            GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Naturals_List.Count + " Objects in the Naturals_List");
                        }
                        //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 8 ");

                        if (LCARS_NCI.Instance.Data.Mines_List != null)
                        {
                            GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Mines_List.Count + " Objects in the Mines_List");
                        }
                        //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 9 ");

                        if (LCARS_NCI.Instance.Data.Stations_List != null)
                        {
                            GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Stations_List.Count + " Objects in the Stations_List");
                        }
                        if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList!=null)
                        {
                            foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                            {
                                try
                                {
                                    GUILayout.Label("Object in List: " + pair.Value.partname + " " + pair.Key);
                                }
                                catch (Exception ex) { GUILayout.Label("2)" + ex); }
                                try
                                {
                                    if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.First.Value == null)
                                    {
                                        GUILayout.Label("Next Mission to come: No Missions for this Object");
                                    }
                                    else
                                    {
                                        GUILayout.Label("Next Mission to come: " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.First.Value);
                                        GUILayout.Label("Last Mission played: " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.Last.Value);
                                        GUILayout.Label(" ");
                                    }
                                }
                                catch /*(Exception ex)*/ {  GUILayout.Label("Next Mission to come: No Missions for this Object"); /* GUILayout.Label("3)" + ex); */ }
                            }
                        }

                        break;
                    case 4://Inventory
                        GUILayout.Label("Inventory: ToDo: ");
                        Texture2D icon_tex = null;
                        bool use_default_icon = false;
                        string Artefact_default_icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_noPic";
                        if (Inventory_Artefact_showDetails=="")
                        {
                            GUILayout.BeginHorizontal();
                            int count = 1;
                            foreach(KeyValuePair<string,LCARS_ArtefactInventory_Type> pair in LCARS.lODN.ArtefactInventory)
                            {
                                LCARS_ArtefactInventory_Type A = pair.Value;

                                try
                                {
                                    icon_tex = GameDatabase.Instance.GetTexture(A.icon, false);
                                }
                                catch (Exception ex)
                                {
                                    UnityEngine.Debug.Log("### NCI Gather_Object_Lists skipping icon_tex A.icon=" + A.icon + " - file not found ex=" + ex);
                                    use_default_icon = true;
                                }
                                if (A.icon == "" || icon_tex==null)
                                {
                                    use_default_icon = true;
                                }
                                if (use_default_icon)
                                {
                                    icon_tex = GameDatabase.Instance.GetTexture(Artefact_default_icon, false);
                                }
                                GUILayout.BeginVertical();
                                if(GUILayout.Button(icon_tex, GUILayout.Height(50), GUILayout.Width(50)))
                                {
                                    Inventory_Artefact_showDetails=A.idcode;
                                }
                                GUILayout.Label(A.idcode, GUILayout.Width(50));
                                GUILayout.EndVertical();


                                if(count == 5)
                                {
                                    GUILayout.EndHorizontal();
                                    GUILayout.BeginHorizontal();
                                    count = 0;
                                }
                                count++;
                            }
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            LCARS_ArtefactInventory_Type I = LCARS.lODN.ArtefactInventory[Inventory_Artefact_showDetails];
                            GUILayout.BeginHorizontal();

                                try
                                {
                                    icon_tex = GameDatabase.Instance.GetTexture(I.icon, false);
                                }
                                catch (Exception ex)
                                {
                                    UnityEngine.Debug.Log("### NCI Gather_Object_Lists skipping icon_tex I.icon=" + I.icon + " - file not found ex=" + ex);
                                    use_default_icon = true;
                                }
                                if (I.icon == "" || icon_tex==null)
                                {
                                    use_default_icon = true;
                                }
                                if (use_default_icon)
                                {
                                    icon_tex = GameDatabase.Instance.GetTexture(Artefact_default_icon, false);
                                }
                            GUILayout.BeginVertical();
                                GUILayout.Button(icon_tex, GUILayout.Height(200), GUILayout.Width(200));
                            GUILayout.EndVertical();

                            GUILayout.BeginVertical();
                            if (GUILayout.Button("Back"))
                            {
                                Inventory_Artefact_showDetails = "";
                            }

                            GUILayout.Label("Name: " + I.name);
                            GUILayout.Label("Description: " + I.description);
                            GUILayout.Label("isDamagable: " + I.isDamagable);
                            GUILayout.Label("integrity: " + I.integrity);
                            GUILayout.Label("powerconsumption: " + I.powerconsumption);
                            GUILayout.Label("usage_amount (-1 = unl.): " + I.usage_amount);
                            GUILayout.Label("usage_times: " + I.usage_times);
                                

                            GUILayout.EndVertical();

                            GUILayout.EndHorizontal();


                        }
                        break;
                    case 5://Options
                        GUILayout.Label("LoadDistance: " + Vessel.unloadDistance);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Reset"))
                        {
                            SetLoadDistance();
                        }
                        if (GUILayout.Button("20k"))
                        {
                            SetLoadDistance(20250f, 20000f);
                        }
                        if (GUILayout.Button("50k"))
                        {
                            SetLoadDistance(50250f, 50000f);
                        }

                        if (GUILayout.Button("90k"))
                        {
                            SetLoadDistance(90250f, 90000f);
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Label("Mission Options: ");
                        try
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission != null)
                                {
                                    GUILayout.Label("Title: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
                                    if (GUILayout.Button("End Mission"))
                                    {
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission = null;
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("mission == null ");
                                }
                            }
                            else
                            {
                                GUILayout.Label("RunningMission == null ");
                            }
                        }
                        catch
                        {
                            GUILayout.Label("catch RunningMission section ");
                        }
                        try
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                            {
                                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission != null)
                                {
                                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype == "awaitingUserInput")
                                    {
                                        if (GUILayout.Button("Force Next Job"))
                                        {
                                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Job();
                                        }
                                    }
                                    else
                                    {
                                        GUILayout.Label("jobtype != awaitingUserInput ");
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("mission == null ");
                                }
                            }
                            else
                            {
                                GUILayout.Label("RunningMission == null ");
                            }
                        }
                        catch
                        {
                            GUILayout.Label("catch RunningJob section ");
                        }

                        break;
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();






            
            // fix the "on first load no missions bug" - might be due to scenario, I don't know, this should fix it
                bool UpdateUniverseOnce = false;
                bool UpdateUniverseOnce_done = false;
                if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList != null && !UpdateUniverseOnce_done)
                {
                    UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI UpdateUniverseOnce check start ");
                    foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                    {
                        try
                        {
                            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.First.Value == null)
                            {
                                UpdateUniverseOnce = true;
                            }
                            else
                            {
                                UpdateUniverseOnce = false;
                            }
                        }
                        catch 
                        {
                            UpdateUniverseOnce = true;
                        }
                    }
                    UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI UpdateUniverseOnce check end UpdateUniverseOnce=" + UpdateUniverseOnce);
                }
                if (UpdateUniverseOnce && !UpdateUniverseOnce_done)
                {
                    UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI UpdateUniverseOnce ");
                    LCARS_NCI.Instance.Data.sfs_ActiveObjectsList_IsUpdated = false;
                    LCARS_NCI.Instance.Data.init();
                    UpdateUniverseOnce = false;
                    UpdateUniverseOnce_done = true;
                    UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI UpdateUniverseOnce Done ");
                }


            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 3 ");



            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 4 ");


            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI 4b ");


            //UnityEngine.Debug.Log("LCARS_Subsystem_NCI  getGUI done ");
        }

    }

}
