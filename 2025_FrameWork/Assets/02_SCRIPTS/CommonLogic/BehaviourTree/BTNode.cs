namespace BeHaviorTree
{
    public enum NodeStatus
    {
        Success,
        Failure,
        Running
    }

    public abstract class BTNode
    {
        public abstract NodeStatus Tick(float deltaTime);
    }

}
