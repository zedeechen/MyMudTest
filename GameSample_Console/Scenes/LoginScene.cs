using ZDMMO;

namespace GameSample
{
    public sealed class LoginScene : IScenario
    {
        public LoginScene() : base()
        {
            m_FixedMapId = 1;
        }

        protected override void InitCommands()
        {
            AddCommand(new Command("新的游戏", "New", "N", DoNewGame));
            AddCommand(new Command("加载游戏", "Load", "L", DoLoadGame));
        }

        private void DoNewGame(object[] param)
        {
            SingletonFactory<ScenarioController>.Instance.EnterMap(2);
        }
        private void DoLoadGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();

            if (SingletonFactory<GameController>.Instance.LoadGame(fileName))
                SingletonFactory<ScenarioController>.Instance.EnterMap(3);
            else
                ShowCommandList();
        }


    }
}
