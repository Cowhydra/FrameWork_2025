using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatchScene : UIScene
{
    public GameObject MessageObj;       // 진행 메시지 객체
    public TextMeshProUGUI MessageLbl;  // 진행 메시지 라벨


    protected override void Awake()
    {
        base.Awake();

        InitUI();
    }


    // UI 초기화
    private void InitUI()
    {
        ShowMessage(false);
    }


    // 시작
    private IEnumerator Start()
    {
        // 메시지 표시 (업데이트 체크)
        SetMessage("업데이트 체크중");
        ShowMessage(true);
        yield return null;

        // 애셋 번들 로딩
        GetBundleLoader(new List<string>() { "ui", "model", "data", "animation" }).Initalize(this);
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
}
