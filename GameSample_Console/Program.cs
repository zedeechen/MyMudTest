using System;
using ZDMMO;

namespace GameSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigFactory.Create<ClassConfig>(ClassConfig.ID);
            ConfigFactory.Create<RaceConfig>(RaceConfig.ID);
            ConfigFactory.Create<DungeonConfig>(DungeonConfig.ID);
            ConfigFactory.Create<EquipConfig>(EquipConfig.ID);
            ConfigFactory.Create<NamesConfig>(NamesConfig.ID);

            SingletonFactory<ScenarioController>.Instance.InitScenarioes();
            SingletonFactory<ScenarioController>.Instance.EnterScenario(SCENARIO_TYPE.GLOBAL);

            while (true)
            {
                 SingletonFactory<ScenarioController>.Instance.ProcessUserInput(Console.ReadLine());
            }
        }
    }
}
