using System;
using System.Text;
using ZDMMO;

namespace GameSample
{
    public class EventInfo
    {
        private enmEventType mType;
        public EventInfo()
        {
            int res = GameLogic.Dice(1, 20, 0);
            Console.WriteLine("EventInfo " + res);
            if (res == 1)
                mType = enmEventType.TRAP;
            else if (res <= 10)
                mType = enmEventType.BATTLE;
            else if (res == 20)
                mType = enmEventType.CHEST;
            else
                mType = enmEventType.NOTHING;
        }

        public string EventType
        {
            get
            {
                return mType.ToString();
            }
        }
    }
}
