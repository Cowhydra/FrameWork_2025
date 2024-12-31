using System.Collections;
using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    protected UIFader _Fader;


    public IEnumerator FadeIn(Color color, float fadeDuration)
    {
        if (_Fader == null)
        { 
            yield break;
        }

        yield return _Fader.StartFade(color, true, fadeDuration);
    }


    public IEnumerator FadeOut(Color color, float fadeDuration)
    {
        if (_Fader == null)
        { 
            yield break;
        }

        yield return _Fader.StartFade(color, false, fadeDuration);
    }
}