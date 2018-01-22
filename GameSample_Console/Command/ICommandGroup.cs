using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public abstract class ICommandGroup
    {
        private Dictionary<string, ENM_PARAM_DELEGATE> m_RegisteredCommands;

        public void Init()
        {
            if (m_RegisteredCommands == null)
                m_RegisteredCommands = new Dictionary<string, ENM_PARAM_DELEGATE>();
            RegisterCommands();
        }

        ~ICommandGroup()
        {
            foreach(string key in m_RegisteredCommands.Keys)
            {
                SingletonFactory<CommandController>.Instance.UnregisterSystemCommand(key, m_RegisteredCommands[key]);
            }
            m_RegisteredCommands.Clear();
            m_RegisteredCommands = null;
        }

        protected void RegisterCommand(string key, ENM_PARAM_DELEGATE func)
        {
            m_RegisteredCommands[key] = func;
            SingletonFactory<CommandController>.Instance.RegisterSystemCommand(key, m_RegisteredCommands[key]);
        }

        protected abstract void RegisterCommands();
    }
}
