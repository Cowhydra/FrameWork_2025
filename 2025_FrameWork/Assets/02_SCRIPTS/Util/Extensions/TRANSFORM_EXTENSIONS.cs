using UnityEngine;

public static class TRANSFORM_EXTENTIONS
{
    // 자식 전부 파괴
    public static void DestroyAllChildren(this Transform tr)
    {
        if (tr == null)
        { 
            return;
        }

        for (int idx = tr.childCount - 1; idx >= 0; --idx)
        {
            Transform childT = tr.GetChild(idx);
            if (childT != null)
            {
                childT.parent = null;    // 이게 중요함. Destroy() 했다고 해서 바로 삭제되는 게 아님.
                GameObject.Destroy(childT.gameObject);
            }
        }
    }


    // 이름으로 자식 찾기
    public static Transform FindChildByName(this Transform tr, string objName)
    {
        if (tr == null)
        { 
            return null;
        }

        if (tr.name.Equals(objName))
        { 
            return tr;
        }

        for (int i = 0, cnt = tr.childCount; i < cnt; ++i)
        {
            Transform retT = FindChildByName(tr.GetChild(i), objName);
            if (retT != null)
            { 
                return retT;
            }
        }

        return null;
    }


    #region 자식 붙이기

    // 자식 붙이기
    public static void AttachChild(this Transform tr, Transform childT, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (tr == null || childT == null)
        { 
            return;
        }

        childT.SetParent(tr, false);
        childT.localScale = scale;
        childT.SetLocalPositionAndRotation(pos, rot);
    }


    public static void AttachChild(this Transform tr, Transform childT)
    {
        if (tr == null || childT == null)
        { 
            return;
        }

        childT.SetParent(tr, false);
        childT.localScale = Vector3.one;
        childT.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    public static void AttachChildObj(this Transform tr, GameObject childObj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (tr == null || childObj == null)
        {
            return;
        }

        Transform t = childObj.transform;
        t.SetParent(tr, false);
        t.localScale = scale;
        t.SetLocalPositionAndRotation(pos, rot);
    }


    public static void AttachChildObj(this Transform tr, GameObject childObj)
    {
        if (tr == null || childObj == null)
        {
            return;
        }

        Transform t = childObj.transform;
        t.SetParent(tr, false);
        t.localScale = Vector3.one;
        t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    // UI 자식 붙이기
    public static void AttachChildUI(this RectTransform tr, RectTransform childT, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (tr == null || childT == null)
        {
            return;
        }

        childT.SetParent(tr, false);
        childT.localScale = scale;
        childT.localRotation = rot;
        childT.anchoredPosition = pos;
    }


    public static void AttachChildUI(this RectTransform tr, RectTransform childT)
    {
        if (tr == null || childT == null)
        {
            return;
        }

        childT.SetParent(tr, false);
        childT.localScale = Vector3.one;
        childT.localRotation = Quaternion.identity;
        childT.anchoredPosition = Vector3.zero;
    }


    public static void AttachChildUIObj(this RectTransform tr, GameObject childObj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (tr == null || childObj == null)
        {
            return;
        }

        RectTransform t = childObj.GetComponent<RectTransform>();
        t.SetParent(tr, false);
        t.localScale = scale;
        t.localRotation = rot;
        t.anchoredPosition = pos;
    }


    public static void AttachChildUIObj(this RectTransform tr, GameObject childObj)
    {
        if (tr == null || childObj == null)
        {
            return;
        }

        RectTransform t = childObj.GetComponent<RectTransform>();
        t.SetParent(tr, false);
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
        t.anchoredPosition = Vector3.zero;
    }
    #endregion
}
