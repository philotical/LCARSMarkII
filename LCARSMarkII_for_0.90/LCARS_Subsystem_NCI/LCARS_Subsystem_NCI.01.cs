using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_NCI : ILCARSPlugin
    {
        public string subsystemName { get { return "NCI Connection"; } }
        public string subsystemDescription { get { return "Uplink with the NCI system"; } }
        public string subsystemStation { get { return "Bridge"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 0f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start






        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {
            UnityEngine.Debug.Log("### LCARS_Subsystem_NCI customCallback key=" + key + " value1=" + value1 + " value2=" + value2 + " value3=" + value3 + " value4=" + value4 + " value5=" + value5);
            switch (key)
            {
                    //"SendMessageReply", qT.Message_Object.id.ToString(), R.replyCode, R.replyID
                case"SendMessageReply":
                    UnityEngine.Debug.Log("### LCARS_Subsystem_NCI SendMessageReply   value1=" + value1 + " value2=" + value2 + " value3=" + value3);
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.processMessageReply(value1,value2,value3);
                    break;
            }
        }

        private float originalLoadDistance = 2500f;
        private float originalUnLoadDistance = 2250f;
        public void SetLoadDistance(float loadDistance = 0f, float unloadDistance = 0f)
        {
            loadDistance = (loadDistance == 0f) ? originalLoadDistance : loadDistance;
            unloadDistance = (unloadDistance == 0f) ? originalUnLoadDistance : unloadDistance;
            Vessel.loadDistance = loadDistance;
            Vessel.unloadDistance = unloadDistance;
        }
        public void getGUI()
        {
            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI begin ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("LCARS_Subsystem_NCI: ");
            GUILayout.EndHorizontal();


            GUILayout.Label("LoadDistance: " + Vessel.unloadDistance);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset"))
            {
                SetLoadDistance();
            }
            if (GUILayout.Button("20k"))
            {
                SetLoadDistance(20250f, 20000f);
            }
            if (GUILayout.Button("50k"))
            {
                SetLoadDistance(50250f, 50000f);
            }

            if (GUILayout.Button("90k"))
            {
                SetLoadDistance(90250f, 90000f);
            }
            GUILayout.EndHorizontal();


            
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 1 ");

            if (LCARS_NCI.Instance.Data.vessel != thisVessel)
            {
                GUILayout.Label("This is NOT your mission vessel!!");
                if (GUILayout.Button("Set this Vessel as your Mission Vessel"))
                {
                    LCARS_NCI.Instance.Data.vessel = thisVessel;
                }
            }
            else
            {
                GUILayout.Label("This is your mission vessel.");
            }
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 2 ");

            if (LCARS_NCI.Instance.Data.kerbal == "" || LCARS_NCI.Instance.Data.kerbal == null)
            {
                GUILayout.Label("There is no mission Kerbal defined");
            }
            else
            {
                GUILayout.Label("There is a mission Kerbal defined");
            }

            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 3 ");



            try
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.RunningMission != null)
                {
                    GUILayout.Label("RunningMission title: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
                    //GUILayout.Label("RunningMission missionStep.id: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.id);
                    //GUILayout.Label("RunningMission missionJob.jobID: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobID);
                    GUILayout.Label("RunningMission current_step: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep());
                    GUILayout.Label("RunningMission current_job: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob());
                    GUILayout.Label("Task: " +
                        LCARS_NCI_Mission_Archive_Tools.TaskStringConstructor_Jobs(LCARS_NCI.Instance.Data.MissionArchive.RunningMission)
                        /*
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.jobtype + " than " +
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.distance + " to " +
                        LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningJob.missionJob.NCI_object
                        */
                        );
                    GUILayout.Label("RunningMission check Step Condition: " + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.missionStep.checkCondition());
                }
                else 
                {
                }
            }
            catch (Exception ex) { GUILayout.Label("Currently, there is no running Mission.");} // + ex); }

            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 4 ");

            if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList!=null)
            {
                foreach (KeyValuePair<Guid, LCARS_NCI_Object> pair in LCARS_NCI.Instance.Data.sfs_ActiveObjectsList)
                {
                    try
                    {
                        GUILayout.Label("Object in List: " + pair.Value.partname + " " + pair.Key);
                    }
                    catch (Exception ex) { GUILayout.Label("2)" + ex); }
                    try
                    {
                        if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack == null)
                        {
                            GUILayout.Label("Next Mission to come: No Missions for this Object");
                        }
                        else
                        {
                            GUILayout.Label("Next Mission to come: " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.First.Value);
                            GUILayout.Label("Last Mission played: " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList[pair.Key].missionStack.Last.Value);
                            GUILayout.Label(" ");
                        }
                    }
                    catch (Exception ex) { GUILayout.Label("3)" + ex); }
                }
            }

            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 4b ");

            try
            {
                if (LCARS_NCI.Instance.Data.sfs_ActiveObjectsList != null)
                {
                    GUILayout.Label("There are " + LCARS_NCI.Instance.Data.sfs_ActiveObjectsList.Count + " Objects in the sfs_ActiveObjectsList");
                }
            }
            catch  { }
            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 5 ");

            try
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions != null)
                {
                    GUILayout.Label("There are " + LCARS_NCI.Instance.Data.MissionArchive.playable_backgroundMissions.Count + " Objects in the playable_backgroundMissions");
                }
            }
            catch { }
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 6 ");

            try
            {
                if (LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions != null)
                {
                    GUILayout.Label("There are " + LCARS_NCI.Instance.Data.MissionArchive.playable_UserMissions.Count + " Objects in the playable_UserMissions");
                }
            }catch{}
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 7 ");





            /*
            if (LCARS_NCI.Instance.Data.Naturals_List != null)
            {
                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Naturals_List.Count + " Objects in the Naturals_List");
            }
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 8 ");

            if (LCARS_NCI.Instance.Data.Mines_List != null)
            {
                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Mines_List.Count + " Objects in the Mines_List");
            }
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI 9 ");

            if (LCARS_NCI.Instance.Data.Stations_List != null)
            {
                GUILayout.Label("There are " + LCARS_NCI.Instance.Data.Stations_List.Count + " Objects in the Stations_List");
            }
            */

            UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI done ");
        }

    }

}
