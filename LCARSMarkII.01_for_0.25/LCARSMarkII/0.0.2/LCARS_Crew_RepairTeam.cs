using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{

    public class LCARS_RepairTeam_Type
    {

        public string CO { get; set; }

        public string id { get; set; }

        public float efficiency { get; set; }

        public float powerusage { get; set; }

        public string AssignedSystem { get; set; }

        public Dictionary<string, float> WorkList { get; set; }

        public float TotalDamageRepaired { get; set; }

        public string TeamName { get; set; }

        public string StarTrekReference { get; set; }

        public string Description { get; set; }

        public bool isBusy
        {
            get
            {
                return (AssignedSystem != "none");
            }
        }

        public void getBusy(string damaged_system)
        {
            if (AssignedSystem=="none")
            {
            }
                AssignedSystem = damaged_system;
        }

        public LCARSMarkII LCARS { get; set; }
        public void doRepairs()
        {
            if (!isBusy)
            {return;}

            if (!LCARS.lODN.ShipSystems[TeamName].isNominal)
            {
                return;
            }
            LCARS.lPowSys.draw(TeamName, powerusage);

            float repairAmount = 0.01f;
            repairAmount *= (efficiency/100);

            if (LCARS.lODN.ShipSystems[AssignedSystem].integrity < 100f)
            {
                LCARS.lODN.ShipSystems[AssignedSystem].integrity += repairAmount;
                TotalDamageRepaired += repairAmount;
                if (!WorkList.ContainsKey(AssignedSystem))
                { WorkList.Add(AssignedSystem, 0f); }
                WorkList[AssignedSystem] += repairAmount;
                LCARS.lODN.ShipSystems[AssignedSystem].integrity = (LCARS.lODN.ShipSystems[AssignedSystem].integrity > 100) ? 100 : LCARS.lODN.ShipSystems[AssignedSystem].integrity;
            }
            if (LCARS.lODN.ShipSystems[AssignedSystem].integrity < 100f)
            {
                //LCARS.lODN.ShipSystems[AssignedSystem].damaged = true;
            }
            else 
            {
                if (LCARS.lRepairTeams.lastRepairedSystem != AssignedSystem)
                {
                    LCARS_Message_Type mT = new LCARS_Message_Type();
                    mT.receiver_vessel = LCARS.vessel;
                    mT.sender = TeamName;
                    mT.sender_id_line = TeamName + " reports:";
                    mT.receiver_type = "Station";
                    mT.receiver_code = "Bridge";
                    mT.priority = 3;
                    mT.setTitle("System Back online");
                    mT.setMessage("We have repaired the " + AssignedSystem + "! It should be back to operational now!");
                    mT.Queue();
                }
                LCARS.lRepairTeams.lastRepairedSystem = AssignedSystem;
                LCARS.lODN.ShipSystems[AssignedSystem].damaged = false;
                AssignedSystem = "none";
                LCARS.lODN.ShipSystems[TeamName].powerSystem_consumption_current = 0f;

            }

        }


        public string Quote { get; set; }
    }


    public class LCARS_RepairTeam_Crew
    {
        public Dictionary<int, LCARS_RepairTeam_Type> RepairTeams;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_RepairTeam_Crew  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_RepairTeam_Crew  SetVessel done ");

            if (RepairTeams==null)
            {
                RepairTeams = new Dictionary<int, LCARS_RepairTeam_Type>();

                Dictionary<string, float> WorkList1 = new Dictionary<string, float>();

                LCARS_RepairTeam_Type Rockney = new LCARS_RepairTeam_Type();
                Rockney.CO = "Lieutenant Commander Arvin Rockney";
                Rockney.id = "Rockney";
                Rockney.TeamName = "RepairTeam_Rockney";
                Rockney.Description = "Rockney doesn’t have a personal life. He’s more comfortable with engines than people, to the point where he has better conversations with them. Rockney mumbles constantly to the ship, the impulse engines, the manifolds. He argues philosophy with the ODN relays and tells dirty jokes to the Bussard collectors. The local legend is that he once talked the warp core out of breaching.";
                Rockney.Quote = "She said her port nacelle hurts and that last overload gave her a headache! She needs to rest in a StarBase Captain";
                Rockney.StarTrekReference = "Hidden Frontier: Helena Chronicals, USS Helena NCC-80455";
                Rockney.efficiency = 120f;
                Rockney.powerusage = 100f;
                Rockney.AssignedSystem = "none";
                Rockney.TotalDamageRepaired = 0f;
                Rockney.WorkList = WorkList1;
                Rockney.LCARS = LCARS;
                RepairTeams.Add(1, Rockney);
                //LCARS.lPowSys.setPowerTaker(Rockney.TeamName, "MainSystem", "Engineering", 100, 0, 0);

                Dictionary<string, float> WorkList2 = new Dictionary<string, float>();
                LCARS_RepairTeam_Type Scott = new LCARS_RepairTeam_Type();
                Scott.CO = "Lieutenant Commander Montgommery Scott";
                Scott.id = "Scott";
                Scott.TeamName = "RepairTeam_Scott";
                Scott.Description = "Montgomery Scott (referred to as Scotty by his shipmates), serial number SE 19754 T, was the chief engineer of both the USS Enterprise and the USS Enterprise-A for a period of nearly thirty years. Having the reputation as a miracle worker, he is a man of superior technical and engineering skill, experience and ingenuity. Despite his superior talents as an Engineer, he is often the source of comic relief amongst the crew. ";
                Scott.Quote = "She can't handle any more of this Capt'in - she will just blow up!";
                Scott.StarTrekReference = "TOS, USS Enterprise NCC 1701";
                Scott.efficiency = 100f;
                Scott.powerusage = 100f;
                Scott.AssignedSystem = "none";
                Scott.TotalDamageRepaired = 0f;
                Scott.WorkList = WorkList2;
                Scott.LCARS = LCARS;
                RepairTeams.Add(2, Scott);
                //LCARS.lPowSys.setPowerTaker(Scott.TeamName, "MainSystem", "Engineering", 100, 0, 0);

                Dictionary<string, float> WorkList3 = new Dictionary<string, float>();
                LCARS_RepairTeam_Type LaForge = new LCARS_RepairTeam_Type();
                LaForge.CO = "Lieutenant Commander Geordi La Forge";
                LaForge.id = "La Forge";
                LaForge.TeamName = "RepairTeam_LaForge";
                LaForge.Description = "La Forge was assigned to pilot Jean-Luc Picard on an inspection tour. En route to their destination, Picard made an off-hand remark about the shuttle's engine efficiency not being what it should. In response to this, La Forge stayed up all night refitting the shuttle's fusion initiators.";
                LaForge.Quote = "We could try to reroute the secondary hull relay input stabilizer coupling matrix to the main deflector dish. That should make your tea hot Sir!";
                LaForge.StarTrekReference = "TNG, USS Enterprise NCC 1701-D";
                LaForge.efficiency = 80f;
                LaForge.powerusage = 100f;
                LaForge.AssignedSystem = "none";
                LaForge.TotalDamageRepaired = 0f;
                LaForge.WorkList = WorkList3;
                LaForge.LCARS = LCARS;
                RepairTeams.Add(3, LaForge);
                //LCARS.lPowSys.setPowerTaker(LaForge.TeamName, "MainSystem", "Engineering", 100, 0, 0);
            }

        }

        public bool isBusy(string damaged_system) // runs in fixed update in main class
        {
            foreach (KeyValuePair<int, LCARS_RepairTeam_Type> pair in RepairTeams)
            {
                int key = pair.Key;
                LCARS_RepairTeam_Type team = pair.Value;
                if (team.AssignedSystem == damaged_system)
                {
                    return true;
                }
            }
            return false;

        }
 

        public void AssignToWork(string damaged_system) 
        {
            foreach (KeyValuePair<int, LCARS_RepairTeam_Type> pair in RepairTeams)
            {
                int key = pair.Key;
                LCARS_RepairTeam_Type team = pair.Value;

                team.getBusy(damaged_system);

            }
        }

        public string lastRepairedSystem = "";
        public void GetToWork() // runs in fixed update in main class
        {
        
            foreach(KeyValuePair<int,LCARS_RepairTeam_Type> pair in RepairTeams)
            {
                int key = pair.Key;
                LCARS_RepairTeam_Type team = pair.Value;

                team.doRepairs();

            }

        }

    }
}
