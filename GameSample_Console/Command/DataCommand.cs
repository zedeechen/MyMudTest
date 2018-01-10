using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDMMO;

namespace GameSample
{
    public class DataCommand
    {
        public static void DoNewGame(object[] param)
        {
            SingletonFactory<MapController>.Instance.EnterMap(int.Parse(Properties.Resources.CreateRoleMapID));
        }
        public static void DoLoadGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();

            if (SingletonFactory<GameController>.Instance.LoadGame(fileName))
                SingletonFactory<MapController>.Instance.EnterMap(int.Parse(Properties.Resources.DefaultGameMapID));
            //else
            //    ShowCommandList();
        }

        public static void DoSaveGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();
            SingletonFactory<GameController>.Instance.SaveGame(fileName);
        }

        public static void DoExit(object[] param)
        {
            Environment.Exit(0);
        }
    }
}
