using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    public Image FadeImage;


    private void Awake()
    {
        FadeImage.SetActiveEx(false);
    }


    public IEnumerator StartFade(Color color, bool isFadeIn, float fadeDuration)
    {
        FadeImage.SetActiveEx(true);

        float targetAlpha;
        float curAlpha;

        if (isFadeIn)
        {
            targetAlpha = 0f;
            curAlpha = 1f;
        }
        else
        {
            targetAlpha = 1f;
            curAlpha = 0f;
        }

        color.a = curAlpha;
        FadeImage.color = color;

        if (fadeDuration > 0f)
        {
            float deltaAlpha = (targetAlpha - curAlpha) / fadeDuration;

            float endTime = Time.unscaledTime + fadeDuration;
            while (Time.unscaledTime < endTime)
            {
                color.a += (deltaAlpha * Time.unscaledDeltaTime);
                FadeImage.color = color;

                yield return null;
            }
        }

        color.a = targetAlpha;
        FadeImage.color = color;

        if (isFadeIn)
        { 
            FadeImage.SetActiveEx(false);
        }
    }
}