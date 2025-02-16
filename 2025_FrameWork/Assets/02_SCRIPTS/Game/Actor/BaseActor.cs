using System;
using D_F_Enum;
using UnityEngine;



/// <summary>
/// 기본적으로 플레이어 몬스터 모두 ACtor 로서 Skill을 소환해서 공격한다.
/// 플레이어, 몬스터 액션을 따로 관리하지 않는 방향
/// --> 액션 테이블에 이펙트 리소스 연결 및 총 모션 프레임을 가지고 있어야함  
/// </summary>
public partial class BaseActor : MonoBehaviour
{
    public virtual D_F_Enum.E_OBJECT_TYPE ObjectType {  get; }
    public BaseActor TargetActor;

    //0은 현재, 1 은 최대
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
    public static string DETECT = "Walk";//순찰
    public static string Chase = "Walk";//추격
    public static string Attack = "Attack";
    public static string Shot = "Shot";
    public static string BigShot = "BigShot";

    public static string BEATEN = "beaten";
    public static string HIT = "hit";

}
