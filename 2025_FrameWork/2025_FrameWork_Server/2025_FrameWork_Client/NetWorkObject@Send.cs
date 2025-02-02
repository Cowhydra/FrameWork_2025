using System;

public partial class NetWorkObject 
{

    public void T_LOGIN(N_E_LOGIN_TYPE loginType)
    {
        // ��Ŷ ũ�� ��� (Size = ��Ŷ ID(2) + �α��� Ÿ��(2) + ���(2))
        ushort size = 2 + 2 + 2;
        ushort packetId = (ushort)N_E_PACKET_ID.CONNECT;

        // ���� ��û 
        ArraySegment<byte> s = sendBuffer.Open(4096);

        // ArraySegment�� ���� �����Ϸ��� Span<byte> ���
        Span<byte> buffer = s.Array.AsSpan(s.Offset, size);

        int offset = 0;
        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), size);
        offset += 2;

        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), packetId);
        offset += 2;

        BitConverter.TryWriteBytes(buffer.Slice(offset, 2), (ushort)loginType);
        offset += 2;

        if (serverSession != null)
        {
            // ��Ŷ ����
            serverSession.Send(s.Slice(0, size));  // ���� ����� ũ�⸸ŭ ����
        }

        Console.WriteLine($"T_LOGIN ->  {loginType}");
    }
}
