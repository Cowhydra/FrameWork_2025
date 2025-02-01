using System;
using UnityEngine;

public partial class NetWorkObject : SingletonObj<NetWorkObject>
{

    public void T_LOGIN(N_E_LOGIN_TYPE loginType)
    {
        // 패킷 크기 계산 (Size = 패킷 ID(2) + 로그인 타입(2) + 헤더(2))
        ushort size = 2 + 2 + 2;
        ushort packetId = (ushort)N_E_PACKET_ID.CONNECT;

        // 버퍼 요청 
        ArraySegment<byte> s = sendBuffer.Open(4096);

        // ArraySegment를 직접 수정하려면 Span<byte> 사용
        Span<byte> buffer = s.Array.AsSpan(s.Offset, size);

        int offset = 0;
        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), size);
        offset += 2;

        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), packetId);
        offset += 2;

        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), (ushort)loginType);
        offset += 2;

        // 패킷 전송
        serverSession.Send(s.Slice(0, size));  // 실제 사용한 크기만큼 전송

        Debug.Log($"T_LOGIN ->  {loginType}");
    }
}
