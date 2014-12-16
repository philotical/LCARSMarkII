using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_ReplicatorStation : PartModule
    {

        LCARSMarkII LCARS = null;
        Vessel thisVessel;
        public void init(Vessel v,string ShipSystemID, Transform GameObjectLocation)
        {
            thisVessel = v;
            if (LCARS==null)
            {
                UnityEngine.Debug.Log("ReplicatorStation init");
                try
                {
                    LCARS = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("ReplicatorStation init: LCARSRef ex=" + ex);
                }
            }
            if (LCARS != null)
            {
                if (!LCARS.lODN.ShipSystems.ContainsKey(""))
                {
                    LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                    ShipSystem.name = "ShipReplicator";
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
                }
            }

        }

        public void GUI(Transform ReplicatorLocation)
        {
            if (LCARSRef==null) // because onStart is too early sometimes..
            {
                try
                {
                    LCARSRef = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("ReplicatorStation GUI: LCARSRef ex=" + ex);
                }
            }


            ReplicatorManagementWindowPosition = GUILayout.Window(ReplicatorManagementWindowID, ReplicatorManagementWindowPosition, ReplicatorManagementWindow_GUI, "");

            
        }

        void ReplicatorManagementWindow_GUI(int ReplicatorManagementWindowID)
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label("Replicator ToDo", GUILayout.Width(280));

            if (LCARSRef.gravityEnabled)
            {
                get_console();
            }
            else
            {
                GUILayout.Label("Connection to LCARS is disabled!", GUILayout.Width(280));
                if (GUILayout.Button("Connect with LCARS"))
                {
                    LCARSRef = this.vessel.GetComponent<LCARS_ImpulseDrive>();
                    if (LCARSRef != null)
                    {
                        LCARS_is_installed = true;
                    }
                    LCARSRef.gravityEnabled = true;
                    LCARSRef.lAudio.play("LCARS_ImpulseOn", ReactorCoreManagementNode.gameObject);
                }

            }

            GUILayout.EndVertical();
        }

        private void get_console()
        {
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label("Select your Poison:", GUILayout.Width(280));

            if (GUILayout.Button("Earl Gray, Hot"))
            {
                LCARSRef.lAudio.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Earl Gray, Hot ");
            }
            if (GUILayout.Button("Finelian Toddy"))
            {
                LCARSRef.lAudio.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Finelian Toddy ");
            }
            if (GUILayout.Button("Panfried Catfish"))
            {
                LCARSRef.lAudio.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Panfried Catfish ");
            }

            GUILayout.EndVertical();
        }
    
    }
}
