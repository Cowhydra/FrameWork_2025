using UnityEngine;

public class UIMsgBox_YesNo : UIMsgBox
{
    public UIButton YesBtn;
    public UIButton NoBtn;


    protected override void Awake()
    {
        base.Awake();

        YesBtn.onClick = OnYesBtnClick;
        NoBtn.onClick = OnNoBtnClick;
    }


    // YES 버튼 클릭시.
    private void OnYesBtnClick()
    {
        Close(BT_YES);
    }


    // NO 버튼 클릭시.
    private void OnNoBtnClick()
    {
        Close(BT_NO);
    }


    // 안드로이드 백 버튼 클릭시.
    public override bool OnABB()
    {
        Close(BT_NO);

        return true;
    }
}