using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class Room
    {
        private List<Command> m_Commands;
        public IReadOnlyList<Command> CommandList { get { return m_Commands; } }
        private Dictionary<string, List<Objects>> m_Objects;
        private RoomConfig m_Config;

        private Dictionary<enmDirectionType, int> m_Directions;
        public int ID { get; private set; }
        public Room(int roomId)
        {
            m_Config = SingletonFactory<RoomConfig>.Instance.GetDataById(roomId);
            if (m_Config != null)
            {
                ID = roomId;

                InitCommands();
                InitObjects();
            }
        }

        private void InitCommands()
        {
            if (string.IsNullOrEmpty(m_Config.specialCommands))
            {
                return;
            }
            string[] commands = m_Config.specialCommands.Split(CSVUtilBase.SYMBOL_FOURTH);
            if (commands.Length > 0 && m_Commands == null)
                m_Commands = new List<Command>();

            m_Commands.Clear();
            Command command;
            for (int i = 0; i < commands.Length; ++i)
            {
                SingletonFactory<GameController>.Instance.SetCommand(commands[i], ref m_Commands);
                //command = GameUtil.ConvertParamsToCommand(commands[i].Split(CSVUtilBase.SYMBOL_SECOND));
                //if (command != null)
                //    m_Commands.Add(command);
            }
        }

        internal void SetDirections(string[] dirParams)
        {
            if (dirParams != null && dirParams.Length > 0)
            {
                if (m_Directions == null)
                    m_Directions = new Dictionary<enmDirectionType, int>();
                m_Directions.Clear();

                string[] dir;
                for (int i= 0;i < dirParams.Length;i++)
                {
                    dir = dirParams[i].Split(CSVUtilBase.SYMBOL_SIXTH);
                    m_Directions[(enmDirectionType)int.Parse(dir[0])] = int.Parse(dir[1]);
                }
            }

        }

        internal void ShowRoomInfo()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m_Config.desc);

            if (m_Objects != null)
            {
                for (int i = 0; i < m_Objects.Count; ++i)
                {
                    Console.WriteLine();
                }
            }

            if (m_Commands != null)
            {
                for (int i = 0; i < m_Commands.Count; ++i)
                {
                    m_Commands[i].DoPrint();
                }
            }

            if (m_Directions != null && m_Directions.Count > 0)
            {
                List<enmDirectionType> keys = m_Directions.Keys.ToList();
                Console.Write("Directions:");
                
                for (int i = 0;i < keys.Count; ++i)
                {
                    if (i > 0)
                        Console.Write(",");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(keys[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = defaultColor;
        }

        private void InitObjects()
        {
            if (string.IsNullOrEmpty(m_Config.objects))
            {
                return;
            }

        }
    }
}
