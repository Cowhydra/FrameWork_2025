using UnityEngine;

public static class COMPONENT_EXTENTIONS
{
    public static void SetActiveEx(this Component c, bool isActive)
    {
        if (c != null)
        { 
            if (c.gameObject.activeSelf != isActive)
            { 
                c.gameObject.SetActive(isActive);
            }
        }
    }


    public static bool IsActiveSelf(this Component c)
    {
        if (c != null)
        {
            return c.gameObject.activeSelf;
        }

        return false;
    }
}
