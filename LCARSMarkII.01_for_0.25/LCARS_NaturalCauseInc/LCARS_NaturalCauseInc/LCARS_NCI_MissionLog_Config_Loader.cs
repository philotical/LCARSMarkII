using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_MissionLog_Config_Loader
    {
        public LCARS_NCI_Personalities deconstruct_missionlog_config(LCARS_NCI_Personalities obj, ConfigNode config)
        {
            /*
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_MissionLog_Config_Loader deconstruct_missionlog_config");
            ConfigNode REQUIREMENTS = config.GetNode("PERSONALITIES");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            obj = new LCARS_NCI_Personalities();
            obj.list = new Dictionary<string, LCARS_NCI_Personality>();
            foreach (ConfigNode Item in LIST.GetNodes("PERSONALITY"))
            {
                LCARS_NCI_Personality NCIPers = new LCARS_NCI_Personality();

                NCIPers.name = Item.GetValue("name");
                NCIPers.description = Item.GetValue("description");
                NCIPers.idcode = Item.GetValue("idcode");
                NCIPers.portrait = Item.GetValue("portrait");

                obj.list.Add(NCIPers.idcode, NCIPers);
            }
            return obj;
            */
            return null;
        }
    }
}
