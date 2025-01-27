using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    private UICommonMsg _CommonMsg;
    private UINoticeMsg _NoticeMsg;
    private UISystemMsg _SystemMsg;
    private UISliderMsg _SliderMsg;


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


    public void InitSliderMsg(string msg)
    {
        if (_SliderMsg == null)
        {
            return;
        }

        _SliderMsg.InitSliderMsg(msg);
    }


    public void ClearSliderMsg()
    {
        if (_SliderMsg == null)
        {
            return;
        }

        _SliderMsg.ClearSliderMsg();
    }


    public void ChangeSliderMsg(string msg)
    {
        if (_SliderMsg == null)
        {
            return;
        }

        _SliderMsg.ChangeSliderMsg(msg);
    }


    // 진행률 갱신
    public void UpdateSliderProgress(float rate)
    {
        if (_SliderMsg == null)
        {
            return;
        }

        _SliderMsg.UpdateProgress(rate);
    }
}