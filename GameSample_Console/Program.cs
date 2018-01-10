using System;
using ZDMMO;

namespace GameSample
{
    class Program
    {
        static void Main(string[] args)
        {
            SingletonFactory<GameController>.Instance.InitConfigs();
            SingletonFactory<CommandController>.Instance.Init();

            SingletonFactory<MapController>.Instance.EnterMap(int.Parse(Properties.Resources.LoginMapID));//.EnterScenario(SCENARIO_TYPE.GLOBAL, 0);

            while (true)
            {
                 SingletonFactory<CommandController>.Instance.ProcessUserInput(Console.ReadLine());
            }
        }
    }
}
