using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCARSMarkII
{
    class LCARS_NCI_Personalities_Config_Writer
    {
        public ConfigNode construct_personalities_config(LCARS_NCI_Personalities List)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Personalities_Config_Writer construct_personalities_config");
            ConfigNode cn_personalities = new ConfigNode("PERSONALITIES");
            ConfigNode cn_personalities_list = new ConfigNode("LIST");

            if (List == null)
            {
                List = new LCARS_NCI_Personalities();
            }
            if (List.list == null)
            {
                List.list = new Dictionary<string, LCARS_NCI_Personality>();
            }
            foreach (LCARS_NCI_Personality p in List.list.Values)
            {
                ConfigNode cn_personalities_personality = new ConfigNode("PERSONALITY");
                cn_personalities_personality.AddValue("name", p.name);
                cn_personalities_personality.AddValue("idcode", p.idcode);
                cn_personalities_personality.AddValue("description", p.description);
                cn_personalities_personality.AddValue("portrait", p.portrait);

                cn_personalities_list.AddNode(cn_personalities_personality);
            }

            cn_personalities.AddNode(cn_personalities_list);

            return cn_personalities;
        }
    }
    class LCARS_NCI_Artefacts_Config_Writer
    {
        public ConfigNode construct_artefacts_config(LCARS_NCI_Artefacts List)
        {
            UnityEngine.Debug.Log("### NCIMC LCARS_NCI_Artefacts_Config_Writer construct_artefacts_config");
            ConfigNode cn_artefacts = new ConfigNode("ARTEFACTS");
            ConfigNode cn_artefacts_list = new ConfigNode("LIST");

            if (List == null)
            {
                List = new LCARS_NCI_Artefacts();
            }
            if (List.list == null)
            {
                List.list = new Dictionary<string,LCARS_NCI_InventoryItem_Type>();
            }
            foreach (LCARS_NCI_InventoryItem_Type e in List.list.Values)
            {
                ConfigNode cn_artefacts_artefact = new ConfigNode("ARTEFACT");
                cn_artefacts_artefact.AddValue("name", e.name);
                cn_artefacts_artefact.AddValue("idcode", e.idcode);
                cn_artefacts_artefact.AddValue("description", e.description);
                cn_artefacts_artefact.AddValue("icon", e.icon);

                cn_artefacts_artefact.AddValue("isDamagable", e.isDamagable);
                cn_artefacts_artefact.AddValue("integrity", e.integrity);
                cn_artefacts_artefact.AddValue("usage_amount", e.usage_amount);
                cn_artefacts_artefact.AddValue("usage_times", e.usage_times);
                cn_artefacts_artefact.AddValue("powerconsumption", e.powerconsumption);

                cn_artefacts_list.AddNode(cn_artefacts_artefact);
            }

            cn_artefacts.AddNode(cn_artefacts_list);

            return cn_artefacts;
        }
    }
}
