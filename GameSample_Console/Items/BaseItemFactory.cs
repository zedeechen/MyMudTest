using System;
using ZDMMO;

namespace GameSample
{
    public class BaseResourceFactory
    {
        internal static IBaseItem CreateResource(enmBaseResourceType type)
        {
            switch (type)
            {
                case enmBaseResourceType.GOLD:
                    return new Gold();
                case enmBaseResourceType.DIAMOND:
                    return new Diamond();
                case enmBaseResourceType.ENERGY:
                    return new Energy();
                default:
                    return null;
            }

        }
    }
}
