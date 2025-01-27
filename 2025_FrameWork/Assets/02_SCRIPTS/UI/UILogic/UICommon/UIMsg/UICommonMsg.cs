using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICommonMsg : MonoBehaviour
{
    private enum EState
    {
        READY,
        SHOW,
        FADE_OUT
    }

    public Image MsgBG;
    public TextMeshProUGUI MsgLbl;

    private Color _MsgColor;
    private Color _BGColor;
    private float _CurAlpha;

    private EState _State = EState.READY;
    private float _StateTime = 0f;

    private const float SHOW_TIME = 3f;
    private const float FADE_SPEED = 2f;


    public void ShowMessage(string message)
    {
        ShowMessage(message, Color.white);
    }


    public void ShowMessage(string message, Color color)
    {
        gameObject.SetActive(true);

        _MsgColor = color;
        _BGColor = new Color(1f, 1f, 1f, 1f);
        _CurAlpha = color.a;

        MsgLbl.text = message;
        MsgLbl.color = _MsgColor;
        MsgBG.color = _BGColor;

        ChangeState(EState.SHOW);
    }


    private void ChangeState(EState newState)
    {
        _State = newState;
        _StateTime = 0f;
    }


    private void Update()
    {
        if (_State == EState.SHOW)
        {
            PROCESS_SHOW(Time.deltaTime);
        }
        else if (_State == EState.FADE_OUT)
        {
            PROCESS_FADE_OUT(Time.deltaTime);
        }
    }


    private void PROCESS_SHOW(float deltaTime)
    {
        _StateTime += deltaTime;

        if (_StateTime > SHOW_TIME)
        {
            ChangeState(EState.FADE_OUT);
        }
    }


    private void PROCESS_FADE_OUT(float deltaTime)
    {
        _CurAlpha -= (Time.deltaTime * FADE_SPEED);
        if (_CurAlpha < 0f)
        {
            _CurAlpha = 0f;
        }

        _MsgColor.a = _CurAlpha;
        _BGColor.a = _CurAlpha;

        MsgLbl.color = _MsgColor;
        MsgBG.color = _BGColor;

        if (_CurAlpha <= 0f)
        {
            ChangeState(EState.READY);

            gameObject.SetActive(false);
        }
    }
}
