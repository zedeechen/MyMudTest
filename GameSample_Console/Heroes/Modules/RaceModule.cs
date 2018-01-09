using System;
using ZDMMO;

namespace GameSample
{
    [Serializable]
    /// <summary>
    /// Race of Hero
    /// Only initialize once when hero instance is created, as race cannot be changed once determined.
    /// </summary>
    public class RaceModule
    {
        private byte mRaceId;
        private PropGroup<int> mRaceMod;

        private HeroInfo mHeroInfo;

        public RaceModule(HeroInfo info)
        {
            mHeroInfo = info;
            
            mRaceMod = new PropGroup<int>(RecalcRaceMod);//, "RaceMod");
            mHeroInfo.AddBasePropRelation(mRaceMod);
        }
        #region Props
        public void SetRace(byte raceId)
        {
            mRaceId = raceId;
            RecalcRaceMod();
        }

        private void RecalcRaceMod()
        {
            RaceConfig config = SingletonFactory<RaceConfig>.Instance.GetDataById(mRaceId);

            if (config != null)
            {
                mRaceMod.SetPropItem((int)enmPropType.STR, config.strMod);
                mRaceMod.SetPropItem((int)enmPropType.DEX, config.dexMod);
                mRaceMod.SetPropItem((int)enmPropType.INT, config.intMod);
                mRaceMod.SetPropItem((int)enmPropType.CON, config.conMod);
                mRaceMod.SetPropItem((int)enmPropType.WIS, config.wisMod);
                mRaceMod.SetPropItem((int)enmPropType.CHA, config.chaMod);
            }
        }

        public int GetRaceMod(enmPropType type)
        {
            PropItem<int> pi = mRaceMod.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }
        #endregion

        #region Base Info
        public string GetRace()
        {
            RaceConfig config = SingletonFactory<RaceConfig>.Instance.GetDataById(mRaceId);

            if (config != null)
            {
                return config.name;
            }
            return string.Empty;
        }
        #endregion

        #region Data

        public byte MRaceId { get { return mRaceId; } }
        #endregion
    }
}
