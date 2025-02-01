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
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 10001);

        Listener _listener = new Listener(endPoint,
            () => 
        { 
                ClientSession client = new ClientSession();
                Clients.Add(client);
                return client;
        });

        while (true)
        {
        }

    }
}