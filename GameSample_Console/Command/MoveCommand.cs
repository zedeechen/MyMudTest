using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSample
{
    public class MoveCommand
    {
        public static enmCommandResult DoTeleportRoom(object[] param)
        {
            if (param == null || param.Length <= 0)
            {
                return enmCommandResult.FAILED;
            }

            //TODO

            return enmCommandResult.SUCCESS;
        }

        public static enmCommandResult DoMove(object[] param)
        {
            if (param == null || param.Length < 0)
                return enmCommandResult.FAILED;

            enmDirectionType dir = (enmDirectionType)(int.Parse(param[0].ToString()));

            //TODO

            return enmCommandResult.SUCCESS;
        }
    }
}
