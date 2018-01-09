using GameSample.Equipments;
using GameSample.Heroes;
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

        public static void Attack(Heroes.BattleHeroInfo attacker, Heroes.BattleHeroInfo defender, ref StringBuilder log)
        {
            int ab = attacker.GetAdvProp(enmPropType.ATTACK_BONUS);
            int ac = defender.GetAdvProp(enmPropType.ARMOR_CLASS);

            int roll = GameLogic.Dice(1, 20, ab);
            log.AppendFormat("{0} attack to {1}!(Roll {2}),", attacker.Name, defender.Name, roll);
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
                log.AppendLine(defender.Name + " dead");
            }
        }

        public static int CalcDamage(Heroes.BattleHeroInfo attacker, Heroes.BattleHeroInfo defender, bool isCritical)
        {
            EquipInfo wInfo = attacker.GetWeapon();
            
            int dmg = 0;
            if (wInfo == null)
            {
                dmg = attacker.GetAdvProp(enmPropType.ATTACK_BONUS);
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

        public static HeroInfo CreateRandomHero(int crLevel)
        {
            HeroInfo info = new HeroInfo();
            info.SetRace((byte)Dice(1, SingletonFactory<RaceConfig>.Instance.GetMaxId()));
            info.SetClassLevel((byte)Dice(1, SingletonFactory<ClassConfig>.Instance.GetMaxId()), 1);

            int minProp, maxProp;
            switch (crLevel)
            {
                case 1:
                    minProp = 3;
                    maxProp = 10;
                    break;
                default:
                    minProp = 3;
                    maxProp = 18;
                    break;
            }

            Dictionary<enmPropType, int> basePoints = new Dictionary<enmPropType, int>();
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; type++)
            {
                basePoints[type] = Dice(minProp, maxProp);
            }
            info.InitBasePoints(basePoints);

            return info;
        }
    }
}
