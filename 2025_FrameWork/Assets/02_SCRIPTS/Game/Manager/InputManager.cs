using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonObj<InputManager>
{
    private InputSystem_Actions _InputActions; // 생성된 InputActions 클래스

    private event Action<Vector2> _OnPlayerMoveInput;
    private event Action<InputAction.CallbackContext> _OnUIPointInput;
    private event Action<InputAction.CallbackContext> _OnUIPressInput;


    private void Awake()
    {
        _InputActions = new InputSystem_Actions();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _InputActions = null;
    }


    public void OnPlayerMoveInput(Action<Vector2> moveInput)
    {
        _OnPlayerMoveInput += moveInput;
    }


    public void OnUIPointInput(bool isregister,Action<InputAction.CallbackContext> Input)
    {
        if (isregister)
        {
            _OnUIPointInput += Input;
        }
        else
        {
            _OnUIPointInput -= Input;
        }
    }


    public void OnUIPressInput(bool isregister,Action<InputAction.CallbackContext> Input)
    {
        if (isregister)
        {
            _OnUIPressInput += Input;
        }
        else
        {
            _OnUIPressInput -= Input;
        }
    }


    private void OnEnable()
    {
        //Player
        _InputActions.Player.Enable();

        _InputActions.Player.Move.performed += OnPlayterMoveCallBack;
        _InputActions.Player.Move.canceled += OnPlayterMoveCallBack;

        //UI
        //UI용도 Navigate를 사용하는 방법도 공부해볼 것
        _InputActions.UI.Enable();
        _InputActions.UI.Point.performed += OnUIPointCallBack;

        _InputActions.UI.Press.started += OnUIPressCallBack;
        _InputActions.UI.Press.performed += OnUIPressCallBack;
        _InputActions.UI.Press.canceled += OnUIPressCallBack;
    }


    private void OnDisable()
    {
        //Player
        _InputActions.Player.Disable();

        _InputActions.Player.Move.performed -= OnPlayterMoveCallBack;
        _InputActions.Player.Move.canceled -= OnPlayterMoveCallBack;


        //UI
        _InputActions.UI.Disable();
        _InputActions.UI.Point.performed -= OnUIPointCallBack;

        _InputActions.UI.Press.started -= OnUIPressCallBack;
        _InputActions.UI.Press.performed -= OnUIPressCallBack;
        _InputActions.UI.Press.canceled -= OnUIPressCallBack;
    }

    private void OnPlayterMoveCallBack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _OnPlayerMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            _OnPlayerMoveInput?.Invoke(Vector2.zero);
        }
    }


    private void OnUIPressCallBack(InputAction.CallbackContext context)
    {
        _OnUIPressInput?.Invoke(context);
    }


    private void OnUIPointCallBack(InputAction.CallbackContext context)
    {
        _OnUIPointInput?.Invoke(context);
    }
}
