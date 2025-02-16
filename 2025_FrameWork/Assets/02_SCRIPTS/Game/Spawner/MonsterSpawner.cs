using System.Collections;
using UnityEngine;

public class MonsterSpawner :MonoBehaviour
{
    //��������  ���� ���̺� �ʿ�
    private float _spawnRadius = 50f; // �÷��̾� �ֺ� ���� �ݰ�
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
