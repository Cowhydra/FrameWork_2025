using ServerCore;
using System.Net;
using System.Collections.Generic;
using System;
using static NetWorkObject;


//Ŭ�� ������ �ٴϴ� ���� 
//�� ������ Session  ���� �ٲٸ� Session�� �ٲ��ָ� �� 
public class ServerSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"OnConnected : {endPoint}");
    }


    public override void OnDisconnected(EndPoint endPoint)
    {
        Console.WriteLine($"OnDisconnected : {endPoint}");
    }

    //������ ���� �� 
    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        int offset = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array!, buffer.Offset);
        offset += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array!, buffer.Offset + offset);
        offset += 2;

        if ((N_E_PACKET_ID)id == N_E_PACKET_ID.CONNECT)
        {
            ushort loginmethod = BitConverter.ToUInt16(buffer.Array!, buffer.Offset + offset);
            offset += 2;
          
        }
        else
        {
            Console.WriteLine($"Un Define Packet ID : {id.ToString()}");
        }

        Console.WriteLine($"RecvPacketId: {id}, Size {size}");
    }

    public override void OnSend(int numOfBytes)
    {
        Console.WriteLine($"Transferred bytes: {numOfBytes}");
    }

}
