using System.Text;


//모든 Behavior Tree 요소의 기본 클래.
public abstract class BTElementBase
{
    //이름
    public string Name { get; protected set; }
    

    //디버그
    public string GetDebugText(int indentLevel = 0)
    {
        StringBuilder debugTextBuilder = new StringBuilder();

        GetDebugTextInternal(debugTextBuilder, indentLevel);

        return debugTextBuilder.ToString();
    }


    //디버그 관련 텍스트 오버라이딩
    protected abstract void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0);
}
