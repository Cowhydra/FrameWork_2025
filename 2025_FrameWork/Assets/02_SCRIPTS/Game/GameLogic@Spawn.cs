using System.Collections;
using UnityEngine;

public partial class GameLogic : SingletonObj<GameLogic>
{
    
    //몬스터 관리용도
    private MonsterActor[] _Monsters = new MonsterActor[100];
    public  MonsterActor[] Monsters =>  _Monsters;


    public int StageMaxMonster = 100;

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
}
