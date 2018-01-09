using System;
using System.Collections.Generic;
using ZDMMO;

namespace GameSample.Heroes
{
    [Serializable]
    public class BasePointModule
    {
        private PropGroup<int> mBasePoints;

        private HeroInfo mHeroInfo;

        public BasePointModule(HeroInfo info)
        {
            mHeroInfo = info;
            mBasePoints = new PropGroup<int>(null);//, "Base");
            
            info.AddBasePropRelation(mBasePoints);
        }

        #region Props
        public void InitBasePoints(Dictionary<enmPropType, int> points)
        {
            PropItem<int> pi;
            foreach (enmPropType type in points.Keys)
            {
                pi = mBasePoints.GetPropItem((int)type);
                if (pi == null)
                    mBasePoints.SetPropItem((int)type, points[type]);
                else
                    pi.SetValue(points[type]);
            }
        }

        public void AddBasePoint(enmPropType type, int addPoint = 1)
        {
            PropItem<int> pi = mBasePoints.GetPropItem((int)type);
            if (pi != null)
                pi.SetValue(pi.GetValue() + addPoint);
        }

        public void ReduceBasePoint(enmPropType type, int reducePoint = 1)
        {
            PropItem<int> pi = mBasePoints.GetPropItem((int)type);
            if (pi != null)
                pi.SetValue(pi.GetValue() - reducePoint);
        }

        public int GetBasePoint(enmPropType type)
        {
            PropItem<int> pi = mBasePoints.GetPropItem((int)type);
            if (pi != null)
                return pi.GetValue();
            return 0;
        }
        #endregion
    }
}
