using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_Astrometrics : ILCARSPlugin
    {
        public string subsystemName { get { return "Astrometrics"; } }
        public string subsystemDescription {get{return "Orbital Information";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
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
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        public static double DegreeFix(double inAngle, double rangeStart)
        {
            double rangeEnd = rangeStart + 360.0;
            double outAngle = inAngle;
            while (outAngle > rangeEnd)
                outAngle -= 360.0;
            while (outAngle < rangeStart)
                outAngle += 360.0;
            return outAngle;
        }
        public static float AngleDelta(float a, float b)
        {
            var delta = b - a;
            return (float)DegreeFix(delta, -180.0);
        }
        public static float GetHeading(Vessel vessel)
        {
            var up = vessel.upAxis;
            var north = GetNorthVector(vessel);
            var headingQ =
            Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(vessel.GetTransform().rotation) *
            Quaternion.LookRotation(north, up));
            return headingQ.eulerAngles.y;
        }
        public static float GetVelocityHeading(Vessel vessel)
        {
            var up = vessel.upAxis;
            var north = GetNorthVector(vessel);
            var headingQ =
            Quaternion.Inverse(Quaternion.Inverse(Quaternion.LookRotation(vessel.srf_velocity, up)) *
            Quaternion.LookRotation(north, up));
            return headingQ.eulerAngles.y;
        }
        public static float GetTargetBearing(Vessel vessel, Vessel target)
        {
            return AngleDelta(GetHeading(vessel), GetTargetHeading(vessel, target));
        }
        public static float GetTargetHeading(Vessel vessel, Vessel target)
        {
            var up = vessel.upAxis;
            var north = GetNorthVector(vessel);
            var vector =
            Vector3d.Exclude(vessel.upAxis, target.GetWorldPos3D() - vessel.GetWorldPos3D()).normalized;
            var headingQ =
            Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(Quaternion.LookRotation(vector, up)) *
            Quaternion.LookRotation(north, up));
            return headingQ.eulerAngles.y;
        }
        public static Vector3d GetNorthVector(Vessel vessel)
        {
            return Vector3d.Exclude(vessel.upAxis, vessel.mainBody.transform.up);
        }
        /// <summary>
        /// The compass heading from the current position of the CPU vessel to the
        /// LAT/LANG position on the SOI body's surface.
        /// </summary>
        /// <returns>compass heading in degrees</returns>
        private double GetHeadingFrom()
        {
            var up = thisVessel.upAxis;
            var north = GetNorthVector(thisVessel);
            var targetWorldCoords = thisVessel.mainBody.GetWorldSurfacePosition(thisVessel.mainBody.GetLatitude(thisVessel.CoM), thisVessel.longitude, thisVessel.terrainAltitude);
            var vector = Vector3d.Exclude(up, targetWorldCoords - thisVessel.GetWorldPos3D()).normalized;
            var headingQ =
            Quaternion.Inverse(Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(Quaternion.LookRotation(vector, up)) *
            Quaternion.LookRotation(north, up));
            return headingQ.eulerAngles.y;
        }


        Transform pitchLevel = new GameObject().transform;
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
                GUILayout.BeginVertical();
                GUILayout.Label("ToDo: " + thisVessel.vesselName);

                GUILayout.Label("Name: " + thisVessel.vesselName);
                GUILayout.Label("Altitude: " + thisVessel.altitude);
                GUILayout.Label("Surface Speed: " + thisVessel.srf_velocity.magnitude);
                GUILayout.Label("Heading: " + AngleDelta(GetHeading(thisVessel), (float) GetHeadingFrom()));

                pitchLevel.rotation = Quaternion.LookRotation(thisVessel.transform.right, Vector3.forward);
                float pitch = pitchLevel.localRotation.eulerAngles.z;
                if (pitch > 180f) pitch -= 360f;
                GUILayout.Label("Pitch: " + pitch);
            
            /*
                GUILayout.Label("Roll: " + );
                GUILayout.Label("  Avg. PartTemperature" + Math.Round(thisVessel.LCARSVessel_AveragePartTemperature(), 2) + "° of " + Math.Round(thisVessel.LCARSVessel_AveragePartTemperatureMax(), 2) + "° max =" + Math.Round(thisVessel.LCARSVessel_Heat_percentage(), 2) + "% ");
                GUILayout.Label("Mass: " + Math.Round(thisVessel.LCARSVessel_WetMass(), 2) + " t");
                GUILayout.Label("   Dry: " + Math.Round(thisVessel.LCARSVessel_DryMass(), 2) + " t");
                GUILayout.Label("   Resources: " + Math.Round(thisVessel.LCARSVessel_ResourceMass(), 2) + " t");
                GUILayout.Label("Parts: " + thisVessel.parts.Count);
                GUILayout.Label("Crew compliment: " + thisVessel.GetCrewCount() + "/" + thisVessel.GetCrewCapacity());
                GUILayout.EndVertical();

            

                thisVessel.orbit.timeToAp;
            thisVessel.orbit.timeToPe;
                thisVessel.orbit.timeToTransition1;
                    thisVessel.orbit.timeToTransition2;
                        thisVessel.orbit.PeA;
                            thisVessel.orbit.ApA;
                    thisVessel.orbit.closestTgtApprUT;
                    thisVessel.orbit.closestEncounterBody;
                    thisVessel.orbit.altitude;

            */

                GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
