using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample.Dungeon
{
    public class DungeonInfo
    {
        private List<EventInfo> mEvents;
        private byte mDungeonId;

        public int EventsCount { get { return mEvents.Count; } }

        public DungeonInfo()
        {

        }
        public void SetDungeon(byte dungeonId)
        {
            mDungeonId = dungeonId;

            DungeonConfig config = SingletonFactory<DungeonConfig>.Instance.GetDataById(mDungeonId);

            if (config != null)
            {
                mEvents = new List<EventInfo>();
                for (int i = 0; i < config.eventNum;i++)
                {
                    mEvents.Add(new EventInfo());
                }
            }
        }

        internal EventInfo GetEventAtIndex(int i)
        {
            if (i < mEvents.Count)
            {
                return mEvents[i];
            }
            return null;
        }
    }
}
