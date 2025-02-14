using System.Net;
using _2025_FrameWork_Server.Object;
using ServerCore;

namespace _2025_FrameWork_Server
{
    //서버가 클라이언트에게 받은 부분
    public class ClientSession : PacketSession
    {
        //ushort -> 2바이트
        //int - > 4바이트 

        public Zone? Owner;
        public S_OBJECT_PLAYER? S_OBJECT_PLAYER;


        public enum N_E_PACKET_ID :ushort
        {
           CONNECT=10000,

           //오브젝트 처리
           OBJECT_MOVE=1,
           OBJECT_HP=2,
           OBJECT_MP=3,
           OBJECT_ACTION=4,
           OBJECT_INFO=5,


        


        }


        public enum N_E_LOGIN_TYPE : ushort
        {
            EDITOR=1,
            GOOGLE=2,
            APPLE=3,
        }

        public void Initainlize(Zone? owner)
        {
            //TODO: DB 서치 후 S_BOEJCT PLAYER 넣어주기
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
