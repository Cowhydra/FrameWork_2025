using D_F_Enum;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public readonly int StageIndex;         // ���� �������� �ε���
    public readonly int RequiredExperience; // �䱸 ����ġ
    public readonly bool IsBossStage;       // ���� �������� ����
    public readonly int Difficulty;         // ���̵� (1~5)
    public readonly int ExpPerEnemy;        // ������ ����ġ
    public readonly int GoldPerEnemy;       // ������ ���
    public readonly int MapResourceNumber;  // �� ���ҽ� ��ȣ (���ҽ� ���Ͽ� �ش��ϴ� ��ȣ)
    public readonly int BossNumber;         // ���� ��ȣ

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
