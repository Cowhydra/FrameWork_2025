using System.Net;
using ServerCore;

namespace _2025_FrameWork_Server
{
    //서버가 클라이언트에게 받은 부분
    public class ClientSession : PacketSession
    {
        //ushort -> 2바이트
        //int - > 4바이트 

        public enum N_E_PACKET_ID :ushort
        {
           CONNECT=1,

        }

        public enum N_E_LOGIN_TYPE : ushort
        {
            EDITOR=1,
            GOOGLE=2,
            APPLE=3,
        }


        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
        }


        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            int offset = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array!, buffer.Offset);
            offset += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array!, buffer.Offset + offset);
            offset += 2;

            switch ((N_E_PACKET_ID)id)
            {
                case N_E_PACKET_ID.CONNECT:
                    ushort loginmethod = BitConverter.ToUInt16(buffer.Array!, buffer.Offset + offset);
                    offset += 2;
                    Console.WriteLine($"loginmethod: {(N_E_LOGIN_TYPE)loginmethod}");
                    break;
                default:
                    break;
            }

            Console.WriteLine($"RecvPacketId: {id}, Size {size}");
        }


        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
