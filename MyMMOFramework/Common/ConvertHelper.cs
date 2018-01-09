using GameSample.Heroes;
using System.Collections.Generic;

namespace GameSample.Common
{
    public class ConvertHelper
    {
        public static BattleHeroInfo ConvertHeroInfoToBattleInfo(HeroInfo info)
        {
            BattleHeroInfo ret = new BattleHeroInfo();
            ret.SetRace(info.GetRaceId());

            foreach(byte classId in info.MClassLevels.Keys)
            {
                ret.SetClassLevel(classId, info.MClassLevels[classId].GetPropItem().GetValue());
            }

            Dictionary<enmPropType, int> _bps = new Dictionary<enmPropType, int>();
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type ++)
            {
                _bps[type] = info.GetBaseProp(type);
            }
            ret.InitBasePoints(_bps);

            return ret;
        }
    }
}
