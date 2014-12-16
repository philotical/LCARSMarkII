using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_PteroplasticScrambler : ILCARSPlugin
    {
        public string subsystemName { get { return "Pteroplastic Scrambler"; } }
        public string subsystemDescription {get{return "Can convert artefacts";}}
        public string subsystemStation { get { return "Science"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 20f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 30f; } } // Power draw for additional consumtion - is added to running
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

        string artefact2 = null;
        string artefact1 = null;
        bool progressLog_written = false;
        LCARS_ArtefactInventory_Type artefact_of_interest_backup = null;
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Reset this system"))
            {
                artefact2 = null;
                artefact1 = null;
                progressLog_written = false;
                LCARS.lODN.PteroplasticScrambler_artefact_of_interest = null;
            }
            if (LCARS.lODN.PteroplasticScrambler_artefact_of_interest != null)
            {
                if (artefact_of_interest_backup != LCARS.lODN.PteroplasticScrambler_artefact_of_interest)
                {
                    artefact2 = null;
                    artefact1 = null;
                    progressLog_written = false;
                    artefact_of_interest_backup = LCARS.lODN.PteroplasticScrambler_artefact_of_interest;
                }

                GUILayout.Label("Scrambling: " + LCARS.lODN.PteroplasticScrambler_artefact_of_interest.name);

                if (artefact2 == null || artefact1 == null)
                {
                    artefact1 = LCARSNCI_Bridge.Instance.GetEquippment_Setting("PteroplasticScrambler", "artefact1", LCARS.lODN.PteroplasticScrambler_artefact_of_interest.idcode);
                    artefact2 = LCARSNCI_Bridge.Instance.GetEquippment_Setting("PteroplasticScrambler", "artefact2", LCARS.lODN.PteroplasticScrambler_artefact_of_interest.idcode);
                }
                if (artefact1 == LCARS.lODN.PteroplasticScrambler_artefact_of_interest.idcode)
                {

                    if (!progressLog_written)
                    {
                        LCARSNCI_Bridge.Instance.takeArtefactFromPlayer(artefact1, false);
                        LCARSNCI_Bridge.Instance.giveArtefactToPlayer(artefact2, false, false);

                        try
                        {
                            LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog("PteroplasticScrambler was used on " + LCARS.lODN.PteroplasticScrambler_artefact_of_interest.name, "The Artefact was converted. Result: " + artefact2);
                        }
                        catch
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_PteroplasticScrambler getGUI LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog failed <PteroplasticScrambler_artefact_of_interest.name=" + LCARS.lODN.PteroplasticScrambler_artefact_of_interest.name + " artefact1=" + artefact1 + " artefact2=" + artefact2 + ">");
                        }
                        progressLog_written = true;
                    }
                    GUILayout.Label("The Artefact was converted.");
                    GUILayout.Label("Result: " + artefact2);
                }
                else
                {
                    GUILayout.Label("Result: Scrambling failed");

                    if (!progressLog_written)
                    {
                        try
                        {
                            LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog("PteroplasticScrambler was used on " + artefact1, "The Artefact was not converted. Result: Scrambling failed");
                        }
                        catch
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_PteroplasticScrambler getGUI LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog failed <artefact1=" + artefact1 + " =>no conclusive results>");
                        }
                        progressLog_written = true;
                    }
                }
            }
            else
            {
                GUILayout.Label("Scrambling: no artefact selected");
            }


            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
