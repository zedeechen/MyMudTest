using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample.Dungeon
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

        internal string DoEvent()
        {
            StringBuilder sb;
            switch (mType)
            {
                case enmEventType.BATTLE:
                    sb = new StringBuilder();

                    SingletonFactory<BattleController>.Instance.InitBattle();

                    while (!SingletonFactory<BattleController>.Instance.MBattleEnd)
                    {
                        SingletonFactory<BattleController>.Instance.NextRound(ref sb);
                    }

                    return sb.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}
