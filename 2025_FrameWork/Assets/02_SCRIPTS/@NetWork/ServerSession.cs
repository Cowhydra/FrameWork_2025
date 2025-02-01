using UnityEngine;
using ServerCore;
using System.Net;
using System.Collections.Generic;
using System;
using static NetWorkObject;


//클라가 가지고 다니는 서버 
//각 서버가 Session  존을 바꾸면 Session을 바꿔주면 됨 
public class ServerSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"OnConnected : {endPoint}");
    }


    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"OnDisconnected : {endPoint}");
    }

    //서버랑 같을 듯 
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
            NetWorkObject.Instance.R_LOGIN((N_E_LOGIN_TYPE)loginmethod);
        }
        else
        {
            Debug.LogWarning($"Un Define Packet ID : {id.ToString()}");
        }
    
        Debug.Log($"RecvPacketId: {id}, Size {size}");
    }

    public override void OnSend(int numOfBytes)
    {
        Debug.Log($"Transferred bytes: {numOfBytes}");
    }

}
