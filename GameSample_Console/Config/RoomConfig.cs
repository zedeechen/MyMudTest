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

        public static Dictionary<int, RoomConfig> mConfig;
        public void Dispose()
        {
            mConfig.Clear();
            mConfig = null;
        }

        public void InitConfig()
        {
            if (mConfig == null)
            {
                mConfig = CSVUtilBase.ParseContent<int, RoomConfig>(Properties.Resources.room, "id");
            }
        }
    }
}
