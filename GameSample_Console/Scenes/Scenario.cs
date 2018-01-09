using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public abstract class IScenario
    {
        private Dictionary<int,List<Command>> m_Commands;
        protected int m_Step;

        public IScenario()
        {
            m_Commands = new Dictionary<int, List<Command>>();
            m_Step = 0;
            InitCommands();
        }

        abstract protected void InitCommands();
        protected void ExitToScene(SCENARIO_TYPE targetScene)
        {
            ResetScene();
            SingletonFactory<ScenarioController>.Instance.EnterScenario(targetScene);
        }

        protected virtual void ResetScene()
        {
            m_Step = 0;
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

        //public bool DoCommand(string s)
        //{
        //    if (m_Commands != null)
        //    {
        //        List<Command> _cmds;

        //        string[] arr = s.Split(' ');

        //        string key = arr[0];
        //        object[] param = new object[arr.Length - 1];
        //        for (int i = 1; i < arr.Length; ++i)
        //        {
        //            param[i - 1] = arr[i];
        //        }

        //        if (m_Commands.TryGetValue(m_Step, out _cmds))
        //        {
        //            for (int i = 0; i < _cmds.Count; ++i)
        //            {
        //                if (_cmds[i].TryOperateWithKey(key, param))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
