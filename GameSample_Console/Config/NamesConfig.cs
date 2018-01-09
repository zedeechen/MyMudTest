using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class NamesConfig : IConfig
    {
        public static uint ID = 5;
        
        private static Dictionary<byte, List<NamesConfig>> mConfigsByRace;
        public void Dispose()
        {
            mConfigsByRace.Clear();
            mConfigsByRace = null;    
        }

        public List<NamesConfig> GetNameListByRace(byte raceId)
        {
            List<NamesConfig> list;
            mConfigsByRace.TryGetValue(raceId, out list);

            return list;
        }

        public void InitConfig()
        {
            if (mConfigsByRace == null)
            {
                mConfigsByRace = CSVUtilBase.ParseContentWithGroup<byte, NamesConfig>(Properties.Resources.names, "race_id");                
            }
        }
        
        [CSVElement("race_id")]
        public byte raceId { get; set; }
        [CSVElement("fname")]
        public string fName { get; set; }
        [CSVElement("lname")]
        public string lName { get; set; }
    }
}
