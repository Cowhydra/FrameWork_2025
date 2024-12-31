#if UNITY_EDITOR
#define TS_ENABLE_DEBUG
#endif

#if !TS_ENABLE_DEBUG
using UnityEngine;
using System.Diagnostics;

public class Debug
{
    public static bool developerConsoleVisible
    {
        get { return UnityEngine.Debug.developerConsoleVisible; }
        set { UnityEngine.Debug.developerConsoleVisible = value; }
    }

    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    [Conditional("UNITY_EDITOR")]
    public static void ClearDeveloperConsole() { }

    [Conditional("UNITY_EDITOR")]
    public static void Break() { }
    
    [Conditional("UNITY_EDITOR")]
    public static void DebugBreak() { }


    [Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) { }


    [Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { }

    [Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) { }


    [Conditional("UNITY_EDITOR")]
    public static void Log(object message) { }

    [Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context) { }

    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args) { }

    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(Object context, string format, params object[] args) { }


    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message) { }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context) { }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args) { }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(Object context, string format, params object[] args) { }


    public static void LogError(object message) 
    {
#if UNITY_ANDROID
        // HACK: LogError는 Unity Crash Report에 안 찍히기 때문에 LogException으로 처리함
        UnityEngine.Debug.LogException(new System.Exception(message.ToString())); 
#endif
    }

    public static void LogError(object message, Object context)
    {
#if UNITY_ANDROID
        UnityEngine.Debug.LogException(new System.Exception(message.ToString()), context);
#endif
    }

    public static void LogErrorFormat(string format, params object[] args) 
    {
#if UNITY_ANDROID
        UnityEngine.Debug.LogException(new System.Exception(string.Format(format, args)));
#endif
    }

    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
#if UNITY_ANDROID
        UnityEngine.Debug.LogException(new System.Exception(string.Format(format, args)), context);
#endif
    }

    public static void LogException(System.Exception exception)
    {
#if UNITY_ANDROID
        UnityEngine.Debug.LogException(exception); 
#endif
    }

    public static void LogException(System.Exception exception, Object context)
    {
#if UNITY_ANDROID
        UnityEngine.Debug.LogException(exception, context); 
#endif
    }


    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition) { }

    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message) { }
}
#endif