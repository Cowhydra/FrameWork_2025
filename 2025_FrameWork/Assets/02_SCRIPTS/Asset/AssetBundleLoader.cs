using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


//만들고싶은거 
//2. 업데이트 확인  번들 말고 ( 다른 방식 -> 강제 패치가 필요할 경우 ( 스토어 이동 , 게임 앞단 이동)
//3. 아직 리소스 받지 않은 컨텐츠의 경우 라벨별로 다운로드


public class AssetBundleLoader :MonoBehaviour
{
    public static string InitDownloadURL;
    private StateMachine _BundleState;
    private System.DateTime _EnterTime;
    AssetBundleLoader_Error _BundleError;// 에러상태는 외부에서도 호출 가능하도록
    AssetBundleLoader_DownloadSize _DownloadSize;
    public List<string> DownLoadLabels;

    //여기에 콜백 함수를 넣어줘야할 수 있음 그래야 외부에서 가능
    public void Initalize(IBundleLoaderOwner owner ) 
    {
        _EnterTime = System.DateTime.Now;
        DownLoadLabels = owner.BundleLabels;

        if (_BundleState == null)
        {
            _BundleState = new StateMachine();
        }

        if (_BundleError == null)
        {
            _BundleError = new AssetBundleLoader_Error("BundleError", owner.OnBundlerEnterError);
        }

        AssetBundleLoader_Init _BundleInit = new AssetBundleLoader_Init("BundleInit");
        AssetBundleLoader_CheckValid _BundleCheckValid = new AssetBundleLoader_CheckValid("BundleCheckValid");
        AssetBundleLoader_UpdateCatalog _UpdateCatalog = new AssetBundleLoader_UpdateCatalog("UpdateCatalog");

        if (_DownloadSize == null)
        {
            _DownloadSize = new AssetBundleLoader_DownloadSize("DownloadSize", DownLoadLabels, owner.OnBundleSizeAction);
        }

        AssetBundleLoader_DownloadBundles _DownloadBundles = new AssetBundleLoader_DownloadBundles("DownloadBundles", DownLoadLabels, owner.OnBundleDownLoadAction);
        AssetBundleLoader_LoadToMemory _LoadToMemory = new AssetBundleLoader_LoadToMemory("LoadToMemory", DownLoadLabels, owner.OnLoadMemoryAction);
        AssetBundleLoader_LoadToMemory_Complete _LoadToMemory_Complete = new AssetBundleLoader_LoadToMemory_Complete("LoadToMemory_Complete", owner.OnLoadToMemoryComplete);

        //초기화
        _BundleInit.Transitions.Add(new StateTransition(_BundleCheckValid, () => _BundleInit.StateID == E_STATE_ID.ERROR));
        _BundleInit.Transitions.Add(new StateTransition(_BundleError, () => _BundleInit.StateID == E_STATE_ID.QUIT));
        _BundleInit.Transitions.Add(new StateTransition(_BundleCheckValid, () => _BundleInit.StateID == E_STATE_ID.SUCCESS));

        //유효성체크 -> 실패시 초기화로 보낼줄 알아야함
        _BundleCheckValid.Transitions.Add(new StateTransition(_BundleInit, () => _BundleCheckValid.StateID == E_STATE_ID.ERROR));
        _BundleCheckValid.Transitions.Add(new StateTransition(_BundleError, () => _BundleCheckValid.StateID == E_STATE_ID.QUIT));
        _BundleCheckValid.Transitions.Add(new StateTransition(_UpdateCatalog, () => _BundleCheckValid.StateID == E_STATE_ID.SUCCESS));

        //카탈로그 다운로드
        _UpdateCatalog.Transitions.Add(new StateTransition(_BundleInit, () => _UpdateCatalog.StateID == E_STATE_ID.ERROR));
        _UpdateCatalog.Transitions.Add(new StateTransition(_BundleError, () => _UpdateCatalog.StateID == E_STATE_ID.QUIT));
        _UpdateCatalog.Transitions.Add(new StateTransition(_DownloadSize, () => _UpdateCatalog.StateID == E_STATE_ID.SUCCESS));

        //사이즈 다운로드
        _DownloadSize.Transitions.Add(new StateTransition(_BundleInit, () => _DownloadSize.StateID == E_STATE_ID.ERROR));
        _DownloadSize.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadSize.StateID == E_STATE_ID.SUCCESS));
        _DownloadSize.Transitions.Add(new StateTransition(_BundleError, () => _DownloadSize.StateID == E_STATE_ID.QUIT));

        //번들 다운로드
        _DownloadBundles.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadBundles.StateID == E_STATE_ID.ERROR));
        _DownloadBundles.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadBundles.StateID == E_STATE_ID.QUIT));
        _DownloadBundles.Transitions.Add(new StateTransition(_LoadToMemory, () => _DownloadBundles.StateID == E_STATE_ID.SUCCESS));

        //번들로드
        _LoadToMemory.Transitions.Add(new StateTransition(_BundleError, () => _LoadToMemory.StateID == E_STATE_ID.ERROR));
        _LoadToMemory.Transitions.Add(new StateTransition(_BundleError, () => _LoadToMemory.StateID == E_STATE_ID.QUIT));
        _LoadToMemory.Transitions.Add(new StateTransition(_LoadToMemory_Complete, () => _LoadToMemory.StateID == E_STATE_ID.SUCCESS));

        //상태머신 초기화
        _BundleState.SetInitialState(_BundleInit);
    }


    private void Update()
    {
        if (_BundleState == null)
        {
            return;
        }

        _BundleState.Update();

        //취소토큰 고려해볼 것 --> 넷이 끊기면 
        if (BundleUtil.IsNetworkValid() == false)
        {
            _BundleState.ForceChangeState(_BundleError);
            return;
        }

        //일정 시간이상 지날경우 번들 실패 처리
        if (TimeUtil.HasElapsed(_EnterTime, 600) == true)
        {
            _BundleState.ForceChangeState(_BundleError);

            //앱 재실행 같은로직 필요
            UIScene.Current.ShowCommonMsg("시간 초과 오류");
        }

        _BundleState?.TryTransition();
    }



    private void OnDestroy()
    {
        _BundleState = null;
    }



    private void SetErrCode(int errCo)
    {
        if (_BundleError != null)
        {
            _BundleError.ErrorCode = errCo;
        }
    }


    public void AgreeDownLoad()
    {
        if (_DownloadSize != null)
        {
            _DownloadSize.AgreeDownLoad = true;
        }
    }
  
}

