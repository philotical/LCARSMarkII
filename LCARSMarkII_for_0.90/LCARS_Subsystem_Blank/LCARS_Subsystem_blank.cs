
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_Blank : ILCARSPlugin
    {
        public string subsystemName { get { return "Blank"; } }
        public string subsystemDescription { get { return "Demo Subsystem"; } }
        public string subsystemStation { get { return "Bridge"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 0f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemPanelState { get; set; } // has to be false at start

	// not used, unless LCARS explicitly calls it, wich doesn't happen if I don't add that call to LCARS - it's required for the NCI subsystem
	// for you it's just clutter..
        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = ""){}

	// at scene start, LCARS will let your subsystem know, wich vessel is the one to work with..
	// use the var thisVessel as only HostVessel-ref in your code
	// you have access to a valid LCARS-ref with the var LCARS
        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  SetVessel done ");
        }

	// the subsystem Util will call this function in onGUI() to display your gui in the right station.
        public void getGUI()
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI begin ");
            GUILayout.BeginHorizontal();
            GUILayout.Label("LCARS_Subsystem_Blank : ");
		
		/*
			put your GUI here!
		*/
		coolTestMethode();

            GUILayout.EndHorizontal();
            //UnityEngine.Debug.Log("LCARS_Subsystem_Blank  getGUI done ");
        }

	// add below this all the methodes you need in on gui - they can be private because no one else will be able to call them anyway..
	// only the interface functions can be accessed by LCARS or any other assembly..
	private void coolTestMethode()
	{
		GUILayout.Label(subsystemName  + " is operational");		
	}

    }

}
