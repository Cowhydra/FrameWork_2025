using D_F_Enum;
using UnityEngine;

public partial class GameLogic : MonoBehaviour
{
    public static PlayerActor MyPlayer;

    //몬스터 관리용도
    private MonsterActor[] _Monsters = new MonsterActor[GameDefine.MAX_MONSTER];
    public MonsterActor[] Monsters => _Monsters;

    //몬스터 관리용도
    private PlayerActor[] _Players= new PlayerActor[GameDefine.MAX_PLAYER];

    public PlayerActor[] Players => _Players;

    public void Initialzied_Actor()
    {
        MyPlayer = GameObject.FindGameObjectWithTag(TAG.MyPlayer).GetComponent<MyPlayerActor>();

        if (MyPlayer == null)
        {
            Debug.LogWarning("My Player Not Found!!!");
        }
    }


    public bool FindPlayer(int serverIndex, out PlayerActor actor)
    {
        actor = null;

        for(int i = 0, len = _Players.Length; i < len; ++i)
        {
            if (_Players[i].IsValid == false)
            {
                continue;
            }

            if (_Players[i].ServerIndex == serverIndex)
            {
                actor= _Players[i];
                return true;
            }
        }

        return false;
    }


    public bool FindMonster(int serverIndex, out MonsterActor actor)
    {
        actor = null;

        for (int i = 0, len = _Monsters.Length; i < len; ++i)
        {
            if (_Monsters[i].IsValid == false)
            {
                continue;
            }

            if (_Monsters[i].ServerIndex == serverIndex)
            {
                actor = _Monsters[i];
                return true;
            }
        }

        return false;
    }
}
