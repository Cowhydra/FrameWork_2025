using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatchScene : UIScene
{
    public GameObject MessageObj;       // ���� �޽��� ��ü
    public TextMeshProUGUI MessageLbl;  // ���� �޽��� ��


    protected override void Awake()
    {
        base.Awake();

        InitUI();
    }


    // UI �ʱ�ȭ
    private void InitUI()
    {
        ShowMessage(false);
    }


    // ����
    private IEnumerator Start()
    {
        // �޽��� ǥ�� (������Ʈ üũ)
        SetMessage("������Ʈ üũ��");
        ShowMessage(true);
        yield return null;

        // �ּ� ���� �ε�
        GetBundleLoader(new List<string>() { "ui", "model", "data", "animation" }).Initalize(this);
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
}
