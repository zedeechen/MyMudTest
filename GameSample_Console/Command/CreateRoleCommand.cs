using System;

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
            catch (Exception e)
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
            catch (Exception e)
            {
                return enmCommandResult.FAILED;
            }

            mCreatingHero.ResetAllClassLevel();
            mCreatingHero.SetClassLevel(classId, 1);

            //Roll();
            return enmCommandResult.SUCCESS;
        }
    }
}
