using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//
// ������ �߾� ��� �ý��� ����
//
public class UISystemMsg : MonoBehaviour
{
    public Image MsgBG;
    public TextMeshProUGUI MsgLbl;

    public Queue<string> _MsgQ = new Queue<string>();

    private Coroutine _ShowRoutine = null;

    private const float SHOW_TIME = 3f;
    private const float FADE_SPEED = 2f;


    private void OnEnable()
    {
        _ShowRoutine = null;
    }


    private void InitMsgColor()
    {
        MsgLbl.color = new Color(1f, 1f, 1f, 1f);
        MsgBG.color = new Color(1f, 1f, 1f, 1f);
    }


    public void ShowMessage(string message)
    {
        gameObject.SetActive(true);

        // ť�� �޽��� �߰�
        // HACK: �޽��� ���� üũ (���������� ���̴� �� ����, �� ���� ���� ������ ���� �Ŵ�)
        if (_MsgQ.Count < 100)
        {
            _MsgQ.Enqueue(message);
        }

        if (_ShowRoutine == null)
        {
            _ShowRoutine = StartCoroutine(ShowMessageInner(message));
        }
    }


    private IEnumerator ShowMessageInner(string message)
    {
        MsgLbl.text = message;
        InitMsgColor();

        while (true)
        { 
            yield return new WaitForSeconds(SHOW_TIME);

            float alpha = MsgLbl.color.a;
            while (alpha > 0f)
            {
                alpha -= (Time.deltaTime * FADE_SPEED);
                if (alpha < 0f)
                {
                    alpha = 0f;
                }

                MsgLbl.color = new Color(MsgLbl.color.r, MsgLbl.color.g, MsgLbl.color.b, alpha);
                MsgBG.color = new Color(MsgBG.color.r, MsgBG.color.g, MsgBG.color.b, alpha);

                yield return null;
            }

            // �ϴ� ���� �޽����� ����
            if (_MsgQ.Count > 0)
            { 
                _MsgQ.Dequeue();
            }

            // ���̻� �޽����� ������ break
            if (_MsgQ.Count <= 0)
            { 
                break;
            }

            // �� �޽��� ���
            message = _MsgQ.Peek();

            MsgLbl.text = message;
            InitMsgColor();
        }

        _ShowRoutine = null;
        gameObject.SetActive(false);
    }
}
