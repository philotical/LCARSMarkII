using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_Mission_Config_Loader
    {


        LCARS_NCI_Mission mission;
        public LCARS_NCI_Mission deconstruct_mission_config(LCARS_NCI_Mission EmptyMission, ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config");
            ConfigNode NCIMISSION = config.GetNode("NCIMISSION");

            mission = EmptyMission;

            // data
            try { 
                mission.story_type = NCIMISSION.GetValue("story_type");
                mission.title = NCIMISSION.GetValue("title");
                mission.filename = NCIMISSION.GetValue("filename");
                mission.description = NCIMISSION.GetValue("description");
                mission.skill_level = NCIMISSION.GetValue("skill_level");
                mission.creator = NCIMISSION.GetValue("creator");
                mission.url = NCIMISSION.GetValue("url");
            }catch { }

            // assets
            try { deconstruct_mission_config_requirements(NCIMISSION); }catch { }
            try { deconstruct_mission_config_personalities(NCIMISSION); }catch { }
            try { deconstruct_mission_config_equipment(NCIMISSION); }catch { }
            try { deconstruct_mission_config_artefacts(NCIMISSION); }catch { }
            //try { deconstruct_mission_config_sealDeal(NCIMISSION); }catch { }
            
            // story
            try { deconstruct_mission_config_steps(NCIMISSION); }catch { }

            return mission;
        }

        public void deconstruct_mission_config_requirements(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_requirements");
            ConfigNode REQUIREMENTS = config.GetNode("REQUIREMENTS");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            mission.requirements = new LCARS_NCI_Mission_Requirement_Manager();
            mission.requirements.list = new Dictionary<int,LCARS_NCI_Mission_Requirement>();
            foreach (ConfigNode Item in LIST.GetNodes("REQUIREMENT"))
            {
                LCARS_NCI_Mission_Requirement NCIObj = new LCARS_NCI_Mission_Requirement();

                NCIObj.id = LCARS_NCI_Mission_Archive_Tools.ToInt(Item.GetValue("id"));
                NCIObj.name = Item.GetValue("name");
                NCIObj.description = Item.GetValue("description"); 
                NCIObj.part_idcode = Item.GetValue("part_idcode");

                NCIObj.actionpoints = new List<string>();
                foreach (string s in Item.GetValue("actionpoints").Split(new Char[] { ',' }))
                {
                    NCIObj.actionpoints.Add(s);
                }
                NCIObj.doors = new List<string>();
                foreach (string s in Item.GetValue("doors").Split(new Char[] { ',' }))
                {
                    NCIObj.doors.Add(s);
                }
                NCIObj.GUIWindows = new List<string>();
                foreach (string s in Item.GetValue("GUIWindows").Split(new Char[] { ',' }))
                {
                    NCIObj.GUIWindows.Add(s);
                }
                
                mission.requirements.list.Add(NCIObj.id,NCIObj);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader mission.requirements.list.Count=" + mission.requirements.list.Count);
        }
        public void deconstruct_mission_config_personalities(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_personalities");
            ConfigNode REQUIREMENTS = config.GetNode("PERSONALITIES");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            mission.personalities = new LCARS_NCI_Mission_Personalities();
            mission.personalities.list = new List<LCARS_NCI_Personality>();
            foreach (ConfigNode Item in LIST.GetNodes("PERSONALITY"))
            {
                LCARS_NCI_Personality NCIPers = new LCARS_NCI_Personality();

                NCIPers.name = Item.GetValue("name"); 
                NCIPers.description = Item.GetValue("description");
                NCIPers.idcode = Item.GetValue("idcode");
                NCIPers.portrait = Item.GetValue("portrait");

                mission.personalities.list.Add(NCIPers);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader mission.personalities.list.Count=" + mission.personalities.list.Count);
        }
        public void deconstruct_mission_config_equipment(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_equipment");
            ConfigNode REQUIREMENTS = config.GetNode("EQUIPMENTS");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            mission.equippment = new LCARS_NCI_Mission_Equippment_List();
            mission.equippment.list = new Dictionary<int, LCARS_NCI_Equippment>();
            foreach (ConfigNode Item in LIST.GetNodes("EQUIPMENT"))
            {
                LCARS_NCI_Equippment NCIEqu = new LCARS_NCI_Equippment();

                NCIEqu.name = Item.GetValue("name"); ;
                NCIEqu.description = Item.GetValue("description");
                NCIEqu.idcode = Item.GetValue("idcode");
                NCIEqu.icon = Item.GetValue("icon");


                NCIEqu.resource = Item.GetValue("resource");
                NCIEqu.distance_threshhold = Item.GetValue("distance_threshhold");
                NCIEqu.part = Item.GetValue("part");
                NCIEqu.egg = Item.GetValue("egg");
                NCIEqu.artefact1 = Item.GetValue("artefact1");
                NCIEqu.textline = Item.GetValue("textline");
                NCIEqu.artefact2 = Item.GetValue("artefact2");

                mission.equippment.list.Add(mission.equippment.list.Count + 1, NCIEqu);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader mission.equippment.list.Count=" + mission.equippment.list.Count);
        }
        public void deconstruct_mission_config_artefacts(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_artefacts");
            ConfigNode REQUIREMENTS = config.GetNode("ARTEFACTS");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            mission.artefacts = new LCARS_NCI_Mission_Artefacts();
            mission.artefacts.list = new List<LCARS_NCI_InventoryItem_Type>();
            foreach (ConfigNode Item in LIST.GetNodes("ARTEFACT"))
            {
                LCARS_NCI_InventoryItem_Type NCIArt = new LCARS_NCI_InventoryItem_Type();

                NCIArt.name = Item.GetValue("name");
                NCIArt.description = Item.GetValue("description");
                NCIArt.idcode = Item.GetValue("idcode");
                NCIArt.icon = Item.GetValue("icon");
                NCIArt.isDamagable = Item.GetValue("isDamagable");
                NCIArt.integrity = Item.GetValue("integrity");
                NCIArt.usage_amount = Item.GetValue("usage_amount");
                NCIArt.usage_times = Item.GetValue("usage_times");
                NCIArt.powerconsumption = Item.GetValue("powerconsumption");

                mission.artefacts.list.Add(NCIArt);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader mission.artefacts.list.Count=" + mission.artefacts.list.Count);
        }
        /*
        public void deconstruct_mission_config_sealDeal(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_sealDeal");
            ConfigNode SEALDEAL = config.GetNode("SEALDEAL");

            mission.sealDeal = new LCARS_NCI_Mission_SealDeal();
            mission.sealDeal.SealDeal_messageID = SEALDEAL.GetValue("name");
            mission.sealDeal.CloseDeal_messageID = SEALDEAL.GetValue("name");

            deconstruct_mission_config_sealDeal_Gain(SEALDEAL);
        }
        public void deconstruct_mission_config_sealDeal_Gain(ConfigNode SEALDEAL)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_sealDeal_Gain");
            ConfigNode GAIN = SEALDEAL.GetNode("GAIN");

            mission.sealDeal.Gain = new LCARS_NCI_Mission_SealDeal_Gain();
            mission.sealDeal.Gain.cash = GAIN.GetValue("cash");
            mission.sealDeal.Gain.reputation = GAIN.GetValue("reputation");
            mission.sealDeal.Gain.science = GAIN.GetValue("science");
            
            mission.sealDeal.Gain.objects = new List<string>();
            foreach (string s in GAIN.GetValue("objects").Split(new Char[] { ',' }))
            {
                mission.sealDeal.Gain.objects.Add(s);
            }
        }
        */

        public void deconstruct_mission_config_steps(ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps");
            ConfigNode STEPS = config.GetNode("STEPS");

            mission.steps = new Dictionary<int, LCARS_NCI_Mission_Step>();
            foreach (ConfigNode Step in STEPS.GetNodes("STEP"))
            {
                LCARS_NCI_Mission_Step NCIStep = new LCARS_NCI_Mission_Step();

                NCIStep.id = LCARS_NCI_Mission_Archive_Tools.ToInt(Step.GetValue("id"));
                NCIStep.locationtype = Step.GetValue("locationtype");
                NCIStep.location_part = Step.GetValue("location_part");
                NCIStep.requirementPart_id = LCARS_NCI_Mission_Archive_Tools.ToInt(Step.GetValue("requirementPart_id"));
                NCIStep.location_egg = Step.GetValue("location_egg");
                NCIStep.conversation_trigger_distance = Step.GetValue("conversation_trigger_distance");
                NCIStep.location_distance = Step.GetValue("location_distance");
                //NCIStep.sealdeal_textID = LCARS_NCI_Mission_Archive_Tools.ToInt(Step.GetValue("sealdeal_textID"));
                NCIStep.next_step = Step.GetValue("next_step");
                NCIStep.stepStart_messageID_email = Step.GetValue("stepStart_messageID_email");
                NCIStep.stepStart_messageID_console = Step.GetValue("stepStart_messageID_console");
                NCIStep.stepStart_messageID_screen = Step.GetValue("stepStart_messageID_screen");
                NCIStep.stepEnd_messageID_email = Step.GetValue("stepEnd_messageID_email");
                NCIStep.stepEnd_messageID_console = Step.GetValue("stepEnd_messageID_console");
                NCIStep.stepEnd_messageID_screen = Step.GetValue("stepEnd_messageID_screen");
                NCIStep.LCARS_Systems_NotificationText = Step.GetValue("LCARS_Systems_NotificationText");


                //try { NCIStep.GUIButton = deconstruct_mission_config_steps_step_GUIButton(Step); }catch { }
                try { NCIStep.Conversation = deconstruct_mission_config_steps_step_Conversation(Step); }catch { }
                try { NCIStep.Messages = deconstruct_mission_config_steps_step_Messages(Step); }catch { }
                try { NCIStep.Jobs = deconstruct_mission_config_steps_step_Jobs(Step); }catch { }
                //try { NCIStep.PartOptions = deconstruct_mission_config_steps_step_PartOptions(Step); }catch { }
                try { NCIStep.LCARS_Systems_Options = deconstruct_mission_config_steps_step_LCARS_Systems_Options(Step); }catch { }


                mission.steps.Add(NCIStep.id, NCIStep);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader mission.steps.Count=" + mission.steps.Count);
        }
        /*
        private LCARS_NCI_Mission_Step_GUIButton deconstruct_mission_config_steps_step_GUIButton(ConfigNode Step_Config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_GUIButton");
            ConfigNode GUIBUTTON = Step_Config.GetNode("GUIBUTTON");
            LCARS_NCI_Mission_Step_GUIButton GUIButton = null;
            if (GUIBUTTON.GetValue("buttontext1") != null && GUIBUTTON.GetValue("buttontext1") != "")
            {
                GUIButton = new LCARS_NCI_Mission_Step_GUIButton();
                GUIButton.buttontext1 = GUIBUTTON.GetValue("buttontext1");
                GUIButton.textID = LCARS_NCI_Mission_Archive_Tools.ToInt(GUIBUTTON.GetValue("textID"));
            }

            return GUIButton;
        }
        */
        private LCARS_NCI_Mission_Conversation deconstruct_mission_config_steps_step_Conversation(ConfigNode Step_Config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_Conversation");
            ConfigNode CONVERSATION = Step_Config.GetNode("CONVERSATION");

            LCARS_NCI_Mission_Conversation Conversation = new LCARS_NCI_Mission_Conversation();
            Conversation.personality = CONVERSATION.GetValue("personality");
            Conversation.SpeechList = new Dictionary<int, LCARS_NCI_Mission_Conversation_Speech>();

            foreach (ConfigNode Speech_node in CONVERSATION.GetNodes("SPEECH"))
            {

                LCARS_NCI_Mission_Conversation_Speech Speech = new LCARS_NCI_Mission_Conversation_Speech();
                Speech.textID = Conversation.SpeechList.Count + 1;
                Speech.text = Speech_node.GetValue("text");
                Speech.reward_player = (Speech_node.GetValue("reward_player") == "True") ? true : false;
                Speech.reward_artefact = Speech_node.GetValue("reward_artefact");
                Speech.reward_cash = Speech_node.GetValue("reward_cash");
                Speech.reward_reputation = Speech_node.GetValue("reward_reputation");
                Speech.reward_science = Speech_node.GetValue("reward_science");
                Speech.speechStart_messageID_email = Speech_node.GetValue("speechStart_messageID_email");
                Speech.speechStart_messageID_console = Speech_node.GetValue("speechStart_messageID_console");
                Speech.speechStart_messageID_screen = Speech_node.GetValue("speechStart_messageID_screen");

                Speech.ResponsList = deconstruct_mission_config_steps_step_Conversation_ResponsList(Speech_node);
                
                Conversation.SpeechList.Add(Speech.textID, Speech);
            }
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader Conversation.SpeechList.Count=" + Conversation.SpeechList.Count);
            return Conversation;
        }
        private List<LCARS_NCI_Mission_Speech_Respons> deconstruct_mission_config_steps_step_Conversation_ResponsList(ConfigNode Speech_node)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_Conversation_ResponsList");
            ConfigNode RESPONSELIST = Speech_node.GetNode("RESPONSELIST");

            List<LCARS_NCI_Mission_Speech_Respons> ResponsList = new List<LCARS_NCI_Mission_Speech_Respons>();
            foreach (ConfigNode response_node in RESPONSELIST.GetNodes("RESPONSE"))
            {
                LCARS_NCI_Mission_Speech_Respons nR = new LCARS_NCI_Mission_Speech_Respons();
                nR.responsText = response_node.GetValue("responsText");
                nR.responsEvent = response_node.GetValue("responsEvent");
                nR.responsTextID = response_node.GetValue("responsTextID");
                nR.responsStepID = response_node.GetValue("responsStepID");
                nR.responsCashAmount = response_node.GetValue("responsCashAmount");
                nR.responsArtefact = response_node.GetValue("responsArtefact");
                //nR.isStepEnd = ( response_node.GetValue("isStepEnd")== "True" ) ? true : false ;
                ResponsList.Add(nR);
            }
            return ResponsList;
        }
        private LCARS_NCI_Mission_Messages deconstruct_mission_config_steps_step_Messages(ConfigNode Step_Config)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_Messages");
            ConfigNode MESSAGES = Step_Config.GetNode("MESSAGES");

            LCARS_NCI_Mission_Messages Messages = new LCARS_NCI_Mission_Messages();
            Messages.MessageList = new Dictionary<int, LCARS_NCI_Mission_Message>();

            foreach (ConfigNode Message_node in MESSAGES.GetNodes("MESSAGE"))
            {
                LCARS_NCI_Mission_Message Message = new LCARS_NCI_Mission_Message();
                Message.messageID = LCARS_NCI_Mission_Archive_Tools.ToInt(Message_node.GetValue("messageID"));
                Message.message = Message_node.GetValue("message");
                Message.title = Message_node.GetValue("title");
                Message.sender = Message_node.GetValue("sender");
                Message.sender_id_line = Message_node.GetValue("sender_id_line");
                Message.priority = LCARS_NCI_Mission_Archive_Tools.ToInt(Message_node.GetValue("priority"));
                Message.Encrypted = ( Message_node.GetValue("Encrypted") == "True" ) ? true : false ;
                Message.encryptionMode = Message_node.GetValue("encryptionMode"); 
                Message.decryption_artefact = Message_node.GetValue("decryption_artefact"); 
                //Message.reply_sent = ( Message_node.GetValue("reply_sent") == "True" ) ? true : false ;
                Message.messageType = Message_node.GetValue("messageType"); 
                Message.loopMessage = ( Message_node.GetValue("loopMessage") == "True" ) ? true : false ;
                Message.NCI_object = Message_node.GetValue("NCI_object");
                Message.NCI_egg = Message_node.GetValue("NCI_egg"); 
                
                Message.reply_options = deconstruct_mission_config_steps_step_Messages_ReplyOptions(Message_node);

                Messages.MessageList.Add(Message.messageID, Message);
            }
            return Messages;
        }
        private List<LCARS_NCI_Mission_Message_Reply> deconstruct_mission_config_steps_step_Messages_ReplyOptions(ConfigNode Message_node)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_Messages_ReplyOptions");
            ConfigNode REPLYOPTIONS = Message_node.GetNode("REPLYOPTIONS");

            List<LCARS_NCI_Mission_Message_Reply> reply_options = new List<LCARS_NCI_Mission_Message_Reply>();
            foreach (ConfigNode response_node in REPLYOPTIONS.GetNodes("REPLY"))
            {
                LCARS_NCI_Mission_Message_Reply reply = new LCARS_NCI_Mission_Message_Reply();
                reply.replyCode = response_node.GetValue("replyCode");
                reply.buttonText = response_node.GetValue("buttonText");
                reply.replyID = response_node.GetValue("replyID"); 
                reply_options.Add(reply);
            }
            return reply_options;
        }
        private LCARS_NCI_Mission_Step_Jobs deconstruct_mission_config_steps_step_Jobs(ConfigNode Step_Config)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_Jobs");
            ConfigNode JOBS = Step_Config.GetNode("JOBS");
            ConfigNode JOBLIST = JOBS.GetNode("JOBLIST");

            LCARS_NCI_Mission_Step_Jobs Jobs = new LCARS_NCI_Mission_Step_Jobs();
            Jobs.jobList = new Dictionary<int,LCARS_NCI_Mission_Job>();

            foreach (ConfigNode Job_node in JOBLIST.GetNodes("JOB"))
            {

                LCARS_NCI_Mission_Job Job = new LCARS_NCI_Mission_Job();
                Job.jobID = LCARS_NCI_Mission_Archive_Tools.ToInt(Job_node.GetValue("jobID"));
                Job.isStepEnd = (Job_node.GetValue("isStepEnd") == "True") ? true : false;
                Job.jobtype = Job_node.GetValue("jobtype");
                Job.jobStart_messageID_email = Job_node.GetValue("jobStart_messageID_email");
                Job.jobStart_messageID_console = Job_node.GetValue("jobStart_messageID_console");
                Job.jobStart_messageID_screen = Job_node.GetValue("jobStart_messageID_screen");
                Job.jobEnd_messageID_email = Job_node.GetValue("jobEnd_messageID_email");
                Job.jobEnd_messageID_console = Job_node.GetValue("jobEnd_messageID_console");
                Job.jobEnd_messageID_screen = Job_node.GetValue("jobEnd_messageID_screen");
                Job.target = Job_node.GetValue("target");
                Job.distance = Job_node.GetValue("distance");
                Job.NCI_object = Job_node.GetValue("NCI_object");
                Job.NCI_egg = Job_node.GetValue("NCI_egg");
                Job.NCI_artefact = Job_node.GetValue("NCI_artefact");
                Jobs.jobList.Add(Job.jobID,Job);

            }
            return Jobs;
        }
        /*
        private Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type> deconstruct_mission_config_steps_step_PartOptions(ConfigNode Step_Config)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_PartOptions");
            ConfigNode PARTOPTIONS = Step_Config.GetNode("PARTOPTIONS");

            Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type> PartOptions = new Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type>();
            foreach (ConfigNode OPTION in PARTOPTIONS.GetNodes("OPTION"))
            {
                LCARS_NCI_Step_Remote_PartOptions_Type option = new LCARS_NCI_Step_Remote_PartOptions_Type();
                option.requirementID = LCARS_NCI_Mission_Archive_Tools.ToInt(OPTION.GetValue("requirementID"));
                option.part_idcode = OPTION.GetValue("part_idcode");
                option.OptionID = OPTION.GetValue("OptionID");
                option.isDisabled = (OPTION.GetValue("isDisabled") == "True") ? true : false;
                PartOptions.Add(OPTION.GetValue("key"), option);
            }
            return PartOptions;
        }
        */
        private List<LCARS_ShipSystems_Options> deconstruct_mission_config_steps_step_LCARS_Systems_Options(ConfigNode Step_Config)
        {
            //UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Mission_Config_Loader deconstruct_mission_config_steps_step_LCARS_Systems_Options");
            ConfigNode LCARSOPTIONS = Step_Config.GetNode("LCARSOPTIONS");

            List<LCARS_ShipSystems_Options> LCARS_Systems_Options = new List<LCARS_ShipSystems_Options>();
            foreach (ConfigNode OPTION in LCARSOPTIONS.GetNodes("ODNITEM"))
            {
                LCARS_ShipSystems_Options option = new LCARS_ShipSystems_Options();
                option.system_name = OPTION.GetValue("system_name");
                option.disable_this = (OPTION.GetValue("disable_this") == "True") ? true : false;
                option.damage_this = (OPTION.GetValue("damage_this") == "True") ? true : false;
                LCARS_Systems_Options.Add(option);
            }
            return LCARS_Systems_Options;
        }
    }
}
