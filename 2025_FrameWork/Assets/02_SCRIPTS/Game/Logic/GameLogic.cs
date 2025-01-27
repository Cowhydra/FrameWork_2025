using D_F_Enum;
using UnityEngine;
using System.Collections;

public partial class GameLogic : MonoBehaviour
{
    //���� �̸����� ���� �����ؾ��� ���͵��� �����;� �մϴ�.


    public static GameLogic Instance;

    public static StageData StageData;

    public MonsterSpawner _Spawner;

    public Vector3 MyPlayerPos => MyPlayer != null ? MyPlayer.GetCord : Vector3.zero;


    //TODO; �ӽ�
    private void Awake()
    {
        Initialzied();

        if (Instance != null)
        {
            GameObject.Destroy(Instance.gameObject);
        }

        Instance = this;
    }



    public void Initialzied()
    {
        Initialzied_Actor();

    }




    public IEnumerator StageLoad()
    {
        _Spawner.SpawnStop();
        yield return null;
        ClearMonster();
        yield return null;
        SetUpNextStage();
        yield return null;
        _Spawner.SpawnStart();
    }
}
