using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class RaceConfig : IConfig
    {
        public static uint ID = 2;
        private static Dictionary<byte, RaceConfig> mConfigs;
        private static Dictionary<byte, List<RaceConfig>> mConfigsWithType;
        public void Dispose()
        {
            mConfigs.Clear();
            mConfigs = null;
            mConfigsWithType.Clear();
            mConfigsWithType = null;
        }

        public void InitConfig()
        {
            if (mConfigs == null)
            {
                mConfigs = CSVUtilBase.ParseContent<byte, RaceConfig>(Properties.Resources.race, "id");
                mConfigsWithType = CSVUtilBase.ParseContentWithGroup<byte, RaceConfig>(Properties.Resources.race, "type");
            }
        }

        public List<RaceConfig> GetNameListByType(byte type)
        {
            List<RaceConfig> list;
            mConfigsWithType.TryGetValue(type, out list);

            return list;
        }

        public RaceConfig GetDataById(byte id)
        {
            InitConfig();
            RaceConfig config;
            mConfigs.TryGetValue(id, out config);
            return config;
        }

        public int GetMaxId(byte type)
        {
            InitConfig();
            List<RaceConfig> list = GetNameListByType(type);
            if (list == null) return 0;
            return list.Count;
        }

        [CSVElement("id")]
        public byte id { get; set; }
        [CSVElement("type")]
        public byte type { get; set; }
        [CSVElement("name")]
        public string name { get; set; }
        [CSVElement("str_m")]
        public short strMod { get; set; }
        [CSVElement("int_m")]
        public short intMod { get; set; }
        [CSVElement("dex_m")]
        public short dexMod { get; set; }
        [CSVElement("con_m")]
        public short conMod { get; set; }
        [CSVElement("wis_m")]
        public short wisMod { get; set; }
        [CSVElement("cha_m")]
        public short chaMod { get; set; }

        
    }
}
