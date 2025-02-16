using D_F_Enum;
using UnityEngine;

public partial class BaseActor : MonoBehaviour
{

    //�̱� Ÿ���� ������ �뵵
    public virtual void T_HIT_INFO_FOR_ONE_TARGET()
    {
        if (TargetActor == null)
        {
            Debug.Log($" Ÿ�� ������ �������� �ʽ��ϴ�");
            return;
        }

        //E_HIT_SORT�� ���� �ڿ����� Ȯ�� ---> �׳� �Լ� 3�� ������
        GameLogic.Instance.HIT(ObjectType, ServerIndex, ThisAttackID, TargetActor.ObjectType, TargetActor.ServerIndex, E_HIT_SORT.NORMAL, Damage);
    }



    //�ϴ� �÷��̾ ���͸� ������ �͸� ����...
    //���� Ȯ�� �ʿ�   MonsterActor[] monsters �������� �κ�
    //��Ƽ Ÿ���� ������ �뵵-> �ڱ� �ֺ� 
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


    //�ϴ� �÷��̾ ���͸� ������ �͸� ����...
    //���� Ȯ�� �ʿ�   MonsterActor[] monsters �������� �κ�
    //��Ƽ Ÿ���� ������ �뵵-> �ڱ� �ֺ� 
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
