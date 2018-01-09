using System;
using ZDMMO;

namespace GameSample
{
    class Program
    {
        static void Main(string[] args)
        {
            SingletonFactory<GameController>.Instance.InitConfigs();

            SingletonFactory<ScenarioController>.Instance.InitScenarioes();
            SingletonFactory<ScenarioController>.Instance.EnterScenario(SCENARIO_TYPE.GLOBAL, 0);

            while (true)
            {
                 SingletonFactory<ScenarioController>.Instance.ProcessUserInput(Console.ReadLine());
            }
        }
    }
}
