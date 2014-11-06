using System;
using UnityEngine;

namespace LCARSMarkII
{
    public class Aurora : PartModule
    {


        /*
         
         * ScienceProp
         * SensorProp
         * EngineeringProp
         * ReplicatorProp  
         * ReactorProp
         
         */


        public FXGroup reactorSound = null; // Make sure this is public so it can be initialised internally.
       

        private Rect ReactorCoreManagementWindowPosition = new Rect(120, 120, 380, 230);
        private Rect EngineeringManagementWindowPosition = new Rect(120, 120, 380, 230);
        private Rect ScienceManagementWindowPosition = new Rect(120, 120, 380, 230);
        private Rect ScannerManagementWindowPosition = new Rect(120, 120, 380, 230);
        private int ReactorCoreManagementWindowID = new System.Random().Next();
        private int EngineeringManagementWindowID = new System.Random().Next();
        private int ScienceManagementWindowID = new System.Random().Next();
        private int ScannerManagementWindowID = new System.Random().Next();
        
        
        LCARSMarkII LCARSRef;
        
        
        public override void OnStart(StartState state)
        {
            base.OnStart(state);
        }

        [KSPEvent(guiName = "Toggle InternalLight: Off",/**/ category = "Lights", isDefault = true, guiActive = true)]
        public void ToggleInternalLight()
        {
            if (this.vessel.rootPart.FindModelTransform("Internal_Light_01").light.enabled)
            {
                Events["ToggleInternalLight"].guiName = "Toggle InternalLight: Off";

                this.vessel.rootPart.FindModelTransform("Internal_Light_01").light.enabled = false;
                this.vessel.rootPart.FindModelTransform("Internal_Light_02").light.enabled = false;
                this.vessel.rootPart.FindModelTransform("Internal_Light_03").light.enabled = false;
            }
            else
            {
                Events["ToggleInternalLight"].guiName = "Toggle InternalLight: On";
                this.vessel.rootPart.FindModelTransform("Internal_Light_01").light.enabled = true;
                this.vessel.rootPart.FindModelTransform("Internal_Light_02").light.enabled = true;
                this.vessel.rootPart.FindModelTransform("Internal_Light_03").light.enabled = true;
            }
        }

        [KSPEvent(guiName = "Toggle ExternalLight: Off",/**/ category = "Lights", isDefault = true, guiActive = true)]
        public void ToggleExternalLight()
        {
            if (this.vessel.rootPart.FindModelTransform("External_Light_01").light.enabled)
            {
                Events["ToggleExternalLight"].guiName = "Toggle ExternalLight: Off";
                this.vessel.rootPart.FindModelTransform("External_Light_01").light.enabled = false;
                this.vessel.rootPart.FindModelTransform("External_Light_02").light.enabled = false;
                this.vessel.rootPart.FindModelTransform("External_Light_03").light.enabled = false;
            }
            else
            {
                Events["ToggleExternalLight"].guiName = "Toggle ExternalLight: On";
                this.vessel.rootPart.FindModelTransform("External_Light_01").light.enabled = true;
                this.vessel.rootPart.FindModelTransform("External_Light_02").light.enabled = true;
                this.vessel.rootPart.FindModelTransform("External_Light_03").light.enabled = true;
            }
        }

        [KSPEvent(guiName = "Toggle SpotLight: Off",/**/ category = "Lights", isDefault = true, guiActive = true)]
        public void ToggleSpotLight()
        {
            if (this.vessel.rootPart.FindModelTransform("SpotYellow01").light.enabled)
            {
                Events["ToggleSpotLight"].guiName = "Toggle SpotLight: Off";
                this.vessel.rootPart.FindModelTransform("SpotYellow01").light.enabled = false;
                this.vessel.rootPart.FindModelTransform("SpotYellow02").light.enabled = false;
            }
            else
            {
                Events["ToggleSpotLight"].guiName = "Toggle SpotLight: On";
                this.vessel.rootPart.FindModelTransform("SpotYellow01").light.enabled = true;
                this.vessel.rootPart.FindModelTransform("SpotYellow02").light.enabled = true;
            }
        }

        [KSPAction("ToggleInternalLight")]
        public void ToggleInternalLightAction(KSPActionParam param)
        {
            ToggleInternalLight();
        }
        [KSPAction("ToggleExternalLight")]
        public void ToggleExternalLightAction(KSPActionParam param)
        {
            ToggleExternalLight();
        }
        [KSPAction("ToggleSpotLight")]
        public void ToggleSpotLightAction(KSPActionParam param)
        {
            ToggleSpotLight();
        }