public class AssetBundleLoader_Error : StateNode
{
    public Action<int> OnBundlerEnter;

    public AssetBundleLoader_Error(string name, Action<int> onBundlerEnter) : base(name)
    {
        OnBundlerEnter = onBundlerEnter;
    }


    public override void Enter()
    {
        base.Enter();
        OnBundlerEnter?.Invoke(ErrorCode);
    }
}

public class AssetBundleLoader_Init : StateNode
{
    private CancellationTokenSource cancellationTokenSource;

    public AssetBundleLoader_Init(string name) : base(name)
    {
    }

    public override async void Enter()
    {
        base.Enter();
        cancellationTokenSource = new CancellationTokenSource(); // 토큰 생성
        await Init(cancellationTokenSource.Token);
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        // Addressables 초기화 요청
        AsyncOperationHandle _Handle = Addressables.InitializeAsync(true);

        try
        {
            // 취소가 가능하도록 await 처리
            await WaitForInitialization(_Handle, cancellationToken);

            // 취소가 되지 않았다면 상태에 따라 처리
            if (_Handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressables initialized successfully!");
                StateID = E_STATE_ID.SUCCESS;
            }
            else
            {
                Debug.LogError("Addressables initialization failed!");
                StateID = E_STATE_ID.QUIT;
            }
        }
        catch (OperationCanceledException)
        {
            // 취소된 경우 처리
            Debug.Log("Addressables initialization was cancelled.");
            StateID = E_STATE_ID.QUIT; // 상태 종료로 설정
        }
        catch (System.Exception ex)
        {
            // 예외 발생 시 처리
            Debug.LogError($"Initialization error: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            // 작업 핸들 해제
        }

        CanTransition = true;
    }

    // 비동기 작업을 취소 가능하게 대기하는 메서드
    private async Task WaitForInitialization(AsyncOperationHandle handle, CancellationToken cancellationToken)
    {
        while (handle.IsDone == false) 
        {
            cancellationToken.ThrowIfCancellationRequested(); // 취소 요청이 있으면 예외를 던짐
            await Task.Yield(); // 계속 대기
        }
    }

    // 종료 시 취소 처리
    public override void Exit()
    {
        base.Exit();

        // 작업이 진행 중이라면 취소 요청
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // 자원 해제
    }
}

public class AssetBundleLoader_CheckValid : StateNode
{

