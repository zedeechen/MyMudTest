using System;
using System.Collections.Generic;

namespace GameSample
{
    public class CreateRoleCommand
    {
        private static HeroInfo mCreatingHero;

        public static enmCommandResult DoChooseRace(object[] param)
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

        public static enmCommandResult DoChooseClass(object[] param)
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

        private static Dictionary<enmPropType, int> _bp;
        public static void Roll()
        {
            _bp = new Dictionary<enmPropType, int>();
            for (enmPropType type = enmPropType.BP_MIN + 1; type < enmPropType.BP_MAX; ++type)
            {
                _bp[type] = GameLogic.Dice(3, 6);
            }
            mCreatingHero.InitBasePoints(_bp);
        }

        public static void DoPrint()
        {
            if (mCreatingHero != null)
                mCreatingHero.DoPrint();
        }
    }
}
