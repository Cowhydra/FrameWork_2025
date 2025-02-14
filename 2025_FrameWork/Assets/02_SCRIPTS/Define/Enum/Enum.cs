
namespace D_F_Enum
{
    public enum E_SIZE
    {
        Byte,
        KB, 
        MB,
        GB
    }


    public enum E_OBJECT_TYPE:byte
    {
        NONE=0,
        PLAYER=1,
        MONSTER=2,
        BOSS=4,
        PROJECTTILE=5,
        ITEM=6,
    }


    public enum E_ATTACK_TYPE : byte
    {
        NEAR=0,
        FAR=1,
        NONE=2,

    }


    public enum E_HIT_SORT : byte
    {
        NORMAL=0,
        FAR=1,
        MAGIC=2,
    }


    public enum E_BEATEN_SORT : byte
    {
        NORMAL = 0,
        CRITICAL = 1,
        FAR = 2,
        MAGIC = 3,
    }


    public enum E_BUNDLE_DOWNLOAD_STATE : byte
    {
        NONE=0,
        AGREE=1,
        DISAGREE=2,
    }


    public enum E_BUNDLE_DOWNLOAD_ERROR : int
    {
        NONE=0,
        INTERNET_ERR=-1,
        INIT_ERR=-2,
        UPDATE_CATALOG_ERR=-3,
        ON_DOWNLOAD_ERR=-4,
        MEMORYLOAD_ERR=-5,

        SAVE_STORAGE_NOT_ENOUGH=-998,
        USER_CANCEL=-999,

    }


    // PASSIVE(1) or SINGLE(2) or MULTI(3) or AREA(4)
    public enum E_SKILL_TYPE
    {
        NONE=0,
        PASSIVE=1,
        SINGLE=2,
        MULTI=3,
        AREA=4
    }



    public static class SCENE_NAME
    {
        public static string Start = "01_Start";
        public static string Patch = "02_Patch";
        public static string Game = "04_Game";
        public static string Lobby = "03_Looby";
    }


    public static class TAG
        {
        public static string MyPlayer = "MyPlayer";
        }
}
