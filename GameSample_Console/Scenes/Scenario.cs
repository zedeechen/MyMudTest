using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public abstract class IScenario
    {
        private Dictionary<int,List<Command>> m_Commands;
        protected MapConfig m_MapConfig;
        protected RoomConfig m_currentRoomConfig;
        protected int m_Step;

        protected int m_FixedMapId = 0;

        public IScenario()
        {
            m_Commands = new Dictionary<int, List<Command>>();
            m_Step = 0;
            //InitCommands();
        }

        public void OnExit()
        {
            ResetScene();
        }

        //abstract protected void InitCommands();
        protected virtual void InitCommands() { }

        public void AttachMapById(int mapId)
        {
            if (m_FixedMapId != 0)
            {
                mapId = m_FixedMapId;
            }
            if (mapId != 0)
            {
                MapConfig config = SingletonFactory<MapConfig>.Instance.GetDataById(mapId);
                if (config != null)
                    m_MapConfig = config;
            }
        }

        public void EnterRoom(int targetRoomId)
        {
            ResetScene();
            m_currentRoomConfig = SingletonFactory<RoomConfig>.Instance.GetDataById(targetRoomId);

            InitObjects();
            InitSpecialCommands();

            ShowCommandList();
        }

        private void InitSpecialCommands()
        {
            if (string.IsNullOrEmpty(m_currentRoomConfig.specialCommands))
            {
                return;
            }
            if (m_currentRoomConfig.specialCommands == "Fixed")
            {
                InitCommands();
                return;
            }
            string[] commands = m_currentRoomConfig.specialCommands.Split(CSVUtilBase.SYMBOL_FOURTH);

            for (int i = 0;i < commands.Length;++i)
            {
                AddCommand(commands[i]);
            }
        }

        private void InitObjects()
        {

        }

        public void ExitToScene(SCENARIO_TYPE targetScene, int mapId = 0)
        {
            ResetScene();
            
            SingletonFactory<ScenarioController>.Instance.EnterScenario(targetScene, mapId);
        }

        protected virtual void ResetScene()
        {
            m_Step = 0;
            m_MapConfig = null;
            if (m_Commands != null)
                m_Commands.Clear();
        }

        public List<Command> CommandList
        {
            get {
                List<Command> _cmds;
                m_Commands.TryGetValue(m_Step, out _cmds);
                return _cmds;
            }
        }

        protected void AddCommand(Command command, int step = 0)
        {
            List<Command> _cmds;
            if (!m_Commands.TryGetValue(step, out _cmds))
            {
                _cmds = new List<Command>();
                m_Commands[step] = _cmds;
            }
            _cmds.Add(command);
        }

        protected void AddCommand (string commandParam)
        {
            string[] commandParams = commandParam.Split(CSVUtilBase.SYMBOL_SECOND);

            AddCommand(new Command(commandParams[0], commandParams[0], commandParams[1], SingletonFactory<ScenarioController>.Instance.DoSpecialCommandWithType(int.Parse(commandParams[2]))), int.Parse(commandParams[3]));
        }

        protected void NextStep()
        {
            m_Step++;
            ShowCommandList();
        }
        protected void PrevStep()
        {
            if (m_Step > 0)
            {
                m_Step--;
            }
            ShowCommandList();
        }

        public void ShowCommandList()
        {
            ShowWordsBeforeCommands();
            if (m_Commands != null)
            {
                List<Command> _cmds;

                if (m_Commands.TryGetValue(m_Step, out _cmds))
                {
                    
                    for (int i = 0; i < _cmds.Count; ++i)
                    {
                        Console.WriteLine(_cmds[i].DoPrint());
                    }
                    //DoCommand(Console.ReadLine());
                    
                }
            }
            ShowWordsAfterCommands();
        }
        protected virtual void ShowWordsBeforeCommands()
        {
        }
        protected virtual void ShowWordsAfterCommands()
        {
        }
    }
}
