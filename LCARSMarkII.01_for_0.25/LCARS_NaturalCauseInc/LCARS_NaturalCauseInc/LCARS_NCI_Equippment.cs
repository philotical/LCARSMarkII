using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    // gathers a defined resource while in range
    [Serializable]
    public class LCARS_NCI_BussardCollectors
    {
        public void init()
        { 
        }

        public void GUI()
        { 
        }

        public float collect(PartResource partResource, GameObject targetEgg, float distance_threshhold)
        {

            return 0f;
        }
        public float calculateDensity(GameObject targetEgg, float distance_threshhold)
        {

            return 0f;
        }

    }

    // finds a defined gameObject by messuring the distance to vessel
    [Serializable]
    public class LCARS_NCI_IsoFluxDetector
    {
        public void init()
        {
        }

        public void GUI()
        {
        }

        public float detect(Vessel v, GameObject targetEgg, float distance_threshhold)
        {
            float d = Vector3.Distance(v.transform.position,targetEgg.transform.position);
            if (d > distance_threshhold)
            {
                return 0;
            }
            return d;
        }

    }

    // can be used to analyze artefact, will return a defined result 
    [Serializable]
    public class LCARS_NCI_NucleonicAnalyzer
    {
        public void init()
        {
        }

        public void GUI()
        {
        }

        public string analyze(string invItemIDCode, GameObject targetEgg)
        {
            //float d = Vector3.Distance(v.transform.position, targetEgg.transform.position);
            return "";
        }
    }

    // can be used to transform artefacts, feed one, receive an other
    [Serializable]
    public class LCARS_NCI_PteroplasticScrambler
    {
        public void init()
        {
        }

        public void GUI()
        {
        }
    }
}
