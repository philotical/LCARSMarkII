
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{

    public sealed class LCARSNCI_Bridge
    {
        private static LCARSNCI_Bridge _instance;
        public static LCARSNCI_Bridge Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LCARSNCI_Bridge();
                }

                return _instance;
            }
        }

        public LCARSMarkII shipLCARS;
        //public LCARS_NCI_Scenario ScenarioRef=null;
        public ConfigNode NCI_Node_ScenarioLoad { get; set; }
        public ConfigNode NCIMISSION_Node_ScenarioLoad { get; set; }
        public string CommunicationQueue_String_SFS_Backup { get; set; }
        public string ArtefactInventory_String_SFS_Backup { get; set; }

        //public lODN_ArtefactInventory

        public bool LCARS_NCI_BridgeReady()
        {
            //Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady begin ");
            try
            {
                if (LCARS_NCI.Instance.Data != null)
                {
                    //Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady LCARS_NCI.Instance.Data is present "); 
                }
            }
            catch { Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady LCARS_NCI.Instance.Data == null "); return false; }

            Vessel ship = getCurrentShip();
            //Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady ship found" );

            try
            {
                shipLCARS = ship.LCARS();
                //Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady shipLCARS is present "); 
            }
            catch { Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady shipLCARS == null "); return false; }
            
            Debug.Log("LCARSNCI_Bridge LCARS_NCI_BridgeReady ");

            return true;
        }
        /*
        public LCARS_NCI_Scenario getScenarioRef()
        {
            ScenarioRef = ScenarioRunner.fetch.GetComponent<LCARS_NCI_Scenario>();
            return ScenarioRef;
            return null;
        }
         */

        public Vessel getCurrentShip()
        {
            try
            {
                return LCARS_NCI.Instance.Data.vessel;
            }
            catch 
            {
                return FlightGlobals.ActiveVessel;
            }
        }

        public LCARSMarkII getCurrentShipLCARS()
        {
            shipLCARS = getCurrentShip().LCARS();
            return shipLCARS;
        }

        public void SendMessageReply(string value1 = "", string value2 = "", string value3 = "")
        {
            Debug.Log("LCARSNCI_Bridge SendMessageReply begin ");
            if (!LCARS_NCI_BridgeReady()) { return; }

            UnityEngine.Debug.Log("### LCARSNCI_Bridge SendMessageReply   value1=" + value1 + " value2=" + value2 + " value3=" + value3);
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.processMessageReply(value1,value2,value3);
        }


        public int getCurrentStepID()
        {
            try
            {
                return LCARS_NCI_Mission_Archive_Tools.get_current_stepID();
            }
            catch 
            {
                return 0;
            }
        }
        public int getCurrentJobID()
        {
            try
            {
            return LCARS_NCI_Mission_Archive_Tools.get_current_jobID();
            }
            catch
            {
                return 0;
            }
        }



        public string get_ArtefactInventory_String_for_SFS()
        {
            if (!LCARS_NCI_BridgeReady()) { return "ERROR: Bridge Not Ready"; }

            string ArtefactInventory = "";
            try
            {
                foreach (KeyValuePair<string, LCARS_ArtefactInventory_Type> pair in LCARSNCI_Bridge.Instance.shipLCARS.lODN.ArtefactInventory)
                {
                    LCARS_ArtefactInventory_Type s = pair.Value;
                    string tmp = pair.Key + "," + s.usage_amount + "," + s.usage_times + "," + s.integrity + "|";
                    ArtefactInventory = ArtefactInventory + tmp;
                }
            }
            catch (Exception ex) { 
                Debug.Log("LCARSNCI_Bridge get_ArtefactInventory_String_for_SFS failed LCARS_Inventory_data=" + ArtefactInventory + " ex=" + ex);
                ArtefactInventory = LCARSNCI_Bridge.Instance.ArtefactInventory_String_SFS_Backup;
                Debug.Log("LCARSNCI_Bridge get_ArtefactInventory_String_for_SFS loading from backup string ArtefactInventory=" + ArtefactInventory);
            }

            return ArtefactInventory;
        }

        public bool DoesPlayerHaveArtefact(string itemKey = null)
        {
            Debug.Log("LCARSNCI_Bridge DoesPlayerHaveArtefact begin ");
            //LCARS_NCI_InventoryItem_Type foo = new LCARS_NCI_InventoryItem_Type();
            //LCARS_ArtefactInventory_Type foo2 = new LCARS_ArtefactInventory_Type();
            try
            {
                return (getShipInventory(itemKey) == null) ? false : true;
            }
            catch { return false; }
        }

        public bool giveArtefactToPlayer(string artefactIDcode, bool write_to_progresslog = true, bool bypass_mission = false)
        {
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer begin ");
            if (!LCARS_NCI_BridgeReady()) { return false; }


            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 1 ");

            LCARS_NCI_InventoryItem_Type tmp = null;

            if(bypass_mission)
            {
                try
                {
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 2 ");
                    tmp = LCARS_NCI.Instance.Data.Artefact_List[artefactIDcode];
                }
                catch { }
            }
            else
            {
                try
                {
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 3 ");
                    tmp = getMissionInventory(artefactIDcode);
                }
                catch { }
            }
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 4 ");
            if (tmp == null) { return false; }

            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 ");
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 artefactIDcode=" + artefactIDcode);
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 shipLCARS=" + shipLCARS);
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 shipLCARS.lODN=" + shipLCARS.lODN);
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 shipLCARS.lODN.ArtefactInventory=" + shipLCARS.lODN.ArtefactInventory);
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 5 shipLCARS.lODN.ArtefactInventory.ContainsKey(artefactIDcode)=" + shipLCARS.lODN.ArtefactInventory.ContainsKey(artefactIDcode));
            if (!shipLCARS.lODN.ArtefactInventory.ContainsKey(artefactIDcode)) 
            {
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 6 ");
                try
                {
                    LCARS_ArtefactInventory_Type tmp2 = new LCARS_ArtefactInventory_Type();
                    tmp2.name = tmp.name;
                    tmp2.idcode = tmp.idcode;
                    tmp2.description = tmp.description;
                    tmp2.icon = tmp.icon;
                    tmp2.integrity = tmp.integrity;
                    tmp2.isDamagable = tmp.isDamagable;
                    tmp2.powerconsumption = tmp.powerconsumption;
                    tmp2.usage_amount = tmp.usage_amount;
                    tmp2.usage_times = "0";
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 7 ");
                    shipLCARS.lODN.ArtefactInventory.Add(tmp2.idcode, tmp2);
                }
                catch { 
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 8 ");
                    return false; 
                }
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9 ");
            }
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9b ");
            if (!LCARS_NCI.Instance.Data.Artefacts.list.ContainsKey(artefactIDcode))
            {
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9c ");
                try
                {
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9d ");
                    LCARS_NCI_InventoryItem_Type NCIArt = new LCARS_NCI_InventoryItem_Type();

                    NCIArt.name = tmp.name;
                    NCIArt.description = tmp.description;
                    NCIArt.idcode = tmp.idcode;
                    NCIArt.icon = tmp.icon;
                    NCIArt.isDamagable = tmp.isDamagable;
                    NCIArt.integrity = tmp.integrity;
                    NCIArt.usage_amount = tmp.usage_amount;
                    NCIArt.usage_times = tmp.usage_times;
                    NCIArt.powerconsumption = tmp.powerconsumption;

                    LCARS_NCI.Instance.Data.Artefacts.list.Add(NCIArt.idcode, NCIArt);
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9e ");
                }
                catch
                {
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9f ");
                }
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9b ");
            }
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 9b ");
            try
            {
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 10 ");
                if (write_to_progresslog)
                {
                    Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 11 ");
                    string newOwner = "You";
                    if (LCARS_NCI.Instance.Data.kerbal != "" && LCARS_NCI.Instance.Data.kerbal != null) { newOwner = LCARS_NCI.Instance.Data.kerbal; }
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                                LCARS_NCI_Mission_Archive_Tools.getNow(),
                                LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                                LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "Artefact Inventory:",
                                "The Artefact " + artefactIDcode + " has changed the owner. New owner: " + newOwner
                            );
                }
                Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 12 ");
            }
            catch { }
            Debug.Log("LCARSNCI_Bridge giveArtefactToPlayer 13 ");

            return true;
        }

        public bool takeArtefactFromPlayer(string artefactIDcode,bool write_to_progresslog = true)
        {
            Debug.Log("LCARSNCI_Bridge takeArtefactFromPlayer begin ");
            if (!LCARS_NCI_BridgeReady()) { return false; }

            if (!shipLCARS.lODN.ArtefactInventory.ContainsKey(artefactIDcode)) { return false; }
            try
            {
                shipLCARS.lODN.ArtefactInventory.Remove(artefactIDcode);
            }
            catch { return false; }

            try
            {
                if (write_to_progresslog)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                                LCARS_NCI_Mission_Archive_Tools.getNow(),
                                LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                                LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "Artefact Inventory:",
                                "The Artefact " + artefactIDcode + " has changed the owner. New owner: " + LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality].name
                            );
                }
            }
            catch { }

            return true;
        }

        public LCARS_ArtefactInventory_Type getShipInventory(string itemKey = null)
        {
            Debug.Log("LCARSNCI_Bridge getShipInventory <" + itemKey + "> begin ");
            //LCARS_NCI_InventoryItem_Type foo = new LCARS_NCI_InventoryItem_Type();
            //LCARS_ArtefactInventory_Type foo2 = new LCARS_ArtefactInventory_Type();
            try
            {
                return shipLCARS.lODN.ArtefactInventory[itemKey];
            }
            catch { return null; }
        }

        /*
        public string getShipInventoryForPersistentFile()
        {
            Debug.Log("LCARSNCI_Bridge getShipInventoryForPersistentFile begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            string LCARS_Inventory_data = "";
            try
            {
                foreach (KeyValuePair<string, LCARS_ArtefactInventory_Type> pair in shipLCARS.lODN.ArtefactInventory)
                {
                    LCARS_ArtefactInventory_Type s = pair.Value;
                    string tmp = pair.Key + "," + s.usage_amount + "," + s.usage_times + "," + s.integrity + "|";
                    LCARS_Inventory_data = LCARS_Inventory_data + tmp;
                }
            }
            catch (Exception ex) { Debug.Log("LCARSNCI_Bridge getShipInventoryForPersistentFile failed ex=" + ex); }
            Debug.Log("LCARSNCI_Bridge getShipInventoryForPersistentFile end LCARS_Inventory_data=" + LCARS_Inventory_data);
            return LCARS_Inventory_data;
        }
        */
        public void setShipInventoryFromPersistentFile(string ArtefactInventory, Vessel ship)
        {
            Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile begin ");
            Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  1 (string)ArtefactInventory=" + ArtefactInventory);
            if (!LCARS_NCI_BridgeReady()) { return; }
            Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  2 LCARS_NCI_BridgeReady done ");

            string LCARS_Inventory_data = "";
            LCARS_Inventory_data = ArtefactInventory;

            Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  3 LCARS_Inventory_data=" + LCARS_Inventory_data);
            
            string[] tmp1 = LCARS_Inventory_data.Split(new Char[] { '|' });
            foreach (string tmp2 in tmp1)
            {
                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  4 ");
                if (tmp2 == "" || tmp2 == null) { continue; }
                string[] tmp3 = tmp2.Split(new Char[] { ',' });
                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  5 ");

                //LCARS_NCI_InventoryItem_Type tmp = getMissionInventory(tmp3[0]);
                LCARS_NCI_InventoryItem_Type tmp = null;
                try
                {
                    tmp = LCARS_NCI.Instance.Data.Artefact_List[tmp3[0]];
                }
                catch { }

                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile  5b ");


                LCARS_ArtefactInventory_Type tmp4 = new LCARS_ArtefactInventory_Type();
                tmp4.name = tmp.name;
                tmp4.idcode = tmp.idcode;
                tmp4.description = tmp.description;
                tmp4.icon = tmp.icon;
                tmp4.integrity = tmp.integrity;
                tmp4.isDamagable = tmp.isDamagable;
                tmp4.powerconsumption = tmp.powerconsumption;
                tmp4.usage_amount = tmp.usage_amount;
                tmp4.usage_times = "0";
                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile 6 ");
                if (ship.LCARS().lODN == null)
                {
                    Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile lODN missing ");
                    ship.LCARS().lODN = new LCARS_ODN_Util();
                }
                if (ship.LCARS().lODN.ArtefactInventory == null)
                {
                    Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile ArtefactInventory missing ");
                    ship.LCARS().lODN.ArtefactInventory = new Dictionary<string, LCARS_ArtefactInventory_Type>();
                }
                try
                {
                    ship.LCARS().lODN.ArtefactInventory.Add(tmp4.idcode, tmp4);
                }
                catch { Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile ArtefactInventory.Add failed shipLCARS missing! ");  }
                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile 7 ");

                ship.LCARS().lODN.ArtefactInventory[tmp4.idcode].usage_amount = tmp3[1];
                ship.LCARS().lODN.ArtefactInventory[tmp4.idcode].usage_times = tmp3[2];
                ship.LCARS().lODN.ArtefactInventory[tmp4.idcode].integrity = tmp3[3];
                Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile 8 ");
            }
            Debug.Log("LCARSNCI_Bridge setShipInventoryFromPersistentFile done ");
        }
        /*
        public void reinstateArtefactInventory()
        {
            UnityEngine.Debug.Log("### LCARSNCI_Bridge reinstateArtefactInventory begin ");
            if (!LCARS_NCI_BridgeReady()) { return; }
            ConfigNode RUNNINGMISSION = NCIMISSION_Node_ScenarioLoad.GetNode("RUNNINGMISSION");
            if (RUNNINGMISSION != null)
            {
                if (RUNNINGMISSION.HasValue("CommunicationQueue"))
                {
                    try
                    {

                        //if (ScenarioRef == null) { ScenarioRef = getScenarioRef(); }
                        setShipInventoryFromPersistentFile(NCIMISSION_Node_ScenarioLoad);
                        UnityEngine.Debug.Log("### LCARSNCI_Bridge reinstateArtefactInventory done ");
                    }
                    catch (Exception ex) { UnityEngine.Debug.Log("### LCARSNCI_Bridge reinstateArtefactInventory failed ex=" + ex); }
                }
            }            
        }
    */

        public Dictionary<string, LCARS_ArtefactInventory_Type> getShipInventory()
        {
            Debug.Log("LCARSNCI_Bridge getShipInventory begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            try
            {
                return shipLCARS.lODN.ArtefactInventory;
            }
            catch { return null; }

            //LCARS_NCI_InventoryItem_Type foo = new LCARS_NCI_InventoryItem_Type();
            //LCARS_ArtefactInventory_Type foo2 = new LCARS_ArtefactInventory_Type();
        }
        
        public LCARS_NCI_InventoryItem_Type getMissionInventory(string itemKey = null)
        {
            Debug.Log("LCARSNCI_Bridge getMissionInventory <" + itemKey + "> begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            try
            {
                //return LCARS_NCI.Instance.Data.Artefacts.list[itemKey];
                foreach (LCARS_NCI_InventoryItem_Type A in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.artefacts.list)
                {
                    if (itemKey == A.idcode)
                    {
                        return A;
                    }
                }
            }
            catch { Debug.Log("LCARSNCI_Bridge getMissionInventory <" + itemKey + "> failed 1"); return null; }

            Debug.Log("LCARSNCI_Bridge getMissionInventory <" + itemKey + "> failed 2 "); 
            return null;
        }

        public List<LCARS_NCI_InventoryItem_Type> getMissionInventory()
        {
            Debug.Log("LCARSNCI_Bridge getMissionInventory begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            try
            {
                return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.artefacts.list;
            }
            catch { return null; }

            //LCARS_NCI_InventoryItem_Type foo = new LCARS_NCI_InventoryItem_Type();
            //LCARS_ArtefactInventory_Type foo2 = new LCARS_ArtefactInventory_Type();
        }


        public List<string> get_ActionPoints_From_sfs_ActiveObject(Guid guid)
        {
            List<string> l = new List<string>();
            try
            {
                return LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[guid].actionpoints;
            }
            catch
            {
                return l;
            }
        }




        public string GetEquippment_Setting(string equippment_code, string setting_name, string artefact_id_code="")
        {
            Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <equippment_code=" + equippment_code + " setting_name=" + setting_name + " artefact_id_code=" + artefact_id_code + "> begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }
            LCARS_NCI_Equippment foo = null;
            if (equippment_code == "NucleonicAnalyzer")
            {
                if(artefact_id_code!="")
                {
                    foreach (KeyValuePair<string,LCARS_NCI_Equippment> pair in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list)
                    {
                        string key = pair.Key;
                        LCARS_NCI_Equippment E = pair.Value;
                        //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting test loop item <E.idcode=" + E.idcode + " and E.artefact1=" + E.artefact1 + " and E.textline=" + E.textline + "> ");
                        if (equippment_code + "_" + artefact_id_code == key)
                        {
                            //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <equippment_code=" + equippment_code + " and key="+key+"> ");
                            if (artefact_id_code == E.artefact1)
                            {
                                //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <artefact_id_code=" + artefact_id_code + " and E.artefact1=" + E.artefact1 + "> ");
                                    //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo found> ");
                                try
                                {
                                    foo = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list[equippment_code + "_" + artefact_id_code];
                                }
                                catch(Exception ex)
                                {
                                    //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo ERROR> ex=" + ex);
                                    //return "ERROR";
                                }
                            }
                        
                        }
                    }
                    if (foo==null)
                    {
                        return "ERROR";
                    }
                    switch (setting_name)
                    {
                        case "distance_threshhold":
                            try
                            {
                                //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo.distance_threshhold=" + foo.distance_threshhold + "> ");
                                return foo.distance_threshhold;
                            }
                            catch { return "ERROR"; }
                            break;
                        case "artefact1":
                            try
                            {
                                //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo.artefact1=" + foo.artefact1 + "> ");
                                return foo.artefact1;
                            }
                            catch { return "ERROR"; }
                            break;
                        case "artefact2":
                            try
                            {
                                //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo.artefact2=" + foo.artefact2 + "> ");
                                return foo.artefact2;
                            }
                            catch { return "ERROR"; }
                            break;
                        case "textline":
                            try
                            {
                                //Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <foo.textline=" + foo.textline + "> ");
                                return foo.textline;
                            }
                            catch { return "ERROR"; }
                            break;

                        default:
                                Debug.Log("LCARSNCI_Bridge GetEquippment_Setting <ERROR> ");
                            return "ERROR";
                            break;
                    }
                }
                return "ERROR";
            }
            else
            {
                switch (setting_name)
                {
                    case "distance_threshhold":
                        try
                        {
                            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list[equippment_code + "_" + artefact_id_code].distance_threshhold;
                        }
                        catch { return "ERROR"; }
                        break;
                    case "artefact1":
                        try
                        {
                            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list[equippment_code + "_" + artefact_id_code].artefact1;
                        }
                        catch { return "ERROR"; }
                        break;
                    case "artefact2":
                        try
                        {
                            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list[equippment_code + "_" + artefact_id_code].artefact2;
                        }
                        catch { return "ERROR"; }
                        break;
                    case "textline":
                        try
                        {
                            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.equippment.list[equippment_code + "_" + artefact_id_code].textline;
                        }
                        catch { return "ERROR"; }
                        break;

                    default:
                        return "ERROR";
                        break;
                }
            }
        }


        public LCARS_NCI_Mission_Message getMissionStepMessage(int messageID, int stepID)
        {
            Debug.Log("LCARSNCI_Bridge getMissionMessageList message=" + messageID + " step=" + stepID + "> begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            try
            {
                return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[stepID].Messages.MessageList[messageID];
            }
            catch { return null; }
        }

        public Dictionary<int, LCARS_NCI_Mission_Message> getMissionStepMessageList(int stepID)
        {
            Debug.Log("LCARSNCI_Bridge getMissionMessageList step=" + stepID + "> begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            try
            {
                return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[stepID].Messages.MessageList;
            }
            catch { return null; }

            //LCARS_NCI_InventoryItem_Type foo = new LCARS_NCI_InventoryItem_Type();
            //LCARS_ArtefactInventory_Type foo2 = new LCARS_ArtefactInventory_Type();
        }
        /*
        public void reinstateCommunicationQueue()
        {
            UnityEngine.Debug.Log("### LCARSNCI_Bridge reinstateCommunicationQueue begin ");
            if (!LCARS_NCI_BridgeReady()) { return; }
            //if (ScenarioRef == null) { ScenarioRef = getScenarioRef(); }
            setCommunicationQueueFromPersistentFile(NCIMISSION_Node_ScenarioLoad);
            UnityEngine.Debug.Log("### LCARSNCI_Bridge reinstateCommunicationQueue done ");
        }
    */
        /*
        public string getCommunicationQueueForPersistentFile()
        {
            Debug.Log("LCARSNCI_Bridge getCommunicationQueueForPersistentFile begin ");
            if (!LCARS_NCI_BridgeReady()) { return null; }

            string LCARS_Queue_data = "";
            try
            {
                Debug.Log("LCARSNCI_Bridge getCommunicationQueueForPersistentFile begin CommunicationQueue.Count=" + shipLCARS.lODN.CommunicationQueue.Count);
                foreach (LCARS_CommunicationQueue_Type qItem in shipLCARS.lODN.CommunicationQueue)
                {
                    if (qItem.Message_Object.mission_message_id == null || qItem.Message_Object.mission_message_id < 1)
                    {continue;}

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
            catch (Exception ex) { Debug.Log("LCARSNCI_Bridge getCommunicationQueueForPersistentFile failed LCARS_Queue_data=" + LCARS_Queue_data+" ex="+ex);  }
            Debug.Log("LCARSNCI_Bridge getCommunicationQueueForPersistentFile end LCARS_Queue_data=" + LCARS_Queue_data);
            return LCARS_Queue_data;
        }
        */
        public void setCommunicationQueueFromPersistentFile(string CommunicationQueue)
        {
            Debug.Log("LCARSNCI_Bridge setCommunicationQueueFromPersistentFile begin ");



            string LCARS_Queue_data = "";
            LCARS_Queue_data = CommunicationQueue;
            string[] tmp1 = LCARS_Queue_data.Split(new Char[] { '|' });
            foreach (string tmp2 in tmp1)
            {
                if (tmp2 == "" || tmp2 == null) { continue; }
                string[] tmp3 = tmp2.Split(new Char[] { ',' });


                Guid gUID = new Guid(tmp3[0]);
                int messageID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[1]);
                int from_StepID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[2]);
                int from_JobID = LCARS_NCI_Mission_Archive_Tools.ToInt(tmp3[3]);
                bool reply_sent = (tmp3[4] == "True" ?  true: false );
                string reply_sent_buttonText = tmp3[5];
                string reply_sent_replyCode = tmp3[6];
                string reply_sent_replyID = tmp3[7];
                bool panel_state = (tmp3[8] == "True" ?  true: false );
                bool Decrypted = (tmp3[9] == "True" ?  true: false );
                bool Archive = (tmp3[10] == "True" ?  true: false );

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile loop  messageID=" + messageID + " from_StepID=" + from_StepID);

                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
                {
                    return;
                }
                int currentStep = from_StepID;

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 1 ");


                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep] == null)
                {
                    return;
                }

                LCARS_NCI_Mission_Message Message = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID];

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile  1a ");

                //if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Contains(currentStep + "_" + Message.messageID))
                //{ return; }

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 2 ");


                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 3 ");

                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack = new LCARS_NCI_Mission_Archive_RunningStep_SentMessageStack();
                }
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list = new Dictionary<string,LCARS_NCI_Mission_Archive_RunningStep_MessageItem>();
                }
                /*
                LCARS_NCI_Mission_Archive_RunningStep_MessageItem SentMessageStack_Item = new LCARS_NCI_Mission_Archive_RunningStep_MessageItem();
                SentMessageStack_Item.id = gUID;
                SentMessageStack_Item.stepID = from_StepID;
                SentMessageStack_Item.messageID = Message.messageID;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list.Add(SentMessageStack_Item.id.ToString(), SentMessageStack_Item);
                */

                LCARS_Message_Type mT = new LCARS_Message_Type();
                mT.id = gUID;
                mT.receiver_vessel = getCurrentShip();
                mT.sender = Message.sender;
                mT.mission_message_id = messageID;
                mT.mission_message_stepID = currentStep;
                mT.mission_message_jobID = from_JobID;

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 4 ");

                mT.reply_options = new List<LCARS_Message_Reply>();
                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 5 ");
                foreach (LCARS_NCI_Mission_Message_Reply R in Message.reply_options)
                {
                    UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 6 ");
                    LCARS_Message_Reply tmp = new LCARS_Message_Reply();

                    UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 7 ");
                    tmp.buttonText = R.buttonText;
                    tmp.replyCode = R.replyCode;
                    tmp.replyID = R.replyID;

                    UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 8 ");
                    mT.reply_options.Add(tmp);
                    UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 9 ");
                }

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 10 ");

                mT.sender_id_line = Message.sender_id_line;
                mT.receiver_type = Message.receiver_type;
                mT.receiver_code = Message.receiver_code;
                mT.priority = Message.priority;
                mT.setTitle(Message.title);
                mT.setMessage(Message.message);

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 11 ");

                if (Message.Encrypted && !Decrypted)
                {
                    mT.Encrypt(LCARS_NCI_Mission_Archive_Tools.ToInt(Message.encryptionMode));
                    if (Message.decryption_artefact != "")
                    {
                        mT.decryption_artefact = Message.decryption_artefact;
                    }
                }
                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile 12 ");
                //mT.Queue();


                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog = new List<string>();
                }
                if (!LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Contains(currentStep + "_" + Message.messageID))
                { 
                    //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Add(currentStep + "_" + Message.messageID);
                }

                UnityEngine.Debug.Log("setCommunicationQueueFromPersistentFile done ");

                if (shipLCARS.lODN.CommunicationQueue == null)
                {
                    shipLCARS.lODN.CommunicationQueue = new System.Collections.Generic.List<LCARS_CommunicationQueue_Type>();
                }

                //List<LCARS_CommunicationQueue_Type> tmp_CommunicationQueue_Type = shipLCARS.lODN.CommunicationQueue;
                shipLCARS.lODN.CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();

                LCARS_CommunicationQueue_Type q = new LCARS_CommunicationQueue_Type();
                q.id = gUID;
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

                shipLCARS.lODN.CommunicationQueue.Add(q);
                //foreach (LCARS_CommunicationQueue_Type qT in tmp_CommunicationQueue_Type)
                //{
                //    shipLCARS.lODN.CommunicationQueue.Add(qT);
                //}


            }


            Debug.Log("LCARSNCI_Bridge setCommunicationQueueFromPersistentFile done ");
        }


        public void LCARS_Message_SendBride(int messageID, int from_StepID = 0)
        {
            if (!LCARS_NCI_BridgeReady()) { return; }


            UnityEngine.Debug.Log("LCARS_Message_SendBride  begin ");

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
            {
                return;
            }
            int currentStep = (from_StepID == 0) ? LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step : from_StepID;

            UnityEngine.Debug.Log("LCARS_NCI_Mission  send_message 1 ");

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep] == null)
            {
                return;
            }

            LCARS_NCI_Mission_Message Message = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[currentStep].Messages.MessageList[messageID];

            UnityEngine.Debug.Log("LCARS_Message_SendBride  1a ");

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Contains(currentStep + "_" + Message.messageID))
            { return; }

            UnityEngine.Debug.Log("LCARS_Message_SendBride 2 ");


            UnityEngine.Debug.Log("LCARS_Message_SendBride 3 ");

            LCARS_NCI_Mission_Archive_RunningStep_MessageItem SentMessageStack_Item = new LCARS_NCI_Mission_Archive_RunningStep_MessageItem();
            SentMessageStack_Item.id = Guid.NewGuid();
            SentMessageStack_Item.stepID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id;
            SentMessageStack_Item.messageID = Message.messageID;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list.Add(SentMessageStack_Item.id.ToString(), SentMessageStack_Item);

            LCARS_Message_Type mT = new LCARS_Message_Type();
            mT.id = SentMessageStack_Item.id;
            mT.receiver_vessel = LCARS_NCI.Instance.Data.vessel;
            mT.sender = Message.sender;
            mT.mission_message_id = messageID;
            mT.mission_message_stepID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep();
            mT.mission_message_jobID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob();

            UnityEngine.Debug.Log("LCARS_Message_SendBride 4 ");

            mT.reply_options = new List<LCARS_Message_Reply>();
            UnityEngine.Debug.Log("LCARS_Message_SendBride 5 ");
            foreach (LCARS_NCI_Mission_Message_Reply R in Message.reply_options)
            {
                UnityEngine.Debug.Log("LCARS_Message_SendBride 6 ");
                LCARS_Message_Reply tmp = new LCARS_Message_Reply();

                UnityEngine.Debug.Log("LCARS_Message_SendBride 7 ");
                tmp.buttonText = R.buttonText;
                tmp.replyCode = R.replyCode;
                tmp.replyID = R.replyID;

                UnityEngine.Debug.Log("LCARS_Message_SendBride 8 ");
                mT.reply_options.Add(tmp);
                UnityEngine.Debug.Log("LCARS_Message_SendBride 9 ");
            }

            UnityEngine.Debug.Log("LCARS_Message_SendBride 10 ");

            mT.sender_id_line = Message.sender_id_line;
            mT.receiver_type = Message.receiver_type;
            mT.receiver_code = Message.receiver_code;
            mT.priority = Message.priority;
            mT.setTitle(Message.title);
            mT.setMessage(Message.message);

            UnityEngine.Debug.Log("LCARS_Message_SendBride 11 ");

            if (Message.Encrypted)
            {
                mT.Encrypt(LCARS_NCI_Mission_Archive_Tools.ToInt(Message.encryptionMode));
                if (Message.decryption_artefact != "")
                {
                    mT.decryption_artefact = Message.decryption_artefact;
                }
            }
            UnityEngine.Debug.Log("LCARS_Message_SendBride 12 ");
            mT.Queue();
            UnityEngine.Debug.Log("LCARS_Message_SendBride done ");

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Add(currentStep + "_" + Message.messageID);
            UnityEngine.Debug.Log("LCARS_Message_SendBride done ");

            string enc_note = (Message.Encrypted) ? "n encrypted (Mode:" + Message.encryptionMode + ")" : "";
            string enc_note2 = (Message.Encrypted && Message.decryption_artefact != "") ? " - You might be able to decrypt it with the Artefact: " + Message.decryption_artefact + "" : "";
            DateTime saveNow = DateTime.Now;
            string title = (Message.Encrypted) ? StringToBinary(Message.title): Message.title;
            string message = (Message.Encrypted) ? StringToBinary(Message.message): Message.message;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                    saveNow,
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step,
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job,
                    "You received a" + enc_note + " Message from: " + Message.sender,
                    "Subject: <" + Message.sender_id_line + ">  Title: <" + title + ">  Message: <" + message + ">" + enc_note2
            );
            UnityEngine.Debug.Log("LCARS_Message_SendBride added to missionLog ");

            ScreenMessages.PostScreenMessage(
               "<color=#ff9900ff>A message was sent to your NCI Mission Vessel</color>",
               5f, ScreenMessageStyle.UPPER_CENTER
           );


        }

        private string StringToBinary(string data)
        {
            UnityEngine.Debug.Log("### LCARS_Bridge StringToBinary data=" + data);
            //return data;
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }





        public DateTime getNow()
        {
            return DateTime.Now;
        }



        public void LCARS_add_to_MissionLog(string subject, string details)
        {
            if (!LCARS_NCI_BridgeReady()) { return; }

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
            {
                return;
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog == null)
            {
                return;
            }

            LCARS_NCI_MissionProgressLog_Item I = new LCARS_NCI_MissionProgressLog_Item();
            I.timestamp = LCARSNCI_Bridge.Instance.getNow();
            I.stepID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep();
            I.jobID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob();
            I.subject = subject;
            I.details = details;
            Debug.Log("LCARSNCI_Bridge LCARS_add_to_MissionLog timestamp=" + I.timestamp + " stepID=" + I.stepID + " jobID=" + I.jobID + " subject=" + I.subject + " details=" + I.details + " ");
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList.Add(I);

        }


        public bool validate_LCARS_Scan(Guid guid, string idcode, float distance)
        {
            try
            {
                return shipLCARS.lODN.ScanReport.verify_required_Scan_distance_for_NCI(guid, idcode, distance);
            }
            catch { }
            return false;
        }


        public void LCARS_NCI_Mission_Archive_RunningStep_interfere_with_ship_systems()
        {
            if (!LCARS_NCI_BridgeReady()) { return; }

            if (LCARS_NCI.Instance.Data.vessel == FlightGlobals.ActiveVessel)
            {
                Debug.Log("LCARSNCI_Bridge LCARS_NCI_Mission_Archive_RunningStep processStep Connect to LCARS.ODN and interfere with ship systems ");
                if (LCARS_NCI.Instance.Data.vessel.LCARS() != null)
                {
                    Debug.Log("LCARSNCI_Bridge LCARS_NCI_Mission_Archive_RunningStep processStep LCARS.ODN found ");

                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.LCARS_Systems_Options == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.LCARS_Systems_Options = new List<LCARS_ShipSystems_Options>(); }

                    if (shipLCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems == null) { shipLCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems = new List<string>(); }
                    if (shipLCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems == null) { shipLCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems = new List<string>(); }
                    foreach (LCARS_ShipSystems_Options ssO in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.LCARS_Systems_Options)
                    {
                        if (ssO.disable_this)
                        {
                            if (!shipLCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Contains(ssO.system_name))
                            {
                                Debug.Log("LCARSNCI_Bridge LCARS_NCI_Mission_Archive_RunningStep ssO.disable_this " + ssO.system_name);
                                shipLCARS.lODN.ShipStatus.NCI_missionOptions_DeactivatedSystems.Add(ssO.system_name);
                                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.NCI_LCARSShipSystems_are_damaged = true;
                            }
                        }
                        if (ssO.damage_this && !LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done)
                        {
                            if (!shipLCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems.Contains(ssO.system_name))
                            {
                                Debug.Log("LCARSNCI_Bridge LCARS_NCI_Mission_Archive_RunningStep ssO.damage_this " + ssO.system_name);
                                shipLCARS.lODN.ShipStatus.NCI_missionOptions_DamagedSystems.Add(ssO.system_name);
                                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.NCI_LCARSShipSystems_are_damaged = true;
                            }
                        }
                    }
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.NCI_LCARSShipSystems_are_damaged && !LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done && LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.LCARS_Systems_NotificationText != "" && !LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.NCI_LCARSShipSystems_are_damaged_notification_sent)
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                            "Your ship is affected by external forces",
                            "" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.LCARS_Systems_NotificationText + ""
                            );
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.NCI_LCARSShipSystems_are_damaged_notification_sent = true;
                    }
                }
            }
        }


    }

}
