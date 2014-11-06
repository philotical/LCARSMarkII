using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_Mission_Config_Writer
    {
        public ConfigNode construct_mission_config(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config");
            ConfigNode cn_mission = new ConfigNode("NCIMISSION");

            if (mission == null)
            {
                mission = new LCARS_NCI_Mission();
                mission.story_type = "";
                mission.title = "Enter here your Title";
                mission.filename = "validMissionFileName";
                mission.description = "";
                mission.skill_level = "Medium";
                mission.creator = "";
                mission.url = "";
                mission.missionSteps = 0;
            }
            
            cn_mission.AddValue("story_type", mission.story_type);
            cn_mission.AddValue("title", mission.title);
            cn_mission.AddValue("filename", mission.filename);
            cn_mission.AddValue("description", mission.description);
            cn_mission.AddValue("skill_level", mission.skill_level);
            cn_mission.AddValue("creator", mission.creator);
            cn_mission.AddValue("url", mission.url);
            cn_mission.AddValue("missionSteps", mission.steps.Count);

            ConfigNode cn_requirements = construct_mission_config_requirements(mission);
            ConfigNode cn_personalities = construct_mission_config_personalities(mission);
            ConfigNode cn_equipment = construct_mission_config_equipment(mission);
            ConfigNode cn_artefacts = construct_mission_config_artefacts(mission);
            //ConfigNode cn_sealDeal = construct_mission_config_sealDeal(mission);
            ConfigNode cn_steps = construct_mission_config_steps(mission);



            cn_mission.AddNode(cn_requirements);
            cn_mission.AddNode(cn_personalities);
            cn_mission.AddNode(cn_equipment);
            cn_mission.AddNode(cn_artefacts);
            //cn_mission.AddNode(cn_sealDeal);
            cn_mission.AddNode(cn_steps);

            return cn_mission;
        }
        public ConfigNode construct_mission_config_requirements(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_requirements");
            ConfigNode cn_requirements = new ConfigNode("REQUIREMENTS");
            ConfigNode cn_requirements_list = new ConfigNode("LIST");

            if (mission.requirements == null)
            {
                mission.requirements = new LCARS_NCI_Mission_Requirement_Manager();
            }
            if (mission.requirements.list == null)
            {
                mission.requirements.list = new Dictionary<int, LCARS_NCI_Mission_Requirement>();
            }
            foreach (LCARS_NCI_Mission_Requirement mR in mission.requirements.list.Values)
            {
                ConfigNode cn_requirements_requirement = new ConfigNode("REQUIREMENT");
                cn_requirements_requirement.AddValue("id", mR.id);
                cn_requirements_requirement.AddValue("name", mR.name);
                cn_requirements_requirement.AddValue("description", mR.description);
                cn_requirements_requirement.AddValue("part_idcode", mR.part_idcode);
                cn_requirements_requirement.AddValue("egg_idcode", mR.egg_idcode);
                string actionpoints = "";
                if (mR.actionpoints.Count > 0)
                {
                    actionpoints = mR.actionpoints.Aggregate((i, j) => i + "," + j);
                }
                cn_requirements_requirement.AddValue("actionpoints", actionpoints);
                string doors = "";
                if (mR.doors.Count > 0)
                {
                    doors = mR.doors.Aggregate((i, j) => i + "," + j);
                }
                cn_requirements_requirement.AddValue("doors", doors);
                string GUIWindows = "";
                if (mR.GUIWindows.Count > 0)
                {
                    GUIWindows = mR.GUIWindows.Aggregate((i, j) => i + "," + j);
                }
                cn_requirements_requirement.AddValue("GUIWindows", GUIWindows);

                cn_requirements_list.AddNode(cn_requirements_requirement);
            }

            cn_requirements.AddNode(cn_requirements_list);

            return cn_requirements;
        }
        public ConfigNode construct_mission_config_personalities(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_personalities");
            ConfigNode cn_personalities = new ConfigNode("PERSONALITIES");
            ConfigNode cn_personalities_list = new ConfigNode("LIST");

            if (mission.personalities == null)
            {
                mission.personalities = new LCARS_NCI_Mission_Personalities();
            }
            if (mission.personalities.list == null)
            {
                mission.personalities.list = new List<LCARS_NCI_Personality>();
            }
            foreach (LCARS_NCI_Personality p in mission.personalities.list)
            {
                ConfigNode cn_personalities_personality = new ConfigNode("PERSONALITY");
                cn_personalities_personality.AddValue("name", p.name);
                cn_personalities_personality.AddValue("idcode", p.idcode);
                cn_personalities_personality.AddValue("description", p.description);
                cn_personalities_personality.AddValue("portrait", p.portrait);

                cn_personalities_list.AddNode(cn_personalities_personality);
            }

            cn_personalities.AddNode(cn_personalities_list);

            return cn_personalities;
        }
        public ConfigNode construct_mission_config_equipment(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_equipment");
            ConfigNode cn_equipment = new ConfigNode("EQUIPMENTS");
            ConfigNode cn_equipment_list = new ConfigNode("LIST");

            if (mission.equippment == null)
            {
                UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_equipment mission.equippment=null");
                mission.equippment = new LCARS_NCI_Mission_Equippment_List();
            }
            if (mission.equippment.list == null)
            {
                UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_equipment mission.equippment.list=null");
                mission.equippment.list = new Dictionary<int, LCARS_NCI_Equippment>();
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_equipment mission.equippment.list.Count=" + mission.equippment.list.Count);

            foreach (LCARS_NCI_Equippment e in mission.equippment.list.Values)
            {
                UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_equipment e.idcode=" + e.idcode);
                ConfigNode cn_equipment_equipment = new ConfigNode("EQUIPMENT");
                cn_equipment_equipment.AddValue("name", e.name);
                cn_equipment_equipment.AddValue("idcode", e.idcode);
                cn_equipment_equipment.AddValue("description", e.description);
                cn_equipment_equipment.AddValue("icon", e.icon);

                cn_equipment_equipment.AddValue("resource", e.resource);
                cn_equipment_equipment.AddValue("distance_threshhold", e.distance_threshhold);
                cn_equipment_equipment.AddValue("part", e.part);
                cn_equipment_equipment.AddValue("egg", e.egg);
                cn_equipment_equipment.AddValue("artefact1", e.artefact1);
                cn_equipment_equipment.AddValue("textline", e.textline);
                cn_equipment_equipment.AddValue("artefact2", e.artefact2);

                cn_equipment_list.AddNode(cn_equipment_equipment);
            }

            cn_equipment.AddNode(cn_equipment_list);

            return cn_equipment;
        }
        public ConfigNode construct_mission_config_artefacts(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_artefacts");
            ConfigNode cn_artefacts = new ConfigNode("ARTEFACTS");
            ConfigNode cn_artefacts_list = new ConfigNode("LIST");

            if (mission.artefacts == null)
            {
                mission.artefacts = new LCARS_NCI_Mission_Artefacts();
            }
            if (mission.artefacts.list == null)
            {
                mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
            }
            foreach (LCARS_NCI_InventoryItem_Type e in mission.artefacts.list)
            {
                ConfigNode cn_artefacts_artefact = new ConfigNode("ARTEFACT");
                cn_artefacts_artefact.AddValue("name", e.name);
                cn_artefacts_artefact.AddValue("idcode", e.idcode);
                cn_artefacts_artefact.AddValue("description", e.description);
                cn_artefacts_artefact.AddValue("icon", e.icon);

                cn_artefacts_artefact.AddValue("isDamagable", e.isDamagable);
                cn_artefacts_artefact.AddValue("integrity", e.integrity);
                cn_artefacts_artefact.AddValue("usage_amount", e.usage_amount);
                cn_artefacts_artefact.AddValue("usage_times", e.usage_times);
                cn_artefacts_artefact.AddValue("powerconsumption", e.powerconsumption);

                cn_artefacts_list.AddNode(cn_artefacts_artefact);
            }

            cn_artefacts.AddNode(cn_artefacts_list);

            return cn_artefacts;
        }
        /*
        public ConfigNode construct_mission_config_sealDeal(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_sealDeal");
            ConfigNode cn_sealDeal = new ConfigNode("SEALDEAL");
            cn_sealDeal.AddValue("SealDeal_messageID", mission.sealDeal.SealDeal_messageID);
            cn_sealDeal.AddValue("CloseDeal_messageID", mission.sealDeal.CloseDeal_messageID);
            ConfigNode cn_sealDeal_Gain = construct_mission_config_sealDeal_Gain(mission);
            cn_sealDeal.AddNode(cn_sealDeal_Gain);

            return cn_sealDeal;
        }
        public ConfigNode construct_mission_config_sealDeal_Gain(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_sealDeal_Gain");
            if (mission.sealDeal == null)
            {
                mission.sealDeal = new LCARS_NCI_Mission_SealDeal();
                mission.sealDeal.SealDeal_messageID = "";
                mission.sealDeal.CloseDeal_messageID = "";
            }
            if (mission.sealDeal.Gain == null)
            {
                mission.sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
                mission.sealDeal.Gain.cash = "0";
                mission.sealDeal.Gain.reputation = "0";
                mission.sealDeal.Gain.science = "0";
                mission.sealDeal.Gain.objects = new List<string>();
            }

            ConfigNode cn_sealDeal_Gain = new ConfigNode("GAIN");
            cn_sealDeal_Gain.AddValue("cash", mission.sealDeal.Gain.cash);
            cn_sealDeal_Gain.AddValue("reputation", mission.sealDeal.Gain.reputation);
            cn_sealDeal_Gain.AddValue("science", mission.sealDeal.Gain.science);
            string objects = "";
            if (mission.sealDeal.Gain.objects.Count > 0)
            {
                objects = mission.sealDeal.Gain.objects.Aggregate((i, j) => i + "," + j);
            }
            cn_sealDeal_Gain.AddValue("objects", objects);

            return cn_sealDeal_Gain;
        }
        */

        public ConfigNode construct_mission_config_steps(LCARS_NCI_Mission mission)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps");
            ConfigNode cn_steps = new ConfigNode("STEPS");

            if (mission.steps == null)
            {
                mission.steps = new Dictionary<int,LCARS_NCI_Mission_Step>();
            }
            foreach (LCARS_NCI_Mission_Step e in mission.steps.Values)
            {
                ConfigNode cn_steps_step = new ConfigNode("STEP");
                cn_steps_step.AddValue("id", e.id);
                cn_steps_step.AddValue("locationtype", e.locationtype);
                cn_steps_step.AddValue("location_part", e.location_part);
                cn_steps_step.AddValue("requirementPart_id", e.requirementPart_id);
                cn_steps_step.AddValue("location_egg", e.location_egg);

                cn_steps_step.AddValue("conversation_trigger_distance", e.conversation_trigger_distance);
                cn_steps_step.AddValue("location_distance", e.location_distance);
                //cn_steps_step.AddValue("sealdeal_textID", e.sealdeal_textID);
                cn_steps_step.AddValue("next_step", e.next_step);
                cn_steps_step.AddValue("stepStart_messageID_email", e.stepStart_messageID_email);
                cn_steps_step.AddValue("stepStart_messageID_console", e.stepStart_messageID_console);
                cn_steps_step.AddValue("stepStart_messageID_screen", e.stepStart_messageID_screen);
                cn_steps_step.AddValue("stepEnd_messageID_email", e.stepEnd_messageID_email);
                cn_steps_step.AddValue("stepEnd_messageID_console", e.stepEnd_messageID_console);
                cn_steps_step.AddValue("stepEnd_messageID_screen", e.stepEnd_messageID_screen);
                cn_steps_step.AddValue("LCARS_Systems_NotificationText", e.LCARS_Systems_NotificationText);

                //ConfigNode cn_steps_step_GUIButton = construct_mission_config_steps_step_GUIButton(e);
                ConfigNode cn_steps_step_Conversation = construct_mission_config_steps_step_Conversation(e);
                ConfigNode cn_steps_step_Messages = construct_mission_config_steps_step_Messages(e);
                ConfigNode cn_steps_step_Jobs = construct_mission_config_steps_step_Jobs(e);
                //ConfigNode cn_steps_step_Remote_PartOptions = construct_mission_config_steps_step_Remote_PartOptions(e);
                ConfigNode cn_steps_step_LCARS_Systems_Options = construct_mission_config_steps_step_LCARS_Systems_Options(e);

                //cn_steps_step.AddNode(cn_steps_step_GUIButton);
                cn_steps_step.AddNode(cn_steps_step_Conversation);
                cn_steps_step.AddNode(cn_steps_step_Messages);
                cn_steps_step.AddNode(cn_steps_step_Jobs);
                //cn_steps_step.AddNode(cn_steps_step_Remote_PartOptions);
                cn_steps_step.AddNode(cn_steps_step_LCARS_Systems_Options);

                cn_steps.AddNode(cn_steps_step);
            }


            return cn_steps;
        }
        /*
        private ConfigNode construct_mission_config_steps_step_GUIButton(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_GUIButton");
            ConfigNode cn_Step_GUIButton = new ConfigNode("GUIBUTTON");
            try
            {
                if (Step.GUIButton == null)
                { return cn_Step_GUIButton; }
                if (Step.GUIButton.buttontext1 == "")
                { return cn_Step_GUIButton; }

                if (Step.GUIButton == null)
                {
                    Step.GUIButton = new LCARS_NCI_Mission_Step_GUIButton();
                    Step.GUIButton.buttontext1 = "";
                    Step.GUIButton.textID = 0;
                }
                cn_Step_GUIButton.AddValue("buttontext1", Step.GUIButton.buttontext1);
                cn_Step_GUIButton.AddValue("textID", Step.GUIButton.textID);
            }
            catch { return cn_Step_GUIButton; }

            return cn_Step_GUIButton;
        }
        */
        private ConfigNode construct_mission_config_steps_step_Conversation(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Conversation");
            ConfigNode cn_Conversation = new ConfigNode("CONVERSATION");
            if (Step.Conversation == null)
            {
                Step.Conversation = new LCARS_NCI_Mission_Conversation();
                Step.Conversation.personality = "";
            }
            if (Step.Conversation.SpeechList == null)
            {
                Step.Conversation.SpeechList = new Dictionary<int,LCARS_NCI_Mission_Conversation_Speech>();
            }

            cn_Conversation.AddValue("personality", Step.Conversation.personality);

            foreach (LCARS_NCI_Mission_Conversation_Speech Speech in Step.Conversation.SpeechList.Values)
            {
                ConfigNode cn_Conversation_speech = new ConfigNode("SPEECH");
                cn_Conversation_speech.AddValue("textID", Speech.textID);
                cn_Conversation_speech.AddValue("text", Speech.text);
                cn_Conversation_speech.AddValue("reward_player", Speech.reward_player);
                cn_Conversation_speech.AddValue("reward_science", Speech.reward_science);
                cn_Conversation_speech.AddValue("reward_cash", Speech.reward_cash);
                cn_Conversation_speech.AddValue("reward_reputation", Speech.reward_reputation);
                cn_Conversation_speech.AddValue("reward_artefact", Speech.reward_artefact);
                cn_Conversation_speech.AddValue("speechStart_messageID_email", Speech.speechStart_messageID_email);
                cn_Conversation_speech.AddValue("speechStart_messageID_console", Speech.speechStart_messageID_console);
                cn_Conversation_speech.AddValue("speechStart_messageID_screen", Speech.speechStart_messageID_screen);

                ConfigNode cn_steps_step_ResponsList = construct_mission_config_steps_step_Conversation_ResponsList(Speech);
                cn_Conversation_speech.AddNode(cn_steps_step_ResponsList);

                cn_Conversation.AddNode(cn_Conversation_speech);
            }

            return cn_Conversation;
        }
        private ConfigNode construct_mission_config_steps_step_Conversation_ResponsList(LCARS_NCI_Mission_Conversation_Speech Speech)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Conversation_ResponsList");
            ConfigNode cn_ResponsList = new ConfigNode("RESPONSELIST");
            if (Speech.ResponsList == null)
            {
                Speech.ResponsList = new List<LCARS_NCI_Mission_Speech_Respons>();
            }
            foreach (LCARS_NCI_Mission_Speech_Respons R in Speech.ResponsList)
            {
                ConfigNode cn_ResponsList_response = new ConfigNode("RESPONSE");
                cn_ResponsList_response.AddValue("responsText", R.responsText);
                cn_ResponsList_response.AddValue("responsEvent", R.responsEvent);
                cn_ResponsList_response.AddValue("responsTextID", R.responsTextID);
                cn_ResponsList_response.AddValue("responsStepID", R.responsStepID);
                cn_ResponsList_response.AddValue("responsArtefact", R.responsArtefact);
                cn_ResponsList_response.AddValue("responsCashAmount", R.responsCashAmount);
                //cn_ResponsList_response.AddValue("isStepEnd", R.isStepEnd);
                cn_ResponsList_response.AddValue("response_messageID", R.response_messageID);

                cn_ResponsList.AddNode(cn_ResponsList_response);
            }
            return cn_ResponsList;
        }

        private ConfigNode construct_mission_config_steps_step_Messages(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Messages");
            ConfigNode cn_Messages = new ConfigNode("MESSAGES");
            if (Step.Messages == null)
            {
                Step.Messages = new LCARS_NCI_Mission_Messages();
            }
            if (Step.Messages.MessageList == null)
            {
                Step.Messages.MessageList = new Dictionary<int,LCARS_NCI_Mission_Message>();
            }

            foreach(LCARS_NCI_Mission_Message M in Step.Messages.MessageList.Values)
            {
                ConfigNode cn_Messages_Message = new ConfigNode("MESSAGE");
                cn_Messages_Message.AddValue("messageID", M.messageID);
                cn_Messages_Message.AddValue("messageType", M.messageType);
                cn_Messages_Message.AddValue("loopMessage", M.loopMessage);
                cn_Messages_Message.AddValue("NCI_object", M.NCI_object);
                cn_Messages_Message.AddValue("NCI_egg", M.NCI_egg);
                cn_Messages_Message.AddValue("message", M.message);
                cn_Messages_Message.AddValue("title", M.title);
                cn_Messages_Message.AddValue("sender", M.sender);
                cn_Messages_Message.AddValue("sender_id_line", M.sender_id_line);
                cn_Messages_Message.AddValue("receiver_type", M.receiver_type);
                cn_Messages_Message.AddValue("receiver_code", M.receiver_code);
                cn_Messages_Message.AddValue("priority", M.priority);
                cn_Messages_Message.AddValue("Encrypted", M.Encrypted);
                cn_Messages_Message.AddValue("encryptionMode", M.encryptionMode);
                cn_Messages_Message.AddValue("decryption_artefact", M.decryption_artefact);
                //cn_Messages_Message.AddValue("reply_sent", M.reply_sent);

                ConfigNode cn_Messages_Message_ReplyList = construct_mission_config_steps_step_Messages_ReplyOptions(M);
                cn_Messages_Message.AddNode(cn_Messages_Message_ReplyList);

                cn_Messages.AddNode(cn_Messages_Message);
            }
            return cn_Messages;
        }
        private ConfigNode construct_mission_config_steps_step_Messages_ReplyOptions(LCARS_NCI_Mission_Message Message)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Messages_ReplyOptions");
            ConfigNode cn_ReplyList = new ConfigNode("REPLYOPTIONS");
            if (Message.reply_options == null)
            {
                Message.reply_options = new List<LCARS_NCI_Mission_Message_Reply>();
            }
            foreach (LCARS_NCI_Mission_Message_Reply R in Message.reply_options)
            {
                ConfigNode cn_ReplyList_reply = new ConfigNode("REPLY");
                cn_ReplyList_reply.AddValue("buttonText", R.buttonText);
                cn_ReplyList_reply.AddValue("replyCode", R.replyCode);
                cn_ReplyList_reply.AddValue("replyID", R.replyID);

                cn_ReplyList.AddNode(cn_ReplyList_reply);
            }
            return cn_ReplyList; 
        }

        private ConfigNode construct_mission_config_steps_step_Jobs(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Jobs");
            
            ConfigNode cn_jobs = new ConfigNode("JOBS");
            
            ConfigNode cn_jobList = new ConfigNode("JOBLIST");
            if (Step.Jobs == null)
            {
                Step.Jobs = new LCARS_NCI_Mission_Step_Jobs();
            }
            if (Step.Jobs.jobList == null)
            {
                Step.Jobs.jobList = new Dictionary<int,LCARS_NCI_Mission_Job>();
            }
            foreach (LCARS_NCI_Mission_Job M in Step.Jobs.jobList.Values)
            {
                ConfigNode cn_jobList_Job = new ConfigNode("JOB");
                cn_jobList_Job.AddValue("jobID", M.jobID);
                cn_jobList_Job.AddValue("isStepEnd", M.isStepEnd);

                cn_jobList_Job.AddValue("jobtype", M.jobtype);
                cn_jobList_Job.AddValue("target", M.target);
                cn_jobList_Job.AddValue("distance", M.distance);
                cn_jobList_Job.AddValue("NCI_object", M.NCI_object);
                cn_jobList_Job.AddValue("NCI_egg", M.NCI_egg);
                cn_jobList_Job.AddValue("NCI_artefact", M.NCI_artefact);
                cn_jobList_Job.AddValue("jobStart_messageID_email", M.jobStart_messageID_email);
                cn_jobList_Job.AddValue("jobStart_messageID_console", M.jobStart_messageID_console);
                cn_jobList_Job.AddValue("jobStart_messageID_screen", M.jobStart_messageID_screen);
                cn_jobList_Job.AddValue("jobEnd_messageID_email", M.jobEnd_messageID_email);
                cn_jobList_Job.AddValue("jobEnd_messageID_console", M.jobEnd_messageID_console);
                cn_jobList_Job.AddValue("jobEnd_messageID_screen", M.jobEnd_messageID_screen);


                cn_jobList.AddNode(cn_jobList_Job);
            }

            cn_jobs.AddNode(cn_jobList);

            return cn_jobs;

        }

        /*
        private ConfigNode construct_mission_config_steps_step_Remote_PartOptions(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_Remote_PartOptions");

            ConfigNode cn_PartOptions = new ConfigNode("PARTOPTIONS");
            if (Step.PartOptions == null)
            {
                Step.PartOptions = new Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type>();
            }
            foreach (KeyValuePair<string, LCARS_NCI_Step_Remote_PartOptions_Type> pair in Step.PartOptions)
            {
                ConfigNode cn_PartOption = new ConfigNode("OPTION");
                cn_PartOption.AddValue("key", pair.Key);
                cn_PartOption.AddValue("requirementID", pair.Value.requirementID);
                cn_PartOption.AddValue("part_idcode", pair.Value.part_idcode);
                cn_PartOption.AddValue("OptionID", pair.Value.OptionID);
                cn_PartOption.AddValue("isDisabled", pair.Value.isDisabled);

                cn_PartOptions.AddNode(cn_PartOption);
            }

            return cn_PartOptions;
        }
        */
        private ConfigNode construct_mission_config_steps_step_LCARS_Systems_Options(LCARS_NCI_Mission_Step Step)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Writer construct_mission_config_steps_step_LCARS_Systems_Options");

            ConfigNode cn_LCARSOPTIONS = new ConfigNode("LCARSOPTIONS");
            if (Step.LCARS_Systems_Options == null)
            {
                Step.LCARS_Systems_Options = new List<LCARS_ShipSystems_Options>();
            }
            foreach (LCARS_ShipSystems_Options o in Step.LCARS_Systems_Options)
            {
                ConfigNode cn_ODNITEM = new ConfigNode("ODNITEM");
                cn_ODNITEM.AddValue("system_name", o.system_name);
                cn_ODNITEM.AddValue("disable_this", o.disable_this);
                cn_ODNITEM.AddValue("damage_this", o.damage_this);

                cn_LCARSOPTIONS.AddNode(cn_ODNITEM);
            }

            return cn_LCARSOPTIONS;
        }
    }
}
