using UnityEngine;

public partial class SoundManager : SingletonObj<SoundManager>
{
    private AudioSource _AudioSrc_BGM = null;


    // BGM용 AudioSource 생성
    private void CreateAudioSourceForBGM()
    {
        GameObject go = new GameObject();
        go.name = "BGM_SOUND";

        Transform t = go.transform;
        t.SetParent(this.transform, false);
        t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        _AudioSrc_BGM = go.AddComponent<AudioSource>();
        InitAudioSource(ref _AudioSrc_BGM);
    }


    // BGM 재생
    public void PLAY_BGM(int bgmIndex)
    {
        if (AppOption.EnableBgmSound == false)
        {
            return;
        }

        try
        {
            if (_AudioSrc_BGM == null)
            {
                return;
            }

            _AudioSrc_BGM.Stop();

            _AudioSrc_BGM.clip = null;
            if (_AudioSrc_BGM.clip == null)
            {
                return;
            }

            _AudioSrc_BGM.loop = false; // HACK: 사장님께서 BGM은 진입시 한번만
            _AudioSrc_BGM.volume = 1.0f;
            _AudioSrc_BGM.Play();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    // BGM 멈추기
    public void STOP_BGM()
    {
        if (_AudioSrc_BGM == null)
        {
            return;
        }

        _AudioSrc_BGM.Stop();
    }
}