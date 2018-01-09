namespace GameSample
{
    public enum SCENARIO_TYPE
    {
        BEGIN = 0,
        GLOBAL = 1,
        CREATE_ROLE = 2,
        SAFETY_AREA = 3,
        BATTLE = 4,
        END = 5,
    }

    public enum enmPropType
    {
        BP_MIN = 0,
        STR = 1,
        DEX = 2,
        INT = 3,
        CON = 4,
        WIS = 5,
        CHA = 6,
        BP_MAX = 7,

        AP_MIN = 100,
        HP = 101,
        ATTACK_BONUS = 102,
        ARMOR_CLASS = 103,
        SAVING_FORT = 104,
        SAVING_REFLECT = 105,
        SAVING_WILL = 106,
        SKILL_POINT = 107,
        AP_MAX = 108,
    };

    public enum enmEventType
    {
        NOTHING = 0,
        BATTLE = 1,
        CHEST = 2,
        TRAP = 3,
        MAX = 4
    }

    public enum enmBaseResourceType
    {
        GOLD = 1,
        DIAMOND = 2,
        ENERGY = 3,
    }
}
