using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public sealed class CreateRoleScene : IScenario
    {
        private HeroInfo mInfo;

        public CreateRoleScene() : base()
        {
        }

        private Dictionary<enmPropType, int> _bp = new Dictionary<enmPropType, int>();

        protected override void ShowWordsBeforeCommands()
        {
            switch (m_Step)
            {
                case 0:
                    Console.WriteLine("选择种族");
                    break;
                case 1:
                    Console.WriteLine("选择职业");
                    break;
                case 2:
                    {
                        StringBuilder sb = null;
                        mInfo.DoPrint(ref sb);
                        Console.WriteLine(sb.ToString());
                    }
                    break;
            }
        }

        protected override void InitCommands()
        {
            {
                RaceConfig conf;
                byte key;
                for (int i = 0, count = SingletonFactory<RaceConfig>.Instance.GetMaxId(1); i < count; i++)
                {
                    key = (byte)(i + 1);
                    conf = SingletonFactory<RaceConfig>.Instance.GetDataById(key);
                    AddCommand(new Command(conf.name, key.ToString(), null, DoChooseRace, key), 0);
                }
            }

            {
                ClassConfig conf;
                byte key;
                for (int i = 0, count = SingletonFactory<ClassConfig>.Instance.GetMaxId(); i < count; i++)
                {
                    key = (byte)(i + 1);
                    conf = SingletonFactory<ClassConfig>.Instance.GetDataById(key);
                    AddCommand(new Command(conf.name, key.ToString(), null, DoChooseClass, key), 1);
                }
            }

            //AddCommand(new Command("分配点数", "Arrange", "A", DoArrangePoints), 2);
            AddCommand(new Command("完成创建", "Finish","F", DoneCreateHero), 2);
            AddCommand(new Command("重新投点", "Reroll","R", DoReRoll), 2);
            AddCommand(new Command("重新创建", "Back","B", DoChoose), 2);
        }

        //private void DoArrangePoints(object[] param)
        //{
        //    int val;
        //    List<int> tempVals = new List<int>(_bp.Values);
        //    List<int> targetVals = new List<int>();
        //    for (int i = 0;i < param.Length;++i)
        //    {
        //        val = int.Parse(param[i].ToString());
        //        if (tempVals[0] != val)
        //        {
        //            for (int j = 1;j < tempVals.Count;++j)
        //            {
        //                if (tempVals[j] == val)
        //                {
        //                    tempVals[j] = tempVals[0];
        //                    targetVals.Add(val);
        //                    break;
        //                }
        //            }
        //            if (targetVals.Count <= i)//not found
        //            {
        //                ShowCommandList();
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            targetVals.Add(val);
        //        }
        //        tempVals.RemoveAt(0);
        //    }
        //    for (int i = 0;i < tempVals.Count; ++i)
        //    {
        //        targetVals.Add(tempVals[i]);
        //    }
        //    RearrangePoints(targetVals);
        //    ShowCommandList();
        //    return;
        //}

        //private void RearrangePoints(List<int> vals)
        //{
        //    for (int i = 0;i < vals.Count; ++i)
        //    {
        //        _bp[(enmPropType)(i + 1)] = vals[i];
        //    }
        //    mInfo.InitBasePoints(_bp);
        //}
        

        private void DoChooseRace(object[] param)
        {
            if (mInfo == null)
                mInfo = new HeroInfo();

            byte raceId = 0;
            try
            {
                raceId = byte.Parse(param[0].ToString());
            }catch(Exception e)
            {
            }

            mInfo.SetRace(raceId);

            NextStep();
        }

        private void DoChooseClass(object[] param)
        {
            byte classId = 0;
            try
            {
                classId = byte.Parse(param[0].ToString());
            }
            catch (Exception e)
            {
            }

            mInfo.ResetAllClassLevel();
            mInfo.SetClassLevel(classId, 1);

            Roll();
        }

        private void Roll()
        { 
            _bp = new Dictionary<enmPropType, int>();
            for(enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; ++type)
            {
                _bp[type] = GameLogic.Dice(3,6);
            }
            mInfo.InitBasePoints(_bp);

            NextStep();
        }

        private void DoneCreateHero(object[] param)
        {
            SingletonFactory<GameController>.Instance.AddHero(mInfo);
            ExitToScene(SCENARIO_TYPE.SAFETY_AREA);
        }

        private void DoReRoll(object[] param)
        {
            m_Step -= 1;
            Roll();
        }

        private void DoChoose(object[] param)
        {
            m_Step = 0;
            ShowCommandList();
        }

        protected override void ResetScene()
        {
            base.ResetScene();
            if (_bp != null)
            {
                _bp.Clear();
                _bp = null;
            }
        }
    }
}
