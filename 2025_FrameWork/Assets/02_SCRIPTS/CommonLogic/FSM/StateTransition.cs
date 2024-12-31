using System;
using UnityEngine;


//상태 변경을 위한 것 
public class StateTransition 
{
    //변경을 원하는 상태
    public StateNode TargetState { get; private set; }
    //조건
    public Func<bool> Condition { get; private set; }

    //생성자
    public StateTransition(StateNode targetState, Func<bool> condition = null)
    {
        TargetState = targetState;
        Condition = condition;
    }
}
