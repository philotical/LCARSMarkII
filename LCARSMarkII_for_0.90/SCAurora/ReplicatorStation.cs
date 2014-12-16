using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Philotical
{
    internal class LCARS_ReplicatorStation : PartModule
    {

        LCARS_ImpulseDrive LCARSRef = null;
        Vessel thisVessel;
        internal void init(Vessel v)
        {
            thisVessel = v;
            if (LCARSRef==null)
            {
                UnityEngine.Debug.Log("ReplicatorStation init");
                try
                {
                    LCARSRef = thisVessel.GetComponent<LCARS_ImpulseDrive>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("ReplicatorStation init: LCARSRef ex=" + ex);
                }
            }

        }

        internal void GUI(Transform ReplicatorLocation)
        {
            if (LCARSRef==null) // because onStart is too early sometimes..
            {
                try
                {
                    LCARSRef = thisVessel.GetComponent<LCARS_ImpulseDrive>();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("ReplicatorStation GUI: LCARSRef ex=" + ex);
                }
            }
            GUILayout.BeginVertical(GUILayout.Width(280));
            GUILayout.Label("Select your Poison:", GUILayout.Width(280));

            if (GUILayout.Button("Earl Gray, Hot"))
            {
                LCARSRef.audLib.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Earl Gray, Hot ");
            }
            if (GUILayout.Button("Finelian Toddy"))
            {
                LCARSRef.audLib.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Finelian Toddy ");
            }
            if (GUILayout.Button("Panfried Catfish"))
            {
                LCARSRef.audLib.play("LCARS_Replicator", ReplicatorLocation.gameObject);
                UnityEngine.Debug.Log("ReplicatorStation:  Panfried Catfish ");
            }

            GUILayout.EndVertical();
        }
    }
}
