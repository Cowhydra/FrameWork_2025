

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class AssetServer 
{
    // 실제 로드된 리소스 
    // 하나의 Dictionary 으로 관리중이지만 나눌 필요는 있음
    public static Dictionary<string, UnityEngine.Object> TotalResourceDict { get; private set; } = new Dictionary<string, UnityEngine.Object>();


    public static bool AddResource(string key, UnityEngine.Object obj)
    {
        if (TotalResourceDict.ContainsKey(key) == true)
        {
            return false;
        }
        else
        {
            TotalResourceDict.Add(key, obj);
            return true;
        }
    }


    #region 리소스 로드
    public static T Load<T>(string key) where T : UnityEngine.Object
    {
        if (TotalResourceDict.TryGetValue(key, out UnityEngine.Object resource))
        {
            return resource as T;
        }

        return null;
    }


    public static GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        if (pooling)
        {
           return ObjectPool.Instance.Pop(prefab);
        }

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);

        go.name = prefab.name;
        return go;
    }


    public static GameObject Instantiate(GameObject resource, Transform parent = null, bool pooling = false)
    {
        if (resource == null)
        {
            Debug.LogError($"Prefab is Null");
            return null;
        }

        if (pooling)
        {
            return ObjectPool.Instance.Pop(resource);
        }

        GameObject go = UnityEngine.Object.Instantiate(resource, parent);

        go.name = resource.name;

        return go;
    }


    public static void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        if (ObjectPool.Instance.Push(go))
        {
            return;
        }

        go.SafeDestroy();
    }


    public static T InstantiateFromResource<T>(string path, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Resources.Load<GameObject>($"{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return default;
        }

        if (prefab.TryGetComponent(out T component) == false)
        {
            Debug.LogErrorFormat("AssetServer.InstantiateFromResource> Invalid prefab({0})!!", prefab.name);
            return default;
        }

        if (pooling)
        {
            ObjectPool.Instance.Pop(prefab);
            return component;
        }

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return component;
    }
    #endregion
}
