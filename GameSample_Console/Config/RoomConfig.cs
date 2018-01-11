using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class RoomConfig : IConfig
    {
        public static uint ID = 7;

        public static Dictionary<int, RoomConfig> mConfigs;
        public void Dispose()
        {
            mConfigs.Clear();
            mConfigs = null;
        }

        public void InitConfig()
        {
            if (mConfigs == null)
            {
                mConfigs = CSVUtilBase.ParseContent<int, RoomConfig>(Properties.Resources.room, "id");
            }
        }

        public RoomConfig GetDataById(int roomId)
        {
            InitConfig();
            RoomConfig config;
            mConfigs.TryGetValue(roomId, out config);
            return config;
        }

        [CSVElement("id")]
        public int id { get; set; }
        [CSVElement("desc")]
        public string desc { get; set; }
        [CSVElement("objects")]
        public string objects { get; set; }
        [CSVElement("special_commands")]
        public string specialCommands { get; set; }
    }
}