        void FixedUpdate()
        {
            //UnityEngine.Debug.Log("Aurora Update:  begin ");



            if (!HighLogic.LoadedSceneIsFlight || this.vessel == null)
            {
                return;
            }


            try
            {
                reactorSound.audio = gameObject.AddComponent<AudioSource>();
                reactorSound.audio.volume = GameSettings.SHIP_VOLUME*0.3f;
                reactorSound.audio.Stop();

                reactorSound.audio.clip = GameDatabase.Instance.GetAudioClip("LCARSMarkII/sounds/LCARS_Reactor");
                reactorSound.audio.loop = true; // Repeat the sound from the start if we reach the end of the file. Use this for engine sounds etc.
                //base.OnStart(state); // Allow OnStart to do what it usually does.
            }
            catch (Exception ex)
            {
                //UnityEngine.Debug.Log("Aurora FixedUpdate:  LCARSRef exception ex.Message=" + ex.Message);

            }


            if (LCARSRef == null)
            {
                try
                {

                    LCARSRef = this.vessel.GetComponent<LCARSMarkII>();
                }
                catch (Exception ex)
                {
                    //UnityEngine.Debug.Log("Aurora FixedUpdate:  LCARSRef exception ex.Message=" + ex.Message);

                }
            }

            try
            {
                    if (!reactorSound.audio.isPlaying)
                    { reactorSound.audio.Play(); }
            }
            catch (Exception ex)
            {
                //UnityEngine.Debug.Log("Aurora FixedUpdate:  reactorSound exception ex.Message=" + ex.Message);

            }


            if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA || FlightGlobals.ActiveVessel.vesselType == VesselType.EVA)
            {
                this.vessel.rootPart.SetHighlight(false); // turn off the vessel highlight while in EVA or IVA
            }

            if (FlightGlobals.ActiveVessel.vesselType != VesselType.EVA)
            {
                return;
            }


        }




        internal LCARS_ReplicatorProp repProp;
        internal Transform Replicator1StationManagementNode = null;

        internal LCARS_EngineeringProp engProp;
        internal Transform EngineeringStationManagementNode = null;

        internal LCARS_ScienceProp sciProp;
        internal Transform ScienceStationManagementNode = null;

        internal LCARS_CommunicationProp comProp;
        internal Transform CommunicationStationManagementNode = null;

        internal LCARS_TacticalProp tacProp;
        internal Transform TacticalStationManagementNode = null;

        internal LCARS_HelmProp helmProp;
        internal Transform HelmStationManagementNode = null;

        internal LCARS_BridgeProp bridgeProp;
        internal Transform BridgeStationManagementNode = null;

