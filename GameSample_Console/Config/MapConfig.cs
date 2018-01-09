using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public class MapConfig : IConfig
    {
        public static uint ID = 6;

        public static Dictionary<int, MapConfig> mConfig;

        public void Dispose()
        {
            mConfig.Clear();
            mConfig = null;
        }

        public void InitConfig()
        {
            if (mConfig == null)
            {
                mConfig = CSVUtilBase.ParseContent<int, MapConfig>(Properties.Resources.map, "id");
            }
        }
    }
}
