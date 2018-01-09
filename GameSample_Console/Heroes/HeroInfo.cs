using ZDMMO;
using System.Collections.Generic;
using System;
using System.Text;

namespace GameSample
{
    [Serializable]
    /// <summary>
    /// Hero Instance.
    /// </summary>
    public class HeroInfo
    {
        /// <summary>
        /// Base Points includes 6 base props, which consists of Race-modification, Equip-mod, Skill-mod, Feat-mod
        /// </summary>
        private BasePointModule mBasePointModule;
        /// <summary>
        /// level of each learned classes
        /// </summary>
        private ClassModule     mClassModule;
        /// <summary>
        /// hero race
        /// </summary>
        private RaceModule      mRaceModule;

        /// <summary>
        /// advanced(final) props
        /// </summary>
        private PropGroup<int> mAdvProp;
        /// <summary>
        /// Prop modifications, equals to (BaseProp - 10) / 2
        /// </summary>
        private PropGroup<int> mPropMod;
        /// <summary>
        /// Base Props
        /// </summary>
        private PropGroup<int> mBaseProp;
        
        public string Name { set; get; }

        public HeroInfo()
        {
            mAdvProp = new PropGroup<int>(RecalcAdvanceProps);//,"Final");
            mPropMod = new PropGroup<int>(RecalcPropMod);//,"");
            mBaseProp = new PropGroup<int>(RecalcBaseProps);//,"Base");

            mPropMod.AddParentProp(mAdvProp);
            mBaseProp.AddParentProp(mPropMod);

            mRaceModule = new RaceModule(this);
            mClassModule = new ClassModule(this);
            mBasePointModule = new BasePointModule(this);

            //System.Reflection.MemberInfo info = typeof(HeroInfo);
            //mReadOnly = (ReadOnlyAttribute)Attribute.GetCustomAttribute(info, typeof(ReadOnlyAttribute));
        }

        #region Props
        public void AddBasePropRelation(PropBase prop)
        {
            prop.AddParentProp(this.mBaseProp);
        }
        public void AddAdvPropRelation(PropBase prop)
        {
            prop.AddParentProp(this.mAdvProp);
        }

        private void RecalcPropMod()
        {
            PropItem<int> piBase;
            int val;
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
            {
                piBase = mBaseProp.GetPropItem((int)type);
                if (piBase != null)
                    if (piBase.GetValue() < 10)
                        val = (piBase.GetValue() - 11) / 2;
                    else
                        val = (piBase.GetValue() - 10) / 2;
                else
                    val = 0;
                
                mPropMod.SetPropItem((int)type, val);
            }
        }

        private void RecalcBaseProps()
        {
            int val;
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
            {                
                val = mRaceModule.GetRaceMod(type) + mBasePointModule.GetBasePoint(type);
                mBaseProp.SetPropItem((int)type, val);
            }
        }

        private void RecalcAdvanceProps()
        {
            int hp = 0;
            hp += mClassModule.GetClassMod(enmPropType.HP);
            PropItem<int> piMod = mPropMod.GetPropItem((int)enmPropType.CON);
            if (piMod != null)
                hp += piMod.GetValue();
            mAdvProp.SetPropItem((int)enmPropType.HP, hp);

            int ab = 0;
            ab += mClassModule.GetClassMod(enmPropType.ATTACK_BONUS);

            piMod = mPropMod.GetPropItem((int)enmPropType.STR);
            if (piMod != null)
            {
                ab += mPropMod.GetPropItem((int)enmPropType.STR).GetValue();
                if (ab < 0) ab = 0;
            }
            mAdvProp.SetPropItem((int)enmPropType.ATTACK_BONUS, ab);

            int ac = 10;
            piMod = mPropMod.GetPropItem((int)enmPropType.DEX);
            if (piMod != null)
                ac += piMod.GetValue();
            mAdvProp.SetPropItem((int)enmPropType.ARMOR_CLASS, ac);
        }

        public int GetBaseProp(enmPropType type)
        {
            PropItem<int> pi = mBaseProp.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }
        
        public int GetAdvProp(enmPropType type)
        {
            PropItem<int> pi = mAdvProp.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }

        public void SetRace(byte raceId)
        {
            mRaceModule.SetRace(raceId);

            
            List<NamesConfig> list = SingletonFactory<NamesConfig>.Instance.GetNameListByRace(raceId);
            if (list != null && list.Count > 0)
            {
                int fNameIndex = GameLogic.Dice(1, list.Count, -1);
                int lNameIndex = GameLogic.Dice(1, list.Count, -1);

                Name = string.Format("{0} {1}", list[fNameIndex].fName, list[lNameIndex].lName);
            }
        }

        public void ResetAllClassLevel()
        {
            mClassModule.ResetAllClassLevel();
        }

        public void SetClassLevel(byte classId, int level = -1)
        {
            if (level > 0)
            {
                mClassModule.SetClassLevel(classId, level);
            }
            else
            {
                mClassModule.SetClassLevel(classId, mClassModule.GetLevelById(classId) + 1);
            }
        }

        public void InitBasePoints(Dictionary<enmPropType, int> bp)
        {
            mBasePointModule.InitBasePoints(bp);
        }

        public int GetBasePropMod(enmPropType type)
        {
            PropItem<int> pi = mPropMod.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }
        public int GetInitBP(enmPropType type)
        {
            return mBasePointModule.GetBasePoint(type);
        }
        public void AddInitBp(enmPropType type)
        {
            mBasePointModule.AddBasePoint(type);
        }
        public void ReduceInitBp(enmPropType type)
        {
            mBasePointModule.ReduceBasePoint(type);
        }
        #endregion

        #region Base info
        public string GetMainClassName()
        {
            return mClassModule.GetMainClassName();
        }
        public string GetClasses()
        {
            return mClassModule.GetAllClass();
        }
        public string GetRace()
        {
            return mRaceModule.GetRace();
        }
        public byte GetRaceId()
        {
            return mRaceModule.MRaceId;
        }
        public IReadOnlyDictionary<byte, PropReference<int>> MClassLevels { get { return this.mClassModule.MClassLevels; } }

        public void DoPrint(ref StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.AppendLine(string.Format("{0} ({1} {2})", Name, GetRace(), GetMainClassName()));
            mClassModule.DoPrintExp(ref sb);

            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; ++type)
            {
                sb.AppendLine(string.Format("{0} {1} ({2})", type, this.GetBaseProp(type), this.GetBasePropMod(type)));
            }
        }

        public void AddExp(uint deltaExp)
        {
            mClassModule.AddExp(deltaExp);
        }
        #endregion
    }
}
