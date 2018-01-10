using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class Map
    {
        private Dictionary<int, Room> m_Rooms;
        private int m_DefaultRoomId;
        private Room m_CurrentRoom;

        public IReadOnlyList<Command> CommandList
        {
            get
            {
                if (m_CurrentRoom != null)
                    return m_CurrentRoom.CommandList;
                return null;
            }
        }

        public void AttachMapId(int mapId)
        {
            MapConfig config = SingletonFactory<MapConfig>.Instance.GetDataById(mapId);

            if (!string.IsNullOrEmpty(config.roomList))
            {
                string[] roomParams = config.roomList.Split(CSVUtilBase.SYMBOL_FOURTH);
                m_Rooms = new Dictionary<int, Room>();
                Room room;
                for (int i = 0;i < roomParams.Length; ++i)
                {
                    room = GameUtil.ConvertParamsToRoom(roomParams[i].Split(CSVUtilBase.SYMBOL_SECOND));
                    
                    if (room != null && room.ID != 0)
                    {
                        m_Rooms[room.ID] = room;
                    }
                }
            }
            m_DefaultRoomId = config.defaultRoomId;
        }

        public void EnterRoom(int roomId = 0)
        {
            if (roomId == 0)
                roomId = m_DefaultRoomId;
            Room room;
            if (m_Rooms.TryGetValue(roomId, out room))
            {
                m_CurrentRoom = room;
            }
            m_CurrentRoom.ShowRoomInfo();
        }

        internal void QuitMap()
        {
            
        }
    }
}
