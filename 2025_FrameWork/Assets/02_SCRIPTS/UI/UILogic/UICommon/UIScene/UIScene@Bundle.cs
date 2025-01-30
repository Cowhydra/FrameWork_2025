using System.Collections.Generic;
using System.Drawing;
using D_F_Enum;
using UnityEngine;

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


    public AssetBundleLoader GetBundleLoader(List<string> bundleLabels)
    {
        //�ʱ�ȭ ��ġ ���
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

        //���⼭ ���ǹڽ� �ؼ� �ϸ� �帧 �̾������ ���⵵ YES/NO �ڽ� �ʿ�
        Current.OpenYesNoMsgBox(string.Format("���� �ٿ�ε� �ϽÁٽ��ϱ�? :{0}", size.ToString()), null, 0,

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



    void OnBundlerEnterError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR errocde)
    {
        if (_AssetBundleLoader != null)
        {
            _AssetBundleLoader.StopBundlesDownLoadLogic();
        }

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


    void IBundleLoaderOwner.OnBundlerEnterError(D_F_Enum.E_BUNDLE_DOWNLOAD_ERROR errocde)
    {
        OnBundlerEnterError(errocde);
    }
}
