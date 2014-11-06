using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public static class LCARS_Utilities
    {

        /*
        http://forum.kerbalspaceprogram.com/threads/94011-Unity-animation-Play-Problem?p=1422965&viewfull=1#post1422965
        public delegate void GameObjectVisitor(GameObject go, int indent);
        public static class DebugExtensions
        {
            private static void internal_PrintComponents(GameObject go, int indent)
            {
                Log.Debug("{0}{1} has components:", indent > 0 ? new string('-', indent) + ">" : "", go.name);

                var components = go.GetComponents<Component>();
                foreach (var c in components)
                    Log.Debug("{0}: {1}", new string('.', indent + 3) + "c", c.GetType().FullName);
            }
            public static void PrintComponents(this UnityEngine.GameObject go)
            {
                go.TraverseHierarchy(internal_PrintComponents);
            }
            public static void TraverseHierarchy(this UnityEngine.GameObject go, GameObjectVisitor visitor, int indent = 0)
            {
                visitor(go, indent);

                for (int i = 0; i < go.transform.childCount; ++i)
                    go.transform.GetChild(i).gameObject.TraverseHierarchy(visitor, indent + 3);
            }
        }
        */

        public static int ToInt(string s)
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools ToInt");
            int number;
            bool result = Int32.TryParse(s, out number);
            if (result)
            {
                //Console.WriteLine("Converted '{0}' to {1}.", s, number);
                return number;
            }
            else
            {
                //if (s == null) s = "";
                //Console.WriteLine("Attempted conversion of '{0}' failed.", s);
                return 0;
            }
        }
        public static float ToFloat(string s)
        {
            //UnityEngine.Debug.Log("### LCARS_NCI_Mission_Archive_Tools ToFloat");
            float number;
            //bool result = float.TryParse(s, out number);
            bool result = float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out number);
            if (result)
            {
                //Console.WriteLine("Converted '{0}' to {1}.", s, number);
                return number;
            }
            else
            {
                //if (s == null) s = "";
                //Console.WriteLine("Attempted conversion of '{0}' failed.", s);
                return 0;
            }
        }

        public static Dictionary<string, string> VesselTypeRecognizer_list = null;
        public static Dictionary<string, string> init_VesselTypeRecognizer()
        {
            if (VesselTypeRecognizer_list == null)
            {
                VesselTypeRecognizer_list = new Dictionary<string, string>() { };
                VesselTypeRecognizer_list.Add("FASAAsteroid", "It seems to be an Asteroid");
                VesselTypeRecognizer_list.Add("PotatoRoid", "It seems to be an Asteroid");
                VesselTypeRecognizer_list.Add("voy", "It seems to be a Intrepid Class Starship");
                VesselTypeRecognizer_list.Add("shuttle1", "It seems to be a Type 3 Shuttle");
                VesselTypeRecognizer_list.Add("shuttle2", "It seems to be a Runabout Class Starship");
                VesselTypeRecognizer_list.Add("base", "It seems to be a Space Dock");
                VesselTypeRecognizer_list.Add("enterprisescale", "It seems to be a  Constitution Class Starship");
                VesselTypeRecognizer_list.Add("CardassianStation", "It seems to be a Cardassian Station");
                VesselTypeRecognizer_list.Add("borg", "It is Borg!");
                VesselTypeRecognizer_list.Add("Borg", "It is Borg!");
                VesselTypeRecognizer_list.Add("tng_saucer2", "It seems to be a Constellation Class Starship");
                VesselTypeRecognizer_list.Add("tng_saucer", "It seems to be a Galaxy Class Starship");
                VesselTypeRecognizer_list.Add("tardis", "It seems to be a phone both");
                VesselTypeRecognizer_list.Add("car", "It seems to be a flying car - maybe a Ford");
                VesselTypeRecognizer_list.Add("phoenix", "It seems to be an ancient warp ship");
                VesselTypeRecognizer_list.Add("CustomShuttlePod", "It seems to be a civilian ship of the Merchant Fleet");
                VesselTypeRecognizer_list.Add("daedalus", "It seems to be a civilian ship of the Merchant Fleet");
                VesselTypeRecognizer_list.Add("IonStorm", "It's a large space disturbance, it emmits notable gravity wells in an irratic pattern");
                VesselTypeRecognizer_list.Add("Badlands", "It's a very large space disturbance, it emmits huge gravity wells in an irratic pattern");
                VesselTypeRecognizer_list.Add("Belerophon", "Rather Large Colonization Transport, Model CX-101, it has an expired transponder signal of the Merchant Fleet");
                VesselTypeRecognizer_list.Add("CustomShuttlePod", "A small cargo shuttle, it has an expired transponder signal of the Merchant Fleet");
                VesselTypeRecognizer_list.Add("RLCT01", "It's a huge cargo vessel, they don't build these anymore. It has an expired transponder signal of the Merchant Fleet");
                VesselTypeRecognizer_list.Add("SCAurora", "A medium sized space cruiser, seems to be Star Fleet");
                VesselTypeRecognizer_list.Add("DefenderOne", "A satelite - some times, these are armed.");
                VesselTypeRecognizer_list.Add("ARBlackHole", "it's a black hole - why is there no warning sign!");
                VesselTypeRecognizer_list.Add("ShermansDepot", "It's a station in the Sherman Design - they have walkable docking ports");
                VesselTypeRecognizer_list.Add("Sphere39", "It's.. it's  - I don't know, it's composed of unknown alloys, it seems to have an entrance though.");
            }
            return VesselTypeRecognizer_list;
        }
        public static string VesselTypeRecognizer(string part_name)
        {
                return VesselTypeRecognizer_list[part_name];
        }


        public static Dictionary<string, string> PlanetaryLocationsRecognizer_list = null;
        public static Dictionary<string, string> init_PlanetaryLocationsRecognizer()
        {
            if (PlanetaryLocationsRecognizer_list == null)
            {


                PlanetaryLocationsRecognizer_list = new Dictionary<string, string>() { };
                PlanetaryLocationsRecognizer_list.Add("Fuel Pump", "");
                PlanetaryLocationsRecognizer_list.Add("Fuel Tank", "");
                PlanetaryLocationsRecognizer_list.Add("Walkway", "");
                PlanetaryLocationsRecognizer_list.Add("Launch Pad", "");
                PlanetaryLocationsRecognizer_list.Add("Fuel Port", "");
                PlanetaryLocationsRecognizer_list.Add("Oxygen Pipe", "");
                PlanetaryLocationsRecognizer_list.Add("Fuel Pipe", "");
                PlanetaryLocationsRecognizer_list.Add("Water Pipe", "");
                PlanetaryLocationsRecognizer_list.Add("Oxygen Tank", "");
                PlanetaryLocationsRecognizer_list.Add("Water Tank", "");
                PlanetaryLocationsRecognizer_list.Add("Water Tower", "");
                PlanetaryLocationsRecognizer_list.Add("Satellite Dish", "");

                PlanetaryLocationsRecognizer_list.Add("The Great Pyramid", "");
                PlanetaryLocationsRecognizer_list.Add("Slightly Less Great Pyramid", "");
                PlanetaryLocationsRecognizer_list.Add("Stacked Bricks", "");
                PlanetaryLocationsRecognizer_list.Add("A Ziggurat Thing", "");
                PlanetaryLocationsRecognizer_list.Add("Wall", "");
                PlanetaryLocationsRecognizer_list.Add("UFO", "UFO");


                PlanetaryLocationsRecognizer_list.Add("IslandAirfield", "IslandAirfield");
                PlanetaryLocationsRecognizer_list.Add("statue", "");
                PlanetaryLocationsRecognizer_list.Add("rubies", "");
                PlanetaryLocationsRecognizer_list.Add("Tut-Un Jeb-Ahn", "");
                PlanetaryLocationsRecognizer_list.Add("Tomb of the lost Kerbal", "");
                PlanetaryLocationsRecognizer_list.Add("Priceless Artifact You should not crash into", "");
                PlanetaryLocationsRecognizer_list.Add("Pyramids", "Pyramids");



                PlanetaryLocationsRecognizer_list.Add("RockArch01", "");
                PlanetaryLocationsRecognizer_list.Add("RockArch00", "");
                PlanetaryLocationsRecognizer_list.Add("RockArch02", "");

                PlanetaryLocationsRecognizer_list.Add("Monolith00", "It's the rising of the apes!");
                PlanetaryLocationsRecognizer_list.Add("Monolith01", "It's the rising of the apes!");
                PlanetaryLocationsRecognizer_list.Add("Monolith02", "It's the rising of the apes!");
                PlanetaryLocationsRecognizer_list.Add("Monolith03", "It's the rising of the apes!");

                PlanetaryLocationsRecognizer_list.Add("Vehicle Assembly Building", "");
                PlanetaryLocationsRecognizer_list.Add("Tracking Station", "");
                PlanetaryLocationsRecognizer_list.Add("tuberia", "");
                PlanetaryLocationsRecognizer_list.Add("Launchpad Tower", "");
                PlanetaryLocationsRecognizer_list.Add("Coolant Tanks", "");
                PlanetaryLocationsRecognizer_list.Add("Coolant Tank", "");
                PlanetaryLocationsRecognizer_list.Add("PlatformPlane", "");
                PlanetaryLocationsRecognizer_list.Add("Launchpad Platform", "");
                PlanetaryLocationsRecognizer_list.Add("launchpad", "");
                PlanetaryLocationsRecognizer_list.Add("KSC2", "It's the Island Runway.. - good 'ol times eh?");
                PlanetaryLocationsRecognizer_list.Add("SpaceCenterMarker", "");
                PlanetaryLocationsRecognizer_list.Add("Runway_spawn", "");
                PlanetaryLocationsRecognizer_list.Add("memorialpod", "");
                PlanetaryLocationsRecognizer_list.Add("memorialbase", "");
                PlanetaryLocationsRecognizer_list.Add("Pod Memorial", "");
                PlanetaryLocationsRecognizer_list.Add("LaunchPad_spawn", "");

                PlanetaryLocationsRecognizer_list.Add("KSCVehicleAssemblyBuilding", "");
                PlanetaryLocationsRecognizer_list.Add("KSCTrackingStation", "");
                PlanetaryLocationsRecognizer_list.Add("KSCSpacePlaneHangar", "");
                PlanetaryLocationsRecognizer_list.Add("KSCRunway", "");
                PlanetaryLocationsRecognizer_list.Add("KSCRnDFacility", "");
                PlanetaryLocationsRecognizer_list.Add("KSCMissionControl", "");
                PlanetaryLocationsRecognizer_list.Add("KSCLaunchPad", "");
                PlanetaryLocationsRecognizer_list.Add("KSCFlagPoleLaunchPad", "");
                PlanetaryLocationsRecognizer_list.Add("KSCDecorativeFillerHolder", "");
                PlanetaryLocationsRecognizer_list.Add("KSCCrewBuilding", "");
                PlanetaryLocationsRecognizer_list.Add("KSC", "It's the Space Center");
            
            }
            return PlanetaryLocationsRecognizer_list;
        }
        public static string PlanetaryLocationsRecognizer(string go_name)
        {
            return PlanetaryLocationsRecognizer_list[go_name];
        }





        /// <summary>
        /// Takes a Charge and a Vessel as argument and pulls that amount of energy from the provided vessel
        /// </summary>
        public static float total_force = 0;
        public static float charge = 0;
        public static bool usePower(float eCharge, Vessel thisVessel)
        {
            foreach (Part part in thisVessel.parts)
            {
                if (eCharge <= 0)
                {
                    continue;
                }
                eCharge = eCharge - part.RequestResource("ElectricCharge", eCharge);
            }
            return false;
        }

        /// <summary>
        /// Takes a Vessel as argument and tryes to calculate the power consumption at current accelleration
        /// will call usePower(float eCharge, Vessel thisVessel)
        /// </summary>
        public static Dictionary<string, float> CalculatePowerConsumption(Dictionary<string, float> Powerstats, Vessel thisVessel, bool gravityEnabled, bool UseFullImpulse, bool UseReservePower, float UseFullImpulse_multiplier, float UseReservePower_multiplier, float vSliderValue, float hSliderValue, float zSliderValue)
        {
            /*
            Powerstats.Add("charge", 0);
            Powerstats.Add("total_force", 0);
            Powerstats.Add("force_x", 0);
            Powerstats.Add("force_y", 0);
            Powerstats.Add("force_z", 0);
            */
            if (!gravityEnabled)
            {
                return Powerstats;
            }
            charge = 200;
            float x = (vSliderValue < 0) ? vSliderValue * -1 : vSliderValue;
            float y = (hSliderValue < 0) ? hSliderValue * -1 : hSliderValue;
            float z = (zSliderValue < 0) ? zSliderValue * -1 : zSliderValue;
            Powerstats["force_x"] = x;
            Powerstats["force_y"] = y;
            Powerstats["force_z"] = z;

            total_force = x + y + z;
            total_force = (UseFullImpulse) ? total_force * UseFullImpulse_multiplier : total_force;
            total_force = (UseReservePower) ? total_force * UseReservePower_multiplier : total_force;

            charge += total_force * 12;
            charge = (UseFullImpulse) ? charge * UseFullImpulse_multiplier : charge;
            charge = (UseReservePower) ? charge * UseReservePower_multiplier : charge;

            if (charge > 0)
            {
                LCARS_Utilities.usePower(charge, thisVessel);
            }
            Powerstats["charge"] = charge;
            Powerstats["total_force"] = total_force;

            return Powerstats;
        }
        public static void AdditionalPowerConsumption(Vessel thisVessel, float vSliderValue, float hSliderValue, float zSliderValue)
        {
            charge = 200;
            float x = (vSliderValue < 0) ? vSliderValue * -1 : vSliderValue;
            float y = (hSliderValue < 0) ? hSliderValue * -1 : hSliderValue;
            float z = (zSliderValue < 0) ? zSliderValue * -1 : zSliderValue;
            total_force = x + y + z;
            charge += total_force * 12;
            if (charge > 0)
            {
                LCARS_Utilities.usePower(charge, thisVessel);
            }
        }

        public static Rect ClampToScreen(Rect r)
        {
            //r.x = Mathf.Clamp(r.x, 0, Screen.width - r.width);
            //r.y = Mathf.Clamp(r.y, 0, Screen.height - r.height);
            r.x = Mathf.Clamp(r.x, 0 + (r.x / 2), Screen.width - (r.width / 2));
            r.y = Mathf.Clamp(r.y, 0, Screen.height - 50);
            return r;
        }

        static public void SetLoadDistance(float loadDistance = 2500, float unloadDistance = 2250)
        {
            Vessel.loadDistance = loadDistance;
            Vessel.unloadDistance = unloadDistance;
        }

        static public string AlphaCharlyTango(string prefix,string input,int length=0)
        {
            length = (length > 0) ? length : input.Count();
			//pid = 92b38429ef9548439ec82d1f6ceccaf9
            //pid = 67cab79924cb484d94f99aed0914a036
            Dictionary<string,string> ACT = new Dictionary<string,string>(){};
            ACT.Add(" ", "Space");
            ACT.Add("-", "Dash");
            ACT.Add("A", "Alpha");
            ACT.Add("B","Bravo");
            ACT.Add("C","Charlie");
            ACT.Add("D","Delta");
            ACT.Add("E","Echo");
            ACT.Add("F","Foxtrot");
            ACT.Add("G","Golf");
            ACT.Add("H","Hotel");
            ACT.Add("I","India");
            ACT.Add("J","Juliet");
            ACT.Add("K","Kilo");
            ACT.Add("L","Lima");
            ACT.Add("M","Mike");
            ACT.Add("N","November");
            ACT.Add("O","Oscar");
            ACT.Add("P","Papa");
            ACT.Add("Q","Quebec");
            ACT.Add("R","Romeo");
            ACT.Add("S","Sierra");
            ACT.Add("T","Tango");
            ACT.Add("U","Uniform");
            ACT.Add("V","Victor");
            ACT.Add("W","Whiskey");
            ACT.Add("X","X-ray");
            ACT.Add("Y","Yankee");
            ACT.Add("Z","Zulu");
            ACT.Add("0","Zero");
            ACT.Add("1","One");
            ACT.Add("2","Two");
            ACT.Add("3","Three");
            ACT.Add("4","Four");
            ACT.Add("5","Five");
            ACT.Add("6","Six");
            ACT.Add("7","Seven");
            ACT.Add("8","Eight");
            ACT.Add("9","Nine");

            input = input.ToUpper();
            string output = "";
            string last_output=null;
            for (int i = 0; i < length; i++)
            {
                string c = input[i].ToString();
                //Console.WriteLine(input[i]);
                if (last_output==null)
                {
                    output = prefix + "-" + ACT[c];
                }
                else
                {
                    output = last_output + "-" + ACT[c];
                }

                last_output = output;
                output = "";
            }
            output = null;
            output = last_output;
            last_output = null;

            return output;

        }





    }
}
