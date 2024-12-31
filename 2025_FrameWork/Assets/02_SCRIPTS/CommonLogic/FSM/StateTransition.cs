using System;
using UnityEngine;


//���� ������ ���� �� 
public class StateTransition 
{
    //������ ���ϴ� ����
    public StateNode TargetState { get; private set; }
    //����
    public Func<bool> Condition { get; private set; }

    //������
    public StateTransition(StateNode targetState, Func<bool> condition = null)
    {
        TargetState = targetState;
        Condition = condition;
    }
}
