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

        private string m_DefaultFileName = null;

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

        

        //public DungeonInfo ExploringDungeon
        //{
        //    get;
        //    private set;
        //}
        //internal void EnterDungeon()
        //{
        //    ExploringDungeon = SingletonFactory<DungeonInfo>.Instance;
        //    ExploringDungeon.SetDungeon(1);
        //}

        internal void SaveGame(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                fileName = m_DefaultFileName;

            if (string.IsNullOrEmpty(fileName))
                fileName = "autosave";

            if (FileUtil.SaveData(string.Format(".\\{0}.txt", fileName), SingletonFactory<UserInfo>.Instance))
            {
                m_DefaultFileName = fileName;
            }
        }

        public void CreateNewGame()
        {
            SingletonFactory<UserInfo>.Destroy();
        }

        internal bool LoadGame(string fileName)
        {
            CreateNewGame();

            UserInfo info;
            if (!string.IsNullOrEmpty(fileName))
                fileName = m_DefaultFileName;

            if (string.IsNullOrEmpty(fileName))
                fileName = "autosave";

            info = FileUtil.LoadData<UserInfo>(string.Format(".\\{0}.txt", fileName));
            if (info != null)
            {
                SingletonFactory<UserInfo>.ResetInstance(info);
                m_DefaultFileName = fileName;
                return true;
            }
            return false;
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
