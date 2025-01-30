using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


public class AssetBundleLoader : MonoBehaviour
{
    private IBundleLoaderOwner _Owner;
    private Coroutine _BundleDownLoadCoro;


    private void OnDisable()
    {
        StopBundlesDownLoadLogic();
    }



    public void StopBundlesDownLoadLogic()
    {
        this.SafeStopCoroutine(ref _BundleDownLoadCoro);
    }


    public void Initalize(IBundleLoaderOwner owner)
    {
        StopBundlesDownLoadLogic();

        _Owner = owner;
        _BundleDownLoadCoro = StartCoroutine(LoadAssetBundles());
    }


    private void GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR errNo)
    {
        if (_Owner != null)
        {
            _Owner.OnBundlerEnterError(errNo);
        }
    }


    private IEnumerator LoadAssetBundles()
    {
        if (_Owner == null)
        {
            yield break;
        }

        // 1. Addressables 초기화
        yield return InitializeAddressables();

        // 2. 카탈로그 업데이트 확인
        yield return UpdateCatalogs();

        // 3. 다운로드 크기 계산
        yield return CalculateDownloadSize();

        yield return new WaitUntil(() => _Owner.AgreeBundleDownLoad != D_F_Enum.E_BUNDLE_DOWNLOAD_STATE.NONE);

        if (_Owner.AgreeBundleDownLoad != D_F_Enum.E_BUNDLE_DOWNLOAD_STATE.AGREE)
        {
            GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.USER_CANCEL);
            yield break;
        }

        // 4. 다운로드 진행
        yield return DownloadBundles();

        // 5. 리소스 메모리 로드
        yield return LoadResourcesToMemory();

        // 6. 완료 처리 -> 지금은 비동기처리중..
        //_Owner.OnLoadToMemoryComplete();
    }

    // 1. Addressables 초기화
    private IEnumerator InitializeAddressables()
    {
        AsyncOperationHandle initHandle = Addressables.InitializeAsync(false);
        yield return initHandle;

        if (initHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Addressables initialized successfully!");
        }
        else
        {
            GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.INIT_ERR);
            Debug.LogError("Addressables initialization failed!");
        }

        Addressables.Release(initHandle);
    }


    // 2. 카탈로그 업데이트 확인
    private IEnumerator UpdateCatalogs()
    {
        AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        if (checkHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var catalogsToUpdate = checkHandle.Result;

            if (catalogsToUpdate != null && catalogsToUpdate.Count > 0)
            {
                Debug.Log($"Updating {catalogsToUpdate.Count} catalogs...");
                AsyncOperationHandle updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
                yield return updateHandle;

                if (updateHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Catalogs updated successfully!");
                }
                else
                {
                    GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.UPDATE_CATALOG_ERR);
                    Debug.LogError("Failed to update catalogs!");
                }
            }
            else
            {
                Debug.Log("No catalogs to update.");
            }
        }
        else
        {
            Debug.LogError("Failed to check for catalog updates!");
        }

        Addressables.Release(checkHandle);
    }


    // 3. 다운로드 크기 계산
    private IEnumerator CalculateDownloadSize()
    {
        AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(_Owner.BundleLabels);
        yield return sizeHandle;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            long totalSize = sizeHandle.Result;
            Debug.Log($"Total download size: {totalSize} bytes");
            _Owner.TotalDownLoadBundleSize = totalSize;
            _Owner.OnBundleSizeAction(totalSize);
        }
        else
        {
            Debug.LogError("Failed to calculate download size!");
            GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.ON_DOWNLOAD_ERR);
        }

        Addressables.Release(sizeHandle);
    }

    // 4. 번들 다운로드
    private IEnumerator DownloadBundles()
    {
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(_Owner.BundleLabels, Addressables.MergeMode.Union);
        while (!downloadHandle.IsDone)
        {
            long downloadedBytes = downloadHandle.GetDownloadStatus().DownloadedBytes;
            _Owner.OnBundleSizeAction(downloadedBytes);
            yield return null;
        }

        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Download completed successfully!");
        }
        else
        {
            GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.ON_DOWNLOAD_ERR);
            Debug.LogError("Download failed!");
        }
    }


    // 5. 리소스 메모리 로드
    //private IEnumerator LoadResourcesToMemory()
    //{
    //    AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync(_Owner.BundleLabels, Addressables.MergeMode.Union, typeof(UnityEngine.Object));
    //    yield return locationHandle;

    //    if (locationHandle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        int totalCount = locationHandle.Result.Count;
    //        int loadedCount = 0;

    //        foreach (var location in locationHandle.Result)
    //        {
    //            AsyncOperationHandle<UnityEngine.Object> loadHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(location.PrimaryKey,f);
    //            yield return loadHandle;

    //            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
    //            {
    //                loadedCount++;
    //                _Owner.OnLoadMemoryAction(location.PrimaryKey, loadedCount, totalCount);
    //                Debug.Log($"Loaded: {location.PrimaryKey}");
    //            }
    //            else
    //            {
    //                Debug.LogError($"Failed to load: {location.PrimaryKey}");
    //            }

    //            Addressables.Release(loadHandle);
    //        }
    //    }
    //    else
    //    {
    //        GetError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR.MEMORYLOAD_ERR);
    //        Debug.LogError("Failed to load resource locations!");
    //    }
    //}


    private async Task LoadResourcesToMemory()
    {
        var locationHandle = Addressables.LoadResourceLocationsAsync(_Owner.BundleLabels, Addressables.MergeMode.Union, typeof(UnityEngine.Object));
        await locationHandle.Task;

        if (!locationHandle.IsValid() || locationHandle.Status != AsyncOperationStatus.Succeeded || locationHandle.Result == null || locationHandle.Result.Count == 0)
        {
            Debug.LogError($"Failed to load resource locations! Error: {locationHandle.OperationException?.Message}");
            return;
        }

        Debug.Log($"Found {locationHandle.Result.Count} resources. Starting parallel load...");

        List<Task> loadTasks = new List<Task>();

        int totalCount = locationHandle.Result.Count;
        int loadedCount = 0; 

        foreach (var location in locationHandle.Result)
        {
            loadTasks.Add(LoadAssetAsync(location.PrimaryKey, totalCount, () => Interlocked.Increment(ref loadedCount)));
        }

        await Task.WhenAll(loadTasks);

        Debug.Log("All resources loaded!");
    }

    private async Task LoadAssetAsync(string primaryKey, int totalCount, Func<int> onLoaded)
    {
        var loadHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(primaryKey);
        await loadHandle.Task;

        int loadedCount = onLoaded();

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Loaded: {primaryKey} ({loadedCount}/{totalCount})");

            _Owner.OnLoadMemoryAction(primaryKey, loadedCount, totalCount); 
        }
        else
        {
            Debug.LogError($"Failed to load asset: {primaryKey} | Error: {loadHandle.OperationException?.Message}");
        }

        Addressables.Release(loadHandle);

        if (loadedCount == totalCount)
        {
            _Owner.OnLoadToMemoryComplete();
        }
    }
}