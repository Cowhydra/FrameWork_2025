using System.Collections;
using UnityEngine;

public class MonsterSpawner :MonoBehaviour
{
    //��������  ���� ���̺� �ʿ�
    private float _spawnRadius = 50f; // �÷��̾� �ֺ� ���� �ݰ�

    private GameLogic _Logic;
    private PlayerActor _MyPlayer;
    private Vector3 _MyPlayerPos;

    private Coroutine _SpawnCo;

    private void OnDestroy()
    {
        this.SafeStopCoroutine(ref _SpawnCo);
    }

    public void Initialize(GameLogic logic)
    {
        _Logic = logic;
        _MyPlayer = _Logic.MyPlayer;
        _MyPlayerPos = _Logic.MyPlayerPos;
    }


    public void SpawnStart()
    {
        //���������� ���� �ְ� 
        // �����κ��� �ҷ��;ߤ���

        _SpawnCo = StartCoroutine(nameof(SpawnMonsters));
    }


    //5�ʿ� �ѹ��� ����
    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            int curMonster =GameLogic.Instance.MonsterCount;

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
                    AssetServer.Instantiate(BundleUtil.GetMonsterAssetLabel(AssetServer.MonsterDataDict.Value[monsterIndex].ModelNumber), null, true);
                    _Logic.Monsters[slotIndex].Set(monsterIndex, slotIndex);
                }

                yield return null;
            }

            _Logic.UpdateMonsterCount();

            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }


}
