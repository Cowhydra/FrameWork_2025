using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Listener
    {
        Socket? _listenSocket;
        Func<Session> _sessionFactory;

        public Listener(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            _listenSocket.Bind(endPoint);

            // backlog : 최대 대기수
            _listenSocket.Listen(backlog);

            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }


        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            if (_listenSocket == null)
            {
                return;
            }

            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnAcceptCompleted(null, args);
            }
        }


        void OnAcceptCompleted(object? sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success)
                {  
                    Socket acceptedSocket = args.AcceptSocket!;

                    // 소켓이 Dispose되기 전에 원격 엔드포인트 정보를 가져옴
                    // 왜그런지 모르겠는데 Dispose됨..
                    EndPoint remoteEndPoint = acceptedSocket.RemoteEndPoint!;

                    Session session = _sessionFactory.Invoke();
                    session.Start(acceptedSocket);
                    session.OnConnected(remoteEndPoint!);
                }
                else
                {
                    Console.WriteLine(args.SocketError.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnAcceptCompleted 예외: {ex}");
            }
            finally
            {
                RegisterAccept(args);
            }
        }
    }
}
