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
            //Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " has components:");

            var components = go.GetComponents<PQSCity>();
            foreach (var c in components)
            {
                //Debug.Log(new string('.', indent + 3) + "c" + ": " + c.GetType().FullName);
                if (c.GetType().FullName == "PQSCity")
                {
                    Debug.Log((indent > 0 ? new string('-', indent) + ">" : "") + " " + go.name + " has component PQSCity");
                    if (Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position) < 5000f)
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
        //private private float lastflyUpdate = 0.0f;
        private float logInterval = 5.0f;
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
            GUILayout.Label("ToDo: " + thisVessel.vesselName);


            GUILayout.BeginVertical(scrollview_style1, GUILayout.Width(445));
            ScrollPosition1 = GUILayout.BeginScrollView(ScrollPosition1);

            if(ScanType != null)
            {
                if (GUILayout.Button("Back"))
                {
                    SelectedTargetShip = null;
                    ScanType = null;
                    ShipScanMode = 0;
                    BodyScanMode = 0;
                    SelectedTargetGameObject = null;
                    objectConf = "";
                    shipConf = "";
                    selGridInt2 = 0;
                    selGridInt3 = 0;
                }
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
                                break;

                            case 3:
                                GUILayout.Label("Interior Scan Results");
                                GUI_Ship_Scan_Level1(SelectedTargetShip);
                                GUI_Ship_Scan_Level2(SelectedTargetShip);
                                GUI_Ship_Scan_Level3(SelectedTargetShip);
                                LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Sensor Array"].name, LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L1_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L2_usage + LCARS.lODN.ShipSystems["Sensor Array"].powerSystem_L3_usage);
                                break;
                        }
                        break;
                
                    case"body":
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
                             case 2: // biome
                                GUILayout.Label("Biome Scan Results");
                                GUILayout.BeginVertical();
                                //CelestialBody.BiomeMap.Map;
                                BiomeTexture = FlightGlobals.currentMainBody.BiomeMap.Map;
                                GUIContent content = new GUIContent(BiomeTexture, "");
                                GUILayout.Box(content, GUILayout.Height(260), GUILayout.Width(260));
                                GUILayout.EndVertical();
                               break;
                             case 3: // surface
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
                                    float d = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                                    if (GUILayout.Button((d / 1000).ToString("F1") + "km " + LCARS.grav.vesselList[i].vesselName, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetShip = LCARS.grav.vesselList[i];
                                        ScanType = "ship";
                                    }
                                }
                                break;
                            case 2: //LongRange
                                GUILayout.Label("longRange Scan: ");
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
                                    float d = Vector3.Distance(LCARS.grav.vesselList[i].transform.position, thisVessel.transform.position);

                                    if (GUILayout.Button((d / 1000).ToString("F1") + "km " + LCARS.grav.vesselList[i].vesselName, GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetShip = LCARS.grav.vesselList[i];
                                        ScanType = "ship";
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
                                    foreach (GameObject go in ScanGOExtensions.PQSCity_Objects_closeRange.Values)
                                    {
                                        if (GUILayout.Button("Name: " + go.name + " Distance: " + Math.Round(Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position), 2), GUILayout.ExpandWidth(true)))
                                        {
                                            SelectedTargetGameObject = go;
                                            ScanType = "body";
                                            BodyScanMode = 1;
                                        }
                                    }
                                break;
                            case 1: //longRange
                                GUILayout.Label("longRange Objects: ");
                                foreach (GameObject go in ScanGOExtensions.PQSCity_Objects_longRange.Values)
                                {
                                    if (GUILayout.Button("Name: " + go.name + " Distance: " + Math.Round(Vector3.Distance(FlightGlobals.ActiveVessel.CoM, go.transform.position), 2), GUILayout.ExpandWidth(true)))
                                    {
                                        SelectedTargetGameObject = go;
                                        ScanType = "body";
                                        BodyScanMode = 1;
                                    }
                                }
                                break;
                            case 2: //Biom
                                GUILayout.Label("Biom: todo");
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
                catch { objectConf = "scan error"; }
            }
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(); GUILayout.Label("Name: "); GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Label("" + SelectedTargetGameObject.name); GUILayout.EndHorizontal();
            GUILayout.Label("Long: " + "todo" + " ");
            GUILayout.Label("Alt: " + "todo" + " ");
            GUILayout.Label("Description: " + objectConf);
            if (GUILayout.Button("Set as Target"))
            {
                Vessel v = null;
                try
                {
                    v = SelectedTargetGameObject.GetComponent<Vessel>();
                }
                catch 
                {
                    v.vesselType = VesselType.Flag;
                    v.vesselName = objectConf;
                    v = SelectedTargetGameObject.AddComponent<Vessel>();
                }
                if(v!=null)
                {
                    FlightGlobals.fetch.SetVesselTarget(v);
                }
            }
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
                catch { shipConf = "scan error"; }
            }

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(); GUILayout.Label("Name: "); GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace(); GUILayout.Label("" + TargetShip_1.vesselName); GUILayout.EndHorizontal();
            GUILayout.Label("Mass: " + Math.Round(TargetShip_1.GetTotalMass(), 2) + " t");
            GUILayout.Label("DryMass: " + Math.Round(thisVessel.LCARSVessel_DryMass(), 2) + " t");
            GUILayout.Label("Parts: " + TargetShip_1.parts.Count);
            GUILayout.Label("Ship Configuration: " + shipConf);
            if(GUILayout.Button("Set as Target"))
            {
                FlightGlobals.fetch.SetVesselTarget(TargetShip_1);
            }
            GUILayout.EndVertical();
        }
        private string determine_ship_configuration(Vessel TargetShip_1)
        {
            Debug.LogError("LCARS_SensorArray: determine_ship_configuration TargetShip_1.vesselName=" + TargetShip_1.vesselName);
            Dictionary<string, string> VTRL = LCARS_Utilities.init_VesselTypeRecognizer();
            try
            {
                foreach (ProtoPartSnapshot p in TargetShip_1.protoVessel.protoPartSnapshots)
                {
                    foreach (KeyValuePair<string, string> pair in LCARS_Utilities.VesselTypeRecognizer_list)
                    {
                        if (p.partRef.name.Contains(pair.Key))
                        {
                            return pair.Value;
                        }
                    }
                }
            }
            catch
            {
                if (!TargetShip_1.loaded) { TargetShip_1.Load(); }
                foreach (Part p in TargetShip_1.parts)
                {
                    Debug.LogError("LCARS_SensorArray: determine_ship_configuration p.partName=" + p.partName);
                    Debug.LogError("LCARS_SensorArray: determine_ship_configuration p.name=" + p.name);
                    foreach (KeyValuePair<string, string> pair in LCARS_Utilities.VesselTypeRecognizer_list)
                    {
                        if (p.name.Contains(pair.Key))
                        {
                            return pair.Value;
                        }
                    }
                }
            }
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

            bool weaponsystems_present = false;
            bool torpedo_present = false;
            bool cloak_present = false;
            bool SIF_present = false;

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
        }

        private void GUI_Ship_Scan_Level3(Vessel TargetShip_1)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Crew compliment current: " + TargetShip_1.GetCrewCount());
            GUILayout.Label("Crew compliment max: " + TargetShip_1.GetCrewCapacity());
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Installed Systems:");
            GUILayout.BeginVertical(scrollview_style2);
            ScrollPosition2 = GUILayout.BeginScrollView(ScrollPosition2);
            foreach (KeyValuePair<string, PartModule> pair in TargetShip_1.LCARSVessel_PartModules())
            {
                GUILayout.Label("System Name: " + pair.Value.moduleName);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Available Resources: " + TargetShip_1.LCARSVessel_ResourceMass() + "t total");
            GUILayout.BeginVertical(scrollview_style3);
            ScrollPosition3 = GUILayout.BeginScrollView(ScrollPosition3);
            foreach (KeyValuePair<string, PartResource> pair in TargetShip_1.LCARSVessel_Resource_List())
            {
                GUILayout.Label(pair.Value.resourceName);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndVertical();

        }



    } 
    
}
