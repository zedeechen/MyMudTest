using System;
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

        public DungeonInfo ExploringDungeon
        {
            get;
            private set;
        }
        internal void EnterDungeon()
        {
            ExploringDungeon = SingletonFactory<DungeonInfo>.Instance;
            ExploringDungeon.SetDungeon(1);
        }

        internal void SaveGame(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                FileUtil.SaveData(".\\autosave.txt", SingletonFactory<UserInfo>.Instance);
            else
                FileUtil.SaveData(string.Format(".\\{0}.txt", fileName), SingletonFactory<UserInfo>.Instance);
        }

        public void CreateNewGame()
        {
            InitConfigs();

            SingletonFactory<UserInfo>.Destroy();
        }

        internal bool LoadGame(string fileName)
        {
            CreateNewGame();

            UserInfo info;
            if (string.IsNullOrEmpty(fileName))
                info = FileUtil.LoadData<UserInfo>(".\\autosave.txt");
            else
                info = FileUtil.LoadData<UserInfo>(string.Format(".\\{0}.txt", fileName));
            if (info != null)
            {
                SingletonFactory<UserInfo>.ResetInstance(info);
                return true;
            }
            return false;
        }

        private void InitConfigs()
        {
            ConfigFactory.Create<ClassConfig>(ClassConfig.ID);
            ConfigFactory.Create<RaceConfig>(RaceConfig.ID);
            ConfigFactory.Create<DungeonConfig>(DungeonConfig.ID);
            ConfigFactory.Create<EquipConfig>(EquipConfig.ID);
        }

        internal void AddHero(HeroInfo newHero)
        {
            SingletonFactory<UserInfo>.Instance.AddHero(newHero);
            Console.WriteLine(string.Format("HP:{0}, AB:{1}, AC:{2}",
                newHero.GetAdvProp(enmPropType.HP),
                newHero.GetAdvProp(enmPropType.ATTACK_BONUS),
                newHero.GetAdvProp(enmPropType.ARMOR_CLASS)));
        }

        internal void LeaveDungeon()
        {
        }

        internal void EnterHome()
        {
        }

        internal void FinishedCreatingRole()
        {
            EnterHome();
        }

        
    }
}
