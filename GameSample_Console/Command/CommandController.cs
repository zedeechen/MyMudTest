using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class CommandController
    {
        private List<Command> m_GlobalCommands;

        public void Init()
        {
            m_GlobalCommands = new List<Command>();
            m_GlobalCommands.Add(new Command("查看", "Look", "L", DoShowInfo));
            m_GlobalCommands.Add(new Command("向北走", "North", "N", MoveCommand.DoMove, (int)enmDirectionType.NORTH));
            m_GlobalCommands.Add(new Command("向南走", "South", "S", MoveCommand.DoMove, (int)enmDirectionType.SOUTH));
            m_GlobalCommands.Add(new Command("向东走", "East", "E", MoveCommand.DoMove, (int)enmDirectionType.EAST));
            m_GlobalCommands.Add(new Command("向西走", "West", "W", MoveCommand.DoMove, (int)enmDirectionType.WEST));

            m_GlobalCommands.Add(new Command("保存游戏", "Save", "S", DataCommand.DoSaveGame));
            m_GlobalCommands.Add(new Command("退出游戏", "Exit", "X", DataCommand.DoExit));
            m_GlobalCommands.Add(new Command("帮助", "Help", "H", DoHelp));
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

            if (SingletonFactory<MapController>.Instance.MapCommandList == null || !DoCommand(key, param, SingletonFactory<MapController>.Instance.MapCommandList))
            {
                DoCommand(key, param, m_GlobalCommands);
            }
        }

        public VOID_PARAM_DELEGATE DoSpecialCommandWithType(int type)
        {
            switch (type)
            {
                case 1:
                    return DataCommand.DoNewGame;
                case 2:
                    return DataCommand.DoLoadGame;
                case 3:
                    return MoveCommand.DoTeleportRoom;
                default:
                    return null;
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

        private void DoShowInfo(object[] param)
        {
            if (SingletonFactory<UserInfo>.Instance != null)
            {
                StringBuilder sb = null;
                SingletonFactory<UserInfo>.Instance.DoPrint(ref sb);
                Console.WriteLine(sb.ToString());
            }
        }

        private void DoHelp(object[] param)
        {

        }
    }
}
