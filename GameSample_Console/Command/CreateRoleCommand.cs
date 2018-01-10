using System;

namespace GameSample
{
    public class CreateRoleCommand
    {
        private static HeroInfo mCreatingHero;

        public static void DoChooseRace(object[] param)
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
            }

            mCreatingHero.SetRace(raceId);
        }

        public static void DoChooseClass(object[] param)
        {
            byte classId = 0;
            try
            {
                classId = byte.Parse(param[0].ToString());
            }
            catch (Exception e)
            {
            }

            mCreatingHero.ResetAllClassLevel();
            mCreatingHero.SetClassLevel(classId, 1);

            //Roll();
        }
    }
}
