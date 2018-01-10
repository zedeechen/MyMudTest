using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public class MapConfig : IConfig
    {
        public static uint ID = 6;

        public static Dictionary<int, MapConfig> mConfigs;

        public void Dispose()
        {
            mConfigs.Clear();
            mConfigs = null;
        }

        public MapConfig GetDataById(int mapId)
        {
            InitConfig();
            MapConfig config;
            mConfigs.TryGetValue(mapId, out config);
            return config;
        }

        public void InitConfig()
        {
            if (mConfigs == null)
            {
                mConfigs = CSVUtilBase.ParseContent<int, MapConfig>(Properties.Resources.map, "id");
            }
        }

        [CSVElement("id")]
        public int id { get; set; }
        [CSVElement("scene_type")]
        public int sceneType { get; set; }
        [CSVElement("default_room")]
        public int defaultRoomId { get; set; }
        [CSVElement("room_list")]
        public string roomList { get; set; }
        [CSVElement("directions")]
        public string directions { get; set; }
    }
}
