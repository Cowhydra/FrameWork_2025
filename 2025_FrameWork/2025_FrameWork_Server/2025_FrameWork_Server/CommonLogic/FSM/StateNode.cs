using System.Collections.Generic;
using UnityEngine;


public enum E_STATE_ID
{
    ERROR=-1,
    NONE=0,
    SUCCESS=1,
    QUIT=2,

}

public abstract class StateNode
{
    public string Name { get; private set; }
    public List<StateTransition> Transitions { get; private set; }
    public bool CanTransition { get; protected set; } = false; // 상태 변경 가능 여부

    public E_STATE_ID StateID { get; set; } = E_STATE_ID.NONE;

    public int ErrorCode { get; set; }

    protected StateNode(string name)
    {
        Name = name;
        Transitions = new List<StateTransition>();
    }

    public virtual void Enter()
    {
        ErrorCode = 0;
        Debug.Log($"Entering State: {Name}");
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
        ErrorCode = 0;
        Debug.Log($"Exiting State: {Name}");
    }
}
