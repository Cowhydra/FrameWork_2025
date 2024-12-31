using System.Text;
using UnityEngine;
using TMPro;

public class UIFPS : MonoBehaviour
{
    public TextMeshProUGUI ScreenSizeLbl;
    public TextMeshProUGUI FPSLbl;

    private int _FrameCount;
    private int _FPS;
    private float _FrameTime;
    private float _ElapTime;

    private StringBuilder _SB = new StringBuilder(32);


    void Awake()
    {
        _FrameCount = 0;
        _FPS = 0;
        _FrameTime = 0f;
        _ElapTime = 0f;

        ScreenSizeLbl.text = string.Format("Screen Size : {0} X {1}", Screen.width, Screen.height);
        FPSLbl.text = "";
    }


    private void UpdateFPSLabel()
    {
        _SB.Clear();
        _SB.Append("FPS : ");
        _SB.Append(_FPS);

        FPSLbl.text = _SB.ToString();
    }


    void Update()
    {
        ++_FrameCount;

        _ElapTime += Time.unscaledDeltaTime;
        if (_ElapTime > 1f)
        {
            _FPS = _FrameCount;
            _FrameTime = (_ElapTime / _FPS) * 1000f;
            _FrameCount = 0;

            _ElapTime %= 1f;

            UpdateFPSLabel();
        }
    }
}
