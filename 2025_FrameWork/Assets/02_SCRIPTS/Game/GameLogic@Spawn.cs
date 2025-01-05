using System.Collections;
using UnityEngine;

public partial class GameLogic : MonoBehaviour 
{
    //���� �����뵵
    private MonsterActor[] _Monsters = new MonsterActor[100];
    public  MonsterActor[] Monsters =>  _Monsters;


    //���͵� ��������
    private int _MonsterCount;
    public int MonsterCount
    {
        get
        {
            return _MonsterCount;
        }
        set
        {
            _MonsterCount = value;
        }
    }


    //���� ����ִ� ���� ã��
    public bool FindEmptyMonster(int uniqueId, out int slotIndex)
    {
        slotIndex = -1;
        for (int i = 0, len = _Monsters.Length; i < len; ++i)
        {
            if (_Monsters[i].IsValid == false)
            {
                slotIndex = i;
                return true;
            }
        }

        return false;
    }


    public void UpdateMonsterCount()
    {
        int count = 0;

        for (int i = 0, len = _Monsters.Length; i < len; ++i)
        {
            if (_Monsters[i].IsValid == true)
            {
                count++;
            }
        }

        MonsterCount = count;
    }


    public void ClearMonster()
    {
        for (int i = 0, len = _Monsters.Length; i < len; ++i)
        {
            _Monsters[i].ForceDie();
        }
    }
}
