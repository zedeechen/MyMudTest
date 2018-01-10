using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class GameUtil
    {
        public static Command ConvertParamsToCommand(string[] param)
        {
            if (param == null)
                return null;
            return new Command(param[0], param[0], param[1], SingletonFactory<CommandController>.Instance.DoSpecialCommandWithType(int.Parse(param[2])));
        }

        public static Room ConvertParamsToRoom(string[] param)
        {
            if (param == null || param.Length < 1)
                throw new Exception("[ConvertParamsToRoom] Illegal room param (at lease one elements to indicate room id)");

            try {
                Room room = new Room(int.Parse(param[0].ToString()));
                if (param.Length >= 2 && !string.IsNullOrEmpty(param[1]))
                {
                    room.SetDirections(param[1].Split(CSVUtilBase.SYMBOL_THIRD));
                }
                return room;
            }catch(Exception e)
            {
                throw new Exception("[ConvertParamsToRoom] Illegal room id - " + param[0].ToString());
            }

            return null;
        }
    }
}
