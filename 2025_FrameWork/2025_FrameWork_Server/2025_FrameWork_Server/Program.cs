using System.Net;
using _2025_FrameWork_Server;
using ServerCore;

public class Program
{
    public static List<ClientSession> Clients = new List<ClientSession>();

    //여기는 1번 서버..
    //다른 존 서버들을 만든다면 포트만 다르게 

    static void Main(string[] args)
    {
        // DNS (Domain Name System)

        // -- 서버 로컬 주소 --
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
        IPEndPoint endPoint = new IPEndPoint(ipAddr!, 54321);

        Listener _listener = new Listener(endPoint,
            () => 
        {
                var clientSession = SessionPool<ClientSession>.Instance.GetSession();
                Clients.Add(clientSession);
                return clientSession;
        });

        while (true)
        {
        }

    }
}