using UnityEngine;

public partial class BaseActor : MonoBehaviour
{
    protected virtual D_F_Enum.E_OBJECT_TYPE ObjectType {  get; }

    //0은 현재, 1 은 최대
    public int[] Health = new int[2];
    public int[] Mana = new int[2];

    public Vector3 GetCord => gameObject.transform.position;
    public int ClientIndex { get; protected set; } = -1;
    public int UniqueIndex { get; protected set; }

    private void Awake()
    {
        TryGetComponent(out _animation);
    }



    public virtual void OnDamaged(int damage)
    {
        Health[0] -= damage;

        if (Health[0] < 0)
        {
            Die();
        }
        else
        {
            PlayAnimator(ActorActionID.HIT);
        }
    }


    public virtual void Die()
    {
        AssetServer.Destroy(gameObject);

        PlayAnimator(ActorActionID.Die);
    }


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

    public static string HIT = "hit";

}
