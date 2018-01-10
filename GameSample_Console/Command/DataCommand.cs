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
        private static string m_DefaultFileName = null;
        public static enmCommandResult DoNewGame(object[] param)
        {
            SingletonFactory<MapController>.Instance.EnterMap(int.Parse(Properties.Resources.CreateRoleMapID));
            return enmCommandResult.SUCCESS;
        }
        public static enmCommandResult DoLoadGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();

            if (LoadGame(fileName))
            {
                SingletonFactory<MapController>.Instance.EnterMap(int.Parse(Properties.Resources.DefaultGameMapID));
                return enmCommandResult.SUCCESS;
            }
            else
            {
                //    ShowCommandList();
                return enmCommandResult.FAILED;
            }

        }

        public static enmCommandResult DoSaveGame(object[] param)
        {
            string fileName = null;
            if (param.Length > 0)
                fileName = param[0].ToString();
            if (SaveGame(fileName))
            {
                return enmCommandResult.SUCCESS;
            }
            return enmCommandResult.FAILED;
        }

        public static enmCommandResult DoExit(object[] param)
        {
            Environment.Exit(0);
            return enmCommandResult.IGNORE;
        }

        private static bool SaveGame(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                fileName = m_DefaultFileName;

            if (string.IsNullOrEmpty(fileName))
                fileName = "autosave";

            if (FileUtil.SaveData(string.Format(".\\{0}.txt", fileName), SingletonFactory<UserInfo>.Instance))
            {
                m_DefaultFileName = fileName;
                return true;
            }
            return false;
        }

        private static bool LoadGame(string fileName)
        {
            SingletonFactory<GameController>.Instance.CreateNewGame();

            UserInfo info;
            if (!string.IsNullOrEmpty(fileName))
                fileName = m_DefaultFileName;

            if (string.IsNullOrEmpty(fileName))
                fileName = "autosave";

            info = FileUtil.LoadData<UserInfo>(string.Format(".\\{0}.txt", fileName));
            if (info != null)
            {
                SingletonFactory<UserInfo>.ResetInstance(info);
                m_DefaultFileName = fileName;
                return true;
            }
            return false;
        }
    }
}
