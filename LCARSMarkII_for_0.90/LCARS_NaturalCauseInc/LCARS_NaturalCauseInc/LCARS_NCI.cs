using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public sealed class LCARS_NCI
    {
        private static LCARS_NCI _instance;
        private string NCIPluginPath = "LCARS_NaturalCauseInc/";

        public static LCARS_NCI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LCARS_NCI();
                    _instance.init();
                    //_instance.Data.init();
                }

                return _instance;
            }
        }


        public LCARS_NCI_Data Data;
        public LCARS_NCI_GUI GUI;

        public Vessel thisVessel;
        //public LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            //LCARS = thisVessel.LCARS();
        }



        public void init()
        {

            if (this.Data == null)
            {
                this.Data = new LCARS_NCI_Data();
                this.Data.init();
            }
            if (this.GUI == null)
            {
                this.GUI = new LCARS_NCI_GUI();
                this.GUI.init();
            }
        }


        public Rect ClampToScreen(Rect r)
        {
            r.x = Mathf.Clamp(r.x, 0 + (r.x / 2), Screen.width - (r.width / 2));
            r.y = Mathf.Clamp(r.y, 0, Screen.height - 50);
            return r;
        }
        public string NCI_Plugin_Path()
        {
            return NCIPluginPath;
        }

    }



    public static class NCIScanGOExtensions
    {
        public static Dictionary<string, GameObject> PQSCity_Objects_closeRange = new Dictionary<string, GameObject>();
        public static Dictionary<string, GameObject> PQSCity_Objects_longRange = new Dictionary<string, GameObject>();

        /* ******************************************* */
        private static string _filterKey = "";
        //public static List<string> KSCActionPoints = new List<string>(new string[] { "KSCAdminBuilding", "KSCCrewBuilding", "FlagPole", "KSCLaunchPad", "ksp_pad_cylTank", "ksp_pad_launchPad", "ksp_pad_pipes", "ksp_pad_sphereTank", "ksp_pad_waterTower", "KSCFlagPoleLaunchPad", "KSCMissionControl", "KSCRnDFacility", "ksp_pad_cylTank", "SmallLab", "CentralBuilding", "BridgeCap_CentralSide", "CentralShed", "Bridge", "MainBuilding", "CornerLab", "ksp_pad_waterTower", "WindTunnel", "Observatory", "SideLab", "KSCRunway", "KSCSpacePlaneHangar", "Tank", "ksp_pad_cylTank", "ksp_pad_waterTower", "mainBuilding", "KSCTrackingStation", "dish_south", "dish_north", "dish_east", "MainBuilding", "KSCVehicleAssemblyBuilding", "ksp_pad_cylTank", "mainBuilding", "Pod Memorial" });
        public static Dictionary<string, GameObject> ActionPointsGameObjects = null;
        public static void FilterChildGameObjects(this UnityEngine.GameObject go,string filterKey)
        {
            if (_filterKey != filterKey)
            {
                ActionPointsGameObjects = new Dictionary<string, GameObject>();
                _filterKey = filterKey;
                Debug.Log("FilterChildGameObjects: _filterKey=" + _filterKey);
                if (_filterKey == "KSC" || _filterKey == "KSC2")
                {
                    Debug.Log("FilterChildGameObjects: TraverseHierarchy ");
                    go.TraverseHierarchy(internal_FilterChildGameObjects);
                }
            }
        }
        private static void internal_FilterChildGameObjects(GameObject go, int indent)
        {
            Debug.Log("internal_FilterChildGameObjects: _filterKey=" + _filterKey);
            List<string> _ActionPointsList = new List<string>();
            List<string> ActionPoints = null;
            switch (_filterKey)
            {
                case "KSC":
                    ActionPoints = new List<string>(new string[] { "KSCAdminBuilding", "KSCCrewBuilding", "FlagPole", "KSCLaunchPad", "ksp_pad_cylTank", "ksp_pad_launchPad", "ksp_pad_pipes", "ksp_pad_sphereTank", "ksp_pad_waterTower", "KSCFlagPoleLaunchPad", "KSCMissionControl", "KSCRnDFacility", "ksp_pad_cylTank", "SmallLab", "CentralBuilding", "BridgeCap_CentralSide", "CentralShed", "Bridge", "MainBuilding", "CornerLab", "ksp_pad_waterTower", "WindTunnel", "Observatory", "SideLab", "KSCRunway", "KSCSpacePlaneHangar", "Tank", "ksp_pad_cylTank", "ksp_pad_waterTower", "mainBuilding", "KSCTrackingStation", "dish_south", "dish_north", "dish_east", "MainBuilding", "KSCVehicleAssemblyBuilding", "ksp_pad_cylTank", "mainBuilding", "Pod Memorial" });
                    _ActionPointsList = ActionPoints;
                    break;
                case "KSC2":
                    //ActionPointsGameObjects = new Dictionary<string, GameObject>();
                    ActionPoints = new List<string>(new string[] { });
                    _ActionPointsList = ActionPoints;
                    break;
                case "WhatEver":
                    //ActionPointsGameObjects = new Dictionary<string, GameObject>();
                    ActionPoints = new List<string>(new string[] { });
                    _ActionPointsList = ActionPoints;
                    break;
            }
            
            if (_filterKey == "KSC")
            {
                //Debug.Log("internal_FilterChildGameObjects: " + go.name);
                if (_ActionPointsList.Contains(go.name) && !ActionPointsGameObjects.ContainsKey(go.name + "_" + go.GetInstanceID()))
                {
                    ActionPointsGameObjects.Add(go.name + "_" + go.GetInstanceID(), go);
                    Debug.Log("internal_FilterChildGameObjects: go.name =" + go.name + " ActionPointsGameObjects.Count=" + ActionPointsGameObjects.Count);
                }

                /*var components = go.GetComponents<PQSCity>();
                foreach (var c in components)
                {
                    Debug.Log("internal_FilterChildGameObjects: " + new string('.', indent + 3) + "c" + ": " + c.GetType().FullName);
                }*/
            }
            if (_filterKey == "KSC2")
            {
                //to do
            }
        }
        /* ******************************************* */

    }

}
