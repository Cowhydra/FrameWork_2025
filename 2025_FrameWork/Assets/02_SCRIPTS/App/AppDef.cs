
public static partial class AppDef
{
    // 애셋 번들 사용 여부
    // 주의: const로 선언하지 말 것, 경고 엄청 발생함.
    public static readonly bool ASSET_BUNDLE_MODE
#if UNITY_EDITOR
        = false;
#else
        = true;
#endif
}