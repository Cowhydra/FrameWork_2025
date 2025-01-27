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
            //체력설정
            Health[0] = _MonsterData.Health;
            Health[1] = _MonsterData.Health;
            //마나 설정
            Mana[0] = 0;
            Mana[1] = _MonsterData.Mana;
        }

        gameObject.SetActive(true);
    }


    public override void Die()
    {
        base.Die();

        //몬스터 들 죽 으면 아이템 떨궈야함 
        //플레이어 경험치도 줘야함
    }


    public void ForceDie()
    {

    }



    public void SetOff()
    {
        AssetServer.Destroy(gameObject);
    }
}
