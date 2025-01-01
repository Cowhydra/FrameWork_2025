using D_F_Enum;
using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    private Flags _ABBLockFlags = new Flags();


    public bool IsABBLocked
    {
        get { return !_ABBLockFlags.IsClear; }
    }


    public void LockABB(ABBLockID id)
    {
        _ABBLockFlags.Set((int)id, true);
    }


    public void UnlockABB(ABBLockID id)
    {
        _ABBLockFlags.Set((int)id, false);
    }


    // 안드로이드 백 버튼 처리
    private void HandleABB()
    {
        if (!Input.GetKeyUp(KeyCode.Escape))
        { 
            return;
        }

        OnABB();
    }


    // 외부에서 호출하는 경우
    public void OnABB()
    {
        if (IsABBLocked)
        { 
            return;
        }

        if (OnABBInner(this.transform) == true)
        { 
            return;
        }

        // 게임 종료 상자 열기.
        OpenAppQuitBox();
    }


    private bool OnABBInner(Transform t)
    {
        if (t == null)
        { 
            return false;
        }

        if (t.childCount > 0)
        {
            for (int i = t.childCount - 1; i >= 0; --i)
            {
                if (OnABBInner(t.GetChild(i)) == true)
                { 
                    return true;
                }
            }
        }

        if (t.gameObject.activeInHierarchy && t.TryGetComponent(out UIWindow wnd) == true)
        { 
            return wnd.OnABB();
        }

        return false;
    }


    private void OpenAppQuitBox()
    {

    }
}