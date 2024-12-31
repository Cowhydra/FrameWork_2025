using UnityEngine;
using UnityEngine.InputSystem;


//=====================================//
// Stated, Perforemd, Canceld �� ���ε��� �̺�Ʈ ���//
public class UIJoyStick : MonoBehaviour
{
    [SerializeField] private RectTransform _JoyStickHandle; // ���̽�ƽ �ڵ� UI
    [SerializeField] private RectTransform _JoyStickBG; // ���̽�ƽ ��� UI
    [SerializeField] private float _JoyStickRange = 50f; // �ڵ��� ������ �� �ִ� �ִ� �Ÿ�

    private InputSystem_Actions _InputActions; // ������ InputActions Ŭ����

    private bool IsValid => _JoyStickBG.gameObject.activeSelf;
    private RectTransform _MyRectTransform;

    private bool isDragging = false;


    private void Awake()
    {
        _InputActions = new InputSystem_Actions();
        TryGetComponent(out _MyRectTransform);
    }


    //�׼� Ȱ��
    private void OnEnable()
    {
        _InputActions.UI.Enable();
        
        //UI�뵵 Navigate�� ����ϴ� ����� �����غ� ��
        _InputActions.UI.Point.performed += OnPointerPointPerformed;

        _InputActions.UI.Press.performed += OnPointerClick;
        _InputActions.UI.Press.started += OnPointerClick;
        _InputActions.UI.Press.canceled += OnPointerClick;

        _JoyStickBG.SetActiveEx(false);
    }


    //�׼� ��Ȱ��
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

                // pointerPosition�� ���� ��ǥ�� ��ȯ
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_MyRectTransform, pointerPosition, null, out localPointerPosition);

                // _JoyStickBG�� ���� ��ġ���� ���� ���
                Vector2 direction = localPointerPosition - _JoyStickBG.anchoredPosition;

                // ���̽�ƽ �ڵ� ��ġ ������Ʈ
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
