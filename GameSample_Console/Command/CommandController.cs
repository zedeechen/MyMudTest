using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public delegate enmCommandResult ENM_PARAM_DELEGATE(params object[] param);

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
            //m_GlobalCommands.Add(new Command("帮助", "Help", "H", DoHelp));
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

        public void BindCommand(string commParam, ref List<Command> m_Commands)
        {
            if (string.IsNullOrEmpty(commParam))
                return;

            string[] param = commParam.Split(CSVUtilBase.SYMBOL_SECOND);
            if (param.Length == 2)
            {
                switch (param[0].ToLower())
                {
                    case "list":
                        switch (param[1].ToLower())
                        {
                            case "race":
                                {
                                    RaceConfig conf;
                                    byte key;
                                    for (int i = 0, count = SingletonFactory<RaceConfig>.Instance.GetMaxId(1); i < count; i++)
                                    {
                                        key = (byte)(i + 1);
                                        conf = SingletonFactory<RaceConfig>.Instance.GetDataById(key);
                                        m_Commands.Add(new Command(conf.name, key.ToString(), null, CreateRoleCommand.DoChooseRace, key));
                                    }
                                }
                                break;
                            case "class":
                                {
                                    ClassConfig conf;
                                    byte key;
                                    for (int i = 0, count = SingletonFactory<ClassConfig>.Instance.GetMaxId(); i < count; i++)
                                    {
                                        key = (byte)(i + 1);
                                        conf = SingletonFactory<ClassConfig>.Instance.GetDataById(key);
                                        m_Commands.Add(new Command(conf.name, key.ToString(), null, CreateRoleCommand.DoChooseClass, key));
                                    }
                                }
                                break;
                        }
                        break;
                    case "info":

                        break;
                }
            }
            else {
                m_Commands.Add(GameUtil.ConvertParamsToCommand(param));
            }
        }

        public ENM_PARAM_DELEGATE DoSpecialCommandWithType(int type)
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

        private enmCommandResult DoShowInfo(object[] param)
        {
            if (SingletonFactory<UserInfo>.Instance.MTeamCreated != null)
            {
                StringBuilder sb = null;
                SingletonFactory<UserInfo>.Instance.DoPrint(ref sb);
                Console.WriteLine(sb.ToString());
                return enmCommandResult.SUCCESS;
            }
            return enmCommandResult.FAILED;
        }

        private void DoHelp(object[] param)
        {

        }
    }
}
