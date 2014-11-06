using System;
using System.Collections.Generic;
using System.Linq;

namespace LCARSMarkII
{


















    [KSPScenario(
                /*ScenarioCreationOptions.AddToNewCareerGames |
                ScenarioCreationOptions.AddToExistingCareerGames |
                ScenarioCreationOptions.AddToNewScienceSandboxGames |
                ScenarioCreationOptions.AddToExistingScienceSandboxGames,*/
                ScenarioCreationOptions.AddToAllGames,

                GameScenes.FLIGHT, GameScenes.SPACECENTER,GameScenes.TRACKSTATION  /*, GameScenes.EDITOR, GameScenes.SPH etc */)]
    public class LCARS_NCI_Scenario : ScenarioModule
    {


        public LCARS_NCI NCI;


        public LCARS_NCI_Scenario controller
        {
            get
            {


                Game g = HighLogic.CurrentGame;
                if (g == null) return null;
                foreach (ProtoScenarioModule mod in g.scenarios)
                {
                    if (mod.moduleName == typeof(LCARS_NCI_Scenario).Name)
                    {
                        return (LCARS_NCI_Scenario)mod.moduleRef;
                    }
                }
                return (LCARS_NCI_Scenario)g.AddProtoScenarioModule(typeof(LCARS_NCI_Scenario), GameScenes.FLIGHT).moduleRef;
            }
            private set { }
        }





        [KSPField(isPersistant = true)]
        internal int loadcount = 0;

        internal ConfigNode NCI_Node = null;
        
        public override void OnLoad(ConfigNode node)
        {
            /*if (NCI == null)
            {
                NCI = LCARS_NCI.Instance;
            }*/

            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  Begin ");

            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList==null)
            {
                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList = new Dictionary<Guid, LCARS_NCI_Object>();
            }
            NCI_Node = node;
            LCARSNCI_Bridge.Instance.NCI_Node_ScenarioLoad = NCI_Node;


            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  1 ");

            if (NCI_Node.HasValue("loadcount"))
            {
                loadcount = Int16.Parse(NCI_Node.GetValue("loadcount"));
                loadcount++;
            }




            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  2 ");

            ConfigNode RUNNINGMISSION = NCI_Node.GetNode("RUNNINGMISSION");
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
                if (RUNNINGMISSION.GetValue("mission_id") != "" && RUNNINGMISSION.GetValue("mission_id")!=null)
                {

                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = new LCARS_NCI_Mission();
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.load(RUNNINGMISSION.GetValue("mission_id"));
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_step"));
                    
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = LCARS_NCI_Mission_Archive_Tools.ToInt(RUNNINGMISSION.GetValue("current_job"));
                }
            }
            /*
            */
            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  3 ");


