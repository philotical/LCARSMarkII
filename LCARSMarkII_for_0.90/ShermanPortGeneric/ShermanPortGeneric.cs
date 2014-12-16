using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace LCARSMarkII
{
    public class ShermanPortGeneric : PartModule
    {
        [KSPField(isPersistant = true)]
        public string nodename;
        [KSPField(isPersistant = true)]
        public float nodeforce;

        public Transform ShermanPortNode = null;
        public Transform ShermanPortCounterNode = null;

        public override void OnLoad(ConfigNode node)
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                Debug.Log("ShermanPortGeneric: LoadedSceneIsEditor - skipping ");
                return;
            }
            if (this.part == null)
            {
                Debug.Log("ShermanPortGeneric: this.part == null - skipping ");
                return;
            }

            nodename = node.GetValue("nodename");
            Debug.Log("ShermanPortGeneric: OnLoad   nodename=" + nodename);
            nodeforce = ToFloat(node.GetValue("nodeforce"));
            Debug.Log("ShermanPortGeneric: OnLoad   nodeforce=" + nodeforce);
            /*
            try
            {
                ShermanPortNode = this.part.FindModelTransform(nodename);
                Debug.Log("ShermanPortGeneric: ShermanPortNode was found nodename=" + nodename);
            }
            catch 
            {
                Debug.Log("ShermanPortGeneric: ShermanPortNode was not found nodename=" + nodename);
            }
            */
        }
        public static float ToFloat(string s)
        {
            //UnityEngine.Debug.Log("ShermanPortGeneric: ToFloat s="+s);
            float number;
            bool result = float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            if (result)
            {
                return number;
            }
            else
            {
                return 0;
            }
        }

        private float lastUpdate = 0.0f;
        private float Interval = 5.0f;
        void OnTriggerStay(Collider other)
        {
            if (ShermanPortNode == null)
            {
                try
                {
                    ShermanPortNode = this.part.FindModelTransform(nodename);
                }
                catch 
                {
                }
                if (ShermanPortNode == null)
                {
                }
                else
                {
                    Debug.Log("ShermanPortGeneric: ShermanPortNode was found nodename=" + nodename);
                }
            }
            if (ShermanPortNode == null)
            {
                Debug.Log("ShermanPortGeneric: ShermanPortNode is NULL - skipping  nodename=" + nodename);
                return;
            }

            ShermanPortCounterNode = other.transform;

            if (ShermanPortCounterNode.parent.gameObject.name.Contains("StandardShermanPort"))
            {
                Debug.Log("ShermanPortGeneric: collider name did match ShermanPortCounterNode.parent.gameObject=" + ShermanPortCounterNode.parent.gameObject);

                Vector3 pullDirection = ShermanPortNode.position - ShermanPortCounterNode.parent.gameObject.transform.position;

                other.attachedRigidbody.AddForce(pullDirection * nodeforce);

                if ((Time.time - lastUpdate) > Interval)
                {
                    //ShermanPortCounterNode.parent.gameObject.transform.position = ShermanPortNode.position;
                }            
            
            }
            else
            {
                //Debug.Log("ShermanPortGeneric: collider name did not match other.name=" + other.name);
            }
        }
       
        
        





    }





}
