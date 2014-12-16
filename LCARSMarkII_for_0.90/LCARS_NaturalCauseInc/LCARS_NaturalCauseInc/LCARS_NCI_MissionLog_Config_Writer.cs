using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_MissionLog_Config_Writer
    {
        internal ConfigNode construct_missionlog_config(LCARS_NCI_MissionProgressLog MissionProgressLog)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_MissionLog_Config_Writer construct_missionlog_config begin ");
            ConfigNode cn_MISSION = new ConfigNode("MISSION");
            try
            {
                cn_MISSION.AddValue("missionGuid", MissionProgressLog.missionGuid);
            }
            catch { }
            try
            {
                cn_MISSION.AddValue("title", MissionProgressLog.mission_name);
            }
            catch { }
            try
            {
                cn_MISSION.AddValue("current_step", MissionProgressLog.current_step);
            }
            catch { }
            try
            {
                cn_MISSION.AddValue("current_job", MissionProgressLog.current_job);
            }
            catch { }
            try
            {
                cn_MISSION.AddValue("mission_end_reached", MissionProgressLog.mission_end_reached);
            }
            catch { }
            /*try
            {
                cn_MISSION.AddValue("gain_collected", MissionProgressLog.gain_collected);
            }
            catch { }*/

            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_MissionLog_Config_Writer construct_missionlog_config preparing itemlist ");
            ConfigNode cn_MISSION_ITEMLIST = new ConfigNode("ITEMLIST");
            foreach(LCARS_NCI_MissionProgressLog_Item I in MissionProgressLog.ItemList)
            {
                ConfigNode cn_MISSION_LogItem = new ConfigNode("ITEM");
                cn_MISSION_LogItem.AddValue("timestamp", I.timestamp);
                cn_MISSION_LogItem.AddValue("stepID", I.stepID);
                cn_MISSION_LogItem.AddValue("jobID", I.jobID);
                cn_MISSION_LogItem.AddValue("subject", I.subject);
                cn_MISSION_LogItem.AddValue("details", I.details);
                cn_MISSION_ITEMLIST.AddNode(cn_MISSION_LogItem);
            }
            cn_MISSION.AddNode(cn_MISSION_ITEMLIST);

            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_MissionLog_Config_Writer construct_missionlog_config done ");
            return cn_MISSION;
        }
    }
}
