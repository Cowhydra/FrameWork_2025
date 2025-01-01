
public static partial class AppOption
{
    // 효과음
    private static bool _EnableEffectSound;

    public static bool EnableEffectSound
    {
        get { return _EnableEffectSound; }
        set
        {
            if (value != _EnableEffectSound)
            {
                _EnableEffectSound = value;
                PlayerPrefs.SetInt(TSPPK.OPT_ENABLE_EFFECT_SOUND, value ? 1 : 0);
            }
        }
    }


    // 배경음
    private static bool _EnableBgmSound;

    public static bool EnableBgmSound
    {
        get { return _EnableBgmSound; }
        set
        {
            if (value != _EnableBgmSound)
            {
                _EnableBgmSound = value;
                PlayerPrefs.SetInt(TSPPK.OPT_ENABLE_BGM_SOUND, value ? 1 : 0);
            }
        }
    }


    // 사운드 옵션들 로딩
    private static void LoadSoundOptions()
    {
        _EnableEffectSound = (PlayerPrefs.GetInt(TSPPK.OPT_ENABLE_EFFECT_SOUND, 1) == 1);
        _EnableBgmSound = (PlayerPrefs.GetInt(TSPPK.OPT_ENABLE_BGM_SOUND, 1) == 1);
    }
}
