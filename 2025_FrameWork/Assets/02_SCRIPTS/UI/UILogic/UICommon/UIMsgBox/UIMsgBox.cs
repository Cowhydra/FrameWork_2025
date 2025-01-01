using System;
using UnityEngine;
using TMPro;

public partial class UIMsgBox : UIWindow
{
    public TextMeshProUGUI MsgLbl;

    public int UserData { get; protected set; }

    private Action<UIMsgBox, int> _OnClose;


    // 열기
    public virtual void Open(string msg, Color? msgColor, int userData, Action<UIMsgBox, int> onClose)
    {
        gameObject.SetActive(true);

        UserData = userData;
        _OnClose = onClose;

        // 메시지 설정
        SetMessageLabel(msg, msgColor);
    }


    // 닫기
    public void Close(int btnID = 0)
    {
        gameObject.SetActive(false);
        if (_OnClose != null)
        {
            try
            {
                _OnClose.Invoke(this, btnID);
            }
            finally
            {
                _OnClose = null;
            }
        }

        gameObject.SafeDestroy();
    }


    // 메시지 라벨 설정
    private void SetMessageLabel(string msg, Color? msgColor)
    {
        if (msgColor == null)
        {
            MsgLbl.text = msg;
            return;
        }

        Color c = msgColor.Value;
        byte r = (byte)(c.r * 255);
        byte g = (byte)(c.g * 255);
        byte b = (byte)(c.b * 255);

        MsgLbl.text = string.Format("<color=#{0:x2}{1:x2}{2:x2}>{3}</color>", r, g, b, msg);
    }
}
