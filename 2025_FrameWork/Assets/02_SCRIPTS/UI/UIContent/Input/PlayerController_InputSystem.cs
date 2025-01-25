using UnityEngine;
using UnityEngine.InputSystem;


//=====================================//
// Stated, Perforemd, Canceld 다 따로따로 이벤트 등록//
public class PlayerController_InputSystem : MonoBehaviour
{
    [SerializeField] private RectTransform _JoyStickHandle; // 조이스틱 핸들 UI
    [SerializeField] private RectTransform _JoyStickBG; // 조이스틱 배경 UI
    [SerializeField] private float _JoyStickRange = 50f; // 핸들이 움직일 수 있는 최대 거리
    private bool IsValid => _JoyStickBG.gameObject.activeSelf;
    private RectTransform _MyRectTransform;
    private bool isDragging = false;


    private void Awake()
    {
        TryGetComponent(out _MyRectTransform);
        InputManager.Instance.OnUIPointInput(true,OnPointerPointPerformed);
        InputManager.Instance.OnUIPressInput(true,OnPointerPress);

        _JoyStickBG.SetActiveEx(false);
    }
    private void OnDestroy()
    {
        InputManager.Instance.OnUIPointInput(false, OnPointerPointPerformed);
        InputManager.Instance.OnUIPressInput(false, OnPointerPress);
    }

    private void OnPointerPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _JoyStickBG.SetActiveEx(true);
#if ENABLE_INPUT_LOG
            Debug.Log($"OnPointerPress : started");
#endif
            }
        else if (context.performed)
        {

            if (_JoyStickBG != null)
            {
                Vector2 screenPos = Mouse.current.position.ReadValue();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_MyRectTransform, screenPos, null, out Vector2 localPos);
                _JoyStickBG.anchoredPosition = localPos;
            }

            isDragging = true;
#if ENABLE_INPUT_LOG
            Debug.Log($"OnPointerPress : performed");
#endif
        }
        else
        {
            _JoyStickBG.SetActiveEx(false);
            isDragging = false;
#if ENABLE_INPUT_LOG
            Debug.Log($"OnPointerPress : canceled");
#endif
        }

    }


    private void OnPointerPointPerformed(InputAction.CallbackContext context)
    {
        if (IsValid == false)
        {
            return;
        }

        if (context.performed)
        {
            if (isDragging == true)
            {
                Vector2 pointerPosition = context.ReadValue<Vector2>();
                Vector2 localPointerPosition;

                // pointerPosition을 로컬 좌표로 변환
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_MyRectTransform, pointerPosition, null, out localPointerPosition);

                // _JoyStickBG의 시작 위치와의 차이 계산
                Vector2 direction = localPointerPosition - _JoyStickBG.anchoredPosition;

                // 조이스틱 핸들 위치 업데이트
                if (_JoyStickHandle != null)
                {
                    _JoyStickHandle.anchoredPosition = Vector2.ClampMagnitude(direction, _JoyStickRange);
                }

                Messenger<Vector2>.Broadcast(MsgID.CHAR_MOVING_DIRECTION, _JoyStickHandle.anchoredPosition);
            }
            else
            {
                #if ENABLE_INPUT_LOG
                Debug.Log($"Dragging: perforemd");
#endif
            }
        }
    }
}
