using GameSample.Dungeon;
using GameSample.Heroes;
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
            SingletonFactory<UIManager>.Instance.AddUI<frmDungeon>();
        }

        internal void SaveData()
        {
            FileUtil.SaveData(System.Windows.Forms.Application.StartupPath + "\\save.txt", SingletonFactory<UserInfo>.Instance);
        }

        public void CreateNewGame()
        {
            InitConfigs();

            SingletonFactory<UserInfo>.Destroy();
        }

        internal void TryLoadGame()
        {
            CreateNewGame();

            SingletonFactory<UserInfo>.ResetInstance(FileUtil.LoadData<UserInfo>(System.Windows.Forms.Application.StartupPath + "\\save.txt"));

            SingletonFactory<UIManager>.Instance.AddUI<frmHome>();
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
            SingletonFactory<UIManager>.Instance.CloseUI<frmDungeon>();
        }

        internal void EnterHome()
        {
            SingletonFactory<UIManager>.Instance.AddUI<frmHome>();
        }

        internal void FinishedCreatingRole()
        {
            SingletonFactory<UIManager>.Instance.CloseUI<frmCreateRole>();
            EnterHome();
        }

        
    }
}
