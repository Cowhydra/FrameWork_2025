using D_F_Enum;
using UnityEngine;

public partial class MonsterActor : BaseActor
{
    MonsterData _MonsterData;
    BaseActor _Target;

    public override E_OBJECT_TYPE ObjectType => E_OBJECT_TYPE.MONSTER;

    public bool IsDead => Health[0] > 0;

    public bool IsValid => IsDead == false && _MonsterData != null;

    public void Set(int UniqueIndex)
    {
        if (AssetServer.MonsterDataDict.Value.TryGetValue(UniqueIndex, out _MonsterData) == false)
        {
            Debug.LogError($"MonsterDataDict CanNot Find : {UniqueIndex}");
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
    }


    public override void Die()
    {
        base.Die();

        //몬스터 들 죽 으면 아이템 떨궈야함 
    }

}
