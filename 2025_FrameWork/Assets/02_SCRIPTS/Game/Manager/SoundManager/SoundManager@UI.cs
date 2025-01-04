using UnityEngine;

public partial class SoundManager : SingletonObj<SoundManager>
{
    private AudioSource[] _AudioSrc_UI = null;

    private const int MAX_NUM_AUDIO_SOURCES_FOR_UI = 3; // UI 사운드용 AudioSource 개수


    // UI 사운드용 AudioSource들 생성
    private void CreateAudioSourcesForUI()
    {
        Transform thisTr = GetComponent<Transform>();

        _AudioSrc_UI = new AudioSource[MAX_NUM_AUDIO_SOURCES_FOR_UI];
        for (int i = 0; i < MAX_NUM_AUDIO_SOURCES_FOR_UI; ++i)
        {
            GameObject go = new GameObject();
            go.name = string.Format("UI_SOUND_{0:D3}", (i + 1));

            Transform t = go.transform;
            t.SetParent(thisTr, false);
            t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            _AudioSrc_UI[i] = go.AddComponent<AudioSource>();
            InitAudioSource(ref _AudioSrc_UI[i]);
        }
    }


    // 사용 가능한 AudioSource 찾기
    private AudioSource FindAvailableAudioSourceForUI()
    {
        for (int i = 0, len = _AudioSrc_UI.Length; i < len; ++i)
        {
            AudioSource audioSrc = _AudioSrc_UI[i];
            if (audioSrc != null && audioSrc.isPlaying == false)
            {
                return audioSrc;
            }
        }

        return null;
    }


    // UI 사운드 재생
    public void PLAY_UI_SOUND(int soundID)
    {
        if (AppOption.EnableEffectSound == false)
        {
            return;
        }

        try
        {
            if (soundID <= 0)
            {
                return;
            }

            AudioSource audioSrc = FindAvailableAudioSourceForUI();
            if (audioSrc == null)
            {
                return;
            }

            if (audioSrc.clip == null)
            {
                return;
            }

            audioSrc.loop = false;
            audioSrc.volume = 1f;
            audioSrc.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    public void PLAY_UI_SOUND(AudioClip ac)
    {
        if (AppOption.EnableEffectSound == false)
        {
            return;
        }

        if (ac == null)
        {
            return;
        }

        try
        {
            AudioSource audioSrc = FindAvailableAudioSourceForUI();
            if (audioSrc == null)
            {
                return;
            }

            audioSrc.clip = ac;
            audioSrc.loop = false;
            audioSrc.volume = 1f;
            audioSrc.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}