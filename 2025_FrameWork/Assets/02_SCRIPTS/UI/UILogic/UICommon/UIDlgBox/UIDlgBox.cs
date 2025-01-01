using D_F_Enum;

public class UIDlgBox : UIWindow
{
    public IDlgBoxOwner Owner;
    public E_DLG_ID DlgID { get; protected set; }
    public object DlgValue { get; protected set; }

    public bool IsOpened { get; protected set; }

    protected System.Action<UIDlgBox, int> _OnClose;


    public virtual bool IsPage
    {
        get { return false; }
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        Messenger<UIDlgBox, bool>.Broadcast(MsgID.DLG_BOX_ENABLED, this, true);
    }


    protected override void OnDisable()
    {
        Messenger<UIDlgBox, bool>.Broadcast(MsgID.DLG_BOX_ENABLED, this, false);

        base.OnDisable();
    }


    public void Open(IDlgBoxOwner owner, E_DLG_ID dlgID, object dlgVal, System.Action<UIDlgBox, int> onClose)
    {
        Owner = owner;
        _OnClose = onClose;

        DlgID = dlgID;
        DlgValue = dlgVal;

        IsOpened = true;

        gameObject.SetActive(true);

        OnOpen();
    }


    public void Reopen()
    {
        IsOpened = true;

        if (gameObject.activeSelf)
        { 
            return;
        }

        gameObject.SetActive(true);

        OnReopen();
    }


    public void Hide()
    {
        IsOpened = false;

        if (gameObject.activeSelf == false) 
        {
            return;
        }

        gameObject.SetActive(false);

        OnHide();
    }


    public void Close(int param)
    {
        IsOpened = false;

        OnClose(param);

        if (_OnClose != null)
        {
            try
            { 
                _OnClose.Invoke(this, param);
            }
            finally
            { 
                _OnClose = null;
            }
        }

        gameObject.SafeDestroy();
    }


    protected virtual void OnOpen() { }
    protected virtual void OnReopen() { }

    protected virtual void OnHide() { }
    protected virtual void OnClose(int param) { }


    // 안드로이드 백 버튼 클릭시.
    public override bool OnABB()
    {
        Close(0);

        return true;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (Owner != null)
        {
            Owner.OnDestoryed();
        }

    }
}
