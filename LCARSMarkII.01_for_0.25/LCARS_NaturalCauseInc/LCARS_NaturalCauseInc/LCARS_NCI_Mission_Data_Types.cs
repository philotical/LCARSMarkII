using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{




    /*
     * *************************************************
     The Mission Structure
     * *************************************************
     */
    /// <summary>
    /// LCARS_NCI_Mission holds all data for one mission
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission
    {
        // controll
        public string story_type { set; get; } // background, userStarted

        // data
        public string title { set; get; }
        public string filename { set; get; }
        public string description { set; get; }
        public string skill_level { set; get; }
        public string creator { set; get; }
        public string url { set; get; }
        public int missionSteps { set; get; }

        public LCARS_NCI_Mission_Requirement_Manager requirements { set; get; }
        public List<Guid> requirements_confirmation_list = new List<Guid>();
        public void addRequirement(LCARS_NCI_Object NCI_Object)
        {
            if (requirements.list == null)
            {
                requirements.list = new Dictionary<int, LCARS_NCI_Mission_Requirement>();
            }
            LCARS_NCI_Mission_Requirement p = new LCARS_NCI_Mission_Requirement();
            //p.type = "part";
            p.id = requirements.list.Count + 1;
            p.name = NCI_Object.title;
            p.description = NCI_Object.description;
            p.actionpoints = NCI_Object.actionpoints;
            p.doors = NCI_Object.doors;
            p.GUIWindows = NCI_Object.GUIWindows;
            p.partmodulename = NCI_Object.partmodulename;
            p.part_idcode = NCI_Object.partname;
            requirements.list.Add(requirements.list.Count + 1, p);
        }

        public LCARS_NCI_Mission_Personalities personalities { set; get; }
        public void addPersonality(LCARS_NCI_Personality o)
        {
            if (personalities.list == null)
            {
                personalities.list = new List<LCARS_NCI_Personality>();
            }
            personalities.list.Add(o);
        }

        public LCARS_NCI_Mission_Equippment_List equippment { set; get; }
        public void addEquippment(LCARS_NCI_Equippment e)
        {
            if (equippment.list == null)
            {
                equippment.list = new Dictionary<int, LCARS_NCI_Equippment>();
            }
            equippment.list.Add(equippment.list.Count+1, e);
        }

        public LCARS_NCI_Mission_Artefacts artefacts { set; get; }
        public void addArtefact(LCARS_NCI_InventoryItem_Type o)
        {
            if (artefacts.list == null)
            {
                artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
            }
            artefacts.list.Add(o);
        }

        /*
        public LCARS_NCI_Mission_SealDeal sealDeal { set; get; }
        public void setGain(string type, int amount)
        {
            if (sealDeal.Gain == null)
            {
                sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
            }
            if (sealDeal.Gain.science == null)
            {
                sealDeal.Gain.science = "";
            }
            if (sealDeal.Gain.reputation == null)
            {
                sealDeal.Gain.reputation = "";
            }
            if (sealDeal.Gain.cash == null)
            {
                sealDeal.Gain.cash = "";
            }
            switch(type)
            {
                case "science":
                    sealDeal.Gain.science = amount.ToString();
                    break;
                case "reputation":
                    sealDeal.Gain.reputation = amount.ToString();
                    break;
                case "cash":
                    sealDeal.Gain.cash = amount.ToString();
                    break;
            }
        }
        public void addArtefactGain(LCARS_NCI_InventoryItem_Type o)
        {
            if (sealDeal.Gain == null)
            {
                sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
            }
            if (sealDeal.Gain.objects == null)
            {
                sealDeal.Gain.objects = new List<string>();
            }
            sealDeal.Gain.objects.Add(o.idcode);
        }
        */

        public Dictionary<int, LCARS_NCI_Mission_Step> steps { set; get; }
        public void addStep(LCARS_NCI_Mission_Step step)
        {
            steps.Add(step.id,step);
        }


        public LCARS_NCI_Mission load(string fName)
        {
            string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions";
            string loadfileName = fName + ".mission_cfg";
            string missionCFG_path = path + "/" + loadfileName;
            ConfigNode missionroot = ConfigNode.Load(missionCFG_path);

            LCARS_NCI_Mission_Config_Loader cfgLoader = new LCARS_NCI_Mission_Config_Loader();
            return cfgLoader.deconstruct_mission_config(this,missionroot);
        }
        public bool save()
        {
            LCARS_NCI_Mission_Config_Writer cfgWriter = new LCARS_NCI_Mission_Config_Writer();
            ConfigNode config = new ConfigNode("root");
            ConfigNode mission = cfgWriter.construct_mission_config(this);
            config.AddNode(mission);

            string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions";
            string loadfileName = filename + ".mission_cfg";
            string missionCFG_path = path + "/" + loadfileName;
            if (!config.Save(missionCFG_path))
            {
                return false;
            }
            return true;
        }


        public bool meetsRequirements()
        {
            UnityEngine.Debug.Log("LCARS_NCI_Mission  meetsRequirements begin ");
            if (LCARS_NCI.Instance.Data == null)
            {
                LCARS_NCI.Instance.init();
                LCARS_NCI.Instance.Data.init();
            }
            /*
            */
            int req_count = 0;
            foreach (LCARS_NCI_Mission_Requirement R in requirements.list.Values)
            {
                foreach (LCARS_NCI_Object O in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Values)
                {
                    UnityEngine.Debug.Log("LCARS_NCI_Mission  meetsRequirements O.partname=" + O.partname + " vs. R.part_idcode=" + R.part_idcode);
                    if (O.partname == R.part_idcode)
                    {
                        req_count++;
                    }
                }
            }
            UnityEngine.Debug.Log("LCARS_NCI_Mission  meetsRequirements end ");
            return (req_count >= requirements.list.Count) == true;
        }

        public void send_message(int messageID,int from_StepID = 0)
        {
            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message begin ");
            if (messageID == 0)
            { return; }

            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message 1 ");

            LCARS_NCI_Mission_Archive_RunningMission RunningMission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission;
            //LCARS_NCI_Mission mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission;

            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message 3 ");
            int currentStep = (from_StepID == 0) ? LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step : from_StepID;

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID] == null)
            {
                return;
            }

            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message 4 ");

            switch (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID].messageType)
            {

                case "eMail":
                    try
                    {
                        LCARSNCI_Bridge.Instance.LCARS_Message_SendBride(messageID, from_StepID);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("LCARS_NCI_Mission  send_message failed ex=" + ex);
                    }
                    break;

                case "console":
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID].loopMessage)
                    {
                        try
                        {
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.looplist begin ");
                            if (LCARS_NCI.Instance.Data.NCIConsole == null)
                            {
                                LCARS_NCI.Instance.Data.NCIConsole = new NCIConsole();
                            }
                            if (LCARS_NCI.Instance.Data.NCIConsole.looplist == null)
                            {
                                LCARS_NCI.Instance.Data.NCIConsole.looplist = new List<LoopingConsoleMessage>();
                            }
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.looplist 1  looplist.Count=" + LCARS_NCI.Instance.Data.NCIConsole.looplist.Count);
                            LoopingConsoleMessage tmp = new LoopingConsoleMessage();
                            tmp.messageID = messageID;
                            tmp.step = (from_StepID == 0) ? LCARS_NCI_Mission_Archive_Tools.get_current_stepID() : from_StepID;
                            tmp.job = LCARS_NCI_Mission_Archive_Tools.get_current_jobID();
                            tmp.contextTransform = null;
                            LCARS_NCI.Instance.Data.NCIConsole.looplist.Add(tmp);
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.looplist done looplist.Count=" + LCARS_NCI.Instance.Data.NCIConsole.looplist.Count);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("LCARS_NCI_Mission  NCIConsole.looplist.Add failed jobID=" + LCARS_NCI_Mission_Archive_Tools.get_current_jobID() + "  stepID=" + LCARS_NCI_Mission_Archive_Tools.get_current_stepID() + "  messageID=" + messageID + " failed ex=" + ex);
                        }
                    }
                    else
                    {
                        string text = "";
                        try
                        {
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.list begin ");
                            text = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID].title;
                            if (text.Contains("[DISTANCE]"))
                            {
                                int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object);
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission  send_message mR index=" + index);
                                if (index > 0)
                                {
                                    LCARS_NCI_Mission_Requirement mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];

                                    Transform context_objective = null;
                                    if (mR.assigned_sfs_object_Guid.ToString() != "00000000-0000-0000-0000-000000000000" && context_objective == null)
                                    {
                                        foreach (Vessel v in FlightGlobals.Vessels)
                                        {
                                            if (v.id == mR.assigned_sfs_object_Guid)
                                            {
                                                context_objective = v.transform;
                                            }
                                        }
                                    }

                                    context_objective = (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg == "") ? context_objective : context_objective.transform.Find(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);

                                    float distance = Vector3.Distance(LCARS_NCI.Instance.Data.vessel.CoM, context_objective.position);
                                    text.Replace("[DISTANCE]", distance + " Meter");
                                }
                                Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.list 1 list.Count=" + LCARS_NCI.Instance.Data.NCIConsole.list.Count +"text=" + text);

                            }
                            if (LCARS_NCI.Instance.Data.NCIConsole == null)
                            {
                                LCARS_NCI.Instance.Data.NCIConsole = new NCIConsole();
                            }
                            if (LCARS_NCI.Instance.Data.NCIConsole.list == null)
                            {
                                LCARS_NCI.Instance.Data.NCIConsole.list = new List<string>();
                            }
                            LCARS_NCI.Instance.Data.NCIConsole.list.Add(text);
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.list done  list.Count=" + LCARS_NCI.Instance.Data.NCIConsole.list.Count);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("LCARS_NCI_Mission  send_message NCIConsole.list.Add  failed text=" + text + " ex=" + ex);
                        }
                    }
                    break;

                case "screen":

                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID].loopMessage)
                    {
                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages==null)
                        {
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages = new List<LoopingScreenMessage>();
                        }
                        LoopingScreenMessage tmp = new LoopingScreenMessage();
                        tmp.messageID = messageID;
                        tmp.step = (from_StepID == 0) ? LCARS_NCI_Mission_Archive_Tools.get_current_stepID() : from_StepID;
                        tmp.job = LCARS_NCI_Mission_Archive_Tools.get_current_jobID();
                        tmp.contextTransform = null;
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages.Add(tmp);
                    }
                    else
                    {
                        string text = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID].title;
                        if (text.Contains("[DISTANCE]"))
                        {
                            int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object);
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission  send_message mR index=" + index);
                            if (index > 0)
                            {
                                LCARS_NCI_Mission_Requirement mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];
                                
                                Transform context_objective = null;
                                if (mR.assigned_sfs_object_Guid.ToString() != "00000000-0000-0000-0000-000000000000" && context_objective==null)
                                {
                                    foreach(Vessel v in FlightGlobals.Vessels)
                                    {
                                        if(v.id == mR.assigned_sfs_object_Guid)
                                        {
                                            context_objective = v.transform;
                                        }
                                    }
                                }

                                context_objective = (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg == "") ? context_objective  : context_objective.transform.Find(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);

                                float distance = Vector3.Distance(LCARS_NCI.Instance.Data.vessel.CoM, context_objective.position);
                                text.Replace("[DISTANCE]",distance+" Meter");
                            }

                        }
                        ScreenMessages.PostScreenMessage(
                               "<color=#ff9900ff>SingleScreenMessage: " + text + "</color>",
                          20f, ScreenMessageStyle.UPPER_CENTER
                        );
                    }
                    break;

            }

            //Message.sent = true;

            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message 11 ");



        }
        
    }
    public class LoopingScreenMessage
    {
        public int messageID { get; set; }
        public int step { get; set; }
        public int job { get; set; }
        public string context { get; set; }
        public Vector3 contextPosition { get; set; }
        public Transform contextTransform { get; set; }

        public Vessel vessel { get; set; }
    }
    public class NCIConsole
    {
        public List<LoopingConsoleMessage> looplist { get; set; }
        public List<string> list { get; set; }
    }
    public class LoopingConsoleMessage
    {
        public int messageID { get; set; }
        public int step { get; set; }
        public int job { get; set; }
        public string consoleID { get; set; }
        public string context { get; set; }
        public Vector3 contextPosition { get; set; }
        public Transform contextTransform { get; set; }

        public Vessel vessel { get; set; }
    }


    /// <summary>
    /// LCARS_NCI_Mission_Requirement holds all requirements for this mission
    /// parts : List of parts thi mission needs
    /// eggs : list of EasterEgg Points this mission needs
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission_Requirement_Manager
    {

        public Dictionary<int, LCARS_NCI_Mission_Requirement> list { set; get; }

        public int getIndex_By_PartIDcode(string idcode)
        {
            foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pair in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Requirement_Manager getIndex_By_PartIDcode if " + idcode + " == " + pair.Value.part_idcode + " return=" + pair.Key);
                if (idcode == pair.Value.part_idcode)
                { return pair.Key; }
            }
            return 0;
        }

        //public Dictionary<int, LCARS_NCI_Mission_Requirements_Part> parts { set; get; }
        //public List<LCARS_NCI_Egg> eggs { set; get; }

    }

    /// <summary>
    /// LCARS_NCI_Mission_Requirement - a type for the requirement list
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission_Requirement
    {
        public int id { set; get; }
        public string name { set; get; }
        //public string type { set; get; } // deprecated
        public string description { set; get; }
        public List<string> actionpoints { get; set; }
        public List<string> doors { get; set; }
        public List<string> GUIWindows { get; set; }
        public string partmodulename { set; get; }
        public string part_idcode { set; get; }
        public string egg_idcode { set; get; }
        
        public Guid assigned_sfs_object_Guid { set; get; }

    }


    /// <summary>
    /// LCARS_NCI_Mission_Personalities - the list of all virtual individual that participate in this mission
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission_Personalities
    {
        public List<LCARS_NCI_Personality> list { set; get; }

        public LCARS_NCI_Personality getPersonalityByName(string idcode)
        {
            foreach (LCARS_NCI_Personality P in list)
            {
                if (P.idcode == idcode)
                {
                    return P;
                }
            }
            return null;
        }

    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_Equippment_List
    {
        public Dictionary<int,LCARS_NCI_Equippment> list { set; get; }
    }

    /// <summary>
    /// LCARS_NCI_Mission_Artefacts - the list of all virtual artefacts that participate in this mission
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission_Artefacts
    {
        public List<LCARS_NCI_InventoryItem_Type> list { set; get; }

        public LCARS_NCI_InventoryItem_Type getArtefactByName(string idcode)
        {
            foreach (LCARS_NCI_InventoryItem_Type invItem in list)
            {
                if (invItem.idcode == idcode)
                {
                    return invItem;
                }
            }
            return null;
        }

    }

    /// <summary>
    /// LCARS_NCI_Mission_Step - the mission is organized in steps, those are stored here
    /// </summary>
    //[Serializable]
    public class LCARS_NCI_Mission_Step
    {
        public int id { set; get; }
        public string locationtype { set; get; } //distanceLower, distanceGreater
        public int requirementPart_id { set; get; }
        //public Dictionary<string,LCARS_NCI_Step_Remote_PartOptions_Type> PartOptions { set; get; }
        public string location_part { set; get; }
        public string location_egg { set; get; }
        public string conversation_trigger_distance { set; get; }
        public string location_distance { set; get; }
        //public int sealdeal_textID { set; get; }
        public string next_step { set; get; }
        //public LCARS_NCI_Mission_Step_GUIButton GUIButton { set; get; }
        public LCARS_NCI_Mission_Conversation Conversation { set; get; }
        public LCARS_NCI_Mission_Messages Messages { set; get; }
        public LCARS_NCI_Mission_Step_Jobs Jobs { set; get; }
        public string stepStart_messageID_email { set; get; } // send mesage at job start
        public string stepStart_messageID_console { set; get; } // send mesage at job start
        public string stepStart_messageID_screen { set; get; } // send mesage at job start
        public string stepEnd_messageID_email { set; get; } // send mesage at job end
        public string stepEnd_messageID_console { set; get; } // send mesage at job end
        public string stepEnd_messageID_screen { set; get; } // send mesage at job end

        public List<LCARS_ShipSystems_Options> LCARS_Systems_Options { set; get; }
        public string LCARS_Systems_NotificationText { set; get; }

            
        public bool checkCondition()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition begin ");
            float locationdistance = LCARS_NCI_Mission_Archive_Tools.ToFloat(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_distance);
            Vector3 messure_point = Vector3.zero;

            LCARS_NCI_Mission_Requirement mR = null;
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype == "distanceLower" ||
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype == "distanceGreater")
            {

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " find mR ");
                int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part);
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " mR index=" + index);
                if (index > 0)
                {
                    mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " mR mR.id=" + mR.id);
                }
                else
                {
                    mR = null;
                }

                if (mR == null)
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " mR==null skipping");
                    return false;
                }
            }

            
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                /*
                if (!v.loaded)
                { continue; }

                if (!v.mainBody != FlightGlobals.ActiveVessel.mainBody)
                { continue; }
                */

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition locationdistance=" + locationdistance + " loop start for v.id=" + v.id);
                try
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition locationdistance=" + locationdistance + " assigned_sfs_object_Guid=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id].assigned_sfs_object_Guid + " v.id=" + v.id);
                }
                catch { }

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 1 v.loaded=" + v.loaded + " for v.id=" + v.id);
                if (!v.loaded || v.id == FlightGlobals.ActiveVessel.id)
                {
                    continue;
                }


                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id].assigned_sfs_object_Guid.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition fix missing sfs_object_Guid ");
                    string part_idcode = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id].part_idcode;
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition fix missing sfs_object_Guid part_idcode=" + part_idcode);
                    foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition fix missing sfs_object_Guid loop item pair.Value.partname=" + pair.Value.partname);
                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id].assigned_sfs_object_Guid.ToString() != "00000000-0000-0000-0000-000000000000")
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition allready fixed missing sfs_object_Guid skipping loop item pair.Value.partname=" + pair.Value.partname);
                            continue;
                        }
                        if (pair.Value.partname == part_idcode)
                        {
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id].assigned_sfs_object_Guid = pair.Key;
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition fixing missing sfs_object_Guid with loop item pair.Value.partname=" + pair.Value.partname + " pair.Key=" + pair.Key);
                            continue;
                        }
                    }
                }

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 2 ");

                try
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition checking: location_egg=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg + " and location_part=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part + " at v.rootPart.name=" + v.rootPart.name);
                }
                catch 
                {
                    //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition checking: no rootpart, skipping ");
                    //continue;
                }

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 3 ");

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition checking: location_egg=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg + " and location_part=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part + " at v.rootPart.name=" + v.rootPart.name);


                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 4 ");

                if (v.rootPart.name.Contains(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part) && mR.assigned_sfs_object_Guid == v.id)
                {

                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 5 ");

                    Transform tmp = null;
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg != "")
                    {
                        tmp = v.transform.Find(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg);
                    }
                    if (tmp != null)
                    {
                        messure_point = tmp.position;
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition was found: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg + " at " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part + " " + mR.assigned_sfs_object_Guid);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition was found: location_part=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part + " " + mR.assigned_sfs_object_Guid);
                        messure_point = v.transform.position;
                    }

                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 6 ");

                }
            }

            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 7 ");
            if (messure_point == Vector3.zero)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " " + locationdistance + " messure_point == Vector3.zero return false ");
                return false;
            }

            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition loop 8 ");

            if (
                    (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype == "distanceLower" && LCARS_NCI_Mission_Archive_Tools.isDistanceLower(messure_point, locationdistance)) ||
                    (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype == "distanceGreater" && LCARS_NCI_Mission_Archive_Tools.isDistanceGreater(messure_point, locationdistance))
                )
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " " + locationdistance + " return true ");
                return true;
            }

            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.locationtype + " " + locationdistance + " return false ");
            return false;
        }

    }

    /*
    //[Serializable]
    public class LCARS_NCI_Step_Remote_PartOptions_Type
    {
        public int requirementID { set; get; }
        public string partmodulename { set; get; }
        public string part_idcode { set; get; }
        public string OptionID { set; get; }
        public bool isDisabled { set; get; }
    }
    */

    //[Serializable]
    public class LCARS_ShipSystems_Options
    {
        public string system_name { set; get; }
        public bool disable_this { set; get; }
        public bool damage_this { set; get; }
    }
    /*
    //[Serializable]
    public class LCARS_NCI_Mission_Step_GUIButton
    {
        public string buttontext1 { set; get; }
        public int textID { set; get; }
    }
    */

    //[Serializable]
    public class LCARS_NCI_Mission_Messages
    {
        public Dictionary<int, LCARS_NCI_Mission_Message> MessageList { set; get; }

        public void addMessage(LCARS_NCI_Mission_Message m)
        {
            MessageList.Add(m.messageID, m);
        }
    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_Message
    {
        public int messageID { set; get; }
        public bool sent { get; set; } // only for console and screen

        public string messageType { set; get; } // eMail,console,screen
        public bool loopMessage { get; set; } // only for console and screen
        //public string loopCondition { get; set; } // end of job, end of step,
        //public int loopCondition_id { get; set; } // jobID or stepID
        //public string DISTANCE_type { get; set; } // missionvessel, missionkerbal, Object/Egg, body, KSC
        public string NCI_object { get; set; } //requirement ID, body name,
        public string NCI_egg { get; set; } // egg name


        public string message { set; get; }
        public string title { set; get; }

        public string sender { get; set; }
        public string sender_id_line { get; set; } // like subject

        public string receiver_type { get { return "Station"; } }
        public string receiver_code { get { return "Bridge"; } }

        public int priority { get; set; }
        //public string reply_code { get; set; }

        public bool Encrypted { get; set; }
        public string encryptionMode { get; set; } // normal, defect, unbreakable, specialItem
        public string decryption_artefact { get; set; } // special Item

        //public bool reply_sent { get; set; }
        public List<LCARS_NCI_Mission_Message_Reply> reply_options { get; set; }
        public void addReply(LCARS_NCI_Mission_Message_Reply r)
        {
            reply_options.Add(r);
        }

    }

    //[Serializable]
    public class LCARS_NCI_Mission_Message_Reply
    {
        public string buttonText { set; get; }
        public string replyCode { set; get; } // goto: none, message, step, job
        public string replyID { set; get; }
        //public string stepID { set; get; }
        //public string jobID { set; get; }
    }

    //[Serializable]
    public class LCARS_NCI_Mission_Conversation
    {
        public string personality { set; get; }
        public Dictionary<int, LCARS_NCI_Mission_Conversation_Speech> SpeechList { set; get; }

        public void addSpeech(LCARS_NCI_Mission_Conversation_Speech s)
        {
            SpeechList.Add(s.textID,s);
        }
    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_Conversation_Speech
    {
        public int textID  { set; get; }
        public string text { set; get; }
        public bool reward_player { set; get; }
        public string reward_science { set; get; } // science, cash, reputation, artefact
        public string reward_cash { set; get; } // science, cash, reputation, artefact
        public string reward_reputation { set; get; } // science, cash, reputation, artefact
        public string reward_artefact { set; get; } // science, cash, reputation, artefact
        public List<LCARS_NCI_Mission_Speech_Respons> ResponsList { set; get; }
        public string speechStart_messageID_email { set; get; } // send mesage at speech start
        public string speechStart_messageID_console { set; get; } // send mesage at speech start
        public string speechStart_messageID_screen { set; get; } // send mesage at speech start

        public void addRespons(LCARS_NCI_Mission_Speech_Respons r)
        {
            ResponsList.Add(r);
        }
    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_Speech_Respons
    {
        public string responsText  { set; get; } 
        public string responsEvent { set; get; }
        public string responsTextID { set; get; }
        public string responsStepID { set; get; }
        public string responsArtefact { set; get; }
        public string responsCashAmount { set; get; }
        //public bool isStepEnd { set; get; }
        public string response_messageID { set; get; } // send mesage at response
    }

    //[Serializable]
    public class LCARS_NCI_Mission_Step_Jobs
    {
        public Dictionary<int,LCARS_NCI_Mission_Job> jobList { set; get; }
        public void addJob(LCARS_NCI_Mission_Job j)
        {
            jobList.Add(j.jobID,j);
        }
        public LCARS_NCI_Mission_Job getJobByID(int jobID)
        {
            foreach (LCARS_NCI_Mission_Job j in jobList.Values)
            {
                if (j.jobID == jobID)
                {
                    return j;
                }
            }
            return null;
        }
    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_Job
    {
        public int jobID { set; get; }
        public bool isStepEnd { set; get; }

        public string jobtype { set; get; } // distanceLower, distanceGreater, destroy, scan, collect
        public string target { set; get; } // object (distance, destroy, scan), egg (distance, scan), artefact (collect)
        public string distance { set; get; } // how close is required meter
        public string NCI_object { set; get; } // what object idcode
        public string NCI_egg { set; get; } // what artefact idcode
        public string NCI_artefact { set; get; } // what artefact idcode
        public string jobStart_messageID_email { set; get; } // send mesage at job start
        public string jobStart_messageID_console { set; get; } // send mesage at job start
        public string jobStart_messageID_screen { set; get; } // send mesage at job start
        public string jobEnd_messageID_email { set; get; } // send mesage at job end
        public string jobEnd_messageID_console { set; get; } // send mesage at job end
        public string jobEnd_messageID_screen { set; get; } // send mesage at job end

    }

    /*
    //[Serializable]
    public class LCARS_NCI_Mission_SealDeal
    {
        public LCARS_NCI_Mission_SealDeal_Gain Gain { set; get; }
        public string SealDeal_messageID { set; get; } // send mesage at SealDeal
        public string CloseDeal_messageID { set; get; } // send mesage at SealDeal
    }

    //[Serializable]
    public class LCARS_NCI_Mission_SealDeal_Gain
    {
        public string cash { set; get; }
        public string reputation { set; get; }
        public string science { set; get; }
        public List<string> objects { set; get; }
    }
    
    //[Serializable]
    public class LCARS_NCI_Mission_CloseDeal
    {
            public string payout_cash  { set; get; } 
            public string payout_reputation { set; get; }
            public string payout_science { set; get; }
            public string payout_objects { set; get; }
    }
    */
    /*
     * *************************************************
     The Mission Structure
     * *************************************************
     */








}
