using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//하나라도 실패하면 부모 노드도 실패로 간주되고 종료됩니다.
public class BTNodeSequence : BTNodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return false;
    }


    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }


    //부모의 상태를 자식에 맞게 갱신
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }
}
