using System;

namespace BeHaviorTree
{
    
    public class BTLeaftNode : BTNode
    {
        private Func<float, NodeStatus> _action;

        public BTLeaftNode(Func<float, NodeStatus> action)
        {
            _action = action;
        }

        public override NodeStatus Tick(float deltaTime)
        {
            return _action.Invoke(deltaTime);
        }
    }
}