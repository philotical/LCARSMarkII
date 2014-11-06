using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace LCARSMarkII
{


    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    internal class LCARS_NCI_StockToolbar : MonoBehaviour
    {
        private static Texture2D ButtonActive;
        private static Texture2D ButtonInactive;
        private ApplicationLauncherButton stockToolbarBtn;
        private bool buttonState = false;

        public LCARS_NCI NCI;

        private void stockToolbarBtnShow()
        {
            // Add here the code to open your app window
            if (NCI == null)
            {
                NCI = LCARS_NCI.Instance;
                NCI.init();
            }
            NCI.GUI.SpaceCenter.setWindowState(true);
            buttonState = true;
            RefreshButtonTexture();
            print("### NCI StockToolbar stockToolbarBtnShow  NCI.GUI.SpaceCenter.getWindowState=" + NCI.GUI.SpaceCenter.getWindowState());
        }
        private void stockToolbarBtnHide()
        {
            // Add here the code to close your app window
            NCI.GUI.SpaceCenter.setWindowState(false);
            buttonState = false;
            RefreshButtonTexture();
            print("### NCI StockToolbar stockToolbarBtnHide  NCI.GUI.SpaceCenter.getWindowState=" + NCI.GUI.SpaceCenter.getWindowState());
        }

        public void OnDestroy()
        {
        }

        private void OnGUI()
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            {return;}

            if (NCI == null)
            {
                NCI = LCARS_NCI.Instance;
                NCI.init();
            }

            NCI.GUI.SpaceCenter.OnGUI();
        }
        
        public bool ButtonState
        {
            get { return buttonState; }
            set { buttonState = value; }
        }
        void Start()
        {
            print("### NCI StockToolbar Start");
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            { return; }
            
                Load(ref ButtonActive, "Icons/ButtonActive.png");
                Load(ref ButtonInactive, "Icons/ButtonInactive.png");
                GameEvents.onGUIApplicationLauncherReady.Add(CreateButton);
            DontDestroyOnLoad(this); // twiddle - new
        }
        private void Load(ref Texture2D tex, string file)
        {
            if (tex == null)
            {
                tex = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                tex.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), file)));
            }
        }
        public void CreateButton()
        {

            print("### NCI MakeButton");
            if (stockToolbarBtn != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(stockToolbarBtn);
            }
            stockToolbarBtn = ApplicationLauncher.Instance.AddModApplication(
            stockToolbarBtnHide, stockToolbarBtnShow, null, null, null, null,
            ApplicationLauncher.AppScenes.SPACECENTER, GetTexture());
            DontDestroyOnLoad(stockToolbarBtn);
                stockToolbarBtn.SetTrue(false);
        }
        public void RefreshButtonTexture()
        {
            if (stockToolbarBtn != null)
            {
                // here be twiddles
                stockToolbarBtn.SetTexture(GetTexture());
            }
        }
        private Texture2D GetTexture()
        {
            Texture2D tex;
            tex = (buttonState ? ButtonActive : ButtonInactive);
            return tex;
        }

    }
}





















