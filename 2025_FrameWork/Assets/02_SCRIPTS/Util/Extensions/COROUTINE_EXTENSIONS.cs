using UnityEngine;

public static class COROUTINE_EXTENTIONS
{
    public static void SafeStopCoroutine(this MonoBehaviour mb, ref Coroutine routine)
    {
        if (mb != null && routine != null)
        { 
            mb.StopCoroutine(routine);
            routine = null;
        }
    }
}