            ConfigNode node_OBJECTS = NCI_Node.GetNode("OBJECTS");
            if (node_OBJECTS != null)
            {
                print("LCARS_NCI_Scenario: Loading " + node_OBJECTS.CountNodes.ToString() + " known NCIObj");
                foreach (ConfigNode node_NCIObj in node_OBJECTS.GetNodes("NCIObj"))
                {
                    Guid id = new Guid(node_NCIObj.GetValue("guid"));
                    LCARS_NCI_Object NCIObj = new LCARS_NCI_Object();
                    NCIObj.group = node_NCIObj.GetValue("group");
                    NCIObj.partname = node_NCIObj.GetValue("partname");
                    NCIObj.actionpoints = new List<string>();
                    foreach (string s in split_EggString(node_NCIObj.GetValue("actionpoints")))
                    {
                        NCIObj.actionpoints.Add(s);
                    }
                    ConfigNode node_missions = NCI_Node.GetNode("MISSIONS");
                    if (node_missions != null)
                    {
                        foreach (ConfigNode aM in node_missions.GetNodes("MISSION"))
                        {
                            LCARS_NCI_Object_assigned_missions nAM = new LCARS_NCI_Object_assigned_missions();
                            nAM.mission_id = aM.GetValue("mission_id");
                            nAM.condition_type = aM.GetValue("condition_type");
                            nAM.condition_egg = aM.GetValue("condition_egg");
                            nAM.condition_part = aM.GetValue("condition_part");
                            nAM.condition_distance = aM.GetValue("condition_distance");
                            nAM.mission = new LCARS_NCI_Mission();
                            nAM.mission = nAM.mission.load(nAM.mission_id);
                            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions == null)
                            {
                                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions = new Dictionary<string,LCARS_NCI_Object_assigned_missions>();
                            }
                            if (!LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions.ContainsKey(nAM.mission_id))
                            {
                                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions.Add(nAM.mission_id,nAM);
                            }
                        }
                    }
                    registerNCIObjects(id, NCIObj);
                }

            }

            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  4 ");

            LCARS_NCI.Instance.Data.OnLoad();

            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  5 ");

            base.OnLoad(node);
            DontDestroyOnLoad(this);
            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  end ");

        }
        
        internal void registerNCIObjects(Guid id, LCARS_NCI_Object NCIObj)
        {
            if (!LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.ContainsKey(id))
            {
                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Add(id, NCIObj);
            }
        }
        /*
        public void reinstateCommunicationQueue()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Scenario reinstateCommunicationQueue begin ");
            ConfigNode RUNNINGMISSION = NCI_Node.GetNode("RUNNINGMISSION");
            if (RUNNINGMISSION != null)
            {
                if (RUNNINGMISSION.HasValue("CommunicationQueue"))
                {
                    try
                    {
                        LCARSNCI_Bridge.Instance.setCommunicationQueueFromPersistentFile(NCI_Node);
                    }
                    catch (Exception ex) { UnityEngine.Debug.Log("### LCARS_NCI_Scenario reinstateCommunicationQueue failed ex=" + ex); }
                }
            }
        }
        public void reinstateArtefactInventory()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Scenario reinstateArtefactInventory begin ");
            if (NCI_Node.HasValue("ArtefactInventory"))
            {
                try
                {
                    LCARSNCI_Bridge.Instance.setShipInventoryFromPersistentFile(NCI_Node);
                }
                catch (Exception ex) { UnityEngine.Debug.Log("### LCARS_NCI_Scenario reinstateArtefactInventory failed ex=" + ex); }
            }
        }
        */
        public override void OnSave(ConfigNode node)
        {
            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave:  Begin ");

            node.SetValue("loadcount", loadcount.ToString());

            /*
            try
            {
                node.SetValue("ArtefactInventory", LCARSNCI_Bridge.Instance.getShipInventoryForPersistentFile());
            }
            catch (Exception ex)
            {
                Debug.Log("LCARS_NCI_Scenario OnSave  ArtefactInventory failed ex=" + ex);
            }
             * 
            ConfigNode RUNNINGMISSION = new ConfigNode("RUNNINGMISSION");
            try
            {
                RUNNINGMISSION.AddValue("missionGuid", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid);
                RUNNINGMISSION.AddValue("mission_id", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.filename);
                RUNNINGMISSION.AddValue("current_step", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step);
                RUNNINGMISSION.AddValue("current_job", LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
                try
                {
                    RUNNINGMISSION.AddValue("CommunicationQueue", LCARSNCI_Bridge.Instance.getCommunicationQueueForPersistentFile());
                }
                catch (Exception ex) { UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave:  getCommunicationQueueForPersistentFile failed ex="+ex); }
                
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
            catch 
            {
            
            }
            node.AddNode(RUNNINGMISSION);
            */

            ConfigNode node_OBJECTS = new ConfigNode("OBJECTS");
            foreach (Guid id in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Keys)
            {
                ConfigNode NCIObj = new ConfigNode("NCIObj");
                NCIObj.AddValue("guid", id.ToString());
                NCIObj.AddValue("partname", LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].partname);
                NCIObj.AddValue("actionpoints", join_EggList(LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].actionpoints));
                ConfigNode MISSIONS = new ConfigNode("MISSIONS");
                try
                {
                    foreach (LCARS_NCI_Object_assigned_missions aM in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions.Values)
                    {
                        ConfigNode naM = new ConfigNode("MISSION");
                        naM.AddValue("mission_id", aM.mission.filename);
                        naM.AddValue("condition_type", aM.condition_type);
                        naM.AddValue("condition_egg", aM.condition_egg);
                        naM.AddValue("condition_part", aM.condition_part);
                        naM.AddValue("condition_distance", aM.condition_distance);
                        MISSIONS.AddNode(naM);
                    }
                }
                catch
                {

                }
                NCIObj.AddNode(MISSIONS);

                node_OBJECTS.AddNode(NCIObj);
            }
            node.AddNode(node_OBJECTS);

            LCARS_NCI.Instance.Data.OnSave();
 
            base.OnSave(node);
            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave:  end ");
        }


        internal Dictionary<Guid, LCARS_NCI_Object> get_availableEggs()
        {
            return LCARS_NCI.Instance.Data.sfs_ActiveObjectsList;
        }
        internal LCARS_NCI_Object get_availableEggs(Guid id)
        {
            return LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id];
        }
        private List<string> split_EggString(string eggString)
        {
            return eggString.Split(new Char[] { ',' }).ToList<string>();
        }
        private string join_EggList(List<string> eggList)
        {
            string tmp = "";
            foreach (string egg in eggList)
            {
                if(tmp.Length>1){tmp+=",";}
                tmp += egg;
            }
            return tmp;
        }

        
        /*
        internal void addObject(LCARS_NCI_Object Selected_LCARS_NCI_Object, Guid guid)
        {
            UnityEngine.Debug.Log("LCARS_NCI_Scenario addObject Begin ");
            UnityEngine.Debug.Log("NCI_Node=" + NCI_Node);
            string group = Selected_LCARS_NCI_Object.group;
            string partname = Selected_LCARS_NCI_Object.partname;
            List<string> actionpoints = Selected_LCARS_NCI_Object.actionpoints;

            UnityEngine.Debug.Log("LCARS_NCI_Scenario addObject Selected_LCARS_NCI_Object parsed ");

            ConfigNode node_OBJECTS = NCI_Node.GetNode("OBJECTS");
            ConfigNode NCIObj = new ConfigNode("NCIObj");
            NCIObj.AddValue("guid", guid.ToString());
            NCIObj.AddValue("group", group);
            NCIObj.AddValue("partname", partname);
            string tmp = "";
            foreach (string egg in actionpoints)
            {
                tmp = ","+egg;
            }
            NCIObj.AddValue("actionpoints", tmp);
            UnityEngine.Debug.Log("LCARS_NCI_Scenario addObject actionpoints parsed ");
            UnityEngine.Debug.Log("NCIObj=" + NCIObj);

            node_OBJECTS.AddNode(NCIObj);
            NCI_Node.AddNode(node_OBJECTS);
            UnityEngine.Debug.Log("NCI_Node=" + NCI_Node);
            UnityEngine.Debug.Log("LCARS_NCI_Scenario addObject end ");
        }
        */
    }

}
