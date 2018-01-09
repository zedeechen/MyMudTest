using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class EquipConfig : IConfig
    {
        public static uint ID = 4;

        private static Dictionary<byte, EquipConfig> mConfigs;
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
                sb.AppendLine("id,name,pos");
                sb.AppendLine("1,Sword,1");
                sb.AppendLine("2,Helmet,2");
                sb.AppendLine("3,PlatArmor,3");
                mConfigs = CSVUtilBase.ParseContent<byte, EquipConfig>(sb.ToString(), "id");
            }
        }

        [CSVElement("id")]
        public byte id { get; set; }
        [CSVElement("name")]
        public string name { get; set; }
        [CSVElement("pos")]
        public byte position { get; set; }
    }
}
