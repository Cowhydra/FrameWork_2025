using UnityEngine;

public static class PlayerPrefs
{
    public static void SetInt(string key, int value)
    {
        Debug.Log($"PlayerPrefs.SetInt({key}, {value})");
        UnityEngine.PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key, int defaultValue)
    {
        int value = UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
        Debug.Log($"PlayerPrefs.GetInt({key}, {defaultValue}) return {value}");

        return value;
    }

    public static int GetInt(string key)
    {
        int value = UnityEngine.PlayerPrefs.GetInt(key);
        Debug.Log($"PlayerPrefs.GetInt({key}) return {value}");

        return value;
    }

    public static void SetFloat(string key, float value)
    {
        Debug.Log($"PlayerPrefs.SetFloat({key}, {value})");
        UnityEngine.PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key, float defaultValue)
    {
        float value = UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
        Debug.Log($"PlayerPrefs.GetFloat({key}, {defaultValue}) return {value}");

        return value;
    }

    public static float GetFloat(string key)
    {
        float value = UnityEngine.PlayerPrefs.GetFloat(key);
        Debug.Log($"PlayerPrefs.GetFloat({key}) return {value}");

        return value;
    }

    public static void SetString(string key, string value)
    {
        Debug.Log($"PlayerPrefs.SetString({key}, {value})");
        UnityEngine.PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key, string defaultValue)
    {
        string value = UnityEngine.PlayerPrefs.GetString(key, defaultValue);
        Debug.Log($"PlayerPrefs.GetString({key}, {defaultValue}) return {value}");

        return value;
    }

    public static string GetString(string key)
    {
        string value = UnityEngine.PlayerPrefs.GetString(key);
        Debug.Log($"PlayerPrefs.GetString({key}) return {value}");

        return value;
    }

    public static bool HasKey(string key)
    {
        bool ret = UnityEngine.PlayerPrefs.HasKey(key);
        Debug.Log($"PlayerPrefs.HasKey({key}) return {ret}");

        return ret;
    }

    public static void DeleteKey(string key)
    {
        Debug.Log($"PlayerPrefs.DeleteKey({key})");
        UnityEngine.PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        Debug.Log($"PlayerPrefs.DeleteAll()");
        UnityEngine.PlayerPrefs.DeleteAll();
    }

    public static void Save()
    {
        Debug.Log($"PlayerPrefs.Save()");
        UnityEngine.PlayerPrefs.Save();
    }
}