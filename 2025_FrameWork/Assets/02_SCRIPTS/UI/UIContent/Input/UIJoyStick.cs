using UnityEngine;
using UnityEngine.InputSystem;


//=====================================//
// Stated, Perforemd, Canceld 다 따로따로 이벤트 등록//
public class UIJoyStick : MonoBehaviour
{
    [SerializeField] private RectTransform _JoyStickHandle; // 조이스틱 핸들 UI
    [SerializeField] private RectTransform _JoyStickBG; // 조이스틱 배경 UI
    [SerializeField] private float _JoyStickRange = 50f; // 핸들이 움직일 수 있는 최대 거리

    private InputSystem_Actions _InputActions; // 생성된 InputActions 클래스

    private bool IsValid => _JoyStickBG.gameObject.activeSelf;
    private RectTransform _MyRectTransform;

    private bool isDragging = false;


    private void Awake()
    {
        _InputActions = new InputSystem_Actions();
        TryGetComponent(out _MyRectTransform);
    }


    //액션 활성
    private void OnEnable()
    {
        _InputActions.UI.Enable();
        
        //UI용도 Navigate를 사용하는 방법도 공부해볼 것
        _InputActions.UI.Point.performed += OnPointerPointPerformed;

        _InputActions.UI.Press.performed += OnPointerClick;
        _InputActions.UI.Press.started += OnPointerClick;
        _InputActions.UI.Press.canceled += OnPointerClick;

        _JoyStickBG.SetActiveEx(false);
    }


    //액션 비활성
    private void OnDisable()
    {
        // Disable Input Actions
        _InputActions.UI.Point.performed -= OnPointerPointPerformed;

        _InputActions.UI.Press.performed-= OnPointerClick;
        _InputActions.UI.Press.started -= OnPointerClick;
        _InputActions.UI.Press.canceled -= OnPointerClick;

        _InputActions.UI.Disable();
    }

    private void OnPointerClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _JoyStickBG.SetActiveEx(true);
            Debug.Log($"OnPointerClick : started");
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

            Debug.Log($"OnPointerClick : performed");
        }
        else
        {
            _JoyStickBG.SetActiveEx(false);
            isDragging = false;

            Debug.Log($"OnPointerClick : canceled");
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
            }
            else
            {
                Debug.Log($"Dragging: perforemd");
            }
        }
    }
}
