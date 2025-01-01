using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    private UICommonMsg _CommonMsg;
    private UINoticeMsg _NoticeMsg;
    private UISystemMsg _SystemMsg;


    public void ShowCommonMsg(string msg)
    {
        if (_CommonMsg == null)
        { 
            return;
        }

        _CommonMsg.ShowMessage(msg);
    }


    public void ShowCommonMsg(string msg, Color color)
    {
        if (_CommonMsg == null)
        {
            return;
        }

        _CommonMsg.ShowMessage(msg, color);
    }


    public void ShowErrorMsg(string msg)
    {
        ShowCommonMsg(msg, Color.red);
    }


    public void ShowNoticeMsg(string msg)
    {
        if (_NoticeMsg == null)
        {
            return;
        }

        _NoticeMsg.ShowMessage(msg);
    }


    public void ShowSystemMsg(string msg)
    {
        if (_SystemMsg == null)
        {
            return;
        }

        _SystemMsg.ShowMessage(msg);
    }


    public void ShowBroadcastMsg(string[] args = null)
    {
        
    }
}