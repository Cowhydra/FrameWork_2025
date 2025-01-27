using D_F_Enum;
using UnityEngine;

public partial class BaseActor : MonoBehaviour
{
    public virtual D_F_Enum.E_OBJECT_TYPE ObjectType {  get; }

    //0은 현재, 1 은 최대
    public int[] Health = new int[2];
    public int[] Mana = new int[2];

    public bool IsValid => Health[0] > 0 && gameObject.activeSelf == true;
    public Vector3 GetCord => gameObject.transform.position;
    public int ClientIndex { get; protected set; } = -1;
    public int UniqueIndex { get; protected set; }

    public int ThisActionID { get; protected set; }

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

    //타겟 받을 필요는 있음 --> 타겟을 떄린다고 보낸다 게임로직에
    public virtual void T_HIT_INFO(BaseActor target)
    {
        if (target == null)
        {
            return;
        }

        //E_HIT_SORT를 어디거 자여올지 확인 ---> 그냥 함수 3갤 ㅗ할지
        GameLogic.Instance.HIT(ObjectType,UniqueIndex, ThisActionID, target.ObjectType, target.UniqueIndex, E_HIT_SORT.NORMAL, Damage);
    }


    public virtual void R_BEATEN_INFO(E_BEATEN_SORT beatenSort, int damage,BaseActor hitter)
    {
        Health[0] -= damage;

        if (Health[0] < 0)
        {
            Health[0] = 0;
            Die();
        }
        else
        {
            PlayAnimator(ActorActionID.BEATEN);
        }
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
