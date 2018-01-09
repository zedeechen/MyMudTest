using System;
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
        public bool PlayerWin
        {
            get
            {
                if (MBattleEnd && mTeam1.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public uint RewardXP
        {
            get; set;
        }

        public void InitBattle()
        {
            mTotalTurn = 0;

            TeamInfo team1 = SingletonFactory<UserInfo>.Instance.MTeam;
            TeamInfo team2 = new TeamInfo();

            int count = GameLogic.Dice(1, 4);

            HeroInfo info;
            int crLevel = 1;
            for (int i = 0; i < count; i++)
            {
                info = GameLogic.CreateRandomHero(2, crLevel);
                info.Name = string.Format("{0} {1}",info.GetRace(),info.GetMainClassName());
                team2.AddHero(info);
            }

            RewardXP = (uint)(team2.MHeroes.Count * (crLevel * 10));

            SetTeam(team1, team2);

            MBattleEnd = false;
        }

        internal void DoPrintTeamInfo(ref StringBuilder sb)
        {
            if (sb == null) sb = new StringBuilder();

            sb.AppendLine("===Your Team===");
            for (int i = 0;i < mTeam1.Count;++i)
            {
                sb.Append(i + 1).Append(" . ");
                mTeam1[i].DoPrint(ref sb);
            }
            sb.AppendLine("===Enemy Team===");
            for (int i = 0; i < mTeam2.Count; ++i)
            {
                sb.Append(i + 1).Append(" . ");
                mTeam2[i].DoPrint(ref sb);
            }
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
                //bInfo = ConvertHelper.ConvertHeroInfoToBattleInfo(info);// new BattleHeroInfo(info, GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                //bInfo.SetAttackTurn(GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                //bInfo.SetRemainHp(info.GetAdvProp(enmPropType.HP));
                //bInfo.Name = "Your " + bInfo.GetRace() + " " + (i+1);
                //bInfo.Side = 1;
                bInfo = new BattleHeroInfo(info, GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)), 1);
                mTeam1.Add(bInfo);
                mActionTurn.Add(bInfo);
            }

            for (int i = 0; i < team2.MHeroes.Count; i++)
            {
                info = team2.MHeroes[i];
                //bInfo = ConvertHelper.ConvertHeroInfoToBattleInfo(info);
                //bInfo.SetAttackTurn(GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)));
                //bInfo.SetRemainHp(info.GetAdvProp(enmPropType.HP));
                //bInfo.Name = bInfo.GetRace() + " " + (i + 1);
                //bInfo.Side = 2;
                bInfo = new BattleHeroInfo(info, GameLogic.Dice(1, 20, info.GetBasePropMod(enmPropType.DEX)), 2);
                mTeam2.Add(bInfo);
                mActionTurn.Add(bInfo);
            }

            mActionTurn.Sort(GameLogic.SortHeroesByActionTurn);
        }

        public void NextRound(int targetIndex, ref StringBuilder log)
        {
            //if (mTotalTurn > 20)
            //{
            //    MBattleEnd = true;
            //    return;
            //}
            if (log == null)
                log = new StringBuilder();

            int i = 0;
            while (i < mActionTurn.Count)
            {
                if (mActionTurn[i].MIsAlive)
                {
                    if (mActionTurn[i].MSide == 1)
                    {
                        if (mTeam2.Count <= targetIndex || !mTeam2[targetIndex].MIsAlive)
                        {
                            for (int j = 0; j < mTeam2.Count; j++)
                            {
                                if (mTeam2[j].MIsAlive)
                                {
                                    targetIndex = j;
                                    break;
                                }
                            }
                        }
                        GameLogic.Attack(mActionTurn[i], mTeam2[targetIndex], ref log);
                        if (!mTeam2[targetIndex].MIsAlive)
                        {
                            mTeam2.RemoveAt(targetIndex);
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
