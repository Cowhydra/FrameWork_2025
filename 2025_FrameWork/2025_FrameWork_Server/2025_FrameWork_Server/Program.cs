using System.Net;
using _2025_FrameWork_Server;
using ServerCore;


public class Program
{
    static void Main(string[] args)
    {
        //데이터 정리
        DataManager.LoadMonsterData();

        //서버 스타트(리슨)
        // 간단 ->  1개의 존서버
        Zone zoneManager = new Zone();

        zoneManager.Init();

    
        while (true)
        {
        }

    }
}