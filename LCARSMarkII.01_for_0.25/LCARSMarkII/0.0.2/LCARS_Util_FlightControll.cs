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
        internal Dictionary<string, Vessel> ImpulseVesselList;

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
            UnityEngine.Debug.Log("LCARS_Util_FlightControll:MonoBehaviour Start : running ");
            if (HighLogic.LoadedSceneIsEditor)
            {
                return;
            }

            //EVAPartModules.Add("LCARS_Tricorder");
            EVAPartModules.Add("MagBoots");

            UnityEngine.Debug.Log("LCARS_Util_FlightControll:MonoBehaviour Start : done ");
        }
        void FixedUpdate()
        {
            //UnityEngine.Debug.Log("LCARS_Util_FlightControll:MonoBehaviour FixedUpdate : running ");
            if (HighLogic.LoadedSceneIsEditor)
                return;


            if (FlightGlobals.ready)
            {
                this.ImpulseVesselList = null;
                this.ImpulseVesselList = new Dictionary<string, Vessel>();
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v == null)
                    { continue; }

                    if (!v.loaded)
                    { continue; }



                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found v.vesselName=" + v.vesselName);
                    try
                    {
                        LCARS = v.GetComponent<LCARSMarkII>();
                    }
                    catch { continue; }
                    if (LCARS != null)
                    {
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found LCARSMarkII ");

                        /*
                         Keep Impulse Vessel List up to date
                         */
                        if (!this.ImpulseVesselList.ContainsKey(v.id.ToString()))
                        {
                            this.ImpulseVesselList.Add(v.id.ToString(), thisVessel);
                        }
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : ImpulseVesselList done ");

                        /*
                         keep all impulse vessel floating
                         */
                        if (v.LCARS().lODN.ShipSystems["PropulsionMatrix"].isNominal && v.LCARS().lODN.ShipSystems["HoverForce"].isNominal)
                        {

                            geeVector = FlightGlobals.getGeeForceAtPosition(v.findWorldCenterOfMass());
                            var antiGeeVector = geeVector * -1;

                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : found active LCARSMarkII "+v.vesselName);
                            LCARS.grav.CancelG2(v, geeVector);
                            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : LCARSMarkII CancelG2 done ");

                        }
                        //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : loop end ");

                    }
                    //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : FlightGlobals.Vessels done ");


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

                //HashSet<Vessel> gamevessels = new HashSet<Vessel>();
                //HashSet<Vessel> LCARSvessels = new HashSet<Vessel>();

                //gamevessels.IntersectWith(LCARSvessels);

                /*
                UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList begin ");
                foreach (Vessel checkV in this.ImpulseVesselList.Values)
                {
                    bool delete = true;
                    UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList loop start ");
                    foreach (Vessel compareV in FlightGlobals.Vessels)
                    {
                        UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList searching ship ");
                        if (checkV.id.ToString() == compareV.id.ToString())
                        {
                            UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList found ship ");
                            UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate :  clean ImpulseVesselList keep checkV.vesselName" + checkV.vesselName);
                            delete = false;
                        }
                    }
                    UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList v=" + checkV.vesselName + " delete=" + delete);
                    if (delete)
                    {
                        UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList checkV=" + checkV.vesselName + " delete start");
                        ImpulseVesselList.Remove(checkV.id.ToString());
                        UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : clean ImpulseVesselList checkV=" + checkV.vesselName + " delete done ");
                    }
                }
                */


            }
            //UnityEngine.Debug.Log("LCARS_Util_FlightControll: FixedUpdate : done ");

        }
    }
}






















