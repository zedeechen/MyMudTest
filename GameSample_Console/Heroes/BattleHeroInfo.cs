using System;
using System.Text;
using GameSample.Equipments;

namespace GameSample
{
    public class BattleHeroInfo
    {
        private HeroInfo mHeroBaseInfo;

        private int mRemainHp;
        public int MAttackTurn { get; private set; }
        public bool MIsAlive { get; internal set; }
        
        public void SetRemainHp(int remainHp)
        {
            mRemainHp = remainHp;
        }

        public int MSide { set; get; }

        public HeroInfo BaseInfo { get { return mHeroBaseInfo; } }

        public BattleHeroInfo(HeroInfo info, int attackTurn, int side)
        {
            mHeroBaseInfo = info;
            
            mRemainHp = mHeroBaseInfo.GetAdvProp(enmPropType.HP);
            MIsAlive = true;
            MSide = side;
            MAttackTurn = attackTurn;
        }

        public object Clone()//implement Clonable
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = 
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(
                    null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            Formatter.Serialize(stream, this);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }

        internal void DoPrint(ref StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append(string.Format("{0} / {1} {2} / ",BaseInfo.Name, BaseInfo.GetRace(), BaseInfo.GetClasses()))
                .AppendLine(string.Format("HP {0}({1})", mRemainHp, BaseInfo.GetAdvProp(enmPropType.HP)));
        }

        internal EquipInfo GetWeapon()
        {
            return null;
        }

        public void ReduceHp(int deltaHp)
        {
            mRemainHp -= deltaHp;
            if (mRemainHp <= 0)
            {
                MIsAlive = false;
            }
        }
    }
    
}
