using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;

public static partial class UIUtil
{
    public static void SetLabelColor(ref TextMeshProUGUI text,string msg,Color color)
    {
        if (text == null)
        {
            Debug.LogErrorFormat("TSUIUtil.SetLabelColor> text({0}) not found!!", text);
            return;
        }

        text.text = msg;
        text.color = color;
    }


    public static GameObject CreateUIGameObject(string name)
    {
        GameObject go = new GameObject(name);
        go.layer = Layer.UI;

        RectTransform tr = go.AddComponent<RectTransform>();
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.pivot = new Vector2(0.5f, 0.5f);

        return go;
    }
}