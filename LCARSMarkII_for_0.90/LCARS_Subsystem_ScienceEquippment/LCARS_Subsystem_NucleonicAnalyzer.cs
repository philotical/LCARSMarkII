using System;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_NucleonicAnalyzer : ILCARSPlugin
    {
        public string subsystemName { get { return "Nucleonic Analyzer"; } }
        public string subsystemDescription { get { return "Analyze Artefacts"; } }
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

        string textline = null;
        string artefact1 = null;
        bool progressLog_written = false;
        LCARS_ArtefactInventory_Type artefact_of_interest_backup = null;
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI begin ");
            GUILayout.BeginVertical();
            if (GUILayout.Button("Reset this system"))
            {
                textline = null;
                artefact1 = null;
                progressLog_written = false;
                LCARS.lODN.NucleonicAnalyzer_artefact_of_interest = null;
            }

            if (LCARS.lODN.NucleonicAnalyzer_artefact_of_interest != null)
            {
                if (artefact_of_interest_backup != LCARS.lODN.NucleonicAnalyzer_artefact_of_interest)
                {
                    textline = null;
                    artefact1 = null;
                    progressLog_written = false;
                    artefact_of_interest_backup = LCARS.lODN.NucleonicAnalyzer_artefact_of_interest;
                }
                if (textline == null || artefact1 == null)
                {
                    textline = LCARSNCI_Bridge.Instance.GetEquippment_Setting("NucleonicAnalyzer", "textline",LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.idcode);
                    artefact1 = LCARSNCI_Bridge.Instance.GetEquippment_Setting("NucleonicAnalyzer", "artefact1", LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.idcode);
                }
                GUILayout.Label("Analyzing: " + LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.name);
                if (artefact1 == LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.idcode)
                {
                    GUILayout.Label("Result: " + textline);

                    if (!progressLog_written)
                    {
                        try
                        {
                            LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog("NucleonicAnalyzer was used on " + LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.name, "The Artefact showed the following result: " + textline);
                        }
                        catch
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_NucleonicAnalyzer getGUI LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog failed <artefact1=" + LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.name + " textline=" + textline + ">");
                        }
                        progressLog_written = true;
                    }
                }
                else
                {
                    GUILayout.Label("Result: no conclusive results");

                    if (!progressLog_written)
                    {
                        try
                        {
                            LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog("NucleonicAnalyzer was used on " + LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.name, "The Artefact showed the following result: no conclusive results");
                        }
                        catch
                        {
                            UnityEngine.Debug.Log("### LCARS_Subsystem_NucleonicAnalyzer getGUI LCARSNCI_Bridge.Instance.LCARS_add_to_MissionLog failed <artefact1=" + LCARS.lODN.NucleonicAnalyzer_artefact_of_interest.name + " =>no conclusive results>");
                        }
                        progressLog_written = true;
                    }
                }
            }
            else 
            {
                GUILayout.Label("Analyzing: no artefact selected");
            }
            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("LCARS_Subsystem_ShipInfo getGUI done ");
        }

    } 
    
}
