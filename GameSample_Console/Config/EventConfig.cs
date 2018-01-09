using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class EventConfig : IConfig
    {
        public static uint ID = 8;

        private static Dictionary<int, EventConfig> mConfigs;

        public void Dispose()
        {
            mConfigs.Clear();
            mConfigs = null;
        }

        public void InitConfig()
        {
            if (mConfigs == null)
            {
                mConfigs = CSVUtilBase.ParseContent<int, EventConfig>(Properties.Resources.events, "id");
            }
        }

        [CSVElement("id")]
        public int id { get; set; }
        [CSVElement("type")]
        public int type { get; set; }
        [CSVElement("params")]
        public string eventParams { get; set; }
    }
}
