using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _2025_FrameWork_Server;
using _2025_FrameWork_Server.Object;
using ServerCore;

public class ZoneManager
{

    //현재 존에 존재하는 모든 플레이어
    public static List<ClientSession> Clients = new List<ClientSession>();

    internal static List<S_OBJECT_PLAYER> Players = new List<S_OBJECT_PLAYER>();


    //모든 몬스터들..
    internal static List<S_OBJECT_MONSTER> Nonsters = new List<S_OBJECT_MONSTER>();

    //거리 및 Cell로 구역을 나눠서.. 패킷을 보내야함 ? 


    protected virtual int Index => 1;
    protected virtual int Port => 54321;


    public void Init()
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
        Listener _listener = new Listener(endPoint,
            () =>
            {
                var clientSession = SessionPool<ClientSession>.Instance.GetSession();
                Clients.Add(clientSession);
                return clientSession;
            });


        Console.WriteLine("_listener listen Start");
    }



    private void SetupMonster()
    {
        while (true)
        {

        }
    }
}
