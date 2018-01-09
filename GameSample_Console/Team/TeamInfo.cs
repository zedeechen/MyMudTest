using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    [Serializable]
    public class TeamInfo
    {
        private List<HeroInfo> mHeroes;
        public IReadOnlyList<HeroInfo> MHeroes {
            get {
                if (mHeroes == null)
                    mHeroes = new List<HeroInfo>();
                return mHeroes;
            }
        }

        internal void AddHero(HeroInfo heroInfo)
        {
            if (mHeroes == null)
                mHeroes = new List<HeroInfo>();

            mHeroes.Add(heroInfo);
        }
    }
}
