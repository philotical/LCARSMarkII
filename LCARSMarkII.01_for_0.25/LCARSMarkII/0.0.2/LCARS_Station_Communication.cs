using System;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_Communication_Station
    {
        private GUIStyle Communication_BackGroundLayoutStyle = null;
        private GUIStyle Communication_BackGroundLayoutStyle2 = null;
        private GUIStyle Communication_BackGroundLayoutStyle3 = null;
        private GUIStyle Communication_BackGroundLayoutStyle3b = null;
        private GUIStyle Communication_BackGroundLayoutStyle4 = null;
        private GUIStyle Communication_BackGroundLayoutStyle5 = null;
        private GUIStyle toggle_style = null;
        private GUIStyle caption_style = null;

        private GUIStyle scrollview_style;
        private Vector2 Communication_ScrollPosition1;

        private int Content_Mode = 1;
        private int Filter_Mode = 0;
      
        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
        }
        public void getGUI()
        {
            //UnityEngine.Debug.Log("### LCARS_Communication_Station getGUI begin ");
            if (Communication_BackGroundLayoutStyle == null)
            {
                Communication_BackGroundLayoutStyle = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle.margin = new RectOffset(0, 0, -11, 0);
                Communication_BackGroundLayoutStyle.padding = new RectOffset(12, 12, 64, 0);
                Communication_BackGroundLayoutStyle.fixedWidth = 459;
                Communication_BackGroundLayoutStyle.fixedHeight = 600;
                Communication_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Background", false);
                Communication_BackGroundLayoutStyle.onNormal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Background", false);
                Communication_BackGroundLayoutStyle.onHover.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Background", false);
                Communication_BackGroundLayoutStyle.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Background", false);

                Communication_BackGroundLayoutStyle2 = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle2.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle2.padding = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle2.margin = new RectOffset(0, 0, 4, 0);
                Communication_BackGroundLayoutStyle2.fixedHeight = 16;
                Communication_BackGroundLayoutStyle2.fixedWidth = 407;
                Communication_BackGroundLayoutStyle2.normal.textColor = Color.white;
                //Communication_BackGroundLayoutStyle2.fixedHeight = 443;

                Communication_BackGroundLayoutStyle3 = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle3.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle3.padding = new RectOffset(9, 5, 0, 0);
                Communication_BackGroundLayoutStyle3.margin = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle3.fixedWidth = 407;
                Communication_BackGroundLayoutStyle3.normal.textColor = Color.white;
                //Communication_BackGroundLayoutStyle3.fixedHeight = 443;
                
                Communication_BackGroundLayoutStyle3b = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle3b.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle3b.padding = new RectOffset(9, 5, 0, 0);
                Communication_BackGroundLayoutStyle3b.margin = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle3b.fixedWidth = 407;
                Communication_BackGroundLayoutStyle3b.normal.textColor = Color.white;
                //Communication_BackGroundLayoutStyle3b.fixedHeight = 443;

                Communication_BackGroundLayoutStyle4 = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle4.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle4.padding = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle4.margin = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle4.fixedWidth = 407;
                Communication_BackGroundLayoutStyle4.fixedHeight = 13;
                Communication_BackGroundLayoutStyle4.normal.textColor = Color.white;

                Communication_BackGroundLayoutStyle5 = new GUIStyle(GUI.skin.box);
                Communication_BackGroundLayoutStyle5.alignment = TextAnchor.UpperLeft;
                Communication_BackGroundLayoutStyle5.padding = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle5.margin = new RectOffset(0, 0, 0, 0);
                Communication_BackGroundLayoutStyle5.fixedWidth = 407;
                Communication_BackGroundLayoutStyle5.fixedHeight = 10;
                Communication_BackGroundLayoutStyle5.normal.textColor = Color.white;
                //Communication_BackGroundLayoutStyle5.fixedHeight = 443;
            }
            if (scrollview_style == null)
            {
                scrollview_style = new GUIStyle();
                scrollview_style.fixedHeight = 490;
                scrollview_style.fixedWidth = 430;
            }
            if (toggle_style == null)
            {
                toggle_style = new GUIStyle();
                toggle_style.alignment = TextAnchor.MiddleCenter;
                toggle_style.padding = new RectOffset(0, 0, 0, 0);
                toggle_style.margin = new RectOffset(0, 0, 0, 0);
                //toggle_style.imagePosition = ImagePosition.ImageOnly;
                toggle_style.normal.textColor = Color.black;
            }
            if (caption_style == null)
            {
                caption_style = new GUIStyle();
                //caption_style.alignment = TextAnchor.MiddleCenter;
                //caption_style.padding = new RectOffset(0, 0, 0, 0);
                caption_style.margin = new RectOffset(20, 0, -4, 0);
                caption_style.fontSize = 12;
                caption_style.fixedHeight = 14;
                //caption_style.imagePosition = ImagePosition.ImageOnly;
                caption_style.normal.textColor = Color.black;
                caption_style.active.textColor = Color.black;
                caption_style.fontStyle = FontStyle.Italic;
            }

            if (LCARS.lODN.CommunicationQueue == null)
            {
                LCARS.lODN.CommunicationQueue = new System.Collections.Generic.List<LCARS_CommunicationQueue_Type>();
            }


            //UnityEngine.Debug.Log("### LCARS_Communication_Station getGUI 1 ");

            GUILayout.BeginVertical(Communication_BackGroundLayoutStyle, GUILayout.Width(320), GUILayout.Height(199));

            GUIContent content = new GUIContent("");

            toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 112;
            toggle_style.fixedHeight = 17;
            if (GUI.Button(new Rect(375, 9, 76, 12), content, toggle_style))
            {
                LCARS.lWindows.setWindowState("Communication", false);
            }


            // fitler buttons
            toggle_style.active.background = (Filter_Mode != 0) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_all", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_all_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 21;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(92, 27, 21, 23), "", toggle_style))
            {
                Filter_Mode = 0;
            }

            toggle_style.active.background = (Filter_Mode != 1) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_1", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_1_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 15;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(115, 27, 15, 23), "", toggle_style))
            {
                Filter_Mode = 1;
            }

            toggle_style.active.background = (Filter_Mode != 2) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_2", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_2_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 19;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(132, 27, 19, 23), "", toggle_style))
            {
                Filter_Mode = 2;
            }

            toggle_style.active.background = (Filter_Mode != 3) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_3", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_3_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 17;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(153, 27, 17, 23), "", toggle_style))
            {
                Filter_Mode = 3;
            }

            toggle_style.active.background = (Filter_Mode != 4) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_4", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_4_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 19;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(172, 27, 19, 23), "", toggle_style))
            {
                Filter_Mode = 4;
            }

            toggle_style.active.background = (Filter_Mode != 5) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_5", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.priority_5_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 17;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(193, 27, 17, 23), "", toggle_style))
            {
                Filter_Mode = 5;
            }
            
            
            
            // mode buttons
            toggle_style.active.background = (Content_Mode != 1) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.Queue", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.Queue_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 49;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(235, 27, 49, 23), "", toggle_style))
            {
                Content_Mode = 1;
            }

            toggle_style.active.background = (Content_Mode != 2) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.Archive", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.Archive_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 55;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(286, 27, 55, 23), "", toggle_style))
            {
                Content_Mode = 2;
            }

            toggle_style.active.background = (Content_Mode != 3) ? GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.ShipToShip", false) : GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Button.ShipToShip_active", false);
            toggle_style.normal.background = toggle_style.active.background;
            toggle_style.fixedWidth = 72;
            toggle_style.fixedHeight = 23;
            if (GUI.Button(new Rect(357, 27, 72, 23), "", toggle_style))
            {
                Content_Mode = 3;
            }



            GUILayout.BeginVertical(scrollview_style);
            //UnityEngine.Debug.Log("### LCARS_Communication_Station getGUI 2 ");
            Communication_ScrollPosition1 = GUILayout.BeginScrollView(Communication_ScrollPosition1);

            switch(Content_Mode)
            {
                case 1: //"queue":
                    getQueueGui();
                    break;

                case 2: // "archive":
                    getQueueGui();
                    break;

                case 3: // "shiptoship":
                    getShipToShipGui();
                    break;

            }


            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            //UnityEngine.Debug.Log("### LCARS_Communication_Station getGUI done ");


            GUILayout.EndVertical();



        }
        public void getQueueGui()
        {
                Communication_BackGroundLayoutStyle2.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Message_toggle", false);
                Communication_BackGroundLayoutStyle2.onNormal.background = Communication_BackGroundLayoutStyle2.normal.background;
                Communication_BackGroundLayoutStyle2.onHover.background = Communication_BackGroundLayoutStyle2.normal.background;

                Communication_BackGroundLayoutStyle3.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Message_border", false);
                Communication_BackGroundLayoutStyle3.onNormal.background = Communication_BackGroundLayoutStyle3.normal.background;
                Communication_BackGroundLayoutStyle3.onHover.background = Communication_BackGroundLayoutStyle3.normal.background;

                Communication_BackGroundLayoutStyle3b.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Message_border_dark", false);
                Communication_BackGroundLayoutStyle3b.onNormal.background = Communication_BackGroundLayoutStyle3.normal.background;
                Communication_BackGroundLayoutStyle3b.onHover.background = Communication_BackGroundLayoutStyle3.normal.background;

                Communication_BackGroundLayoutStyle4.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Message_bottom", false);
                Communication_BackGroundLayoutStyle4.onNormal.background = Communication_BackGroundLayoutStyle4.normal.background;
                Communication_BackGroundLayoutStyle4.onHover.background = Communication_BackGroundLayoutStyle4.normal.background;

                Communication_BackGroundLayoutStyle5.normal.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/Station_Communication_Message_closed", false);
                Communication_BackGroundLayoutStyle5.onNormal.background = Communication_BackGroundLayoutStyle5.normal.background;
                Communication_BackGroundLayoutStyle5.onHover.background = Communication_BackGroundLayoutStyle5.normal.background;

            string listname = (Content_Mode == 1) ? "Queue" : "Archive";
            GUILayout.Label(listname);
            foreach (LCARS_CommunicationQueue_Type qT in LCARS.lODN.CommunicationQueue)
            {

                if (Filter_Mode != 0)
                {
                    if (Filter_Mode != qT.priority)
                    {
                        continue;
                    }
                }
                if (Content_Mode == 1 && qT.Archive)
                {
                    continue;
                }
                if (Content_Mode == 2 && !qT.Archive)
                {
                    continue;
                }


                //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 3 ");

                // button row start 
                GUILayout.BeginVertical(Communication_BackGroundLayoutStyle2, GUILayout.Width(407), GUILayout.Height(16));
                toggle_style.active.background = GameDatabase.Instance.GetTexture(LCARS.lODN.ShipStatus.LCARS_Plugin_Path + "Icons/blind", false);
                toggle_style.normal.background = toggle_style.active.background;
                toggle_style.padding = new RectOffset(30, 0, 0, 0);
                toggle_style.fixedWidth = 407;
                toggle_style.fixedHeight = 16;
                qT.panel_state = GUILayout.Toggle(qT.panel_state, qT.Message_Object.sender_id_line + "   Priority:" + qT.priority, toggle_style);
                GUILayout.EndVertical();
                // button row end
                if (qT.panel_state)
                {
                    // content row start
                    GUILayout.BeginVertical(Communication_BackGroundLayoutStyle3, GUILayout.Width(407)); //middle row
                    //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 8 ");


                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Sender: " + qT.Message_Object.sender);
                    GUILayout.Label("Recipient: " + qT.Message_Object.receiver_type + " " + qT.Message_Object.receiver_code);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(qT.plain_title);
                    GUILayout.EndHorizontal();
                    //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9 ");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(qT.plain_message);
                    GUILayout.EndHorizontal();
                    //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9b ");

                    try
                    {
                        if (qT.Message_Object.reply_options.Count > 0)
                        {
                            if (!qT.Encrypted || qT.Decrypted )
                            {
                                //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9c ");
                                foreach (LCARS_Message_Reply R in qT.Message_Object.reply_options)
                                {
                                    //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9d ");
                                    if (GUILayout.Button(R.buttonText))
                                    {
                                        UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI LCARS_Message_Reply was sent - " + R.replyCode + ":" + R.replyID);
                                        try
                                        {
                                            //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9e ");
                                            LCARSNCI_Bridge.Instance.SendMessageReply(qT.Message_Object.id.ToString(), R.replyCode, R.replyID);
                                            qT.reply_sent = true;
                                            qT.reply_sent_buttonText = R.buttonText;
                                            qT.reply_sent_replyCode = R.replyCode;
                                            qT.reply_sent_replyID = R.replyID;
                                        }
                                        catch
                                        {
                                            UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI LCARSNCI_Bridge.Instance.SendMessageReply failed ");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 9g ");
                    }
                    //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 10 ");
                    GUILayout.EndVertical();
                    // content row end


                    // options row start
                    GUILayout.BeginVertical(Communication_BackGroundLayoutStyle3b, GUILayout.Width(407)); //middle row

                    GUILayout.BeginHorizontal();
                        
                        GUILayout.BeginVertical();
                        if (qT.Message_Object.isEncrypted || qT.EncryptionMode == 0 || qT.EncryptionMode == 1 || qT.EncryptionMode == 2)
                        {
                            if (qT.EncryptionMode == 0 && !qT.Decrypted)
                            {
                                GUILayout.BeginHorizontal();
                                if (GUILayout.Button("Tie in the UT"))
                                {
                                    UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI a: Tie in the UT ");
                                    qT.DecryptHelper1 = qT.orig_title;
                                    qT.plain_title = "";
                                    qT.DecryptHelper2 = qT.orig_message;
                                    qT.plain_message = "";
                                    qT.Decrypted = false;
                                }
                                GUILayout.EndHorizontal();
                                //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 2 ");
                            }
                            if (qT.EncryptionMode == 1)
                            {
                                if (qT.Decrypted)
                                {
                                    GUILayout.Label("This code is broken Sir, we can encode it only partially!");
                                }

                                GUILayout.BeginHorizontal();
                                if (LCARS.lODN.ShipSystems["UniversalTranslator"].isNominal)
                                {
                                    string label = (qT.Message_Object.isEncrypted) ? "Tie in the UT" : "..try the UT again";
                                    if (GUILayout.Button(label))
                                    {
                                        UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI b: " + label);
                                        qT.DecryptHelper1 = qT.orig_title;
                                        qT.plain_title = "";
                                        qT.DecryptHelper2 = qT.orig_message;
                                        qT.plain_message = "";
                                        qT.Decrypted = false;
                                    }
                                }
                                else 
                                {
                                    GUILayout.Label("The Universal Translater is not Online!");
                                }
                                GUILayout.EndHorizontal();
                                //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI 2 ");
                            }
                            if (qT.EncryptionMode == 2)
                            {
                                if (!qT.Decrypted)
                                {
                                    if (LCARS.lODN.ShipSystems["UniversalTranslator"].isNominal)
                                    {
                                        if (GUILayout.Button("Tie in the UT"))
                                        {
                                            UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI c: Tie in the UT ");
                                            qT.DecryptHelper1 = qT.orig_title;
                                            qT.plain_title = "";
                                            qT.DecryptHelper2 = qT.orig_message;
                                            qT.plain_message = "";
                                            qT.Decrypted = false;
                                        }
                                    }
                                    else
                                    {
                                        GUILayout.Label("The Universal Translater is not Online!");
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("This code is of unknow origin Sir, we can not encode it!");
                                    try
                                    {
                                        if (qT.Message_Object.decryption_artefact != null)
                                        {
                                            if (LCARS.lODN.ArtefactInventory.ContainsKey(qT.Message_Object.decryption_artefact))
                                            {
                                                GUILayout.Label("Luckily we have an artefact that should help.");
                                                if (GUILayout.Button("Use " + qT.Message_Object.decryption_artefact + " to decode"))
                                                {
                                                    UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI use decryption_artefact " + qT.Message_Object.decryption_artefact);
                                                    qT.EncryptionMode = 0;
                                                    qT.DecryptHelper1 = qT.orig_title;
                                                    qT.plain_title = "";
                                                    qT.DecryptHelper2 = qT.orig_message;
                                                    qT.plain_message = "";
                                                    qT.Decrypted = false;
                                                }
                                            }
                                            else
                                            {
                                                GUILayout.Label("To decode this, we need to find: " + qT.Message_Object.decryption_artefact);
                                            }
                                        }
                                    }
                                    catch(Exception ex){}
                                }
                            }

                        }
                        if ((qT.DecryptHelper1.Length != 0 || qT.DecryptHelper2.Length != 0) && !qT.Decrypted)
                        {
                            UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI attemptToDecrypt ");
                            qT.attemptToDecrypt();
                        }
                        GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    if (Content_Mode != 2)
                    {
                        GUILayout.BeginVertical();
                        if (GUILayout.Button("Archive"))
                        {
                            UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI Button Archive ");
                            qT.Archive = true;
                            qT.panel_state = false;
                        }
                        GUILayout.EndVertical();
                    }
                    if (qT.reply_sent || qT.Archive)
                    {
                        GUILayout.BeginVertical();
                        if (GUILayout.Button("Dismiss"))
                        {
                            UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI Button Dismiss ");
                            LCARS.lODN.CommunicationQueue.Remove(qT);
                        }
                        GUILayout.EndVertical();
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    // options row end




                    // bottom border-row start
                    GUILayout.BeginVertical(Communication_BackGroundLayoutStyle4, GUILayout.Width(407), GUILayout.Height(6)); // bottom row
                    caption_style.fontSize = 2;
                    caption_style.fixedHeight = 2;
                    GUILayout.Label("", caption_style);
                    GUILayout.EndVertical();
                    // bottom border-row end

                }
                else
                {
                    // closed content row start 
                    GUILayout.BeginVertical(Communication_BackGroundLayoutStyle5, GUILayout.Width(407), GUILayout.Height(10));
                    caption_style.fontSize = 2;
                    caption_style.fixedHeight = 2;
                    GUILayout.Label("", caption_style);
                    GUILayout.EndVertical();
                    // closed content row end 
                }





                //GUILayout.Label("Prior.: " + qT.priority + " - Encr.:" + qT.Message_Object.isEncrypted + " - Decr.:" + qT.Message_Object.isDecrypted);
                //UnityEngine.Debug.Log("### LCARS_Communication_Station getQueueGUI done ");

            }
        }


        public void getShipToShipGui()
        {
            GUILayout.Label("ShipToShip");
            GUILayout.Label("ToDo");
        }
    }
}
