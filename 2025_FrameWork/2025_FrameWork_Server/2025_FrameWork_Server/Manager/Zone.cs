using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _2025_FrameWork_Server;
using _2025_FrameWork_Server.CommonLogic.Util;
using _2025_FrameWork_Server.Object;
using ServerCore;

public class Zone
{
    //현재 존에 존재하는 모든 플레이어
    public static List<ClientSession> Clients = new List<ClientSession>();

    internal static List<S_OBJECT_PLAYER> Players = new List<S_OBJECT_PLAYER>();


    //셀로 나눠서 관리해야할지 확인?..

    //모든 몬스터들.
    internal static List<S_OBJECT_MONSTER> Monsters =new List<S_OBJECT_MONSTER>(300);

    //거리 및 Cell로 구역을 나눠서.. 패킷을 보내야함 ? 


    protected virtual int Index => 1;
    protected virtual int Port => 54321;

    protected virtual int MaxMonster => 300;


    public void Init()
    {
        NetWorkInit();
        ContentInit();

        // 몬스터 AI 루프 실행 (별도 스레드)
        Thread monsterThread = new Thread(SetupMonster);
        monsterThread.Start();
    }


    public void Update()
    {

    }



    private void NetWorkInit()
    {
        // -- 서버 로컬 주소 --//
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress? ipAddr = null;
        foreach (IPAddress addr in ipHost.AddressList)
        {
            if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ipAddr = addr;
                break;
            }
        }

        IPEndPoint endPoint = new IPEndPoint(ipAddr!, Port);

        //로그인 영역 - 1개의 존 -
        Listener _listener = new Listener(endPoint, OnListenCallBack);
    }


    public ClientSession OnListenCallBack()
    {
        //세션이 꺼내오기
        var clientSession = SessionPool<ClientSession>.Instance.GetSession();

        //존에 들어왔다면
        //주위몬스터 및 플레이어들을 기본적으로 넣어주고
        clientSession.Owner = this;


        //다음 프레임 부터 행동들을 넣어주던가 해야함 
        Clients.Add(clientSession);
        return clientSession;
    }



    //맨처음 서버가 띄워진다면?
    private void ContentInit()
    {
        //몬스터들을 뿌려서 스폰해야함
        for(int i=0,len= Monsters.Count; i < len; ++i)
        {
            if (Monsters[i].IsDead == true)
            {
                Monsters[i].Revive(i+1, i+1, i+1);
            }
        }


        while (true)
        {
            if (Monsters.Count > MaxMonster)
            {
                break;
            }

            foreach (var monster in DataManager.MonsterDataDict.Values)
            {
                Monsters.Add(new S_OBJECT_MONSTER(monster.Health, monster.Mana, Monsters.Count, Monsters.Count, Monsters.Count, monster.UniqueKey));
            }
        }
    }


    private void SetupMonster()
    {
        while (true)
        {

            Thread.Sleep(2000);
        }
    }
}
