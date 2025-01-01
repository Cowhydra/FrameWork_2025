public class BTNodeAction : BTNodeBase
{
    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[_children.Count - 1].LastStatus;
    }

}
