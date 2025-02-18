using System.Collections.Generic;

namespace BeHaviorTree
{

    public abstract class CompositeNode : BTNode
    {
        protected List<BTNode> _children = new List<BTNode>();

        public void AddChild(BTNode node)
        {
            _children.Add(node);
        }
    }


    //�ϳ��� �����̸� ����
    public class BTSelectorNode : CompositeNode
    {
        public override NodeStatus Tick(float deltaTime)
        {
            foreach (var child in _children)
            {
                var status = child.Tick(deltaTime);

                if (status == NodeStatus.Success)
                {
                    return NodeStatus.Success;
                }

                if (status == NodeStatus.Running)
                {
                    return NodeStatus.Running;
                }
            }
            return NodeStatus.Failure;
        }
    }


    //��� �����ؾ� ����
    public class BTSequenceNode : CompositeNode
    {
        public override NodeStatus Tick(float deltaTime)
        {
            foreach (var child in _children)
            {
                var status = child.Tick(deltaTime);
                if (status == NodeStatus.Failure)
                {
                    return NodeStatus.Failure;
                }
                if (status == NodeStatus.Running)
                {
                    return NodeStatus.Running;
                }
            }

            return NodeStatus.Success;
        }
    }

}
