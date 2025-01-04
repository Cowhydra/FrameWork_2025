using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatchScene : UIScene, IBundleLoaderOwner
{
    public GameObject MessageObj;       // ���� �޽��� ��ü
    public TextMeshProUGUI MessageLbl;  // ���� �޽��� ��

    public GameObject ProgressObj;      // Progress ��ü
    public Image ProgressImg;           // ProgressBar
    public TextMeshProUGUI PercentLbl;  // �ۼ�Ʈ ��
    public TextMeshProUGUI TipLbl;

    [SerializeField] private  AssetBundleLoader AssetBundleLoader;
    private long _TotalBundleDownloadSize = 0;

    public List<string> BundleLabels { get => new List<string>() { "ui", "model", "data", "animation" }; }

    protected override void Awake()
    {
        base.Awake();

        InitUI();

        _TotalBundleDownloadSize = 0;
    }


    // UI �ʱ�ȭ
    private void InitUI()
    {
        ShowMessage(false);
        ShowProgressBar(false);
        ShowTipLabel(false);
    }


    // ����
    private IEnumerator Start()
    {
        // �޽��� ǥ�� (������Ʈ üũ)
        SetMessage("������Ʈ üũ��");
        ShowMessage(true);
        yield return null;

        // �ּ� ���� �ε�
        AssetBundleLoader.Initalize(this);

    }


    // �޽��� ǥ��
    private void ShowMessage(bool val)
    {
        MessageObj.SetActive(val);
    }


    // �޽��� ����
    private void SetMessage(string txt)
    {
        MessageLbl.text = txt;
    }


    // ����� ǥ��
    private void ShowProgressBar(bool val)
    {
        ProgressObj.SetActive(val);
    }


    // ����� ����
    private void UpdateProgress(float rate)
    {
        ProgressImg.fillAmount = Mathf.Clamp01(rate);
        PercentLbl.text = string.Format("{0}%", (int)(rate * 100f));
    }


    // ���� �� ǥ��
    public void ShowTipLabel(bool val)
    {
        TipLbl.SetActiveEx(val);
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
                        if (AssetBundleLoader != null)
                        {
                            AssetBundleLoader.AgreeDownLoad();
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
        SetMessage(string.Format("���� �ٿ�ε���.. :{0}/{1}", size.ToString(), _TotalBundleDownloadSize.ToString()));

        if (_TotalBundleDownloadSize == 0)
        {
            return;
        }

        ShowProgressBar(true);
        UpdateProgress(size / _TotalBundleDownloadSize);
    }



     void IBundleLoaderOwner.OnLoadMemoryAction(string label, int loadCount, int totalCount)
    {
        ShowProgressBar(true);
        UpdateProgress((float)loadCount / totalCount);
        SetMessage($"�޸𸮷ε��� �ٿ�ε���.. ���� :{label}:{loadCount}/{totalCount}");
    }



     void OnBundlerEnterError(int errocde)
    {
        //���۾����� ��������  �޼��� ��� �� 
        Current.OpenOKMsgBox($" ���� �ٿ�ε� Error : {errocde}", null, 0,
            (msgbox, btnid) =>
            {
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
