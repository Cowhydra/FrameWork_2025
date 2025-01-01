using UnityEngine;

public partial class SoundManager : SingletonObj<SoundManager>
{
    private AudioSource[] _AudioSrc = null;


    private void Awake()
    {
        // AudioSource들 생성
        CreateAudioSources();

        // BGM용 AudioSource 생성
        CreateAudioSourceForBGM();

        // UI 사운드용 AudioSource들 생성
        CreateAudioSourcesForUI();
    }


    private void InitAudioSource(ref AudioSource audioSrc)
    {
        if (audioSrc == null)
        {
            return;
        }

        audioSrc.playOnAwake = false;
        audioSrc.spatialBlend = 0f;
        audioSrc.dopplerLevel = 0f;
        audioSrc.reverbZoneMix = 0f;
        audioSrc.rolloffMode = AudioRolloffMode.Linear;
        audioSrc.maxDistance = 10000f;
        audioSrc.minDistance = 9990f;
        audioSrc.volume = 1f;
    }


    // AudioSource들 생성
    private void CreateAudioSources()
    {
        Transform thisTr = GetComponent<Transform>();

        _AudioSrc = new AudioSource[10];
        for (int i = 0; i < 10; ++i)
        {
            GameObject go = new GameObject();
            go.name = string.Format("SOUND_{0:D3}", (i + 1));

            Transform t = go.transform;
            t.SetParent(thisTr, false);
            t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            _AudioSrc[i] = go.AddComponent<AudioSource>();
            InitAudioSource(ref _AudioSrc[i]);
        }
    }


    // 사용 가능한 AudioSource 찾기
    private AudioSource FindAvailableAudioSource()
    {
        for (int i = 0, len = _AudioSrc.Length; i < len; ++i)
        {
            AudioSource audioSrc = _AudioSrc[i];
            if (audioSrc != null && audioSrc.isPlaying == false)
            {
                return audioSrc;
            }
        }

        return null;
    }


    // 사운드 재생
    public void PLAY_SOUND(AudioClip audioClip, float volume = 1f)
    {
        if (AppOption.EnableEffectSound == false)
        {
            return;
        }

        try
        {
            AudioSource audioSrc = FindAvailableAudioSource();
            if (audioSrc == null)
            {
                return;
            }

            audioSrc.clip = audioClip;
            if (audioSrc.clip == null)
            {
                return;
            }

            audioSrc.loop = false;
            audioSrc.volume = volume;
            audioSrc.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    // 사운드 재생
    public void PLAY_SOUND(int soundID, float volume = 1f)
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

            AudioSource audioSrc = FindAvailableAudioSource();
            if (audioSrc == null)
            {
                return;
            }

            //클립로드
            if (audioSrc.clip == null)
            {
                return;
            }

            audioSrc.loop = false;
            audioSrc.volume = volume;
            audioSrc.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
