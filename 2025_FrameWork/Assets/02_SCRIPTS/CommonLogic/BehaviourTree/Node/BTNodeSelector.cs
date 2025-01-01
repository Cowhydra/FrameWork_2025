using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Selector�� �ϳ��� �ڽ� ���� �����ϸ� �θ� ��尡 ������ ������ ó��
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
