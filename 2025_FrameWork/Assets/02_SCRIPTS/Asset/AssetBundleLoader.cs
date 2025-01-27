using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;


//���������� 
//2. ������Ʈ Ȯ��  ���� ���� ( �ٸ� ��� -> ���� ��ġ�� �ʿ��� ��� ( ����� �̵� , ���� �մ� �̵�)
//3. ���� ���ҽ� ���� ���� �������� ��� �󺧺��� �ٿ�ε�


public class AssetBundleLoader :MonoBehaviour
{
    public static string InitDownloadURL;
    private StateMachine _BundleState;
    private System.DateTime _EnterTime;
    AssetBundleLoader_Error _BundleError;// �������´� �ܺο����� ȣ�� �����ϵ���
    AssetBundleLoader_DownloadSize _DownloadSize;
    public List<string> DownLoadLabels;

    //���⿡ �ݹ� �Լ��� �־������ �� ���� �׷��� �ܺο��� ����
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

        //�ʱ�ȭ
        _BundleInit.Transitions.Add(new StateTransition(_BundleCheckValid, () => _BundleInit.StateID == E_STATE_ID.ERROR));
        _BundleInit.Transitions.Add(new StateTransition(_BundleError, () => _BundleInit.StateID == E_STATE_ID.QUIT));
        _BundleInit.Transitions.Add(new StateTransition(_BundleCheckValid, () => _BundleInit.StateID == E_STATE_ID.SUCCESS));

        //��ȿ��üũ -> ���н� �ʱ�ȭ�� ������ �˾ƾ���
        _BundleCheckValid.Transitions.Add(new StateTransition(_BundleInit, () => _BundleCheckValid.StateID == E_STATE_ID.ERROR));
        _BundleCheckValid.Transitions.Add(new StateTransition(_BundleError, () => _BundleCheckValid.StateID == E_STATE_ID.QUIT));
        _BundleCheckValid.Transitions.Add(new StateTransition(_UpdateCatalog, () => _BundleCheckValid.StateID == E_STATE_ID.SUCCESS));

        //īŻ�α� �ٿ�ε�
        _UpdateCatalog.Transitions.Add(new StateTransition(_BundleInit, () => _UpdateCatalog.StateID == E_STATE_ID.ERROR));
        _UpdateCatalog.Transitions.Add(new StateTransition(_BundleError, () => _UpdateCatalog.StateID == E_STATE_ID.QUIT));
        _UpdateCatalog.Transitions.Add(new StateTransition(_DownloadSize, () => _UpdateCatalog.StateID == E_STATE_ID.SUCCESS));

