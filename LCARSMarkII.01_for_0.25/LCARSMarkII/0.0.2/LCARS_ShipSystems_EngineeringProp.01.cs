using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_EngineeringProp
    {

        LCARSMarkII LCARS = null;
        Vessel thisVessel = null;
        Transform GameObjectLocation = null;
        string ShipSystemID = null;
        float ActivationDistance = 0;
        public void setVessel(Vessel _thisVessel)
        {
            if (thisVessel == null)
            {
                thisVessel = _thisVessel;
            }
            if (LCARS == null)
            {
                UnityEngine.Debug.Log("LCARS_EngineeringProp setVessel");
                try
                {
                    LCARS = thisVessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("LCARS_EngineeringProp setVessel: LCARSRef ex=" + ex);
                }
            }

        }
        public void setShipSystemID(string _ShipSystemID)
        {
            if (ShipSystemID == null)
            {
                UnityEngine.Debug.Log("LCARS_EngineeringProp setShipSystemID _ShipSystemID=" + _ShipSystemID);
                ShipSystemID = _ShipSystemID;
            }
        }
        public void setGameObjectLocation(Transform _GameObjectLocation)
        {
            if (GameObjectLocation == null)
            {
                UnityEngine.Debug.Log("LCARS_EngineeringProp setGameObjectLocation attempt to register GameObjectLocation ");
                GameObjectLocation = _GameObjectLocation;
            }
        }
        public void setActivationDistance(float _ActivationDistance)
        {
            //UnityEngine.Debug.Log("LCARS_EngineeringProp setActivationDistance attempt to register ActivationDistance ");
            ActivationDistance = _ActivationDistance;
        }

        float dist = 0f;
        bool sound1_was_played = false;
        bool sound2_was_played = false;
        void runProcessingLogic()
        {
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate 1 ");
            if (GameObjectLocation == null)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate 2 ");
            if (FlightGlobals.ActiveVessel.vesselType != VesselType.EVA)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate 3 ");
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
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate 4 ");
            if (LCARS == null)
            {
                return;
            }
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate 5 ");
            
            LCARS.WindowState_bypass = false;


            Transform kerbal = FlightGlobals.ActiveVessel.rootPart.transform;
            dist = Vector3.Distance(GameObjectLocation.position, kerbal.position);
            //UnityEngine.Debug.Log("LCARS_EngineeringProp FixedUpdate attempt to messure ActivationDistance dist=" + dist);
            if (dist < 1f)
            {
                if (!sound1_was_played)
                {
                    try
                    {
                        //LCARS.lAudio.play("LCARS_SubsystemOpen", GameObjectLocation.gameObject);
                    }
                    catch (Exception ex) { }
                    sound1_was_played = true;
                    sound2_was_played = false;
                    LCARS.lWindows.setWindowState("Engineering", true);
                    LCARS.lWindows.LCARSWindows["Communication"].state = false;
                    LCARS.lWindows.LCARSWindows["Science"].state = false;
                    LCARS.lWindows.LCARSWindows["Tactical"].state = false;
                    LCARS.lWindows.LCARSWindows["Helm"].state = false;
                    LCARS.lWindows.LCARSWindows["Bridge"].state = false;
                    LCARS.WindowState_bypass = true;
                }
                LCARS.lWindows.DrawWindows();
            }
            else
            {
                if (!sound2_was_played)
                {
                    //try { LCARS.lAudio.play("LCARS_SubsystemClose", GameObjectLocation.gameObject); }
                    //catch (Exception ex) { }
                    sound2_was_played = true;
                    sound1_was_played = false;
                    LCARS.lWindows.setWindowState("Engineering", false);
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


        }


    }
}
