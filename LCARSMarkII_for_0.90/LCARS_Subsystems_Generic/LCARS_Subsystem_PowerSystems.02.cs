
using System;
using System.Collections.Generic;
using UnityEngine;
using LCARSMarkII;

namespace LCARSMarkII
{
    public class LCARS_Subsystem_PowerSystems : ILCARSPlugin
    {
        public string subsystemName { get { return "Power Systems"; } }
        public string subsystemDescription { get { return "Manage power & heat, see stats about consumption"; } }
        public string subsystemStation { get { return "Engineering"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start

        public Dictionary<string, LCARS_PowerTaker_Type> LCARS_PowerTakers = null;

        GUIStyle scrollview_style;
        Vector2 PowerSystem_SubSystem_ScrollPosition;
        Vector2 PowerSystem_MainSystem_ScrollPosition;
        Vector2 PowerSystem_Generators1_ScrollPosition;
        Vector2 PowerSystem_Generators2_ScrollPosition;




        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        GUIStyle PowerSlider;
        GUIStyle PowerSliderThumb;

        Dictionary<int, string> Filter_Types = null;
        Dictionary<int, string> Filter_SubTypes = null;
        private int Filter_Type = 0;
        private int Filter_SubType = 0;
        public string[] selStrings_Filter_Type = new string[] { "Ship Wide", "Main System", "Sub Systems","Crew" };
        public string[] selStrings_Filter_SubType = new string[] { "All", "Bridge", "Helm", "Eng.", "Tact.", "Science", "Comm." };

        public int selGridIntMain = 0;
        public int selGridIntDrain = 0;
        public int selGridIntProduction = 0;
        public string[] selStringsMain = new string[] { "Drain", "Production" };
        //public string[] selStringsDrain = new string[] { "Ship Wide", "Main System", "Sub Systems" };
        public string[] selStringsProduction = new string[] { "Electric Charge", "Other" };
        public void getGUI()
        {
            if (this.LCARS_PowerTakers == null)
            {
                //this.LCARS_PowerTakers = new Dictionary<string, LCARS_PowerTaker_Type>() { };
            }
            if (PowerSlider == null)
            {
                PowerSlider = new GUIStyle();
                PowerSlider.fixedHeight = 18;
                PowerSlider.fixedWidth = 420;
                PowerSlider.padding = new RectOffset(0, 0, 0, 0);
                PowerSlider.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/GeneratorSliderBackgroundTexture", false);
            }
            if (PowerSliderThumb == null)
            {
                PowerSliderThumb = new GUIStyle();
                PowerSliderThumb.fixedHeight = 18;
                PowerSliderThumb.fixedWidth = 18;
                PowerSliderThumb.padding = new RectOffset(0, 0, 0, 0);
                PowerSliderThumb.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/GeneratorSliderThumbBackgroundTexture", false);
            }
            if (scrollview_style == null)
            {
                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 250;
            }
            if (Filter_Types == null)
            {
                Filter_Types = new Dictionary<int, string>() { };
                Filter_SubTypes = new Dictionary<int, string>() { };
                
                Filter_Types.Add(0, "All");
                Filter_Types.Add(1, "MainSystem");
                Filter_Types.Add(2, "SubSystem");
                Filter_Types.Add(3, "Crew");

                Filter_SubTypes.Add(0, "All");
                Filter_SubTypes.Add(1, "Bridge");
                Filter_SubTypes.Add(2, "Helm");
                Filter_SubTypes.Add(3, "Engineering");
                Filter_SubTypes.Add(4, "Tactical");
                Filter_SubTypes.Add(5, "Science");
                Filter_SubTypes.Add(6, "Communication");
            }
            selGridIntMain = GUILayout.SelectionGrid(selGridIntMain, selStringsMain, 2);
            switch (selGridIntMain)
            {
                case 0:
                    PowerGUI("newDrain");
                    //selGridIntDrain = GUILayout.SelectionGrid(selGridIntDrain, selStringsDrain, 3);
                    /*switch (selGridIntDrain)
                    {
                        case 0:
                            GUILayout.Label("Ship Wide: ");
                            PowerGUI("Drain_Total");
                            break;
                        case 0:
                            break;
            
                        case 1:
                            GUILayout.Label("Main System: ");
                            PowerGUI("Drain_Main");
                            break;
            
                        case 2:
                            GUILayout.Label("Sub Systems: ");
                            PowerGUI("Drain_Sub");
                            break;
                    }   
            */
                    break;
            
                case 1:
                    selGridIntProduction = GUILayout.SelectionGrid(selGridIntProduction, selStringsProduction, 4);
                    switch (selGridIntProduction)
                    {
                        case 0:
                            GUILayout.Label("Electric Charge: ");
                            PowerGUI("Production_EC");
                            break;
            
                        case 1:
                            GUILayout.Label("Other: ");
                            PowerGUI("Production_Other");
                           break;
            
                    }
                    break;
            /*
                case 2:
                    GUILayout.Label("Ship Power Status: ");
                    PowerGUI("Status");
                    break;
            */
            }


        }

        bool show_totals = true;
        bool show_details = false;
        private void PowerGUI(string GUI_Section)
        {

            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems getGUI begin ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("LCARS_Subsystem_PowerSystems: ");
            GUILayout.EndHorizontal();

            switch (GUI_Section)
            {

                case "newDrain":

                        GUILayout.BeginHorizontal();
                            show_totals = GUILayout.Toggle(show_totals, "Show Totals");
                            show_details = GUILayout.Toggle(show_details, "Show Details");
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(GUILayout.Width(220));
                        GUILayout.Label("System");
                        GUILayout.EndVertical();
                        GUILayout.Label("Current MW");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Total MW");
                        GUILayout.EndHorizontal();

                    if (show_totals)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(GUILayout.Width(220));
                        GUILayout.Label("Ship Wide: ");
                        GUILayout.EndVertical();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_total() / 100000 + " MW");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_total(true) / 100000 + " MW");
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(GUILayout.Width(220));
                        GUILayout.Label("Main System: ");
                        GUILayout.EndVertical();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_main_systems() / 100000 + " MW");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_main_systems(true) / 100000 + " MW");
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical(GUILayout.Width(220));
                        GUILayout.Label("Sub System: ");
                        GUILayout.EndVertical();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_sub_systems() / 100000 + " MW");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(LCARS.lPowSys.get_consumption_sub_systems(true) / 100000 + " MW");
                        GUILayout.EndHorizontal();
                    }

                    if (show_details)
                    {
                        GUILayout.Label("Details: ");
                        Filter_Type = GUILayout.SelectionGrid(Filter_Type, selStrings_Filter_Type, 4);
                        Filter_SubType = GUILayout.SelectionGrid(Filter_SubType, selStrings_Filter_SubType, 7);

                        scrollview_style.fixedHeight = 200;
                        GUILayout.BeginVertical(scrollview_style);
                        PowerSystem_MainSystem_ScrollPosition = GUILayout.BeginScrollView(PowerSystem_MainSystem_ScrollPosition);
                        foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lPowSys.getPowerTakers())
                        {
                            if (Filter_Types[Filter_Type] != "All")
                            {
                                if (Filter_Types[Filter_Type] != pair.Value.powerSystem_takerType)
                                {
                                    continue;
                                }
                            }
                            if (Filter_SubTypes[Filter_SubType] != "All")
                            {
                                if (Filter_SubTypes[Filter_SubType] != pair.Value.powerSystem_takerSubType)
                                {
                                    continue;
                                }
                            }
                            GUILayout.BeginHorizontal();
                            GUILayout.BeginVertical(GUILayout.Width(220));
                            GUILayout.Label(pair.Value.name);
                            GUILayout.EndVertical();
                            GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_current / 100000, 7) + " MW");
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_total / 100000, 2) + " MW");
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    }
                    break;


