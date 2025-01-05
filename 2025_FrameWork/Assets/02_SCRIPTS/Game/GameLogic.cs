using D_F_Enum;
using UnityEngine;
using System.Collections;

public partial class GameLogic : MonoBehaviour
{
    //씬의 이름으로 부터 생성해야할 몬스터들을 가져와야 합니다.
    public static PlayerActor MyPlayer;

    public static StageData StageData;

    public MonsterSpawner _Spawner;

    public Vector3 MyPlayerPos => MyPlayer != null ? MyPlayer.GetCord : Vector3.zero;


    //TODO; 임시
    private void Awake()
    {
        Initialzied();
    }


    public void Initialzied()
    {
        MyPlayer = GameObject.FindGameObjectWithTag(TAG.MyPlayer).GetComponent<PlayerActor>();

        if (MyPlayer == null)
        {
            Debug.LogWarning("My Player Not Found!!!");
        }
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
