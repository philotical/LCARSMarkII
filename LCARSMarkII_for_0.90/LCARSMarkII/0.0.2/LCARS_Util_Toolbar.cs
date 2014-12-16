using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LCARSMarkII
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public class LCARS_Util_Toolbar : MonoBehaviour
    {
        private static Texture2D ButtonActive;
        private static Texture2D ButtonInactive;
        private ApplicationLauncherButton stockToolbarBtn;
        private bool buttonState = false;


        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            print("###LCARS_Util_Toolbar SetVessel");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }


            internal void stockToolbarBtnShow()
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                if (LCARS == null)
                {
                    return;
                }

                // Add here the code to open your app window
                LCARS.setWindowState(true);
                buttonState = true;
                RefreshButtonTexture();
                print("###LCARS_Util_Toolbar stockToolbarBtnShow  LCARS.getWindowState=" + LCARS.getWindowState());
            }
            internal void stockToolbarBtnHide()
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                if (LCARS == null)
                {
                    return;
                }


                // Add here the code to close your app window
                LCARS.setWindowState(false);
                buttonState = false; 
                RefreshButtonTexture();
                print("###LCARS_Util_Toolbar stockToolbarBtnHide  LCARS.getWindowState=" + LCARS.getWindowState());
            }

            private void OnGUI()
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                if (LCARS == null)
                {
                    return;
                }
                //if (HighLogic.LoadedScene != GameScenes.FLIGHT || LCARS == null)
                //{ return; }
                if (LCARS != null)
                {
                    buttonState = LCARS.getWindowState();
                    //LCARS.OnGUI();
                }
                else 
                {
                    buttonState = false;
                }
            }

            public bool ButtonState
            {
                get { return buttonState; }
                set { buttonState = value; }
            }
            void Start()
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                print("###LCARS_Util_Toolbar Start begin ");
                //if (HighLogic.LoadedScene != GameScenes.FLIGHT || LCARS == null)
                //{ return; }

                Load(ref ButtonActive, "Icons/ButtonActive.png");
                Load(ref ButtonInactive, "Icons/ButtonInactive.png");
                GameEvents.onGUIApplicationLauncherReady.Add(CreateButton);
                DontDestroyOnLoad(this); // twiddle - new
                print("###LCARS_Util_Toolbar Start end ");
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
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                print("###LCARS_Util_Toolbar MakeButton 1 ");

                if (LCARS == null /*|| thisVessel != FlightGlobals.ActiveVessel*/)
                {
                    //return; // this line makes trouble - button dissappears
                }

                print("###LCARS_Util_Toolbar MakeButton 2 ");
                if (stockToolbarBtn != null)
                {
                    ApplicationLauncher.Instance.RemoveModApplication(stockToolbarBtn);
                }
                stockToolbarBtn = ApplicationLauncher.Instance.AddModApplication(
                stockToolbarBtnHide, stockToolbarBtnShow, null, null, null, null,
                ApplicationLauncher.AppScenes.FLIGHT,
                GetTexture());
                DontDestroyOnLoad(stockToolbarBtn);
                stockToolbarBtn.SetTrue(false);
                print("###LCARS_Util_Toolbar MakeButton 3 ");
            }
            public void RefreshButtonTexture()
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    return;
                }
                if (stockToolbarBtn != null)
                {
                    print("###LCARS_Util_Toolbar RefreshButtonTexture");
                    // here be twiddles
                    stockToolbarBtn.SetTexture(GetTexture());
                }
            }
            private Texture2D GetTexture()
            {
                Texture2D tex;
                tex = (buttonState ? ButtonActive : ButtonInactive);
                print("###LCARS_Util_Toolbar GetTexture="+tex.ToString());
                return tex;
            }

        }
}
