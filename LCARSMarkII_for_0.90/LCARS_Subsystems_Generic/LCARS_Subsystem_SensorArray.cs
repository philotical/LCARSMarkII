using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public delegate void GameObjectVisitor(GameObject go, int indent);

    public static class ScanGOExtensions
    {
        public static Dictionary<string, GameObject> PQSCity_Objects_closeRange = new Dictionary<string, GameObject>();
        public static Dictionary<string, GameObject> PQSCity_Objects_longRange = new Dictionary<string, GameObject>();
        
        private static void internal_PrintComponents(GameObject go, int indent)
        {
            Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " has components:");

            var components = go.GetComponents<PQSCity>();
            foreach (var c in components)
            {
                Debug.Log(new string('.', indent + 3) + "c" + ": " + c.GetType().FullName);
                if (c.GetType().FullName == "PQSCity")
                {
                    //Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " has component PQSCity");
                    if (Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position) <= 5000f)
                    {
                        if (!PQSCity_Objects_closeRange.ContainsKey(go.name + "_" + go.GetInstanceID()))
                        {
                            Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " PQSCity_Objects_closeRange");
                            PQSCity_Objects_closeRange.Add(go.name + "_" + go.GetInstanceID(), go);
                        }
                    }
                    else
                    {
                        if (!PQSCity_Objects_longRange.ContainsKey(go.name + "_" + go.GetInstanceID()))
                        {
                            Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " PQSCity_Objects_longRange");
                            PQSCity_Objects_longRange.Add(go.name + "_" + go.GetInstanceID(), go);
                        }
                    }
                }
            }
        }

        public static void PrintComponents(this UnityEngine.GameObject go)
        {
            if (PQSCity_Objects_closeRange == null)
            {
                PQSCity_Objects_closeRange = new Dictionary<string, GameObject>();
            }
            if (PQSCity_Objects_longRange == null)
            {
                PQSCity_Objects_longRange = new Dictionary<string, GameObject>();
            }
            go.TraverseHierarchy(internal_PrintComponents);
        }
        public static void TraverseHierarchy(this UnityEngine.GameObject go, GameObjectVisitor visitor, int indent = 0)
        {
            visitor(go, indent);

            for (int i = 0; i < go.transform.childCount; ++i)
                go.transform.GetChild(i).gameObject.TraverseHierarchy(visitor, indent + 3);
        }

        /* ******************************************* */
        private static string _filterKey = "";
        public static List<string> KSCActionPoints = new List<string>(new string[] { "SpaceCenter", "monolith00", "Administration", "mainBuilding", "AstronautComplex", "FlagPole", "LaunchPad", "ksp_pad_pipes", "ksp_pad_sphereTank", "ksp_pad_waterTower", "KSCFlagPoleLaunchPad", "MissionControl", "ResearchAndDevelopment", "SmallLab", "CentralBuilding", "Bridge", "MainBuilding", "CornerLab", "WindTunnel", "Observatory", "SideLab", "Runway", "SpaceplaneHangar", "Tank", "TrackingStation", "dish_south", "dish_north", "dish_east", "MainBuilding", "VehicleAssemblyBuilding", "Pod Memorial", "Crawlerway" });
        // old // public static List<string> KSCActionPoints = new List<string>(new string[] { "KSCAdminBuilding","AstronautComplex", "KSCCrewBuilding", "FlagPole", "KSCLaunchPad", "ksp_pad_cylTank", "ksp_pad_launchPad", "ksp_pad_pipes", "ksp_pad_sphereTank", "ksp_pad_waterTower", "KSCFlagPoleLaunchPad", "KSCMissionControl", "KSCRnDFacility", "ksp_pad_cylTank", "SmallLab", "CentralBuilding", "BridgeCap_CentralSide", "CentralShed", "Bridge", "MainBuilding", "CornerLab", "ksp_pad_waterTower", "WindTunnel", "Observatory", "SideLab", "KSCRunway", "KSCSpacePlaneHangar", "Tank", "ksp_pad_cylTank", "ksp_pad_waterTower", "mainBuilding", "KSCTrackingStation", "dish_south", "dish_north", "dish_east", "MainBuilding", "KSCVehicleAssemblyBuilding", "ksp_pad_cylTank", "mainBuilding", "Pod Memorial" });
        public static Dictionary<string, GameObject> KSCActionPointsGameObjects = null;
        public static void FilterChildGameObjects(this UnityEngine.GameObject go,string filterKey)
        {
            _filterKey = filterKey;
            Debug.Log("FilterChildGameObjects: _filterKey=" + _filterKey);
            Debug.Log("FilterChildGameObjects: TraverseHierarchy ");
            go.TraverseHierarchy(internal_FilterChildGameObjects);
        }
        private static void internal_FilterChildGameObjects(GameObject go, int indent)
        {
            Debug.Log("internal_FilterChildGameObjects: _filterKey=" + _filterKey);
            if (_filterKey == "KSC")
            {
                Debug.Log("internal_FilterChildGameObjects: " + (indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " has components:");
                if (KSCActionPoints.Contains(go.name) && !KSCActionPointsGameObjects.ContainsKey(go.name + "_" + go.GetInstanceID()))
                {
                    KSCActionPointsGameObjects.Add(go.name + "_" + go.GetInstanceID(), go);
                    Debug.Log("internal_FilterChildGameObjects: go.name =" + go.name + " KSCActionPoints.Count=" + KSCActionPoints.Count);
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

    internal class tmp_distance_array
    {
        public GameObject SelectedTargetGameObject { get; set; }
        public Vessel SelectedTargetShip { get; set; }
        public string ScanType { get; set; }
        public int BodyScanMode { get; set; }
        public double distance { get; set; }
    }


    public class LCARS_Subsystem_SensorArray : ILCARSPlugin
    {
        public string subsystemName { get { return "Sensor Array"; } }
        public string subsystemDescription {get{return "Scientiffic Scanner";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 200f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 1500f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return true; } }
        public bool subsystemPanelState { get; set; } // has to be false at start

        

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        GUIStyle scrollview_style1;
        Vector2 ScrollPosition1;
        GUIStyle scrollview_style2;
        Vector2 ScrollPosition2;
        GUIStyle scrollview_style3;
        Vector2 ScrollPosition3;
        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        private float lastBodyScan = 0.0f;
        private float lastVesselScan = 0.0f;
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 10.0f;
        private float BodyScanInterval = 0.5f;
        private float VesselScanInterval = 0.5f;
        public string[] selStrings;
        public int selGridInt = 0;
        public string[] selStrings2;
        public int selGridInt2 = 0;
        public string[] selStrings3;
        public int selGridInt3 = 0;
        string ScanType = null;
        int ShipScanMode = 0;
        int BodyScanMode = 0;
        Vessel SelectedTargetShip;
        GameObject SelectedTargetGameObject;
        Texture2D BiomeTexture = null;
        bool ScanReport_created = false;
        LCARS_ScanReportItem_Type ScanReport_Item = null;
        Dictionary<string, tmp_distance_array> tmp_array = null;
        public void getGUI()
        {
            if (scrollview_style1 == null)
            {
                scrollview_style1 = new GUIStyle();
                scrollview_style1.fixedHeight = 255;
            }
            if (scrollview_style2 == null)
            {
                scrollview_style2 = new GUIStyle();
                scrollview_style2.fixedHeight = 255;
            }
            if (scrollview_style3 == null)
            {
                scrollview_style3 = new GUIStyle();
                scrollview_style3.fixedHeight = 255;
            }
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            if ((Time.time - lastFixedUpdate) > logInterval)
            {
                lastFixedUpdate = Time.time;

                ScanGOExtensions.PQSCity_Objects_closeRange = new Dictionary<string, GameObject>();
                ScanGOExtensions.PQSCity_Objects_longRange = new Dictionary<string, GameObject>();
                ScanGOExtensions.PrintComponents(thisVessel.mainBody.gameObject);
            }

            selStrings = new string[] { "StandBy", "Vessel Scan", "Body Scan" };
            GUILayout.BeginHorizontal();
            selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 3);
            GUILayout.EndHorizontal();
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();
            //GUILayout.Label("ToDo: " + thisVessel.vesselName);


            GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(445));
            ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

            if(ScanType != null)
            {
                if (ScanReport_Item==null)
                {
                    ScanReport_Item = new LCARS_ScanReportItem_Type();
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Back"))
                {
                    SelectedTargetShip = null;
                    ScanType = null;
                    ScanReport_Item = null;
                    ShipScanMode = 0;
                    BodyScanMode = 0;
                    SelectedTargetGameObject = null;
                    objectConf = "";
                    shipConf = "";
                    selGridInt = 0;
                    selGridInt2 = 0;
                    selGridInt3 = 0;
                    ScanGOExtensions.KSCActionPointsGameObjects = null;
                }
                if (GUILayout.Button("Save Scan Results"))
                {
                    if(ScanReport_Item.scan_level=="biome")
                    {
                        //string name = HighLogic.SaveFolder + "_" + ScanReport_Item.body_name + "_biome";
                        //ScanReport_Item.body_biom_map = LCARS_Utilities.SavePNG(name, FlightGlobals.currentMainBody.BiomeMap.Map);
                    }
                    LCARS.lODN.ScanReport.list.Add(ScanReport_Item);
                    try
                    {
                        LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog("The Object " + ScanReport_Item.idcode + " was scanned!", "You've made a scan at a distance of " + ScanReport_Item.distance + " meter with the resolution level: " + ScanReport_Item.scan_level);
                    }
                    catch
                    {
                        UnityEngine.Debug.Log("### LCARS_SensorArray Save Scan Results: LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog failed ");
                        UnityEngine.Debug.Log("### LCARS_SensorArray Save Scan Results: <ScanReport_Item.idcode=" + ScanReport_Item.idcode + " ScanReport_Item.distance=" + ScanReport_Item.distance + " ScanReport_Item.scan_level=" + ScanReport_Item.scan_level + ">");
                    }
                }
                GUILayout.EndHorizontal();
                switch (ScanType)
                {
                    case"ship":
                        switch (ShipScanMode)
                        {

                            default: //case 1
                                //sub_VPI = new LCARS_VesselPartsInventory();
                                //sub_VPI.init(SelectedTargetShip);
                                //sub_VPI.scanVessel();

                                GUILayout.Label("Standard Scan Results");
                                GUI_Ship_Scan_Level1(SelectedTargetShip);
                                LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Sensor Array"].name, LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L1_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L2_usage);

                                if (GUILayout.Button("Perform a Tactical Scan"))
                                {
                                    ShipScanMode = 2;
                                }
                                break;

                            case 2:
                                GUILayout.Label("Tactical Scan Results");
                                GUI_Ship_Scan_Level1(SelectedTargetShip);
                                GUI_Ship_Scan_Level2(SelectedTargetShip);
                                LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Sensor Array"].name, LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L1_usage + (LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L2_usage * 3));

                                    if (GUILayout.Button("Scan the Interior"))
                                    {
                                        ShipScanMode = 3;
                                    }
                                ScanReport_Item.target_type = "ship";
                                ScanReport_Item.idcode = SelectedTargetShip.vesselName;
                                ScanReport_Item.guid = SelectedTargetShip.id;
                                ScanReport_Item.time = DateTime.Now.ToString();
                                ScanReport_Item.scan_level = "2";
                                if (ScanReport_Item.distance<0.001f)
                                {
                                    ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, SelectedTargetShip.CoM);
                                }
                                break;

                            case 3:
                                GUILayout.Label("Interior Scan Results");
                                GUI_Ship_Scan_Level1(SelectedTargetShip);
                                GUI_Ship_Scan_Level2(SelectedTargetShip);
                                GUI_Ship_Scan_Level3(SelectedTargetShip);
                                LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Sensor Array"].name, LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L1_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L2_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L3_usage);
                                ScanReport_Item.target_type = "ship";
                                ScanReport_Item.idcode = SelectedTargetShip.vesselName;
                                ScanReport_Item.guid = SelectedTargetShip.id;
                                ScanReport_Item.time = DateTime.Now.ToString();
                                ScanReport_Item.scan_level = "3";
                                if (ScanReport_Item.distance<0.001f)
                                {
                                    ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, SelectedTargetShip.CoM);
                                }
                                try
                                {
                                    ScanReport_Item.NCIactionpoints = LCARSNCI_Bridge.Instance.get_ActionPoints_From_sfs_ActiveObject(ScanReport_Item.guid);
                                }
                                catch{}
                                break;
                        }
                        break;
                
                    case"body":
/*
    public class LCARS_ScanReportItem_Type
    {
        public string target_type { set; get; }
        public string idcode { set; get; }
        public Guid guid { set; get; }
        public string time { set; get; }

        public string scan_level { set; get; }
        public float distance { set; get; }

        public string body_name { set; get; }
        public string body_biom_map { set; get; }
        public string body_surface_map { set; get; }

        public List<string> NCIactionpoints { set; get; }

    }
 */                                
                        switch (BodyScanMode)
                        {
                            default: // reset
                                selGridInt = 0;
                                break;
                            case 1: // gameobject
                                GUILayout.Label("Surface Object Scan Results");
                                GUI_GameObject_Scan(SelectedTargetGameObject);
                                LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Sensor Array"].name, LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L1_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L2_usage);
                               break;
                            case 2: // Biome
                                GUILayout.Label("Biome Scan Results");
                                GUILayout.BeginVertical();
                                //CelestialBody.BiomeMap.Map;
                                //BiomeTexture = FlightGlobals.currentMainBody.BiomeMap.Map;
                                //GUIContent content = new GUIContent(BiomeTexture, "");
                                //GUILayout.Box(content, GUILayout.Height(260), GUILayout.Width(260));
                                GUILayout.EndVertical();

                                ScanReport_Item.target_type = "body";
                                ScanReport_Item.idcode = FlightGlobals.currentMainBody.name;
                                ScanReport_Item.body_name = FlightGlobals.currentMainBody.name;
                                ScanReport_Item.time = DateTime.Now.ToString();
                                ScanReport_Item.scan_level = "biome";
                                ScanReport_Item.gameobject = FlightGlobals.currentMainBody.gameObject;
                                if (ScanReport_Item.distance<0.001f)
                                {
                                    ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, FlightGlobals.currentMainBody.transform.position);
                                }
                                try
                                {
                                    ScanReport_Item.body_biom_map = "todo";
                                }
                                catch{}
                               break;
                             case 3: // surface

                                ScanReport_Item.target_type = "body";
                                ScanReport_Item.idcode = FlightGlobals.currentMainBody.name;
                                ScanReport_Item.body_name = FlightGlobals.currentMainBody.name;
                                ScanReport_Item.time = DateTime.Now.ToString();
                                ScanReport_Item.scan_level = "surface";
                                ScanReport_Item.gameobject = FlightGlobals.currentMainBody.gameObject;
                                if (ScanReport_Item.distance<0.001f)
                                {
                                    ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, FlightGlobals.currentMainBody.transform.position);
                                }
                                try
                                {
                                    ScanReport_Item.body_surface_map = "todo";
                                }
                                catch{}
                               break;
                        }
                        
                        break;
                
                }

            }
            else
            {

                switch (selGridInt)
                {
                    case 0: //StandBy


                        break;

                    case 1: //Vessel
                        // Generate and sort an array of vessels by distance.
                        LCARS.grav.vesselList = new List<Vessel>(FlightGlobals.Vessels);
                        var vdc = new VesselDistanceComparer();
                        vdc.OriginVessel = thisVessel;
                        LCARS.grav.vesselList.Sort(vdc);

                        selStrings2 = new string[] { "Loaded", "SOI", "LongRange" };
                        GUILayout.BeginHorizontal();
                        selGridInt2 = GUILayout.SelectionGrid(selGridInt2, selStrings2, 3);
                        GUILayout.EndHorizontal();
                        switch (selGridInt2)
                        {
                            case 0: //Loaded
                                GUILayout.Label("Loaded Vessels: ");
                                for (int i = 0; i < LCARS.grav.vesselList.Count; i++)
                                {
                                    // Skip ourselves.
                                    if (LCARS.grav.vesselList[i] == thisVessel)
                                        continue;

                                    if (!LCARS.grav.vesselList[i].loaded)
                                    continue;

                                    // Skip stuff around other worlds.
                                    if (thisVessel.orbit.referenceBody != LCARS.grav.vesselList[i].orbit.referenceBody)
                                        continue;

                                    // Calculate the distance.
                                    float d = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                                    if (GUILayout.Button((d / 1000).ToString("F1") + "km " + LCARS.grav.vesselList[i].vesselName, GUILayout.ExpandWidth(true)))
                                    {
                                        //Mode = UIMode.SELECTED;
                                        //LCARS.grav.selectedVesselInstanceId = LCARS.grav.vesselList[i].GetInstanceID();
                                        //LCARS.grav.selectedVesselIndex = FlightGlobals.Vessels.IndexOf(LCARS.grav.vesselList[i]);
                                        //FlightGlobals.fetch.SetVesselTarget(LCARS.grav.vesselList[i]);
                                        SelectedTargetShip = LCARS.grav.vesselList[i];
                                        ScanType = "ship";
                                    }
                                }
                                break;
                            case 1: //CloseRange
                                GUILayout.Label("SOI Scan: ");
                                if ((Time.time - lastVesselScan) > VesselScanInterval || tmp_array == null)
                                {
                                    lastVesselScan = Time.time;
                                    tmp_array = new Dictionary<string, tmp_distance_array>();

                                    for (int i = 0; i < LCARS.grav.vesselList.Count; i++)
                                    {
                                        // Skip ourselves.
                                        if (LCARS.grav.vesselList[i] == thisVessel)
                                            continue;

                                        //if (LCARS.grav.vesselList[i].LandedOrSplashed)
                                        //continue;

                                        // Skip stuff around other worlds.
                                        if (thisVessel.orbit.referenceBody != LCARS.grav.vesselList[i].orbit.referenceBody)
                                            continue;

                                        // Calculate the distance.
                                        float dist = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                                        tmp_distance_array foo = new tmp_distance_array();
                                        foo.BodyScanMode = 1;
                                        foo.distance = dist;
                                        foo.ScanType = "ship";
                                        foo.SelectedTargetShip = LCARS.grav.vesselList[i];

                                        tmp_array.Add(LCARS.grav.vesselList[i].vesselName, foo);
                                    }
                                }
                                foreach (KeyValuePair<string, tmp_distance_array> pair in tmp_array)
                                {
                                    tmp_distance_array foo = pair.Value;
                                    if (GUILayout.Button((foo.distance / 1000).ToString("F1") + "km " + pair.Key, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetShip = foo.SelectedTargetShip;
                                        ScanType = foo.ScanType;
                                    }
                                }
                                break;
                            case 2: //LongRange
                                GUILayout.Label("longRange Scan: ");
                                if ((Time.time - lastVesselScan) > VesselScanInterval || tmp_array == null)
                                {
                                    lastVesselScan = Time.time;
                                    tmp_array = new Dictionary<string, tmp_distance_array>();

                                    for (int i = 0; i < LCARS.grav.vesselList.Count; i++)
                                    {
                                        // Skip ourselves.
                                        if (LCARS.grav.vesselList[i] == thisVessel)
                                            continue;

                                        //if (LCARS.grav.vesselList[i].LandedOrSplashed)
                                        //continue;

                                        // Skip stuff around this world.
                                        if (thisVessel.orbit.referenceBody == LCARS.grav.vesselList[i].orbit.referenceBody)
                                            continue;

                                        // Calculate the distance.
                                        float dist = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                                        tmp_distance_array foo = new tmp_distance_array();
                                        foo.BodyScanMode = 1;
                                        foo.distance = dist;
                                        foo.ScanType = "ship";
                                        foo.SelectedTargetShip = LCARS.grav.vesselList[i];

                                        tmp_array.Add(LCARS.grav.vesselList[i].vesselName, foo);
                                    }
                                }
                                foreach (KeyValuePair<string, tmp_distance_array> pair in tmp_array)
                                {
                                    tmp_distance_array foo = pair.Value;
                                    if (GUILayout.Button((foo.distance / 1000).ToString("F1") + "km " + pair.Key, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetShip = foo.SelectedTargetShip;
                                        ScanType = foo.ScanType;
                                    }
                                }
                                break;
                        }


                        break;

                    case 2: //Body
                        selStrings3 = new string[] { "closeRange", "longRange", "Biome", "Surface" };
                        GUILayout.BeginHorizontal();
                        selGridInt3 = GUILayout.SelectionGrid(selGridInt3, selStrings3, 4);
                        GUILayout.EndHorizontal();
                        switch (selGridInt3)
                        {
                            case 0: //closeRange
                                GUILayout.Label("closeRange Objects: ");
                                if ((Time.time - lastBodyScan) > BodyScanInterval || tmp_array == null)
                                {
                                    lastBodyScan = Time.time;
                                    tmp_array = new Dictionary<string, tmp_distance_array>();
                                    foreach (GameObject go in ScanGOExtensions.PQSCity_Objects_closeRange.Values)
                                    {
                                        double dist = Math.Round(Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position), 2);
                                        //if (dist >= 5000f) { continue; }

                                        tmp_distance_array foo = new tmp_distance_array();
                                        foo.BodyScanMode = 1;
                                        foo.distance = dist;
                                        foo.ScanType = "body";
                                        foo.SelectedTargetGameObject = go;

                                        tmp_array.Add(go.name, foo);
                                    }
                                }
                                foreach (KeyValuePair<string, tmp_distance_array> pair in tmp_array)
                                {
                                    tmp_distance_array foo = pair.Value;
                                    if (GUILayout.Button("Name: " + pair.Key + " Distance: " + foo.distance, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetGameObject = foo.SelectedTargetGameObject;
                                        ScanType = foo.ScanType;
                                        BodyScanMode = foo.BodyScanMode;
                                    }
                                }
                                break;
                            case 1: //longRange
                                GUILayout.Label("longRange Objects: ");
                                if ((Time.time - lastBodyScan) > BodyScanInterval || tmp_array == null)
                                {
                                    lastBodyScan = Time.time;
                                    tmp_array = new Dictionary<string, tmp_distance_array>();
                                    foreach (GameObject go in ScanGOExtensions.PQSCity_Objects_longRange.Values)
                                    {
                                        double dist = Math.Round(Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position), 2);
                                        //if (dist < 5000f) { continue; }

                                        tmp_distance_array foo = new tmp_distance_array();
                                        foo.BodyScanMode = 1;
                                        foo.distance = dist;
                                        foo.ScanType = "body";
                                        foo.SelectedTargetGameObject = go;

                                        tmp_array.Add(go.name, foo);
                                    }
                                }
                                foreach (KeyValuePair<string, tmp_distance_array> pair in tmp_array)
                                {
                                    tmp_distance_array foo = pair.Value;
                                    if (GUILayout.Button("Name: " + pair.Key + " Distance: " + foo.distance, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetGameObject = foo.SelectedTargetGameObject;
                                        ScanType = foo.ScanType;
                                        BodyScanMode = foo.BodyScanMode;
                                    }
                                }
                                break;
                            case 2: //Biom
                                GUILayout.Label("Biome: todo");
                                        ScanType = "body";
                                        BodyScanMode = 2;
                                break;
                            case 3: //Surface
                                GUILayout.Label("Surface: todo");
                                        ScanType = "body";
                                        BodyScanMode = 3;
                                break;
                        }

                        break;
                }


            }











            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            //PQSCity

            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

        string objectConf = "";

        private void GUI_GameObject_Scan(GameObject SelectedTargetGameObject)
        {
            if (objectConf == "")
            {
                try
                {
                    objectConf = determine_object_configuration(SelectedTargetGameObject);
                }
                catch (Exception ex) { objectConf = "scan error"; UnityEngine.Debug.Log("LCARS_SensorArray: GUI_GameObject_Scan objectConf scan error ex=" + ex); }
            }

            ScanReport_Item.target_type = "body";
            ScanReport_Item.idcode = SelectedTargetGameObject.name;
            ScanReport_Item.description = objectConf;
            ScanReport_Item.time = DateTime.Now.ToString();
            ScanReport_Item.scan_level = "structure";
            ScanReport_Item.gameobject = SelectedTargetGameObject;
            if (ScanReport_Item.distance < 0.001f)
            {
                ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, SelectedTargetGameObject.transform.position);
            }

            
            
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(); GUILayout.Label("Name: "); GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Label("" + SelectedTargetGameObject.name); GUILayout.EndHorizontal();
            GUILayout.Label("Long: " + "todo" + " ");
            GUILayout.Label("Alt: " + "todo" + " ");
            GUILayout.Label("Description: " + objectConf);
            if (GUILayout.Button("Set as Target (Experimental)"))
            {
                /*
                Vessel v = null;
                 try
                 {
                     v = SelectedTargetGameObject.GetComponent<Vessel>();
                 }
                 catch { }
                 if (v==null)
                 {
                     v = SelectedTargetGameObject.AddComponent<Vessel>();
                     v.vesselType = VesselType.Flag;
                     v.transform.position = SelectedTargetGameObject.transform.position;
                     v.vesselName = objectConf;
                     v.gameObject.SetActive(true);
                     v.Landed = true;
                 }
                 if(v!=null)
                 {
                     FlightGlobals.fetch.SetVesselTarget(v);
                 }
                 */







                    UnityEngine.Debug.Log("Set as Target: 1 ");
                    AvailablePart avPart = PartLoader.getPartInfoByName("flag");
                    Part obj = (Part)UnityEngine.Object.Instantiate((UnityEngine.Object)avPart.partPrefab);
                    UnityEngine.Debug.Log("Set as Target: 2 ");
                    if (!obj)
                    {
                        UnityEngine.Debug.Log("Set as Target Failed to instantiate ");// + avPart.partPrefab.name);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Set as Target: 3 ");

                        Part newPart = obj;
                        newPart.gameObject.SetActive(true);
                        newPart.enabled = true;
                        //newPart.gameObject.name = "LCARS_NaturalCauseInc_Part_" + SelectedPart + "_at_" + SelectedBody.name;
                        newPart.gameObject.name = SelectedTargetGameObject.name;
                        newPart.partInfo = avPart;
                        newPart.partName = SelectedTargetGameObject.name;
                        UnityEngine.Debug.Log("Set as Target: 4 ");
                        ShipConstruct newShip = new ShipConstruct();
                        newShip.Add(newPart);

                        //newShip.Add(FlightGlobals.ActiveVessel.rootPart);

                        newShip.SaveShip();
                        newShip.shipName = avPart.title;
                        //newShip.shipType = 1;
                        UnityEngine.Debug.Log("Set as Target: 5 ");

                        VesselCrewManifest vessCrewManifest = new VesselCrewManifest();
                        Vessel currentVessel = FlightGlobals.ActiveVessel;
                        UnityEngine.Debug.Log("Set as Target: 6 ");

                        UnityEngine.Debug.Log("Set as Target: Vessel v ");
                        Vessel v = newShip.parts[0].localRoot.gameObject.AddComponent<Vessel>();
                        v.id = Guid.NewGuid();
                        v.vesselName = "LCARS-Target: "+SelectedTargetGameObject.name;
                        v.gameObject.SetActive(true);
                        v.gameObject.AddComponent<Part>();
                        v.parts = new List<Part>();
                        UnityEngine.Debug.Log("Set as Target: v.parts.Add ");
                        v.parts.Add(newShip.parts[0]);
                        v.rootPart = newPart;
                        v.rootPart.partInfo = newPart.partInfo;
                        v.rootPart.partName = SelectedTargetGameObject.name;
                        v.rootPart.name = SelectedTargetGameObject.name;
                        v.vesselType = VesselType.Flag;
                        try
                        {
                            v.Initialize(false);
                        }
                        catch { }
                        v.Landed = true;
                        UnityEngine.Debug.Log("Set as Target: v.parts ");
                        //v.parts[0] = newPart;
                        UnityEngine.Debug.Log("Set as Target: v.rootPart ");
                        UnityEngine.Debug.Log("Set as Target: v.rootPart.name=" + v.rootPart.name);
                        v.rootPart.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                        v.rootPart.missionID = (uint)Guid.NewGuid().GetHashCode();
                        UnityEngine.Debug.Log("Set as Target: 6 ");
                        UnityEngine.Debug.Log("Set as Target: v.rootPart.missionID=" + v.rootPart.missionID);

                        v.orbitDriver = new OrbitDriver();
                        v.orbitDriver.orbit = new Orbit();
                        v.flightIntegrator = new FlightIntegrator();

                        v.orbitDriver.orbit = Orbit.CreateRandomOrbitAround(FlightGlobals.currentMainBody, 0, 0);


                        v.SetPosition(SelectedTargetGameObject.transform.position);

                        v.SetWorldVelocity(Vector3.zero);
                        UnityEngine.Debug.Log("Set as Target: obt_velocity=" + v.obt_velocity);

                        v.orbitDriver.orbit.semiMajorAxis = Double.NaN;
                        v.orbitDriver.orbit.inclination = Double.NaN;

                        v.rootPart.maxTemp = 3000000;
                        v.rootPart.crashTolerance = 3000000;

                        v.situation = Vessel.Situations.LANDED;
                        v.Landed = true;
                        v.landedAt = SelectedTargetGameObject.name;
                        
                        v.orbit.Init();
                        v.orbit.UpdateFromUT(Planetarium.GetUniversalTime());

                        if (v != null)
                        {
                            FlightGlobals.fetch.SetVesselTarget(v);
                        }
                    }

            }



            GUILayout.BeginVertical();
            GUILayout.Label("Internal Locations:");
            if (SelectedTargetGameObject.name.Contains("KSC"))
            {
            }

                if (ScanGOExtensions.KSCActionPointsGameObjects == null)
                {
                    ScanGOExtensions.KSCActionPointsGameObjects = new Dictionary<string, GameObject>();
                    ScanGOExtensions.FilterChildGameObjects(thisVessel.mainBody.gameObject, SelectedTargetGameObject.name);
                }

                foreach (KeyValuePair<string, GameObject> pair in ScanGOExtensions.KSCActionPointsGameObjects)
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Location: " + pair.Value.name);
                    if (GUILayout.Button("Send to IFD"))
                    {
                        UnityEngine.Debug.Log("GUI_GameObject_Scan Send to IFD: thisVessel.LCARS().lODN.IFD_object_of_interest=" + thisVessel.LCARS().lODN.IFD_object_of_interest);
                        UnityEngine.Debug.Log("GUI_GameObject_Scan Send to IFD: pair.Value.name=" + pair.Value.name);
                        thisVessel.LCARS().lODN.IFD_object_of_interest = pair.Value;
                        UnityEngine.Debug.Log("GUI_GameObject_Scan Send to IFD: thisVessel.LCARS().lODN.IFD_object_of_interest=" + thisVessel.LCARS().lODN.IFD_object_of_interest);
                    }
                    GUILayout.EndHorizontal();

                }

            GUILayout.EndVertical();
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
            GUILayout.EndVertical();
        }
        private string determine_object_configuration(GameObject SelectedTargetGameObject)
        {
            Debug.LogError("LCARS_SensorArray: determine_object_configuration SelectedTargetGameObject.name=" + SelectedTargetGameObject.name);
            Dictionary<string, string> PLRL = LCARS_Utilities.init_PlanetaryLocationsRecognizer();
            foreach (KeyValuePair<string, string> pair in LCARS_Utilities.PlanetaryLocationsRecognizer_list)
            {
                if (SelectedTargetGameObject.name.Contains(pair.Key))
                {
                    Debug.LogError("LCARS_SensorArray: determine_object_configuration pair.Key=" + pair.Key);
                    Debug.LogError("LCARS_SensorArray: determine_object_configuration SelectedTargetGameObject.name=" + SelectedTargetGameObject.name);
                    return pair.Value;
                }
            }
            return "It's a ship of unknown configuration";
        }

        string shipConf = "";
        private void GUI_Ship_Scan_Level1(Vessel TargetShip_1)
        {
            if (shipConf == "")
            {
                try
                {
                    shipConf = determine_ship_configuration(TargetShip_1);
                }
                catch (Exception ex) { shipConf = "scan error"; UnityEngine.Debug.Log("LCARS_SensorArray: determine_object_configuration shipConf scan error ex=" + ex); }
            }

            ScanReport_Item.target_type = "ship";
            ScanReport_Item.idcode = SelectedTargetShip.vesselName;
            ScanReport_Item.guid = SelectedTargetShip.id;
            ScanReport_Item.description = objectConf;
            ScanReport_Item.time = DateTime.Now.ToString();
            ScanReport_Item.scan_level = "1";
            ScanReport_Item.gameobject = SelectedTargetShip.gameObject;
            if (ScanReport_Item.distance < 0.001f)
            {
                ScanReport_Item.distance = Vector3.Distance(LCARS.vessel.CoM, SelectedTargetShip.CoM);
            }

            GUILayout.BeginVertical();
            GUILayout.Label("Name: " + TargetShip_1.vesselName);
            GUILayout.Label("Distance: " + Math.Round(Vector3.Distance(TargetShip_1.transform.position, thisVessel.transform.position),2)+" meter");
            if (GUILayout.Button("Set as Target"))
            {
                FlightGlobals.fetch.SetVesselTarget(TargetShip_1);
            }
            GUILayout.Label("in SOI of: " + TargetShip_1.mainBody + "");
            GUILayout.Label("Mass: " + Math.Round(TargetShip_1.GetTotalMass(), 2) + " t");
            GUILayout.Label("DryMass: " + Math.Round(thisVessel.LCARSVessel_DryMass(), 2) + " t");
            GUILayout.Label("Parts: " + TargetShip_1.parts.Count);
            GUILayout.Label("Ship Configuration: " + shipConf);
            GUILayout.EndVertical();
        }
        private string determine_ship_configuration(Vessel TargetShip_1)
        {
            Debug.Log("LCARS_SensorArray: determine_ship_configuration TargetShip_1.vesselName=" + TargetShip_1.vesselName);
            //Dictionary<string, string> VTRL = LCARS_Utilities.init_VesselTypeRecognizer();
            LCARS_Utilities.VesselTypeRecognizer("init");
            try
            {
                foreach (ProtoPartSnapshot p in TargetShip_1.protoVessel.protoPartSnapshots)
                {
                    foreach (KeyValuePair<string, string> pair in LCARS_Utilities.VesselTypeRecognizer_list)
                    {
                        Debug.Log("LCARS_SensorArray: determine_ship_configuration p.partRef.partName=" + p.partRef.partName);
                        Debug.Log("LCARS_SensorArray: determine_ship_configuration p.partRef.name=" + p.partRef.name);
                        if (p.partRef.name.Contains(pair.Key))
                        {
                            Debug.Log("LCARS_SensorArray: determine_ship_configuration found pair.Key=" + pair.Key);
                            return pair.Value;
                        }
                    }
                }
            }
            catch
            {}
            try
            {

            if (!TargetShip_1.loaded) { TargetShip_1.Load(); }
            foreach (Part p in TargetShip_1.parts)
            {
                Debug.Log("LCARS_SensorArray: determine_ship_configuration p.partName=" + p.partName);
                Debug.Log("LCARS_SensorArray: determine_ship_configuration p.name=" + p.name);
                foreach (KeyValuePair<string, string> pair in LCARS_Utilities.VesselTypeRecognizer_list)
                {
                    Debug.Log("LCARS_SensorArray: determine_ship_configuration pair.Key=" + pair.Key);
                    if (p.name.Contains(pair.Key))
                    {
                        Debug.Log("LCARS_SensorArray: determine_ship_configuration found pair.Key=" + pair.Key);
                        return pair.Value;
                    }
                }
            }
            }
            catch
            { }
            /*
             */
            return "It's a ship of unknown configuration";
        }

        private void GUI_Ship_Scan_Level2(Vessel TargetShip_1)
        {
            float current_temp_total = 0;
            float max_temp_total = 0;
            foreach (Part p in TargetShip_1.parts)
            {
                current_temp_total += p.temperature;
                max_temp_total += p.maxTemp;
            }
            float heat_percentage = current_temp_total / (max_temp_total / 100);
            float hullintegrity_percentage = 100 - heat_percentage;

            GUILayout.BeginVertical();
            GUILayout.Label("Hull Temperatur is at: " + Math.Round(heat_percentage, 2) + "%");
            GUILayout.Label("Hull Integrity is at: " + Math.Round(hullintegrity_percentage, 2) + "%");
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Tactical: ToDo");
            GUILayout.EndVertical();

            /*
            bool weaponsystems_present = false;
            bool torpedo_present = false;
            bool cloak_present = false;
            bool SIF_present = false;
            
            TargetShip_1.LCARS().lODN.ShipSystems.
            
            weaponsystems_present = TargetShip_1.LCARSVessel_checkForPartWithModule("LCARS_WeaponSystems");
            torpedo_present = TargetShip_1.LCARSVessel_checkForPartWithModule("LCARS_PhotonTorpedo");
            cloak_present = TargetShip_1.LCARSVessel_checkForPartWithModule("LCARS_CloakingDevice");
            SIF_present = TargetShip_1.LCARSVessel_checkForPartWithModule("LCARS_StructuralIntegrityField");
            GUILayout.BeginVertical();
            GUILayout.Label("Phasers: " + weaponsystems_present);
            GUILayout.Label("Torpedos: " + torpedo_present);
            GUILayout.Label("Cloak: " + cloak_present);
            GUILayout.Label("Struct.Integr.Field: " + SIF_present);
            GUILayout.EndVertical();
            */
        }

        bool IFD_error = false;
        private void GUI_Ship_Scan_Level3(Vessel TargetShip_1)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Crew compliment current: " + TargetShip_1.GetCrewCount());
            GUILayout.Label("Crew compliment max: " + TargetShip_1.GetCrewCapacity());
            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            GUILayout.Label("Internal Locations:");
            if (IFD_error)
            {
                GUILayout.Label("Error: IFD was not able to identify this GameObject!");
                GUILayout.Label("Maybe the object is not loaded.");
            }
            foreach (string aP in LCARSNCI_Bridge.Instance.get_ActionPoints_From_sfs_ActiveObject(TargetShip_1.id))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Location: " + aP);
                if (GUILayout.Button("Send to IFD"))
                {
                    IFD_error = false;
                    try
                    {
                        GameObject tmp = TargetShip_1.rootPart.FindModelTransform(aP).gameObject;
                        thisVessel.LCARS().lODN.IFD_object_of_interest = tmp;
                    }
                    catch { IFD_error = true; }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                GUILayout.Label("Installed Systems:");
                foreach (KeyValuePair<string, PartModule> pair in TargetShip_1.LCARSVessel_PartModules())
                {
                    GUILayout.Label("System Name: " + pair.Value.moduleName);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("Available Resources: " + TargetShip_1.LCARSVessel_ResourceMass() + "t total");
                foreach (KeyValuePair<string, PartResource> pair in TargetShip_1.LCARSVessel_Resource_List())
                {
                    GUILayout.Label(pair.Value.resourceName);
                }
                GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }



    } 
    
}
