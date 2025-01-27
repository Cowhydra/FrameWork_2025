using D_F_Enum;
using UnityEngine;

public partial class MonsterActor : BaseActor
{
    MonsterData _MonsterData;
    StageData _StageData;

    public override E_OBJECT_TYPE ObjectType => E_OBJECT_TYPE.MONSTER;

    public bool IsDead => Health[0] > 0;


    public void Set(int monsterIndex, int uniqueIndex,int clientIndex, StageData stageData)
    {
        ClientIndex = clientIndex;
        UniqueIndex = uniqueIndex;
        _StageData = stageData;

        if (AssetServer.MonsterDataDict.Value.TryGetValue(monsterIndex, out _MonsterData) == false)
        {
            Debug.LogError($"MonsterDataDict CanNot Find : {monsterIndex}");
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

        gameObject.SetActive(true);
    }


    public override void Die()
    {
        base.Die();

        //���� �� �� ���� ������ ���ž��� 
        //�÷��̾� ����ġ�� �����
    }


    public void ForceDie()
    {

    }



    public void SetOff()
    {
        AssetServer.Destroy(gameObject);
    }
}
