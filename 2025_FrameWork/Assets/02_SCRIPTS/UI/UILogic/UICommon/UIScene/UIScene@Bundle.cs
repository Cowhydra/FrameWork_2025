using System.Collections.Generic;
using D_F_Enum;
using UnityEngine;

//고려할 것 
// 1. 프리팹 하나마다 In

public partial class UIScene : MonoBehaviour ,IBundleLoaderOwner
{
    private long _TotalBundleDownloadSize = 0;
    [SerializeField] private AssetBundleLoader _AssetBundleLoader;

    protected List<string> _BundleLabels;
    public List<string> BundleLabels { get => _BundleLabels; }

    protected long _TotaBundleSize;
    public long TotalDownLoadBundleSize { get => _TotaBundleSize; set => _TotaBundleSize=value; }

    public E_BUNDLE_DOWNLOAD_STATE AgreeBundleDownLoad => _BundleDownloadAgree;

    protected E_BUNDLE_DOWNLOAD_STATE _BundleDownloadAgree = E_BUNDLE_DOWNLOAD_STATE.NONE;


    //항상 메모리에 로드되야 하는 주제들 --> 데이터들  등..
    public void LoadMemoryAlways(List<string> labels)
    {
        //우선 번들 다운로드가 끝나면 한번 더 강제적으로 실행 ( 메모리 올려야할 것들)
        //이미 받은 번들이라면 메세지가 안뜰 것이고.. 업데이트 내용이 있으면 뜰것..
        GetBundleLoader(labels).Initalize(this, true);
    }


    public AssetBundleLoader GetBundleLoader(List<string> bundleLabels)
    {
        //초기화 위치 고민
        _BundleDownloadAgree = E_BUNDLE_DOWNLOAD_STATE.NONE;
        _BundleLabels = bundleLabels;


        if (_AssetBundleLoader != null)
        {
            return _AssetBundleLoader;
        }

        GameObject go = new GameObject();
        go.name = "AssetBundleLoader";
        go.AddComponent<AssetBundleLoader>();
        _AssetBundleLoader=go.GetComponent<AssetBundleLoader>();

        return _AssetBundleLoader;
    }
    


    void IBundleLoaderOwner.OnBundleSizeAction(long size)
    {
        _TotalBundleDownloadSize = size;

        //낮은거의 경우 그냥 다운로드
        if (size <= 1)
        {
            _BundleDownloadAgree = E_BUNDLE_DOWNLOAD_STATE.AGREE;

        }
        else
        {
            //여기서 동의박스 해서 하면 흐름 이어줘야함 여기도 YES/NO 박스 필요
            Current.OpenYesNoMsgBox(string.Format("번들 다운로드 하시곘습니까? :{0}", size.ToString()), null, 0,

                (uimsgbox, btnid) =>
                {
                    if (btnid == UIMsgBox_YesNo.BT_YES)
                    {
                        if (BundleUtil.IsDiskSpaceEnough(size) == false)
                        {
                            OnBundlerEnterError(E_BUNDLE_DOWNLOAD_ERROR.SAVE_STORAGE_NOT_ENOUGH);
                        }
                        else
                        {
                            _BundleDownloadAgree = E_BUNDLE_DOWNLOAD_STATE.AGREE;
                        }
                    }
                    else
                    {
                        _BundleDownloadAgree = E_BUNDLE_DOWNLOAD_STATE.DISAGREE;
                        OnBundlerEnterError(E_BUNDLE_DOWNLOAD_ERROR.USER_CANCEL);
                    }
                }
                );
        }
    }



    void IBundleLoaderOwner.OnBundleDownLoadAction(long size)
    {
        //여기서는 그냥 몇초뒤에 다음 씬으로 넘기면 될듯 
        Current.InitSliderMsg(string.Format("번들 다운로드중.. :{0}/{1}", size.ToString(), _TotalBundleDownloadSize.ToString()));

        if (_TotalBundleDownloadSize == 0)
        {
            return;
        }

        Current.UpdateSliderProgress(size / _TotalBundleDownloadSize);
    }



    void IBundleLoaderOwner.OnLoadMemoryAction(string label, int loadCount, int totalCount)
    {
        Current.InitSliderMsg($"메모리에 적재중 ! :{loadCount}/{totalCount}");
        Current.UpdateSliderProgress((float)loadCount / totalCount);
    }



    void OnBundlerEnterError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR errocde)
    {
        if (_AssetBundleLoader != null)
        {
            _AssetBundleLoader.StopBundlesDownLoadLogic();
        }

        //시작씬으로 보내야함  메세지 출력 후 
        Current.OpenOKMsgBox($" 번들 다운로드 Error : {errocde}", null, 0,
            (msgbox, btnid) =>
            {
                Debug.Log("스타트 씬으로");
                SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Start);
            });
    }


    void IBundleLoaderOwner.OnLoadToMemoryComplete()
    {
        Debug.Log("로드 완료 !");
        SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Lobby);
    }


    void IBundleLoaderOwner.OnBundlerEnterError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR errocde)
    {
        OnBundlerEnterError(errocde);
    }
}
