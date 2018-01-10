using System;
using System.Collections.Generic;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class MapController
    {
        private Dictionary<int, Map> m_Maps;
        private Map m_currentMap;
        
        public IReadOnlyList<Command> MapCommandList
        {
            get
            {
                if (m_currentMap != null)
                    return m_currentMap.CommandList;
                return null;
            }
        }

        internal bool TryTeleport()
        {
            if (m_currentMap != null)
            {
                return m_currentMap.TryTeleport();
            }
            return false;
        }

        internal void EnterRecordedMap()
        {
            if (SingletonFactory<UserInfo>.Instance.MRecordMapId == 0)
            {
                SingletonFactory<UserInfo>.Instance.MRecordMapId = 3;
                SingletonFactory<UserInfo>.Instance.MRecordRoomId = 3;
            }
            else
            {

            }
            EnterMap(SingletonFactory<UserInfo>.Instance.MRecordMapId, SingletonFactory<UserInfo>.Instance.MRecordRoomId);

        }

        internal bool EnterMap(int mapId, int defaultRoomId = 0)
        {
            if (m_Maps == null)
                m_Maps = new Dictionary<int, Map>();

            Map map;
            if (!m_Maps.TryGetValue(mapId, out map))
            {
                map = new Map();
                map.AttachMapId(mapId);

                m_Maps[mapId] = map;                
            }

            if (m_currentMap != map)
            {
                if (m_currentMap != null)
                    m_currentMap.QuitMap();
                m_currentMap = map;
                m_currentMap.EnterRoom();
                
            }
            else
            {
            }
            return true;
        }

        internal void ShowCurrentMapInfo()
        {
            if (m_currentMap != null)
            {
                m_currentMap.ShowInfo();
            }
        }
    }
}
