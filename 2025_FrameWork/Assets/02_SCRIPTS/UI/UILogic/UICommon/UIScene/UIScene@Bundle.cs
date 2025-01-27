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

        //���⼭ ���ǹڽ� �ؼ� �ϸ� �帧 �̾������ ���⵵ YES/NO �ڽ� �ʿ�
        Current.OpenYesNoMsgBox(string.Format("���� �ٿ�ε� �ϽÁٽ��ϱ�? :{0}", size.ToString()), null, 0,

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
        //���⼭�� �׳� ���ʵڿ� ���� ������ �ѱ�� �ɵ� 
        Current.InitSliderMsg(string.Format("���� �ٿ�ε���.. :{0}/{1}", size.ToString(), _TotalBundleDownloadSize.ToString()));

        if (_TotalBundleDownloadSize == 0)
        {
            return;
        }

        Current.UpdateSliderProgress(size / _TotalBundleDownloadSize);
    }



    void IBundleLoaderOwner.OnLoadMemoryAction(string label, int loadCount, int totalCount)
    {
        Current.InitSliderMsg($"�޸𸮷ε��� �ٿ�ε���.. ���� :{label}:{loadCount}/{totalCount}");
        Current.UpdateSliderProgress((float)loadCount / totalCount);
    }



    void OnBundlerEnterError(int errocde)
    {
        //���۾����� ��������  �޼��� ��� �� 
        Current.OpenOKMsgBox($" ���� �ٿ�ε� Error : {errocde}", null, 0,
            (msgbox, btnid) =>
            {
                Debug.Log("��ŸƮ ������");
                SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Start);
            });
    }


    void IBundleLoaderOwner.OnLoadToMemoryComplete()
    {
        Debug.Log("�ε� �Ϸ� !");
        SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Lobby);
    }

    void IBundleLoaderOwner.OnBundlerEnterError(int errocde)
    {
        OnBundlerEnterError(errocde);
    }
}
