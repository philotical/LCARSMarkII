
using System;
using System.Collections.Generic;

namespace LCARSMarkII
{



    public class LCARS_CrewQuartier
    {

        Vessel thisVessel = null;
        LCARSMarkII LCARS;
        internal void setVessel(Vessel this_Vessel)
        {
            thisVessel = this_Vessel;
            LCARS = thisVessel.LCARS();
            //this.part.AddModule("STCrewQuarters");
            //this.part.CreateInternalModel(PartLoader.GetInternalPart("mk1PodCockpit"));
            
            //UnityEngine.Debug.Log("LCARS_CrewQuartier: setVessel  end");

        }

        public void KerbalCheckIn()
        {
            LCARS.CrewQuartiers_UsedSpace++;
        }

        public void KerbalCheckOut()
        {
            LCARS.CrewQuartiers_UsedSpace--;
        }

        public int getFreeCrewSpace()
        {
            while ((thisVessel.GetCrewCount() + (LCARS.CrewQuartiers_MaxCrewSpace - LCARS.CrewQuartiers_UsedSpace)) > thisVessel.rootPart.CrewCapacity)
            {
                KerbalCheckIn();
            }
            while ((thisVessel.GetCrewCount() + (LCARS.CrewQuartiers_MaxCrewSpace - LCARS.CrewQuartiers_UsedSpace)) < thisVessel.rootPart.CrewCapacity)
            {
                KerbalCheckOut();
            }
            return LCARS.CrewQuartiers_MaxCrewSpace - LCARS.CrewQuartiers_UsedSpace;
        }

        public int getCrewQuartersTotal()
        {
            return LCARS.CrewQuartiers_MaxCrewSpace;
        }

