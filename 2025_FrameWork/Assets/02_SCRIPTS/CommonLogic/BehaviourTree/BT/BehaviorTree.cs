using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    public enum ENodeStatus
    {
        Unknown,    //초기화X 
        InProgress, //진행중
        Failed,    // 실패
        Succeeded // 성공
    }


    protected BTNodeBase RootNode { get; private set; } = new BTNodeBase("ROOT");


    protected void ResetRootNode()
    {
        RootNode.Reset();
    }


    protected void Tick(float time = 0)
    {
        if (time == 0)
        {
			time = Time.deltaTime;
		}

		RootNode.Tick(Time.deltaTime);
    }


    public string GetDebugText()
    {
        return RootNode.GetDebugText();
    }

}
