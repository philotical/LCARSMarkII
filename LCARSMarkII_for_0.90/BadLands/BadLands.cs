using UnityEngine;
using System.Collections;
using System;


/*
 Contains Egg: NCI_AlienPoint
 */


public class BadLands : PartModule
{
    private LineRenderer lineRenderer = null;
    private GameObject targetObject;
    Component[] BCols;

    void Start()
    {
        UnityEngine.Debug.Log("BadLands: started ");
        BCols = this.vessel.gameObject.GetComponentsInChildren<SphereCollider>(true);
    }
    void OnDestroy()
    {
        //Vessel.loadDistance = originalLoadDistance;
    }

    // Update is called once per frame
    private float lastFixedUpdate = 0.0f;
    private float logInterval = 0.1f;
    void FixedUpdate()
    {
        if (HighLogic.LoadedSceneIsEditor)
            return;

        if (BCols==null)
            return;

        if ((Time.time - lastFixedUpdate) > logInterval)
        {
            lastFixedUpdate = Time.time;

            if (PullShip != null && this.vessel.id != FlightGlobals.ActiveVessel.id && PullShip.gameObject.name != "DestructionCollider")
            {
                    int vortex_counter = 1;
            //Part.FindModelTransforms(string)
                    foreach (SphereCollider Vortex in BCols)
                    {
                    //vesselPart.rigidbody.AddForce(antiGeeVector, ForceMode.Acceleration);
                    //UnityEngine.Debug.Log("LCARS_Background_Ghost: CancelG2 thisVessel.vesselName=" + thisVessel.vesselName + " vesselPart=" + vesselPart + " antiGeeVector=" + antiGeeVector.ToString());
                    if (Vortex.gameObject.name.Contains("PlasmaWake"))
                    {
                        //UnityEngine.Debug.Log("BadLands: Transform found Vortex.gameObject.name=" + Vortex.gameObject.name);
                        //UnityEngine.Debug.Log("BadLands: Transform found Vortex.name=" + Vortex.name);



                            //UnityEngine.Debug.Log("BadLands: Update PullShip=" + PullShip.name);
                            //UnityEngine.Debug.Log("BadLands:  Update PullShip interacting with Vortex.gameObject.name=" + Vortex.gameObject.name);
                            int gravitational_constant = 19; //<-- make something up here until you get pleasing results
                            float distance = UnityEngine.Vector3.Distance(Vortex.transform.position, PullShip.transform.position);
                            float mass1 = 100;// <-- mess around with these until you get pleasing results
                            float mass2 = 1;// <-- this can probably always stay 1 for the player
                            float force = gravitational_constant * ((mass1 * mass2) / (distance/3));
                            // apply the force from the player toward the Vortex
                            Vector3 force_direction = (Vortex.transform.position - PullShip.transform.position).normalized;
                            Vector3 force_vector = force_direction * force;
                            PullShip.attachedRigidbody.AddForce(force_vector * force, ForceMode.Acceleration);

                        vortex_counter++;
                    }
                }
            }
            //UnityEngine.Debug.Log("BadLands:  Update done ");
        }



    }




    bool forcetype = true;
    bool gravitytype = true;
    Collider PullShip = null;
    void OnTriggerStay(Collider other)
    {
        if (HighLogic.LoadedSceneIsEditor)
            return;

        //UnityEngine.Debug.Log("BadLands:  OnTriggerStay ");


        if (forcetype && this.vessel.id != FlightGlobals.ActiveVessel.id)
        {
            if (other.attachedRigidbody)
            {



                // Calculate the y-axis relative to us
                //Vector3 cameraRelativeRight = transform.TransformDirection (Vector3.up);
                // Apply a force relative to the our y-axis

                PullShip = other;
            }
        }


    }



    //works if upright

    void OnTriggerEnter(Collider other)
    {
        if (HighLogic.LoadedSceneIsEditor)
            return;

        //UnityEngine.Debug.Log("BadLands:  OnTriggerEnter ");


        if (gravitytype && this.vessel.id != FlightGlobals.ActiveVessel.id)
        {

            if (other.attachedRigidbody)
            {
                //other.attachedRigidbody.useGravity = true;
                PullShip = other;

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (HighLogic.LoadedSceneIsEditor)
            return;


        //UnityEngine.Debug.Log("BadLands:  OnTriggerExit ");

        if (gravitytype && this.vessel.id != FlightGlobals.ActiveVessel.id)
        {
            if (other.attachedRigidbody)
            {

                PullShip = null;
                //other.attachedRigidbody.useGravity = false;
            }
        }
    }
}





















