using System.Text;


//단순하게 onTickFn Auction을 받아서 실행 
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
