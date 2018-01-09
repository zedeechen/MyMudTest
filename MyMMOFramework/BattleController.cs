using GameSample.Common;
using GameSample.Heroes;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class BattleController
    {
        private List<BattleHeroInfo> mTeam1;
        private List<BattleHeroInfo> mTeam2;

        private List<BattleHeroInfo> mActionTurn;
        private int mTotalTurn;

        public bool MBattleEnd { get; private set; }

        public void InitBattle()
        {
            mTotalTurn = 0;

            TeamInfo team1 = SingletonFactory<UserInfo>.Instance.MTeam;
            TeamInfo team2 = new TeamInfo();

            int count = GameLogic.Dice(1, 4);

            for (int i = 0; i < count; i++)
            {
                team2.AddHero(GameLogic.CreateRandomHero(1));
            }

            SetTeam(team1, team2);

            MBattleEnd = false;
        }

        public void SetTeam(TeamInfo team1, TeamInfo team2)
        {
            mTeam1 = new List<BattleHeroInfo>();
            mTeam2 = new List<BattleHeroInfo>();
            mActionTurn = new List<BattleHeroInfo>();

            HeroInfo info;
            BattleHeroInfo bInfo;
            for (int i = 0;i < team1.MHeroes.Count; i ++)
            {
                info = team1.MHeroes[i];
                bInfo = ConvertHelper.ConvertHeroInfoToBattleInfo(info);// new BattleHeroInfo(info, GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                bInfo.SetAttackTurn(GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                bInfo.SetRemainHp(info.GetAdvProp(enmPropType.HP));
                bInfo.Name = "Your " + bInfo.GetRace() + " " + (i+1);
                bInfo.Side = 1;
                mTeam1.Add(bInfo);
                mActionTurn.Add(bInfo);
            }

            for (int i = 0; i < team2.MHeroes.Count; i++)
            {
                info = team2.MHeroes[i];
                bInfo = ConvertHelper.ConvertHeroInfoToBattleInfo(info);
                bInfo.SetAttackTurn(GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                bInfo.SetRemainHp(info.GetAdvProp(enmPropType.HP));
                bInfo.Name = bInfo.GetRace() + " " + (i + 1);
                bInfo.Side = 2;
                mTeam2.Add(bInfo);
                mActionTurn.Add(bInfo);
            }

            mActionTurn.Sort(GameLogic.SortHeroesByActionTurn);
        }

        public void NextRound(ref StringBuilder log)
        {
            //if (mTotalTurn > 20)
            //{
            //    MBattleEnd = true;
            //    return;
            //}

            int i = 0;
            while (i < mActionTurn.Count)
            {
                if (mActionTurn[i].MIsAlive)
                {
                    if (mActionTurn[i].Side == 1)
                    {
                        for (int j = 0; j < mTeam2.Count; j++)
                        {
                            if (mTeam2[j].MIsAlive)
                            {
                                GameLogic.Attack(mActionTurn[i], mTeam2[j], ref log);
                                if (!mTeam2[j].MIsAlive)
                                {
                                    mTeam2.RemoveAt(j);
                                }
                                break;
                            }
                        }
                        
                    }
                    else
                    {
                        for (int j = 0; j < mTeam1.Count; j++)
                        {
                            if (mTeam1[j].MIsAlive)
                            {
                                GameLogic.Attack(mActionTurn[i], mTeam1[j], ref log);
                                if (!mTeam1[j].MIsAlive)
                                {
                                    mTeam1.RemoveAt(j);
                                }
                                break;
                            }
                        }
                    }
                }

                i++;
            }
            

            if (mTeam1.Count == 0 || mTeam2.Count == 0)
            {
                MBattleEnd = true;
                if (mTeam2.Count == 0)
                {
                    log.AppendLine("You win!");
                }
                else
                {
                    log.AppendLine("You lose!");
                }
                return;
            }
            else
            {
                mTotalTurn++;
            }


        }
    }
}
