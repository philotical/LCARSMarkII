using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace LCARSMarkII
{
    public static class LCARS_NCI_Mission_Archive_Tools
    {
        public static int ToInt(string s)
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools ToInt");
            int number;
            bool result = Int32.TryParse(s, out number);
            if (result)
            {
                //Console.WriteLine("Converted '{0}' to {1}.", s, number);
                return number;
            }
            else
            {
                //if (s == null) s = "";
                //Console.WriteLine("Attempted conversion of '{0}' failed.", s);
                return 0;
            }
        }
        public static float ToFloat(string s)
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools ToFloat");
            float number;
            //bool result = float.TryParse(s, out number);
            bool result = float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            if (result)
            {
                //Console.WriteLine("Converted '{0}' to {1}.", s, number);
                return number;
            }
            else
            {
                //if (s == null) s = "";
                //Console.WriteLine("Attempted conversion of '{0}' failed.", s);
                return 0;
            }
        }
        public static Vector3 getPlayerPosition()
        {
            Vessel player = FlightGlobals.ActiveVessel;
            return player.transform.position;
        }

        public static bool isReady()
        {
            if (!HighLogic.LoadedSceneIsFlight)
            {
                return false;
            }
            if (FlightGlobals.ActiveVessel == null)
            {
                return false;
            }
            if (LCARS_NCI.Instance.Data.vessel == null)
            {
                LCARS_NCI.Instance.Data.vessel = FlightGlobals.ActiveVessel;
            }
            if (LCARS_NCI.Instance.Data.vessel.vesselType == VesselType.EVA)
            {
                LCARS_NCI.Instance.Data.kerbal = FlightGlobals.ActiveVessel.vesselName;
            }
            if (LCARS_NCI.Instance.Data.vessel == null)
            {
                //LCARS_NCI.Instance.Data.vessel;
                //LCARS_NCI.Instance.Data.kerbal;
                return false;
            }



            return true;
        }

        public static string TaskStringConstructor_Steps(LCARS_NCI_Mission_Archive_RunningMission m)
        {
            string task = "";
            /*public string locationtype { set; get; } //distanceLower, distanceGreater
            public string location_part { set; get; }
            public string location_egg { set; get; }
            public string location_distance { set; get; }*/
            if (m.RunningStep.missionStep.locationtype == "distanceLower" || m.RunningStep.missionStep.locationtype == "distanceGreater")
            {
                if (m.RunningStep.missionStep.locationtype == "distanceLower")
                {
                    task = "get closer than " + m.RunningStep.missionStep.location_distance + " to ";
                }
                if (m.RunningStep.missionStep.locationtype == "distanceGreater")
                {
                    task = "get farther away than " + m.RunningStep.missionStep.location_distance + " from ";
                }
                if (m.RunningStep.missionStep.location_egg != "")
                {
                    task = task + m.RunningStep.missionStep.location_egg + " at ";
                }
                if (m.RunningStep.missionStep.location_part != "")
                {
                    task = task + m.RunningStep.missionStep.location_part;
                }
            }
            return task;
        }

        public static string TaskStringConstructor_Jobs(LCARS_NCI_Mission_Archive_RunningMission m)
        {
            string task = "";
            if (m.RunningStep.RunningJob.missionJob.target == "none")
            {
                if (m.RunningStep.RunningJob.missionJob.jobtype == "awaitingUserInput")
                {
                    task = "<make a decission, the system will wait>";
                }
            }
            if (m.RunningStep.RunningJob.missionJob.target == "object" || m.RunningStep.RunningJob.missionJob.target == "egg")
            {
                if (m.RunningStep.RunningJob.missionJob.jobtype == "distanceLower" || m.RunningStep.RunningJob.missionJob.jobtype == "distanceGreater")
                {
                    if (m.RunningStep.RunningJob.missionJob.jobtype == "distanceLower")
                    {
                        task = "get closer than " + m.RunningStep.RunningJob.missionJob.distance + " to ";
                    }
                    if (m.RunningStep.RunningJob.missionJob.jobtype == "distanceGreater")
                    {
                        task = "get farther away than " + m.RunningStep.RunningJob.missionJob.distance + " from ";
                    }
                    if (m.RunningStep.RunningJob.missionJob.NCI_egg != "")
                    {
                        task = task + m.RunningStep.RunningJob.missionJob.NCI_egg + " at ";
                    }
                    if (m.RunningStep.RunningJob.missionJob.NCI_object != "")
                    {
                        task = task + m.RunningStep.RunningJob.missionJob.NCI_object;
                    }
                }
                if (m.RunningStep.RunningJob.missionJob.jobtype == "destroy")
                {
                    task = "destroy the Object " + m.RunningStep.RunningJob.missionJob.NCI_object;
                }
                if (m.RunningStep.RunningJob.missionJob.jobtype == "scan")
                {
                    task = "(ToDo)";
                }
            }
            if (m.RunningStep.RunningJob.missionJob.target == "artefact")
            {
                if (m.RunningStep.RunningJob.missionJob.jobtype == "collect")
                {
                    task = "Collect the Object " + m.RunningStep.RunningJob.missionJob.NCI_artefact;
                }
            }
                    
            return task;
        }

        public static bool isDistanceLower(Vector3 position, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceLower distance=" + distance + " tmp=" + tmp);
            return (distance > tmp) ? true : false;
        }
        public static bool isDistanceLower(GameObject go, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), go.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceLower distance=" + distance + " tmp=" + tmp);
            return (distance > tmp) ? true : false;
        }
        public static bool isDistanceLower(Part part, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), part.gameObject.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceLower distance=" + distance + " tmp=" + tmp);
            return (distance > tmp) ? true : false;
        }
        public static bool isDistanceLower(Vessel vessel, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), vessel.gameObject.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceLower distance=" + distance + " tmp=" + tmp);
            return (distance > tmp) ? true : false;
        }


        public static bool isDistanceGreater(Vector3 position, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceGreater distance=" + distance + " tmp=" + tmp);
            return (distance < tmp) ? true : false;
        }
        public static bool isDistanceGreater(GameObject go, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), go.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceGreater distance=" + distance + " tmp=" + tmp);
            return (distance < tmp) ? true : false;
        }
        public static bool isDistanceGreater(Part part, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), part.gameObject.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceGreater distance=" + distance + " tmp=" + tmp);
            return (distance < tmp) ? true : false;
        }
        public static bool isDistanceGreater(Vessel vessel, float distance)
        {
            float tmp = Vector3.Distance(getPlayerPosition(), vessel.gameObject.transform.position);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools.isDistanceGreater distance=" + distance + " tmp=" + tmp);
            return (distance < tmp) ? true : false;
        }


        public static DateTime getNow()
        {
            return DateTime.Now;
        }

        public static int get_current_stepID()
        {
            int sID = 0;
            try { sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id; }
            catch { }
            return sID;
        }

        public static int get_current_jobID()
        {
            int jID = 0;
            try { jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID; }
            catch { }
            return jID;
        }
    }
    
    public class LCARS_NCI_Mission_Archive
    {

        public Dictionary<string, LCARS_NCI_Mission> all_Missions { set; get; }
        public Dictionary<string, LCARS_NCI_Mission> all_backgroundMissions { set; get; }
        public Dictionary<string, LCARS_NCI_Mission> playable_backgroundMissions { set; get; }
        public Dictionary<string, LCARS_NCI_Mission> all_UserMissions { set; get; }
        public Dictionary<string, LCARS_NCI_Mission> playable_UserMissions { set; get; }
        public Dictionary<LCARS_NCI_Mission, List<string>> MissingPartMissions { set; get; }

        public LCARS_NCI_Mission_Archive_RunningMission RunningMission { set; get; }
        //public LCARS_NCI_Mission_Archive_RunningBackgroundMissions RunningBackgroundMissions { set; get; }
        public Vessel player { set; get; }


        public void ObserveBackgroundMissions()
        {

            if (!LCARS_NCI_Mission_Archive_Tools.isReady())
            { return; }

            if (!HighLogic.LoadedSceneIsFlight)
            {
                return;
            }


            UnityEngine.Debug.Log("### ObserveBackgroundMissions begin ");

            //LCARS_NCI.Instance.Data.vessel;
            //LCARS_NCI.Instance.Data.kerbal;



            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission = new LCARS_NCI_Mission_Archive_RunningMission();
            }
            UnityEngine.Debug.Log("### ObserveBackgroundMissions 1 ");
                
            try
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation == null)
                {
                        LCARS_NCI.Instance.GUI.Generic.setWindowState(false);
                }
            }
            catch { }

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission != null)
            {
                UnityEngine.Debug.Log("### ObserveBackgroundMissions skipped ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.processMission();
                try
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.conditionConfirmedOnce = true;
                }
                catch { }
            return;
            }
            UnityEngine.Debug.Log("### ObserveBackgroundMissions 2 ");





            foreach (KeyValuePair<Guid,LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
            {
               UnityEngine.Debug.Log("### ObserveBackgroundMissions 3 ");
                LCARS_NCI_Object O = pair.Value;
                Guid GID = pair.Key;
                bool delete_this_entry = true;
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if(GID == v.id)
                    {
                        delete_this_entry = false;
                    }
                }
                if (delete_this_entry)
                {
                    LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Remove(GID);
                    continue;
                }
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission != null)
                {
                    return;
                }
               UnityEngine.Debug.Log("### ObserveBackgroundMissions 4 ");
               if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack == null)
               {
                   LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack = new LinkedList<string>();
               }
               UnityEngine.Debug.Log("### ObserveBackgroundMissions 4b ");

                int stackcount = 0;
                UnityEngine.Debug.Log("### missionStack debug total items=" + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.Count);
                foreach (string s in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack)
                {
                   UnityEngine.Debug.Log("### missionStack debug before  s=" + s + " stackcount=" + stackcount);
                    stackcount++;
                }
                UnityEngine.Debug.Log("### ObserveBackgroundMissions 4c ");
                /*
                */

                if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missions == null)
                {
                    LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missions = new Dictionary<string, LCARS_NCI_Object_assigned_missions>();
                }
                foreach (LCARS_NCI_Object_assigned_missions aM in O.missions.Values)
                {
                    UnityEngine.Debug.Log("### ObserveBackgroundMissions 5 loop missions start aM.mission_id=" + aM.mission_id);

                    if (!aM.MissionObjectsAssignedToSFSObjects) { LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missions[aM.mission_id] = Assigne_MissionObjectsToSFSObjects(aM); }

                    if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack == null)
                    {
                    }

                    // get only the first mission in this Object's mission stack!
                    if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.First.Value == aM.mission_id)
                    {
                       UnityEngine.Debug.Log("### ObserveBackgroundMissions 6 first stack item found =" + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.First.Value);
                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission != null)
                        {
                            return;
                        }
                        UnityEngine.Debug.Log("### ObserveBackgroundMissions 7 aM.mission_id=" + aM.mission_id + " aM.assigned_sfs_object_Guid=" + aM.assigned_sfs_object_Guid);
                        if (aM.checkCondition(aM))
                        {
                            Debug.Log("NCIGD ObserveBackgroundMissions valid mission found: " + aM.mission.title + " is valid to be processed");
                            UnityEngine.Debug.Log("### ObserveBackgroundMissions RunningMission was changed  m.title=" + aM.mission.title);
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = aM.mission;
                            Debug.Log("NCIGD ObserveBackgroundMissions RunningMission.processStep m.title=" + aM.mission.title);

                            // move the mission to the end of the stack!
                            LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.AddLast(aM.mission_id);
                            LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.Remove(aM.mission_id);
                            // move the mission to the end of the stack!

                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.processMission();
                        }
                       UnityEngine.Debug.Log("### ObserveBackgroundMissions 8  first stack item done ");
                    }
                   UnityEngine.Debug.Log("### ObserveBackgroundMissions 9  loop missions end ");

                }
                /*
                stackcount = 0;
                UnityEngine.Debug.Log("### missionStack debug total items=" + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack.Count);
                foreach (string s in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[GID].missionStack)
                {
                    UnityEngine.Debug.Log("### missionStack debug after  s=" + s + " stackcount=" + stackcount);
                    stackcount++;
                }
                */
            }
            
            
            
            
            /*
            foreach (LCARS_NCI_Mission m in playable_backgroundMissions.Values)
            {
                UnityEngine.Debug.Log("### ObserveBackgroundMissions 1 ");
                Debug.Log("NCIGD ObserveBackgroundMissions m.title=" + m.title);
                if (m.steps[1]==null)
                {continue;}
                UnityEngine.Debug.Log("### ObserveBackgroundMissions 2 ");

                LCARS_NCI_Mission_Step this_step = m.steps[1];
                UnityEngine.Debug.Log("### ObserveBackgroundMissions 3 ");

                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission == null)
                {
                    UnityEngine.Debug.Log("### ObserveBackgroundMissions RunningMission==null create new object m.title=" + m.title);
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission = new LCARS_NCI_Mission_Archive_RunningMission();
                    //RunningMission.current_job = 1;
                }


                if (this_step.checkCondition())
                {
                    Debug.Log("NCIGD ObserveBackgroundMissions mission: "+m.title+" Step: "+this_step.id+" is valid to be processed");
                        UnityEngine.Debug.Log("### ObserveBackgroundMissions RunningMission was changed  m.title=" + m.title);
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = m;
                        //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = current_step;
                        //RunningMission.current_job = 1;
                    Debug.Log("NCIGD ObserveBackgroundMissions RunningMission.processStep m.title=" + m.title);
                }

                RunningMission.processMission();

            }
            */
            UnityEngine.Debug.Log("### ObserveBackgroundMissions done ");
        }



        public void addMission(LCARS_NCI_Mission m)
        {
            Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission begin m.filename=" + m.filename);

            if (LCARS_NCI.Instance.Data == null)
            {
                Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission LCARS_NCI.Instance.Data == null ");
                LCARS_NCI.Instance.init();
                LCARS_NCI.Instance.Data.init();
            }

            if (LCARS_NCI.Instance.Data.MissionArchive.all_Missions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.all_Missions = new Dictionary<string, LCARS_NCI_Mission>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.all_backgroundMissions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.all_backgroundMissions = new Dictionary<string, LCARS_NCI_Mission>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions = new Dictionary<string, LCARS_NCI_Mission>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.all_UserMissions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.all_UserMissions = new Dictionary<string, LCARS_NCI_Mission>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions = new Dictionary<string, LCARS_NCI_Mission>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.MissingPartMissions == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.MissingPartMissions = new Dictionary<LCARS_NCI_Mission, List<string>>();
            }
            //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 1 m.filename=" + m.filename);

            if (m.story_type == "AutomaticBackgroundStory" && !LCARS_NCI.Instance.Data.MissionArchive.all_backgroundMissions.ContainsKey(m.filename))
            {
                LCARS_NCI.Instance.Data.MissionArchive.all_backgroundMissions.Add(m.filename, m);
            }
            //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 2 ");
            if (m.story_type == "UserStartedStory" && !LCARS_NCI.Instance.Data.MissionArchive.all_UserMissions.ContainsKey(m.filename))
            {
                LCARS_NCI.Instance.Data.MissionArchive.all_UserMissions.Add(m.filename, m);
            }

            if (!LCARS_NCI.Instance.Data.MissionArchive.all_Missions.ContainsKey(m.filename))
            {
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 3 ");
                LCARS_NCI.Instance.Data.MissionArchive.all_Missions.Add(m.filename, m);
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 4 ");
            }
            if (LCARS_NCI.Instance.Data.ObjectArchive == null)
            {
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 6 ");
                LCARS_NCI.Instance.Data.ObjectArchive = new Dictionary<string, LCARS_NCI_Object>();
            }

            //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 7 m.filename=" + m.filename);

            if (m.meetsRequirements())
            {
                if (m.story_type != null)
                {
                    if (m.story_type == "AutomaticBackgroundStory" && !LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions.ContainsKey(m.filename))
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions.Add(m.filename, m);
                        //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 10 ");
                    }
                    if (m.story_type == "UserStartedStory" && !LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions.ContainsKey(m.filename))
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions.Add(m.filename, m);
                        //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 11 ");
                    }
                }
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12 ");
                if (m.requirements != null)
                {
                    //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12a ");
                    if (m.requirements.list != null)
                    {
                        //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12b ");
                        foreach (LCARS_NCI_Mission_Requirement R in m.requirements.list.Values)
                        {
                            foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                            {
                                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission checking object: " + pair.Value.partname);
                                if (pair.Value.partname == R.part_idcode)
                                {
                                    //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12c " + pair.Value.partname);
                                    LCARS_NCI_Object_assigned_missions nAM = new LCARS_NCI_Object_assigned_missions();
                                    nAM.mission_id = m.filename;
                                    nAM.condition_type = m.steps[1].locationtype;
                                    nAM.condition_egg = m.steps[1].location_egg;
                                    nAM.condition_part = m.steps[1].location_part;
                                    nAM.condition_distance = m.steps[1].location_distance;
                                    nAM.mission = m;
                                    if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missions == null)
                                    {
                                        LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missions = new Dictionary<string, LCARS_NCI_Object_assigned_missions>();
                                    }
                                    if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack == null)
                                    {
                                        LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack = new LinkedList<string>();
                                    }
                                    if (!LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missions.ContainsKey(nAM.mission_id))
                                    {
                                        LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missions.Add(nAM.mission_id,nAM);
                                        LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.AddFirst(nAM.mission_id);
                                    }
                                }
                            }
                            //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12c ");
                            if (LCARS_NCI.Instance.Data.ObjectArchive.ContainsKey(R.part_idcode))
                            {
                                if (LCARS_NCI.Instance.Data.ObjectArchive[R.part_idcode].missions == null)
                                {
                                    LCARS_NCI.Instance.Data.ObjectArchive[R.part_idcode].missions = new Dictionary<string,LCARS_NCI_Object_assigned_missions>();
                                }
                                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12d ");
                                LCARS_NCI.Instance.Data.ObjectArchive[R.part_idcode].assignMission(m);
                                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12e ");
                            }
                        }
                    }
                    //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 12f ");
                }
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 13 ");
            }
            else 
            {
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 14 ");
                if (m.requirements != null)
                {
                    if (m.requirements.list != null)
                    {
                        foreach (LCARS_NCI_Mission_Requirement R in m.requirements.list.Values)
                        {
                            if (R.part_idcode != null)
                            {
                                if (MissingPartMissions.ContainsKey(m))
                                {
                                    MissingPartMissions[m].Add(R.part_idcode);
                                    //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 15 ");
                                }
                                else
                                {
                                    List<string> tmp = new List<string>();
                                    tmp.Add(R.part_idcode);
                                    MissingPartMissions.Add(m, tmp);
                                    //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 16 ");
                                }
                            }
                        }
                    }
                }
                //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 17 ");
            }
            //Debug.Log("LCARS_NCI_Data LCARS_NCI_Mission_Archive addMission 18 ");


        }

        private static System.Random rnd = new System.Random();
        private LCARS_NCI_Object_assigned_missions Assigne_MissionObjectsToSFSObjects(LCARS_NCI_Object_assigned_missions this_aM)
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects begin ");

            LCARS_NCI_Mission mission = this_aM.mission;

            if (this_aM.MissionObjectsAssignedToSFSObjects) { return this_aM; }

            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects !this_aM.MissionObjectsAssignedToSFSObjects ");

            if (mission.requirements_confirmation_list == null) { mission.requirements_confirmation_list = new List<Guid>(); }


            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects requirements.list.Count=" + mission.requirements.list.Count);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects mission.requirements_confirmation_list.Count=" + mission.requirements_confirmation_list.Count);
            if (mission.requirements.list.Count >= mission.requirements_confirmation_list.Count)
            {
                foreach (KeyValuePair<int, LCARS_NCI_Mission_Requirement> pairR in mission.requirements.list)
                {
                    LCARS_NCI_Mission_Requirement R = pairR.Value;
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects R.part_idcode=" + R.part_idcode);
                    foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                    {
                        //int thisRandom = rnd.Next(1, 2);
                        this_aM.MissionObjectsAssignedToSFSObjects = false;
                        LCARS_NCI_Object O = pair.Value;
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects O.partname=" + O.partname);
                        if (O.partname == R.part_idcode /*&& thisRandom>1*/)
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects assigned pair.Key=" + pair.Key);
                            mission.requirements.list[pairR.Key].assigned_sfs_object_Guid = pair.Key;
                            if (pairR.Key==1)
                            {
                                this_aM.assigned_sfs_object_Guid = pair.Key;
                            }
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects assigned mission.requirements.list[" + pairR.Key + "].assigned_sfs_object_Guid=" + mission.requirements.list[pairR.Key].assigned_sfs_object_Guid);
                            mission.requirements_confirmation_list.Add(R.assigned_sfs_object_Guid);
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission Assigne_MissionObjectsToSFSObjects this_aM.MissionObjectsAssignedToSFSObjects=true, All requirements are assigned, skipping ");
                this_aM.MissionObjectsAssignedToSFSObjects = true;
            }
            return this_aM;
        }
    }


    public class LCARS_NCI_Mission_Archive_RunningMission
    {
        public LCARS_NCI_Mission mission { set; get; }
        public string missionGuid { set; get; }
        public LCARS_NCI_MissionProgressLog progressLog { set; get; }
        public List<string> sentMessageLog { get; set; }
        public List<LoopingScreenMessage> LoopingScreenMessages { get; set; }
        public int current_step { set; get; }
        public int current_job { set; get; }
        public LCARS_NCI_Mission_Archive_RunningStep RunningStep { set; get; }
        public bool step_start_done { set; get; }
        public bool step_conversation_done { get; set; }
        public bool sealDeal_done { get; set; }
        public bool job_start_done { set; get; }
        public bool step_end_reached { set; get; }
        public bool job_end_reached { set; get; }

        public int get_currentStep()
        {
            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step;
        }
        public int get_currentJob()
        {
            return LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job;
        }
        public void go_to_next_Step(string RequestedStepID = "")
        {
            int sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            int jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step begin " + sID + "/" + jID + " RequestedStepID=" + RequestedStepID);

            if ((LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.next_step == "missionEnd" && RequestedStepID == "") || RequestedStepID == "missionEnd")
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step missionEnd " + sID + "/" + jID + " missionEnd");

                UnityEngine.Debug.Log("### The NCI Mission <<" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title+ ">> has ended, saving now the mission log");

                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                            "Mission End",
                            "You have successfully played through this mission!"
                    );

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.saveMissionLog();

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.reset_mission();

                    
                UnityEngine.Debug.Log("### Mission Log saved system cleaned up, resuming surveilance mode");
                return;
            }

           //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step");
            try
            {
               //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step try");
                if (LCARS_NCI_Mission_Archive_Tools.ToInt(RequestedStepID)>0)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(RequestedStepID);
                }
                else
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.next_step);
                }
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = 0;

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission;
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid = Guid.NewGuid().ToString();  }
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step];
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog = new List<string>();
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack = new LCARS_NCI_Mission_Archive_RunningStep_SentMessageStack(); LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list = new Dictionary<string, LCARS_NCI_Mission_Archive_RunningStep_MessageItem>(); }
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = 1;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob = new LCARS_NCI_Mission_Archive_RunningJob();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job];

                sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
                jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step end " + sID + "/" + jID + " ");
            }
            catch
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Step catch STEPERROR abort mission");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.reset_mission();
                return;

            }

        }

        private void reset_mission()
        {
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.conditionConfirmedOnce = false;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep = null;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = 0;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = 0;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done = false;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done = false;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = false;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission = null;
        }

        public void go_to_next_Job(int RequestedJobID = 0)
        {
            int sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            int jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            
            
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job begin " + sID + "/" + jID + " ");
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList.Count >= LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job && !LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached)
            {
                if (RequestedJobID>0)
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = RequestedJobID;
                }
                else
                {
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job++;
                }
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob = new LCARS_NCI_Mission_Archive_RunningJob();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job];
               //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID);
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog = new List<string>();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.id = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done = false;
            }
            
            //current_job++;
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList.Count < LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job || LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job step_end_reached");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = true;
                /*if (
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.missionSteps == LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id && 
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.isStepEnd &&
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.next_step == "missionEnd")
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job This Mission has Ended!!!!!!!!!!!!!!!!!!");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission = null;
                }
                else
                {
                }*/
                //go_to_next_Step();
            }
            
            
            sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            
            
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job end " + sID + "/" + jID + " ");
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission go_to_next_Job RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID);
        }


        public LCARS_NCI_Mission_Archive_RunningStep_SentMessageStack SentMessageStack { set; get; }
        public void processMessageReply(string Guid, string replyCode, string replyID)
        {
            int sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            int jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply begin " + sID + "/" + jID + " ");
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);

            LCARS_NCI_Mission_Archive_RunningStep_MessageItem I_tmp = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list[Guid];

            string response_task = "";
            switch (replyCode)
            {
                case "none":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply replyCode=" + replyCode + " replyID=" + replyID);
                    response_task = " The System is waiting for a user input, like: a click on a response option in your messages!";
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply mission log  ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "You replied to the message " + I_tmp.messageID + "",
                                "The reply code was: <" + replyCode + ">" + response_task
                            );
                    break;

                case "message":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply replyCode=" + replyCode + " replyID=" + replyID);
                    response_task = " the system will send you the message " + replyID + ".";
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply mission log  ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "You replied to the message " + I_tmp.messageID + "",
                                "The reply code was: <" + replyCode + ">" + response_task
                            );
                    try
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog.Remove(I_tmp.stepID + "_" + replyID);
                    }
                    catch { }
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(replyID), I_tmp.stepID);
                    break;

                case "step":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply replyCode=" + replyCode + " replyID=" + replyID);
                    response_task = " the mission moves on to Chapter " + replyID + ".";
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply mission log  ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "You replied to the message " + I_tmp.messageID + "",
                                "The reply code was: <" + replyCode + ">" + response_task
                            );
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Step(replyID);
                    break;

                case "job":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply replyCode=" + replyCode + " replyID=" + replyID);
                    response_task = " the mission moves on to Job " + replyID + ".";
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply mission log  ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                "You replied to the message " + I_tmp.messageID + "",
                                "The reply code was: <" + replyCode + ">" + response_task
                            );
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Job(LCARS_NCI_Mission_Archive_Tools.ToInt(replyID));
                    break;
            }
            sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply done " + sID + "/" + jID + " ");
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningMission processMessageReply current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
        }


        public void processMission()
        {
            Debug.Log("LCARS_NCI_Mission_Archive_RunningMission processMission begin ");


            try { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID);             }
            catch { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=ERROR");             }
            
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission == null) { return; }
            if (!LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.meetsRequirements()) { return; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step < 1) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step = 1; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step] == null) { return; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep = new LCARS_NCI_Mission_Archive_RunningStep(); LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done = false; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack = new LCARS_NCI_Mission_Archive_RunningStep_SentMessageStack(); LCARS_NCI.Instance.Data.MissionArchive.RunningMission.SentMessageStack.list = new Dictionary<string,LCARS_NCI_Mission_Archive_RunningStep_MessageItem>(); }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid = Guid.NewGuid().ToString(); }

            

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step;
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step];
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sentMessageLog = new List<string>();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog = new LCARS_NCI_MissionProgressLog();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.ItemList = new List<LCARS_NCI_MissionProgressLog_Item>();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.missionGuid = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.missionGuid;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.mission_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.gain_collected = false;

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                        "Mission Start",
                        "The mission <<"+LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title+">> was loaded in the GameDriver"
                    );
            }

            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processMission LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.id);


            try { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=ERROR");}

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_job = LCARS_NCI_Mission_Archive_Tools.get_current_jobID();
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.current_step = LCARS_NCI_Mission_Archive_Tools.get_current_stepID();
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.processStep();

            try { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### processMission RunningJob.missionJob.jobID=ERROR"); }

        }

    }


    public class LCARS_NCI_Mission_Archive_RunningStep
    {
        public int id { get; set; }
        public LCARS_NCI_Mission_Step missionStep { get; set; }
        public bool conditionConfirmedOnce { get; set; }
        public LCARS_NCI_Mission_Archive_RunningJob RunningJob { set; get; }
        public LCARS_NCI_Mission_Archive_RunningConversation RunningConversation { get; set; }

        public bool NCI_LCARSShipSystems_are_damaged { get; set; }
        public bool NCI_LCARSShipSystems_are_damaged_notification_sent { get; set; }
        public void processStep()
        {
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep begin ");

            try { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=ERROR"); }
            /*
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job < 1) 
            {   LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = 1; }
            */
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.checkCondition())
            {
                //Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep missionStep.checkCondition=true");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.conditionConfirmedOnce = true;
                Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep conditionConfirmedOnce=true");
            }
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep missionStep.checkCondition=true");

#region NCIObject Connect
/*
            // ******************************************************
            // Connect to NCIObject and interfere with Part Options:
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep Connect to NCIObject and interfere with Part Options begin");
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.PartOptions == null) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.PartOptions = new Dictionary<string, LCARS_NCI_Step_Remote_PartOptions_Type>(); }
            
            Debug.Log("Connect to NCIObject 1");

            int requirementPart_id = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.requirementPart_id;

            Debug.Log("Connect to NCIObject 2");

            string location_part = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.location_part;

            Debug.Log("Connect to NCIObject 3");

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.PartOptions.Count>0)
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    Debug.Log("Connect to NCIObject 3 loop 1");
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[requirementPart_id].assigned_sfs_object_Guid != v.id)
                    {continue;}

                    Debug.Log("Connect to NCIObject 3 loop 2");
                    Dictionary<string, bool> sendOptions = new Dictionary<string, bool>();
                    foreach(LCARS_NCI_Step_Remote_PartOptions_Type pO in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.PartOptions.Values)
                    {
                        sendOptions.Add(pO.OptionID,pO.isDisabled);
                    }
                    Debug.Log("Connect to NCIObject 3 loop 3");
                    v.SendMessageUpwards("NCIRemote_Generic_EnableDisable", sendOptions, SendMessageOptions.DontRequireReceiver);
                    Debug.Log("Connect to NCIObject 3 loop 4");
                }
            }

            Debug.Log("Connect to NCIObject 4");

            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep Connect to NCIObject and interfere with Part Options done");
            // Connect to NCIObject and interfere with Part Options:
            // ******************************************************
*/
#endregion


#region LCARS Connect
            // ******************************************************
            // Connect to LCARS ODN and interfere with ship systems:
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep Connect to LCARS ODN and interfere with ship systems begin");
            try
            {
                LCARSNCI_Bridge.Instance.LCARS_NCI_Mission_Archive_RunningStep_interfere_with_ship_systems();
            }
            catch (Exception ex)
            {
                Debug.Log("LCARS_NCI_Mission_Archive_RunningStep LCARS_NCI_Mission_Archive_RunningStep_interfere_with_ship_systems failed ex="+ex);
            }
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep Connect to LCARS ODN and interfere with ship systems done");
            // Connect to LCARS ODN and interfere with ship systems:
            // ******************************************************
#endregion

            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep checking for step_start_done");
            if (!LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done && LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.conditionConfirmedOnce) 
            {
                Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep step_start");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
                PerformStartStepTasks();
            }
            Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep step_start_done=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done);

            try { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=ERROR"); }

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done)
            {
                Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep step_start_done=true on to PerformStepTasks");

                PerformStepTasks();

                Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep PerformStepTasks done checking for job_end_reached");

                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached) 
                {
                    Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep job_end_reached");
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job == LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList.Count)
                    {
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = true;
                        Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep step_end_reached=true");
                        PerformEndStepTasks(); 
                        //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Step();
                        //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.job_end_reached = false;
                    }
                }
            }

            //try { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { UnityEngine.Debug.Log("### processStep RunningJob.missionJob.jobID=ERROR"); }

            //Debug.Log("LCARS_NCI_Mission_Archive_RunningStep processStep done");
        }

        internal void PerformStartStepTasks()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStartStepTasks");

            try { UnityEngine.Debug.Log("### PerformStartStepTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### PerformStartStepTasks RunningJob.missionJob.jobID=ERROR"); }
            
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
            "Chapter Start",
            "Your task <" + LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Steps(LCARS_NCI.Instance.Data.MissionArchive.RunningMission) + "> has started."
            );

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepStart_messageID_email));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepStart_messageID_console));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepStart_messageID_screen));

            
            
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_start_done = true;
        }

        private float LoopingScreenMessage_lastUpdate = 0.0f;
        private float LoopingScreenMessage_Interval = 0.5f;
        internal void PerformStepTasks()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks");

            try { UnityEngine.Debug.Log("### PerformStepTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### PerformStepTasks RunningJob.missionJob.jobID=ERROR"); }

            bool show_conversation = false;
            try
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks check for Conversation begin ");
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation != null)
                {
                    show_conversation = true;
                }
                /*if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton != null)
                {
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton.buttontext1 != "")
                    {
                        show_conversation = true;
                    }
                }*/
                if (!LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done)
                {
                    show_conversation = true;
                }
                else
                {
                    show_conversation = false;
                }
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks check for Conversation done ");
            }
            catch {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks check for Conversation failed ");
            }
            if(show_conversation)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks => RunConversation");
                RunConversation();
            }
            else 
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation == null)
                {
                    LCARS_NCI.Instance.GUI.Generic.setWindowState(false);
                }
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks => RunJobs");
                RunJobs();
            }

            try
            {
                if ((Time.time - LoopingScreenMessage_lastUpdate) > LoopingScreenMessage_Interval)
                {
                    LoopingScreenMessage_lastUpdate = Time.time;

                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID > 0)
                    {
                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages == null)
                        {
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages = new List<LoopingScreenMessage>();
                        }
                        foreach (LoopingScreenMessage L in LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages)
                        {
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage loop running L.messageID=" + L.messageID);
                            if (L.step < LCARS_NCI_Mission_Archive_Tools.get_current_stepID() || L.job < LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID)
                            {
                                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.LoopingScreenMessages.Remove(L);
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage message removed ");
                            }
                            else
                            {
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage attempt to find L.messageID=" + L.messageID + " L.step=" + L.step + " L.job=" + L.job);
                                string text = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.steps[L.step].Messages.MessageList[L.messageID].title;
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage text=" + text);
                                if (text.Contains("[DISTANCE]"))
                                {
                                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage text contains [DISTANCE]  text=" + text);
                                    int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object);
                                    float distance = 0f;
                                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage  mR index=" + index);
                                    if (index > 0)
                                    {
                                        L.contextPosition = Vector3.zero;
                                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage L.contextPosition=null");
                                        LCARS_NCI_Mission_Requirement mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];
                                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage mR.name=" + mR.name);

                                        if (mR.assigned_sfs_object_Guid.ToString() != "00000000-0000-0000-0000-000000000000" && L.contextPosition == Vector3.zero)
                                        {
                                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage mR.assigned_sfs_object_Guid=" + mR.assigned_sfs_object_Guid);
                                            foreach (Vessel v in FlightGlobals.Vessels)
                                            {
                                                if (v.id == mR.assigned_sfs_object_Guid)
                                                {
                                                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage mR.assigned_sfs_object_Guid found v.name=" + v.name);
                                                    L.contextTransform = v.transform;
                                                    L.contextPosition = v.CoM;
                                                    L.vessel = v;
                                                }
                                            }
                                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage set L.contextPosition again ");
                                            L.contextPosition = (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg == "") ? L.contextPosition : L.vessel.rootPart.FindModelTransform(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg).position;
                                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage mR.assigned_sfs_object_Guid found missionJob.NCI_egg=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);
                                        }
                                        if (L.contextPosition != Vector3.zero)
                                        {
                                            distance = Vector3.Distance(FlightGlobals.ActiveVessel.CoM, L.contextPosition);
                                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage mR.assigned_sfs_object_Guid found distance=" + distance);
                                            text = text.Replace("[DISTANCE]", distance + " Meter");
                                        }


                                    }
                                }
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage display message text=" + text);
                                ScreenMessages.PostScreenMessage(
                                   "<color=#ff9900ff>LoopingScreenMessage: " + text + "</color>",
                                  LoopingScreenMessage_Interval, ScreenMessageStyle.UPPER_CENTER
                                );
                            }
                            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage loop end L.messageID=" + L.messageID);
                        }
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Step PerformStepTasks LoopingScreenMessage done ");
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformStepTasks LoopingScreenMessage failed ex=" + ex);
            }

        }

        internal void PerformEndStepTasks()
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep PerformEndStepTasks");

            try { UnityEngine.Debug.Log("### PerformEndStepTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { UnityEngine.Debug.Log("### PerformEndStepTasks RunningJob.missionJob.jobID=ERROR"); }
            
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepEnd_messageID_email));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepEnd_messageID_console));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.stepEnd_messageID_screen));

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                "Chapter End",
                "Your task <" + LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Steps(LCARS_NCI.Instance.Data.MissionArchive.RunningMission) + "> has ended."
                );

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Step();
        }

        internal void RunConversation()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation begin ");
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done)
            { return; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation == null && LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation.SpeechList.Count > 0)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation create new RunningConversation ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation = new LCARS_NCI_Mission_Archive_RunningConversation();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = 0;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done = false;
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation.SpeechList.Count < 1)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation obsolete, no speeches, skipping!");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done = true;
            }

            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation 1 ");

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation != null && !LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation 2 ");

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation;
            }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation.SpeechList != null && LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Conversation.SpeechList.Count>=1)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation 3 ");

                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID == 0)
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation 4 ");

                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = 1;
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.WindowState = true;
                }
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation 5 ");

                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.drawWindow();
            }
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunConversation done ");

        }

        internal void RunJobs()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs begin ");

            try { UnityEngine.Debug.Log("### RunJobs RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { }

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep == null) { return; }

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs missionStep.checkCondition ");

            //if (!missionStep.checkCondition()) { return; }
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job < 1) { LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job = 1; }

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs test jobList ");
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs test jobList count=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList.Count);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs test jobList current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
            try
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job] == null) { return; }
            }
            catch (Exception ex) { UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs test jobList failed ex="+ex); }
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs check for RunningJob ");

            //try { UnityEngine.Debug.Log("### RunJobs RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { }

            
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob == null)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs create new RunningJob ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob = new LCARS_NCI_Mission_Archive_RunningJob();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done = false;
            }

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs set current RunningJob ");
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.jobList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job];
            //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.Jobs.getJobByID(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs set current RunningJob.id ");
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.id = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job;
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep()=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep());
           // UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob()=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob());
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_step);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.current_job);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs RunningJob.mission.title=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs RunningJob.id=" + RunningJob.id);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs RunningJob.missionJob.jobID=" + RunningJob.missionJob.jobID);

            if (!LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs PerformStartJobTasks ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.PerformStartJobTasks();
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = false;
            }

            //try { UnityEngine.Debug.Log("### RunJobs RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { }


            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs PerformJobTasks ");
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.PerformJobTasks();

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs RunningJob.checkCondition ");
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.checkCondition())
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs job_end_reached");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = true;
            }


            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached)
            {
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs PerformEndJobTasks ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.PerformEndJobTasks();
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.isStepEnd)
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs RunningJob.missionJob.isStepEnd ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = true;
                }
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs go_to_next_Job ");
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Job();
                //LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.job_end_reached = false;
            }


            try { UnityEngine.Debug.Log("### RunJobs RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            catch { }



            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningStep RunJobs done ");

        }



    }

    public class LCARS_NCI_Mission_Archive_RunningStep_SentMessageStack
    {
        public Dictionary<string, LCARS_NCI_Mission_Archive_RunningStep_MessageItem> list { set; get; }
    }
    public class LCARS_NCI_Mission_Archive_RunningStep_MessageItem
    {
        public Guid id { set; get; }
        public int stepID { set; get; }
        public int messageID { set; get; }
    }


    public class LCARS_NCI_MissionProgressLog
    {
        public LCARS_NCI_Mission mission { set; get; }
        public string missionGuid { set; get; }
        public List<LCARS_NCI_MissionProgressLog_Item> ItemList { get; set; }
        public int current_step { set; get; }
        public int current_job { set; get; }
        public bool mission_end_reached { set; get; }
        public bool gain_collected { set; get; }


        public bool saveMissionLog()
        {
            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog begin ");
            string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/NCI";
            string loadfileName = "MissionLog.log_cfg";
            string missionCFG_path = path + "/" + loadfileName;

            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 1 ");

            ConfigNode config = null;
            ConfigNode oldconfig = null;
            ConfigNode MISSIONPROGRESSLOG = null;
            try
            {
                config = ConfigNode.Load(missionCFG_path);
                if (config == null)
                {
                    config = new ConfigNode("root");
                    MISSIONPROGRESSLOG = new ConfigNode("MISSIONPROGRESSLOG");
                    config.AddNode(MISSIONPROGRESSLOG);
                    Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 1a ");
                }
                oldconfig = config;
                config = new ConfigNode("root");
            }
            catch
            {
                config = new ConfigNode("root");
                MISSIONPROGRESSLOG = new ConfigNode("MISSIONPROGRESSLOG");
                config.AddNode(MISSIONPROGRESSLOG);
                oldconfig = config;
                config = new ConfigNode("root");
                Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 1c ");
            }
            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 1d");
            MISSIONPROGRESSLOG = oldconfig.GetNode("MISSIONPROGRESSLOG");


            //config = null;

            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 2 ");


            LCARS_NCI_MissionLog_Config_Writer cfgWriter = new LCARS_NCI_MissionLog_Config_Writer();
            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 3 ");

            ConfigNode MissionLog = null;
            try
            {
                MissionLog = cfgWriter.construct_missionlog_config(this);
                Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 4 construct_missionlog_config worked ");
            }
            catch
            {
                Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 4 construct_missionlog_config failed ");
            }
            try
            {
                MISSIONPROGRESSLOG.AddNode(MissionLog);
                Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 4b worked ");
            }
            catch
            {
                MISSIONPROGRESSLOG = new ConfigNode("MISSIONPROGRESSLOG");
                MISSIONPROGRESSLOG.AddNode(MissionLog);
                Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 4b failed ");
            }

            config.AddNode(MISSIONPROGRESSLOG);

            // Try to create the directory.
            System.IO.DirectoryInfo di = Directory.CreateDirectory(path);

            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog 5 ");
            if (!config.Save(missionCFG_path))
            {
                return false;
            }
            Debug.Log("LCARS_NCI_MissionProgressLog saveMissionLog done ");
            return true;
        }

        public void addItem(DateTime timestamp, int stepID, int jobID, string subject, string details)
        {
            Debug.Log("LCARS_NCI_MissionProgressLog addItem timestamp=" + timestamp + " stepID=" + stepID + " jobID=" + jobID + " subject=" + subject + " details=" + details + " ");
            LCARS_NCI_MissionProgressLog_Item I = new LCARS_NCI_MissionProgressLog_Item();
            I.timestamp = timestamp;
            I.stepID = stepID;
            I.jobID = jobID;
            I.subject = subject;
            I.details = details;
            ItemList.Add(I);
        }
    }
    public class LCARS_NCI_MissionProgressLog_Item
    {
        public DateTime timestamp { set; get; }
        public int stepID { set; get; }
        public int jobID { set; get; }
        public string subject { set; get; }
        public string details { set; get; }

    }
    
    public class LCARS_NCI_Mission_Archive_RunningConversation
    {
        public LCARS_NCI_Mission_Conversation missionConversation { get; set; }
        public bool WindowState { get; set; }
        private bool oldWindowState { get; set; }
        private int oldspeechID { get; set; }
        public int current_speechID { get; set; }
        public LCARS_NCI_Mission_Conversation_Speech missionSpeech { get; set; }
        GUIStyle scrollview_style2;
        Vector2 ScrollPosition1;

        public void drawWindow()
        {
            if (!LCARS_NCI_Mission_Archive_Tools.isReady())
            { return; }

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation drawWindow begin ");
            if (LCARS_NCI.Instance.GUI.Generic == null)
            {
                LCARS_NCI.Instance.GUI.Generic = new LCARS_NCIGUI_Generic();
            }
            if (WindowState != oldWindowState)
            {
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                "A Conversation with " + LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality].name + " has started",
                LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality].description
                );
                oldWindowState = WindowState;
            }
            LCARS_NCI.Instance.GUI.Generic.setWindowContent("NCIConversation");
            LCARS_NCI.Instance.GUI.Generic.setWindowState(WindowState);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation drawWindow done ");
        }

        Texture2D icon_tex = null;
        string Personality_default_icon = "LCARS_NaturalCauseInc/NCIAssets/Icons/NCIPersonality_noPic";
        public void ConversationGUI()
        {
            if (!LCARS_NCI_Mission_Archive_Tools.isReady())
            { return; }

            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI begin ");
            if (scrollview_style2 == null)
            {
                scrollview_style2 = new GUIStyle();
                scrollview_style2.fixedHeight = 360;
            }

            LCARS_NCI_Personality P = LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality];
            GUILayout.Label("GUI NCI Conversation");
            GUILayout.BeginHorizontal(GUILayout.Height(370), GUILayout.Width(670));

                GUILayout.BeginVertical(GUILayout.Height(364), GUILayout.Width(331));
                    if (P.portrait != "")
                    {
                        icon_tex = GameDatabase.Instance.GetTexture(P.portrait, false);
                    }
                    else 
                    {
                        icon_tex = GameDatabase.Instance.GetTexture(Personality_default_icon, false);
                    }
                    GUILayout.Button(icon_tex, GUILayout.Height(364), GUILayout.Width(331));
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Height(364), GUILayout.Width(331));
                    GUILayout.BeginVertical(scrollview_style2);
                    ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID < 1)
                    {
                        /*
                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton != null)
                        {
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton.buttontext1 != "")
                            {
                                GUILayout.Label("Hello, my nyme is " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality);
                                GUILayout.Label("Please select the topic:");

                                if (GUILayout.Button(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton.buttontext1))
                                {
                                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.GUIButton.textID;
                                }
                            }
                        }
                        else 
                        {
                        }
                        */
                            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI GUIButton is missing ");
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = 1;
                    }
                    if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID >= 1)
                    {
                        //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI Display Speech " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID);
                        LCARS_NCI_Mission_Conversation_Speech Speech = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.SpeechList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID];

                        GUILayout.Label(Speech.text);

                        if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID != oldspeechID)
                        {
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID + ".) " + LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality].name + " said: ",
                            "<" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.SpeechList[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID].text + ">"
                            );
                            oldspeechID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID;

                            if (Speech.reward_player)
                            {
                                Debug.Log("LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI Speech.reward_artefact Speech.reward_player=" + Speech.reward_player);
                                if (Speech.reward_artefact != "")
                                {
                                    Debug.Log("LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI Speech.reward_artefact reward now " + Speech.reward_artefact);
                                    try
                                    {
                                        LCARSNCI_Bridge.Instance.giveArtefactToPlayer(Speech.reward_artefact);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Log("LCARS_NCI_Mission_Archive_RunningConversation giveArtefactToPlayer now failed ex=" + ex);
                                    }
                                }
                                if (Speech.reward_cash != "")
                                {
                                }
                                if (Speech.reward_reputation != "")
                                {
                                }
                                if (Speech.reward_science != "")
                                {
                                }
                            }

                        }

                        foreach (LCARS_NCI_Mission_Speech_Respons R in Speech.ResponsList)
                        {
                            if(R.responsEvent=="giveArtefact" && R.responsArtefact!="")
                            {
                                try
                                {
                                    if (!LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact(R.responsArtefact))
                                    {
                                        GUILayout.Label("[You do NOT have the artefact " + R.responsArtefact + "]");
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.Log("LCARS_NCI_Mission_Archive_RunningConversation DoesPlayerHaveArtefact failed ex=" + ex);
                                }
                            }


                            if (GUILayout.Button(R.responsText))
                            {
                                string response_task = "";
                                switch(R.responsEvent)
                                {
                                    case "stepID":
                                    case "closeWindow":
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_conversation_done = true;
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.WindowState = false;
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.go_to_next_Step(R.responsStepID);
                                        response_task = " go to step " + R.responsStepID + " and the conversation ended.";
                                        break;

                                    case "textID":
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = LCARS_NCI_Mission_Archive_Tools.ToInt(R.responsTextID);
                                        response_task = " and the conversation continued with speech " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID + ".";
                                        break;

                                    case "sealdeal":
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.sealDeal_done = true;
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = LCARS_NCI_Mission_Archive_Tools.ToInt(R.responsTextID);
                                        response_task = " the contract was sealed and  the conversation continued with speech " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID + ".";
                                        break;

                                    case "giveArtefact":
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = LCARS_NCI_Mission_Archive_Tools.ToInt(R.responsTextID);
                                        response_task = " and the conversation continued with speech " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID + ".";
                                        break;

                                    case "giveCash":
                                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.current_speechID = LCARS_NCI_Mission_Archive_Tools.ToInt(R.responsTextID);
                                        response_task = " giveCash (todo)";
                                        break;
                                }
                                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                                            "you replied to " + LCARS_NCI.Instance.Data.Personalities.list[LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.missionConversation.personality].name + "",
                                            "The Response: <" + R.responsText + ">"+response_task
                                        );
                                if (R.responsEvent == "giveArtefact" && R.responsArtefact != "")
                                {
                                    try
                                    {
                                        if (LCARSNCI_Bridge.Instance.DoesPlayerHaveArtefact(R.responsArtefact))
                                        {
                                            LCARSNCI_Bridge.Instance.takeArtefactFromPlayer(R.responsArtefact);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Log("LCARS_NCI_Mission_Archive_RunningConversation DoesPlayerHaveArtefact failed ex=" + ex);
                                    }
                                }
                            }
                        }

                    }


                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();

                GUILayout.EndVertical();


            GUILayout.EndHorizontal();
            LCARS_NCI.Instance.GUI.Generic.setWindowState(WindowState);
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningConversation ConversationGUI done ");
        }
    }

    public class LCARS_NCI_Mission_Archive_RunningSpeech
    {
    }

    public class LCARS_NCI_Mission_Archive_RunningJob
    {
        public int id { get; set; }
        public LCARS_NCI_Mission_Job missionJob { get; set; }
        public bool checkCondition()
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition");
            //missionJob.NCI_object;
            //missionJob.NCI_egg;
            //missionJob.jobtype;
            Vector3 messure_point = Vector3.zero;
            int sID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id;
            int jID = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID;
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID+"/"+jID + " checkCondition ");
            bool return_bool = false;
            LCARS_NCI_Mission_Requirement mR = null;

            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype == "distanceLower" ||
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype == "distanceGreater")
            {

                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " find mR ");
                int index = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.getIndex_By_PartIDcode(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object);
                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " mR index=" + index);
                if (index > 0)
                {
                    mR = LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.requirements.list[index];
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " mR mR.id=" + mR.id);
                }
                else
                {
                    mR = null;
                }

                if (mR == null)
                {
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " mR==null skipping");
                    return false;
                }
            }
            switch (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype)
            {
            
                case"awaitingUserInput":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + jID + " checkCondition awaitingUserInput ");
                    return_bool = false;
                    break;

                case"distanceLower":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID + "/" + jID + " checkCondition distanceLower");
                    foreach (Vessel v in FlightGlobals.Vessels)
                    {
                        if (messure_point != Vector3.zero || v.rootPart == null)
                        { continue; }
                        //if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object == v.vesselName)
                        if (v.rootPart.name.Contains(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object) && mR.assigned_sfs_object_Guid == v.id)
                        {
                            Transform tmp = null;
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg != "")
                            {
                                tmp = v.rootPart.FindModelTransform(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);
                            }
                            if (tmp != null)
                            {
                                messure_point = tmp.transform.position;
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceLower messure_point was found: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg + " at " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object + " " + mR.assigned_sfs_object_Guid);
                            }
                            else
                            {
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceLower messure_point was found: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object + " " + mR.assigned_sfs_object_Guid);
                                messure_point = v.transform.position;
                            }
                        }
                    }
                    if (messure_point == Vector3.zero)
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceLower messure_point was NOT found skipping");
                        return_bool = false;
                    }
                    if (LCARS_NCI_Mission_Archive_Tools.isDistanceLower(messure_point, LCARS_NCI_Mission_Archive_Tools.ToFloat(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance)))
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceLower was met: missionJob.distance=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance);
                        return_bool = true;
                    }
                    else 
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceLower was NOT met: missionJob.distance=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance);
                        return_bool = false;
                    }
                    break;

                case"distanceGreater":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID + "/" + jID + " checkCondition distanceGreater");
                    foreach (Vessel v in FlightGlobals.Vessels)
                    {
                        if (messure_point != Vector3.zero || v.rootPart==null)
                        { continue; }
                        //if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object == v.vesselName)
                        if (v.rootPart.name.Contains(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object) && mR.assigned_sfs_object_Guid == v.id)
                        {
                            Transform tmp = null;
                            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg != "")
                            {
                                tmp = v.rootPart.FindModelTransform(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg);
                            }
                            if (tmp != null)
                            {
                                messure_point = tmp.transform.position;
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceGreater messure_point was found: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_egg + " at " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object + " " + mR.assigned_sfs_object_Guid);
                            }
                            else
                            {
                                UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceGreater messure_point was found: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object + " " + mR.assigned_sfs_object_Guid);
                                messure_point = v.transform.position;
                            }
                        }
                    }
                    if (messure_point == Vector3.zero)
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceGreater messure_point was NOT found skipping");
                        return_bool = false;
                    }
                    if (LCARS_NCI_Mission_Archive_Tools.isDistanceGreater(messure_point, LCARS_NCI_Mission_Archive_Tools.ToFloat(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance)))
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceGreater was met: missionJob.distance=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance);
                        return_bool = true;
                    }
                    else 
                    {
                        UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob checkCondition distanceGreater was NOT met: missionJob.distance=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance);
                        return_bool = false;
                    }
                    break;

                case"destroy":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID + "/" + jID + " checkCondition destroy");
                    foreach (Vessel v in FlightGlobals.Vessels)
                    {
                        if (v.rootPart == null)
                        { continue; }
                        //if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object == v.vesselName)
                        if (v.rootPart.name.Contains(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object) && mR.assigned_sfs_object_Guid == v.id)
                        {
                            return_bool = false;
                        }
                    }
                    return_bool = true;
                    break;

                case"scan":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID + "/" + jID + " checkCondition scan todo");
                    return_bool = false;
                    break;

                case"collect":
                    UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob " + sID + "/" + jID + " checkCondition collect");
                    try
                    {
                        LCARSNCI_Bridge.Instance.giveArtefactToPlayer(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_artefact);
                        Debug.Log("LCARS_NCI_Mission_Archive_RunningJob checkCondition collect giveArtefactToPlayer done");
                        return_bool = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("LCARS_NCI_Mission_Archive_RunningJob checkCondition collect giveArtefactToPlayer now failed ex=" + ex);
                        return_bool = false;
                    }
                    break;

            }
            return return_bool;
        }

        internal void PerformStartJobTasks()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob PerformStartJobTasks");

            //try { UnityEngine.Debug.Log("### PerformStartJobTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { }


            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
            "Job Start",
            "Your task " + LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Jobs(LCARS_NCI.Instance.Data.MissionArchive.RunningMission) + " has started."
            );

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobStart_messageID_email));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobStart_messageID_console));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobStart_messageID_screen));

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_start_done = true;
        }

        internal void PerformJobTasks()
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob PerformJobTasks");

            //try { UnityEngine.Debug.Log("### PerformJobTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { }

        }

        internal void PerformEndJobTasks()
        {
            UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_RunningJob PerformEndJobTasks");

            //try { UnityEngine.Debug.Log("### PerformEndJobTasks RunningJob.missionJob.jobID=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID); }
            //catch { }

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobEnd_messageID_email));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobEnd_messageID_console));
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.send_message(LCARS_NCI_Mission_Archive_Tools.ToInt(LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobEnd_messageID_screen));
            
            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.progressLog.addItem(
                            LCARS_NCI_Mission_Archive_Tools.getNow(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_stepID(),
                            LCARS_NCI_Mission_Archive_Tools.get_current_jobID(),
                "Job End",
                "Your task " + LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Jobs(LCARS_NCI.Instance.Data.MissionArchive.RunningMission) + " has ended."
                );

            LCARS_NCI.Instance.Data.MissionArchive.RunningMission.job_end_reached = true;
            if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.isStepEnd)
            {
                LCARS_NCI.Instance.Data.MissionArchive.RunningMission.step_end_reached = true;
            }
        }

    }

}
