﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCARSMarkII
{
    public class VesselDistanceComparer : IComparer<Vessel>
    {
        public Vessel OriginVessel;
        public int Compare(Vessel a, Vessel b)
        {
            float aDist = Vector3.Distance(a.transform.position, OriginVessel.transform.position);
            float bDist = Vector3.Distance(b.transform.position, OriginVessel.transform.position);
            return aDist.CompareTo(bDist);
        }
    }
    public enum Orient
    {
        Off,
        RelativeVelocity,
        RelativeVelocityAway,
        Target,
        TargetAway,
        Normal,
        AntiNormal,
        MatchTarget,
        MatchTargetAway,
        Prograde,
        Retrograde
    }
    public class LCARS_GravityTools
    {
        /// <summary>
        /// Takes a slider value and a Vessel as argument and add a calculated amount of force to the vessel
        /// </summary>
        public void AddNewForce_ffwd_back(float vSliderValue2, Vessel thisVessel)
        {
            if (vSliderValue2 != 0 && thisVessel.IsControllable)
            {
                //UnityEngine.Debug.Log("LCARS_Plugin: AddNewForce_ffwd_back thisVessel.id=" + thisVessel.id + ":  vSliderValue2=" + vSliderValue2);
                foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                {
                    //vesselPart.force_activate();
                    vesselPart.rigidbody.AddForce((Vector3d)thisVessel.transform.up * vSliderValue2, ForceMode.Acceleration);
                }
            }
        }
        /// <summary>
        /// Takes a slider value and a Vessel as argument and add a calculated amount of force to the vessel
        /// </summary>
        public void AddNewForce_left_right(float hSliderValue2, Vessel thisVessel)
        {
            if (hSliderValue2 != 0 && thisVessel.IsControllable)
            {
                //UnityEngine.Debug.Log("LCARS_Plugin: AddNewForce_left_right thisVessel.id=" + thisVessel.id + ":  hSliderValue2=" + hSliderValue2);
                foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                {
                    vesselPart.rigidbody.AddForce((Vector3d)thisVessel.transform.right * hSliderValue2, ForceMode.Acceleration);
                }
            }
        }
        /// <summary>
        /// Takes a slider value and a Vessel as argument and add a calculated amount of force to the vessel
        /// </summary>
        public void AddNewForce_up_down(float zSliderValue2, Vessel thisVessel)
        {
            if (zSliderValue2 != 0 && thisVessel.IsControllable)
            {
                //UnityEngine.Debug.Log("LCARS_Plugin: AddNewForce_up_down thisVessel.id=" + thisVessel.id + ":  (zSliderValue2 * -1)=" + (zSliderValue2 * -1));
                foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                {
                    if (vesselPart.frozen)
                    {
                        //UnityEngine.Debug.Log("LCARS_GravityTools: AddNewForce_up_down vesselPart.frozen thisVessel.id=" + thisVessel.id + ":  (zSliderValue2 * -1)=" + (zSliderValue2 * -1));
                    }
                    vesselPart.rigidbody.AddForce((Vector3d)thisVessel.transform.forward * (zSliderValue2 * -1), ForceMode.Acceleration);
                }
            }
        }

        /// <summary>
        /// Takes a Vessel and a GeeVector as argument and tries to cancel out the gee with counter forces
        /// </summary>
        public void CancelG2(Vessel thisVessel, Vector3d thisGeeVector)
        {
            if (thisVessel.IsControllable)
            {
                var antiGeeVector = thisGeeVector * -1;
                //var antiGeeVector = thisGeeVector * -1 / man.Count();
                //UnityEngine.Debug.Log("ImpulseDrive: CancelG2  thisVessel.id=" + thisVessel.id + ": gravityEnabledShips.Count=" + man.Count());
                foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                {
                    //UnityEngine.Debug.Log("LCARS_GravityTools: CancelG2 thisVessel.vesselName=" + thisVessel.vesselName + " vesselPart=" + vesselPart + " antiGeeVector=" + antiGeeVector.ToString());
                    vesselPart.rigidbody.AddForce(antiGeeVector, ForceMode.Acceleration);
                }
            }
        }

        /// <summary>
        /// Takes a Vessel and ends MOI rotation
        /// </summary>
        public void CancelMoI(Vessel thisVessel)
        {
            if (thisVessel.IsControllable)
            {
                float performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["InertiaDamper"].name, 200f);
                if (performance>0)
                {
                    thisVessel.MOI = Vector3.Slerp(thisVessel.MOI, Vector3.zero, 0.7f * performance);
                    thisVessel.rootPart.rigidbody.angularVelocity = Vector3.Slerp(thisVessel.rootPart.rigidbody.angularVelocity, Vector3.zero, 0.7f * performance);
                }
            }
        }

        public Orient ASAS_order = Orient.Off;
        public Vector3 relativeVelocity;
        public float relativeInclination;
        public Vector3 vectorToTarget;
        public float targetDistance;

        public bool killRelativeVelocity = false;
        public Vector3 localRelativeVelocity = Vector3.zero;
        public int selectedVesselIndex;
        public int selectedVesselInstanceId;
        public List<Vessel> vesselList;

        public Vector3 _tgtFwd;
        public Vector3 _tgtUp;
        public bool homeOnRelativePosition = false;
        public Vector3 localRelativePosition = Vector3.zero;
        public void ASAS(Vessel thisVessel, Orient ASASorder)
        {
            ASAS_order = ASASorder;
            Vector3 up = (thisVessel.findWorldCenterOfMass() - thisVessel.mainBody.position).normalized;
            //Vector3 prograde = thisVessel.orbit.GetRelativeVel().normalized;

            Vector3d position = thisVessel.findWorldCenterOfMass();
            Vector3d nonRotatingFrameVelocity = thisVessel.orbit.GetVel();
            Vector3d rotatingFrameVelocity = nonRotatingFrameVelocity - thisVessel.mainBody.getRFrmVel(position);
            Vector3 prograde = rotatingFrameVelocity.normalized;

            Vessel selectedVessel = null;
            bool noTargetVessel = false;
            try
            {
                ITargetable foo = thisVessel.targetObject;
                selectedVessel = foo.GetVessel(); 
                relativeVelocity = selectedVessel.orbit.GetVel() - thisVessel.orbit.GetVel();
                vectorToTarget = selectedVessel.transform.position - thisVessel.transform.position;
                targetDistance = Vector3.Distance(selectedVessel.transform.position, thisVessel.transform.position);

                relativeInclination = (float)selectedVessel.orbit.inclination - (float)thisVessel.orbit.inclination;
            }
            catch { noTargetVessel = true; }


            switch (ASAS_order)
            {
                case Orient.RelativeVelocity:
                    if (noTargetVessel) { return; }
                    _tgtFwd = relativeVelocity;
                    _tgtUp = up; //Vector3.Cross(_tgtFwd.normalized, thisVessel.orbit.vel.normalized);
                    break;
                case Orient.RelativeVelocityAway:
                    if (noTargetVessel) { return; }
                    _tgtFwd = -relativeVelocity;
                    _tgtUp = up; //Vector3.Cross(_tgtFwd.normalized, thisVessel.orbit.vel.normalized);
                    break;
                case Orient.Target:
                    if (noTargetVessel) { return; }
                    _tgtFwd = vectorToTarget;
                    _tgtUp = up; //Vector3.Cross(_tgtFwd.normalized, thisVessel.orbit.vel.normalized);
                    break;
                case Orient.TargetAway:
                    if (noTargetVessel) { return; }
                    _tgtFwd = -vectorToTarget;
                    _tgtUp = up; //Vector3.Cross(_tgtFwd.normalized, thisVessel.orbit.vel.normalized);
                    break;
                case Orient.MatchTarget:
                    if (noTargetVessel) { return; }
                    _tgtFwd = selectedVessel.transform.forward;
                    _tgtUp = selectedVessel.transform.up;
                    break;
                case Orient.MatchTargetAway:
                    if (noTargetVessel) { return; }
                    _tgtFwd = -selectedVessel.transform.forward;
                    _tgtUp = selectedVessel.transform.up;
                    break;
                case Orient.Normal:
                    _tgtFwd = Vector3.Cross(prograde, up);
                    _tgtUp = up;
                    break;
                case Orient.AntiNormal:
                    _tgtFwd = -Vector3.Cross(prograde, up);
                    _tgtUp = up;
                    break;
                case Orient.Prograde:
                    _tgtFwd = prograde;
                    _tgtUp = up; //new Vector3(0, 1, 0);
                    break;
                case Orient.Retrograde:
                    _tgtFwd = -prograde;
                    _tgtUp = -up; //new Vector3(0, 1, 0);
                    break;
            }

            Quaternion newRotation = new Quaternion();
            //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  3 ");
            newRotation.SetLookRotation(_tgtFwd, _tgtUp);
            //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  4 ");
            thisVessel.SetRotation(Quaternion.Slerp(thisVessel.transform.rotation,newRotation,0.2f));

        }


        //Returns the forward vector of a quaternion
	    private static Vector3 GetForwardVector(Quaternion q){
 
		    return q * Vector3.forward;
	    }
 
	    //Returns the up vector of a quaternion
        private static Vector3 GetUpVector(Quaternion q)
        {
 
		    return q * Vector3.up;
	    }
 
	    //Returns the right vector of a quaternion
        private static Vector3 GetRightVector(Quaternion q)
        {
 
		    return q * Vector3.right;
	    }
        public void HoldSpeed(Vessel thisVessel, double target_speed)
        {
                Vector3d position = thisVessel.findWorldCenterOfMass();
                Vector3d nonRotatingFrameVelocity = thisVessel.orbit.GetVel();
                Vector3d rotatingFrameVelocity = nonRotatingFrameVelocity - thisVessel.mainBody.getRFrmVel(position);
                double target_difference = (thisVessel.srfSpeed - target_speed) * -1;
                double damped_target_difference = target_difference * 0.009;
                if (target_difference!=0)
                {
                    float performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["HoldSpeed"].name, (float)target_difference);
                    /*
                    UnityEngine.Debug.Log("ImpulseDrive: HoldSpeed " +
                        "thisVessel.horizontalSrfSpeed=" + thisVessel.srfSpeed +
                        " target_speed=" + target_speed +
                        " target_difference=" + target_difference +
                        " damped_target_difference=" + damped_target_difference
                        );
                    */
                    foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                    {
                        vesselPart.rigidbody.AddForce(rotatingFrameVelocity * damped_target_difference * performance, ForceMode.Acceleration);
                    }
                }
        }
        public void HoldHeight(Vessel thisVessel,  double target_height)
        {
                Vector3 CoM = thisVessel.findWorldCenterOfMass();
                double altitudeASL = thisVessel.mainBody.GetAltitude(CoM);
                double target_difference = (altitudeASL - target_height) * -1;
                double damped_target_difference = target_difference * 0.0000009;
                if (target_difference != 0)
                {
                    float performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["HoldHeight"].name, (float)(target_difference * -1) );
                    Vector3 bodyPos = thisVessel.mainBody.position;
                    Vector3 shipPos = CoM;
                    Vector3 newVector = shipPos - bodyPos;
                    /*
                    UnityEngine.Debug.Log("ImpulseDrive: HoldHeight " +
                        "altitudeASL=" + altitudeASL +
                        " target_height=" + target_height +
                        " target_difference=" + target_difference +
                        " damped_target_difference=" + damped_target_difference
                        );
                    */
                    foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                    {
                        //UnityEngine.Debug.Log("ImpulseDrive: CancelG2 thisVessel.vesselName=" + thisVessel.vesselName + " vesselPart=" + vesselPart + " antiGeeVector=" + antiGeeVector.ToString());
                        vesselPart.rigidbody.AddForce((Vector3d)newVector * damped_target_difference * performance, ForceMode.Acceleration);
                    }

                }
        }
        /// <summary>
        /// Takes a Vessel as argument and tries keep the prograde vector ligned up with the ships transform position.
        /// Problem: does currently slow down the vessel and prevents high velocitys..
        /// </summary>
        debug_line debugLine0 = null;
        LineRenderer dl0;
        debug_line debugLine1 = null;
        LineRenderer dl1;
        debug_line debugLine2 = null;
        LineRenderer dl2;
        debug_line debugLine3 = null;
        LineRenderer dl3;
        public void PilotMode(Vessel thisVessel/*, float SliderValue2 */)
        {
                //position of the ship's center of mass:
                Vector3d position = thisVessel.findWorldCenterOfMass();
                //ship velocity in an external, non-rotating frame:
                Vector3d nonRotatingFrameVelocity = FlightGlobals.ActiveVessel.orbit.GetVel();
                //velocity as seen in planet's rotating frame
                Vector3d rotatingFrameVelocity = nonRotatingFrameVelocity - thisVessel.mainBody.getRFrmVel(position);
                //direction in which the ship currently points:
                Vector3d heading = (Vector3d)thisVessel.transform.up;
                Vector3 foo = Vector3.Reflect(Vector3.Reflect(rotatingFrameVelocity, thisVessel.transform.right), thisVessel.transform.forward);
                float angle_up = PosNegAngle(foo, thisVessel.transform.up, thisVessel.transform.right) * 10F;
                //float angle_forward = Vector3.Angle(foo, thisVessel.transform.forward);
                float angle_right = PosNegAngle(foo, thisVessel.transform.up, thisVessel.transform.forward) * 10f;

                float performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["ProgradeStabilizer"].name, (float)angle_up);
                AddNewForce_up_down(angle_up * performance, thisVessel);

                performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["ProgradeStabilizer"].name, (float)angle_right);
                AddNewForce_left_right(angle_right * performance, thisVessel);
            
            //UnityEngine.Debug.Log("ImpulseDrive: PilotMode  angle_up=" + angle_up + "  angle_right=" + angle_right);
                /*
                 //Show some debug Lines..
                if (debugLine0 == null)
                {
                    debugLine0 = new debug_line();
                }
                dl0 = debugLine0.render(thisVessel.transform.up, Color.red);
                dl0.SetPosition(0,position);
                dl0.SetPosition(1,thisVessel.transform.TransformPoint(Vector3.up * 5000));

                if (debugLine3 == null)
                {
                    debugLine3 = new debug_line();
                }
                dl3 = debugLine3.render(rotatingFrameVelocity, Color.blue);
                dl3.SetPosition(0, position);
                dl3.SetPosition(1, rotatingFrameVelocity * 1000);
                if (debugLine2 == null)
                {
                    debugLine2 = new debug_line();
                }
                dl2 = debugLine2.render(foo, Color.yellow);
                dl2.SetPosition(0, position);
                dl2.SetPosition(1, foo * 1000);

                debugLine0.Destroy();
                debugLine2.Destroy();
                debugLine3.Destroy();
                */
        }
        public float PosNegAngle(Vector3 a1, Vector3 a2, Vector3 normal)
        {
            float angle = Vector3.Angle(a1, a2);
            float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(a1, a2)));
            return angle * sign;
        }
        public class debug_line
        {
            public GameObject lineObj = null;
            public LineRenderer lr;
            public debug_line()
            {
            }
            public LineRenderer render(Vector3 transform_up, Color c, int Width = 2)
            {
                if (this.lineObj == null)
                {
                    this.lineObj = new GameObject();
                }
                if (this.lr == null)
                {
                    this.lineObj.transform.up = transform_up;
                    this.lr = lineObj.AddComponent<LineRenderer>();
                    this.lr.material = new Material(Shader.Find("Particles/Additive"));

                    this.lr.SetWidth(Width, Width);
                    this.lr.SetColors(c, c);

                    this.lr.SetVertexCount(2);
                    this.lr.useWorldSpace = true;
                }
                return lr;
            }
            public void SetPosition(int index, Vector3 pos)
            {
                this.lr.SetPosition(index, pos);
            }
            public void Destroy()
            {
                this.lr = null;
                this.lineObj = null;
            }
        }
        //create a vector of direction "vector" with length "size"
        public static Vector3 SetVectorLength(Vector3 vector, float size)
        {

            //normalize the vector
            Vector3 vectorNormalized = Vector3.Normalize(vector);

            //scale the vector
            return vectorNormalized *= size;
        }

        /// <summary>
        /// Takes a Vessel as argument and tries to cancel out all valocity with counter forces
        /// </summary>
        public bool FullHalt(Vessel thisVessel)
        {
            float performance = 1;
            try 
            {
                Vessel targetVessel = null;
                targetVessel = thisVessel.targetObject.GetVessel();
                if (thisVessel.targetObject != null)
                {
                    if (targetVessel.Landed == false && targetVessel.loaded == true)
                    {
                        //UnityEngine.Debug.Log("ImpulseDrive: FullHalt  thisVessel.targetObject.GetVessel().id=" + thisVessel.targetObject.GetVessel().id);
                        targetVessel = thisVessel.targetObject.GetVessel();
                        relativeVelocity = targetVessel.orbit.GetVel() - thisVessel.orbit.GetVel();
                        performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["FullHalt"].name, (float)relativeVelocity.magnitude);

                        foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                        {
                            vesselPart.rigidbody.AddForce(relativeVelocity * performance, ForceMode.Acceleration);
                        }
                    }
                    else
                    {
                        thisVessel.LCARS().lODN.ShipStatus.LCARS_FullHalt = false;
                    }
                }
                
                //UnityEngine.Debug.Log("ImpulseDrive: FullHalt  thisVessel.targetObject done");
                return true;
            }
            catch{}
            


                //UnityEngine.Debug.Log("ImpulseDrive: FullHalt  thisVessel.id=" + thisVessel.id);
                //position of the ship's center of mass:
                Vector3d position = thisVessel.findWorldCenterOfMass();

                //unit vectors in the up (normal to planet surface), east, and north (parallel to planet surface) directions
                Vector3d eastUnit = thisVessel.mainBody.getRFrmVel(position).normalized; //uses the rotation of the body's frame to determine "east"
                Vector3d upUnit = (position - thisVessel.mainBody.position).normalized;
                Vector3d northUnit = Vector3d.Cross(upUnit, eastUnit); //north = up cross east

                double altitude = thisVessel.mainBody.GetAltitude(position);

                //ship velocity in an external, non-rotating frame:
                Vector3d nonRotatingFrameVelocity = thisVessel.orbit.GetVel();

                //velocity as seen in planet's rotating frame
                Vector3d rotatingFrameVelocity = nonRotatingFrameVelocity - thisVessel.mainBody.getRFrmVel(position);
                // LCARS_Plugin: FullHalt  rotatingFrameVelocity=[-12.7104615141479, -0.0543791800737381, -15.7670205775624]
                //UnityEngine.Debug.Log("LCARS_Plugin: FullHalt  rotatingFrameVelocity=" + rotatingFrameVelocity);

                performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["FullHalt"].name, (float)rotatingFrameVelocity.magnitude);

                foreach (var vesselPart in thisVessel.parts.Where(p => p.rigidbody != null))
                {
                    vesselPart.rigidbody.AddForce((Vector3d)rotatingFrameVelocity * performance * -1, ForceMode.Acceleration);
                }

            return true;
        }

        public bool checkSlowToSave(Vessel thisVessel, float makeStationarySpeedMax)
        {
            Vector3d foo = thisVessel.GetSrfVelocity();
            if (foo.magnitude < (double)makeStationarySpeedMax)
            {
                //UnityEngine.Debug.Log("ImpulseDrive: checkSlowToSave  yes: " + foo.magnitude);
                return true;
            }
            else
            {
                //UnityEngine.Debug.Log("ImpulseDrive: checkSlowToSave  no: " + foo.magnitude);
                return false;
            }
        }
        public void makeSlowToSave(Vessel thisVessel, bool bool_makeSlowToSave, float makeStationarySpeedClamp)
        {
            if (thisVessel.srf_velocity.magnitude > makeStationarySpeedClamp)
            {
                thisVessel.SetWorldVelocity(thisVessel.srf_velocity.normalized * 0.0F);
            }
            thisVessel.Landed = true;
        }
        public void SatLandedFalse(Vessel thisVessel)
        {
            thisVessel.Landed = false;
        }
        public bool IsSlowToSave(Vessel thisVessel, float makeStationarySpeedClamp)
        {
            if (Mathf.Abs((int)thisVessel.srf_velocity.magnitude) == Mathf.Abs((int)makeStationarySpeedClamp))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }


        public void FormationFlight(Vessel thisVessel, bool bool_FormationMode)
        {
            if (bool_FormationMode)
            {

                float performance = thisVessel.LCARS().lPowSys.draw(thisVessel.LCARS().lODN.ShipSystems["FormationFlight"].name, 200f);
                //thisVessel.GetTransform();
                ////////////////////////////
                // BEGINN THIS IS WORKING but cheaty!!!!!
                ////////////////////////////
                //direction in which the ship currently points:
                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  begin ");
                Vessel leader_v = FlightGlobals.ActiveVessel;
                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  1 ");
                Vector3 LeaderHeading = (Vector3)leader_v.transform.forward;

                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  2 leader_v.id=" + leader_v.id + "  thisVessel.id=" + thisVessel.id + " bool_FormationMode=" + bool_FormationMode);

                Quaternion newRotation = new Quaternion();
                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  3 ");
                newRotation.SetLookRotation(LeaderHeading, leader_v.transform.up);
                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  4 ");
                thisVessel.SetRotation(newRotation);
                //UnityEngine.Debug.Log("ImpulseDrive: FormationFlight  done ");
                ////////////////////////////
                // END THIS IS WORKING but cheaty!!!!!
                ////////////////////////////
                
                
                
                
                /*
                int scalingFactor = 1;
                //Transform rotateTo;
                //Transform rotateFrom;
                Vessel leader_v = man.GetFormationLeaderVessel();
                Vector3 LeaderHeading = (Vector3)leader_v.transform.up;
                Quaternion LeaderHeadingRotation = new Quaternion();
                LeaderHeadingRotation = Quaternion.LookRotation(LeaderHeading);

                Vector3 heading = (Vector3)thisVessel.transform.right;
                Quaternion HeadingRotation = new Quaternion();
                HeadingRotation = Quaternion.LookRotation(heading);

                    //thisVessel.transform.rotation = Quaternion.Slerp(HeadingRotation, LeaderHeadingRotation, Time.deltaTime / scalingFactor);
                    thisVessel.SetRotation(Quaternion.Slerp(HeadingRotation, LeaderHeadingRotation, Time.deltaTime / scalingFactor));

                //Quaternion newRotation = new Quaternion();
                //float step = 1.0F * Time.deltaTime;
                    //newRotation = Quaternion.RotateTowards(HeadingRotation, LeaderHeadingRotation, step);
                    //thisVessel.SetRotation(newRotation);
                    
                if(thisVessel.Landed == false) {
                }
                */
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                

            }
        }

    
    }
}
