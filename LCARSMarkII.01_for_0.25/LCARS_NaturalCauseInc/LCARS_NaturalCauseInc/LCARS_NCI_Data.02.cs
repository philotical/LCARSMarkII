using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public delegate void GameObjectVisitor(GameObject go, int indent);

    public static class DebugExtensions
    {
        private static void internal_PrintComponents(GameObject go, int indent)
        {
            Debug.Log((indent > 0 ? new string('-', indent) + ">" : "")+" "+go.name+" has components:");

            var components = go.GetComponents<Component>();
            foreach (var c in components)
                Debug.Log(new string('.', indent + 3) + "c"+ ": "+c.GetType().FullName);
        }

        public static void PrintComponents(this UnityEngine.GameObject go)
        {
            go.TraverseHierarchy(internal_PrintComponents);
        }

        public static void TraverseHierarchy(this UnityEngine.GameObject go, GameObjectVisitor visitor, int indent = 0)
        {
            visitor(go, indent);

            for (int i = 0; i < go.transform.childCount; ++i)
                go.transform.GetChild(i).gameObject.TraverseHierarchy(visitor, indent + 3);
        }
    }

    public class LCARS_NCI_Data
    {
        public bool sfs_ActiveObjectsList_IsUpdated { get; set; }
        public bool ScanForMissions_IsUpdated { get; set; }

        /*
         the player
         */
        public Vessel vessel { get; set; }
        public string kerbal { get; set; }

        public NCIConsole NCIConsole { get; set; }

        /*
         the assets
         */
        public Dictionary<string, LCARS_NCI_Object> Naturals_List { get; set; }
        public Dictionary<string, LCARS_NCI_Object> Mines_List { get; set; }
        public Dictionary<string, LCARS_NCI_Object> Stations_List { get; set; }
        //public Dictionary<string, LCARS_NCI_Egg> Egg_List { get; set; }
        public Dictionary<string, LCARS_NCI_Object> ObjectArchive { get; set; }
        public Dictionary<string, LCARS_NCI_Equippment> Equippment_List { get; set; }
        public Dictionary<string, LCARS_NCI_InventoryItem_Type> Artefact_List { get; set; }
        public Dictionary<string, LCARS_NCI_Personality> Personality_List { get; set; }


        //public LCARS_NCI NCI;

        /*
         Gather_Object_Lists Stuff
         */
        private bool ObjectListDone = false;
        private LCARS_NCI_Object NCIObj = null;
        private string default_icon = "LCARS_NaturalCauseInc/Icons/NoPreview";


        /*
         the universe
         */
        public Dictionary<Guid, LCARS_NCI_Object> sfs_ActiveObjectsList { get; set; }


        /*
         the storys
         */
        public LCARS_NCI_Mission_Archive MissionArchive { get; set; }


        /*
         the settings
         */

        //LCARSMarkII LCARS;
        public void SetVessel(Vessel v)
        {
            vessel = v;
            //LCARS = vessel.LCARS();
        }
        public void OnSave()
        {
            SavePersonalities();
            //SaveEggs();
            SaveArtefacts();
        }
        public void OnLoad()
        {
            sfs_ActiveObjectsList_IsUpdated = false;
            //LoadPersonalities();
            //LoadEggs();
            //LoadArtefacts();
        }

        public LCARS_NCI_Personalities Personalities;
        //public LCARS_NCI_Eggs eggs;
        public LCARS_NCI_Equippment_List equippments;
        public LCARS_NCI_Artefacts Artefacts;
        public void init()
        {
            /*if (NCI == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init NCI");
                NCI = LCARS_NCI.Instance;
                NCI.init();
            }*/

            if (Personality_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Personality_List");
                try
                {
                    Personalities = new LCARS_NCI_Personalities();
                    Personalities = Personalities.load();
                }
                catch { }
                if (Personalities == null)
                {
                    Personalities = new LCARS_NCI_Personalities();
                    Personalities.init();
                }
                Personality_List = Personalities.list;

            }
            if (Artefact_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Artefact_List");
                
                try
                {
                    Artefacts = new LCARS_NCI_Artefacts();
                    Artefacts = Artefacts.load();
                }
                catch { }
                if (Artefacts == null)
                {
                    Artefacts = new LCARS_NCI_Artefacts();
                    Artefacts.init();
                }
                Artefact_List = Artefacts.list;

            }
            //equippments
            if (Equippment_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Equippment_List");
                equippments = new LCARS_NCI_Equippment_List();
                    equippments.init();
                /*if (!LoadEquippment())
                {
                }*/
                Equippment_List = equippments.list;
            }
            /*
            if (Egg_List == null)
            {
                Debug.Log("NCIGD pragmatic_init Egg_List");
                eggs = new LCARS_NCI_Eggs();
                if (!LoadEggs())
                {
                    eggs.init();
                }
                Egg_List = eggs.list;
            }
            */
            if (Naturals_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Naturals_List");
                Naturals_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (Mines_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Mines_List");
                Mines_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (Stations_List == null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init Stations_List");
                Stations_List = new Dictionary<string, LCARS_NCI_Object>();
            }

            if (ObjectArchive==null)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init ObjectArchive");
                ObjectArchive = new Dictionary<string, LCARS_NCI_Object>();
                Gather_Object_Lists();
            }

            if (!sfs_ActiveObjectsList_IsUpdated && FlightGlobals.ready)
            {
                Debug.Log("LCARS_NCI_Data pragmatic_init sfs_ActiveObjectsList");
                Update_sfs_ActiveObjectsList();

                if (!ScanForMissions_IsUpdated)
                {
                    Debug.Log("LCARS_NCI_Data pragmatic_init MissionArchive");
                    //LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
                    ScanForMissions();
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.all_Missions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.all_Missions.Count);
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.all_UserMissions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.all_UserMissions.Count);
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.all_backgroundMissions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.all_backgroundMissions.Count);
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.playable_UserMissions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions.Count);
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.playable_backgroundMissions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions.Count);
                    Debug.Log("LCARS_NCI_Data pragmatic_object_initializer MissionArchive.MissingPartMissions.Count=" + LCARS_NCI.Instance.Data.MissionArchive.MissingPartMissions.Count);
                }
            }

        }

        public void Gather_Object_Lists()
        {
            if (ObjectListDone)
            { return; }
            Debug.Log("NCIGD pragmatic_init Gather_Object_Lists");

            UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists running ");
            //originalLoadDistance = Vessel.loadDistance;
            //originalUnLoadDistance = Vessel.unloadDistance;

            foreach (UrlDir.UrlConfig mod in GameDatabase.Instance.root.AllConfigs)
            {
                //UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists mod.name=" + mod.name);
                if (mod.name == "NATURALCAUSEINC")
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists CFG found ");
                    foreach (ConfigNode c in mod.config.GetNodes("NCIOBJ"))
                    {
                        string partname = c.GetValue("partname");
                        UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists NCIOBJ cfg found for designated partname=" + partname);

                        NCIObj = new LCARS_NCI_Object();
                        NCIObj.group = c.GetValue("group");
                        NCIObj.partname = partname;
                        NCIObj.partmodulename = c.GetValue("partmodulename");
                        NCIObj.creator = c.GetValue("creator");
                        NCIObj.url = c.GetValue("url");
                        NCIObj.title = c.GetValue("title");
                        NCIObj.description = c.GetValue("description");
                        NCIObj.icon = c.GetValue("icon");
                        NCIObj.isInstalled = NCIObj.CheckPartAvailability();

                        NCIObj.actionpoints = new List<string>();
                        foreach (string s in c.GetValue("actionpoints").Split(new Char[] { ',' }))
                        {
                            if (s == "") { continue; }
                            NCIObj.actionpoints.Add(s);
                        }

                        NCIObj.doors = new List<string>();
                        foreach (string s in c.GetValue("doors").Split(new Char[] { ',' }))
                        {
                            if (s == "") { continue; }
                            NCIObj.doors.Add(s);
                        }

                        NCIObj.GUIWindows = new List<string>();
                        foreach (string s in c.GetValue("GUIWindows").Split(new Char[] { ',' }))
                        {
                            if (s=="") { continue; }
                            NCIObj.GUIWindows.Add(s);
                        }

                        if (NCIObj.isInstalled)
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists NCIOBJ designated part is installed partname=" + partname);
                            try
                            {
                                NCIObj.icon_tex = GameDatabase.Instance.GetTexture(NCIObj.icon, false);
                                UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists loaded icon_tex NCIObj.icon=" + NCIObj.icon);
                                //UnityEngine.Debug.Log("### NCI Gather_Object_Lists   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                            }
                            catch (Exception ex)
                            {
                                UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists skipping icon_tex NCIObj.icon=" + NCIObj.icon + " - file not found ex=" + ex);
                                NCIObj.icon_tex = GameDatabase.Instance.GetTexture(default_icon, false);
                                UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists loaded icon_tex default_icon=" + default_icon);
                                //UnityEngine.Debug.Log("### NCI Gather_Object_Lists   NCIObj.icon_tex.name=" + NCIObj.icon_tex.name);
                            }
                            UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists NCIOBJ adding to group=" + NCIObj.group);

                            LCARS_NCI.Instance.Data.ObjectArchive.Add(NCIObj.partname, NCIObj);


                            switch (NCIObj.group)
                            {
                                case "Naturals":
                                    LCARS_NCI.Instance.Data.Naturals_List.Add(NCIObj.partname, NCIObj);
                                    break;
                                case "Mines":
                                    LCARS_NCI.Instance.Data.Mines_List.Add(NCIObj.partname, NCIObj);
                                    break;
                                case "Stations":
                                    LCARS_NCI.Instance.Data.Stations_List.Add(NCIObj.partname, NCIObj);
                                    break;
                                default:
                                    UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists NCIObj.group was not recognized group=" + NCIObj.group);
                                    break;
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.Log("### NCI Gather_Object_Lists NCIOBJ designated part is not installed partname=" + partname);
                        }
                        NCIObj = null;
                    }
                }
            }

            ObjectListDone = true;
            UnityEngine.Debug.Log("### LCARS_NCI_Data Gather_Object_Lists done");
        }

        public void Update_sfs_ActiveObjectsList()
        {
            Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList");
            sfs_ActiveObjectsList = null;
            sfs_ActiveObjectsList = new Dictionary<Guid, LCARS_NCI_Object>();
            foreach (Vessel v in FlightGlobals.Vessels)
            {
                Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList Load ");
                v.Load();

                DebugExtensions.PrintComponents(v.gameObject);
                /*
                Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.vesselName=" + v.vesselName);
                try
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.rootPart.partName=" + v.rootPart.partName);
                }
                catch (Exception ex) { }
                try
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.rootPart.name=" + v.rootPart.name);
                }catch (Exception ex) { }
                try
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.rootPart.vessel.rootPart.name=" + v.rootPart.vessel.rootPart.name);
                }
                catch (Exception ex) { }
                try
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.parts[0].name=" + v.parts[0].name);
                }
                catch (Exception ex) { }
                try
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.parts[0].partName=" + v.parts[0].partName);
                }
                catch (Exception ex) { }
                */



                /*
                 This needs to be looking for a part name, but the partname is empty on all NCI spawned objects..
                 * I need to recheck the spawn code and fix that.
                 */

                string[] tmp = v.rootPart.name.Split(' ');
                string pName = tmp[0];
                if (LCARS_NCI.Instance.Data.ObjectArchive.ContainsKey(pName))
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList found pName=" + pName);
                    if (!LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.ContainsKey(v.id))
                    {
                        LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Add(v.id, LCARS_NCI.Instance.Data.ObjectArchive[pName]);
                        Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList added ");
                    }
                    else 
                    {
                        Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList allready present ");
                    }
                }
                
                /*foreach (Part vesselPart in v.parts)
                {
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList loop start ");
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList vesselPart.name=" + vesselPart.name);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList vesselPart.partName=" + vesselPart.partName);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList vesselPart.partInfo.name=" + vesselPart.partInfo.name);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList vesselPart.partInfo.partPrefab.partName=" + vesselPart.partInfo.partPrefab.partName);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList vesselPart.partInfo.partPrefab.name=" + vesselPart.partInfo.partPrefab.name);* /
                    string rootpartname = vesselPart.partName;
                    /*
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.rootPart.name=" + v.rootPart.name);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList v.rootPart.partName=" + v.rootPart.partName);
                    Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList loop end ");
                }
                    */

                Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList GoOnRails ");
                v.GoOnRails();

            }
            Debug.Log("LCARS_NCI_Data Update_sfs_ActiveObjectsList done ");
            foreach (KeyValuePair<Guid,LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
            {
                bool cont = false;
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if(v.id==pair.Key)
                    {
                        cont = true ;
                    }
                }
                if (cont)
                {
                    continue;
                }
                LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Remove(pair.Key);
            }
            sfs_ActiveObjectsList_IsUpdated = true;
        }


        private void ScanForMissions()
        {
            Debug.Log("LCARS_NCI_Data ScanForMissions begin");
            string[] filePaths = Directory.GetFiles(@"" + KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions/", "*.mission_cfg");
            foreach (string s in filePaths)
            {
                string fileName;
                fileName = s.Replace(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIMissions/", "");
                fileName = fileName.Replace(".mission_cfg", "");
                Debug.Log("LCARS_NCI_Data ScanForMissions fileName=" + fileName);

                LCARS_NCI_Mission tmp = new LCARS_NCI_Mission();
                LCARS_NCI_Mission m = new LCARS_NCI_Mission();
                m = tmp.load(fileName);

                if (LCARS_NCI.Instance.Data.MissionArchive == null)
                {
                    LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
                }
                LCARS_NCI.Instance.Data.MissionArchive.addMission(m);
                //Debug.Log("LCARS_NCI_Data ScanForMissions mission add done ");
            }
            ScanForMissions_IsUpdated = true;
            Debug.Log("LCARS_NCI_Data ScanForMissions done ");
        }






        void SavePersonalities()
        {
            UnityEngine.Debug.Log("### NCI_Data SavePersonalities");

            Personalities.save();

        }
        /*
        bool LoadPersonalities()
        {
            UnityEngine.Debug.Log("### NCI_Data LoadPersonalities");
            return (Personalities.load()!=null)? true: false;
        }
        */

        bool LoadEquippment()
        {
            UnityEngine.Debug.Log("### NCI_Data LoadEggs");
            //If not blank then load it
            if (File.Exists(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets/Eggs.dat"))
            {
                //Binary formatter for loading back
                var b = new BinaryFormatter();
                //Get the file
                //var f = File.Open(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/NCI/Personalities.dat", FileMode.Open);
                var f = File.Open(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets/Equippment.dat", FileMode.Open);
                //Load back the scores
                equippments = (LCARS_NCI_Equippment_List)b.Deserialize(f);
                f.Close();
                return true;
            }
            return false;
        }
        /*
        void SaveEggs()
        {
            UnityEngine.Debug.Log("### NCI_Data SaveEggs");
            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets");
            //Get a binary formatter
            var b = new BinaryFormatter();
            //Create a file
            var f = File.Create(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets/Eggs.dat");
            //Save the scores
            b.Serialize(f, eggs);
            f.Close();
        }

        bool LoadEggs()
        {
            UnityEngine.Debug.Log("### NCI_Data LoadEggs");
            //If not blank then load it
            if (File.Exists(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets/Eggs.dat"))
            {
                //Binary formatter for loading back
                var b = new BinaryFormatter();
                //Get the file
                //var f = File.Open(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/NCI/Personalities.dat", FileMode.Open);
                var f = File.Open(KSPUtil.ApplicationRootPath + "GameData/LCARS_NaturalCauseInc/NCIAssets/Eggs.dat", FileMode.Open);
                //Load back the scores
                eggs = (LCARS_NCI_Eggs)b.Deserialize(f);
                f.Close();
                return true;
            }
            return false;
        }
*/
        void SaveArtefacts()
        {
            UnityEngine.Debug.Log("### NCI_Data SaveArtefacts");

            Artefacts.save();


        }
        /*
        bool LoadArtefacts()
        {
            UnityEngine.Debug.Log("### NCI_Data LoadArtefacts");

            //Artefacts.load();
            return (Artefacts.load() != null) ? true : false;
        }
        */


    }





}
