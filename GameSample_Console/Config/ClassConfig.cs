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
                mConfigs = CSVUtilBase.ParseContent<byte, ClassConfig>(Properties.Resources.classes, "id");
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
