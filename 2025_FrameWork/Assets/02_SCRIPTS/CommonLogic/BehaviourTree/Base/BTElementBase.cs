using System.Text;


//��� Behavior Tree ����� �⺻ Ŭ��.
public abstract class BTElementBase
{
    //�̸�
    public string Name { get; protected set; }
    

    //�����
    public string GetDebugText(int indentLevel = 0)
    {
        StringBuilder debugTextBuilder = new StringBuilder();

        GetDebugTextInternal(debugTextBuilder, indentLevel);

        return debugTextBuilder.ToString();
    }


    //����� ���� �ؽ�Ʈ �������̵�
    protected abstract void GetDebugTextInternal(StringBuilder debugTextBuilder, int indentLevel = 0);
}
