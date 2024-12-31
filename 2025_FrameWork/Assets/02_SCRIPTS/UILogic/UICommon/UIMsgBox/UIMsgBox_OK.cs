using UnityEngine;

public class UIMsgBox_OK : UIMsgBox
{
    public UIButton OKBtn;


    protected override void Awake()
    {
        base.Awake();

        OKBtn.onClick = OnOKBtnClick;
    }


    // OK 버튼 클릭시.
    private void OnOKBtnClick()
    {
        Close(BT_OK);
    }


    // 안드로이드 백 버튼 클릭시.
    public override bool OnABB()
    {
        Close(BT_OK);

        return true;
    }
}