        private void OnGUI()
        {
            if (!HighLogic.LoadedSceneIsFlight || this.vessel == null || FlightGlobals.ActiveVessel.vesselType != VesselType.EVA)
            {
                return;
            }

            //UnityEngine.Debug.Log("Aurora OnGUI:  we have an EVA Kerbal ");

            // Find your gameobject that marks the prop in your model
            if (Replicator1StationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find ReplicatorProp gameobject in model ");
                Replicator1StationManagementNode = this.vessel.rootPart.FindModelTransform("ReplicatorProp");
            }
            // initialize the prop class
            if (repProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate repProp class ");
                repProp = new LCARS_ReplicatorProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (Replicator1StationManagementNode != null && repProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  repProp start ");


                repProp.setVessel(this.vessel); // your vessel
                repProp.setGameObjectLocation(Replicator1StationManagementNode); // the gameobject transform
                repProp.setShipSystemID("Replicator Station"); // the name you want for this in Main ODN Junction - string
                repProp.setActivationDistance(1.5f); // how close must the kerbal be to see the GUI - float, in meters
                repProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  repProp done ");
            }


            // Find your gameobject that marks the prop in your model
            if (EngineeringStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find EngineeringProp gameobject in model ");
                EngineeringStationManagementNode = this.vessel.rootPart.FindModelTransform("EngineeringProp");
            }
            // initialize the prop class
            if (engProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate engProp class ");
                engProp = new LCARS_EngineeringProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (EngineeringStationManagementNode != null && engProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  engProp start ");


                engProp.setVessel(this.vessel); // your vessel
                engProp.setGameObjectLocation(EngineeringStationManagementNode); // the gameobject transform
                engProp.setShipSystemID("Engineering"); // the name you want for this in Main ODN Junction - string
                engProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                engProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  engProp done ");
            }


            // Find your gameobject that marks the prop in your model
            if (ScienceStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find ReplicatorProp gameobject in model ");
                ScienceStationManagementNode = this.vessel.rootPart.FindModelTransform("ScienceProp");
            }
            // initialize the prop class
            if (sciProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate sciProp class ");
                sciProp = new LCARS_ScienceProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (ScienceStationManagementNode != null && sciProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp start ");


                sciProp.setVessel(this.vessel); // your vessel
                sciProp.setGameObjectLocation(ScienceStationManagementNode); // the gameobject transform
                sciProp.setShipSystemID("Science"); // the name you want for this in Main ODN Junction - string
                sciProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                sciProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp done ");
            }


            // Find your gameobject that marks the prop in your model
            if (CommunicationStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find ReplicatorProp gameobject in model ");
                CommunicationStationManagementNode = this.vessel.rootPart.FindModelTransform("CommunicationProp");
            }
            // initialize the prop class
            if (comProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate comProp class ");
                comProp = new LCARS_CommunicationProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (CommunicationStationManagementNode != null && comProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp start ");


                comProp.setVessel(this.vessel); // your vessel
                comProp.setGameObjectLocation(CommunicationStationManagementNode); // the gameobject transform
                comProp.setShipSystemID("Communication"); // the name you want for this in Main ODN Junction - string
                comProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                comProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp done ");
            }



            // Find your gameobject that marks the prop in your model
            if (TacticalStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find TacticalProp gameobject in model ");
                TacticalStationManagementNode = this.vessel.rootPart.FindModelTransform("TacticalProp");
            }
            // initialize the prop class
            if (tacProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate tacProp class ");
                tacProp = new LCARS_TacticalProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (TacticalStationManagementNode != null && tacProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp start ");


                tacProp.setVessel(this.vessel); // your vessel
                tacProp.setGameObjectLocation(TacticalStationManagementNode); // the gameobject transform
                tacProp.setShipSystemID("Tactical"); // the name you want for this in Main ODN Junction - string
                tacProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                tacProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp done ");
            }



            // Find your gameobject that marks the prop in your model
            if (HelmStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find HelmProp gameobject in model ");
                HelmStationManagementNode = this.vessel.rootPart.FindModelTransform("HelmProp");
            }
            // initialize the prop class
            if (helmProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate helmProp class ");
                helmProp = new LCARS_HelmProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (HelmStationManagementNode != null && helmProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  helmProp start ");


                helmProp.setVessel(this.vessel); // your vessel
                helmProp.setGameObjectLocation(HelmStationManagementNode); // the gameobject transform
                helmProp.setShipSystemID("Helm"); // the name you want for this in Main ODN Junction - string
                helmProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                helmProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp done ");
            }



            // Find your gameobject that marks the prop in your model
            if (BridgeStationManagementNode == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to find HelmProp gameobject in model ");
                BridgeStationManagementNode = this.vessel.rootPart.FindModelTransform("BridgeProp");
            }
            // initialize the prop class
            if (bridgeProp == null)
            {
                UnityEngine.Debug.Log("Aurora OnGUI:  trying to instantiate bridgeProp class ");
                bridgeProp = new LCARS_BridgeProp();
            }
            // run on every frame - this will take care of all things in a civilized manner
            if (BridgeStationManagementNode != null && bridgeProp != null)
            {
                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp start ");


                bridgeProp.setVessel(this.vessel); // your vessel
                bridgeProp.setGameObjectLocation(BridgeStationManagementNode); // the gameobject transform
                bridgeProp.setShipSystemID("Bridge"); // the name you want for this in Main ODN Junction - string
                bridgeProp.setActivationDistance(1.0f); // how close must the kerbal be to see the GUI - float, in meters
                bridgeProp.GUI(); // will display the GUI if a kerbal is closer than the set ActivationDistance


                //UnityEngine.Debug.Log("Aurora OnGUI:  sciProp done ");
            }


        }




    }


}
