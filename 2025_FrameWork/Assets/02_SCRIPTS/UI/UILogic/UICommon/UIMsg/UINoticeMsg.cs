using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//
// 약한 시스템 공지
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

        // 큐에 메시지 추가
        // HACK: 메시지 개수 체크 (무제한으로 쌓이는 거 방지, 이 경우는 뭔가 문제가 생긴 거다)
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

            // 일단 현재 메시지를 빼고
            if (_MsgQ.Count > 0)
            {
                _MsgQ.Dequeue();
            }

            // 더이상 메시지가 없으면 break
            if (_MsgQ.Count <= 0)
            {
                break;
            }

            // 새 메시지 출력
            message = _MsgQ.Peek();
            MsgLbl.text = message;
        }

        _ShowRoutine = null;
        gameObject.SetActive(false);
    }
}
