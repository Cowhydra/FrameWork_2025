using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISafeArea : MonoBehaviour
{
    public bool EnableSafeArea = true;

    private void Awake()
    {
        if (EnableSafeArea)
        { 
            ResolutionUtil.ApplySafeArea(GetComponent<RectTransform>());
        }
    }
}
