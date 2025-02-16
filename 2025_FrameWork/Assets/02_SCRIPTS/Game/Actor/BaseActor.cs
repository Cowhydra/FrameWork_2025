using System;
using D_F_Enum;
using UnityEngine;



/// <summary>
/// �⺻������ �÷��̾� ���� ��� ACtor �μ� Skill�� ��ȯ�ؼ� �����Ѵ�.
/// �÷��̾�, ���� �׼��� ���� �������� �ʴ� ����
/// --> �׼� ���̺� ����Ʈ ���ҽ� ���� �� �� ��� �������� ������ �־����  
/// </summary>
public partial class BaseActor : MonoBehaviour
{
    public virtual D_F_Enum.E_OBJECT_TYPE ObjectType {  get; }
    public BaseActor TargetActor;

    //0�� ����, 1 �� �ִ�
    public int[] Health = new int[2];
    public int[] Mana = new int[2];

    public bool IsValid => Health[0] > 0 && gameObject.activeSelf == true;
    public Vector3 GetCord => gameObject.transform.position;
    public int ClientIndex { get; protected set; } = -1;
    public int UniqueIndex { get; protected set; }
    public int ServerIndex { get; protected set; }

    public int ThisActionID { get; protected set; }

    public int ThisAttackID { get; protected set; }

    public virtual int Damage => 100;

    private void Awake()
    {
        TryGetComponent(out _animator);
    }


    private void Update()
    {
        UpdateAnimation();
    }


    private void OnDestroy()
    {

    }


    public virtual void Die() { }
}


public static class ActorActionID
{
    public static string Idle = "Idle";
    public static string Walk = "Walk";
    public static string Die = "Die";
    public static string DETECT = "Walk";//����
    public static string Chase = "Walk";//�߰�
    public static string Attack = "Attack";
    public static string Shot = "Shot";
    public static string BigShot = "BigShot";

    public static string BEATEN = "beaten";
    public static string HIT = "hit";

}
