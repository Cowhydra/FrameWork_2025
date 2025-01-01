using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void Start() { }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    protected virtual void OnDestroy() { }

    // 안드로이드 백 버튼 클릭시.
    public virtual bool OnABB() { return false; }
}
