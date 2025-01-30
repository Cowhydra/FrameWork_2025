using System.Collections;
using UnityEngine;

public class MonsterSpawner :MonoBehaviour
{
    //스테이지  관리 테이블 필요
    private float _spawnRadius = 50f; // 플레이어 주변 스폰 반경

    private GameLogic _Logic;
    private PlayerActor _MyPlayer;
    private Vector3 _MyPlayerPos;

    private Coroutine _SpawnCo;

    private void OnDestroy()
    {
        SpawnStop();
    }


    public void Initialize(GameLogic logic)
    {
        _Logic = logic;
        _MyPlayer = GameLogic.MyPlayer;
        _MyPlayerPos = _Logic.MyPlayerPos;
    }


    public void SpawnStart()
    {
        //내스테이지 갖고 있고 
        // 정보로부터 불러와야ㅕ함

        _SpawnCo = StartCoroutine(nameof(SpawnMonsters));
    }


    public void SpawnStop()
    {
        this.SafeStopCoroutine(ref _SpawnCo);
    }


    //5초에 한번씩 실행
    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            int curMonster = _Logic.MonsterCount;

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
                Vector3 spawnPosition = VectorUtil.GetRandomPositionAroundPlayer(_MyPlayerPos, _spawnRadius);
                int monsterIndex = Random.Range(1, 11);

                if (_Logic.FindEmptyMonster(monsterIndex, out int slotIndex) == true)
                {
                    AssetServer.InstantiateAtLoaded(BundleUtil.GetMonsterAssetLabel(AssetServer.MonsterDataDict.Value[monsterIndex].ModelNumber), null, true);
                    _Logic.Monsters[slotIndex].Set(monsterIndex,slotIndex, slotIndex, _Logic.CurStage);
                }

                yield return null;
            }

            _Logic.UpdateMonsterCount();

            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }


}
