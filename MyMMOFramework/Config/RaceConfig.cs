using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class RaceConfig : IConfig
    {
        public static uint ID = 2;
        private static Dictionary<byte, RaceConfig> mConfigs;

        public void Dispose()
        {
            mConfigs.Clear();
            mConfigs = null;
        }

        public void InitConfig()
        {
            if (mConfigs == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("id,name,str_m,int_m,dex_m,con_m,wis_m,cha_m");
                sb.AppendLine("1,Human,0,0,0,0,0,0");
                sb.AppendLine("2,Dwarf,2,0,0,2,0,-2");
                sb.AppendLine("3,Elf,0,0,2,-2,0,0");
                mConfigs = CSVUtilBase.ParseContent<byte, RaceConfig>(sb.ToString(), "id");
            }
        }

        public RaceConfig GetDataById(byte id)
        {
            InitConfig();
            RaceConfig config;
            mConfigs.TryGetValue(id, out config);
            return config;
        }

        public int GetMaxId()
        {
            InitConfig();
            return mConfigs.Count;
        }

        [CSVElement("id")]
        public byte id { get; set; }
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
