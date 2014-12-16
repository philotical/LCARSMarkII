using UnityEngine;
using System.Collections;
using System;

public class IonStorm : PartModule 
{
    private LineRenderer lineRenderer=null;
    private GameObject targetObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void FixedUpdate() 
	{
        if (HighLogic.LoadedSceneIsEditor)
            return;

        UnityEngine.Debug.Log("IonStorm: Update this.vessel.rootPart.name=" + this.vessel.rootPart.name);

        this.vessel.distancePackThreshold = 20000f;
        Vessel.loadDistance = 20000f;
        
            //
            Component[] BCols = this.vessel.gameObject.GetComponentsInChildren<SphereCollider>(true);

            //Vessel.ContainsCollider(UnityEngine.Collider)

            int vortex_counter = 1;
            foreach (SphereCollider Vortex in BCols)
            {
                UnityEngine.Debug.Log("IonStorm: SphereCollider found Vortex.gameObject.name=" + Vortex.gameObject.name);

                if (Vortex.gameObject.name.Contains("IonCloud_"))
                {
                    /*
                    UnityEngine.Debug.Log("IonStorm: 1 " );
                    targetObject = null;
                    if (vortex_counter > BCols.Length)
                    {
                        UnityEngine.Debug.Log("IonStorm: 2 ");
                        string foo = "PlasmaVortex_00" + vortex_counter.ToString();
                        targetObject = GameObject.Find(foo);
                        UnityEngine.Debug.Log("IonStorm: 3 ");
                    }
                    else 
                    {
                        UnityEngine.Debug.Log("IonStorm: 4 ");
                        targetObject = GameObject.Find("PlasmaVortex_000");
                        UnityEngine.Debug.Log("IonStorm: 5 ");
                    }
                    if (targetObject == null)
                    {
                        UnityEngine.Debug.Log("IonStorm: target Object not found using alternative ");
                        targetObject = FlightGlobals.ActiveVessel.gameObject;
                    }

                    GameObject ElectricJolt = null;
                    try
                    {
                        ElectricJolt = Vortex.gameObject.transform.FindChild("ElectricJolt_00" + vortex_counter.ToString()).gameObject;
                        lineRenderer = ElectricJolt.GetComponent<LineRenderer>();
                    }
                    catch(Exception e)
                    { 
                    }
                    UnityEngine.Debug.Log("IonStorm: 6 ");

                    if (ElectricJolt == null)
                    {
                        UnityEngine.Debug.Log("IonStorm: 6a ");
                        ElectricJolt = new GameObject("ElectricJolt_00" + vortex_counter.ToString());
                        ElectricJolt.transform.parent = Vortex.gameObject.transform;
                        ElectricJolt.transform.position = Vortex.gameObject.transform.position;
                        lineRenderer = ElectricJolt.AddComponent<LineRenderer>();
                        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                        lineRenderer.transform.parent = ElectricJolt.transform;
                        lineRenderer.SetWidth(5f, 5f);
                        lineRenderer.SetVertexCount(2);
                        lineRenderer.useWorldSpace = false;
                        UnityEngine.Debug.Log("IonStorm: 6b ");
                    }
                    /*
                    else 
                    {
                        lineRenderer.SetWidth(0, 0);
                        lineRenderer.SetPosition(0, Vector3.zero);
                        lineRenderer.SetPosition(1, Vector3.zero);
                        lineRenderer.SetPosition(2, Vector3.zero);
                        lineRenderer.SetPosition(3, Vector3.zero);
                        lineRenderer.SetPosition(4, Vector3.zero);
                        lineRenderer.SetPosition(5, Vector3.zero);
                        lineRenderer.SetPosition(6, Vector3.zero);
                        lineRenderer.SetPosition(7, Vector3.zero);
                        lineRenderer.SetPosition(8, Vector3.zero);
                        lineRenderer.SetPosition(9, Vector3.zero);
                    }
                    * /
                    UnityEngine.Debug.Log("IonStorm: 6c ");

                    lineRenderer.SetPosition(0, ElectricJolt.transform.localPosition);
                    UnityEngine.Debug.Log("IonStorm: 7 ");

                    for (int i = 1; i < 8; i++)
	                {
                        var pos = Vector3.Lerp(ElectricJolt.transform.localPosition, targetObject.transform.localPosition, i / 8.0f);

                        UnityEngine.Debug.Log("IonStorm: 8 ");
                        pos.x += UnityEngine.Random.Range(-10f, 10f);
                        pos.y += UnityEngine.Random.Range(-10f, 10f);

                        UnityEngine.Debug.Log("IonStorm: 9 ");
                        lineRenderer.SetPosition(i, pos);
                        UnityEngine.Debug.Log("IonStorm: 10 ");
                    }
                    lineRenderer.SetPosition(1, targetObject.transform.localPosition);
                    UnityEngine.Debug.Log("IonStorm: 11 ");
                    lineRenderer = null;

	                */

                    if (PullShip != null && this.vessel.id != FlightGlobals.ActiveVessel.id && PullShip.gameObject.name != "DestructionCollider")
		            {

                        UnityEngine.Debug.Log("IonStorm: Update PullShip=" + PullShip.name);
                        UnityEngine.Debug.Log("IonStorm:  Update PullShip interacting with Vortex.gameObject.name=" + Vortex.gameObject.name);
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
            UnityEngine.Debug.Log("IonStorm:  Update done ");




	}




	bool forcetype = true;
	bool gravitytype = true;
	Collider 	PullShip = null;	
	void OnTriggerStay(Collider other) 
	{
        if (HighLogic.LoadedSceneIsEditor)
            return;

        UnityEngine.Debug.Log("IonStorm:  OnTriggerStay ");


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

        UnityEngine.Debug.Log("IonStorm:  OnTriggerEnter ");


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


        UnityEngine.Debug.Log("IonStorm:  OnTriggerExit ");

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





















