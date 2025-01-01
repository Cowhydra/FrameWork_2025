using D_F_Enum;
using UnityEngine;

public partial class MonsterActor : BaseActor
{
    MonsterData _MonsterData;

    public override E_OBJECT_TYPE ObjectType => E_OBJECT_TYPE.MONSTER;

    public bool IsDead => Health[0] > 0;

    public bool IsValid => IsDead == false && _MonsterData != null;


    public void Set(int uniqueIndex, int clientIndex)
    {
        ClientIndex = clientIndex;
        UniqueIndex = uniqueIndex;

        if (AssetServer.MonsterDataDict.Value.TryGetValue(uniqueIndex, out _MonsterData) == false)
        {
            Debug.LogError($"MonsterDataDict CanNot Find : {uniqueIndex}");
            return;
        }
        else
        {
            //ü�¼���
            Health[0] = _MonsterData.Health;
            Health[1] = _MonsterData.Health;
            //���� ����
            Mana[0] = 0;
            Mana[1] = _MonsterData.Mana;
        }
    }


    public override void Die()
    {
        base.Die();

        //���� �� �� ���� ������ ���ž��� 
        //�÷��̾� ����ġ�� �����
    }

}
