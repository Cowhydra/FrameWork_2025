using UnityEngine;
using UnityEngine.InputSystem;


//=====================================//
// Stated, Perforemd, Canceld �� ���ε��� �̺�Ʈ ���//
public class PlayerController_InputSystem : MonoBehaviour
{
    [SerializeField] private RectTransform _JoyStickHandle; // ���̽�ƽ �ڵ� UI
    [SerializeField] private RectTransform _JoyStickBG; // ���̽�ƽ ��� UI
    [SerializeField] private float _JoyStickRange = 50f; // �ڵ��� ������ �� �ִ� �ִ� �Ÿ�
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

                // pointerPosition�� ���� ��ǥ�� ��ȯ
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_MyRectTransform, pointerPosition, null, out localPointerPosition);

                // _JoyStickBG�� ���� ��ġ���� ���� ���
                Vector2 direction = localPointerPosition - _JoyStickBG.anchoredPosition;

                // ���̽�ƽ �ڵ� ��ġ ������Ʈ
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
