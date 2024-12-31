using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatchScene : UIScene
{
    public GameObject MessageObj;       // 진행 메시지 객체
    public TextMeshProUGUI MessageLbl;  // 진행 메시지 라벨

    public GameObject ProgressObj;      // Progress 객체
    public Image ProgressImg;           // ProgressBar
    public TextMeshProUGUI PercentLbl;  // 퍼센트 라벨
    public TextMeshProUGUI TipLbl;

    [SerializeField] private  AssetBundleLoader AssetBundleLoader;
    private long _TotalBundleDownloadSize = 0;

    protected override void Awake()
    {
        base.Awake();

        InitUI();

        _TotalBundleDownloadSize = 0;
    }


    // UI 초기화
    private void InitUI()
    {
        ShowMessage(false);
        ShowProgressBar(false);
        ShowTipLabel(false);
    }


    // 시작
    private IEnumerator Start()
    {
        // 메시지 표시 (업데이트 체크)
        SetMessage("업데이트 체크중");
        ShowMessage(true);
        yield return null;

        // 애셋 번들 로딩
        AssetBundleLoader.Initalize(OnBundlerEnterError, OnBundleSizeAction, OnBundleDownLoadAction, OnLoadMemoryAction,OnLoadToMemoryComplete);
    }


    // 메시지 표시
    private void ShowMessage(bool val)
    {
        MessageObj.SetActive(val);
    }


    // 메시지 설정
    private void SetMessage(string txt)
    {
        MessageLbl.text = txt;
    }


    // 진행바 표시
    private void ShowProgressBar(bool val)
    {
        ProgressObj.SetActive(val);
    }


    // 진행률 갱신
    private void UpdateProgress(float rate)
    {
        ProgressImg.fillAmount = Mathf.Clamp01(rate);
        PercentLbl.text = string.Format("{0}%", (int)(rate * 100f));
    }


    // 설명 라벨 표시
    public void ShowTipLabel(bool val)
    {
        TipLbl.SetActiveEx(val);
    }


    private void OnBundleSizeAction(long size)
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



    private void OnBundleDownLoadAction(long size)
    {
        //여기서는 그냥 몇초뒤에 다음 씬으로 넘기면 될듯 
        SetMessage(string.Format("번들 다운로드중.. :{0}/{1}", size.ToString(), _TotalBundleDownloadSize.ToString()));

        if (_TotalBundleDownloadSize == 0)
        {
            return;
        }

        ShowProgressBar(true);
        UpdateProgress(size / _TotalBundleDownloadSize);
    }



    private void OnLoadMemoryAction(string label, int loadCount, int totalCount)
    {
        ShowProgressBar(true);
        UpdateProgress((float)loadCount / totalCount);
        SetMessage($"메모리로드중 다운로드중.. 주제 :{label}:{loadCount}/{totalCount}");
    }



    private void OnBundlerEnterError(int errocde)
    {
        //시작씬으로 보내야함  메세지 출력 후 
        Current.OpenOKMsgBox($" 번들 다운로드 Error : {errocde}", null, 0,
            (msgbox, btnid) =>
            {
                SceneLoader.LoadScene(D_F_Enum.SCENE_NAME.Start);
            });
    }


    private void OnLoadToMemoryComplete()
    {
        Debug.Log("로드 완료 !");
    }
}
