using System.Collections;
using UnityEngine;

public class MonsterSpawner :MonoBehaviour
{
    //스테이지  관리 테이블 필요
    private float _spawnRadius = 50f; // 플레이어 주변 스폰 반경
    private Vector3 _MyPlayerPos;



    public async void R_INSERT_MONSTER(int monsterIndex, int ClientIndex, float[] pos)
    {
        if(ClientIndex<0 || ClientIndex >= GameDefine.MAX_MONSTER)
        {
            return;
        }

        GameObject go = await AssetServer.InstantiateAsync<GameObject>(BundleUtil.GetMonsterAssetLabel(AssetServer.MonsterDataDict.Value[monsterIndex].ModelNumber), null, true);
        GameLogic.Instance.Monsters[ClientIndex].Set(monsterIndex, ClientIndex);
        go.transform.position=new Vector3(pos[0],pos[1],pos[2]);
    }


}
