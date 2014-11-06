using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public sealed class LCARS_NCI
    {
        private static LCARS_NCI _instance;
        private string NCIPluginPath = "LCARS_NaturalCauseInc/";

        public static LCARS_NCI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LCARS_NCI();
                    _instance.init();
                    //_instance.Data.init();
                }

                return _instance;
            }
        }


        public LCARS_NCI_Data Data;
        public LCARS_NCI_GUI GUI;

        public Vessel thisVessel;
        //public LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            //LCARS = thisVessel.LCARS();
        }



        public void init()
        {

            if (this.Data == null)
            {
                this.Data = new LCARS_NCI_Data();
                this.Data.init();
            }
            if (this.GUI == null)
            {
                this.GUI = new LCARS_NCI_GUI();
                this.GUI.init();
            }
        }


        public Rect ClampToScreen(Rect r)
        {
            r.x = Mathf.Clamp(r.x, 0 + (r.x / 2), Screen.width - (r.width / 2));
            r.y = Mathf.Clamp(r.y, 0, Screen.height - 50);
            return r;
        }
        public string NCI_Plugin_Path()
        {
            return NCIPluginPath;
        }

    }
}
