using System.Collections;
using UnityEngine;

public partial class GameLogic : MonoBehaviour 
{
    //몬스터들 스폰로직
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


    //몬스터 비어있는 슬롯 찾기
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
