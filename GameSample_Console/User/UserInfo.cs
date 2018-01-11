using ZDMMO;
using System.Collections.Generic;
using System;
using System.Text;

namespace GameSample
{
    [Serializable]
    public class UserInfo
    {
        private List<IMaterial> mMaterials;
        private Dictionary<enmBaseResourceType, IBaseItem> mBaseResources;
        
        private List<HeroInfo> mHeroes;
        public IReadOnlyList<HeroInfo> MHeroes{ get { return mHeroes; } }

        private TeamInfo mTeam = new TeamInfo();
        public TeamInfo MTeam {
            get {
                return mTeam;
            }
        }

        public bool MTeamCreated { get; private set; }

        public int MRecordMapId { get; set; }
        public int MRecordRoomId { get; set; }
        
        public void AddHero(HeroInfo hero)
        {
            if (mHeroes == null)
                mHeroes = new List<HeroInfo>();

            if (!mHeroes.Contains(hero))
            {
                mHeroes.Add(hero);
                
                if (mTeam.MHeroes.Count < 6)
                {
                    mTeam.AddHero(hero);
                }

                //SingletonFactory<GameController>.Instance.SaveGame(null);
            }

            MTeamCreated = true;
        }

        internal HeroInfo GetHero(int index)
        {
            if (mHeroes == null || mHeroes.Count <= index)
                return null;
            return mHeroes[index];
        }

        public void AddResources(enmBaseResourceType type, long count)
        {
            if (mBaseResources == null)
                mBaseResources = new Dictionary<enmBaseResourceType, IBaseItem>();

            IBaseItem res;
            if (!mBaseResources.TryGetValue(type, out res))
            {
                res = BaseResourceFactory.CreateResource(type);
                mBaseResources[type] = res;
            }
            res.Add(count);
        }

        private void ClearAllData()
        {
            if (mHeroes == null)
                mHeroes = new List<HeroInfo>();
            else
                mHeroes.Clear();
        }

        internal void AddReward(uint xp)
        {
            for (int i = 0;i < mTeam.MHeroes.Count; ++i)
            {
                mTeam.MHeroes[i].AddExp(xp);
            }
        }

        internal void DoPrint()
        {
            for (int i = 0;i < mHeroes.Count; ++i)
            {
                mHeroes[i].DoPrint();
            }
        }
    }
}
