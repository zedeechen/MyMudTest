using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class MoveCommand
    {
        public static enmCommandResult DoTeleportRoom(object[] param)
        {            
            if (SingletonFactory <MapController>.Instance.TryTeleport())
                return enmCommandResult.SUCCESS;
            return enmCommandResult.IGNORE;
        }

        public static enmCommandResult DoChangeMap(object[] param)
        {
            if (param == null || param.Length <= 0)
            {
                return enmCommandResult.FAILED;
            }

            if (SingletonFactory<MapController>.Instance.EnterMap(int.Parse(param[0].ToString())))
                return enmCommandResult.SUCCESS;
            return enmCommandResult.IGNORE;
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
