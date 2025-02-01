using UnityEngine;
using System.Net;
using System.Threading;
using System;
using ServerCore;
using UnityEngine.Rendering;

public partial class NetWorkObject : SingletonObj<NetWorkObject>
{
    private SendBuffer sendBuffer = new SendBuffer(4096 * 100);
    private ServerSession serverSession;

    //1. 비동기 서버.. 서버 접속 안되었으면 어떻게 처리할지..
    //2. 접속 되었는지 검증하는 부분 Session에 추가 
    //3. 위의 검증부분 & 타임 아웃 등을..해서 초반에 검사하기..


    //TODO: 여기에 지금은 내 로컬로만 돌려서 IP를 이상하게 잡고 있지만
    // 로컬이 아닌쪽으로 돌릴려면 대상 컴퓨터의 ip4 address 랑 포트 넣어주면 됨
    public void ConnectToServer()
    {
        try
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = null;
            foreach (IPAddress addr in ipHost.AddressList)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddr = addr;
                    break;
                }
            }

            IPEndPoint endPoint = new IPEndPoint(ipAddr, 54321);
            Connector connector = new Connector();

            //논블록킹 ..
            connector.Connect(endPoint, () => {
                serverSession = new ServerSession();
                return serverSession;
            });
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
       
    }

    #region 서버와 공유되는 ENUM 지역
    public enum N_E_PACKET_ID : ushort
    {
        CONNECT = 1,
    }

    public enum N_E_LOGIN_TYPE : ushort
    {
        EDITOR = 1,
        GOOGLE = 2,
        APPLE = 3,
    }
    #endregion
}
