using UnityEngine;
using System.Net;
using System.Threading;
using System;
using ServerCore;
using UnityEngine.Rendering;

public partial class NetWorkObject : SingletonObj<NetWorkObject>
{
    private SendBuffer sendBuffer = new SendBuffer(4096 * 100);
    private ServerSession serverSession;

    //1. �񵿱� ����.. ���� ���� �ȵǾ����� ��� ó������..
    //2. ���� �Ǿ����� �����ϴ� �κ� Session�� �߰� 
    //3. ���� �����κ� & Ÿ�� �ƿ� ����..�ؼ� �ʹݿ� �˻��ϱ�..


    //TODO: ���⿡ ������ �� ���÷θ� ������ IP�� �̻��ϰ� ��� ������
    // ������ �ƴ������� �������� ��� ��ǻ���� ip4 address �� ��Ʈ �־��ָ� ��
    public void ConnectToServer()
    {
        try
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = null;
            foreach (IPAddress addr in ipHost.AddressList)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddr = addr;
                    break;
                }
            }

            IPEndPoint endPoint = new IPEndPoint(ipAddr, 54321);
            Connector connector = new Connector();

            //����ŷ ..
            connector.Connect(endPoint, () => {
                serverSession = new ServerSession();
                return serverSession;
            });
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
       
    }

    #region ������ �����Ǵ� ENUM ����
    public enum N_E_PACKET_ID : ushort
    {
        CONNECT = 1,
    }

    public enum N_E_LOGIN_TYPE : ushort
    {
        EDITOR = 1,
        GOOGLE = 2,
        APPLE = 3,
    }
    #endregion
}
