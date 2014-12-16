using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
 /*
  [KSPScenario(
        ScenarioCreationOptions.AddToNewCareerGames |
        ScenarioCreationOptions.AddToExistingCareerGames |
        ScenarioCreationOptions.AddToNewScienceSandboxGames |
        ScenarioCreationOptions.AddToExistingScienceSandboxGames,
        ScenarioCreationOptions.AddToAllGames,
        GameScenes.FLIGHT  , GameScenes.SPACECENTER, GameScenes.TRACKSTATION  , GameScenes.EDITOR, GameScenes.SPH etc )]
 */

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT)]
    public class LCARS_NCIMISSION_Scenario : ScenarioModule
    {

/*
        */

        //[KSPField(isPersistant = true)]
        //internal RUNNINGMISSION ;

        public LCARS_NCIMISSION_Scenario controller
        {
            get
            {


                Game g = HighLogic.CurrentGame;
                if (g == null) return null;
                foreach (ProtoScenarioModule mod in g.scenarios)
                {
                    if (mod.moduleName == typeof(LCARS_NCIMISSION_Scenario).Name)
                    {
                        return (LCARS_NCIMISSION_Scenario)mod.moduleRef;
                    }
                }
                return (LCARS_NCIMISSION_Scenario)g.AddProtoScenarioModule(typeof(LCARS_NCIMISSION_Scenario), GameScenes.FLIGHT).moduleRef;
            }
            private set { }
        }


        /*

        */
        [KSPField(isPersistant = true)]
        internal string ArtefactInventory = "";
        [KSPField(isPersistant = true)]
        internal string CommunicationQueue = "";
        /*
        */
        internal ConfigNode NCIMISSION_Node = null;

        [KSPField(isPersistant = true)]
        internal int loadcount = 0;

        public override void OnLoad(ConfigNode node)
        {

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  Begin ");

            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList == null)
            {
                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList = new Dictionary<Guid, LCARS_NCI_Object>();
            }

            LCARS_NCI.Instance.Data.sfs_ActiveObjectsList_IsUpdated = false;
            LCARS_NCI.Instance.Data.init();

            NCIMISSION_Node = node;
            LCARSNCI_Bridge.Instance.NCIMISSION_Node_ScenarioLoad = NCIMISSION_Node;


            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  1 ");

            if (NCIMISSION_Node.HasValue("loadcount"))
            {
                loadcount = Int16.Parse(NCIMISSION_Node.GetValue("loadcount"));
                loadcount++;
            }

            UnityEngine.Debug.Log("### LCARS_NCIMISSION_Scenario reinstateArtefactInventory begin ");
            if (NCIMISSION_Node.HasValue("ArtefactInventory"))
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  1 ");
                ArtefactInventory = NCIMISSION_Node.GetValue("ArtefactInventory");
                LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup = ArtefactInventory;
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  string copied ArtefactInventory_String_SFS_Backup=" + LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup);
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  done ");
                /*
                     if (LCARSNCI_Bridge.Instance.shipLCARS == null)
                     {
                         LCARSNCI_Bridge.Instance.getCurrentShipLCARS();
                     }
                     if (LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory == null)
                     {
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad attempt to initObjects on shipLCARS ");
                         LCARSNCI_Bridge.Instance.shipLCARS.initObjects();
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad attempt to initObjects on shipLCARS done ");
                         //LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory = new Dictionary<string, LCARS_ArtefactInventory_Type>();
                     }

                     Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  2 ");

                     ArtefactInventory = "";
                     ArtefactInventory = NCIMISSION_Node.GetValue("ArtefactInventory");
                     LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup = ArtefactInventory;

                     Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  3 ArtefactInventory=" + ArtefactInventory);

                     string[] tmp1 = ArtefactInventory.Split(new Char[] { '|' });
                     foreach (string tmp2 in tmp1)
                     {
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  4 tmp2=" + tmp2);
                         if (tmp2 == "" || tmp2 == null) { continue; }
                         string[] tmp3 = tmp2.Split(new Char[] { ',' });
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  5 ");

                         LCARS_NCI_InventoryItem_Type tmp = LCARSNCI_Bridge.Instance.getMissionInventory(tmp3[0]);

                         LCARS_ArtefactInventory_Type tmp4 = new LCARS_ArtefactInventory_Type();
                         tmp4.name = tmp.name;
                         tmp.idcode = tmp.idcode;
                         tmp4.description = tmp.description;
                         tmp4.icon = tmp.icon;
                         tmp.integrity = tmp.integrity;
                         tmp4.isDamagable = tmp.isDamagable;
                         tmp4.powerconsumption = tmp.powerconsumption;
                         tmp.usage_amount = tmp.usage_amount;
                         tmp4.usage_times = "0";
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile 6 ");
                         try
                         {
                             LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory.Add(tmp4.idcode, tmp4);
                         }
                         catch { return; }
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile 7 ");

                         LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory[tmp4.idcode].usage_amount = tmp3[1];
                         LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory[tmp4.idcode].usage_times = tmp3[2];
                         LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory[tmp4.idcode].integrity = tmp3[3];
                         Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile 8 ");
                     }
                     Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile done ");
                 try
                 {
                 }
                 catch (Exception ex) { UnityEngine.Debug.Log("### LCARS_NCIMISSION_Scenario reinstateArtefactInventory failed ex=" + ex); }
                 */
            }
            else 
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setShipInventoryFromPersistentFile  node is missing ");
            
            }
            if (NCIMISSION_Node.HasValue("CommunicationQueue"))
            {
                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile begin ");
                string LCARS_Queue_data = "";
                LCARS_Queue_data = NCIMISSION_Node.GetValue("CommunicationQueue");
                LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup = LCARS_Queue_data;
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  string copied CommunicationQueue_String_SFS_Backup=" + LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup);
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  done ");
                /*
                //try
                //{
                    //LCARSNCI_Bridge.Instance.reinstateCommunicationQueue();
                    if (LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue == null)
                    {
                        LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();
                    }

                    UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 1 ");

                    string LCARS_Queue_data = "";
                    LCARS_Queue_data = NCIMISSION_Node.GetValue("CommunicationQueue");
                    LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup = LCARS_Queue_data;


                    string[] tmp1 = LCARS_Queue_data.Split(new Char[] { '|' });
                    foreach (string tmp2 in tmp1)
                    {
                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 2 loop  begin ");
                        if (tmp2 == "" || tmp2 == null) { continue; }
                        string[] tmp3 = tmp2.Split(new Char[] { ',' });

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 3 loop   ");

                        Guid gUID = new Guid(tmp3[0]);
                        int messageID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[1]);
                        int from_StepID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[2]);
                        int from_JobID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[3]);
                        bool reply_sent = (tmp3[4] == "True" ? true : false);
                        string reply_sent_buttonText = tmp3[5];
                        string reply_sent_replyCode = tmp3[6];
                        string reply_sent_replyID = tmp3[7];
                        bool panel_state = (tmp3[8] == "True" ? true : false);
                        bool Decrypted = (tmp3[9] == "True" ? true : false);
                        bool Archive = (tmp3[10] == "True" ? true : false);

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile loop 4 messageID=" + messageID + " from_StepID=" + from_StepID);

                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
                        {
                            return;
                        }
                        int currentStep = from_StepID;

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 5 ");

                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep] == null)
                        {
                            return;
                        }

                        LCARS_NCI_Mission_Message Message = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID];

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile  6 ");

                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Contains(currentStep + "_" + Message.messageID))
                        { return; }

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 7 ");


                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 8 ");

                        LCARS_NCI_Mission_Archive_RunningStep_MessageItem SentMessageStack_Item = new LCARS_NCI_Mission_Archive_RunningStep_MessageItem();
                        SentMessageStack_Item.id = Guid.NewGuid();
                        SentMessageStack_Item.stepID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id;
                        SentMessageStack_Item.messageID = Message.messageID;
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list.Add(SentMessageStack_Item.id.ToString(), SentMessageStack_Item);

                        LCARS_Message_Type mT = new LCARS_Message_Type();
                        mT.id = SentMessageStack_Item.id;
                        mT.receiver_vessel = LCARSNCI_Bridge.Instance.getCurrentShip();
                        mT.sender = Message.sender;
                        mT.mission_message_id = messageID;
                        mT.mission_message_stepID = currentStep;
                        mT.mission_message_jobID = from_JobID;

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 9 ");

                        mT.reply_options = new List<LCARS_Message_Reply>();
                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 10 ");
                        foreach (LCARS_NCI_Mission_Message_Reply R in Message.reply_options)
                        {
                            UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 11 ");
                            LCARS_Message_Reply tmp = new LCARS_Message_Reply();

                            UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 12 ");
                            tmp.buttonText = R.buttonText;
                            tmp.replyCode = R.replyCode;
                            tmp.replyID = R.replyID;

                            UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 13 ");
                            mT.reply_options.Add(tmp);
                            UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 14 ");
                        }

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 14 ");

                        mT.sender_id_line = Message.sender_id_line;
                        mT.receiver_type = Message.receiver_type;
                        mT.receiver_code = Message.receiver_code;
                        mT.priority = Message.priority;
                        mT.setTitle(Message.title);
                        mT.setMessage(Message.message);

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 15 ");

                        if (Message.Encrypted)
                        {
                            mT.Encrypt(LCARS_NCI_Mission_Archive_Tools.ToInt(Message.encryptionMode));
                            if (Message.decryption_artefact != "")
                            {
                                mT.decryption_artefact = Message.decryption_artefact;
                            }
                        }
                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 16 ");
                        //mT.Queue();

                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Add(currentStep + "_" + Message.messageID);
                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 17 ");

                        if (LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue == null)
                        {
                            LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue = new System.Collections.Generic.List<LCARS_CommunicationQueue_Type>();
                        }
                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 17 ");

                        List<LCARS_CommunicationQueue_Type> tmp_CommunicationQueue_Type = LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue;
                        LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 18 ");

                        LCARS_CommunicationQueue_Type q = new LCARS_CommunicationQueue_Type();
                        q.id = new Guid();
                        q.Vessel = LCARS_NCI.Instance.Data.vessel;
                        q.plain_title = mT.title;
                        q.plain_message = mT.message;
                        q.orig_title = mT.title;
                        q.orig_message = mT.message;
                        q.receiver_code = mT.receiver_code;
                        q.Encrypted = mT.isEncrypted;
                        q.EncryptionMode = mT.encryptionMode;
                        q.DecryptHelper1 = "";
                        q.DecryptHelper2 = "";
                        q.Decrypted = Decrypted;
                        q.panel_state = panel_state;
                        q.Archive = Archive;
                        q.reply_sent = reply_sent;
                        q.reply_sent_buttonText = reply_sent_buttonText;
                        q.reply_sent_replyCode = reply_sent_replyCode;
                        q.reply_sent_replyID = reply_sent_replyID;
                        q.priority = mT.priority;
                        q.Message_Object = mT;

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 19 ");

                        LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue.Add(q);
                        foreach (LCARS_CommunicationQueue_Type qT in tmp_CommunicationQueue_Type)
                        {
                            LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue.Add(qT);
                        }

                        UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 20 loop  end ");

                    }

                //}
                //catch (Exception ex) { UnityEngine.Debug.Log("### LCARS_NCIMISSION_Scenario reinstateCommunicationQueue failed ex=" + ex); }
            */
            }
            else
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  node is missing ");

            }

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  2 ");

            ConfigNode RUNNINGMISSION = NCIMISSION_Node.GetNode("RUNNINGMISSION");
            if (RUNNINGMISSION != null)
            {
                if (LCARS_NCI.Instance.Data.MissionArchive == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
                }
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission = new LCARS_NCI_Mission_Archive_RunningMission();
                }


                
                
                if (RUNNINGMISSION.GetValue("mission_id") != "" && RUNNINGMISSION.GetValue("mission_id") != null)
                {

                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = new LCARS_NCI_Mission();
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.load(RUNNINGMISSION.GetValue("mission_id"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_step"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_job"));


                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog == null)
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog = new LCARS_NCI_MissionProgressLog();
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList = new List<LCARS_NCI_MissionProgressLog_Item>();
                    }

                    ConfigNode MISSIONPROGRESSLOG = RUNNINGMISSION.GetNode("MISSIONPROGRESSLOG");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid = MISSIONPROGRESSLOG.GetValue("missionGuid");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(MISSIONPROGRESSLOG.GetValue("current_step"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job = LCARS_NCI_Mission_Archive_Tools.ToInt(MISSIONPROGRESSLOG.GetValue("current_job"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_end_reached = (MISSIONPROGRESSLOG.GetValue("mission_end_reached") == "True" ? true: false ) ;
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.gain_collected = (MISSIONPROGRESSLOG.GetValue("gain_collected") == "True" ? true : false);

                    ConfigNode ITEMLIST = MISSIONPROGRESSLOG.GetNode("ITEMLIST");
                    ConfigNode[] ITEMS = ITEMLIST.GetNodes("ITEM");
                    foreach (ConfigNode I in ITEMS )
                    {
                        LCARS_NCI_MissionProgressLog_Item tmp = new LCARS_NCI_MissionProgressLog_Item();
                        tmp.timestamp = DateTime.Parse(I.GetValue("timestamp"));
                        tmp.stepID = LCARS_NCI_Mission_Archive_Tools.ToInt(I.GetValue("timestamp"));
                        tmp.jobID = LCARS_NCI_Mission_Archive_Tools.ToInt(I.GetValue("timestamp"));
                        tmp.subject = I.GetValue("timestamp");
                        tmp.details = I.GetValue("timestamp");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList.Add(tmp);
                    }
                    /*

                    ConfigNode ITEMLIST = new ConfigNode("ITEMLIST");
                    foreach (LCARS_NCI_MissionProgressLog_Item I in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList)
                    {
                        ConfigNode ITEM = new ConfigNode("ITEM");
                        ITEM.AddValue("timestamp", I.timestamp);
                        ITEM.AddValue("stepID", I.stepID);
                        ITEM.AddValue("jobID", I.jobID);
                        ITEM.AddValue("subject", I.subject);
                        ITEM.AddValue("details", I.details);
                        ITEMLIST.AddNode(ITEM);
                    }
                    MISSIONPROGRESSLOG.AddNode(ITEMLIST);
*/                
                }
            }

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  3 ");



            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  4 ");

            LCARS_NCI.Instance.Data.OnLoad();

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  5 ");


            base.OnLoad(node);
            DontDestroyOnLoad(this);
            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  end ");

        }

        public override void OnSave(ConfigNode node)
        {

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnSave:  Begin ");

            node.SetValue("loadcount", loadcount.ToString());

            ArtefactInventory = "";
            try
            {
                foreach (KeyValuePair<string, LCARS_ArtefactInventory_Type> pair in LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory)
                {
                    LCARS_ArtefactInventory_Type s = pair.Value;
                    string tmp = pair.Key + "," + s.usage_amount + "," + s.usage_times + "," + s.integrity + "|";
                    ArtefactInventory = ArtefactInventory + tmp;
                }
                node.SetValue("ArtefactInventory", ArtefactInventory);
            }
            catch (Exception ex) { Debug.Log("LCARS_NCIMISSION_Scenario OnSave LCARS_Inventory_data failed LCARS_Inventory_data=" + ArtefactInventory + " ex=" + ex); }
            Debug.Log("LCARS_NCIMISSION_Scenario OnSave LCARS_Inventory_data=" + ArtefactInventory);
            string LCARS_Queue_data = "";
            try
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave CommunicationQueue.Count=" + LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue.Count);
                foreach (LCARS_CommunicationQueue_Type qItem in LCARSNCI_Bridge.Instance.shipLCARS.lODN.CommunicationQueue)
                {
                    if (qItem.Message_Object.mission_message_id == null || qItem.Message_Object.mission_message_id < 1)
                    { continue; }

                    string tmp = qItem.Message_Object.id + "," +
                                    qItem.Message_Object.mission_message_id + "," +
                                    qItem.Message_Object.mission_message_stepID + "," +
                                    qItem.Message_Object.mission_message_jobID + "," +
                                    qItem.reply_sent + "," +
                                    ((qItem.reply_sent_buttonText != null) ? qItem.reply_sent_buttonText : "") + "," +
                                    ((qItem.reply_sent_replyCode != null) ? qItem.reply_sent_replyCode : "") + "," +
                                    ((qItem.reply_sent_replyID != null) ? qItem.reply_sent_replyID : "") + "," +
                                    qItem.panel_state + "," +
                                    qItem.Decrypted + "," +
                                    qItem.Archive + "|";
                    LCARS_Queue_data = LCARS_Queue_data + tmp;
                }

            }
            catch (Exception ex)
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave CommunicationQueue failed ex =" + ex);
            }
            node.SetValue("CommunicationQueue", LCARS_Queue_data);
            Debug.Log("LCARS_NCIMISSION_Scenario OnSave LCARS_Queue_data=" + LCARS_Queue_data);

            ConfigNode RUNNINGMISSION = new ConfigNode("RUNNINGMISSION");
            try
            {
                RUNNINGMISSION.AddValue("missionGuid", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid);
                RUNNINGMISSION.AddValue("mission_id", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.filename);
                RUNNINGMISSION.AddValue("current_step", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step);
                RUNNINGMISSION.AddValue("current_job", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
            }
            catch (Exception ex)
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave RUNNINGMISSION failed ex =" + ex);
            }
            try
            {
                ConfigNode MISSIONPROGRESSLOG = new ConfigNode("MISSIONPROGRESSLOG");
                MISSIONPROGRESSLOG.AddValue("missionGuid", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid);
                MISSIONPROGRESSLOG.AddValue("current_step", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step);
                MISSIONPROGRESSLOG.AddValue("current_job", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job);
                MISSIONPROGRESSLOG.AddValue("mission_end_reached", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_end_reached);
                MISSIONPROGRESSLOG.AddValue("gain_collected", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.gain_collected);

                ConfigNode ITEMLIST = new ConfigNode("ITEMLIST");
                foreach (LCARS_NCI_MissionProgressLog_Item I in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList)
                {
                    ConfigNode ITEM = new ConfigNode("ITEM");
                    ITEM.AddValue("timestamp", I.timestamp);
                    ITEM.AddValue("stepID", I.stepID);
                    ITEM.AddValue("jobID", I.jobID);
                    ITEM.AddValue("subject", I.subject);
                    ITEM.AddValue("details", I.details);
                    ITEMLIST.AddNode(ITEM);
                }
                MISSIONPROGRESSLOG.AddNode(ITEMLIST);

                RUNNINGMISSION.AddNode(MISSIONPROGRESSLOG);
            }
            catch(Exception ex)
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave MISSIONPROGRESSLOG failed ex =" + ex);
            }
            node.AddNode(RUNNINGMISSION);



            LCARS_NCI.Instance.Data.OnSave();

            base.OnSave(node);
            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnSave:  end ");
        }

    }






















 
}
