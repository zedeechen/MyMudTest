﻿using System;
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
        private List<Command> m_CommandsOnEnter;
        public IReadOnlyList<Command> CommandList { get { return m_Commands; } }
        private Dictionary<string, List<Objects>> m_Objects;
        private RoomConfig m_Config;

        private Dictionary<enmDirectionType, int> m_Directions;
        public int MTeleportRoomId { get; private set; }

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
            if (commands.Length > 0)
            {
                if (m_Commands == null)
                    m_Commands = new List<Command>();
                if (m_CommandsOnEnter == null)
                    m_CommandsOnEnter = new List<Command>();
            }

            m_Commands.Clear();
            for (int i = 0; i < commands.Length; ++i)
            {
                SingletonFactory<CommandController>.Instance.BindCommand(commands[i], ref m_Commands, ref m_CommandsOnEnter);
            }
        }

        private void InitObjects()
        {
            if (string.IsNullOrEmpty(m_Config.objects))
            {
                return;
            }
            //TODO
        }

        internal void InitDirections(string[] dirParams)
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
                    if ((enmDirectionType)int.Parse(dir[0]) == enmDirectionType.TELEPORT)
                        MTeleportRoomId = int.Parse(dir[1]);
                    else
                        m_Directions[(enmDirectionType)int.Parse(dir[0])] = int.Parse(dir[1]);
                }
            }

        }

        internal void ShowRoomInfo()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            if (m_Config.desc.ToLower().IndexOf("**") >= 0)
            {
                //SingletonFactory<CommandController>.Instance.ProcessRoomPreProcess(m_Config.desc.Substring(2).ToLower());
            }
            else
            {
                Console.WriteLine(m_Config.desc);
            }

            if (m_CommandsOnEnter != null)
            {
                for (int i = 0;i < m_CommandsOnEnter.Count;++i)
                {
                    m_CommandsOnEnter[i].Execute(null);
                }
            }            

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

    }
}
