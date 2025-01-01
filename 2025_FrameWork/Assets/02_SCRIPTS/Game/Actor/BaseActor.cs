using UnityEngine;

public abstract class BaseActor : MonoBehaviour
{
    public abstract D_F_Enum.E_OBJECT_TYPE ObjectType {  get; }

    [SerializeField] protected Animator _animator;
    //0�� ����, 1 �� �ִ�
    public int[] Health = new int[2];
    public int[] Mana = new int[2];

    public Vector3 GetCord => gameObject.transform.position;
    public int ClientIndex { get; protected set; } = -1;
    public int UniqueIndex { get; protected set; }



    public virtual void OnDamaged(int damage)
    {
        Health[0] -= damage;

        if (_animator != null)
        {
            _animator.Play(ActorActionID.BEATEN);
        }
    }


    public virtual void Die()
    {
        AssetServer.Destroy(gameObject);

        if (_animator != null)
        {
            _animator.Play(ActorActionID.DIE);
        }
    }
}


public static class ActorActionID
{
    public static string IDLE = "idle";
    public static string RUN = "run";
    public static string DIE = "die";
    public static string DETECT = "detect";//����
    public static string Chase = "Chase";//�߰�
    public static string ATTACK = "attack";
    public static int BEATEN = 101;


}
