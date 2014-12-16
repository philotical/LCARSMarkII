using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LCARSMarkII
{





    public class LCARS_NCIGUI_SpaceCenter
    {
        public LCARS_NCI NCI;

        public bool ObjectListDone = false;
        public bool WindowState = false;
        private Rect windowPosition = new Rect(120, 120, 550, 450);
        private static System.Random rnd = new System.Random();
        private int windowID = rnd.Next();


        //private Dictionary<string, LCARS_NCI_Object> Naturals_List = null;
        //private Dictionary<string, LCARS_NCI_Object> Mines_List = null;
        //private Dictionary<string, LCARS_NCI_Object> Stations_List = null;
        //private Dictionary<string, LCARS_NCI_Object> debug_List = null;

        private int GUI_Mode = 0;
        private LCARS_NCI_Scenario ScenarioRef = null;

        //private float originalLoadDistance = Vessel.loadDistance;
        //private float originalUnLoadDistance = Vessel.unloadDistance;
        private float originalLoadDistance = 2500f;
        private float originalUnLoadDistance = 2250f;
        
        internal void setWindowState(bool state)
        {
            UnityEngine.Debug.Log("### NCI setWindowState");
            WindowState = state;
        }
        internal bool getWindowState()
        {
            UnityEngine.Debug.Log("### NCI getWindowState");
            return WindowState;
        }

        internal void OnGUI()
        {
            /*if (Naturals_List==null)
            {
                Naturals_List = new Dictionary<string, LCARS_NCI_Object>();
                Mines_List = new Dictionary<string, LCARS_NCI_Object>();
                Stations_List = new Dictionary<string, LCARS_NCI_Object>();
                debug_List = new Dictionary<string, LCARS_NCI_Object>();
                Gather_Object_Lists();
            }*/
            if (NCI == null)
            {
                NCI = LCARS_NCI.Instance;
                NCI.init();

            }
            /*
            if (NCI.Data.Naturals_List.Count < 1)
            {
                NCI.Data.Naturals_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (NCI.Data.Mines_List.Count < 1)
            {
                NCI.Data.Mines_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            if (NCI.Data.Stations_List.Count < 1)
            {
                NCI.Data.Stations_List = new Dictionary<string, LCARS_NCI_Object>();
            }
            Gather_Object_Lists();
            */

            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            { return; }
            //UnityEngine.Debug.Log("### NCI OnGUI begin WindowState = " + WindowState);

            if (!NCI.GUI.SpaceCenter.WindowState)
            { return; }

            if (HighLogic.LoadedSceneIsEditor)
            { return; }
            //UnityEngine.Debug.Log("### NCI OnGUI 1 ");



            windowPosition = LCARS_NCI.Instance.ClampToScreen(GUILayout.Window(windowID, windowPosition, NaturalCauseInc_Window, ""));
            //UnityEngine.Debug.Log("### NCI OnGUI end ");

        }
        //static public void SetLoadDistance(float loadDistance = 2500, float unloadDistance = 2250)
        public void SetLoadDistance(float loadDistance = 0f, float unloadDistance = 0f)
        {
            loadDistance = (loadDistance == 0f) ? originalLoadDistance : loadDistance;
            unloadDistance = (unloadDistance == 0f) ? originalUnLoadDistance : unloadDistance;
            Vessel.loadDistance = loadDistance;
            Vessel.unloadDistance = unloadDistance;
        }


        private void NaturalCauseInc_Window(int windowID)
        {
            UnityEngine.Debug.Log("### NCI NaturalCauseInc_Window 1 ");
            if (HighLogic.LoadedSceneIsEditor)
                return;

            UnityEngine.Debug.Log("### NCI NaturalCauseInc_Window 2 ");
            if (!WindowState)
                return;
            UnityEngine.Debug.Log("### NCI NaturalCauseInc_Window 3 ");


            GUILayout.BeginVertical();

            ScenarioRef = ScenarioRunner.fetch.GetComponent<LCARS_NCI_Scenario>();

            if (ScenarioRef != null)
            {
                if (GUI_Mode != 0)
                {
                    ListObjects();
                }
                else
                {
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



                    GUILayout.Label("Welcome at Natural Cause Inc. What can we do for you?");
                    if (GUILayout.Button("Generate Natural Phenomena"))
                    {
                        GUI_Mode = 1;
                    }

                    if (GUILayout.Button("Generate Armed Objects"))
                    {
                        GUI_Mode = 2;
                    }

                    if (GUILayout.Button("Generate Stations"))
                    {
                        GUI_Mode = 3;
                    }

                    if (GUILayout.Button("List all NCI Objects"))
                    {
                        GUI_Mode = 4;
                    }
                }
            }
            else
            {
                GUILayout.Label("ScenarioRef not ready!!");
                GUILayout.Label("");
                GUILayout.Label("If this message does not disappear on it's own, it seems to be your first load with NCI.");
                GUILayout.Label("Because of the way KSP works, I need you to quit back to main menu and load this save again.");
                GUILayout.Label("That will solve the issue for this save.");
                GUILayout.Label("Sorry for the trouble..");
            }
            /*if (GUILayout.Button("Update sfs_ActiveObjectsList"))
            {
                NCI.Data.Update_sfs_ActiveObjectsList();
            }*/
            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        string SelectedPart = null;
        LCARS_NCI_Object Selected_LCARS_NCI_Object = null;
        CelestialBody SelectedBody = null;
        int amount = 1;
        bool RandomHeight = true;
        float OrbitAltitude = 0;
        bool RandomInclination = true;
        float OrbitInclination = 0;
        bool RandomLan = true;
        float OrbitLan = 0;
        bool RandomW = true;
        float OrbitW = 0;
        float minAlt = 0f;
        float maxAlt = 0f;
        //Vector2 ScrollPosition;
        //GUIStyle scrollview_style;

        string listDisplayIndex = null;
        Part SelectedPartPrefab = null;
        bool SpawnCrew = false;
        int crewcapacity = 0;
        GUIStyle scrollview_style;
        Vector2 ScrollPosition;

        private void ListObjects()
        {
                Dictionary<string, LCARS_NCI_Object> current_list = null;

                if (GUILayout.Button("Back"))
                {
                    GUI_Mode = 0;
                    SelectedPart = null;
                    SelectedBody = null;
                    Selected_LCARS_NCI_Object = null;
                    amount = 1;
                    listDisplayIndex = null;
                }


            switch (GUI_Mode)
            {
                case 1:
                    GUILayout.Label("::Natural Phenomena::");
                    current_list = NCI.Data.Naturals_List;
                    break;
                case 2:
                    GUILayout.Label("::Armed Objects::");
                    current_list = NCI.Data.Mines_List;
                    break;
                case 3:
                    GUILayout.Label("::Stations::");
                    current_list = NCI.Data.Stations_List;
                    break;
                case 4:
                    GUILayout.Label("::Object List::");
                    ListSpaceObjects();
                    break;
            }

            if (SelectedPart == null && current_list!=null)
            {
                UnityEngine.Debug.Log("### NCI ListObjects current_list.Count=" + current_list.Count);
                GUILayout.Label("Please select the object you desire");
                GUILayout.Label("---------------");
                GUILayout.BeginHorizontal();
                foreach (KeyValuePair<string, LCARS_NCI_Object> pair in current_list)
                {
                    string objID = pair.Key;
                    string partname = pair.Value.partname;
                    if (GUILayout.Button(partname))
                    {
                        listDisplayIndex = objID;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Label("---------------");
                if (listDisplayIndex != null)
                {

                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();

                    GUILayout.Button(current_list[listDisplayIndex].icon_tex, GUILayout.Height(80), GUILayout.Width(80));
                    GUILayout.BeginVertical();
                    GUILayout.Label(current_list[listDisplayIndex].partname);
                    GUILayout.Label(current_list[listDisplayIndex].description);
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                    GUILayout.Label("Created by:" + current_list[listDisplayIndex].creator);
                    GUILayout.Label("URL:" + current_list[listDisplayIndex].url);
                    GUILayout.Label("This Part contains the following ActionPoints:");
                    scrollview_style = new GUIStyle();
                    scrollview_style.fixedHeight = 90;
                    GUILayout.BeginVertical();
                    ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, scrollview_style);
                    foreach (string s in current_list[listDisplayIndex].actionpoints)
                    {
                        GUILayout.Label("-" + s + " ");
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();

                    if (GUILayout.Button("Select"))
                    {
                        SelectedPart = current_list[listDisplayIndex].partname;
                        Selected_LCARS_NCI_Object = current_list[listDisplayIndex];
                    }

                    GUILayout.EndVertical();

                }
                /*

                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 490;
                ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, scrollview_style);
                foreach (KeyValuePair<string, LCARS_NCI_Object> pair in current_list)
                {
                    string objID = pair.Key;
                    string partname = pair.Value.partname;
                    string creator = pair.Value.creator;
                    string url = pair.Value.url;
                    string title = pair.Value.title;
                    string description = pair.Value.description;
                    string icon = pair.Value.icon;
                    Texture2D icon_tex = pair.Value.icon_tex;
                    List<string> eggModes = pair.Value.actionpoints;
                    bool isInstalled = pair.Value.isInstalled;

                    if (isInstalled)
                    {
                        GUILayout.BeginVertical();
                        
                        GUILayout.BeginHorizontal();

                            GUILayout.Button(icon_tex, GUILayout.Height(80), GUILayout.Width(80));
                                GUILayout.BeginVertical();
                                    GUILayout.Label(partname);
                                    GUILayout.Label(description);
                                GUILayout.EndVertical();
                        
                        GUILayout.EndHorizontal();
                        GUILayout.Label("Created by:" + creator);
                        GUILayout.Label("URL:" + url);
                        GUILayout.Label("This Part may hold the following easteregg modes:");
                        GUILayout.BeginHorizontal();
                        foreach(string s in eggModes)
                        {
                            GUILayout.Label("-"+s+" ");
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("Select"))
                        {
                            SelectedPart = partname;
                        }
                        
                        GUILayout.EndVertical();
                    }
                    else
                    {
                        UnityEngine.Debug.Log("### NCI ListObjects skipping " + partname + " - file not found");
                    }
                }
                GUILayout.EndScrollView();
                */
            }
            else
            {
                if (SelectedBody == null)
                {
                    GUILayout.Label("---------------");
                    GUILayout.Label("Please Select the Location you desire");
                    GUILayout.Label("---------------");


                    foreach (CelestialBody CB in FlightGlobals.Bodies)
                    {
                        if (GUILayout.Button(CB.name))
                        {
                            SelectedBody = CB;
                            minAlt = calculateMinAltitude();
                            maxAlt = calculateMaxAltitude(minAlt);
                            OrbitAltitude = minAlt;
                        }
                    }
                }
                else
                {
                    GUILayout.Label("---------------");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Part: " + SelectedPart);
                    if (GUILayout.Button("Change"))
                    {
                        SelectedPart = null;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Body: "+SelectedBody.name);
                    if (GUILayout.Button("Change"))
                    {
                        SelectedBody = null;
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.Label("---------------");
                    GUILayout.Label("Please Select from the available Options");
                    GUILayout.Label("---------------");
                    if (GUI_Mode==2)
                    {
                        GUILayout.Label("How many Mines? " + amount);
                        amount = (int)GUILayout.HorizontalSlider((float)amount, 1, 100);
                    }

                    if(SelectedPartPrefab==null)
                    {
                        foreach(AvailablePart aP in PartLoader.LoadedPartsList)
                        {
                            UnityEngine.Debug.Log("LCARS_NCIGUI_SpaceCenter ListObjects: SelectedPart=" + SelectedPart + " - aP.partPrefab.partName=" + aP.partPrefab.partName);
                            if (aP.partPrefab.name == SelectedPart)
                            {
                                UnityEngine.Debug.Log("LCARS_NCIGUI_SpaceCenter ListObjects: SelectedPartPrefab found ");
                                SelectedPartPrefab = aP.partPrefab;
                            }
                        }
                    }

                    if (SelectedPartPrefab != null)
                    {
                        if (SelectedPartPrefab.CrewCapacity > 0)
                        {
                            SpawnCrew = GUILayout.Toggle(SpawnCrew, "Spawn Crew");
                            if (SpawnCrew)
                            {

                                GUILayout.Label("CrewCapacity: " +crewcapacity+"(max. "+ SelectedPartPrefab.CrewCapacity+")");
                                crewcapacity = (int)Math.Round(GUILayout.HorizontalSlider(crewcapacity, 0f, (float)SelectedPartPrefab.CrewCapacity), 0);
                                GUILayout.BeginHorizontal();
                                if (GUILayout.RepeatButton("Empty")) { crewcapacity = 0; }
                                if (GUILayout.RepeatButton("Fill")) { crewcapacity = SelectedPartPrefab.CrewCapacity; }
                                GUILayout.EndHorizontal();
                            }
                        }
                        else 
                        {
                            GUILayout.Label("SelectedPartPrefab.CrewCapacity == 0 ");
                        }
                    }
                    else 
                    {
                        GUILayout.Label("SelectedPartPrefab == null ");
                    }

                    RandomHeight = GUILayout.Toggle(RandomHeight, "Random Altitude");
                    if (!RandomHeight)
                    {
                        GUILayout.Label("Altitude: " + ((OrbitAltitude - minAlt) / 1000) + "Km");
                        OrbitAltitude = (float)Mathf.Clamp((float)Math.Round(GUILayout.HorizontalSlider(OrbitAltitude, (float)minAlt, (float)maxAlt), 2), (float)minAlt, (float)maxAlt); ;
                        GUILayout.BeginHorizontal();
                        if (GUILayout.RepeatButton("-100Km")){OrbitAltitude -= 100000F;}
                        if (GUILayout.RepeatButton("-10Km")){OrbitAltitude -= 10000F;}
                        if (GUILayout.RepeatButton("-1Km")){OrbitAltitude -= 1000F;}
                        if (GUILayout.RepeatButton("+1Km")){OrbitAltitude += 1000F;}
                        if (GUILayout.RepeatButton("+10Km")){OrbitAltitude += 10000F;}
                        if (GUILayout.RepeatButton("+100Km")){OrbitAltitude += 100000F;}

                        GUILayout.EndHorizontal();
                    }

                    RandomInclination = GUILayout.Toggle(RandomInclination, "Random Inclination");
                    if (!RandomInclination)
                    {
                        GUILayout.Label("Inclination: " + OrbitInclination + "°");
                        OrbitInclination = (int)Mathf.Clamp(GUILayout.HorizontalSlider((float)OrbitInclination, -180f, 180f), -180f, 180f);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.RepeatButton("-10°")) { OrbitInclination -= 10F; }
                        if (GUILayout.RepeatButton("-1°")) { OrbitInclination -= 1F; }
                        if (GUILayout.RepeatButton("+1°")) { OrbitInclination += 1F; }
                        if (GUILayout.RepeatButton("+10°")) { OrbitInclination += 10F; }
                        GUILayout.EndHorizontal();
                    }

                    RandomLan = GUILayout.Toggle(RandomLan, "Random LAN");
                    if (!RandomLan)
                    {
                        GUILayout.Label("LAN: " + OrbitLan+"°");
                        OrbitLan = (int)Mathf.Clamp(GUILayout.HorizontalSlider((float)OrbitLan, 0f, 360f),0,360);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.RepeatButton("-10°")) { OrbitLan -= 10F; }
                        if (GUILayout.RepeatButton("-1°")) { OrbitLan -= 1F; }
                        if (GUILayout.RepeatButton("+1°")) { OrbitLan += 1F; }
                        if (GUILayout.RepeatButton("+10°")) { OrbitLan += 10F; }
                        GUILayout.EndHorizontal();
                    }

                    RandomW = GUILayout.Toggle(RandomW, "Random W");
                    if (!RandomW)
                    {
                        GUILayout.Label("W: " + OrbitW + "°");
                        OrbitW = (int)Mathf.Clamp(GUILayout.HorizontalSlider((float)OrbitW, 0f, 360f),0,360);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.RepeatButton("-10°")) { OrbitW -= 10F; }
                        if (GUILayout.RepeatButton("-1°")) { OrbitW -= 1F; }
                        if (GUILayout.RepeatButton("+1°")) { OrbitW += 1F; }
                        if (GUILayout.RepeatButton("+10°")) { OrbitW += 10F; }
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Label("");
                    if (GUILayout.Button("Make it so.."))
                    {
                        GenerateNaturalCauseAtLocation(SelectedPart, SelectedBody, amount);
                    }

                }
            }

        }

        private void ListSpaceObjects()
        {
            GUILayout.Label("---------------");
            GUILayout.Label("This is what you placed in space!");
            GUILayout.Label("Click the delete button only if you really want to delete the whole group");
            GUILayout.Label("---------------");

            Dictionary<string, int> group_counting = new Dictionary<string, int>();
            Dictionary<Guid, LCARS_NCI_Object> availableEggs = ScenarioRef.get_availableEggs();
            foreach (Guid id in availableEggs.Keys)
            {
                string partname = availableEggs[id].partname;
                if (group_counting.ContainsKey(partname))
                {
                    group_counting[partname] += 1;
                }
                else
                {
                    group_counting.Add(partname,1);
                }
            }
            foreach (string pName in group_counting.Keys)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(pName + ": " + group_counting[pName]);
                    if (GUILayout.Button("Delete (todo)"))
                    {
                    }

                GUILayout.EndHorizontal();
            }

        }





        private float calculateMinAltitude()
        {
            /*
            return (float)Mathf.Clamp(
                        UnityEngine.Random.Range((int)(SelectedBody.Radius + SelectedBody.maxAtmosphereAltitude + 1000), (int)(SelectedBody.sphereOfInfluence / 2)),
                        (int)(SelectedBody.Radius + SelectedBody.maxAtmosphereAltitude),
                        (int)((SelectedBody.sphereOfInfluence / 2) - 2)
                        );
            */
            //return (float)SelectedBody.Radius + SelectedBody.maxAtmosphereAltitude + 1000;
            return (float)SelectedBody.Radius + 1000;
        }
        private float calculateMaxAltitude(float minAlt)
        {
            /*
            return (float)Mathf.Clamp(
                        (int)UnityEngine.Random.Range((int)(SelectedBody.sphereOfInfluence / 2), (int)SelectedBody.sphereOfInfluence),
                        (int)minAlt + 2,
                        (int)(SelectedBody.sphereOfInfluence - 2)
                        );
            */
            return (float)SelectedBody.sphereOfInfluence - 1;
        }















        Orbit lastOrbit = null;
        private void GenerateNaturalCauseAtLocation(string partname, CelestialBody SelectedBody, int amount = 1)
        {
            UnityEngine.Debug.Log("GenerateNaturalCauseAtLocation: 1 ");
            //Vector3 pos = FlightGlobals.ActiveVessel.rootPart.transform.position;
            //Quaternion rot = FlightGlobals.ActiveVessel.rootPart.transform.rotation * Quaternion.Euler(FlightGlobals.ActiveVessel.rootPart.transform.up);
            UnityEngine.Debug.Log("GenerateNaturalCauseAtLocation: 2 ");

            //Part newPart = SpawnGliderPart("LCARS_MagBoots", pos, rot);
            int i = 0;
            lastOrbit = null;
            while (i < amount)
            {
                Part newPart = SpawnPart(partname, SelectedBody);
                if (!newPart)
                {
                    UnityEngine.Debug.Log("GenerateNaturalCauseAtLocation failed to create the part - partname=" + partname + "  amount=" + amount + " number = " + i);
                    return;
                }
                i++;
                UnityEngine.Debug.Log("GenerateNaturalCauseAtLocation: 3 - partname=" + partname + "  amount=" + amount + " number = " + i);
            }

        }

        public Part SpawnPart(string partname, CelestialBody SelectedBody)
        {
            UnityEngine.Debug.Log("SpawnPart: 1a ");
            AvailablePart avPart = PartLoader.getPartInfoByName(partname);
            
            UnityEngine.Debug.Log("SpawnPart: 2a ");
            //UnityEngine.Debug.Log("SpawnPart avPart.name=" + avPart.name);
            //UnityEngine.Debug.Log("SpawnPart avPart.partPrefab.name=" + avPart.partPrefab.name);
            //UnityEngine.Debug.Log("SpawnPart: 3a ");

            return SpawnPart2(avPart, SelectedBody);  
        }

        public Part SpawnPart2(AvailablePart avPart, CelestialBody SelectedBody)
        {
            UnityEngine.Debug.Log("SpawnPart2: 1.1 ");

            Part obj = (Part)UnityEngine.Object.Instantiate((UnityEngine.Object)avPart.partPrefab);
            UnityEngine.Debug.Log("SpawnPart2: 2 ");
            if (!obj)
            {
                UnityEngine.Debug.Log("SpawnPart2 Failed to instantiate ");// + avPart.partPrefab.name);
                return null;
            }

            UnityEngine.Debug.Log("SpawnPart2: 3.1 ");

            Part newPart = obj;
            newPart.gameObject.SetActive(true);
            //newPart.gameObject.name = "LCARS_NaturalCauseInc_Part_" + SelectedPart + "_at_" + SelectedBody.name;
            newPart.gameObject.name = SelectedPart;
            newPart.partInfo = avPart;
            newPart.partName = SelectedPart;
            //newPart.highlightRecurse = false;
            UnityEngine.Debug.Log("SpawnPart2: 3.2 ");
            
            /*
            PartModule pM = newPart.AddModule("LCARS_EastereggController");
            string eggModes = "";
            //newPart.AddModule("LCARS_EastereggController");
            //LCARS_EastereggController LEC = newPart.GetComponent<LCARS_EastereggController>();
            foreach (string s in Selected_LCARS_NCI_Object.actionpoints)
            {
                if (eggModes.Length > 1) { eggModes += ","; }
                eggModes += s;
            }
            pM.Fields.SetValue("eggModes", eggModes);
            */

        //private readonly string ProfileStoragePath = ConfigUtil.GetDllDirectoryPath() + "/profiles.cfg";

        //ProfileTable storedProfiles;

        //VesselTable vesselProfiles;



            ShipConstruct newShip = new ShipConstruct();
            newShip.Add(newPart);

            //newShip.Add(FlightGlobals.ActiveVessel.rootPart);

            newShip.SaveShip();
            newShip.shipName = avPart.title;
            //newShip.shipType = 1;
            UnityEngine.Debug.Log("SpawnPart2: 4 ");

            VesselCrewManifest vessCrewManifest = new VesselCrewManifest();
            Vessel currentVessel = FlightGlobals.ActiveVessel;
            UnityEngine.Debug.Log("SpawnPart2: 5 ");

            UnityEngine.Debug.Log("SpawnPart2: Vessel v ");
            Vessel v = newShip.parts[0].localRoot.gameObject.AddComponent<Vessel>();
            v.id = Guid.NewGuid();
            v.vesselName = newShip.shipName;
            v.gameObject.SetActive(true); 
            v.gameObject.AddComponent<Part>();
            v.parts = new List<Part>();
            UnityEngine.Debug.Log("SpawnPart2: v.parts.Add ");
            v.parts.Add(newShip.parts[0]);
            v.rootPart = newPart;
            v.rootPart.partInfo = newPart.partInfo;
            v.rootPart.partName = SelectedPart;
            v.rootPart.name = SelectedPart;
            v.vesselType = VesselType.Probe;
            try
            {
                v.Initialize(false);
            }
            catch { }
            v.Landed = true;
            UnityEngine.Debug.Log("SpawnPart2: v.parts ");
            //v.parts[0] = newPart;
            UnityEngine.Debug.Log("SpawnPart2: v.rootPart ");
            UnityEngine.Debug.Log("SpawnPart2: v.rootPart.name=" + v.rootPart.name);
            v.rootPart.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
            v.rootPart.missionID = (uint)Guid.NewGuid().GetHashCode();
            UnityEngine.Debug.Log("SpawnPart2: 6 ");
            UnityEngine.Debug.Log("SpawnPart2: v.rootPart.missionID=" + v.rootPart.missionID);

            v.orbitDriver = new OrbitDriver();
            v.orbitDriver.orbit = new Orbit();
            v.flightIntegrator = new FlightIntegrator();
            //v.GetActiveParts();
            //v.Part = new Part();


            UnityEngine.Debug.Log("SpawnPart2: 6b ");

            UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 1 ");
            try
            {
                /*
                            var game = HighLogic.CurrentGame;
                            ProtoScenarioModule psm = game.scenarios.Find(s => s.moduleName == typeof(LCARS_NCI_Scenario).Name);

                            UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 1 ");
                            //KSPScenario.
                            ScenarioModule scm = psm.moduleRef;
                            LCARS_NCI_Scenario.c
                                //(ScenarioRef==null) ? new LCARS_NCI_Scenario() : ScenarioRef;
                            UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 2 ");
                            //ScenarioRef.getScenarioRef();
                            ScenarioRef.addObject(Selected_LCARS_NCI_Object, v.id);
                            UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 3 ");
                 */

                /*
                var game = HighLogic.CurrentGame;
                ProtoScenarioModule psm = game.scenarios.Find(s => s.moduleName == typeof(LCARS_NCI_Scenario).Name);
                UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 2 ");
                LCARS_NCI_Scenario ScenarioRef = (LCARS_NCI_Scenario)psm.moduleRef;
                UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 3 ");
                */

                //Selected_LCARS_NCI_Object.partname = newPart.gameObject.name;
                ScenarioRef.registerNCIObjects(v.id, Selected_LCARS_NCI_Object);

                //ScenarioRef.addObject(Selected_LCARS_NCI_Object, v.id);
                UnityEngine.Debug.Log("SpawnPart2: LCARS_NCI_Scenario registerEggObjects done");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("SpawnPart2: LCARS_NCI_Scenario registerEggObjects ex=" + ex);
            }
            UnityEngine.Debug.Log("SpawnPart2: ScenarioRef 4 ");



            CelestialBody locationTarget = SelectedBody;
            /*
            foreach (CelestialBody CB in FlightGlobals.Bodies)
            { 
                if(CB.name == location_name)
                {
                    locationTarget = CB;
                }
            }
            locationTarget = FlightGlobals.Bodies.Find(body => body.name == location_name);
            */

            UnityEngine.Debug.Log("SpawnPart2: 6c ");
            /*
                double minAlt = (double)Mathf.Clamp(
                                UnityEngine.Random.Range((int)(locationTarget.Radius + locationTarget.maxAtmosphereAltitude + 1000), (int)(locationTarget.sphereOfInfluence / 2)),
                                (int)(locationTarget.Radius + locationTarget.maxAtmosphereAltitude),
                                (int)((locationTarget.sphereOfInfluence / 2) - 2)
                                );
                
                
                double maxAlt = (double)Mathf.Clamp(
                                (int)UnityEngine.Random.Range((int)(locationTarget.sphereOfInfluence / 2), (int)locationTarget.sphereOfInfluence),
                                (int)minAlt+2,
                                (int)(locationTarget.sphereOfInfluence - 2)
                                );
            */
                minAlt = calculateMinAltitude();
                maxAlt = calculateMaxAltitude(minAlt);
                //double altit = rnd.Next((int)minAlt, (int)maxAlt);

                UnityEngine.Debug.Log("SpawnPart2: 7 ");

            if (lastOrbit == null)
            {
                if (RandomHeight)
                {
                    v.orbitDriver.orbit = Orbit.CreateRandomOrbitAround(locationTarget, minAlt, maxAlt);
                }
                else
                {
                    v.orbitDriver.orbit = Orbit.CreateRandomOrbitAround(locationTarget, OrbitAltitude - 1, OrbitAltitude+1);
                }
                if (RandomInclination)
                {
                    v.orbitDriver.orbit.inclination = (double)UnityEngine.Random.Range(-90f, 90f);
                }
                else
                {
                    v.orbitDriver.orbit.inclination = OrbitInclination;
                }
                if (RandomLan)
                {
                    v.orbitDriver.orbit.LAN = (double)UnityEngine.Random.Range(0f, 360f);
                }
                else
                {
                    v.orbitDriver.orbit.LAN = OrbitLan;
                }
                if (RandomW)
                {
                    v.orbitDriver.orbit.argumentOfPeriapsis = (double)UnityEngine.Random.Range(0f, 360f);
                }
                else
                {
                    v.orbitDriver.orbit.argumentOfPeriapsis = OrbitW;
                }

                
                lastOrbit = v.orbit;
                UnityEngine.Debug.Log("SpawnPart2 CreateRandomOrbitAround: inclination=" + v.orbitDriver.orbit.inclination);

            }
            else
            { 
                if (RandomHeight)
                {
                    v.orbitDriver.orbit = Orbit.CreateRandomOrbitNearby(lastOrbit);
                }
                else
                {
                    v.orbitDriver.orbit = Orbit.CreateRandomOrbitAround(locationTarget, OrbitAltitude - 1, OrbitAltitude + 1);
                }
                if (RandomInclination)
                {
                    v.orbitDriver.orbit.inclination = (double)UnityEngine.Random.Range(-90f, 90f);
                }
                if (RandomLan)
                {
                    v.orbitDriver.orbit.LAN = (double)UnityEngine.Random.Range(0f, 360f);
                }
                if (RandomW)
                {
                    v.orbitDriver.orbit.argumentOfPeriapsis = (double)UnityEngine.Random.Range(0f, 360f);
                }

                lastOrbit = v.orbit;
                UnityEngine.Debug.Log("SpawnPart2 CreateRandomOrbitNearby: inclination=" + v.orbitDriver.orbit.inclination);
            }


                Vector3 position = v.orbit.getPositionAtUT(Time.time);
                v.SetPosition(position);


                UnityEngine.Debug.Log("SpawnPart2: SetPosition ");

                v.SetWorldVelocity(v.orbitDriver.orbit.getOrbitalVelocityAtUT(Planetarium.GetUniversalTime()) );
                UnityEngine.Debug.Log("SpawnPart2: obt_velocity=" + v.obt_velocity);

                v.situation = Vessel.Situations.ORBITING;
                v.Landed = false;
                //v.altitude = altit;
                UnityEngine.Debug.Log("SpawnPart2: altitude=" + v.altitude);

                //v.orbitDriver.UpdateOrbit();
                UnityEngine.Debug.Log("SpawnPart2 UpdateFromUT: ");
            v.orbit.Init();
            v.orbit.UpdateFromUT(Planetarium.GetUniversalTime());



            try
            {
                newShip.parts[0].vessel.ResumeStaging();
                UnityEngine.Debug.Log("SpawnPart2: ResumeStaging ");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("SpawnPart2: ResumeStaging failed ex=" + ex.Message);
            }

            try
            {
                Staging.GenerateStagingSequence(newShip.parts[0].localRoot);
                UnityEngine.Debug.Log("SpawnPart2: GenerateStagingSequence ");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("SpawnPart2: GenerateStagingSequence failed ex=" + ex.Message);
            }

            try
            {
                Staging.RecalculateVesselStaging(newShip.parts[0].vessel);
                UnityEngine.Debug.Log("SpawnPart2: RecalculateVesselStaging ");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("SpawnPart2: RecalculateVesselStaging failed ex=" + ex.Message);
            }

            try
            {
                if (crewcapacity > 0)
                {
                    int backup_capacity = v.GetCrewCapacity();
                    v.rootPart.CrewCapacity = crewcapacity;
                    //v.rootPart.CreateInternalModel();
                    v.Load();
                    v.GoOffRails();

                    //ConfigNode foo = ConfigNode.Load();
                    //v.rootPart.internalModel.internalConfig();
                    v.rootPart.internalModel.Initialize(v.rootPart);

                    v.SpawnCrew();
                    v.GoOnRails();
                    v.rootPart.CrewCapacity = backup_capacity;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("SpawnPart2: SpawnCrew failed ex=" + ex.Message);
            }
            return newPart;
        }



        

    }
}
