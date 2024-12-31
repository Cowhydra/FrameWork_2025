using UnityEngine;

public static class GAME_OBJECT_EXTENTIONS
{
    // GameObject 파괴
    public static void SafeDestroy(this GameObject go, float delay = 0f)
    {
        if (go != null)
        { 
            GameObject.Destroy(go, delay);
        }
    }


    // Layer 설정
    public static void SetLayerRecurs(this GameObject go, int layer)
    {
        if (go == null)
        { 
            return;
        }

        go.layer = layer;

        Transform t = go.transform;
        for (int i = 0, cnt = t.childCount; i < cnt; ++i)
        { 
            SetLayerRecurs(t.GetChild(i).gameObject, layer);
        }
    }


    #region 자식 붙이기

    // 자식 붙이기
    public static void AttachChildObj(this GameObject go, GameObject childObj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (go == null || childObj == null)
        { 
            return;
        }

        Transform t = childObj.transform;
        t.SetParent(go.transform, false);
        t.localScale = scale;
        t.SetLocalPositionAndRotation(pos, rot);
    }


    public static void AttachChildObj(this GameObject go, GameObject childObj)
    {
        if (go == null || childObj == null)
        { 
            return;
        }

        Transform t = childObj.transform;
        t.SetParent(go.transform, false);
        t.localScale = Vector3.one;
        t.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    // UI 자식 붙이기
    public static void AttachChildUIObj(this GameObject go, GameObject childObj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (go == null || childObj == null)
        {
            return;
        }

        RectTransform t = childObj.GetComponent<RectTransform>();
        t.SetParent(go.transform, false);
        t.localScale = scale;
        t.localRotation = rot;
        t.anchoredPosition = pos;
    }


    public static void AttachChildUIObj(this GameObject go, GameObject childObj)
    {
        if (go == null || childObj == null)
        {
            return;
        }

        RectTransform t = childObj.GetComponent<RectTransform>();
        t.SetParent(go.transform, false);
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.identity;
        t.anchoredPosition = Vector3.zero;
    }
    #endregion
}
