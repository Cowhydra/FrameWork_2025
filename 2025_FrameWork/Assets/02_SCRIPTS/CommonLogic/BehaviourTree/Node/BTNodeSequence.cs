using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ϳ��� �����ϸ� �θ� ��嵵 ���з� ���ֵǰ� ����˴ϴ�.
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


    //�θ��� ���¸� �ڽĿ� �°� ����
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }
}
