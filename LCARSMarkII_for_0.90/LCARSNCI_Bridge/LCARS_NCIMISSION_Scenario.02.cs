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
        GameScenes.FLIGHT  , GameScenes.SPACECENTER, GameScenes.TRACKSTATION  , GameScenes.EDITOR, etc )]
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

            NCIMISSION_Node = node;
            try
            {
                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList_IsUpdated = false;
                LCARS_NCI.Instance.Data.init();
                LCARSNCI_Bridge.Instance.NCIMISSION_Node_ScenarioLoad = NCIMISSION_Node;
            }
            catch(Exception ex) { UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  LCARS_NCI.Instance.Data.init failed ex="+ex); }



            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  1 ");

            if (NCIMISSION_Node.HasValue("loadcount"))
            {
                loadcount = Int16.Parse(NCIMISSION_Node.GetValue("loadcount"));
                loadcount++;
            }

            if (NCIMISSION_Node.HasValue("CommunicationQueue"))
            {
                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile begin ");
                string LCARS_Queue_data = "";
                LCARS_Queue_data = NCIMISSION_Node.GetValue("CommunicationQueue");
                LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup = LCARS_Queue_data;
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  string copied CommunicationQueue_String_SFS_Backup=" + LCARSNCI_Bridge.Instance.CommunicationQueue_String_SFS_Backup);
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  done ");
            }
            else
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnLoad setCommunicationQueueFromPersistentFile  node is missing ");

            }

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  2 ");

            ConfigNode RUNNINGMISSION = NCIMISSION_Node.GetNode("RUNNINGMISSION");
            if (RUNNINGMISSION != null)
            {
                UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  3 ");
                if (LCARS_NCI.Instance.Data.MissionArchive == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
                }
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission = new LCARS_NCI_Mission_Archive_RunningMission();
                }


                UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  4 ");
               
                
                if (RUNNINGMISSION.GetValue("mission_id") != "" && RUNNINGMISSION.GetValue("mission_id") != null)
                {

                    UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  5 ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = new LCARS_NCI_Mission();
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.load(RUNNINGMISSION.GetValue("mission_id"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_step"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_job"));


                    UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 ");
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog == null)
                    {
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 1");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog = new LCARS_NCI_MissionProgressLog();
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 2");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList = new List<LCARS_NCI_MissionProgressLog_Item>();
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 3");
                    }
                    UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 4");

                    try
                    {
                        ConfigNode MISSIONPROGRESSLOG = RUNNINGMISSION.GetNode("MISSIONPROGRESSLOG");
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 5");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid = MISSIONPROGRESSLOG.GetValue("missionGuid");
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 6");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(MISSIONPROGRESSLOG.GetValue("current_step"));
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 7");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job = LCARS_NCI_Mission_Archive_Tools.ToInt(MISSIONPROGRESSLOG.GetValue("current_job"));
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 8");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_end_reached = (MISSIONPROGRESSLOG.GetValue("mission_end_reached") == "True" ? true : false);
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  6 9");
                        //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.gain_collected = (MISSIONPROGRESSLOG.GetValue("gain_collected") == "True" ? true : false);

                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  7 ");
                        ConfigNode ITEMLIST = MISSIONPROGRESSLOG.GetNode("ITEMLIST");
                        ConfigNode[] ITEMS = ITEMLIST.GetNodes("ITEM");
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList = new List<LCARS_NCI_MissionProgressLog_Item>();
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  8 ");
                        foreach (ConfigNode I in ITEMS)
                        {
                            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  9 ");
                            LCARS_NCI_MissionProgressLog_Item tmp = new LCARS_NCI_MissionProgressLog_Item();
                            tmp.timestamp = DateTime.Parse(I.GetValue("timestamp"));
                            tmp.stepID = LCARS_NCI_Mission_Archive_Tools.ToInt(I.GetValue("stepID"));
                            tmp.jobID = LCARS_NCI_Mission_Archive_Tools.ToInt(I.GetValue("jobID"));
                            tmp.subject = I.GetValue("subject");
                            tmp.details = I.GetValue("details");
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList.Add(tmp);
                            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  10 ");
                        }
                        UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  11 ");
                    }
                    catch { }
                }
                UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  12 ");
            }

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  13 ");



            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  14 ");

            LCARS_NCI.Instance.Data.OnLoad();

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  15 ");


            base.OnLoad(node);
            DontDestroyOnLoad(this);
            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnLoad:  end ");

        }

        public override void OnSave(ConfigNode node)
        {

            UnityEngine.Debug.Log("LCARS_NCIMISSION_Scenario OnSave:  Begin ");

            node.SetValue("loadcount", loadcount.ToString());

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
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid == null || LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid == "") { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid = Guid.NewGuid().ToString(); }
                
                RUNNINGMISSION.AddValue("missionGuid", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid);
                RUNNINGMISSION.AddValue("mission_id", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.filename);
                RUNNINGMISSION.AddValue("mission_name", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
                RUNNINGMISSION.AddValue("current_step", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step);
                RUNNINGMISSION.AddValue("current_job", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_name = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job;

                Debug.Log("LCARS_NCIMISSION_Scenario OnSave RUNNINGMISSION worked ");
            }
            catch (Exception ex)
            {
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave RUNNINGMISSION failed ex =" + ex);
            }
            try
            {
                ConfigNode MISSIONPROGRESSLOG = new ConfigNode("MISSIONPROGRESSLOG");
                MISSIONPROGRESSLOG.AddValue("missionGuid", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid);
                MISSIONPROGRESSLOG.AddValue("mission_name", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_name);
                MISSIONPROGRESSLOG.AddValue("current_step", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step);
                MISSIONPROGRESSLOG.AddValue("current_job", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job);
                MISSIONPROGRESSLOG.AddValue("mission_end_reached", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_end_reached);
                //MISSIONPROGRESSLOG.AddValue("gain_collected", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.gain_collected);

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
                Debug.Log("LCARS_NCIMISSION_Scenario OnSave MISSIONPROGRESSLOG worked ");
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
