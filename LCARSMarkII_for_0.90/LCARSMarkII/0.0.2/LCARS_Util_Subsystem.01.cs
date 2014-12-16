
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LCARSMarkII
{


    public class LCARS_Subsystem_Util
    {
        private GUIStyle SubSys_BackGroundLayoutStyle1 = null;
        private GUIStyle SubSys_BackGroundLayoutStyle2 = null;
        private GUIStyle SubSys_BackGroundLayoutStyle3 = null;
        private GUIStyle SubSys_BackGroundLayoutStyle4 = null;
        private GUIStyle SubSys_BackGroundLayoutStyle5 = null;
        private GUIStyle toggle_style = null;
        private GUIStyle caption_style = null;
        Dictionary<string, PartModule> LCARS_VesselPartModules = null;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        Dictionary<string, ILCARSPlugin> _Plugins;
        ICollection<ILCARSPlugin> plugins;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel begin");

            thisVessel = vessel;
            LCARS = thisVessel.LCARS();

            if (_Plugins==null)
            {
                LCARS_VesselPartModules = thisVessel.LCARSVessel_PartModules();

                //UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel ILCARSPlugin");
                _Plugins = new Dictionary<string, ILCARSPlugin>();
                plugins = GenericPluginLoader<ILCARSPlugin>.LoadPlugins();
                foreach (var item in plugins)
                {
                    _Plugins.Add(item.subsystemName, item);

                    // skip if partmodule required but not in cfg
                    if (item.subsystemIsPartModule && !LCARS_VesselPartModules.ContainsKey(item.GetType().FullName.ToString()))
                    { continue; }

                    LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                    ShipSystem.name = item.subsystemName;
                    ShipSystem.type = "SubSystem";
                    ShipSystem.vessel = thisVessel;
                    ShipSystem.disabled = false;
                    ShipSystem.damaged = false;
                    ShipSystem.damagable = true;
                    ShipSystem.show_in_MODNJ = true;
                    ShipSystem.integrity = 100;
                    ShipSystem.plugin_instance = item;
                    ShipSystem.plugin_type = item.GetType();
                    ShipSystem.powerSystem_consumption_current = 0f;
                    ShipSystem.powerSystem_consumption_total = 0f;
                    ShipSystem.powerSystem_takerType = "SubSystem";
                    ShipSystem.powerSystem_takerSubType = item.subsystemStation;
                    ShipSystem.powerSystem_L1_usage = item.subsystemPowerLevel_standby;
                    ShipSystem.powerSystem_L2_usage = item.subsystemPowerLevel_running;
                    ShipSystem.powerSystem_L3_usage = item.subsystemPowerLevel_additional;
                    LCARS.lODN.ShipSystems.Add(item.subsystemName, ShipSystem);
                    //LCARS.lPowSys.setPowerTaker(ShipSystem.name, "SubSystem", item.subsystemStation, item.subsystemPowerLevel_standby, item.subsystemPowerLevel_running, item.subsystemPowerLevel_additional);


                    //item.getGUI();
                }
                LCARS.lODN.ShipSystems["Damage Controll System"].damagable = false;
                LCARS.lODN.ShipSystems["Main ODN Junction"].damagable = false;
                LCARS.lODN.ShipSystems["Damage Controll System"].show_in_MODNJ = false;
                LCARS.lODN.ShipSystems["Main ODN Junction"].show_in_MODNJ = false;

            }

            // UnityEngine.Debug.Log("LCARS_Subsystem_Util SetVessel done");
        }

        Dictionary<string, bool> backupbool_panelstate = null;
        public void getGUI(string station)
        {
            if (SubSys_BackGroundLayoutStyle1 == null)
            {
                SubSys_BackGroundLayoutStyle1 = new GUIStyle(GUI.skin.box);
                SubSys_BackGroundLayoutStyle1.alignment = TextAnchor.MiddleCenter;
                SubSys_BackGroundLayoutStyle1.padding = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle1.margin = new RectOffset(0, 0, 2, 0);
                SubSys_BackGroundLayoutStyle1.fixedHeight = 17;
                SubSys_BackGroundLayoutStyle1.fixedWidth = 466;
                SubSys_BackGroundLayoutStyle1.normal.textColor = Color.white;
                //SubSys_BackGroundLayoutStyle1.fixedHeight = 443;

                SubSys_BackGroundLayoutStyle2 = new GUIStyle(GUI.skin.box);
                SubSys_BackGroundLayoutStyle2.alignment = TextAnchor.MiddleCenter;
                SubSys_BackGroundLayoutStyle2.padding = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle2.margin = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle2.fixedHeight = 6;
                SubSys_BackGroundLayoutStyle2.fixedWidth = 466;
                SubSys_BackGroundLayoutStyle2.normal.textColor = Color.white;
                //SubSys_BackGroundLayoutStyle2.fixedHeight = 443;

                SubSys_BackGroundLayoutStyle3 = new GUIStyle(GUI.skin.box);
                SubSys_BackGroundLayoutStyle3.alignment = TextAnchor.MiddleCenter;
                SubSys_BackGroundLayoutStyle3.padding = new RectOffset(9, 5, 0, 0);
                SubSys_BackGroundLayoutStyle3.margin = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle3.fixedWidth = 466;
                SubSys_BackGroundLayoutStyle3.normal.textColor = Color.white;
                //SubSys_BackGroundLayoutStyle3.fixedHeight = 443;

                SubSys_BackGroundLayoutStyle4 = new GUIStyle(GUI.skin.box);
                SubSys_BackGroundLayoutStyle4.alignment = TextAnchor.MiddleCenter;
                SubSys_BackGroundLayoutStyle4.padding = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle4.margin = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle4.fixedWidth = 466;
                SubSys_BackGroundLayoutStyle4.fixedHeight = 6;
                SubSys_BackGroundLayoutStyle4.normal.textColor = Color.white;
                //SubSys_BackGroundLayoutStyle4.fixedHeight = 443;

                SubSys_BackGroundLayoutStyle5 = new GUIStyle(GUI.skin.box);
                SubSys_BackGroundLayoutStyle5.alignment = TextAnchor.MiddleCenter;
                SubSys_BackGroundLayoutStyle5.padding = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle5.margin = new RectOffset(0, 0, 0, 0);
                SubSys_BackGroundLayoutStyle5.fixedWidth = 466;
                SubSys_BackGroundLayoutStyle5.fixedHeight = 17;
                SubSys_BackGroundLayoutStyle5.normal.textColor = Color.white;
            }
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                //toggle_style.alignment = TextAnchor.MiddleCenter;
                //toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(10, 0, -2, 0);
                toggle_style.fontSize = 12;
                toggle_style.fixedHeight = 14;
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
                toggle_style.active.textColor = Color.black;
                toggle_style.fontStyle = FontStyle.Bold;
            }
            if (caption_style == null)
            {
                caption_style = new GUIStyle();
                //caption_style.alignment = TextAnchor.MiddleCenter;
                //caption_style.padding = new RectOffset(0, 0, 0, 0);
                caption_style.margin = new RectOffset(20, 0, -4, 0);
                caption_style.fontSize = 12;
                caption_style.fixedHeight = 14;
                //caption_style.imagePosition = ImagePosition.ImageOnly;
                caption_style.normal.textColor = Color.black;
                caption_style.active.textColor = Color.black;
                caption_style.fontStyle = FontStyle.Italic;
            }

            //UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI begin");
            foreach (ILCARSPlugin _subsys in _Plugins.Values)
            {



                //UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI 1 ");
                if (_subsys.subsystemStation == station)
                {
                    if (!LCARS.lODN.ShipSystems[_subsys.subsystemName].isNominal)
                    {
                        GUILayout.Label(_subsys.subsystemName + " not operational");
                        continue;
                    }


                    // skip if partmodule required but not in cfg
                    if (_subsys.subsystemIsPartModule && !LCARS_VesselPartModules.ContainsKey(_subsys.GetType().FullName.ToString()))
                    {continue;}

                        SubSys_BackGroundLayoutStyle1.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/SubSys_" + station + "_toggle", false);
                        SubSys_BackGroundLayoutStyle1.onNormal.background = SubSys_BackGroundLayoutStyle1.normal.background;
                        SubSys_BackGroundLayoutStyle1.onHover.background = SubSys_BackGroundLayoutStyle1.normal.background;
                        
                        SubSys_BackGroundLayoutStyle2.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/SubSys_" + station + "_open_top", false);
                        SubSys_BackGroundLayoutStyle2.onNormal.background = SubSys_BackGroundLayoutStyle2.normal.background;
                        SubSys_BackGroundLayoutStyle2.onHover.background = SubSys_BackGroundLayoutStyle2.normal.background;
                    
                        SubSys_BackGroundLayoutStyle3.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/SubSys_" + station + "_open_content", false);
                        SubSys_BackGroundLayoutStyle3.onNormal.background = SubSys_BackGroundLayoutStyle3.normal.background;
                        SubSys_BackGroundLayoutStyle3.onHover.background = SubSys_BackGroundLayoutStyle3.normal.background;
                        
                        SubSys_BackGroundLayoutStyle4.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/SubSys_" + station + "_open_bottom", false);
                        SubSys_BackGroundLayoutStyle4.onNormal.background = SubSys_BackGroundLayoutStyle4.normal.background;
                        SubSys_BackGroundLayoutStyle4.onHover.background = SubSys_BackGroundLayoutStyle4.normal.background;
                        
                        SubSys_BackGroundLayoutStyle5.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/SubSys_" + station + "_closed", false);
                        SubSys_BackGroundLayoutStyle5.onNormal.background = SubSys_BackGroundLayoutStyle5.normal.background;
                        SubSys_BackGroundLayoutStyle5.onHover.background = SubSys_BackGroundLayoutStyle5.normal.background;


                    //UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI 2 ");
                    float subsys_width = 0f;
                    switch(station)
                    {
                        case "Bridge":
                            subsys_width = 466f;
                            break;
                        case "Helm":
                            subsys_width = 466f;
                            break;
                        case "Engineering":
                            subsys_width = 466f;
                            break;
                        case "Tactical":
                            subsys_width = 466f;
                            break;
                        case "Science":
                            subsys_width = 466f;
                            break;
                        case "Communication":
                            subsys_width = 301f;
                            break;
                    }

                    if (backupbool_panelstate == null)
                    {
                        backupbool_panelstate = new Dictionary<string, bool>();
                    }
                    if (!backupbool_panelstate.ContainsKey(_subsys.subsystemName))
                    {
                        backupbool_panelstate.Add(_subsys.subsystemName, _subsys.subsystemPanelState);
                    }


                    // button row start 
                        GUILayout.BeginVertical(SubSys_BackGroundLayoutStyle1, GUILayout.Width(subsys_width), GUILayout.Height(17));
                            _subsys.subsystemPanelState = GUILayout.Toggle(_subsys.subsystemPanelState, _subsys.subsystemName, toggle_style);
                        GUILayout.EndVertical();
                    // button row end

                    if (backupbool_panelstate[_subsys.subsystemName] != _subsys.subsystemPanelState)
                    {
                        UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI lAudio.play _subsys.subsystemName=" + _subsys.subsystemName + " _subsys.subsystemPanelState=" + _subsys.subsystemPanelState);
                        if (_subsys.subsystemPanelState)
                        {
                            try
                            {
                                UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI LCARS_SubsystemOpen ");
                                LCARS.lAudio.play("LCARS_SubsystemOpen", Camera.main.transform);
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI LCARS_SubsystemClose ");
                                LCARS.lAudio.play("LCARS_SubsystemClose", Camera.main.transform);
                            }
                            catch { }
                        }
                        backupbool_panelstate[_subsys.subsystemName] = _subsys.subsystemPanelState;
                        UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI lAudio.play done ");
                    }


                    if (_subsys.subsystemPanelState)
                    {


                                // top border-row start
                                GUILayout.BeginVertical(SubSys_BackGroundLayoutStyle2, GUILayout.Width(subsys_width), GUILayout.Height(6)); // top row
                                caption_style.fontSize = 2;
                                caption_style.fixedHeight = 2;
                                GUILayout.Label("", caption_style);
                                GUILayout.EndVertical();
                                // top border-row end


                                /*
                                */
                                    // content row start
                                        GUILayout.BeginVertical(SubSys_BackGroundLayoutStyle3, GUILayout.Width(subsys_width)); //middle row
                                        try
                                        {
                                            LCARS.lPowSys.draw(_subsys.subsystemName, _subsys.subsystemPowerLevel_standby);
                                            _subsys.SetVessel(thisVessel);
                                            _subsys.getGUI();
                                        }
                                        catch (Exception ex) { GUILayout.Label("Subsystem Malfunction! Check logs for details"); UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI Subsystem Malfunction ex="+ex); }
                                        GUILayout.EndVertical();
                                    // content row end
                                        /*
                                        */

                          
                          
                            // bottom border-row start
                            GUILayout.BeginVertical(SubSys_BackGroundLayoutStyle4, GUILayout.Width(subsys_width), GUILayout.Height(6)); // bottom row
                                caption_style.fontSize = 2;
                                caption_style.fixedHeight = 2;
                                GUILayout.Label("", caption_style);
                            GUILayout.EndVertical();
                            // bottom border-row end
                        /*
                       */


                    }
                    else 
                    {
                        LCARS.lODN.ShipSystems[_subsys.subsystemName].powerSystem_consumption_current = 0f;

                        // closed content row start 
                        GUILayout.BeginVertical(SubSys_BackGroundLayoutStyle5, GUILayout.Width(subsys_width), GUILayout.Height(17));
                            caption_style.fontSize = 12;
                            caption_style.fixedHeight = 14;
                            GUILayout.Label(_subsys.subsystemDescription, caption_style);
                        GUILayout.EndVertical();
                        // closed content row end 
                    }



                }
            }
            //UnityEngine.Debug.Log("LCARS_Subsystem_Util getGUI done");
        }
    }




    /* 
       Copyright 2013 Christoph Gattnar 
 
       Licensed under the Apache License, Version 2.0 (the "License"); 
       you may not use this file except in compliance with the License. 
       You may obtain a copy of the License at 
 
           http://www.apache.org/licenses/LICENSE-2.0 
 
       Unless required by applicable law or agreed to in writing, software 
       distributed under the License is distributed on an "AS IS" BASIS, 
       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
       See the License for the specific language governing permissions and 
       limitations under the License. 
    */


        public static class GenericPluginLoader<T>
        {
            public static ICollection<T> LoadPlugins()
            {

                UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins begin");

                Type pluginType = typeof(T);
                ICollection<Type> pluginTypes = new List<Type>();

               // UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 1");

                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in loadedAssemblies)
                {
                    //UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 2 assembly.FullName=" + assembly.FullName);
                    if (assembly != null && (assembly.FullName.Contains("LCARS_Subsystem_") || assembly.FullName.Contains("LCARS_Subsystems_")))
                    {
                        Type[] types = null;
                        try
                        {
                            types = assembly.GetTypes();
                        }
                        catch (Exception ex) { continue; }

                        //UnityEngine.Debug.Log("LCARS GenericPluginLoader LoadPlugins 3 assembly.FullName=" + assembly.FullName);

                        foreach (Type type in types)
                        {
                            //UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 4 type.FullName=" + type.FullName);

                            if (type.IsInterface || type.IsAbstract)
                            {
                                //UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 5 skipping ");

                                continue;
                            }
                            else
                            {
                                //UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 6 ");

                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    //UnityEngine.Debug.Log("LCARS GenericPluginLoader LoadPlugins 7 type=" + type);

                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }
                //UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins 8  ");


                ICollection<T> plugins = new List<T>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    UnityEngine.Debug.Log("GenericPluginLoader LoadPlugins type=" + type);

                    T plugin = (T)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }
                UnityEngine.Debug.Log("LCARS GenericPluginLoader LoadPlugins done, Total Subsystems loaded = " + plugins.Count);


                return plugins;

            }
        }



}
