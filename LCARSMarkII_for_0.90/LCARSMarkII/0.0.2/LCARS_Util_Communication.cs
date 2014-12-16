using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace LCARSMarkII
{
    //[Serializable]
    public class LCARS_Message_Type
    {

        // http://www.fluxbytes.com/csharp/convert-string-to-binary-and-binary-to-string-in-c/
        private string StringToBinary(string data)
        {
            UnityEngine.Debug.Log("### LCARS_Message_Type StringToBinary data=" + data);
            //return data;
            StringBuilder sb = new StringBuilder();
            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        private string BinaryToString(string data)
        {
            UnityEngine.Debug.Log("### LCARS_Message_Type BinaryToString data=" + data);
            //return data;
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }



        public Guid id { get; set; }
        public int mission_message_id { get; set; }
        public int mission_message_stepID { get; set; }
        public int mission_message_jobID { get; set; }

        public string sender { get; set; }
        public string sender_id_line { get; set; }

        public Vessel receiver_vessel { get; set; }

        public string receiver_type { get; set; }

        public string receiver_code { get; set; }

        public int priority { get; set; }

        public List<LCARS_Message_Reply> reply_options { get; set; }

        public void setTitle(string title)
        {
            UnityEngine.Debug.Log("### LCARS_Message_Type setTitle title=" + title);
            binary_title = StringToBinary(title); 
        }
        public void setMessage(string message) 
        {
            UnityEngine.Debug.Log("### LCARS_Message_Type setMessage message=" + message);
            binary_message = StringToBinary(message); 
        }

        public string message { get { return (isEncrypted) ? binary_message : BinaryToString(binary_message); } }
        public string title { get { return (isEncrypted) ? binary_title : BinaryToString(binary_title); } }

        public bool isEncrypted { get { return (Encrypted && !Decrypted) ? true : false; } }
        public bool isDecrypted { get { return (Encrypted && !Decrypted) ? false : true; } }
        public bool setDecrypted { set { Decrypted = true; } }
        public int encryptionMode { get; set; }
        public string decryption_artefact { get; set; }
        public bool decryption_sent_to_log { get; set; }

        private bool Encrypted { get; set; }
        private bool Decrypted { get; set; }
        private bool reply_sent { get; set; }
        private bool dismissed { get; set; }
        private string binary_title { get; set; }
        private string binary_message { get; set; }
        public LCARSMarkII LCARS { get { return receiver_vessel.LCARS(); } }

        public void SendReply() { reply_sent = true;}
        public void Encrypt(int encryption_mode = 0) { Encrypted = true; encryptionMode = encryption_mode; }
        public void Queue()
        {

            if (!LCARS.lODN.ShipSystems["Communication"].isNominal)
            {
                ScreenMessages.PostScreenMessage(
                   "<color=#ff9900ff>Communication System is down - An incomming message was lost.</color>",
                  10f, ScreenMessageStyle.UPPER_CENTER
                );
                return;
            }
            LCARS.lPowSys.draw(LCARS.lODN.ShipSystems["Communication"].name, LCARS.lODN.ShipSystems["Communication"].powerSystem_L2_usage);
            //UnityEngine.Debug.Log("### LCARS_Message_Type Queue binary_message=" + binary_message);
            //UnityEngine.Debug.Log("### LCARS_Message_Type Queue message=" + message);
            try 
            {
                if (priority<=3)
                {
                    LCARS.lWindows.setWindowState("Communication", true);
                }

                if (LCARS.lODN.CommunicationQueue == null)
                {
                    LCARS.lODN.CommunicationQueue = new System.Collections.Generic.List<LCARS_CommunicationQueue_Type>();
                }

                List<LCARS_CommunicationQueue_Type> tmp = LCARS.lODN.CommunicationQueue;
                LCARS.lODN.CommunicationQueue = new List<LCARS_CommunicationQueue_Type>();

                LCARS_CommunicationQueue_Type q = new LCARS_CommunicationQueue_Type();
                q.id = id;
                q.Vessel = receiver_vessel;
                q.plain_title = title;
                q.plain_message = message;
                q.orig_title = title;
                q.orig_message = message;
                q.receiver_code = receiver_code;
                q.Encrypted = isEncrypted;
                q.EncryptionMode = encryptionMode;
                    q.DecryptHelper1 = "";
                    q.DecryptHelper2 = "";
                q.Decrypted = isDecrypted;
                q.reply_sent = reply_sent;
                q.priority = priority;
                q.Message_Object = this;

                LCARS.lODN.CommunicationQueue.Add(q);
                foreach (LCARS_CommunicationQueue_Type qT in tmp)
                {
                    LCARS.lODN.CommunicationQueue.Add(qT);
                }
            }
            catch(Exception ex)
            {
                ScreenMessages.PostScreenMessage(
                   "<color=#ff9900ff>Communication System is unresponsive - An incomming message was lost.</color>",
                  10f, ScreenMessageStyle.UPPER_CENTER
                );
                UnityEngine.Debug.Log("### LCARS_Message_Type: Queue()-Exception: receiver_vessel.LCARS().lODN.CommunicationQueue was not reached, message dismissed");
            }
        }
    }
    //[Serializable]
    public class LCARS_Message_Reply
    {
        public string buttonText { set; get; }
        public string replyCode { set; get; } // goto: none, message, step, job
        public string replyID { set; get; }
    }


    //[Serializable]
    public class LCARS_CommunicationQueue_Type
    {

        public Guid id { get; set; }

        public LCARS_Message_Type Message_Object { get; set; }

        public bool panel_state { get; set; }

        public Vessel Vessel { get; set; }

        public string plain_title { get; set; }

        public string plain_message { get; set; }

        public string orig_title { get; set; }

        public string orig_message { get; set; }

        public string receiver_code { get; set; }

        public bool Encrypted { get; set; }

        public int EncryptionMode { get; set; }

        public bool Decrypted { get; set; }

        public bool reply_sent { get; set; }
        public string reply_sent_buttonText { get; set; }
        public string reply_sent_replyCode { get; set; }
        public string reply_sent_replyID { get; set; }

        public int priority { get; set; }

        public bool Archive { get; set; }

        private string _attemptToDecrypt(string input)
        {
            UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type _attemptToDecrypt input=" + input);
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < input.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(input.Substring(i, 8), 2));
            }
            string output = Encoding.ASCII.GetString(byteList.ToArray());
            UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type _attemptToDecrypt output=" + output);
            return output;
        }
        public string DecryptHelper1 { get; set; }
        public string DecryptHelper2 { get; set; }
        public void attemptToDecrypt()
        {
            Vessel.LCARS().lPowSys.draw(Vessel.LCARS().lODN.ShipSystems["UniversalTranslator"].name, Vessel.LCARS().lODN.ShipSystems["UniversalTranslator"].powerSystem_L2_usage);
            Vessel.LCARS().lPowSys.draw(Vessel.LCARS().lODN.ShipSystems["Communication"].name, Vessel.LCARS().lODN.ShipSystems["Communication"].powerSystem_L2_usage);

            
            switch (EncryptionMode)
            {
                case 0: // default => allways works 100%
                    UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt case 0 ");
                    if (DecryptHelper1.Length > 0)
                    {
                        int random_value = new System.Random().Next(1,20);
                        UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt random_value=" + random_value);
                            plain_title = plain_title + _attemptToDecrypt(DecryptHelper1.Substring(0, 8));
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return encrypted plain_title=" + plain_title);
                        DecryptHelper1 = DecryptHelper1.Substring(8);
                    }
                    if (DecryptHelper2.Length > 0)
                    {
                            plain_message = plain_message + _attemptToDecrypt(DecryptHelper2.Substring(0, 8));
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return encrypted plain_message=" + plain_message);
                        DecryptHelper2 = DecryptHelper2.Substring(8);
                    }
                    if (DecryptHelper1.Length == 0 && DecryptHelper2.Length == 0)
                    {
                        Decrypted = true;
                        Message_Object.setDecrypted = true;
                    }
                    break;

                case 1: // only works 80%
                    UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt case 1 ");
                    //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt DecryptHelper1.Length=" + DecryptHelper1.Length);
                    //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt DecryptHelper1=" + DecryptHelper1);
                    if (DecryptHelper1.Length > 0)
                    {
                        int random_value = new System.Random().Next(1,20);
                        UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt random_value=" + random_value);
                        if (random_value == 1)
                        {
                            plain_title = plain_title + DecryptHelper1.Substring(0, 8);
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return an error plain_title=" + plain_title);
                        }
                        else 
                        {
                            plain_title = plain_title + _attemptToDecrypt(DecryptHelper1.Substring(0, 8));
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return encrypted plain_title=" + plain_title);
                        }
                        DecryptHelper1 = DecryptHelper1.Substring(8);
                    }
                    if (DecryptHelper2.Length > 0)
                    {
                        int random_value = new System.Random().Next(1, 20);
                        if (random_value == 1)
                        {
                            plain_message = plain_message + DecryptHelper2.Substring(0, 8);
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return an error plain_message=" + plain_message);
                        }
                        else 
                        {
                            plain_message = plain_message + _attemptToDecrypt(DecryptHelper2.Substring(0, 8));
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return encrypted plain_message=" + plain_message);
                        }
                        DecryptHelper2 = DecryptHelper2.Substring(8);
                    }
                    if (DecryptHelper1.Length == 0 && DecryptHelper2.Length == 0)
                    {
                        Decrypted = true;
                        Message_Object.setDecrypted = true;
                    }
                    break;

                case 2: // never works
                    UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt case 2 ");
                    if (DecryptHelper1.Length > 0)
                    {
                        int random_value = new System.Random().Next(1,20);
                        //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt random_value=" + random_value);
                            plain_title = plain_title + DecryptHelper1.Substring(0, 8);
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return an error plain_title=" + plain_title);
                        DecryptHelper1 = DecryptHelper1.Substring(8);
                    }
                    if (DecryptHelper2.Length > 0)
                    {
                            plain_message = plain_message + DecryptHelper2.Substring(0, 8);
                            //UnityEngine.Debug.Log("### LCARS_CommunicationQueue_Type attemptToDecrypt return an error plain_message=" + plain_message);
                        DecryptHelper2 = DecryptHelper2.Substring(8);
                    }
                    if (DecryptHelper1.Length == 0 && DecryptHelper2.Length == 0)
                    {
                        Decrypted = true;
                        Message_Object.setDecrypted = true;
                    }
                   break;

            }
            //Vessel.LCARS().lPowSys.draw(Vessel.LCARS().lODN.ShipSystems["UniversalTranslator"].name, 0);
            //Vessel.LCARS().lPowSys.draw(Vessel.LCARS().lODN.ShipSystems["Communication"].name, 0);
            //Vessel.LCARS().lODN.ShipSystems["UniversalTranslator"].powerSystem_consumption_current = 0f;
            //Vessel.LCARS().lODN.ShipSystems["Communication"].powerSystem_consumption_current = 0f;
        }

    }
    public class LCARS_Communication_Util
    {

        
        Vessel thisVessel;
        LCARSMarkII LCARS;
        public void SetVessel(Vessel vessel)
        {
            //UnityEngine.Debug.Log("LCARS_RepairTeam_Crew  SetVessel begin ");
            thisVessel = vessel;
            LCARS = thisVessel.LCARS();
            //UnityEngine.Debug.Log("LCARS_RepairTeam_Crew  SetVessel done ");
            /*
            LCARS_ShipSystem_Type ShipSystem = new LCARS_ShipSystem_Type();
            ShipSystem.name = "Universal Translator";
            ShipSystem.disabled = false;
            ShipSystem.damaged = false;
            ShipSystem.integrity = 100;
            LCARS.lODN.ShipSystems.Add(ShipSystem.name, ShipSystem);
            LCARS_Message_Type foo = new LCARS_Message_Type();
            foo.id = 1;
            foo.sender = "";
            foo.receiver_vessel = thisVessel;
            foo.receiver_type = "";
            foo.receiver_code = "";
            foo.reply_code = "";
            foo.priority = 1;
            foo.title = "Hello world!";
            foo.message = "Ground Controll to Major Tom";
            */
        }

    }
}
