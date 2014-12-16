using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public static class LCARS_Vessel_Extender
    {
        static LCARSMarkII LCARSRef;

        public static bool isLCARSVessel(this Vessel v)
        {
            try
            {
                return (v.GetComponent<LCARSMarkII>() != null ? true : false);
            }
            catch { return false; }
        }
        
        public static LCARSMarkII LCARS(this Vessel v)
        {
            try
            {
                LCARSRef = v.GetComponent<LCARSMarkII>();
                return LCARSRef;
            }
            catch { return null; }
        }
        
        public static bool LCARS_FullHalt_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_FullHalt; }
        
        public static bool LCARS_SlowDown_enebled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_SlowDown; }
        
        public static bool LCARS_InertiaDamper_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_InertiaDamper; }
        
        public static bool LCARS_MakeSlowToSave_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_MakeSlowToSave; }
        
        public static bool LCARS_FullImpulse_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_FullImpulse; }
        
        public static bool LCARS_UseReserves_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_UseReserves; }
        
        public static bool LCARS_AccelerationLock_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_AccelerationLock; }
        
        public static bool LCARS_ProgradeStabilizer_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_ProgradeStabilizer; }
        
        public static bool LCARS_HoldSpeed_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_HoldSpeed; }
        
        public static double LCARS_HoldSpeed_value(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_HoldSpeed_value; }
        
        public static bool LCARS_HoldHeight_enabled(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_HoldHeight; }
        
        public static double LCARS_HoldHeight_value(this Vessel v)
        { return v.LCARS().lODN.ShipStatus.LCARS_HoldHeight_value; }
        
        public static List<ProtoCrewMember> LCARSVessel_Crew(this Vessel v)
        { return v.GetVesselCrew(); }
        
        public static List<Part> LCARSVessel_Parts(this Vessel v)
        { return v.Parts; }
        
        public static Dictionary<string, PartModule> LCARSVessel_PartModules(this Vessel v)
        {
            Dictionary<string, PartModule> vesselPartModules = new Dictionary<string, PartModule>();
            foreach (Part p in v.Parts)
            {
                PartModuleList pML = p.Modules;
                foreach (PartModule pm in pML)
                {
                    if (!vesselPartModules.ContainsKey(pm.moduleName))
                    {
                        vesselPartModules.Add(pm.moduleName, pm);
                    }
                }
            }
            try
            {
                if (v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules == null)
                {
                    v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules = new Dictionary<string, PartModule>();
                }
                v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules = vesselPartModules;
            }catch{}
            return vesselPartModules;
        }
        
        public static Dictionary<int, ModuleGenerator> LCARSVessel_ResourceGenerators(this Vessel v)
        {
            if (v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators == null)
            {
                v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators = new Dictionary<int, ModuleGenerator>();
            }
            List<ModuleGenerator> MG_list = v.FindPartModulesImplementing<ModuleGenerator>();
            int i = 0;
            foreach (ModuleGenerator MG in MG_list)
            {
                List<ModuleGenerator.GeneratorResource> MG_GR = MG.outputList;
                if (!v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators.ContainsKey(i))
                {
                    v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators.Add(i, MG);
                }
                i++;
            }
            return v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators;
        }

        public static float LCARSVessel_AveragePartTemperature(this Vessel v)
        {
            v.LCARS().lODN.ShipStatus.AveragePartTemperature = v.LCARSVessel_getAveragePartTemperature();
            return v.LCARS().lODN.ShipStatus.AveragePartTemperature; 
        }

        public static float LCARSVessel_AveragePartTemperatureMax(this Vessel v)
        {
            v.LCARS().lODN.ShipStatus.AveragePartTemperatureMax = v.LCARSVessel_getAveragePartTemperatureMax();
            return v.LCARS().lODN.ShipStatus.AveragePartTemperatureMax; 
        }

        public static float LCARSVessel_Heat_percentage(this Vessel v)
        {
            v.LCARS().lODN.ShipStatus.LCARS_Heat_percentage = v.LCARSVessel_AveragePartTemperature() / (v.LCARSVessel_AveragePartTemperatureMax() / 100);
            return v.LCARS().lODN.ShipStatus.LCARS_Heat_percentage;
        }

        public static float LCARSVessel_HullIntegrity_percentage(this Vessel v)
        { return 100 - v.LCARSVessel_Heat_percentage(); }

        public static float LCARSVessel_getAveragePartTemperature(this Vessel v)
        {
            int i = 0;
            float tmp = 0f;
            v.LCARS().lODN.ShipStatus.AveragePartTemperature = 0f;
            foreach (Part p in v.LCARSVessel_Parts())
            {
                tmp += p.temperature;
                i++;
            }
            v.LCARS().lODN.ShipStatus.AveragePartTemperature = tmp / i;
            return v.LCARS().lODN.ShipStatus.AveragePartTemperature;
        }

        public static float LCARSVessel_getAveragePartTemperatureMax(this Vessel v)
        {
            int i = 0;
            float tmp = 0f;
            v.LCARS().lODN.ShipStatus.AveragePartTemperatureMax = 0f;
            foreach (Part p in v.LCARSVessel_Parts())
            {
                tmp += p.maxTemp;
                i++;
            }
            v.LCARS().lODN.ShipStatus.AveragePartTemperatureMax = tmp / i;
            return v.LCARS().lODN.ShipStatus.AveragePartTemperatureMax;
        }

        public static bool LCARSVessel_checkForPartWithModule(this Vessel v, string modName)
        {
            try
            {
                if (v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules == null)
                {
                    v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules = new Dictionary<string, PartModule>();
                }

                if (v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules.ContainsKey(modName))
                {
                    return (v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules[modName].moduleName == modName) ? true : false;
                }
                return false;
            }
            catch { return false; }
        }

        public static Part LCARSVessel_getPartWithModule(this Vessel v, string modName)
        {
            if (v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules == null)
            {
                v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules = new Dictionary<string, PartModule>();
            }
            return v.LCARS().lODN.ShipStatus.LCARS_VesselPartModules[modName].part;
        }

        public static Part LCARSVessel_ImpulseDrive_Part(this Vessel v)
        {
            return LCARSVessel_getPartWithModule(v, "LCARSMarkII");
        }

        public static Dictionary<string, PartResource> LCARSVessel_Resource_List(this Vessel v)
        {
            Dictionary<string, PartResource>  vesselPartResources = new Dictionary<string, PartResource>() { };
            foreach (Part p in v.parts)
            {
                PartResourceList pRL = p.Resources;
                foreach (PartResource pr in pRL)
                {
                    if (!vesselPartResources.ContainsKey(pr.resourceName))
                    {
                        vesselPartResources.Add(pr.resourceName, pr);
                    }
                }
            }
            return vesselPartResources;
        }

        public static ModuleGenerator.GeneratorResource LCARSVessel_EC_Generator(this Vessel v)
        {
            UnityEngine.Debug.Log("### LCARSMarkII LCARS_Vessel_Extender searching LCARS_VesselResourceGenerators");
            if (v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators == null)
            {
                v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators = new Dictionary<int, ModuleGenerator>();
                v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators = v.LCARSVessel_ResourceGenerators();
            }
            foreach (KeyValuePair<int, ModuleGenerator> pair in v.LCARS().lODN.ShipStatus.LCARS_VesselResourceGenerators)
            {
                List<ModuleGenerator.GeneratorResource> MG_GR = pair.Value.outputList;
                foreach (ModuleGenerator.GeneratorResource GR in MG_GR)
                {
                    if (GR.name == "ElectricCharge")
                    {
                        UnityEngine.Debug.Log("### LCARSMarkII LCARS_Vessel_Extender found LCARS_VesselResourceGenerators");
                        v.LCARS().lODN.ShipStatus.LCARSVessel_EC_Generator = GR;
                        return v.LCARS().lODN.ShipStatus.LCARSVessel_EC_Generator;
                    }
                }
            }
            UnityEngine.Debug.Log("### LCARSMarkII LCARS_Vessel_Extender creating LCARS_VesselResourceGenerators");
            //ModuleGenerator newMG = new ModuleGenerator();
            ConfigNode newMG = new ConfigNode("MODULE");
            newMG.AddValue("name", "ModuleGenerator");
            newMG.AddValue("isAlwaysActive", "true");
            ConfigNode OUTPUT_RESOURCE = new ConfigNode("OUTPUT_RESOURCE");
            OUTPUT_RESOURCE.AddValue("name", "ElectricCharge");
            OUTPUT_RESOURCE.AddValue("rate", v.LCARS().lODN.ShipStatus.LCARSVessel_EC_Generator_default_rate);
            newMG.AddNode(OUTPUT_RESOURCE);
            v.rootPart.AddModule(newMG);
            return v.LCARSVessel_EC_Generator();
        }
/*
*/
        public static float LCARSVessel_WetMass(this Vessel v)
        { return v.GetTotalMass(); }

        public static float LCARSVessel_DryMass(this Vessel v)
        { return v.LCARSVessel_WetMass() - v.LCARSVessel_ResourceMass(); }

        public static float LCARSVessel_ResourceMass(this Vessel v)
        { 
            float rm = 0f;
            foreach (Part p in v.Parts)
            {
                rm += p.GetResourceMass();

            }      
            return rm;
        }
        
        /*



        public static Dictionary<string, PartResource> LCARS_VesselPartResources(this Vessel v)
        { return v.Parts; }
        */

        public static bool LCARSVessel_hasPower(this Vessel v)
        {
            foreach (Part p in v.Parts)
            {
                PartResourceList pRL = p.Resources;
                foreach (PartResource pr in pRL)
                {
                    if (pRL.Contains("ElectricCharge"))
                    {
                        return (pr.amount > 0);
                    }
                }
            }
            return false;
        }
    }


}






















