
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
        PROJECTTILE,
        ITEM,
    }


    public enum E_ATTACK_TYPE : byte
    {
        NEAR=0,
        FAR=1,
        NONE=2,

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
