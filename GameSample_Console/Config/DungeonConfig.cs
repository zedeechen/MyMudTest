using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class DungeonConfig : IConfig
    {
        public static uint ID = 3;
        private static Dictionary<byte, DungeonConfig> mConfigs;

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
                sb.AppendLine("id,name,eventnum");
                sb.AppendLine("1,Raid 1,3");
                sb.AppendLine("2,Raid 2,3");
                sb.AppendLine("3,Raid 3,4");
                mConfigs = CSVUtilBase.ParseContent<byte, DungeonConfig>(sb.ToString(), "id");
            }
        }

        public DungeonConfig GetDataById(byte id)
        {
            InitConfig();
            DungeonConfig config;
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
        [CSVElement("eventnum")]
        public byte eventNum { get; set; }
    }
}
