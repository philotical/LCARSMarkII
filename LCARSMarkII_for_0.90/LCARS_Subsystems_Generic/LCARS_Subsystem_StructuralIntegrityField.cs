using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_StructuralIntegrityField : ILCARSPlugin
    {
        public string subsystemName { get { return "Structural Integrity Field"; } }
        public string subsystemDescription {get{return "Ship Stability Enhancer";}}
        public string subsystemStation { get { return "Engineering"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 200f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
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
            LCARS.lODN.ShipStatus.current_LCARS_SFI_force = SIF_force;
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }



        Transform pitchLevel = new GameObject().transform;
        float SIF_force = 1;
        bool running = false;
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();

                GUILayout.Label("ToDo: " + thisVessel.vesselName);
                if(running)
                {
                    if (GUILayout.Button("Disable"))
                    {
                        running = false;
                        SIF_force = 1;
                    }
                    GUILayout.Label("Set Force: " + Math.Round(SIF_force, 2));
                    SIF_force = GUILayout.HorizontalSlider(SIF_force, 0.0F, 100.0F);
                    set_StructuralIntegrityField(SIF_force);
                    LCARS.lODN.ShipStatus.current_LCARS_SFI_force = SIF_force;
                }
                else
                {
                    if (GUILayout.Button("Enable"))
                    {
                        reset_StructuralIntegrityField();
                        running = true;
                    }
                    GUILayout.Label("The SFI will enhance your ships ability to withstand impacts and external heat sources.");
                }
                GUILayout.Label("Current Force: " + (LCARS.lODN.ShipStatus.current_LCARS_SFI_force-1) + "");
                GUILayout.Label("Current Powerdrain: " + (LCARS.lODN.ShipStatus.current_LCARS_SFI_force - 1) * LCARS.lODN.ShipSystems["Structural Integrity Field"].powerSystem_L2_usage + "MW");
                GUILayout.Label("Current Performance: " + (100*LCARS.lODN.ShipStatus.current_LCARS_SFI_performance) + "%");

            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }




        Dictionary<string, float> backup_values = null;
        Dictionary<Part, Dictionary<string, float>> backup_Parts = null;
        public void reset_StructuralIntegrityField()
        {
            if (backup_Parts != null)
            {
                foreach (Part p in thisVessel.Parts)
                {
                    p.crashTolerance = backup_Parts[p]["crashTolerance"];
                    p.breakingForce = backup_Parts[p]["breakingForce"];
                    p.breakingTorque = backup_Parts[p]["breakingTorque"];
                    p.maxTemp = backup_Parts[p]["maxTemp"];
                }
            }
            backup_Parts = null;
        }

        public void set_StructuralIntegrityField(float force)
        {
            UnityEngine.Debug.Log("LCARS_StructuralIntegrityField: set_StructuralIntegrityField  beginn");
            if (backup_Parts == null)
            {
                backup_Parts = new Dictionary<Part, Dictionary<string, float>>() { };
            }

            foreach (Part p in thisVessel.Parts)
            {
                if (!backup_Parts.ContainsKey(p))
                {
                    backup_values = new Dictionary<string, float>() { };
                    backup_values.Add("crashTolerance", p.crashTolerance);
                    backup_values.Add("breakingForce", p.breakingForce);
                    backup_values.Add("breakingTorque", p.breakingTorque);
                    backup_values.Add("maxTemp", p.maxTemp);
                    backup_values.Add("temperature", p.temperature);
                    backup_Parts.Add(p, backup_values);
                    backup_values = null;
                }

                float current_LCARS_SFI_performance = (LCARS.lODN.ShipStatus.current_LCARS_SFI_performance < 0.1f) ? 0.1f : LCARS.lODN.ShipStatus.current_LCARS_SFI_performance;

                p.crashTolerance = (force >= 1) ? backup_Parts[p]["crashTolerance"] * force * (current_LCARS_SFI_performance) : backup_Parts[p]["crashTolerance"];

                p.breakingForce = (force >= 1) ? backup_Parts[p]["breakingForce"] * force * (current_LCARS_SFI_performance) : backup_Parts[p]["breakingForce"];

                p.breakingTorque = (force >= 1) ? backup_Parts[p]["breakingTorque"] * force * (current_LCARS_SFI_performance) : backup_Parts[p]["breakingTorque"];

                p.maxTemp = (force >= 1) ? backup_Parts[p]["maxTemp"] * (force * current_LCARS_SFI_performance) : backup_Parts[p]["maxTemp"];

                p.temperature = (p.temperature > 20f) ? p.temperature - ((0.005f * force) * current_LCARS_SFI_performance) : p.temperature;

                //FromGO(UnityEngine.GameObject obj)



            }


        }


    
    } 
    
}
