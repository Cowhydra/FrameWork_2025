using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{

    public abstract class PacketSession : Session
    {

        // [헤더][데이터]...
        // 패킷을 받은 경우
        // 모두 받아야함 TCP상 중간에 끊겨 올 수 있어서 .. 약속이 필요
        // 헤더에 데이터 크기를 담아서 해당byte 와야 온전한 데이터 처리가 가능하다.

        public int OnRecv_Test(ArraySegment<byte> buffer)
        {
            //처리한 데이터 사이즈
            int processLen = 0;

            while (true)
            {
                // 최소한 헤더는 파싱할 수 있는지 확인
                if (buffer.Count < SystemDef.HEADER_SIZE)
                {
                    break;
                }

                // 패킷이 완전체로 도착했는지 확인
                ushort dataSize = BitConverter.ToUInt16(buffer.Array!, buffer.Offset);
                if (buffer.Count < dataSize)
                {
                    break;
                }

                // 여기까지 왔으면 패킷 조립 가능
                OnRecvPacket(new ArraySegment<byte>(buffer.Array!, buffer.Offset, dataSize));

                processLen += dataSize;

                //받은  buffer 처리하고 남은 부분을 다시 넣어줌
                buffer = new ArraySegment<byte>(buffer.Array!, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return processLen;
        }


        public sealed override int OnRecv(IList<ArraySegment<byte>> bufferList)
        {
            int processLen = 0;

            for (int i = bufferList.Count - 1; i >= 0; --i)
            {
                ArraySegment<byte> buffer = bufferList[i];

                // 최소한 헤더는 파싱할 수 있는지 확인
                while (buffer.Count >= SystemDef.HEADER_SIZE)
                {
                    // 헤더에서 패킷 크기 확인
                    ushort dataSize = BitConverter.ToUInt16(buffer.Array!, buffer.Offset);

                    // 패킷이 완전한지 확인
                    if (buffer.Count < dataSize)
                    {
                        break; // 패킷이 불완전하면 더 이상 읽지 않고 멈춤
                    }

                    // 패킷이 완전하면 처리
                    OnRecvPacket(new ArraySegment<byte>(buffer.Array!, buffer.Offset, dataSize));

                    // 처리된 데이터 크기만큼 버퍼에서 제거
                    processLen += dataSize;

                    // 나머지 데이터 처리 - 처리된 만큼 버퍼를 갱신
                    int remainingDataSize = buffer.Count - dataSize;
                    buffer = new ArraySegment<byte>(buffer.Array!, buffer.Offset + dataSize, remainingDataSize);

                    // 더 이상 처리할 데이터가 없으면 중단
                    if (buffer.Count < SystemDef.HEADER_SIZE)
                    {
                        break;
                    }
                }

                // 남은 데이터가 있을 경우 bufferList에 갱신
                if (buffer.Count > 0)
                {
                    bufferList[i] = buffer;
                }
                else
                {
                    // 남은 데이터가 없다면 bufferList에서 제거
                    bufferList.RemoveAt(i);
                }
            }

            return processLen;
        }


        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }




    //Session은 그 클라이언트 정보 관리 및 
    //클라이언트와 연결 및 통신하는 주체?
    public abstract class Session
    {
        Socket? _socket;
        public Socket? Socket { get { return _socket; } }


        int _disconnected = 0;
        
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _sendlingList = new List<ArraySegment<byte>>();
        object _lock = new object();

        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        //연결 성공
        public abstract void OnConnected(EndPoint endPoint);
        //패킷 받음
        public abstract int OnRecv(IList<ArraySegment<byte>> bufferList);
        //패킷 보냄
        public abstract void OnSend(int numOfBytes);
        //연결 종료
        public abstract void OnDisconnected(EndPoint endPoint);


        public void Start(Socket? socket)
        {
            _socket = socket;

            BufferManager.Instance.SetBuffer(_recvArgs);

            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
        }

        #region 패킷 보내기
        public void Send(ArraySegment<byte> sendBuffer)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuffer);
                if (_sendlingList.Count == 0) 
                {
                    RegsiterSend();
                }
            }
        }


        void RegsiterSend()
        {
            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _sendlingList.Add(buff);
            }

            _sendArgs.BufferList = _sendlingList;

            if (_socket!.SendAsync(_sendArgs) == false)
            {
                OnSendCompleted(null, _sendArgs);
            }
        }


        void OnSendCompleted(object? sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _sendlingList.Clear();

                        OnSend(_sendArgs.BytesTransferred);

                        if (_sendQueue.Count > 0)
                        {
                            RegsiterSend();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($" OnSendCompleted Errror -> {e.Message}");
                    }
                }
                else
                {
                    DisConnect();
                }
            }
        }
        #endregion


        #region 패킷을 받았음
        void RegisterRecv()
        {
            if (_disconnected == 1)
            {
                return;
            }

            if (_socket == null)
            {
                return;
            }

            if (_socket.Connected == false)
            {
                Console.WriteLine($"_socketConnected ERROR");
            }


            if (_socket.ReceiveAsync(_recvArgs) == false)
            {
                OnRecvCompleted(null, _recvArgs);
            }
        }


        //패킷을 받았으니.. 받은 버퍼에 써야함 
        void OnRecvCompleted(object? sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    // 내 버퍼에 쓰기 .. 실패하면 우선 종료 ..
                    // 추후 조정으로 새로운 버퍼를 할당받을 수 도 있다..
                    if (BufferManager.Instance.CanWrite(_recvArgs)==false)
                    {
                        DisConnect();
                        return;
                    }

                    // 실제로 받은 패킷 처리가 되는 곳
                    OnRecv(_recvArgs.BufferList!);
                }
                catch (Exception e)
                {
                    Console.WriteLine($" OnRecvCompleted Errror -> {e.Message}");
                }

                RegisterRecv();
            }
            else
            {
                Console.WriteLine($"Socket Err -> {args.RemoteEndPoint}");
                Console.WriteLine($"Socket Err -> {args.SocketError}");
                Console.WriteLine($"OnRecvCompleted -> {args.BytesTransferred}bytes");
                DisConnect();
            }
        }
        #endregion


        public void DisConnect()
        {
            //혼자 만 연결끊을 수 있도록 ... 다른 Session이나 다른 여러곳에서도 동시에 연결을 끊을 수 있음
            // - 유효하지 않은 행동을 했을 경우 --..
            if(Interlocked.Exchange(ref _disconnected, 1) == 1)
            {
                return;
            }

            Console.WriteLine($"DisConnect-> {_socket!.RemoteEndPoint}" );
            _socket?.Shutdown(SocketShutdown.Both);
            _socket?.Close();
        }
    }
}
