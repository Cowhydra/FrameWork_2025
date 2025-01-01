using System.Text;


//�ܼ��ϰ� onTickFn Auction�� �޾Ƽ� ���� 
public class BTServiceBase : BTElementBase
{
    protected System.Action<float> onTickFn;


    public void Init(string _Name, System.Action<float> _OnTickFn)
    {
        Name = _Name;
        onTickFn = _OnTickFn;
    }


    public virtual void OnTick(float deltaTime)
    {
        onTickFn?.Invoke(deltaTime);
    }


    protected override void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0)
    {
        for (int index = 0; index < indentLevel; ++index)
        {
			debugTextBuilder.Append(' ');
		}

		debugTextBuilder.Append($"S: {Name}");
    }
}
