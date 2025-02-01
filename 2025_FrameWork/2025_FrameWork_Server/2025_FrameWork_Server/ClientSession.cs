using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace _2025_FrameWork_Server
{
    //서버가 클라이언트에게 받은 부분
    public class ClientSession : PacketSession
    {
        public enum PACKET_ID :ushort
        {
           
        }


        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            Thread.Sleep(5000);
            DisConnect();
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            int offset = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array!, buffer.Offset);
            offset += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array!, buffer.Offset + offset);
            offset += 2;

            switch ((PACKET_ID)id)
            {
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
