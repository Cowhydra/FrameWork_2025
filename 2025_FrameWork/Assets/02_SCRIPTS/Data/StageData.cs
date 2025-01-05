using D_F_Enum;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public readonly int StageIndex;         // 현재 스테이지 인덱스
    public readonly int RequiredExperience; // 요구 경험치
    public readonly bool IsBossStage;       // 보스 스테이지 여부
    public readonly int Difficulty;         // 난이도 (1~5)
    public readonly int ExpPerEnemy;        // 마리당 경험치
    public readonly int GoldPerEnemy;       // 마리당 골드
    public readonly int MapResourceNumber;  // 맵 리소스 번호 (리소스 파일에 해당하는 번호)
    public readonly int BossNumber;         // 보스 번호

    public StageData(int UniqueKey, string[] fields)
    {
        StageIndex = UniqueKey;
        RequiredExperience = int.Parse(fields[1]);
        IsBossStage = int.Parse(fields[2]) == 1 ? true : false;
        Difficulty = int.Parse(fields[3]);
        ExpPerEnemy = int.Parse(fields[4]);
        GoldPerEnemy = int.Parse(fields[5]);
        MapResourceNumber = int.Parse(fields[6]);
        BossNumber=int.Parse(fields[7]);
    }
}
