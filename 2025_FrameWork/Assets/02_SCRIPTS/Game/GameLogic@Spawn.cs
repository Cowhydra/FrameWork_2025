using System.Collections;
using UnityEngine;

public partial class GameLogic : SingletonObj<GameLogic>
{
    
    //몬스터 관리용도
    private MonsterActor[] _Monsters = new MonsterActor[100];
    private Coroutine _MonsterSpawnCoroutine;


    [SerializeField] private int _maxSpawnCount = 100; // 스폰할 몬스터 수
    [SerializeField] private int _leastSpawnMonster = 5;//최소 소환되어 있어야할 몬스터
    [SerializeField] private float _spawnRadius = 50f; // 플레이어 주변 스폰 반경

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


    public int GetCurrMonsterCount()
    {
        int count = 0;

        for (int i = 0, len = _Monsters.Length; i < len; ++i)
        {
            if (_Monsters[i].IsValid == true)
            {
                count++;
            }
        }

        return count;
    }


    //5초에 한번씩 실행
    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            int curMonster = MonsterCount;

            int spawnTarget = 20;

            if (curMonster > 30)
            {
                spawnTarget = 30;
            }
            else if (curMonster > 70)
            {
                spawnTarget = 40;
            }

            for (int i = 0; i < spawnTarget; i++)
            {
                Vector3 spawnPosition = VectorUtil.GetRandomPositionAroundPlayer(MyPlayerPos, _spawnRadius);
                int monsterIndex = Random.Range(1, 11);

                if(FindEmptyMonster(monsterIndex, out int slotIndex) == true)
                {
                    AssetServer.Instantiate(BundleUtil.GetMonsterAssetLabel(AssetServer.MonsterDataDict.Value[monsterIndex].ModelNumber), null, true);
                    _Monsters[slotIndex].Set(monsterIndex, slotIndex);
                }

                yield return null;
            }

            MonsterCount = GetCurrMonsterCount();

            yield return new WaitForSeconds(Random.Range(2, 10));
        }

    }


}
