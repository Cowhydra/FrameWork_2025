using D_F_Enum;
using UnityEngine;

public partial class BaseActor : MonoBehaviour
{

    //싱글 타겟을 떄리는 용도
    public virtual void T_HIT_INFO_FOR_ONE_TARGET()
    {
        if (TargetActor == null)
        {
            Debug.Log($" 타겟 정보가 존재하지 않습니다");
            return;
        }

        //E_HIT_SORT를 어디거 자여올지 확인 ---> 그냥 함수 3갤 ㅗ할지
        GameLogic.Instance.HIT(ObjectType, ServerIndex, ThisAttackID, TargetActor.ObjectType, TargetActor.ServerIndex, E_HIT_SORT.NORMAL, Damage);
    }



    //일단 플레이어가 몬스터를 떄리는 것만 구현...
    //추후 확장 필요   MonsterActor[] monsters 가져오는 부분
    //멀티 타겟을 떄리는 용도-> 자기 주변 
    public virtual void T_HIT_INFO_FOR_MULTI_TARGET(float distance, int maxNum)
    {
        MonsterActor[] monsters = GameLogic.Instance.Monsters;
        int curNum = 0;
        for (int i = 0, len = monsters.Length; i < len; i++)
        {
            if (curNum >= maxNum)
            {
                return;
            }

            if (Vector3.SqrMagnitude(monsters[i].GetCord - GetCord) < distance)
            {
                curNum++;
                GameLogic.Instance.HIT(ObjectType, ServerIndex, ThisAttackID, monsters[i].ObjectType, monsters[i].ServerIndex, E_HIT_SORT.NORMAL, Damage);
            }
        }
    }


    //일단 플레이어가 몬스터를 떄리는 것만 구현...
    //추후 확장 필요   MonsterActor[] monsters 가져오는 부분
    //멀티 타겟을 떄리는 용도-> 자기 주변 
    public virtual void T_HIT_INFO_FOR_AREA_MULTI_TARGET(float distance, int maxNum, Vector3 pos)
    {
        MonsterActor[] monsters = GameLogic.Instance.Monsters;
        int curNum = 0;
        for (int i = 0, len = monsters.Length; i < len; i++)
        {
            if (curNum >= maxNum)
            {
                return;
            }

            if (Vector3.SqrMagnitude(monsters[i].GetCord - pos) < distance)
            {
                curNum++;
                GameLogic.Instance.HIT(ObjectType, ServerIndex, ThisAttackID, monsters[i].ObjectType, monsters[i].ServerIndex, E_HIT_SORT.NORMAL, Damage);
            }
        }
    }



    public virtual void R_BEATEN_INFO(E_BEATEN_SORT beatenSort, int damage, BaseActor hitter)
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

        Debug.Log($"ObjectType -> {ObjectType} damage:{damage} Cur HP: {Health[0]}");
    }
}
