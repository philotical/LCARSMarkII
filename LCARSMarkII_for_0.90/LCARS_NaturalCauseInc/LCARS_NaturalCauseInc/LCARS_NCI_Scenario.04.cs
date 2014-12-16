using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCARSMarkII
{


















    [KSPScenario(
                /*ScenarioCreationOptions.AddToNewCareerGames |
                ScenarioCreationOptions.AddToExistingCareerGames |
                ScenarioCreationOptions.AddToNewScienceSandboxGames |
                ScenarioCreationOptions.AddToExistingScienceSandboxGames,*/
                ScenarioCreationOptions.AddToAllGames,

                GameScenes.FLIGHT, GameScenes.SPACECENTER,GameScenes.TRACKSTATION  /*, GameScenes.EDITOR, etc */)]
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
        [KSPField(isPersistant = true)]
        internal string ArtefactInventory = "";

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
            if (HighLogic.LoadedSceneIsFlight)
            {
            }
                UnityEngine.Debug.Log("### LCARS_NCI_Scenario reinstateArtefactInventory begin ");
                if (NCI_Node.HasValue("ArtefactInventory"))
                {
                    Debug.Log("LCARS_NCI_Scenario OnLoad setShipInventoryFromPersistentFile  1 ");
                    ArtefactInventory = NCI_Node.GetValue("ArtefactInventory");
                    LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup = ArtefactInventory;
                    Debug.Log("LCARS_NCI_Scenario OnLoad setShipInventoryFromPersistentFile  string copied ArtefactInventory_String_SFS_Backup=" + LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup);
                    Debug.Log("LCARS_NCI_Scenario OnLoad setShipInventoryFromPersistentFile  done ");
                }
                else
                {
                    Debug.Log("LCARS_NCI_Scenario OnLoad setShipInventoryFromPersistentFile  node is missing ");
                }




            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnLoad:  3 ");


            ConfigNode node_OBJECTS = NCI_Node.GetNode("OBJECTS");
            if (node_OBJECTS != null)
            {
                print("LCARS_NCI_Scenario: Loading " + node_OBJECTS.CountNodes.ToString() + " known NCIObj");
                foreach (ConfigNode node_NCIObj in node_OBJECTS.GetNodes("NCIObj"))
                {
                    print("LCARS_NCI_Scenario: Loading bodyname=" + node_NCIObj.GetValue("bodyname") + " partname=" + node_NCIObj.GetValue("partname") + " guid=" + node_NCIObj.GetValue("guid"));
                    if (node_NCIObj.GetValue("bodyname") == "" && node_NCIObj.GetValue("partname") == "")
                    {
                        continue;
                    }
                    Guid id = new Guid(node_NCIObj.GetValue("guid"));
                    LCARS_NCI_Object NCIObj = new LCARS_NCI_Object();
                    if (node_NCIObj.GetValue("bodyname") != null && node_NCIObj.GetValue("bodyname") != "")
                    {
                        NCIObj.bodyname = node_NCIObj.GetValue("bodyname");
                        Debug.Log("LCARS_NCI_Scenario: Loading searching NCIObj.bodyname=" + NCIObj.bodyname);
                        bool planet_present = false;
                        foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair_planet_check in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                        {
                            try
                            {
                                if (pair_planet_check.Value.bodyname == NCIObj.bodyname)
                                {
                                    planet_present = true;
                                }
                            }
                            catch { }
                        }
                        if (planet_present)
                        {
                            if (!LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.ContainsKey(id))
                            {
                                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Add(id, LCARS_NCI.Instance.Data.ObjectArchive[NCIObj.bodyname]);
                            }
                            Debug.Log("LCARS_NCI_Scenario: Loading planet present NCIObj.bodyname="+NCIObj.bodyname+" planet_guid=" + id);
                        }
                        else
                        {

                            Guid planet_guid = Guid.NewGuid();
                            ConfigNode PQSCityObjects = node_NCIObj.GetNode("PQSCityObjects");
                            //NCIObj.Locations = PQSCityObjects.GetNodes("Locations");
                            if (NCIObj.Locations == null)
                            {
                                NCIObj.Locations = new Dictionary<string, List<string>>();
                            }
                            foreach (ConfigNode locationC in PQSCityObjects.GetNodes("Location"))
                            {
                                List<string> actionpoint_list = new List<string>();
                                foreach (string s in locationC.GetValue("actionpoints").Split(new Char[] { ',' }))
                                {
                                    if (s == "") { continue; }
                                    actionpoint_list.Add(s);
                                }
                                NCIObj.Locations.Add(locationC.GetValue("name"), actionpoint_list);
                            }
                            LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Add(planet_guid, NCIObj);
                            Debug.Log("LCARS_NCI_Scenario: Loading planet added NCIObj.bodyname="+NCIObj.bodyname+" planet_guid=" + planet_guid);
                        }
                    }
                    else
                    {
                        //NCIObj.group = node_NCIObj.GetValue("group");

                        NCIObj.partname = node_NCIObj.GetValue("partname");
                        NCIObj.actionpoints = new List<string>();
                        try
                        {
                            foreach (string s in split_EggString(node_NCIObj.GetValue("actionpoints")))
                            {
                                NCIObj.actionpoints.Add(s);
                            }
                        }
                        catch { }
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

        public override void OnSave(ConfigNode node)
        {
            UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave:  Begin ");

            node.SetValue("loadcount", loadcount.ToString());

            if (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneHasPlanetarium)
            {

                ArtefactInventory = LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup;
                try
                {
                    ArtefactInventory = LCARSNCI_Bridge.Instance.get_ArtefactInventory_String_for_SFS();
                }
                catch (Exception ex) { Debug.Log("LCARS_NCI_Scenario OnSave LCARS_Inventory_data failed LCARS_Inventory_data=" + ArtefactInventory + " ex=" + ex); }
                node.SetValue("ArtefactInventory", ArtefactInventory);
                Debug.Log("LCARS_NCI_Scenario OnSave LCARS_Inventory_data=" + ArtefactInventory);
            }

            ConfigNode node_OBJECTS = new ConfigNode("OBJECTS");
            foreach (Guid id in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Keys)
            {
                try
                {
                    if ((LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].bodyname == "" || LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].bodyname == null) && (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].partname == "" || LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].partname == null))
                    {
                        continue;
                    }
                }
                catch { continue; }
                ConfigNode NCIObj = new ConfigNode("NCIObj");
                NCIObj.AddValue("guid", id.ToString());

                if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].isCelestialBody)
                {
                    UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave: OBJECTS bodyname=" + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].bodyname);
                    NCIObj.AddValue("bodyname", LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].bodyname);
                    ConfigNode PQSCityObjects = new ConfigNode("PQSCityObjects");
                    foreach (KeyValuePair<string, List<string>> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].Locations)
                    {
                        UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave: OBJECTS Location=" + pair.Key);
                        ConfigNode Location = new ConfigNode("Location");
                        string actpoi = "";
                        if (pair.Value.Count > 0)
                        {
                            actpoi = pair.Value.Aggregate((i, j) => i + "," + j);
                        }
                        UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave: OBJECTS actionpoints=" + actpoi);
                        Location.AddValue("name", pair.Key);
                        Location.AddValue("actionpoints", actpoi);
                        PQSCityObjects.AddNode(Location);
                    }
                    NCIObj.AddNode(PQSCityObjects);
                }
                else
                {
                    UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave: OBJECTS partname=" + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].partname);
                    NCIObj.AddValue("partname", LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].partname);
                    NCIObj.AddValue("actionpoints", join_EggList(LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].actionpoints));
                }
                ConfigNode MISSIONS = new ConfigNode("MISSIONS");
                try
                {
                    foreach (LCARS_NCI_Object_assigned_missions aM in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id].missions.Values)
                    {
                        UnityEngine.Debug.Log("LCARS_NCI_Scenario OnSave: OBJECTS aM.mission.filename=" + aM.mission.filename);
                        ConfigNode naM = new ConfigNode("MISSION");
                        naM.AddValue("mission_id", aM.mission.filename);
                        naM.AddValue("condition_type", aM.condition_type);
                        naM.AddValue("condition_egg", aM.condition_egg);
                        naM.AddValue("condition_part", aM.condition_part);
                        naM.AddValue("condition_body", aM.condition_body);
                        naM.AddValue("condition_PQSCity", aM.condition_PQSCity);
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

        private List<string> split_EggString(string eggString)
        {
            return eggString.Split(new Char[] { ',' }).ToList<string>();
        }
        private string join_EggList(List<string> eggList)
        {
            string tmp = "";
            try
            {
                foreach (string egg in eggList)
                {
                    if (tmp.Length > 1) { tmp += ","; }
                    tmp += egg;
                }
            }
            catch { }
            return tmp;
        }

        internal Dictionary<Guid, LCARS_NCI_Object> get_availableEggs()
        {
            return LCARS_NCI.Instance.Data.sfs_ActiveObjectsList;
        }
        internal LCARS_NCI_Object get_availableEggs(Guid id)
        {
            return LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[id];
        }
    }

}