    public AssetBundleLoader_CheckValid(string name) : base(name)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (Addressables.ResourceLocators.Count() > 0)
        {
            Debug.Log("Addressables initialized successfully.");
            StateID = E_STATE_ID.SUCCESS;
        }
        else
        {
            Debug.LogError("Addressables initialization failed.");
            StateID = E_STATE_ID.ERROR;
        }

        CanTransition = true;
    }
}

public class AssetBundleLoader_UpdateCatalog : StateNode
{
    private CancellationTokenSource cancellationTokenSource;

    public AssetBundleLoader_UpdateCatalog(string name) : base(name)
    {
    }

    public override async void Enter()
    {
        base.Enter();
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource 초기화
        await Init(cancellationTokenSource.Token); // CancellationToken 전달
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        try
        {
            // Check for catalog updates
            var checkHandle = Addressables.CheckForCatalogUpdates(true);

            await checkHandle.Task;

            if (checkHandle.IsValid()==false)
            {
                StateID = E_STATE_ID.SUCCESS; //TODO: 임시 해결을 못하곘음
                return;

                Debug.LogError($"Handle Status: {checkHandle.Status}");
                Debug.LogError($"Operation Exception: {checkHandle.OperationException?.Message}");
            }

            if (checkHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var catalogsToUpdate = checkHandle.Result;

                if (catalogsToUpdate != null && catalogsToUpdate.Count > 0)
                {
                    Debug.Log($"Catalog updates available: {catalogsToUpdate.Count}");

                    // Update catalogs
                    AsyncOperationHandle updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
                    await updateHandle.Task;

                    if (updateHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("Catalogs updated successfully.");
                        StateID = E_STATE_ID.SUCCESS;
                    }
                    else
                    {
                        Debug.LogError("Failed to update catalogs.");
                        StateID = E_STATE_ID.ERROR;
                    }
                }
                else
                {
                    Debug.Log("No catalog updates available.");
                    StateID = E_STATE_ID.SUCCESS;
                }
            }
            else
            {
                Debug.LogError("Failed to check for catalog updates.");
                StateID = E_STATE_ID.ERROR;
            }
        }
        catch (OperationCanceledException)
        {
            // 취소된 경우 처리
            Debug.Log("Catalog update process was cancelled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (Exception ex)
        {
            // 다른 예외 처리
            Debug.LogError($"Error during catalog update: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            CanTransition = true;
        }
    }


    // 종료 시 취소 처리
    public override void Exit()
    {
        base.Exit();

        // 작업이 진행 중이라면 취소 요청
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // 자원 해제
    }
}

public class AssetBundleLoader_DownloadSize : StateNode
{
    public event Action<long> OnSizeCalculated;

    public List<string> DownLoadLabels { get; private set; }
    private CancellationTokenSource cancellationTokenSource;

    public bool AgreeDownLoad = false;

    public AssetBundleLoader_DownloadSize(string name, List<string> downLoadLabels, Action<long> onSizeCalculate) : base(name)
    {
        DownLoadLabels = downLoadLabels;
        OnSizeCalculated = onSizeCalculate;
    }

    public void ResultAction(bool result)
    {
        CanTransition = result; 
    }


    public override async void Enter()
    {
        base.Enter();
        AgreeDownLoad = false;
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource 초기화
        long totalSize = await CalculateTotalDownloadSizeAsync(DownLoadLabels, cancellationTokenSource.Token);
        if (totalSize == 0)
        {
            CanTransition = true;
        }
        else
        {
            //사이즈 계산이 끝나면 팝업을 띄워줘야함
            OnSizeCalculated?.Invoke(totalSize);
        }
    }


    public override void Update()
    {
        base.Update();

        if (CanTransition == false)
        {
            CanTransition = AgreeDownLoad;
        }
    }

    public async Task<long> CalculateTotalDownloadSizeAsync(List<string> downloadLabels, CancellationToken cancellationToken)
    {
        // 다운로드 크기 요청
        AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(downloadLabels);
        long totalSize = 0;

        try
        {
            // 요청 작업 완료 대기 중 취소를 수동으로 처리
            await Task.Run(() => sizeHandle.Task, cancellationToken);

            if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
            {
                totalSize = sizeHandle.Result;

                if (totalSize > 0)
                {
                    Debug.Log($"Total Download Size: {totalSize} bytes");
                }
                else
                {
                    Debug.Log("No download needed for the provided labels.");
                }
            }
            else
            {
                Debug.LogError("Failed to get download size.");
                StateID = E_STATE_ID.ERROR;
            }
        }
        catch (OperationCanceledException)
        {
            // 취소된 경우 처리
            Debug.Log("Download size calculation was cancelled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (Exception ex)
        {
            // 다른 예외 처리
            Debug.LogError($"Failed to get download size. Error: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            // 핸들 해제
            Addressables.Release(sizeHandle);
        }

        StateID = E_STATE_ID.SUCCESS;
        return totalSize;
    }

    // 종료 시 취소 처리
    public override void Exit()
    {
        base.Exit();

        // 작업이 진행 중이라면 취소 요청
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // 자원 해제
    }
}

public class AssetBundleLoader_DownloadBundles : StateNode
{
    public event Action<long> OnDownLoadBundles;

    public List<string> DownLoadLabels { get; private set; }

    private CancellationTokenSource cancellationTokenSource;

    public AssetBundleLoader_DownloadBundles(string name  ,List<string> downLoadLabels, Action<long> onSizeCalculate) : base(name)
    {
        DownLoadLabels = downLoadLabels;
        OnDownLoadBundles = onSizeCalculate;
    }

    public override async void Enter()
    {
        base.Enter();
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource 초기화
        await DownloadBundlesAsync(DownLoadLabels, cancellationTokenSource.Token);
    }

    public async Task<long> DownloadBundlesAsync(List<string> downloadLabels, CancellationToken cancellationToken)
    {
        long curDownloaded = 0;

        // 번들 다운로드 요청
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(downloadLabels, Addressables.MergeMode.Union);

        try
        {
            // 다운로드 진행 상태 추적
            while (!downloadHandle.IsDone && downloadHandle.Status != AsyncOperationStatus.Failed)
            {
                cancellationToken.ThrowIfCancellationRequested(); // 취소 요청이 있는지 체크

                // 현재 진행률을 기반으로 다운로드된 크기 추정
                curDownloaded = downloadHandle.GetDownloadStatus().DownloadedBytes;

                // 진행 상태 로그 출력
                Debug.Log($"Downloading bundles... Progress: {downloadHandle.PercentComplete * 100:F2}%, Downloaded: {curDownloaded} bytes");
                OnDownLoadBundles?.Invoke(curDownloaded);

                await Task.Yield(); // 진행률 업데이트
            }

            // 다운로드 완료 후 처리
            if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Download completed successfully!");
                curDownloaded = downloadHandle.GetDownloadStatus().DownloadedBytes;
                OnDownLoadBundles?.Invoke(curDownloaded);

                StateID = E_STATE_ID.SUCCESS;
            }
            else
            {
                Debug.LogError("Download failed!");
                StateID = E_STATE_ID.ERROR;
            }
        }
        catch (OperationCanceledException)
        {
            // 취소된 경우 처리
            Debug.Log("Download was canceled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (System.Exception ex)
        {
            // 다른 예외 처리
            Debug.LogError($"Error while downloading bundles: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            CanTransition = true;
            Addressables.Release(downloadHandle);
        }


        return curDownloaded;
    }

    // 종료 시 취소 처리
    public override void Exit()
    {
        base.Exit();

        // 다운로드 작업이 진행 중이라면 취소 요청
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // 자원 해제
    }
}

public class AssetBundleLoader_LoadToMemory : StateNode
{
    public event Action<string,int,int> OnBundleLoadMemoty;
    private CancellationTokenSource cancellationTokenSource;
    public List<string> DownLoadLabels { get; private set; }

    public AssetBundleLoader_LoadToMemory(string name, List<string> downloadLabels, Action<string,int,int> onBundleLoadMemory) : base(name)
    {
        OnBundleLoadMemoty= onBundleLoadMemory;
        DownLoadLabels = downloadLabels;
    }

    public override async void Enter()
    {
        base.Enter();
        cancellationTokenSource = new CancellationTokenSource(); // 토큰 생성
        await Init(cancellationTokenSource.Token);
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        // Addressables 초기화 요청

        AsyncOperationHandle<IList<IResourceLocation>> _Handle = Addressables.LoadResourceLocationsAsync(DownLoadLabels, Addressables.MergeMode.Union,typeof(UnityEngine.Object));

        try
        {
            // 취소가 가능하도록 await 처리
            await WaitForInitialization(_Handle, cancellationToken);

            // 취소가 되지 않았다면 상태에 따라 처리
            if (_Handle.Status == AsyncOperationStatus.Succeeded)
            {
                int loadCount = 0;
                int totalCount = _Handle.Result.Count;

                foreach (var resourceLocation in _Handle.Result)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await LoadAsync<UnityEngine.Object>(resourceLocation.PrimaryKey, cancellationToken, (obj) =>
                    {
                        loadCount++;
                        OnBundleLoadMemoty?.Invoke(resourceLocation.PrimaryKey, loadCount, totalCount);
                    });
                }

                StateID = E_STATE_ID.SUCCESS;
            }
            else
            {
                Debug.LogError("Failed to load Addressables locations!");
                StateID = E_STATE_ID.ERROR;
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Addressables loading was cancelled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error while loading Addressables: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            Addressables.Release(_Handle);
        }

        CanTransition = true;
    }

    private async Task LoadAsync<T>(string key, CancellationToken cancellationToken, Action<UnityEngine.Object> callback) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(key);
        try
        {
            await WaitForInitialization(asyncOperation, cancellationToken);

            if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
            {
                AssetServer.AddResource(key, asyncOperation.Result);
                callback?.Invoke(asyncOperation.Result); // 진행 상황 콜백
            }
            else
            {
                Debug.LogError($"Failed to load resource: {key}");
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log($"Loading resource {key} was cancelled.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading resource {key}: {ex.Message}");
        }
        finally
        {
            CanTransition = true;
            Addressables.Release(asyncOperation);
        }
    }

    private async Task WaitForInitialization(AsyncOperationHandle handle, CancellationToken cancellationToken)
    {
        while (!handle.IsDone)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
        }
    }

    public override void Exit()
    {
        base.Exit();
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}


public class AssetBundleLoader_LoadToMemory_Complete : StateNode
{
    public event Action OnBundleLoadMemoryComplete;

    public AssetBundleLoader_LoadToMemory_Complete(string name, Action onBundleLoadMemoryComplete) : base(name)
    {
        OnBundleLoadMemoryComplete = onBundleLoadMemoryComplete;
    }

    public override void Enter()
    {
        base.Enter();
        OnBundleLoadMemoryComplete?.Invoke();
    }
}

