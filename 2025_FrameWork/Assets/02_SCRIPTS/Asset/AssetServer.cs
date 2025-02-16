using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private static List<AsyncOperationHandle> _loadedHandles = new List<AsyncOperationHandle>();


    #region 리소스 로드
    // 비동기 리소스 로드 메서드 -- 스테이지 별로 몬스터나.. 씬 혹은 스테이지마다 메모리에 로드 및 제거할 것들은 여기에 넣어도 된다.
    public static async Task<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        // 이미 로드된 리소스를 확인
        if (TotalResourceDict.TryGetValue(key, out UnityEngine.Object resource))
        {
            return resource as T; // 리소스가 이미 로드되어 있으면 반환
        }

        // 리소스가 로드되지 않았다면 비동기적으로 로드
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

        // 비동기적으로 로드가 완료될 때까지 기다림
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TotalResourceDict[key] = handle.Result; // 리소스를 Dictionary에 저장
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


    public static async Task<T> InstantiateAsync<T>(string key, Transform parent = null, bool pooling = false) where T : UnityEngine.Object
    {
        // LoadAsync로 비동기적으로 리소스를 로드
        GameObject prefab = await LoadAsync<GameObject>(key);

        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab: {key}");
            return null;
        }

        // 풀링 사용 여부 체크
        if (pooling)
        {
            return ObjectPool.Instance.Pop(prefab).GetComponent<T>();
        }

        // 풀링을 사용하지 않는 경우 인스턴스를 생성
        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go.GetComponent<T>();
    }


    #region 생성 
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
        GameObject prefab;
     
        if (TotalResourceDict.ContainsKey(path) == false)
        {
            prefab = LoadFromResources<GameObject>(path);
        }
        else
        {
            prefab = TotalResourceDict[path] as GameObject;
        }

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


    // 리소스 해제 메서드
    public static void Release(string key)
    {
        if (TotalResourceDict.TryGetValue(key, out UnityEngine.Object resource))
        {
            Addressables.Release(resource); // 메모리에서 해제
            TotalResourceDict.Remove(key); // Dictionary에서 제거
        }
    }
}
