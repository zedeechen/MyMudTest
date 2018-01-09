using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class ClassConfig : IConfig
    {
        public static uint ID = 1;
        
        private static Dictionary<byte, ClassConfig> mConfigs;

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
                sb.AppendLine("id,name,hd,bab,skillpoint");//,fort,ref,will,skillpoint");
                sb.AppendLine("1,Cleric,6,0.6,4");
                sb.AppendLine("2,Fighter,10,1,2");
                sb.AppendLine("3,Wizard,4,0.4,6");
                mConfigs = CSVUtilBase.ParseContent<byte, ClassConfig>(sb.ToString(), "id");
            }
        }

        public ClassConfig GetDataById(byte id)
        {
            InitConfig();
            ClassConfig config;
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
        [CSVElement("hd")]
        public byte hpDice { get; set; }
        [CSVElement("bab")]
        public float baseAttackBonus { get; set; }

    }
}
