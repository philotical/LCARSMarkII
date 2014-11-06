using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace LCARSMarkII
{


    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    internal class LCARS_NCIMC_StockToolbar : MonoBehaviour
    {
        private static Texture2D ButtonActive;
        private static Texture2D ButtonInactive;
        private ApplicationLauncherButton stockToolbarBtn;
        private bool buttonState = false;

        public LCARS_NCIMC NCIMC;

        private void stockToolbarBtnShow()
        {
            // Add here the code to open your app window
            if (NCIMC == null)
            {
                NCIMC = new LCARS_NCIMC();
            }
            NCIMC.setWindowState(true);
            buttonState = true;
            RefreshButtonTexture();
            print("### NCIMC StockToolbar stockToolbarBtnShow  NCIMC.getWindowState=" + NCIMC.getWindowState());
        }
        private void stockToolbarBtnHide()
        {
            // Add here the code to close your app window
            NCIMC.setWindowState(false);
            buttonState = false;
            RefreshButtonTexture();
            print("### NCIMC StockToolbar stockToolbarBtnHide  NCIMC.getWindowState=" + NCIMC.getWindowState());
        }

        private void OnGUI()
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            {return;}

            if (NCIMC == null)
            {
                NCIMC = new LCARS_NCIMC();
            }

            NCIMC.OnGUI();
        }
        
        public bool ButtonState
        {
            get { return buttonState; }
            set { buttonState = value; }
        }
        void Start()
        {
            print("### NCIMC StockToolbar Start");
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
            { return; }
            
                Load(ref ButtonActive, "Icons/NCIMC_ButtonActive.png");
                Load(ref ButtonInactive, "Icons/NCIMC_ButtonInactive.png");
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

            print("### NCIMC MakeButton");
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





















