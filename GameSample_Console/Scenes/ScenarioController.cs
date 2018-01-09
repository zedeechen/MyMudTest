using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public delegate void VOID_PARAM_DELEGATE(params object[] param);

    public class ScenarioController
    {
        private Dictionary<SCENARIO_TYPE, IScenario> m_Scenarioes;
        private IScenario m_currentScene;

        internal void EnterMap(int mapId, int defaultRoomId = 0)
        {
            if (m_currentScene != null)
                m_currentScene.OnExit();

            MapConfig config = SingletonFactory<MapConfig>.Instance.GetDataById(mapId);

            m_currentScene = CreateOrGetScene((SCENARIO_TYPE)config.sceneType);
            m_currentScene.AttachMapById(mapId);

            if (defaultRoomId == 0)
            {
                defaultRoomId = config.defaultRoomId;
            }
            m_currentScene.EnterRoom(defaultRoomId);
        }

        private List<Command> m_GlobalCommands;

        public void EnterScenario(SCENARIO_TYPE type, int mapId)
        {
            Console.Clear();
            if (m_Scenarioes != null)
            {
                m_Scenarioes.TryGetValue(type, out m_currentScene);
                if (m_currentScene != null)
                {
                    m_currentScene.AttachMapById(mapId);

                    m_currentScene.ShowCommandList();

                }
            }
        }

        public void ProcessUserInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;

            string[] arr = input.Split(' ');

            string key = arr[0];
            object[] param = new object[arr.Length - 1];
            for (int i = 1; i < arr.Length; ++i)
            {
                param[i - 1] = arr[i];
            }

            if (m_currentScene == null || !DoCommand(key, param, m_currentScene.CommandList))
            {
                DoCommand(key, param, m_GlobalCommands);
            }
        }

        private bool DoCommand(string key, object[] param, List<Command> commList)
        {
            if (commList == null)
                return false;

            for (int i = 0; i < commList.Count; ++i)
            {
                if (commList[i].TryOperateWithKey(key, param))
                {
                    return true;
                }
            }
            return false;
        }

        public void Init()
        {
            if (m_Scenarioes == null)
            {
                m_Scenarioes = new Dictionary<SCENARIO_TYPE, IScenario>();
            }
            else
            {
                m_Scenarioes.Clear();
            }

            for (SCENARIO_TYPE type = SCENARIO_TYPE.BEGIN + 1; type < SCENARIO_TYPE.END; ++type)
            {
                //CreateScene(type);
            }

            m_GlobalCommands = new List<Command>();
            m_GlobalCommands.Add(new Command("查看", "Look", "L", DoShowInfo));
            m_GlobalCommands.Add(new Command("向北走", "North", "N", DoMove, enmDirectionType.NORTH));
            m_GlobalCommands.Add(new Command("向南走", "South", "S", DoMove, enmDirectionType.SOUTH));
            m_GlobalCommands.Add(new Command("向东走", "East", "E", DoMove, enmDirectionType.EAST));
            m_GlobalCommands.Add(new Command("向西走", "West", "W", DoMove, enmDirectionType.WEST));

            m_GlobalCommands.Add(new Command("保存游戏", "Save", "S", DoSaveGame));
            m_GlobalCommands.Add(new Command("退出游戏", "Exit", "X", DoExit));
            m_GlobalCommands.Add(new Command("帮助", "Help", "H", DoHelp));
        }

        internal VOID_PARAM_DELEGATE DoSpecialCommandWithType(int type)
        {
            switch (type)
            {
                default:
                    return null;
            }
        }

        private void DoMove(object[] param)
        {
            enmDirectionType dir = (enmDirectionType)(int.Parse(param[0].ToString()));

            //TODO
        }

        private void DoShowInfo(object[] param)
        {
            if (SingletonFactory<UserInfo>.Instance != null)
            {
                StringBuilder sb = null;
                SingletonFactory<UserInfo>.Instance.DoPrint(ref sb);
                Console.WriteLine(sb.ToString());
            }
        }
        private void DoSaveGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();
            SingletonFactory<GameController>.Instance.SaveGame(fileName);
        }
        private void DoExit(object[] param)
        {
            Environment.Exit(0);
        }
        private void DoHelp(object[] param)
        {

        }

        private IScenario CreateOrGetScene(SCENARIO_TYPE type)
        {
            if (!m_Scenarioes.ContainsKey(type))
            {
                switch (type)
                {
                    case SCENARIO_TYPE.GLOBAL:
                        m_Scenarioes[type] = new LoginScene();
                        break;
                    case SCENARIO_TYPE.CREATE_ROLE:
                        m_Scenarioes[type] = new CreateRoleScene();
                        break;
                    case SCENARIO_TYPE.SAFETY_AREA:
                        m_Scenarioes[type] = new SafetyAreaScene();
                        break;
                    case SCENARIO_TYPE.BATTLE:
                        m_Scenarioes[type] = new BattleScene();
                        break;
                }
            }
            return m_Scenarioes[type];
        }
    }
}
