using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Selector는 하나의 자식 노드라도 성공하면 부모 노드가 성공한 것으로 처리
public class BTNodeSelector : BTNodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }
    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return false;
    }
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }

}
