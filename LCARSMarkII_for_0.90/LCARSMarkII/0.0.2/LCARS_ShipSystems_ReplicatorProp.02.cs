using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_ReplicatorProp
    {

        LCARSMarkII LCARS = null;
        Vessel thisVessel = null;
        Transform GameObjectLocation = null;
        string ShipSystemID = null;
        float ActivationDistance = 0;
        public void setVessel(Vessel _thisVessel)
        {
            if (thisVessel==null)
            {
                thisVessel = _thisVessel;
            }
            if (LCARS == null)
            {
                UnityEngine.Debug.Log("LCARS_ReplicatorProp setVessel");
                try
                {
                    LCARS = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("LCARS_ReplicatorProp setVessel: LCARSRef ex=" + ex);
                }
            }

        }
        public void setShipSystemID(string _ShipSystemID)
        {
            if (ShipSystemID == null)
            {
                UnityEngine.Debug.Log("LCARS_ReplicatorProp setShipSystemID _ShipSystemID=" + _ShipSystemID);
                ShipSystemID = _ShipSystemID;
            }
            try
            {
                if (LCARS != null && ShipSystemID != null)
                {
                    if (!LCARS.lODN.ShipSystems.ContainsKey(ShipSystemID))
                    {
                        UnityEngine.Debug.Log("LCARS_ReplicatorProp setShipSystemID attempt to register ShipSystemID=" + ShipSystemID);
                        LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
                        ShipSystem.name = ShipSystemID;
                        ShipSystem.type = "SubSystem";
                        ShipSystem.vessel = thisVessel;
                        ShipSystem.disabled = false;
                        ShipSystem.damaged = false;
                        ShipSystem.damagable = true;
                        ShipSystem.show_in_MODNJ = true;
                        ShipSystem.integrity = 100;
                        ShipSystem.powerSystem_consumption_current = 0f;
                        ShipSystem.powerSystem_consumption_total = 0f;
                        ShipSystem.powerSystem_takerType = "SubSystem";
                        ShipSystem.powerSystem_takerSubType = "Engineering";
                        ShipSystem.powerSystem_L1_usage = 1f;
                        ShipSystem.powerSystem_L2_usage = 5f;
                        ShipSystem.powerSystem_L3_usage = 10f;
                        LCARS.lODN.ShipSystems.Add(ShipSystem.name, ShipSystem);
                    }
                }
            }
            catch { }
        }
        public void setGameObjectLocation(Transform _GameObjectLocation)
        {
            if (GameObjectLocation == null)
            {
                UnityEngine.Debug.Log("LCARS_ReplicatorProp setGameObjectLocation attempt to register GameObjectLocation ");
                GameObjectLocation = _GameObjectLocation;
            }
        }
        public void setActivationDistance(float _ActivationDistance)
        {
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp setActivationDistance attempt to register ActivationDistance ");
            ActivationDistance = _ActivationDistance;
        }

        float dist = 0f;
        bool sound1_was_played = false;
        bool sound2_was_played = false;
        bool ShowWindow = false;
        void runProcessingLogic()
        {
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate 1 ");
            if (GameObjectLocation == null)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate 2 ");
            if (FlightGlobals.ActiveVessel.vesselType != VesselType.EVA)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate 3 ");
            if (LCARS == null) // because onStart is too early sometimes..
            {
                try
                {
                    LCARS = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    //UnityEngine.Debug.Log("ReplicatorStation GUI: LCARSRef ex=" + ex);
                }
            }
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate 4 ");
            if (LCARS == null)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate 5 ");

            Transform kerbal = FlightGlobals.ActiveVessel.rootPart.transform;
            dist = Vector3.Distance(GameObjectLocation.position, kerbal.position);
            //UnityEngine.Debug.Log("LCARS_ReplicatorProp FixedUpdate attempt to messure ActivationDistance dist=" + dist);
            if (dist < 1f)
            {
                if (!sound1_was_played) 
                {
                    try 
                    {
                        LCARS.lAudio.play("LCARS_SubsystemOpen", LCARS.thisVessel); 
                    }
                    catch (Exception ex) { }
                }
                sound1_was_played = true;
                sound2_was_played = false;
                ShowWindow = true;
            }
            else
            {
                if (!sound2_was_played)
                {
                    try { LCARS.lAudio.play("LCARS_SubsystemClose", LCARS.thisVessel); }
                    catch (Exception ex) { }
                    sound2_was_played = true;
                    sound1_was_played = false;
                    ShowWindow = false;
                }
            }
            
        }

        private Rect ManagementWindowPosition = new Rect(120, 120, 380, 230);
        private int ManagementWindowID = new System.Random().Next();
        public void GUI()
        {
            if (LCARS == null) // because onStart is too early sometimes..
            {
                try
                {
                    LCARS = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    //UnityEngine.Debug.Log("ReplicatorStation GUI: LCARSRef ex=" + ex);
                }
            }

            runProcessingLogic();

            if (ShowWindow)
            {
                ManagementWindowPosition = GUILayout.Window(ManagementWindowID, ManagementWindowPosition, ManagementWindow_GUI, "");
            }

        }

        void ManagementWindow_GUI(int ManagementWindowID)
        {
            GUILayout.BeginVertical();
            if (LCARS.lODN.ShipSystems[ShipSystemID].isNominal)
            {
                get_console();
            }
            else
            {
                GUILayout.Label(ShipSystemID + " is not operational!");

            }
            GUILayout.EndVertical();
        }

        private void get_console()
        {
            GUILayout.Label(ShipSystemID);
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label("Select your Poison:", GUILayout.Width(280));

            if (GUILayout.Button("Earl Gray, Hot"))
            {
                LCARS.lAudio.play("LCARS_Replicator", LCARS.thisVessel);
                UnityEngine.Debug.Log("ReplicatorStation:  Earl Gray, Hot ");
            }
            if (GUILayout.Button("Finelian Toddy"))
            {
                LCARS.lAudio.play("LCARS_Replicator", LCARS.thisVessel);
                UnityEngine.Debug.Log("ReplicatorStation:  Finelian Toddy ");
            }
            if (GUILayout.Button("Panfried Catfish"))
            {
                LCARS.lAudio.play("LCARS_Replicator", LCARS.thisVessel);
                UnityEngine.Debug.Log("ReplicatorStation:  Panfried Catfish ");
            }

            GUILayout.EndVertical();
        
        }

    }
}
