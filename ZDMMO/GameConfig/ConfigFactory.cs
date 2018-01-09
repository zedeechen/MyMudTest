using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDMMO
{
    public class ConfigFactory
    {
        public static Dictionary<uint, IConfig> mConfigs;

        public static void Create<T>(uint configTypeId) where T :IConfig, new()
        {
            if (mConfigs == null)
                mConfigs = new Dictionary<uint, IConfig>();

            if (!mConfigs.ContainsKey(configTypeId))
            {
                IConfig config = new T();
                config.InitConfig();
                mConfigs[configTypeId] = config;
            }
        }

        public static void DestroyAllConfigs()
        {
            foreach (IConfig config in mConfigs.Values)
            {
                if (config != null)
                    config.Dispose();
            }
            mConfigs.Clear();
            mConfigs = null;
        }
    }
}
