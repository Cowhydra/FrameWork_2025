using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//
// ���� �ý��� ����
//
public class UINoticeMsg : MonoBehaviour
{
    public Image MsgBG;
    public TextMeshProUGUI MsgLbl;

    public Queue<string> _MsgQ = new Queue<string>();

    private Coroutine _ShowRoutine = null;

    private const float SHOW_TIME = 3f;

    private void OnEnable()
    {
        _ShowRoutine = null;
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

        while (true)
        {
            yield return new WaitForSeconds(SHOW_TIME);

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
        }

        _ShowRoutine = null;
        gameObject.SetActive(false);
    }
}
