using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public delegate void VOID_PARAM_DELEGATE(params object[] param);

    public class ScenarioController
    {
        private Dictionary<int, Map> m_Maps;
        private Map m_currentMap;

        internal void EnterRecordedMap()
        {
            if (SingletonFactory<UserInfo>.Instance.MRecordMapId == 0)
            {
                SingletonFactory<UserInfo>.Instance.MRecordMapId = 3;
                SingletonFactory<UserInfo>.Instance.MRecordRoomId = 3;
            }
            else
            {

            }
            EnterMap(SingletonFactory<UserInfo>.Instance.MRecordMapId, SingletonFactory<UserInfo>.Instance.MRecordRoomId);

        }

        internal void EnterMap(int mapId, int defaultRoomId = 0)
        {
            if (m_Maps == null)
                m_Maps = new Dictionary<int, Map>();

            Map map;
            if (!m_Maps.TryGetValue(mapId, out map))
            {
                map = new Map();
                map.AttachMapId(mapId);

                m_Maps[mapId] = map;                
            }

            if (m_currentMap != map)
            {
                if (m_currentMap != null)
                    m_currentMap.QuitMap();
                m_currentMap = map;
                m_currentMap.EnterRoom();
            }
            else
            {
            }
        }

        private List<Command> m_GlobalCommands;
        
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

            if (m_currentMap == null || !DoCommand(key, param, m_currentMap.CommandList))
            {
                DoCommand(key, param, m_GlobalCommands);
            }
        }
        
        private bool DoCommand(string key, object[] param, IReadOnlyList<Command> commList)
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
            m_GlobalCommands = new List<Command>();
            m_GlobalCommands.Add(new Command("查看", "Look", "L", DoShowInfo));
            m_GlobalCommands.Add(new Command("向北走", "North", "N", MoveCommand.DoMove, (int)enmDirectionType.NORTH));
            m_GlobalCommands.Add(new Command("向南走", "South", "S", MoveCommand.DoMove, (int)enmDirectionType.SOUTH));
            m_GlobalCommands.Add(new Command("向东走", "East", "E", MoveCommand.DoMove, (int)enmDirectionType.EAST));
            m_GlobalCommands.Add(new Command("向西走", "West", "W", MoveCommand.DoMove, (int)enmDirectionType.WEST));

            m_GlobalCommands.Add(new Command("保存游戏", "Save", "S", DoSaveGame));
            m_GlobalCommands.Add(new Command("退出游戏", "Exit", "X", DoExit));
            m_GlobalCommands.Add(new Command("帮助", "Help", "H", DoHelp));
        }

        internal VOID_PARAM_DELEGATE DoSpecialCommandWithType(int type)
        {
            switch (type)
            {
                case 1:
                    return DoNewGame;
                case 2:
                    return DoLoadGame;
                case 3:
                    return MoveCommand.DoTeleportRoom;
                default:
                    return null;
            }
        }
        private void DoNewGame(object[] param)
        {
            SingletonFactory<ScenarioController>.Instance.EnterMap(int.Parse(Properties.Resources.CreateRoleMapID));
        }
        private void DoLoadGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();

            if (SingletonFactory<GameController>.Instance.LoadGame(fileName))
                SingletonFactory<ScenarioController>.Instance.EnterMap(int.Parse(Properties.Resources.DefaultGameMapID));
            //else
            //    ShowCommandList();
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
    }
}
