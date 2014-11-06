using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{
    public class MagBoots : PartModule
    {
        public bool BootsActive = false;
        public GameObject magBoots;
        public Part MagbootsPart = null;
        public Vessel MagbootsVessel = null;
        GameObject MagBoots_GameObject;
        Transform MagBoots_KerbalPoint = null;
        Transform capsuleCollider;
        RaycastHit rayHit;
        //GimbalDebug foo1 = new GimbalDebug();
        //GimbalDebug foo2 = new GimbalDebug();
        KerbalEVA JebOut;
        Animation JebOutAnimation;
        List<string> excludeList = new List<string>();
        void Start()
        {

        }

        [KSPEvent(guiActive = true, guiName = "Activate MagBoots")]
        public void startMagBoots()
        {
            UnityEngine.Debug.Log("MagBoots:PartModule startMagBoots : Activate MagBoots ");
            Events["startMagBoots"].active = false;
            Events["endMagBoots"].active = true;
            BootsActive = true;
            //MagBoots_KerbalPoint = FlightGlobals.ActiveVessel.rootPart.FindModelTransform("footCollider_l");
            //MagBoots_R_KerbalPoint = FlightGlobals.ActiveVessel.rootPart.FindModelTransform("footCollider_r");
        }
        [KSPEvent(guiActive = true, guiName = "Deactivate MagBoots", active = false)]
        public void endMagBoots()
        {
            UnityEngine.Debug.Log("MagBoots:PartModule endMagBoots : Deactivate MagBoots ");
            Events["startMagBoots"].active = true;
            Events["endMagBoots"].active = false;
            BootsActive = false;
            try
            {
                //foo2.removeGimbal();
            }
            catch (Exception ex) { }
            try
            {
                //foo1.removeGimbal();
            }
            catch (Exception ex) { }
        }

        void Update()
        {
            //UnityEngine.Debug.Log("MagBootsController Update:  1 ");

            if (HighLogic.LoadedSceneIsEditor || this.vessel == null)
            {
                return;
            }

            //UnityEngine.Debug.Log("MagBootsController Update:  2 ");




            if (BootsActive)
            {
                //UnityEngine.Debug.Log("MagBootsController Update:  BootsActive ");
                //JebOut.isRagdoll = false;
                if (MagBoots_KerbalPoint == null)
                {

                    JebOut = this.part.GetComponent<KerbalEVA>();
                    if (JebOut == null)
                    {
                        return;
                    }
                    //UnityEngine.Debug.Log("MagBootsController Update:  KerbalEVA found ");

                    excludeList.Add("capsuleCollider");
                    excludeList.Add("jetpackCollider");
                    excludeList.Add("helmetCollider");
                    excludeList.Add("footCollider_l");
                    excludeList.Add("footCollider_r");
                    excludeList.Add("bn_l_hip01");
                    excludeList.Add("bn_r_hip01");
                    excludeList.Add("bn_l_knee_b01");
                    excludeList.Add("bn_r_knee_b01");
                    /*
                    [LOG 10:44:09.100] MagBootsController Update:  capsuleCollider found co.gameObject.name=capsuleCollider
[LOG 10:44:09.101] MagBootsController Update:  MagBoots_KerbalPoint.childCount=0
[LOG 10:44:09.101] MagBootsController Update: characterColliders  co.name=jetpackCollider
[LOG 10:44:09.102] MagBootsController Update: characterColliders  co.name=helmetCollider
[LOG 10:44:56.658] MagBootsController Update:  Raycast hit MagBoots_KerbalPoint: rayHit.collider.name=footCollider_l
[LOG 10:44:57.098] MagBootsController Update:  Raycast hit MagBoots_KerbalPoint: rayHit.collider.name=footCollider_r
[LOG 10:44:59.018] MagBootsController Update:  Raycast hit MagBoots_KerbalPoint: rayHit.collider.name=bn_l_hip01
[LOG 10:45:17.385] MagBootsController Update:  Raycast hit MagBoots_KerbalPoint: rayHit.collider.name=bn_r_knee_b01
*/


                    /*
                                        foreach (KerbalAnimationState animSt in JebOut.Animations.GetAllAnimations())
                                        {
                                            UnityEngine.Debug.Log("MagBootsController Update: JebOut animSt.State.clip.name=" + animSt.State.clip.name);
                    LOG 10:29:42.246] MagBootsController Update: JebOut animSt.State.clip.nameidle
                    [LOG 10:29:42.247] MagBootsController Update: JebOut animSt.State.clip.namesuspended
                    [LOG 10:29:42.248] MagBootsController Update: JebOut animSt.State.clip.namewkC_forward
                    [LOG 10:29:42.249] MagBootsController Update: JebOut animSt.State.clip.namewkC_run
                    [LOG 10:29:42.249] MagBootsController Update: JebOut animSt.State.clip.namewkC_sideLeft
                    [LOG 10:29:42.250] MagBootsController Update: JebOut animSt.State.clip.namewkC_sideRight
                    [LOG 10:29:42.250] MagBootsController Update: JebOut animSt.State.clip.namewkC_backwards
                    [LOG 10:29:42.250] MagBootsController Update: JebOut animSt.State.clip.nameleftTurn
                    [LOG 10:29:42.251] MagBootsController Update: JebOut animSt.State.clip.namerightTurn
                    [LOG 10:29:42.251] MagBootsController Update: JebOut animSt.State.clip.nameopenJetpack
                    [LOG 10:29:42.252] MagBootsController Update: JebOut animSt.State.clip.namecloseJetpack
                    [LOG 10:29:42.252] MagBootsController Update: JebOut animSt.State.clip.namestUp_front
                    [LOG 10:29:42.253] MagBootsController Update: JebOut animSt.State.clip.namestUp_back
                    [LOG 10:29:42.253] MagBootsController Update: JebOut animSt.State.clip.namejump_forward_start
                    [LOG 10:29:42.253] MagBootsController Update: JebOut animSt.State.clip.namejump_forward_end
                    [LOG 10:29:42.254] MagBootsController Update: JebOut animSt.State.clip.namejumpStill_start
                    [LOG 10:29:42.254] MagBootsController Update: JebOut animSt.State.clip.namejumpStill_end
                    [LOG 10:29:42.255] MagBootsController Update: JebOut animSt.State.clip.nameswim_idle
                    [LOG 10:29:42.255] MagBootsController Update: JebOut animSt.State.clip.nameswim_forward
                    [LOG 10:29:42.255] MagBootsController Update: JebOut animSt.State.clip.nameswim_stUp_front
                    [LOG 10:29:42.256] MagBootsController Update: JebOut animSt.State.clip.nameswim_stUp_back
                    [LOG 10:29:42.256] MagBootsController Update: JebOut animSt.State.clip.namewkC_loG_forward
                    [LOG 10:29:42.257] MagBootsController Update: JebOut animSt.State.clip.nameidle
                    [LOG 10:29:42.257] MagBootsController Update: JebOut animSt.State.clip.nameidle
                    [LOG 10:29:42.257] MagBootsController Update: JebOut animSt.State.clip.namewkC_sideLeft
                    [LOG 10:29:42.258] MagBootsController Update: JebOut animSt.State.clip.namewkC_sideRight
                    [LOG 10:29:42.258] MagBootsController Update: JebOut animSt.State.clip.nameladder_grab
                    [LOG 10:29:42.259] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_grabDown
                    [LOG 10:29:42.259] MagBootsController Update: JebOut animSt.State.clip.nameladder_idle
                    [LOG 10:29:42.260] MagBootsController Update: JebOut animSt.State.clip.nameladder_up
                    [LOG 10:29:42.260] MagBootsController Update: JebOut animSt.State.clip.nameladder_down
                    [LOG 10:29:42.261] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_release
                    [LOG 10:29:42.261] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_release
                    [LOG 10:29:42.262] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_up
                    [LOG 10:29:42.262] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_down
                    [LOG 10:29:42.262] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_left
                    [LOG 10:29:42.263] MagBootsController Update: JebOut animSt.State.clip.nameladder_lean_right
                    [LOG 10:29:42.263] MagBootsController Update: JebOut animSt.State.clip.nameflag_plant
                    [LOG 10:29:42.264] MagBootsController Update: JebOut animSt.State.clip.namedocking
                                            if(animSt.State.clip. == "wkC_loG_forward")
                                            {
                    JebOutAnimation = animSt.State.clip
                                            }
                                        }
                                        //JebOut.Animations.walkLowGee.start;
                                        wkC_loG_forward
                                                animation.PlayQueued("LandingLegsLower");
                    */


                    foreach (Collider co in JebOut.characterColliders)
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update: characterColliders  co.name=" + co.name);
                        if (co.name == "capsuleCollider")
                        {
                            //UnityEngine.Debug.Log("MagBootsController Update:  capsuleCollider found co.gameObject.name=" + co.gameObject.name);
                            capsuleCollider = co.gameObject.transform;
                            MagBoots_GameObject = new GameObject("MagBoots_GameObject");
                            MagBoots_GameObject.transform.position = capsuleCollider.position + new Vector3(0, -0.5f, 0);
                            MagBoots_GameObject.transform.parent = capsuleCollider;
                            MagBoots_KerbalPoint = MagBoots_GameObject.transform;
                            MagBoots_KerbalPoint.gameObject.AddComponent<Rigidbody>();
                            //capsuleCollider.parent = MagBoots_KerbalPoint;
                            //MagBoots_L_KerbalPoint = co.gameObject.transform;
                            //UnityEngine.Debug.Log("MagBootsController Update:  MagBoots_KerbalPoint.childCount=" + MagBoots_KerbalPoint.childCount);
                        }
                    }
                }

                //UnityEngine.Debug.Log("MagBootsController Update:  drawGimbal begin ");
                try
                {
                    //foo2.removeGimbal();
                }
                catch (Exception ex) { }
                try
                {
                    //foo1.removeGimbal();
                }
                catch (Exception ex) { }
                //foo1.drawGimbal(capsuleCollider, 1, 0.2f);
                //foo2.drawGimbal(FlightGlobals.ActiveVessel, 1, 0.2f);
                //UnityEngine.Debug.Log("MagBootsController Update:  drawGimbal end ");

                bool WeHaveColliderContact = false;

                if (Physics.Raycast(capsuleCollider.position, capsuleCollider.up * -1, out rayHit, 0.7f)) // grab floor and hold there
                {
                    if (excludeList.Contains(rayHit.collider.name))
                    {
                        return;
                    }

                    //UnityEngine.Debug.Log("MagBootsController Update:  Raycast hit MagBoots_KerbalPoint: rayHit.collider.name=" + rayHit.collider.name);
                    JebOut.vessel.rootPart.rigidbody.AddForceAtPosition((rayHit.point - capsuleCollider.position) * 2, capsuleCollider.position, ForceMode.Force);
                    //UnityEngine.Debug.Log("MagBootsController Update:  AddForceAtPosition: done ");
                    WeHaveColliderContact = true;
                    //JebOut.vessel.transform.rotation = Quaternion.LookRotation(rayHit.normal) * new Quaternion(0, 90, 0, 0);
                    JebOut.vessel.transform.rotation = Quaternion.LookRotation(Vector3.Exclude(rayHit.normal, transform.forward), rayHit.normal);
                    JebOut.vessel.MOI = Vector3.zero;
                    JebOut.vessel.rigidbody.angularVelocity = Vector3.zero;

                }
                if (FlightGlobals.ActiveVessel.vesselType != VesselType.EVA && FlightGlobals.ActiveVessel != this.vessel) // if not EVA is active vessel, only hold him on the ground but nothing else
                {
                    if (WeHaveColliderContact) // grab floor and hold there with increased strength
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  grab floor and hold there with increased strength: rayHit.collider.name=" + rayHit.collider.name);
                        JebOut.vessel.rootPart.rigidbody.AddForceAtPosition((rayHit.point - capsuleCollider.position) * 200, capsuleCollider.position, ForceMode.Force);
                        //UnityEngine.Debug.Log("MagBootsController Update:  AddForceAtPosition: done ");
                    }
                    return;
                }

                if (WeHaveColliderContact)
                {
                    if (Input.GetKey("w"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown w");
                        MagBoots_KerbalPoint.gameObject.rigidbody.AddForce((Vector3d)this.vessel.transform.forward * 20, ForceMode.Acceleration);
                        JebOut.vessel.rootPart.rigidbody.AddForce((Vector3d)this.vessel.transform.forward * 20, ForceMode.Acceleration);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                        /*
                        foreach (Animation animation in this.vessel.rootPart.FindModelAnimators("wkC_loG_forward"))
                        {
                            UnityEngine.Debug.Log("MagBootsController Update: JebOut runAnimations:  wkC_loG_forward begin ");
                            //animation.Rewind();
                            //animation.PlayQueued("wkC_loG_forward");
                            //JebOut.Animations.walkLowGee.State.clip.;
                            UnityEngine.Debug.Log("MagBootsController Update: JebOut runAnimations:  wkC_loG_forward end ");
                        }
                        */
                    }
                    if (Input.GetKey("s"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown s");
                        MagBoots_KerbalPoint.gameObject.rigidbody.AddForce((Vector3d)this.vessel.transform.forward * -20, ForceMode.Acceleration);
                        JebOut.vessel.rootPart.rigidbody.AddForce((Vector3d)this.vessel.transform.forward * -20, ForceMode.Acceleration);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                    }
                    if (Input.GetKey("a"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown a");
                        MagBoots_KerbalPoint.gameObject.rigidbody.AddForce((Vector3d)this.vessel.transform.right * -10, ForceMode.Acceleration);
                        JebOut.vessel.rootPart.rigidbody.AddForce((Vector3d)this.vessel.transform.right * -10, ForceMode.Acceleration);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                    }
                    if (Input.GetKey("d"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown d");
                        MagBoots_KerbalPoint.gameObject.rigidbody.AddForce((Vector3d)this.vessel.transform.right * 10, ForceMode.Acceleration);
                        JebOut.vessel.rootPart.rigidbody.AddForce((Vector3d)this.vessel.transform.right * 10, ForceMode.Acceleration);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                    }
                    if (Input.GetKey("q"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown q");
                        MagBoots_KerbalPoint.gameObject.transform.Rotate(this.vessel.transform.forward, -4.4f, Space.Self);
                        JebOut.vessel.transform.Rotate(this.vessel.transform.forward, -4.4f, Space.Self);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                    }
                    if (Input.GetKey("e"))
                    {
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown e");
                        MagBoots_KerbalPoint.gameObject.transform.Rotate(rayHit.normal, 4.4f, Space.Self);
                        JebOut.vessel.transform.Rotate(rayHit.normal, 4.4f, Space.Self);
                        //UnityEngine.Debug.Log("MagBootsController Update:  GetKeyDown loop finished ");
                    }
                }

                if (JebOut.isRagdoll) // only on ragdoll mode - if jeb is animated, he can walk on his own
                {
                    //UnityEngine.Debug.Log("MagBootsController Update:  JebOut.isRagdoll ");
                }
                else
                {
                    //UnityEngine.Debug.Log("MagBootsController Update:  JebOut animated ");
                }
            }
            else
            {
                //UnityEngine.Debug.Log("MagBootsController Update:  !BootsActive ");

            }
        }


    }


}