/*
                case "Drain_Total":
                    GUILayout.Label("Power Current: " + LCARS.lPowSys.get_consumption_total() / 100000 + " MW");
                    GUILayout.Label("Power Total: " + LCARS.lPowSys.get_consumption_total(true) / 100000 + " MW");
                    break;

                case "Drain_Main":
                    GUILayout.Label("Main System Power Total: " + LCARS.lPowSys.get_consumption_main_systems(true) / 100000 + " MW");
                    GUILayout.BeginHorizontal(); 
                    GUILayout.Label("System");  GUILayout.FlexibleSpace();
                    GUILayout.Label("Current:"); GUILayout.FlexibleSpace();
                    GUILayout.Label("Total: "); 
                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical(scrollview_style);
                    PowerSystem_MainSystem_ScrollPosition = GUILayout.BeginScrollView(PowerSystem_MainSystem_ScrollPosition);
                    foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lPowSys.getPowerTakers())
                    {
                        if (pair.Value.powerSystem_takerType == "MainSystem")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label(pair.Value.name); GUILayout.FlexibleSpace();
                            GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_current / 100000, 7) + " MW"); GUILayout.FlexibleSpace();
                            GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_total / 100000, 2) + " MW");
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;

                case "Drain_Sub":
                    GUILayout.Label("    " + LCARS.lPowSys.get_consumption_sub_systems(true) / 100000 + " MW");
                    GUILayout.BeginHorizontal(); 
                    GUILayout.Label("System");  GUILayout.FlexibleSpace();
                    GUILayout.Label("Current:"); GUILayout.FlexibleSpace();
                    GUILayout.Label("Total: "); 
                    GUILayout.EndHorizontal();
                        GUILayout.BeginVertical(scrollview_style);
                        PowerSystem_SubSystem_ScrollPosition = GUILayout.BeginScrollView(PowerSystem_SubSystem_ScrollPosition);
                        foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lPowSys.getPowerTakers())
                        {
                            if (pair.Value.powerSystem_takerType == "SubSystem")
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(pair.Value.name); GUILayout.FlexibleSpace();
                                GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_current / 100000, 7) + " MW"); GUILayout.FlexibleSpace();
                                GUILayout.Label(Math.Round(pair.Value.powerSystem_consumption_total / 100000, 2) + " MW");
                                GUILayout.EndHorizontal();
                            }
                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    break;
*/

                case "Production_EC":
                    scrollview_style.fixedHeight = 190;
                        GUILayout.BeginVertical(scrollview_style);
                        PowerSystem_Generators1_ScrollPosition = GUILayout.BeginScrollView(PowerSystem_Generators1_ScrollPosition);
                    //thisVessel.LCARS_VesselResourceGenerators
                    foreach (KeyValuePair<int, ModuleGenerator> pair in thisVessel.LCARSVessel_ResourceGenerators())
                    {
                        GUILayout.BeginVertical();
                        List<ModuleGenerator.GeneratorResource> MG_GR = pair.Value.outputList;
                        foreach (ModuleGenerator.GeneratorResource GR in MG_GR)
                        {
                            if (GR.name == "ElectricCharge")
                            {
                                pair.Value.generatorIsActive = GUILayout.Toggle(pair.Value.generatorIsActive, "Generator " + (pair.Key + 1) + " is active");
                                GUILayout.Label(GR.name + ": " + Math.Round(GR.rate / 100000, 12) + " MW");
                                //GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, MaxPowerGeneratorRate, "myslider", "mysliderThumb");



                                //GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, LCARS.lODN.ShipStatus.MaxPowerGeneratorRate, PowerSlider, "horizontalSliderThumb");
                                GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, LCARS.lODN.ShipStatus.MaxPowerGeneratorRate, PowerSlider, PowerSliderThumb);
                                
                                
                                
                                if (GR.rate > LCARS.lODN.ShipStatus.CoreOverHeating_PowerRate)
                                {
                                    GUILayout.Label("WARNING: You are overheating the core");
                                    GUILayout.Label("Core temp grow rate: " + LCARS.lODN.ShipStatus.LCARS_ImpulseDrive_Part_add_heat);
                                }
                                float heat_percentage = LCARS.lODN.ShipStatus.AveragePartTemperature / (LCARS.lODN.ShipStatus.AveragePartTemperatureMax / 100);
                                GUILayout.Label("Vessel Temp Max: " + Math.Round(LCARS.lODN.ShipStatus.AveragePartTemperatureMax, 2) + "");
                                GUILayout.Label("Vessel Temp avg: " + Math.Round(LCARS.lODN.ShipStatus.AveragePartTemperature, 2) + "");
                                GUILayout.Label("Core Temp perc.: " + Math.Round(heat_percentage, 2) + "%");
                            }
                        }
                        GUILayout.EndVertical();
                    }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    break;

                case "Production_Other":
                    scrollview_style.fixedHeight = 190;
                        GUILayout.BeginVertical(scrollview_style);
                        PowerSystem_Generators2_ScrollPosition = GUILayout.BeginScrollView(PowerSystem_Generators2_ScrollPosition);
                    //thisVessel.LCARS_VesselResourceGenerators
                    foreach (KeyValuePair<int, ModuleGenerator> pair in thisVessel.LCARSVessel_ResourceGenerators())
                    {
                        GUILayout.BeginVertical();
                        List<ModuleGenerator.GeneratorResource> MG_GR = pair.Value.outputList;
                        foreach(ModuleGenerator.GeneratorResource GR in MG_GR)
                        {
                            if (GR.name != "ElectricCharge")
                            {
                                pair.Value.generatorIsActive = GUILayout.Toggle(pair.Value.generatorIsActive, "Generator "+(pair.Key+1)+" is active");
                                GUILayout.Label(GR.name + ": " + GR.rate);
                                //GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, MaxPowerGeneratorRate, "myslider", "mysliderThumb");
                                //GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, LCARS.lODN.ShipStatus.MaxPowerGeneratorRate);
                                GR.rate = GUILayout.HorizontalSlider(GR.rate, 0F, LCARS.lODN.ShipStatus.MaxPowerGeneratorRate, PowerSlider, PowerSliderThumb);
                            }
                        }
                        GUILayout.EndVertical();
                    }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    break;

                case "Status":
                    break;


                default:
                    break;

            }

            
            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems getGUI done ");
        }
    }

}
