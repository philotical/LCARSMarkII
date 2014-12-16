using UnityEngine;
using System.Collections;
using System;

public class IonStorm : PartModule 
{
    private LineRenderer lineRenderer=null;
    private GameObject targetObject;
    Component[] BCols;
	// Use this for initialization
    //private float originalLoadDistance = 2500;
    //private float originalUnLoadDistance = 2250;
    void Start() 
    {
        //originalLoadDistance = Vessel.loadDistance;
        //originalUnLoadDistance = Vessel.unloadDistance;
        BCols = this.vessel.gameObject.GetComponentsInChildren<SphereCollider>(true);
	
	}
    void OnDestroy()
    {
        //Vessel.loadDistance = originalLoadDistance;
    }

/*
    //static public void SetLoadDistance(float loadDistance = 2500, float unloadDistance = 2250)
    public void SetLoadDistance(float loadDistance = 0f, float unloadDistance = 0f)
    {
        loadDistance = (loadDistance == 0f) ? originalLoadDistance : loadDistance;
        unloadDistance = (unloadDistance == 0f) ? originalUnLoadDistance : unloadDistance;
        Vessel.loadDistance = loadDistance;
        Vessel.unloadDistance = unloadDistance;
    }
*/

	// Update is called once per frame
    void FixedUpdate() 
	{
        if (HighLogic.LoadedSceneIsEditor)
            return;


            /*
        if (Vector3.Distance(this.vessel.CoM, FlightGlobals.ActiveVessel.CoM) < 20000f)
        {
            //UnityEngine.Debug.Log("IonStorm: Update this.vessel.rootPart.name=" + this.vessel.rootPart.name);
            SetLoadDistance(20250f, 20000f);
            if(!this.vessel.loaded)
            {
            }
                        //this.vessel.Load();
                        //this.vessel.distancePackThreshold = 20000f;
                        Vessel.loadDistance = 20000f;
                        //this.vessel.distanceUnpackThreshold = 20000f;
                        //this.vessel.GoOffRails();
        }
        else
        {
            UnityEngine.Debug.Log("IonStorm: OnDestroy resetting load distance=" + this.vessel.rootPart.name);
            SetLoadDistance();
        }
            */
        //UnityEngine.Debug.Log("IonStorm: Update this.vessel.rootPart.name=" + this.vessel.rootPart.name);

        //this.vessel.distancePackThreshold = 20000f;
        //Vessel.loadDistance = 20000f;
        
            //

            //Vessel.ContainsCollider(UnityEngine.Collider)

        if (BCols != null)
        { 

            int vortex_counter = 1;
            foreach (SphereCollider Vortex in BCols)
            {
                //UnityEngine.Debug.Log("IonStorm: SphereCollider found Vortex.gameObject.name=" + Vortex.gameObject.name);

                if (Vortex.gameObject.name.Contains("IonCloud_"))
                {

                    if (PullShip != null && this.vessel.id != FlightGlobals.ActiveVessel.id && PullShip.gameObject.name != "DestructionCollider")
		            {

                        //UnityEngine.Debug.Log("IonStorm: Update PullShip=" + PullShip.name);
                        //UnityEngine.Debug.Log("IonStorm:  Update PullShip interacting with Vortex.gameObject.name=" + Vortex.gameObject.name);
                        int gravitational_constant = 19; //<-- make something up here until you get pleasing results
                        float distance = UnityEngine.Vector3.Distance(Vortex.transform.position , PullShip.transform.position);
                        float mass1 = 100;// <-- mess around with these until you get pleasing results
                        float mass2 = 1;// <-- this can probably always stay 1 for the player
                        float force = gravitational_constant * ((mass1 * mass2) / (distance));
                        // apply the force from the player toward the Vortex
                        Vector3 force_direction = (Vortex.transform.position - PullShip.transform.position).normalized;
                        Vector3 force_vector = force_direction * force;
                        PullShip.attachedRigidbody.AddForce(force_vector * force, ForceMode.Acceleration);
		            }
                    
                    vortex_counter++;
                }
            }
            //UnityEngine.Debug.Log("IonStorm:  Update done ");
        }




	}




	bool forcetype = true;
	bool gravitytype = true;
	Collider 	PullShip = null;	
	void OnTriggerStay(Collider other) 
	{
        if (HighLogic.LoadedSceneIsEditor)
            return;

        //UnityEngine.Debug.Log("IonStorm:  OnTriggerStay ");


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

        //UnityEngine.Debug.Log("IonStorm:  OnTriggerEnter ");


        if (gravitytype && this.vessel.id != FlightGlobals.ActiveVessel.id)
		{
				
			if(other.attachedRigidbody) 
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


        //UnityEngine.Debug.Log("IonStorm:  OnTriggerExit ");

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





















