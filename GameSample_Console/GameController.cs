using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public class GameController
    {
        private static GameController mInstance;
        public static GameController Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new GameController();

                return mInstance;
            }
        }

        internal void InitCommandGroups()
        {
            SingletonFactory<DataCommand>.Instance.Init();
            SingletonFactory<MoveCommand>.Instance.Init();
            SingletonFactory<CreateRoleCommand>.Instance.Init();

        }

        public void InitConfigs()
        {
            ConfigFactory.Create<ClassConfig>(ClassConfig.ID);
            ConfigFactory.Create<RaceConfig>(RaceConfig.ID);
            ConfigFactory.Create<DungeonConfig>(DungeonConfig.ID);
            ConfigFactory.Create<EquipConfig>(EquipConfig.ID);
            ConfigFactory.Create<NamesConfig>(NamesConfig.ID);
            ConfigFactory.Create<MapConfig>(MapConfig.ID);
            ConfigFactory.Create<RoomConfig>(RoomConfig.ID);
            ConfigFactory.Create<EventConfig>(EventConfig.ID);
        }

        

        public void CreateNewGame()
        {
            SingletonFactory<UserInfo>.Destroy();
        }

        

        internal void AddHero(HeroInfo newHero)
        {
            SingletonFactory<UserInfo>.Instance.AddHero(newHero);
            //Console.WriteLine(string.Format("HP:{0}, AB:{1}, AC:{2}",
            //    newHero.GetAdvProp(enmPropType.HP),
            //    newHero.GetAdvProp(enmPropType.ATTACK_BONUS),
            //    newHero.GetAdvProp(enmPropType.ARMOR_CLASS)));
        }
    }
}
