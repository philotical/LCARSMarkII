using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_NCIGUI_Generic
    {
        public bool WindowState = false;
        private Rect windowPosition = new Rect(20, 20, 670, 370);
        private static System.Random rnd = new System.Random();
        private int windowID = rnd.Next();
        private string WindowContentKey = "";
        private GUIStyle myStyle = null;

        public void setWindowContent(string key)
        {
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic setWindowContent key=" + key);
            WindowContentKey = key;
        }
        public void setWindowState(bool state)
        {
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic setWindowState state=" + state);
            WindowState = state;
        }
        public bool getWindowState()
        {
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic getWindowState");
            return WindowState;
        }

        public void OnGUI()
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
            { return; }

            if (HighLogic.LoadedSceneIsEditor)
            { return; }

            if (!LCARS_NCI.Instance.GUI.Generic.WindowState)
            { return; }

            //UnityEngine.Debug.Log("### NCI OnGUI 1 ");
            if (myStyle == null)
            {
                myStyle = new GUIStyle();
                myStyle.margin = new RectOffset(0, 0, 0, 0);
                myStyle.padding = new RectOffset(0, 0, 0, 0);
                myStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS_NCI.Instance.NCI_Plugin_Path() + "Icons/blind", false);
                myStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS_NCI.Instance.NCI_Plugin_Path() + "Icons/blind", false);
                myStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS_NCI.Instance.NCI_Plugin_Path() + "Icons/blind", false);
                myStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS_NCI.Instance.NCI_Plugin_Path() + "Icons/blind", false);
            }



            windowPosition = LCARS_NCI.Instance.ClampToScreen(GUILayout.Window(windowID, windowPosition, GenericGUI, ""));
            //UnityEngine.Debug.Log("### NCI OnGUI end ");

        }
        public void GenericGUI(int windowID)
        {
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic NaturalCauseInc_Window 1 ");
            if (HighLogic.LoadedSceneIsEditor)
                return;
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic NaturalCauseInc_Window 2 ");
            if (!WindowState)
                return;
            UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic NaturalCauseInc_Window 3 ");

            GUILayout.BeginVertical();
            switch (LCARS_NCI.Instance.GUI.Generic.WindowContentKey)
            {
                case "NCIConversation":
                    UnityEngine.Debug.Log("### LCARS_NCIGUI_Generic GUI_NCIConversation 1 ");
                    LCARS_NCI.Instance.Data.MissionArchive.RunningMission.RunningStep.RunningConversation.ConversationGUI();
                    break;
            }
            GUILayout.EndVertical();

            GUI.DragWindow();

        }


    }
}
