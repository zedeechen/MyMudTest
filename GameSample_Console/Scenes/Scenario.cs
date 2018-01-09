using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public abstract class IScenario
    {
        private Dictionary<int,List<Command>> m_Commands;
        protected MapConfig m_MapConfig;
        protected int m_Step;

        protected int m_FixedMapId = 0;

        public IScenario()
        {
            m_Commands = new Dictionary<int, List<Command>>();
            m_Step = 0;
            InitCommands();
        }
        abstract protected void InitCommands();

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

        public virtual void OnEnter()
        {

        }

        protected void ExitToScene(SCENARIO_TYPE targetScene, int mapId = 0)
        {
            ResetScene();
            SingletonFactory<ScenarioController>.Instance.EnterScenario(targetScene, mapId);
        }

        protected virtual void ResetScene()
        {
            m_Step = 0;
            m_MapConfig = null;
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
