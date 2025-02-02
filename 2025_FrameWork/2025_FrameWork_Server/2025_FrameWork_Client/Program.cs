class Prmgram
{
    static void Main(string[] args)
    {
        NetWorkObject obj = new NetWorkObject();

        obj.ConnectToServer();


        while (true)
        {
            Thread.Sleep(1000);
            obj.T_LOGIN(NetWorkObject.N_E_LOGIN_TYPE.EDITOR);
        }
    }
}