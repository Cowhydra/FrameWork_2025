using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    public RectTransform CanvasRectTr { get; private set; }
    public RectTransform ObjPoolParent { get; private set; }

    public RectTransform SafeArea { get; private set; }

    public RectTransform ContentsParent { get; private set; }

    public RectTransform DlgBoxParent { get; private set; }
    public RectTransform MsgBoxParent { get; private set; }

    public RectTransform NotiWndParent { get; private set; }

    public GameObject TouchBlockObj { get; private set; }

    public RectTransform CriErrMsgBoxParent { get; private set; }
    public RectTransform AppQuitBoxParent { get; private set; }

    public static UIScene Current { get; private set; } = null;


    protected virtual void Awake()
    {
        // [CANVAS RECT_TRANSFORM]
        Canvas canvas = GetComponent<Canvas>();
        Debug.Assert(canvas != null, "Canvas not found!!");

        CanvasRectTr = canvas.GetComponent<RectTransform>();

        Transform t = GetComponent<Transform>();

        // [SAFE_AREA]
        SafeArea = t.Find("SafeArea") as RectTransform;
        Debug.Assert(SafeArea != null, "SafeArea not found!!");

        // [CONTENTS]
        ContentsParent = SafeArea.Find("Contents") as RectTransform;
        Debug.Assert(ContentsParent != null, "Contents not found!!");

        // [DLG_BOX]
        DlgBoxParent = SafeArea.Find("DlgBox") as RectTransform;
        Debug.Assert(DlgBoxParent != null, "DlgBox not found!!");

        // [MSG_BOX]
        MsgBoxParent = SafeArea.Find("MsgBox") as RectTransform;
        Debug.Assert(MsgBoxParent != null, "MsgBox not found!!");

        // [NOTIFICATION]
        NotiWndParent = SafeArea.Find("Notification") as RectTransform;
        Debug.Assert(NotiWndParent != null, "Notification not found!!");

        _CommonMsg = NotiWndParent.Find("CommonMsg")?.GetComponent<UICommonMsg>();
        Debug.Assert(_CommonMsg != null, "CommonMsg not found!!");

        _NoticeMsg = NotiWndParent.Find("NoticeMsg")?.GetComponent<UINoticeMsg>();
        Debug.Assert(_NoticeMsg != null, "NoticeMsg not found!!");

        _SystemMsg = NotiWndParent.Find("SystemMsg")?.GetComponent<UISystemMsg>();
        Debug.Assert(_SystemMsg != null, "SystemMsg not found!!");

        // [TOUCH_BLOCK_OBJ]
        TouchBlockObj = SafeArea.Find("TouchBlock")?.gameObject;
        Debug.Assert(TouchBlockObj != null, "TouchBlock not found!!");
        TouchBlockObj.SetActive(false);

        // [FADER]
        _Fader = SafeArea.Find("Fader")?.GetComponent<UIFader>();
        Debug.Assert(_Fader != null, "Fader not found!!");

        // [CRI_ERR_MSG_BOX]
        CriErrMsgBoxParent = SafeArea.Find("CriErrorMsgBox") as RectTransform;
        Debug.Assert(CriErrMsgBoxParent != null, "CriErrorMsgBox not found!!");
    }


    protected virtual void OnDestroy()
    {
        if (Current == this)
        { 
            Current = null;
        }
    }


    protected virtual void Update()
    {
        HandleABB();
    }


    // 터치 블럭 활성/비활성 (버튼 등 UI 못 누르게)
    public void ActivateTouchBlockObj(bool val)
    {
        if (TouchBlockObj.activeSelf != val)
        { 
            TouchBlockObj.SetActive(val);
        }
    }
}