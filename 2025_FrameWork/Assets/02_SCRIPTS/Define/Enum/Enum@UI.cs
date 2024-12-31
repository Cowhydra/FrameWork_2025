
namespace D_F_Enum
{
    // 대화상자 옵션
    public enum E_DLG_OPT : byte
    {
        DestoryAll,             // 현재 생성된 대화상자 '모두' 파괴
        DestroyCurrent,         // 현재 생성된 대화상자 '만' 파괴
        ShowCurrent,            // 현재 대화상자 그대로 놔둠
        HideCurrent,             // 현재 대화상자 숨김 [임시] 
    }
}
