

using System.Collections.Generic;
using UnityEngine;

public static class AssetServer 
{
    // ���� �ε�� ���ҽ� 
    // �ϳ��� Dictionary ���� ������������ ���� �ʿ�� ����
    public static Dictionary<string, UnityEngine.Object> _resources { get; private set; } = new Dictionary<string, Object>();


    public static bool AddResource(string key, UnityEngine.Object obj)
    {
        if (_resources.ContainsKey(key) == true)
        {
            return false;
        }
        else
        {
            _resources.Add(key, obj);
            return true;
        }
    }


    #region ���ҽ� �ε�
    public static T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
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

        GameObject go = Object.Instantiate(prefab, parent);

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

        GameObject go = Object.Instantiate(resource, parent);

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

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return component;
    }
    #endregion
}