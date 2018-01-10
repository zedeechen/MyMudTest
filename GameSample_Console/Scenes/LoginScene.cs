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
            //AddCommand(new Command("新的游戏", "New", "N", DoNewGame));
            //AddCommand(new Command("加载游戏", "Load", "L", DoLoadGame));
        }

        


    }
}
