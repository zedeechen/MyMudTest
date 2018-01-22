using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample
{
    public class CreateRoleCommand : ICommandGroup
    {
        private HeroInfo mCreatingHero;
        private Dictionary<enmPropType, int> _bp;
        private List<Command> mTempCommand;

        protected override void RegisterCommands()
        {
            RegisterCommand("crc.cr", DoChooseRace);
            RegisterCommand("crc.cc", DoChooseClass);
            RegisterCommand("crc.roll", DoRoll);
            RegisterCommand("crc.pr", DoPrint);
            RegisterCommand("crc.lr", DoGenerateRaceList);
            RegisterCommand("crc.lc", DoGenerateClassList);
        }

        private enmCommandResult DoGenerateClassList(object[] param)
        {
            if (mTempCommand == null)
                mTempCommand = new List<Command>();
            else
                mTempCommand.Clear();

            ClassConfig conf;
            byte key;
            Command command, commandOnSuccess = null;

            for (int i = 0, count = SingletonFactory<ClassConfig>.Instance.GetMaxId(); i < count; i++)
            {
                key = (byte)(i + 1);
                conf = SingletonFactory<ClassConfig>.Instance.GetDataById(key);
                command = new Command(conf.name, key.ToString(), null, "crc.cc", key);
                command.SetCommandOnSucess(commandOnSuccess);
                mTempCommand.Add(command);

                command.DoPrint();

                RegisterCommand(key.ToString(), DoExecuteTempCommands);
            }
            return enmCommandResult.IGNORE;
        }

        private enmCommandResult DoExecuteTempCommands(object[] param)
        {
            if (param != null && param.Length > 0 && mTempCommand != null)
            {
                int index = int.Parse(param[0].ToString());

                mTempCommand[index].Execute(null);
            }
            return enmCommandResult.IGNORE;
        }

        private enmCommandResult DoGenerateRaceList(object[] param)
        {
            if (mTempCommand == null)
                mTempCommand = new List<Command>();
            else
                mTempCommand.Clear();

            RaceConfig conf;
            byte key;
            Command command, commandOnSuccess = null;

            for (int i = 0, count = SingletonFactory<RaceConfig>.Instance.GetMaxId(1); i < count; i++)
            {
                key = (byte)(i + 1);
                conf = SingletonFactory<RaceConfig>.Instance.GetDataById(key);
                command = new Command(conf.name, key.ToString(), null, "crc.cr", key);
                command.SetCommandOnSucess(commandOnSuccess);
                mTempCommand.Add(command);

                command.DoPrint();

                RegisterCommand(key.ToString(), DoExecuteTempCommands);
            }
            return enmCommandResult.IGNORE;
        }

        private enmCommandResult DoChooseRace(object[] param)
        {
            if (mCreatingHero == null)
                mCreatingHero = new HeroInfo();

            byte raceId = 0;
            try
            {
                raceId = byte.Parse(param[0].ToString());
            }
            catch (Exception)
            {
                return enmCommandResult.FAILED;
            }

            mCreatingHero.SetRace(raceId);

            return enmCommandResult.SUCCESS;
        }

        private enmCommandResult DoChooseClass(object[] param)
        {
            byte classId = 0;
            try
            {
                classId = byte.Parse(param[0].ToString());
            }
            catch (Exception)
            {
                return enmCommandResult.FAILED;
            }

            mCreatingHero.ResetAllClassLevel();
            mCreatingHero.SetClassLevel(classId, 1);

            //Roll();
            return enmCommandResult.SUCCESS;
        }

        private enmCommandResult DoRoll(object[] param)
        {
            _bp = new Dictionary<enmPropType, int>();
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; ++type)
            {
                _bp[type] = GameLogic.Dice(3, 6);
            }
            mCreatingHero.InitBasePoints(_bp);

            return enmCommandResult.SUCCESS;
        }

        private enmCommandResult DoPrint(object[] param)
        {
            if (mCreatingHero != null)
            {
                mCreatingHero.DoPrint();
                return enmCommandResult.SUCCESS;
            }
            return enmCommandResult.IGNORE;
        }
    }
}
