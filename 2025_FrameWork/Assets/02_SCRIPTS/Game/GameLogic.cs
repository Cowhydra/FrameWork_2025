using System.Collections;
using D_F_Enum;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameLogic : SingletonObj<GameLogic>
{
    //���� �̸����� ���� �����ؾ��� ���͵��� �����;� �մϴ�.
    public PlayerActor MyPlayer;

    public Vector3 MyPlayerPos => MyPlayer != null ? MyPlayer.GetCord : Vector3.zero;



    //TODO; �ӽ�
    private void Awake()
    {
        Initialzied();
    }


    //���͵� ��������
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
                Debug.Log("���� �й�");
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
