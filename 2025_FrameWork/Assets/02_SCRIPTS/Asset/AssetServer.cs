using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static partial class AssetServer 
{
    // ���� �ε�� ���ҽ� 
    // �ϳ��� Dictionary ���� ������������ ���� �ʿ�� ����
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

    private static List<AsyncOperationHandle> _loadedHandles = new List<AsyncOperationHandle>();


    //�׻� �޸𸮿� �ε�Ǿ� �ϴ� ������ --> �����͵�  ��..
    public static void LoadMemoryAlways(string label)
    {

    }


    #region ���ҽ� �ε�
    // �񵿱� ���ҽ� �ε� �޼��� -- �������� ���� ���ͳ�.. �� Ȥ�� ������������ �޸𸮿� �ε� �� ������ �͵��� ���⿡ �־ �ȴ�.
    public static async Task<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        // �̹� �ε�� ���ҽ��� Ȯ��
        if (TotalResourceDict.TryGetValue(key, out UnityEngine.Object resource))
        {
            return resource as T; // ���ҽ��� �̹� �ε�Ǿ� ������ ��ȯ
        }

        // ���ҽ��� �ε���� �ʾҴٸ� �񵿱������� �ε�
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

        // �񵿱������� �ε尡 �Ϸ�� ������ ��ٸ�
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TotalResourceDict[key] = handle.Result; // ���ҽ��� Dictionary�� ����
            _loadedHandles.Add(handle);

            Debug.Log($"Loaded resource: {key}");
            Debug.Log($"loadedHandles Count: {_loadedHandles.Count}");

            return handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load resource: {key}");
            return null;
        }
    }


    public static T LoadFromResources<T>(string key) where T : UnityEngine.Object
    {
        GameObject prefab = Resources.Load<GameObject>($"{key}");

        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        return prefab as T;
    }


    public static async Task<GameObject> InstantiateAsync(string key, Transform parent = null, bool pooling = false)
    {
        // LoadAsync�� �񵿱������� ���ҽ��� �ε�
        GameObject prefab = await LoadAsync<GameObject>(key);

        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab: {key}");
            return null;
        }

        // Ǯ�� ��� ���� üũ
        if (pooling)
        {
            return ObjectPool.Instance.Pop(prefab);
        }

        // Ǯ���� ������� �ʴ� ��� �ν��Ͻ��� ����
        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    #region ���� 
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


    public static T Instantiate<T>(GameObject resource, Transform parent = null, bool pooling = false) where T:Component
    {
        if (resource == null)
        {
            Debug.LogError($"Prefab is Null");
            return default(T);
        }

        if (pooling)
        {
            return ObjectPool.Instance.Pop(resource) as T;
        }

        GameObject go = UnityEngine.Object.Instantiate(resource, parent);

        go.name = resource.name;

        return go as T;
    }


    public static bool InstantiateAtLoaded(string label, Transform parent = null, bool pooling = false)
    {
        if (TotalResourceDict.ContainsKey(label) == false)
        {
            return false;
        }

        GameObject resource = TotalResourceDict[label] as GameObject;

        if (resource == null)
        {
            Debug.LogError($"Prefab is Null");
            return false;
        }

        if (pooling)
        {
            return ObjectPool.Instance.Pop(resource);
        }

        GameObject go = UnityEngine.Object.Instantiate(resource, parent);

        go.name = resource.name;

        return go;
    }
    #endregion


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
        GameObject prefab = LoadFromResources<GameObject>(path);
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


    // ���ҽ� ���� �޼���
    public static void Release(string key)
    {
        if (TotalResourceDict.TryGetValue(key, out UnityEngine.Object resource))
        {
            Addressables.Release(resource); // �޸𸮿��� ����
            TotalResourceDict.Remove(key); // Dictionary���� ����
        }
    }
}
