using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    [Serializable]
    public class ClassModule
    {
        private Dictionary<byte, PropReference<int>> mClassLevels;
        public IReadOnlyDictionary<byte, PropReference<int>> MClassLevels { get { return mClassLevels; } }

        private PropGroup<int> mBasePropBonus;
        private HeroInfo mHeroInfo;

        private uint mExp;

        private int TotalLevel
        {
            get
            {
                int totalLevel = 0;
                foreach (PropReference<int> pr in mClassLevels.Values)
                {
                    totalLevel += pr.GetPropItem().GetValue();
                }
                return totalLevel;
            }
        }

        public void AddExp(uint deltaExp)
        {
            mExp += deltaExp;
        }

        public ClassModule(HeroInfo info)
        {
            mHeroInfo = info;
            mBasePropBonus = new PropGroup<int>(RecalcClassMod);

            mHeroInfo.AddAdvPropRelation(mBasePropBonus);
        }
        
        #region Props
        public void SetClassLevel(byte classId, int level)
        {
            if (mClassLevels == null)
                mClassLevels = new Dictionary<byte, PropReference<int>>();

            PropReference<int> prop;
            if (!mClassLevels.TryGetValue(classId, out prop))
            {
                prop = new PropReference<int>(null);
                
                prop.AddParentProp(mBasePropBonus);
                mClassLevels[classId] = prop;
            }
            prop.SetPropItem(level);
        }

        public void ResetAllClassLevel()
        {
            if (mClassLevels == null)
                mClassLevels = new Dictionary<byte, PropReference<int>>();

            foreach (PropReference<int> pr in mClassLevels.Values)
            {
                pr.RemoveAllParentProp();
            }
        }

        public int GetClassMod(enmPropType type)
        {
            PropItem<int> pi = mBasePropBonus.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }

        private void RecalcClassMod()
        {
            ClassConfig config;

            int hpMod = 0;
            float bab = 0f;
            PropItem<int> prop;
            foreach (byte classId in mClassLevels.Keys)
            {
                config = SingletonFactory<ClassConfig>.Instance.GetDataById(classId);

                if ((prop = mClassLevels[classId].GetPropItem()) != null)
                {
                    hpMod += config.hpDice * prop.GetValue();
                    bab += config.baseAttackBonus * prop.GetValue();
                }
            }
            mBasePropBonus.SetPropItem((int)enmPropType.HP, hpMod);
            mBasePropBonus.SetPropItem((int)enmPropType.ATTACK_BONUS, (int)bab);
        }

        public int GetLevelById(byte classId)
        {
            if (mClassLevels == null)
                return 0;

            PropReference<int> pr;
            if (mClassLevels.TryGetValue(classId, out pr))
            {
                PropItem<int> pi = pr.GetPropItem();
                if (pi != null)
                    return pi.GetValue();
            }
            return 0;
        }
        #endregion

        #region base info
        public string GetAllClass()
        {
            if (mClassLevels == null)
                return string.Empty;
            ClassConfig config;
            StringBuilder sb = new StringBuilder();
            foreach(byte classId in mClassLevels.Keys)
            {
                config = SingletonFactory<ClassConfig>.Instance.GetDataById(classId);
                sb.AppendFormat("{0}({1})", config.name, mClassLevels[classId].GetPropItem().GetValue());
            }
            return sb.ToString();
        }

        internal string GetMainClassName()
        {
            int maxLv = 0;
            byte currClassId = 0;
            foreach (byte classId in mClassLevels.Keys)
            {
                if (mClassLevels[classId].GetPropItem().GetValue() > maxLv)
                {
                    maxLv = mClassLevels[classId].GetPropItem().GetValue();
                    currClassId = classId;
                }
            }
            ClassConfig config = SingletonFactory<ClassConfig>.Instance.GetDataById(currClassId);
            return config.name;
        }

        public void DoPrintExp()
        {
            Console.WriteLine(string.Format("XP : {0}/{1}", mExp, GameLogic.GetExpForNextLevel(TotalLevel + 1)));
        }
        #endregion

    }
}
