using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class BattleScene : IScenario
    {
        public BattleScene() : base() { }

        protected override void InitCommands()
        {
            AddCommand(new Command("攻击", "Attack", "A", DoAttack), 0);
            AddCommand(new Command("查看", "Check", "C", DoCheck), 0);

            AddCommand(new Command("返回", "Back", "B", DoBack), 1);
        }

        private void DoCheck(object[] param)
        {
            if (param.Length == 0)
            {
                StringBuilder sb = null;
                SingletonFactory<BattleController>.Instance.DoPrintTeamInfo(ref sb);
                Console.WriteLine(sb.ToString());
            }
        }

        private void DoBack(object[] param)
        {
            ExitToScene(SCENARIO_TYPE.HOME);
        }

        private void DoAttack(object[] param)
        {
            StringBuilder log = null;
            if (!SingletonFactory<BattleController>.Instance.MBattleEnd)
            {
                int targetIndex = 0;
                try
                {
                    targetIndex = int.Parse(param[0].ToString()) - 1;
                }
                catch(Exception e)
                {}
                SingletonFactory<BattleController>.Instance.NextRound(targetIndex,ref log);
                Console.WriteLine(log);
            }

            if (!SingletonFactory<BattleController>.Instance.MBattleEnd)
            {
                ShowCommandList();
            }
            else
            {
                if (SingletonFactory<BattleController>.Instance.PlayerWin)
                {
                    SingletonFactory<UserInfo>.Instance.AddReward(SingletonFactory<BattleController>.Instance.RewardXP);
                }
                NextStep();
            }
            
        }

        protected override void ShowWordsBeforeCommands()
        {
            switch (m_Step)
            {
                case 1:

                    break;
            }
        }
    }
}
