using GameSample.Equipments;

namespace GameSample.Heroes
{
    public class BattleHeroInfo : HeroInfo
    {
        private HeroInfo mHeroBaseInfo;

        private int mRemainHp;
        public int MAttackTurn { get; private set; }
        public bool MIsAlive { get; internal set; }

        public BattleHeroInfo() : base()
        {
            MIsAlive = true;
        }

        public void SetAttackTurn(int attackTurn)
        {
            MAttackTurn = attackTurn;
        }

        public void SetRemainHp(int remainHp)
        {
            mRemainHp = remainHp;
        }

        public int Side { set; get; }

        public BattleHeroInfo(HeroInfo info, int attackTurn)
        {
            mHeroBaseInfo = info;
            
            mRemainHp = mHeroBaseInfo.GetAdvProp(enmPropType.HP);
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
