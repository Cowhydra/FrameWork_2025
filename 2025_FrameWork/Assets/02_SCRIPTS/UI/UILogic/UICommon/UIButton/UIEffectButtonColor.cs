using UnityEngine;
using System;

[Serializable]
public class UIButtonColor
{
    public bool UseOnOFFColor;
    public Color OnColor;
    public Color OffColor;
}


public class UIEffectButtonColor : MonoBehaviour
{
    [Header("On Color 가 디폴트 칼라")]
    public UIButtonColor _Color;

    public Color GetOffColor()
    {
        if (_Color != null)
        {
            return _Color.OffColor;
        }

        return Color.white;
    }


    public Color GetOnColor()
    {
        if (_Color != null)
        {
            return _Color.OnColor;
        }

        return Color.white;
    }


    public bool UseOnOffColor()
    {
        if (_Color != null)
        {
            return _Color.UseOnOFFColor;
        }

        return false;
    }
}