        //������ �ٿ�ε�
        _DownloadSize.Transitions.Add(new StateTransition(_BundleInit, () => _DownloadSize.StateID == E_STATE_ID.ERROR));
        _DownloadSize.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadSize.StateID == E_STATE_ID.SUCCESS));
        _DownloadSize.Transitions.Add(new StateTransition(_BundleError, () => _DownloadSize.StateID == E_STATE_ID.QUIT));

        //���� �ٿ�ε�
        _DownloadBundles.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadBundles.StateID == E_STATE_ID.ERROR));
        _DownloadBundles.Transitions.Add(new StateTransition(_DownloadBundles, () => _DownloadBundles.StateID == E_STATE_ID.QUIT));
        _DownloadBundles.Transitions.Add(new StateTransition(_LoadToMemory, () => _DownloadBundles.StateID == E_STATE_ID.SUCCESS));

        //����ε�
        _LoadToMemory.Transitions.Add(new StateTransition(_BundleError, () => _LoadToMemory.StateID == E_STATE_ID.ERROR));
        _LoadToMemory.Transitions.Add(new StateTransition(_BundleError, () => _LoadToMemory.StateID == E_STATE_ID.QUIT));
        _LoadToMemory.Transitions.Add(new StateTransition(_LoadToMemory_Complete, () => _LoadToMemory.StateID == E_STATE_ID.SUCCESS));

        //���¸ӽ� �ʱ�ȭ
        _BundleState.SetInitialState(_BundleInit);
    }


    private void Update()
    {
        if (_BundleState == null)
        {
            return;
        }

        _BundleState.Update();

        //�����ū ����غ� �� --> ���� ����� 
        if (BundleUtil.IsNetworkValid() == false)
        {
            _BundleState.ForceChangeState(_BundleError);
            return;
        }

        //���� �ð��̻� ������� ���� ���� ó��
        if (TimeUtil.HasElapsed(_EnterTime, 600) == true)
        {
            _BundleState.ForceChangeState(_BundleError);

            //�� ����� �������� �ʿ�
            UIScene.Current.ShowCommonMsg("�ð� �ʰ� ����");
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
        cancellationTokenSource = new CancellationTokenSource(); // ��ū ����
        await Init(cancellationTokenSource.Token);
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        // Addressables �ʱ�ȭ ��û
        AsyncOperationHandle _Handle = Addressables.InitializeAsync(true);

        try
        {
            // ��Ұ� �����ϵ��� await ó��
            await WaitForInitialization(_Handle, cancellationToken);

            // ��Ұ� ���� �ʾҴٸ� ���¿� ���� ó��
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
            // ��ҵ� ��� ó��
            Debug.Log("Addressables initialization was cancelled.");
            StateID = E_STATE_ID.QUIT; // ���� ����� ����
        }
        catch (System.Exception ex)
        {
            // ���� �߻� �� ó��
            Debug.LogError($"Initialization error: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            // �۾� �ڵ� ����
        }

        CanTransition = true;
    }

    // �񵿱� �۾��� ��� �����ϰ� ����ϴ� �޼���
    private async Task WaitForInitialization(AsyncOperationHandle handle, CancellationToken cancellationToken)
    {
        while (handle.IsDone == false) 
        {
            cancellationToken.ThrowIfCancellationRequested(); // ��� ��û�� ������ ���ܸ� ����
            await Task.Yield(); // ��� ���
        }
    }

    // ���� �� ��� ó��
    public override void Exit()
    {
        base.Exit();

        // �۾��� ���� ���̶�� ��� ��û
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // �ڿ� ����
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
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource �ʱ�ȭ
        await Init(cancellationTokenSource.Token); // CancellationToken ����
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
                StateID = E_STATE_ID.SUCCESS; //TODO: �ӽ� �ذ��� ���ρ���
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
            // ��ҵ� ��� ó��
            Debug.Log("Catalog update process was cancelled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (Exception ex)
        {
            // �ٸ� ���� ó��
            Debug.LogError($"Error during catalog update: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            CanTransition = true;
        }
    }


    // ���� �� ��� ó��
    public override void Exit()
    {
        base.Exit();

        // �۾��� ���� ���̶�� ��� ��û
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // �ڿ� ����
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
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource �ʱ�ȭ
        long totalSize = await CalculateTotalDownloadSizeAsync(DownLoadLabels, cancellationTokenSource.Token);
        if (totalSize == 0)
        {
            CanTransition = true;
        }
        else
        {
            //������ ����� ������ �˾��� ��������
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
        // �ٿ�ε� ũ�� ��û
        AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(downloadLabels);
        long totalSize = 0;

        try
        {
            // ��û �۾� �Ϸ� ��� �� ��Ҹ� �������� ó��
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
            // ��ҵ� ��� ó��
            Debug.Log("Download size calculation was cancelled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (Exception ex)
        {
            // �ٸ� ���� ó��
            Debug.LogError($"Failed to get download size. Error: {ex.Message}");
            StateID = E_STATE_ID.QUIT;
        }
        finally
        {
            // �ڵ� ����
            Addressables.Release(sizeHandle);
        }

        StateID = E_STATE_ID.SUCCESS;
        return totalSize;
    }

    // ���� �� ��� ó��
    public override void Exit()
    {
        base.Exit();

        // �۾��� ���� ���̶�� ��� ��û
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // �ڿ� ����
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
        cancellationTokenSource = new CancellationTokenSource(); // CancellationTokenSource �ʱ�ȭ
        await DownloadBundlesAsync(DownLoadLabels, cancellationTokenSource.Token);
    }

    public async Task<long> DownloadBundlesAsync(List<string> downloadLabels, CancellationToken cancellationToken)
    {
        long curDownloaded = 0;

        // ���� �ٿ�ε� ��û
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(downloadLabels, Addressables.MergeMode.Union);

        try
        {
            // �ٿ�ε� ���� ���� ����
            while (!downloadHandle.IsDone && downloadHandle.Status != AsyncOperationStatus.Failed)
            {
                cancellationToken.ThrowIfCancellationRequested(); // ��� ��û�� �ִ��� üũ

                // ���� ������� ������� �ٿ�ε�� ũ�� ����
                curDownloaded = downloadHandle.GetDownloadStatus().DownloadedBytes;

                // ���� ���� �α� ���
                Debug.Log($"Downloading bundles... Progress: {downloadHandle.PercentComplete * 100:F2}%, Downloaded: {curDownloaded} bytes");
                OnDownLoadBundles?.Invoke(curDownloaded);

                await Task.Yield(); // ����� ������Ʈ
            }

            // �ٿ�ε� �Ϸ� �� ó��
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
            // ��ҵ� ��� ó��
            Debug.Log("Download was canceled.");
            StateID = E_STATE_ID.QUIT;
        }
        catch (System.Exception ex)
        {
            // �ٸ� ���� ó��
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

    // ���� �� ��� ó��
    public override void Exit()
    {
        base.Exit();

        // �ٿ�ε� �۾��� ���� ���̶�� ��� ��û
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose(); // �ڿ� ����
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
        cancellationTokenSource = new CancellationTokenSource(); // ��ū ����
        await Init(cancellationTokenSource.Token);
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        // Addressables �ʱ�ȭ ��û

        AsyncOperationHandle<IList<IResourceLocation>> _Handle = Addressables.LoadResourceLocationsAsync(DownLoadLabels, Addressables.MergeMode.Union,typeof(UnityEngine.Object));

        try
        {
            // ��Ұ� �����ϵ��� await ó��
            await WaitForInitialization(_Handle, cancellationToken);

            // ��Ұ� ���� �ʾҴٸ� ���¿� ���� ó��
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
                callback?.Invoke(asyncOperation.Result); // ���� ��Ȳ �ݹ�
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

