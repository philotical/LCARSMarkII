using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_Personalities_Config_Loader
    {
        public LCARS_NCI_Personalities deconstruct_personalities_config(LCARS_NCI_Personalities obj, ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Personalities_Config_Loader deconstruct_personalities_config");
            ConfigNode REQUIREMENTS = config.GetNode("PERSONALITIES");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            obj = new LCARS_NCI_Personalities();
            obj.list = new Dictionary<string,LCARS_NCI_Personality>();
            foreach (ConfigNode Item in LIST.GetNodes("PERSONALITY"))
            {
                LCARS_NCI_Personality NCIPers = new LCARS_NCI_Personality();

                NCIPers.name = Item.GetValue("name");
                NCIPers.description = Item.GetValue("description");
                NCIPers.idcode = Item.GetValue("idcode");
                NCIPers.portrait = Item.GetValue("portrait");

                obj.list.Add(NCIPers.idcode,NCIPers);
            }
            return obj;
        }
    }
    class LCARS_NCI_Artefacts_Config_Loader
    {
        public LCARS_NCI_Artefacts deconstruct_artefacts_config(LCARS_NCI_Artefacts obj, ConfigNode config)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Artefacts_Config_Loader deconstruct_artefacts_config");
            ConfigNode REQUIREMENTS = config.GetNode("ARTEFACTS");
            ConfigNode LIST = REQUIREMENTS.GetNode("LIST");

            obj = new LCARS_NCI_Artefacts();
            obj.list = new Dictionary<string, LCARS_NCI_InventoryItem_Type>();
            foreach (ConfigNode Item in LIST.GetNodes("ARTEFACT"))
            {
                LCARS_NCI_InventoryItem_Type NCIArt = new LCARS_NCI_InventoryItem_Type();

                NCIArt.name = Item.GetValue("name");
                NCIArt.description = Item.GetValue("description");
                NCIArt.idcode = Item.GetValue("idcode");
                NCIArt.icon = Item.GetValue("icon");
                NCIArt.isDamagable = Item.GetValue("isDamagable");
                NCIArt.integrity = Item.GetValue("integrity");
                NCIArt.usage_amount = Item.GetValue("usage_amount");
                NCIArt.usage_times = Item.GetValue("usage_times");
                NCIArt.powerconsumption = Item.GetValue("powerconsumption");

                obj.list.Add(NCIArt.idcode, NCIArt);
            }
            return obj;
        }
    }
}
