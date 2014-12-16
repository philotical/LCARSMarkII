using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public sealed class LCARS_Util_FlightControll : MonoBehaviour
    {
        private static LCARS_Util_FlightControll _instance;
        public static LCARS_Util_FlightControll Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LCARS_Util_FlightControll();
                }

                return _instance;
            }
        }

        public Vector3 geeVector = Vector3.up;
        List<string> EVAPartModules = new List<string>();
        public Dictionary<string, Vessel> ImpulseVesselList = null;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            UnityEngine.Debug.Log("LCARS_Util_FlightControll: Start : running ");
            if (HighLogic.LoadedSceneIsEditor)
            {
                return;
            }

            EVAPartModules.Add("LCARS_Tricorder");
            EVAPartModules.Add("MagBoots");

            UnityEngine.Debug.Log("LCARS_Util_FlightControll: Start : done ");
        }


        private float lastUpdate = 0.0f;
        private float lastFixedUpdate = 0.0f;
        private float logInterval = 0.5f;
        void FixedUpdate()
        {
            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : running ");
            if (HighLogic.LoadedSceneIsEditor)
                return;

            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : 1 ");

            if (FlightGlobals.ready)
            {
                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : 2 ");
                if ((Time.time - lastFixedUpdate) > logInterval || ImpulseVesselList == null)
                {
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval it's time 1 ");
                    ImpulseVesselList = new Dictionary<string, Vessel>();
                    

                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval Vessel loop begin ");
                    foreach (Vessel v in FlightGlobals.Vessels)
                    {
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval Vessel loop iteration start ");

                        if (v == null)
                        { continue; }

                        if (!v.loaded)
                        {
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop !v.loaded skipping ");// v.vesselName=" + v.vesselName + " v.id=" + v.id);
                            continue;
                        }
                        else 
                        {
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop v.loaded=true ");//v.vesselName=" + v.vesselName + " v.id=" + v.id);
                        }
                        try
                        {
                            LCARS = v.LCARS(); //.GetComponent<LCARSMarkII>();
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop LCARS found ");//v.vesselName=" + v.vesselName + " v.id=" + v.id);
                        }
                        catch
                        {
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop LCARS failed skipping ");//v.vesselName=" + v.vesselName + " v.id=" + v.id);
                            continue;
                        }


                        /*
                         Keep Impulse Vessel List up to date
                         */
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop attempt to keep Impulse Vessel List up to date begin ");//v.vesselName=" + v.vesselName + " v.id=" + v.id);
                        if (!ImpulseVesselList.ContainsKey(v.id.ToString()))
                        {
                            if (ImpulseVesselList == null)
                            {
                                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval ImpulseVesselList == null ");
                                ImpulseVesselList = new Dictionary<string, Vessel>();
                            }
                            ImpulseVesselList.Add(v.id.ToString(), v);
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop ImpulseVesselList.Add v.vesselName=" + v.vesselName + " v.id=" + v.id + " ImpulseVesselList.Count=" + ImpulseVesselList.Count);
                        }
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop attempt to keep Impulse Vessel List up to date done ");//v.vesselName=" + v.vesselName + " v.id=" + v.id);


                        /*
                         * Add stuff to EVA Kerbals
                         */
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : VesselType.EVA begin ");
                        if (v.vesselType == VesselType.EVA)
                        {
                            foreach (string pm in EVAPartModules)
                            {
                                if (!v.rootPart.Modules.Contains(pm))
                                {
                                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : EVAPartModules pm=" + pm);
                                    v.rootPart.AddModule(pm);
                                }

                            }
                        }
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : VesselType.EVA done ");
                    }
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval Vessel loop done ");

                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval done ");
                }


                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList loop start  ImpulseVesselList.Count=" + ImpulseVesselList.Count);
                foreach (Vessel v in ImpulseVesselList.Values)
                {
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration begin ");

                    if (v == null)
                    { continue; }
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration not null ");

                    if (!v.loaded)
                    { continue; }
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration loaded ");





                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found v.vesselName=" + v.vesselName);
                    try
                    {
                        LCARS = v.LCARS(); //.GetComponent<LCARSMarkII>();
                    }
                    catch { continue; }
                    if (LCARS != null)
                    {
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found LCARSMarkII ");

                        try
                        {
                            if (!LCARS.lODN.ShipSystems.ContainsKey("PropulsionMatrix"))
                            { continue; }
                        }
                        catch { continue; }
                        /*
                         keep all impulse vessel floating
                         */
                        if (LCARS.lODN.ShipSystems["PropulsionMatrix"].isNominal && LCARS.lODN.ShipSystems["HoverForce"].isNominal)
                        {

                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll:MonoBehaviour FixedUpdate : ImpulseVesselList loop v.vesselName=" + v.id + "" + v.id);

                            geeVector = FlightGlobals.getGeeForceAtPosition(v.findWorldCenterOfMass());
                            var antiGeeVector = geeVector * -1;

                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found active LCARSMarkII "+v.vesselName);
                            LCARS.grav.CancelG2(v, geeVector);
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : LCARSMarkII CancelG2 done ");

                        }
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : loop end ");

                    }
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration done ");



                }
                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : vessel loop end ");

                LCARS = null;
                try
                {
                    LCARS = LCARSNCI_Bridge.Instance.getCurrentShipLCARS(); //.GetComponent<LCARSMarkII>();
                }
                catch 
                {
                    if(FlightGlobals.ActiveVessel.vesselType != VesselType.EVA)
                    {
                        LCARS = FlightGlobals.ActiveVessel.LCARS();
                    }
                    else
                    {
                        foreach (Vessel v in ImpulseVesselList.Values)
                        {
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration begin ");
                            if (v == null)
                            { continue; }
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration not null ");
                            if (!v.loaded)
                            { continue; }
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels iteration loaded ");
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found v.vesselName=" + v.vesselName);
                            try
                            {
                                LCARS = v.LCARS(); //.GetComponent<LCARSMarkII>();
                            }
                            catch { continue; }
                        }
                    }
                }
                if (LCARS != null)
                {

                    try
                    {
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary begin ");
                        // scienceEquippment supplementary stuff
                        if (LCARS.lODN.ShipSystems.ContainsKey("Iso Flux Detector") && LCARS.lODN.IFD_object_of_interest != null)
                        {
                            if (LCARS.lODN.ShipSystems["Iso Flux Detector"].isNominal)
                            {
                                float dist = Vector3.Distance(FlightGlobals.ActiveVessel.CoM, LCARS.lODN.IFD_object_of_interest.transform.position);
                                LCARS.lODN.IFD_object_distance = dist;
                                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary Iso Flux Detector running ");
                            }
                            else
                            {
                                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary Iso Flux Detector not nominal ");
                            }
                        }
                    }
                    catch (Exception ex) { UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary failed ex=" + ex); }
                }
                else
                {
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary LCARS == null skipping ");
                }
                //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : logInterval scienceEquippment supplementary done ");
                // scienceEquippment supplementary stuff



            }
            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : done ");

        }
    }
}






















