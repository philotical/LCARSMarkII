
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCARSMarkII
{

    public class LCARS_Subsystem_DamageControll : ILCARSPlugin
    {
        public string subsystemName { get { return "Damage Controll System"; } }
        public string subsystemDescription { get { return "Asses and repair ship damage"; } }
        public string subsystemStation { get { return "Engineering"; } } // in which station is this displayed
        public float subsystemPowerLevel_standby { get { return 10f; } } // Power draw if activated but idle
        public float subsystemPowerLevel_running { get { return 0f; } } // Power draw if activated and working - is added to standby
        public float subsystemPowerLevel_additional { get { return 0f; } } // Power draw for additional consumtion - is added to running
        public bool subsystemIsPartModule { get { return false; } } // does this subsystem require a part module in cfg file
        public bool subsystemIsDamagable { get { return false; } }
        public bool subsystemPanelState { get; set; } // has to be false at start

        private GUIStyle scrollview_style;
        private Vector2 DamageControll_ScrollPosition1;
        private Vector2 DamageControll_ScrollPosition2;
        private bool toggle_1 = false;
        private bool toggle_2 = false;
        private bool toggle_3 = false;
        private bool toggle_4 = false;
        private GUIStyle toggle_style = null;
        int selGridIntMain=3;

        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_Subsystem_DamageControll  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_Subsystem_DamageControll  SetVessel done ");
        }

        public void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "")
        {

        }

        public void getGUI()
        {

            if (scrollview_style == null)
            {
                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 200;
            }
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                toggle_style.alignment = TextAnchor.MiddleCenter;
                toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(0, 0, 0, 0);
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
                toggle_style.fixedWidth = 74;
                toggle_style.fixedHeight = 20;
            }

            //UnityEngine.Debug.Log("LCARS_Subsystem_DamageControll  getGUI begin ");

            string[] selStringsMain = new string[] { "Team Rockney", "Team Scott", "Team La Forge", "Damage Controll" };
            selGridIntMain = GUILayout.SelectionGrid(selGridIntMain, selStringsMain, 3);

            switch (selGridIntMain)
            {
                case 0:
                case 1:
                case 2:
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Team " + LCARS.lRepairTeams.RepairTeams[selGridIntMain+1].id + ": ");
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    GUILayout.Label("CO: " + LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].CO);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Efficiency: " + LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].efficiency);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Total Damage Repaired: " + LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].TotalDamageRepaired);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Assigned System: " + LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].AssignedSystem);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    toggle_1 = GUILayout.Toggle(toggle_1,"WorkList");
                    if (toggle_1)
                    {
                        scrollview_style.fixedHeight = 100;
                        GUILayout.BeginVertical(scrollview_style);
                        DamageControll_ScrollPosition2 = GUILayout.BeginScrollView(DamageControll_ScrollPosition2);
                        foreach (KeyValuePair<string, float> pair in LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].WorkList)
                        {
                            GUILayout.Label(pair.Key + ": " + pair.Value);
                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    toggle_2 = GUILayout.Toggle(toggle_2, "Description");
                    if (toggle_2)
                    {
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].Description);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    toggle_3 = GUILayout.Toggle(toggle_3, "Quote");
                    if (toggle_3)
                    {
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].Quote);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    toggle_4 = GUILayout.Toggle(toggle_4, "StarTrekReference");
                    if (toggle_4)
                    {
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[selGridIntMain + 1].StarTrekReference);
                    }
                    GUILayout.EndHorizontal();


                    break;

                case 3:
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Damage Controll: ");
                    GUILayout.EndHorizontal();

                        if (!LCARS.lODN.ShipSystems[LCARS.lRepairTeams.RepairTeams[1].TeamName].isNominal)
                        {
                            GUILayout.Label(LCARS.lRepairTeams.RepairTeams[1].TeamName + " is not operational!");
                            LCARS.lRepairTeams.RepairTeams[1].getBusy("none");
                        }
                        if (!LCARS.lODN.ShipSystems[LCARS.lRepairTeams.RepairTeams[2].TeamName].isNominal)
                        {
                            GUILayout.Label(LCARS.lRepairTeams.RepairTeams[2].TeamName + " is not operational!");
                            LCARS.lRepairTeams.RepairTeams[2].getBusy("none");
                       }
                        if (!LCARS.lODN.ShipSystems[LCARS.lRepairTeams.RepairTeams[3].TeamName].isNominal)
                        {
                            GUILayout.Label(LCARS.lRepairTeams.RepairTeams[3].TeamName + " is not operational!");
                            LCARS.lRepairTeams.RepairTeams[3].getBusy("none");
                        }



                        GUILayout.BeginHorizontal();
                        GUILayout.Label("System", GUILayout.Width(440));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Integrity", GUILayout.Width(85));
                        GUILayout.Label("Status", GUILayout.Width(85));
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[1].id, GUILayout.Width(85));
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[2].id, GUILayout.Width(85));
                        GUILayout.Label(LCARS.lRepairTeams.RepairTeams[3].id, GUILayout.Width(85));
                        GUILayout.EndHorizontal();


                        scrollview_style.fixedHeight = 200;
                        GUILayout.BeginVertical(scrollview_style);
                        DamageControll_ScrollPosition1 = GUILayout.BeginScrollView(DamageControll_ScrollPosition1);

                        foreach (KeyValuePair<string, LCARS_ShipSystem_Type> pair in LCARS.lODN.ShipSystems)
                        {
                            if (pair.Value.integrity >= 99f)
                            { continue;}

                            string status = "Operational";
                            status = (pair.Value.isDisabled) ? "Disabled" : status;
                            status = (pair.Value.isDamaged) ? "Damaged" : status;

                                GUILayout.BeginHorizontal();
                                    GUILayout.Label(pair.Value.name+" - Status: "+status);
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();

                                GUILayout.Label(Math.Round(pair.Value.integrity) + "%", GUILayout.Width(85));

                                if (LCARS.lRepairTeams.isBusy(pair.Value.name))
                                {
                                    GUILayout.Label("Repairing", GUILayout.Width(85));
                                }
                                else
                                {
                                    GUILayout.Label("Awaiting", GUILayout.Width(85));
                                }



                                if (LCARS.lRepairTeams.RepairTeams[1].AssignedSystem == pair.Value.name)
                                {
                                    GUILayout.Label("working", GUILayout.Width(85));
                                }
                                else
                                {
                                    if (GUILayout.Button("Assign", GUILayout.Width(85)))
                                    {
                                        LCARS.lRepairTeams.RepairTeams[1].getBusy(pair.Value.name);
                                    }
                                }

                                if (LCARS.lRepairTeams.RepairTeams[2].AssignedSystem == pair.Value.name)
                                {
                                    GUILayout.Label("working", GUILayout.Width(85));
                                }
                                else
                                {
                                    if (GUILayout.Button("Assign", GUILayout.Width(85)))
                                    {
                                        LCARS.lRepairTeams.RepairTeams[2].getBusy(pair.Value.name);
                                    }
                                }

                                if (LCARS.lRepairTeams.RepairTeams[3].AssignedSystem == pair.Value.name)
                                {
                                    GUILayout.Label("working", GUILayout.Width(85));
                                }
                                else
                                {
                                    if (GUILayout.Button("Assign", GUILayout.Width(85)))
                                    {
                                        LCARS.lRepairTeams.RepairTeams[3].getBusy(pair.Value.name);
                                    }
                                }

                            GUILayout.EndHorizontal();



                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();


            
                    break;

            }


            //UnityEngine.Debug.Log("LCARS_Subsystem_DamageControll  getGUI done ");
        }

    }

}
