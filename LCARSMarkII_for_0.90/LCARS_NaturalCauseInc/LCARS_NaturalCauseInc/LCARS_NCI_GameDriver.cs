using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LCARSMarkII
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    internal class LCARS_NCI_GameDriver : MonoBehaviour
    {

        private void OnVesselLoaded(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselCreate(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselGoOffRails(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselTerminated(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselWasModified(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselChange(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }
        private void onVesselDestroy(Vessel data)
        {
            LCARS_NCI.Instance.Data.Update_sfs_ActiveObjectsList();
        }

        //private LCARS_NCI_Scenario ScenarioRef = null;
        public Dictionary<Guid, LCARS_NCI_Object> sfs_ActiveObjectsList = null;

        internal void Awake()
        {
            Debug.Log("NCIGD Awake begin");

            /*
             GameEvents.onVesselDestroy
             * GameEvents.onVesselChange
             * GameEvents.onVesselCreate
             * GameEvents.onVesselGoOffRails
             * GameEvents.onVesselGoOnRails
             * GameEvents.onVesselLoaded
             * GameEvents.onVesselTerminated
             * GameEvents.onVesselWasModified
             * 
             */


            DontDestroyOnLoad(this);

            Debug.Log("NCIGD Awake done");
        }

        internal void Start()
        {
            Debug.Log("NCIGD Start begin ");


            Debug.Log("NCIGD Start done ");
        }
        internal void OnDestroy()
        {
            Debug.Log("NCIGD OnDestroy begin");

            // Save Mission states and Progress logs

            Debug.Log("NCIGD OnDestroy done");
        }

        internal void OnGUI()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
            { return; }

            LCARS_NCI.Instance.GUI.Generic.OnGUI();
        }

        internal void FixedUpdate()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
            { return; }

            //Debug.Log("NCIGD FixedUpdate begin");
            pragmatic_object_initializer();

            // run background missions
            if (LCARS_NCI.Instance.Data == null)
            {
                LCARS_NCI.Instance.init();
                LCARS_NCI.Instance.Data.init();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
            }
            if (LCARS_NCI.Instance.Data.MissionArchive == null)
            {
                LCARS_NCI.Instance.Data.MissionArchive = new LCARS_NCI_Mission_Archive();
            }
            
            
            LCARS_NCI.Instance.Data.MissionArchive.ObserveBackgroundMissions();

            //NCI.Data.MissionArchive.ObserveBackgroundMissions();

            /*
            try
            {
                Debug.Log("NCIGD FixedUpdate NCI.Data.MissionArchive.RunningMission.mission.title=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.mission.title);
                Debug.Log("NCIGD FixedUpdate NCI.Data.MissionArchive.RunningMission.current_step=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentStep());
                Debug.Log("NCIGD FixedUpdate NCI.Data.MissionArchive.RunningMission.current_job=" + LCARS_NCI.Instance.Data.MissionArchive.RunningMission.get_currentJob());
            }
            catch {
                Debug.Log("NCIGD FixedUpdate NCI.Data.MissionArchive.RunningMission.mission.title= ERROR");

            }
            */
            //Debug.Log("NCIGD FixedUpdate done");
        }

        private void pragmatic_object_initializer()
        {

            // connect with NCI singleton
            if (LCARS_NCI.Instance.Data == null)
            {
                Debug.Log("NCIGD pragmatic_object_initializer NCI Data");
                LCARS_NCI.Instance.init();
                LCARS_NCI.Instance.Data.init();
            }


            // connect with scenario singleton
            /*if (ScenarioRef == null)
            {
                //Debug.Log("NCIGD pragmatic_object_initializer ScenarioRef");
                //ScenarioRef = ScenarioRunner.fetch.GetComponent<LCARS_NCI_Scenario>();
            }*/

            /*
            // scan for installed mission configs
            if (MissionArchive == null)
            {
                Debug.Log("NCIGD pragmatic_object_initializer MissionArchive");
                MissionArchive = new LCARS_NCI_Mission_Archive();
                ScanForMissions();
                Debug.Log("NCIGD pragmatic_object_initializer MissionArchive.Count=" + MissionArchive.Count);
            }
            */
            /*
            // Scan FlightGlobals file for objects
            if (NCI.Data.sfs_ActiveObjectsList == null)
            {
                Debug.Log("NCIGD pragmatic_object_initializer sfs_ActiveObjectsList");
                //Update_sfs_ActiveObjectsList();
                Debug.Log("NCIGD pragmatic_object_initializer sfs_ActiveObjectsList.Count=" + sfs_ActiveObjectsList.Count);
            }
            */
            /*
            // prepare the available mission list
            if (AvailableMissionList == null)
            {
                Debug.Log("NCIGD pragmatic_object_initializer MissionArchive");
                //AvailableMissionList = new List<LCARS_NCI_Mission>();
                ScanForMissions();
                Debug.Log("NCIGD pragmatic_object_initializer MissionArchive.Count=" + MissionArchive.Count);
            }
            */

            // prepare the egg list for mission start points, each egg will be checked each frame to see if a mission starts.

            // create the background mission list

        }
        
         
         
        
        


    }
}
