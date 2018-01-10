using System;
using ZDMMO;

namespace GameSample
{
    public sealed class SafetyAreaScene : IScenario
    {
        public SafetyAreaScene() : base()
        {
        }

        protected override void InitCommands()
        {
            //AddCommand(new Command("探险", "Explore","E", DoExplore));
            //AddCommand(new Command("职介所", "Recruit","C", DoRecruit));
            //AddCommand(new Command("酒馆", "Inn","I", DoQuest));

        }

        private void DoQuest(object[] param)
        {
            
        }

        private void DoRecruit(object[] param)
        {

        }

        private void DoExplore(object[] param)
        {
            SingletonFactory<BattleController>.Instance.InitBattle();
            //ExitToScene(SCENARIO_TYPE.BATTLE);
        }
    }
}