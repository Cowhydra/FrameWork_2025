using System.Collections;
using D_F_Enum;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameLogic : SingletonObj<GameLogic>
{
    //씬의 이름으로 부터 생성해야할 몬스터들을 가져와야 합니다.
    public PlayerActor MyPlayer;

    public Vector3 MyPlayerPos => MyPlayer != null ? MyPlayer.GetCord : Vector3.zero;



    //TODO; 임시
    private void Awake()
    {
        Initialzied();
    }


    //몬스터들 스폰로직
    private int _MonsterCount;
    public int MonsterCount { get
        {
            return _MonsterCount;
        }
        set
        {
            _MonsterCount = value;
            if (_MonsterCount > StageMaxMonster)
            {
                Debug.Log("게임 패배");
            }
        }
       }


    public void Initialzied()
    {
        MyPlayer = GameObject.FindGameObjectWithTag(TAG.MyPlayer).GetComponent<PlayerActor>();

        if (MyPlayer == null)
        {
            Debug.LogWarning("My Player Not Found!!!");
        }
    }

  
}
