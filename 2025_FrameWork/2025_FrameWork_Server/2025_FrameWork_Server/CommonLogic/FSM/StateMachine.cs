using System;

public class StateMachine
{
    public StateNode CurrentState { get; private set; }

    public void SetInitialState(StateNode initialState)
    {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void TryTransition()
    {
        if (CurrentState == null)
        {
            return;
        }

        if (CurrentState.CanTransition == false)
        {
            return;
        }

        foreach (var transition in CurrentState.Transitions)
        {
            if (transition.Condition?.Invoke() ?? true) // 조건이 null이면 true로 간주
            {
                ChangeState(transition.TargetState);
                break;
         
            }
        }
    }

    public void Update()
    {
        CurrentState.Update();
    }


    public void ForceChangeState(StateNode targetState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = targetState;

        CurrentState.Enter();
    }

    private void ChangeState(StateNode newState)
    {
        CurrentState.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }
}