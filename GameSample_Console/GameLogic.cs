using GameSample.Equipments;
using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class GameLogic
    {
        public static byte GetReqBuyPoints(int currentPoint)
        {
            if (currentPoint < 8)
                return 0;
            if (currentPoint >= 8 && currentPoint <= 14)
                return 1;
            if (currentPoint <= 16)
                return 2;
            return 3;
        }

        private static bool generatedFirstSeed = false;
        private static int randSeed = 0;

        public static int Dice(int diceNum, int diceType, int addNum = 0)
        {
            if (!generatedFirstSeed)
            {
                long tick = DateTime.Now.Ticks;
                randSeed = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32)).Next();
                generatedFirstSeed = true;
            }
            int retVal = 0;
            Random rand;
            for (int i = 0;i < diceNum;i++)
            {
                rand = new Random(randSeed);
                retVal += rand.Next(1, diceType);
                randSeed = rand.Next();
            }

            retVal += addNum;
            return retVal;
        }

        public static void Attack(BattleHeroInfo attacker, BattleHeroInfo defender, ref StringBuilder log)
        {
            int ab = attacker.BaseInfo.GetAdvProp(enmPropType.ATTACK_BONUS);
            int ac = defender.BaseInfo.GetAdvProp(enmPropType.ARMOR_CLASS);

            int roll = GameLogic.Dice(1, 20, ab);
            log.AppendFormat("{0} attack to {1}!(Roll {2}),", attacker.BaseInfo.Name, defender.BaseInfo.Name, roll);
            if (roll == 1)
            {
                log.AppendLine("Miss!");
            }else if (roll == 20)
            {
                //简化确认重击步骤
                log.AppendLine("Critical!").AppendFormat(" Cause {0} damage!", CalcDamage(attacker, defender, true)).AppendLine();
            }
            else if (roll >= ac)
            {
                log.Append("Hit!").AppendFormat(" Cause {0} damage!", CalcDamage(attacker, defender, false)).AppendLine();
            }
            else
            {
                log.AppendLine("Miss!");
            }
            if (!defender.MIsAlive)
            {
                log.AppendLine(defender.BaseInfo.Name + " dead");
            }
        }

        public static int CalcDamage(BattleHeroInfo attacker, BattleHeroInfo defender, bool isCritical)
        {
            EquipInfo wInfo = attacker.GetWeapon();
            
            int dmg = 0;
            if (wInfo == null)
            {
                dmg = attacker.BaseInfo.GetAdvProp(enmPropType.ATTACK_BONUS);
                if (isCritical)
                {
                    dmg *= 2; //简化徒手攻击
                }
            }
            else
            {

            }

            defender.ReduceHp(dmg);//简化对方减伤
            return dmg;
        }

        public static int SortHeroesByActionTurn(BattleHeroInfo a, BattleHeroInfo b)
        {
            return (a.MAttackTurn > b.MAttackTurn) ? -1 : 1;
        }

        public static HeroInfo CreateRandomHero(byte hType, int crLevel)
        {
            HeroInfo info = new HeroInfo();
            if (hType == 2)
                info.SetRace((byte)Dice(1, SingletonFactory<RaceConfig>.Instance.GetMaxId(hType), 100));
            else
                info.SetRace((byte)Dice(1, SingletonFactory<RaceConfig>.Instance.GetMaxId(hType)));
            info.SetClassLevel((byte)Dice(1, SingletonFactory<ClassConfig>.Instance.GetMaxId()), 1);

            int diceNum, dice;
            switch (crLevel)
            {
                case 1:
                    diceNum = 1;
                    dice = 4;
                    break;
                default:
                    diceNum = 3;
                    dice = 6;
                    break;
            }

            Dictionary<enmPropType, int> basePoints = new Dictionary<enmPropType, int>();
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
            {
                basePoints[type] = Dice(diceNum, dice);
            }
            info.InitBasePoints(basePoints);

            return info;
        }

        private static uint[] expTable = new uint[]{ 0, 1000, 3000, 6000,10000,15000,};
        internal static uint GetExpForNextLevel(int targetLevel)
        {
            if (targetLevel - 1 < expTable.Length)
            {
                return expTable[targetLevel - 1];
            }
            return 0;
        }
    }
}
