
public static partial class AppDef
{
    public static readonly bool ASSET_BUNDLE_MODE
#if UNITY_EDITOR
        = false;
#else
        = true;
#endif
}