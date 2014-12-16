using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{


    public class LCARS_NCI_GUI
    {
        public LCARS_NCIGUI_SpaceCenter SpaceCenter { get; set; }
        public LCARS_NCIGUI_Generic Generic { get; set; }

        public void init()
        {
            if (SpaceCenter == null)
            {
                SpaceCenter = new LCARS_NCIGUI_SpaceCenter();
            }
            if (Generic == null)
            {
                Generic = new LCARS_NCIGUI_Generic();
            }
        }
    }

    //[Serializable]
    /*public class LCARS_NCI_Egg
    {
        public string name { set; get; }
        public string description { set; get; }
        public string idcode { set; get; }
    }*/

    [Serializable]
    public class LCARS_NCI_Personality
    {
        public string name { set; get; }
        public string idcode { set; get; }
        public string description { set; get; }
        public string portrait { set; get; }
    }
    
    [Serializable]
    public class LCARS_NCI_Personalities
    {
        public Dictionary<string, LCARS_NCI_Personality> list { set; get; }

        public void init()
        {
            LCARS_NCI_Personality P = new LCARS_NCI_Personality();

            P = new LCARS_NCI_Personality();
            P.name = "Station: Science";
            P.description = "LCARS-Station: Science";
            P.idcode = "science";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Station: Engineering";
            P.description = "LCARS-Station: Engineering";
            P.idcode = "engineering";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Station: Tactical";
            P.description = "LCARS-Station: Tactical";
            P.idcode = "tactical";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);


            P = new LCARS_NCI_Personality();
            P.name = "Conud'Baah";
            P.description = "He is a Kergon Trader known for eating the heart of his enemies";
            P.idcode = "conudbaah";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Krol'Laackz";
            P.description = "He is a Kerengi Trader known for owning a private moon";
            P.idcode = "krollaackz";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Kuark";
            P.description = "He is a Kerengi Trader and owns a bar";
            P.idcode = "kuark";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);


            P = new LCARS_NCI_Personality();
            P.name = "Flight Controll";
            P.description = "Galactic Spaceflight Controll";
            P.idcode = "flight_controll";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Memory Alpha";
            P.description = "The Federation Database";
            P.idcode = "federation_memory_alpha";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "KCA";
            P.description = "The Kerengi Commerce Authority";
            P.idcode = "kerengi_commerce_authority";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "High Command";
            P.description = "The VulKan Government";
            P.idcode = "vulkan_high_command";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "High Council";
            P.description = "The Kergon Government";
            P.idcode = "kergon_high_council";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Dominion Headquarters";
            P.description = "The Dominion Government";
            P.idcode = "dominion_headquarters";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Star Fleet Command";
            P.description = "The Federation Government";
            P.idcode = "federation_starfleet_command";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Talshiar";
            P.description = "The Romulan Intelligence";
            P.idcode = "romulan_talshiar";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Obsidian Order";
            P.description = "The Kardassian Intelligence";
            P.idcode = "kardassian_obsidian_order";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Starfleet Security";
            P.description = "The Federation official Inteligence Service";
            P.idcode = "federation_starfleet_security";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

            P = new LCARS_NCI_Personality();
            P.name = "Section 31";
            P.description = "The Federation unofficial Inteligence Service";
            P.idcode = "federation_section_31";
            P.portrait = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_" + P.idcode;
            addPersonality(P);

        }

        public void addPersonality(LCARS_NCI_Personality p)
        {
            if (list==null)
            {
                list = new Dictionary<string, LCARS_NCI_Personality>();
            }
            list.Add(p.idcode, p);
       }

        public LCARS_NCI_Personalities load()
        {
            try
            {
                string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets";
                string loadfileName = "Personalities.Personalities_cfg";
                string CFG_path = path + "/" + loadfileName;
                ConfigNode root = ConfigNode.Load(CFG_path);

                LCARS_NCI_Personalities_Config_Loader cfgLoader = new LCARS_NCI_Personalities_Config_Loader();
                return cfgLoader.deconstruct_personalities_config(this, root);
            }
            catch { return null; }
        }
        public bool save()
        {
            LCARS_NCI_Personalities_Config_Writer cfgWriter = new LCARS_NCI_Personalities_Config_Writer();
            ConfigNode config = new ConfigNode("root");
            ConfigNode Personalities = cfgWriter.construct_personalities_config(this);
            config.AddNode(Personalities);

            string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets";
            string loadfileName = "Personalities.Personalities_cfg";
            string missionCFG_path = path + "/" + loadfileName;
            if (!config.Save(missionCFG_path))
            {
                return false;
            }
            return true;
        }
    }

    //[Serializable]
    public class LCARS_NCI_Equippment
    {
        public string idcode { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public string icon { set; get; }

        public string resource { set; get; } // BC
        public string distance_threshhold { set; get; } // BC, IFD
        public string part { set; get; } // BC, IFD
        public string egg { set; get; } // BC, IFD
        public string artefact1 { set; get; }  // NA,PS
        public string textline { set; get; }  // NA
        public string artefact2 { set; get; } // PS

        public LCARS_NCI_BussardCollectors BussardCollector { set; get; } // gathers a defined resource while in range
        public LCARS_NCI_IsoFluxDetector IsoFluxDetector { set; get; } // finds a defined gameObject by messuring the distance to vessel
        public LCARS_NCI_NucleonicAnalyzer NucleonicAnalyzer { set; get; } // can be used to analyze artefact, will return a defined result 
        public LCARS_NCI_PteroplasticScrambler PteroplasticScrambler { set; get; } // can be used to transform artefacts, feed one, receive an other


    }
    
    //[Serializable]
    public class LCARS_NCI_Equippment_List
    {

        public Dictionary<string, LCARS_NCI_Equippment> list { set; get; }

        public void init()
        {
            LCARS_NCI_Equippment P;
            P = new LCARS_NCI_Equippment();
            P.name = "Bussard Collector";
            P.description = "gathers a defined resource while in range";
            P.idcode = "BussardCollector";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIEquippment_" + P.idcode;
            //P.BussardCollector = new LCARS_NCI_BussardCollectors();
            addEquippment(P);

            P = new LCARS_NCI_Equippment();
            P.name = "Iso Flux Detector";
            P.description = "finds a defined gameObject by messuring the distance to vessel";
            P.idcode = "IsoFluxDetector";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIEquippment_" + P.idcode;
            //P.IsoFluxDetector = new LCARS_NCI_IsoFluxDetector();
            addEquippment(P);

            P = new LCARS_NCI_Equippment();
            P.name = "Nucleonic Analyzer";
            P.description = "can be used to analyze artefact, will return a defined result";
            P.idcode = "NucleonicAnalyzer";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIEquippment_" + P.idcode;
            //P.NucleonicAnalyzer = new LCARS_NCI_NucleonicAnalyzer();
            addEquippment(P);

            P = new LCARS_NCI_Equippment();
            P.name = "Pteroplastic Scrambler";
            P.description = "can be used to transform artefacts, feed one, receive an other";
            P.idcode = "PteroplasticScrambler";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIEquippment_" + P.idcode;
            //P.PteroplasticScrambler = new LCARS_NCI_PteroplasticScrambler();
            addEquippment(P);

        }

        public void addEquippment(LCARS_NCI_Equippment p)
        {
            if (list == null)
            {
                list = new Dictionary<string, LCARS_NCI_Equippment>();
            }
            list.Add(p.idcode, p);
        }
    }

    [Serializable]
    public class LCARS_NCI_InventoryItem_Type
    {
        public string idcode { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public string icon { set; get; }
        public string isDamagable { set; get; }
        public string integrity { set; get; }
        public string usage_amount { set; get; }
        public string usage_times { set; get; }
        public string powerconsumption { set; get; }
    }
 
    [Serializable]
    public class LCARS_NCI_Artefacts
    {
        public Dictionary<string, LCARS_NCI_InventoryItem_Type> list { set; get; }

        public void init()
        {
            LCARS_NCI_InventoryItem_Type P;
            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Micro Filament";
            P.description = "Every good Star Fleet Officer can use this like McGiver a Swiss Army Knife.";
            P.idcode = "MicroFilament";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "0";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Hyper Spanner";
            P.description = "Adaptable multipurpose engineering tool carried aboard starships.";
            P.idcode = "HyperSpanner";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Security Rod Level 1";
            P.description = "Used to break low encryption messages";
            P.idcode = "SecurityRodLevel1";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Security Rod Level 2";
            P.description = "Used to break medium encryption messages";
            P.idcode = "SecurityRodLevel2";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Security Rod Level 3";
            P.description = "Used to break high encryption messages";
            P.idcode = "SecurityRodLevel3";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Optolythic Data Rod";
            P.description = "Used to store confidential data";
            P.idcode = "OptolythicDataRod01";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "IsoLinear Controller Alpha";
            P.description = "This Chip grants access to certain Equippment or Information";
            P.idcode = "IsoLinearControllerAlpha";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "IsoLinear Controller Beta";
            P.description = "This Chip grants access to certain Equippment or Information";
            P.idcode = "IsoLinearControllerBeta";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "IsoLinear Controller Gamma";
            P.description = "This Chip grants access to certain Equippment or Information";
            P.idcode = "IsoLinearControllerGamma";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "MAAC: Clearance 1";
            P.description = "Memory Alpha Access Code with Clearance Level 1. Used to access restricted informations";
            P.idcode = "MemoryAlphaAccessCode1";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "MAAC: Clearance 2";
            P.description = "Memory Alpha Access Code with Clearance Level 2. Used to access very restricted informations";
            P.idcode = "MemoryAlphaAccessCode2";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "MAAC: Clearance 3";
            P.description = "Memory Alpha Access Code with Clearance Level 3. Used to access extreemly restricted informations";
            P.idcode = "MemoryAlphaAccessCode3";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Alien Code Peace 'Ousul'";
            P.description = "Alien Code that contains superior decryption algorhytms";
            P.idcode = "AlienCodeOusul";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Alien Code Peace 'Moadib'";
            P.description = "Alien Code that contains superior decryption algorhytms";
            P.idcode = "AlienCodeMoadib";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Alien Code Peace 'Harkonen'";
            P.description = "Alien Code that contains superior decryption algorhytms";
            P.idcode = "AlienCodeHarkonen";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

            P = new LCARS_NCI_InventoryItem_Type();
            P.name = "Pteroplast";
            P.description = "Alien artefact of unknow origin";
            P.idcode = "AlienPteroplast";
            P.icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIArtefact_" + P.idcode;
            P.isDamagable = "False";
            P.integrity = "";
            P.usage_amount = "-1";
            P.usage_times = "0";
            P.powerconsumption = "10";
            addArtefact(P);

        }
        public void addArtefact(LCARS_NCI_InventoryItem_Type p)
        {
            if (list==null)
            {
                list = new Dictionary<string, LCARS_NCI_InventoryItem_Type>();
            }
            list.Add(p.idcode, p);
        }
        public LCARS_NCI_Artefacts load()
        {
            try
            {
                string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets";
                string loadfileName = "Artefacts.Artefacts_cfg";
                string CFG_path = path + "/" + loadfileName;
                ConfigNode root = ConfigNode.Load(CFG_path);

                LCARS_NCI_Artefacts_Config_Loader cfgLoader = new LCARS_NCI_Artefacts_Config_Loader();
                return cfgLoader.deconstruct_artefacts_config(this,root);
            }
            catch { return null; }
        }
        public bool save()
        {
            LCARS_NCI_Artefacts_Config_Writer cfgWriter = new LCARS_NCI_Artefacts_Config_Writer();
            ConfigNode config = new ConfigNode("root");
            ConfigNode Artefacts = cfgWriter.construct_artefacts_config(this);
            config.AddNode(Artefacts);

            string path = KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets";
            string loadfileName = "Artefacts.Artefacts_cfg";
            string missionCFG_path = path + "/" + loadfileName;
            if (!config.Save(missionCFG_path))
            {
                return false;
            }
            return true;
        }
    }

    public class LCARS_NCI_Object
    {
        public string group { set; get; }
        public string partmodulename { set; get; }
        public string partname { set; get; }
        public string creator { set; get; }
        public string url { set; get; }
        public string title { set; get; }
        public string description { set; get; }
        public string icon { set; get; }
        public Texture2D icon_tex { set; get; }
        public List<string> actionpoints { set; get; }
        public List<string> doors { get; set; }
        //public List<string> GUIWindows { get; set; }
        public bool isInstalled { set; get; }
        public Dictionary<string,LCARS_NCI_Object_assigned_missions> missions { set; get; }
        public LinkedList<string> missionStack { set; get; }
        public void assignMission(string part_idcode,LCARS_NCI_Mission mission_to_assign)
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission begin ");
            if (!this.missions.ContainsKey(mission_to_assign.filename))
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission requirements.list=>assigned_sfs_object_Guid=" + mission_to_assign.requirements.list[mission_to_assign.steps[1].requirementPart_id].assigned_sfs_object_Guid);
                LCARS_NCI_Object_assigned_missions nAM = new LCARS_NCI_Object_assigned_missions();
                nAM.mission_id = mission_to_assign.filename;
                nAM.condition_type = mission_to_assign.steps[1].locationtype;
                nAM.condition_egg = mission_to_assign.steps[1].location_egg;
                nAM.condition_part = mission_to_assign.steps[1].location_part;
                nAM.condition_PQSCity = mission_to_assign.steps[1].location_PQSCity;
                nAM.condition_body = mission_to_assign.steps[1].location_body;
                //nAM.assigned_sfs_object_Guid = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[mission_to_assign.steps[1].requirementPart_id].assigned_sfs_object_Guid;
                nAM.condition_distance = mission_to_assign.steps[1].location_distance;
                nAM.mission = mission_to_assign;
                if ((part_idcode == this.partname && nAM.condition_part == part_idcode) || (part_idcode == this.bodyname && nAM.condition_body == part_idcode))
                {
                    this.missions.Add(nAM.mission_id, nAM);
                    UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission nAM.mission_id=" + nAM.mission_id + "  nAM.condition_part=" + nAM.condition_part + "  nAM.assigned_sfs_object_Guid=" + nAM.assigned_sfs_object_Guid);
                }
            }
            else
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission skipped mission_to_assign.filename=" + mission_to_assign.filename);
                UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission mission_to_assign.requirements.list=>assigned_sfs_object_Guid=" + mission_to_assign.requirements.list[mission_to_assign.steps[1].requirementPart_id].assigned_sfs_object_Guid);
                this.missions[mission_to_assign.filename].assigned_sfs_object_Guid = mission_to_assign.requirements.list[mission_to_assign.steps[1].requirementPart_id].assigned_sfs_object_Guid;
                UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission mission_id=" + this.missions[mission_to_assign.filename].mission_id + "  condition_part=" + this.missions[mission_to_assign.filename].condition_part + "  assigned_sfs_object_Guid=" + this.missions[mission_to_assign.filename].assigned_sfs_object_Guid);
            }
            UnityEngine.Debug.Log("### LCARS_NCI_Object assignMission end ");
        }

        public bool CheckEggMode(string eggMode)
        {
            UnityEngine.Debug.Log("### NCI CheckEggMode eggMode=" + eggMode);
            return this.actionpoints.Contains(eggMode);
        }
        public bool CheckPartAvailability()
        {
            UnityEngine.Debug.Log("### NCI CheckPartAvailability this.partname=" + this.partname);
            return (PartLoader.LoadedPartsList.Find(part => part.name == this.partname) == null) ? false : true;
        }


        public string bodyname { get; set; }


        public Dictionary<string, List<string>> Locations { get; set; }


        public bool isCelestialBody { get; set; }
    }

    public class LCARS_NCI_Object_assigned_missions
    {
        public string condition_type { set; get; }
        public string condition_distance { set; get; }
        public LCARS_NCI_Mission mission { set; get; }

        public string condition_body { get; set; }
        public string condition_PQSCity { get; set; }
        public string condition_part { get; set; }
        public string condition_egg { get; set; }

        public Guid assigned_sfs_object_Guid { set; get; }

        public bool MissionObjectsAssignedToSFSObjects { set; get; }


        /*
        public bool checkCondition(LCARS_NCI_Object_assigned_missions this_aM)
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition");
            Vector3 messure_point = Vector3.zero;

            string this_condition_type = this_aM.condition_type;
            string this_condition_distance = this_aM.condition_distance;
            string this_condition_part = this_aM.condition_part;
            string this_condition_egg = this_aM.condition_egg;
            string this_condition_body = this_aM.condition_body;
            string this_condition_PQSCity = this_aM.condition_PQSCity;
            Guid this_assigned_sfs_object_Guid = this_aM.assigned_sfs_object_Guid;

            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition alpha " + this_condition_type + " than " + this_condition_distance + " related to " + this_condition_part + " " + this_condition_body + " - " + this_condition_egg + " todo");
            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition beta this_aM.condition_part=" + this_aM.condition_part + " this_aM.condition_egg=" + this_aM.condition_egg + " this_aM.assigned_sfs_object_Guid=" + this_aM.assigned_sfs_object_Guid);


            switch (this_condition_type)
            {

                case "distanceLower":
                case "distanceGreater":
                    UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition " + this_condition_type);
                    messure_point = Vector3.zero;
                    
                    bool isCelestialBody = false;
                    try
                    {
                        isCelestialBody = (this_condition_body!="") ? true: false;
                    }
                    catch{}
                    UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition isCelestialBody=" + isCelestialBody);
                    if (isCelestialBody)
                    {
                        CelestialBody StepBody = null;
                        foreach (CelestialBody CB in FlightGlobals.Bodies)
                        {
                            if (CB.name == this_condition_body)
                            {
                                StepBody = CB;
                            }
                        }
                        if (StepBody == null)
                        {
                            return false;
                        }
                        UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition StepBody.name=" + StepBody.name);

                        NCIScanGOExtensions.FilterChildGameObjects(StepBody.gameObject, this_condition_PQSCity);

                        Transform tmp = null;
                        if (this_condition_egg != "")
                        {
                            foreach (KeyValuePair<string, GameObject> pair in NCIScanGOExtensions.ActionPointsGameObjects)
                            {
                                string name = pair.Value.name;
                                UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition ActionPointsGameObjects name=" + name + " this_condition_egg=" + this_condition_egg);
                                if (name == this_condition_egg)
                                {
                                    UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition ActionPointsGameObjects found egg");
                                    GameObject go = pair.Value;
                                    tmp = go.transform;
                                }
                            }
                            //tmp = v.rootPart.FindModelTransform(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_egg);
                        }
                        if (tmp != null)
                        {
                            messure_point = tmp.position;
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition was found: " + this_condition_egg + " at " + this_condition_PQSCity + " on " + this_condition_body);
                        }
                        else
                        {
                            messure_point = StepBody.transform.position;
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition was found: " + this_condition_body);
                        }

                    }
                    else
                    {
                        foreach (Vessel v in FlightGlobals.Vessels)
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition messure_point=" + messure_point.ToString());
                            if (messure_point != Vector3.zero)
                            {continue;}
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower 1 => this_assigned_sfs_object_Guid=" + this_assigned_sfs_object_Guid + " v.id=" + v.id);
                            if (this_assigned_sfs_object_Guid == v.id)
                            {
                                UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower 2 => this_assigned_sfs_object_Guid=" + this_assigned_sfs_object_Guid + " v.id=" + v.id);
                                Transform tmp = null;
                                if (this_condition_egg != "")
                                {
                                    tmp = v.rootPart.FindModelTransform(this_condition_egg);
                                }
                                if (tmp != null)
                                {
                                    messure_point = tmp.transform.position;
                                    UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower was found: " + this_condition_egg + " at " + this_condition_part);
                                }
                                else
                                {
                                    UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower was found: this_condition_part=" + this_condition_part);
                                    messure_point = v.transform.position;
                                }
                            }
                        }
                    }
                    if (messure_point == Vector3.zero) { UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition messure_point == Vector3.zero skipping"); return false; }


                    if (this_condition_type == "distanceLower")
                    {
                        if (LCARS_NCI_Mission_Archive_Tools.isDistanceLower(messure_point, LCARS_NCI_Mission_Archive_Tools.ToFloat(this_condition_distance)))
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower was met: this_condition_distance=" + this_condition_distance);
                            return true;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceLower was NOT met: this_condition_distance=" + this_condition_distance);
                        }
                    }
                    if (this_condition_type == "distanceGreater")
                    {
                        if (LCARS_NCI_Mission_Archive_Tools.isDistanceGreater(messure_point, LCARS_NCI_Mission_Archive_Tools.ToFloat(this_condition_distance)))
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceGreater was met: this_condition_distance=" + this_condition_distance);
                            return true;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Object_assigned_missions checkCondition distanceGreater was NOT met: this_condition_distance=" + this_condition_distance);
                        }
                    }
                    break;

            }



            return false;
        }
        */

        public string mission_id { get; set; }
    }
    








}
