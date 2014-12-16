using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    public class LCARS_PowerTaker_Type
    {

        public string takerName { get; set; }
        /*
        public string takerType { get; set; }
        public string takerSubType { get; set; }

        public float L1_usage { get; set; } // Level1: allways power draw
        public float L2_usage { get; set; } // Level2: on action power draw
        public float L3_usage { get; set; } // Level3: something else additionally

        public float consumption_current { get; set; }
        public float consumption_total { get; set; }
         * */
    }

    public class LCARS_PowerSystem_Util
    {


        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_PowerSystems SetVessel done ");
        }



        public LCARS_ShipSystem_Type getPowerTaker(string takerName)
        {
            return LCARS.lODN.ShipSystems[takerName];
        }

        public Dictionary<string, LCARS_ShipSystem_Type> getPowerTakers()
        {
            return LCARS.lODN.ShipSystems;
        }
        /*
        public void setPowerTaker(string takerName, string takerType, string takerSubType, float L1_usage = 0f, float L2_usage = 0f, float L3_usage = 0f)
        {
            if (LCARS.lODN.ShipSystems == null)
            {
                LCARS.lODN.ShipSystems = new Dictionary<string, LCARS_ShipSystem_Type>();
            }
            if (LCARS.lODN.ShipSystems.ContainsKey(takerName))
            {
                LCARS.lODN.ShipSystems[takerName].powerSystem_consumption_current = 0f;
                LCARS.lODN.ShipSystems[takerName].powerSystem_consumption_total = 0f;
                LCARS.lODN.ShipSystems[takerName].powerSystem_takerType = takerType;
                LCARS.lODN.ShipSystems[takerName].powerSystem_takerSubType = takerSubType;
                LCARS.lODN.ShipSystems[takerName].powerSystem_L1_usage = L1_usage;
                LCARS.lODN.ShipSystems[takerName].powerSystem_L2_usage = L2_usage;
                LCARS.lODN.ShipSystems[takerName].powerSystem_L3_usage = L3_usage;
            }
         */   
            /*
            if (LCARS.lODN.ShipStatus.LCARS_PowerTakers == null)
            {
                LCARS.lODN.ShipStatus.LCARS_PowerTakers = new Dictionary<string, LCARS_PowerTaker_Type>();
            }
            LCARS_PowerTaker_Type foo = new LCARS_PowerTaker_Type();
            foo.takerName = takerName;
            foo.takerType = takerType;
            foo.takerSubType = takerSubType;
            foo.L1_usage = L1_usage;
            foo.L2_usage = L2_usage;
            foo.L3_usage = L3_usage;
            if (LCARS.lODN.ShipStatus.LCARS_PowerTakers.ContainsKey(takerName))
            {
                LCARS.lODN.ShipStatus.LCARS_PowerTakers[takerName] = foo;
            }
            else
            {
                foo.consumption_current = 0f;
                foo.consumption_total = 0f;
                LCARS.lODN.ShipStatus.LCARS_PowerTakers.Add(takerName, foo);
            }
            return LCARS.lODN.ShipStatus.LCARS_PowerTakers[takerName];
           */
       // }
        public void reset_Powerstats()
        {
            foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
            {
                LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_current = 0;
            }
        }

        // returns one if 100% was met, 0.831 if only 83.1% was met
        public float draw(string takerName, float amount)
        {
            amount = (amount >= 0) ? amount : amount * -1;

            //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 1 ");
                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 2 ");
                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw takerName=" + takerName + " amount=" + amount);

                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 3 ");
                float draw = thisVessel.rootPart.RequestResource("ElectricCharge", amount);

                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 4 ");
                if (LCARS.lODN.ShipSystems.ContainsKey(takerName))
                {

                    //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 5 ");
                    LCARS.lODN.ShipSystems[takerName].powerSystem_consumption_current = draw;

                    //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 6 ");
                    LCARS.lODN.ShipSystems[takerName].powerSystem_consumption_total += draw;
                }
                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 7 ");

                float performance = draw / (amount / 100) / 100;
                performance = (performance >= 0 && performance <= 1 ) ? performance : 0 ;
                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 8 ");
                
                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw performance=" + performance);

                //UnityEngine.Debug.Log("LCARS_PowerSystem: draw 9 ");
                return performance;
                /*
                */
            //return 1;
            /*try
            {
            }
            catch(Exception ex) {
                UnityEngine.Debug.Log("LCARS_PowerSystem: draw Exception ex=" + ex);
                return 0.0f;
            }*/
        }

        public float get_consumption_total(bool alltime = false)
        {
            float returntotal = 0;
            foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
            {
                float foo = (alltime) ? LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_total : LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_current;
                returntotal += foo;
            }
            return returntotal;
        }

        public float get_consumption_main_systems(bool alltime = false)
        {
            float returntotal = 0;
            foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
            {
                if (pair.Value.powerSystem_takerType == "MainSystem")
                {
                    float foo = (alltime) ? LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_total : LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_current;
                    returntotal += foo;
                }
            }
            return returntotal;
        }


        public float get_consumption_sub_systems(bool alltime = false)
        {
            float returntotal = 0;
            foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
            {
                if (pair.Value.powerSystem_takerType == "SubSystem")
                {
                    float foo = (alltime) ? LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_total : LCARS.lODN.ShipSystems[pair.Value.name].powerSystem_consumption_current;
                    returntotal += foo;
                }
            }
            return returntotal;
        }
    }
}