        public int getCrewQuartersUsed()
        {
            return LCARS.CrewQuartiers_UsedSpace;
        }
        public void addHatch()
        {
            if (thisVessel.rootPart.airlock == null)
            {
                UnityEngine.Debug.Log("LCARS_CrewQuartier: addHatch  added missing hatch "); 
                thisVessel.rootPart.airlock = FlightGlobals.ActiveVessel.transform;
            }
        }
        public void addCrewSpace()
        {
            thisVessel.rootPart.CrewCapacity += LCARS.CrewQuartiers_MaxCrewSpace;

            if (LCARS.CrewQuartiers_SeatGameObject_List != null && LCARS.CrewQuartiers_SeatGameObject_List != "")
            {
                /*
                InternalModule IModule = null;
                InternalModel IModel = null;
                List<InternalSeat> seatList = null;
                try
                {

                    IModule = thisVessel.gameObject.GetComponent<InternalModule>();
                    //thisVessel.rootPart.CreateInternalModel(PartLoader.GetInternalPart("mk1PodCockpit")

                }
                catch (Exception ex) { UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with InternalModule failed ex=" + ex); }

                try
                {
                    //IModel = IModule.internalModel; // failed
                    IModel = IModule.GetComponent<InternalModel>();
                }
                catch (Exception ex) { UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with IModule.internalModel failed ex=" + ex); }

                try
                {
                    seatList = IModel.seats;
                }
                catch (Exception ex) { UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with IModel.seats failed ex=" + ex); }
                */


                try
                {


                    string[] seat_proposals = LCARS.CrewQuartiers_SeatGameObject_List.Split(new Char[] { ',' });
                    UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with InternalSeat seat_proposals.Length=" + seat_proposals.Length + " thisVessel.rootPart.internalModel.seats.Count=" + thisVessel.rootPart.internalModel.seats.Count);
                    foreach (string s in seat_proposals)
                    {
                        UnityEngine.Debug.Log("loop " + s + " thisVessel.rootPart.internalModel.seats.Count=" + thisVessel.rootPart.internalModel.seats.Count);
                        ConfigNode nSeat = new ConfigNode("MODULE");
                        nSeat.AddValue("name", "InternalSeat");
                        nSeat.AddValue("seatTransformName", s);
                        nSeat.AddValue("allowCrewHelmet", "false");
                        thisVessel.rootPart.internalModel.internalConfig.AddNode(nSeat);

                        InternalSeat tmp = new InternalSeat();
                        tmp.seatTransformName = s;
                        tmp.seatTransform = thisVessel.rootPart.internalModel.FindModelTransform(s);
                        //tmp.seatTransform = IModel.transform.Find(s);
                        tmp.allowCrewHelmet = false;
                        //seatList.Add(tmp);
                        thisVessel.rootPart.internalModel.seats.Add(tmp);

                    }
                    thisVessel.rootPart.CreateInternalModel();

                    /*
                      MODULE
                      {
                        name = InternalSeat
                            seatTransformName = PilotSeat
                        //portraitCameraName = KerbalCam1
                        allowCrewHelmet = false
                      }
                    */

                    /*
                    thisVessel.rootPart.internalModel.CrewCapacity;
                    thisVessel.rootPart.internalModel.internalName;
                    thisVessel.rootPart.internalModel.
                    */
                    /*
                    string[] seat_proposals = LCARS.CrewQuartiers_SeatGameObject_List.Split(new Char[] { ',' });
                    UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with InternalSeat seat_proposals.Length=" + seat_proposals.Length + " thisVessel.rootPart.internalModel.seats.Count=" + thisVessel.rootPart.internalModel.seats.Count);
                    foreach (string s in seat_proposals)
                    {
                        UnityEngine.Debug.Log("loop " + s + " thisVessel.rootPart.internalModel.seats.Count=" + thisVessel.rootPart.internalModel.seats.Count);
                        InternalSeat tmp = new InternalSeat();
                        tmp.seatTransformName = s;
                        tmp.seatTransform = thisVessel.rootPart.internalModel.FindModelTransform(s);
                        //tmp.seatTransform = IModel.transform.Find(s);
                        tmp.allowCrewHelmet = false;
                        //seatList.Add(tmp);
                        thisVessel.rootPart.internalModel.seats.Add(tmp);
                    }
                    UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  DONE seat_proposals.Length=" + seat_proposals.Length + " thisVessel.rootPart.internalModel.seats.Count=" + thisVessel.rootPart.internalModel.seats.Count);
                */
                }
                catch (Exception ex) { UnityEngine.Debug.Log("LCARS_CrewQuartier: addCrewSpace  messing with InternalSeat failed ex=" + ex); }

            }

        }
        /*
        internal void addCrewSpace()
        {
            UnityEngine.Debug.Log("StarTrekCrewQuartier: addCrewSpace  begin CrewCapacity=" + thisVessel.rootPart.CrewCapacity);
            LCARS.CrewQuartiers_MaxCrewSpace = calculateCrewSpace();
            int CrewCountTotal = thisVessel.GetCrewCount();
            thisVessel.rootPart.CrewCapacity += CrewQuartersTotal;

            /*
            INTERNAL
            {
                name = crewCabinInternals
            }
            INTERNAL
            {
                name = crewCabinInternals

                MODULE
                {
                name = InternalSeat
                seatTransformName = Seat1
                allowCrewHelmet = false
                }
                MODULE
                {
                name = InternalSeat
                seatTransformName = Seat2
                allowCrewHelmet = false
                }
                MODULE
                {
                name = InternalSeat
                seatTransformName = Seat3
                allowCrewHelmet = false
                }
                MODULE
                {
                name = InternalSeat
                seatTransformName = Seat4
                allowCrewHelmet = false
                }
                MODULE 
                {
                name = InternalCameraSwitch
                colliderTransformName = Window1FocusPoint
                cameraTransformName = Window1EyeTransform
                }
                MODULE 
                {
                name = InternalCameraSwitch
                colliderTransformName = Window2FocusPoint
                cameraTransformName = Window2EyeTransform
                }


            }   
             * /
            this.CrewCapacityTotal = thisVessel.rootPart.CrewCapacity;

            //Part.AddInternalPart(ConfigNode)
            //Part.CreateInternalModel()

            //InternalModel foo = PartLoader.GetInternalPart("GenericSpace1");
            //thisVessel.rootPart.
            // Part.internalModel
            //Part.InternalModelName
            // ProtoPart.newPart
            UnityEngine.Debug.Log("StarTrekCrewQuartier: addCrewSpace  done  CrewCapacity=" + thisVessel.rootPart.CrewCapacity);


        }

        internal int calculateCrewSpace()
        {

            return (int)Math.Round((thisVessel.LCARSVessel_DryMass() / 10), 0);
            //return (int)Math.Round((FlightGlobals.ActiveVessel.GetTotalMass() / 10), 0);
        }
*/
    }
}
