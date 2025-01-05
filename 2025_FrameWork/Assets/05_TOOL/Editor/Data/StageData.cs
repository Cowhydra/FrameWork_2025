using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private static string filePath = "Assets/03_ASSET_BUNDLE/data/StageData.csv";

    [MenuItem("Tools/Data/Generate Stage CSV")]
    private static void GenerateCSV()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("StageIndex,RequiredExperience,IsBossStage,Difficulty,ExpPerEnemy,GoldPerEnemy,MapResourceNumber,BossNumber");

            for (int i = 0; i < 100; i++)
            {
                int StageIndex = i+1;
                int RequiredExperience = Random.Range(50, 500);
                int IsBossStage = (i + 1) % 10 == 0 ? 1 : 0;
                int Difficulty = Random.Range(10, 100);
                int ExpPerEnemy = Random.Range(10, 100);
                int GoldPerEnemy = Random.Range(20, 100);
                int MapResourceNumber = Random.Range(1, 10);
                int BossNumber = Random.Range(1, 10);

                writer.WriteLine($"{StageIndex},{RequiredExperience},{IsBossStage},{Difficulty},{ExpPerEnemy},{GoldPerEnemy},{MapResourceNumber},{BossNumber}");
            }
        }

        Debug.Log($"CSV generated at: {filePath}");
        AssetDatabase.Refresh();
    }
}
