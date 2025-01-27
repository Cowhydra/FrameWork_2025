using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public partial class UIScene : MonoBehaviour ,IBundleLoaderOwner
{
    private long _TotalBundleDownloadSize = 0;
    [SerializeField] private AssetBundleLoader _AssetBundleLoader;

    protected List<string> _BundleLabels = new List<string>() { "ui", "model", "data", "animation" };
    public List<string> BundleLabels { get => _BundleLabels; }


    public AssetBundleLoader GetBundleLoader()
    {
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

        //여기서 동의박스 해서 하면 흐름 이어줘야함 여기도 YES/NO 박스 필요
        Current.OpenYesNoMsgBox(string.Format("번들 다운로드 하시곘습니까? :{0}", size.ToString()), null, 0,

            (uimsgbox, btnid) =>
            {
                if (btnid == UIMsgBox_YesNo.BT_YES)
                {
                    if (BundleUtil.IsDiskSpaceEnough(size) == false)
                    {
                        OnBundlerEnterError(-1);
                    }
                    else
                    {
                        if (_AssetBundleLoader != null)
                        {
                            _AssetBundleLoader.AgreeDownLoad();
                        }
                    }
                }
                else
                {
                    OnBundlerEnterError(-2);
                }
            }
            );
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
        Current.InitSliderMsg($"메모리로드중 다운로드중.. 주제 :{label}:{loadCount}/{totalCount}");
        Current.UpdateSliderProgress((float)loadCount / totalCount);
    }



    void OnBundlerEnterError(int errocde)
    {
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

    void IBundleLoaderOwner.OnBundlerEnterError(int errocde)
    {
        OnBundlerEnterError(errocde);
    }
}
