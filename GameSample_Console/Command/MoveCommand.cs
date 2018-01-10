using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSample
{
    public class MoveCommand
    {
        public static void DoTeleportRoom(object[] param)
        {
            if (param == null || param.Length <= 0)
            {
                return;
            }

            //TODO
        }

        public static void DoMove(object[] param)
        {
            if (param == null || param.Length < 0)
                return;

            enmDirectionType dir = (enmDirectionType)(int.Parse(param[0].ToString()));

            //TODO
        }
    }
}
