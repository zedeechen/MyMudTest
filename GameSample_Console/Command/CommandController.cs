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
            //m_GlobalCommands.Add(new Command("查看", "Look", "L", DoShowInfo));
            //m_GlobalCommands.Add(new Command("向北走", "North", "N", MoveCommand.DoMove, (int)enmDirectionType.NORTH));
            //m_GlobalCommands.Add(new Command("向南走", "South", "S", MoveCommand.DoMove, (int)enmDirectionType.SOUTH));
            //m_GlobalCommands.Add(new Command("向东走", "East", "E", MoveCommand.DoMove, (int)enmDirectionType.EAST));
            //m_GlobalCommands.Add(new Command("向西走", "West", "W", MoveCommand.DoMove, (int)enmDirectionType.WEST));
            
            //m_GlobalCommands.Add(new Command("退出游戏", "Exit", "X", DataCommand.DoExit));
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

        private Dictionary<string, List<ENM_PARAM_DELEGATE>> m_SystemCommands;
        internal void RegisterSystemCommand(string key, ENM_PARAM_DELEGATE func)
        {
            if (m_SystemCommands == null)
                m_SystemCommands = new Dictionary<string, List<ENM_PARAM_DELEGATE>>();

            List<ENM_PARAM_DELEGATE> list_;
            if (!m_SystemCommands.TryGetValue(key, out list_))
            {
                list_ = new List<ENM_PARAM_DELEGATE>();
                m_SystemCommands[key] = list_;
            }

            list_.Add(func);
        }

        internal void UnregisterSystemCommand(string key, ENM_PARAM_DELEGATE func)
        {
            if (m_SystemCommands == null)
                m_SystemCommands = new Dictionary<string, List<ENM_PARAM_DELEGATE>>();

            List<ENM_PARAM_DELEGATE> list_;
            if (!m_SystemCommands.TryGetValue(key, out list_))
            {
                return;
            }
            list_.Remove(func);
        }

        internal enmCommandResult ExecuteCommand(string key, object[] param)
        {
            if (m_SystemCommands == null)
                return enmCommandResult.IGNORE;

            List<ENM_PARAM_DELEGATE> list_;
            if (!m_SystemCommands.TryGetValue(key, out list_) || list_ == null || list_.Count == 0)
            {
                return enmCommandResult.IGNORE;
            }

            return list_[list_.Count - 1](param);
        }

        public void BindCommand(string commParam, ref List<Command> m_Commands)
        {
            if (string.IsNullOrEmpty(commParam))
                return;

            string[] param = commParam.Split(CSVUtilBase.SYMBOL_THIRD);// commParam.Split(CSVUtilBase.SYMBOL_SECOND);

            if (param.Length <= 0)
                return;
            Command command, commandOnSuccess = null;
            if (param.Length >= 2)
            {
                string[] paramSuccess = param[1].Split(CSVUtilBase.SYMBOL_SECOND);
                if (paramSuccess.Length > 1)
                {
                    commandOnSuccess = new Command(null, null, null, paramSuccess[0], paramSuccess[1]);
                }
                else {
                    commandOnSuccess = new Command(null, null, null, paramSuccess[0], null);
                }
            }                   

            string[] commandParam = param[0].Split(CSVUtilBase.SYMBOL_SECOND);
            if (commandParam.Length == 2)
            {
                switch (commandParam[0].ToLower())
                {
                    case "list":
                        switch (commandParam[1].ToLower())
                        {
                            case "race":
                                {
                                    RaceConfig conf;
                                    byte key;
                                    for (int i = 0, count = SingletonFactory<RaceConfig>.Instance.GetMaxId(1); i < count; i++)
                                    {
                                        key = (byte)(i + 1);
                                        conf = SingletonFactory<RaceConfig>.Instance.GetDataById(key);
                                        //command = new Command(conf.name, key.ToString(), null, CreateRoleCommand.DoChooseRace, key);
                                        //command.SetCommandOnSucess(commandOnSuccess);
                                        //m_Commands.Add(command);
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
                                        //command = new Command(conf.name, key.ToString(), null, CreateRoleCommand.DoChooseClass, key);
                                        //command.SetCommandOnSucess(commandOnSuccess);
                                        //m_Commands.Add(command);
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            else {
                command = GameUtil.ConvertParamsToCommand(commandParam);
                command.SetCommandOnSucess(commandOnSuccess);
                m_Commands.Add(command);
            }
        }

        //internal void ProcessRoomPreProcess(string paramString)
        //{
        //    string[] params_ = paramString.Split(CSVUtilBase.SYMBOL_FOURTH);// SYMBOL_SECOND);
        //    for (int i = 0;i < params_.Length;++i)
        //    {
        //        string[] param_ = params_[i].Split(CSVUtilBase.SYMBOL_SECOND);
        //        if (param_.Length < 2)
        //            continue;
        //        switch (param_[0])
        //        {
        //            case "cr":
        //                switch (param_[1])
        //                {
        //                    case "roll":
        //                        CreateRoleCommand.Roll();
        //                        break;
        //                    case "info":
        //                        CreateRoleCommand.DoPrint();
        //                        break;
        //                }
        //                break;
        //        }
        //    }
        //}

        //public ENM_PARAM_DELEGATE DoSpecialCommandWithType(int type)
        //{
        //    switch (type)
        //    {
        //        case 1:
                    
        //            return DataCommand.DoNewGame;
        //        case 2:
        //            return DataCommand.DoLoadGame;
        //        //case 3:
        //        //    return MoveCommand.DoTeleportRoom;
        //        case 4:
        //            return MoveCommand.DoChangeMap;
        //        default:
        //            return null;
        //    }
        //}

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
            if (SingletonFactory<UserInfo>.Instance.MTeamCreated)
            {
                SingletonFactory<UserInfo>.Instance.DoPrint();

                return enmCommandResult.SUCCESS;
            }
            return enmCommandResult.IGNORE;
        }

        private enmCommandResult DoHelp(object[] param)
        {
            return enmCommandResult.IGNORE;
        }
    }
}
