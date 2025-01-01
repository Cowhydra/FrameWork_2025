using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MonsterData))]
public class MonsterDataEditor : Editor
{
    private static string filePath = "Assets/03_ASSET_BUNDLE/data/MonsterData.csv";

    [MenuItem("Tools/Data/Generate Monster CSV")]
    private static void GenerateCSV()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("UniqueKey,Health,Mana,AttackPower,AttackRange,MoveSpeed,DetectionRange,ChaseRange,ModelNumber,DropItem,AttackType,BulletID");

            for (int i = 0; i < 10; i++)
            {
                int uniqueKey = Random.Range(1000, 9999);
                int health = Random.Range(50, 500);
                int mana = Random.Range(20, 200);
                int attackPower = Random.Range(10, 100);
                float attackRange = Random.Range(1.5f, 10f);
                float moveSpeed = Random.Range(2f, 10f);
                float detectionRange = Random.Range(5f, 20f);
                float chaseRange = Random.Range(10f, 30f);
                int modelNumber = Random.Range(1, 10);
                int attackType = Random.Range(0, 2);
                int bulletID = Random.Range(1, 100);

                // 랜덤 드롭 아이템 생성
                string dropItemsField = $"{Random.Range(1, 100)};{Random.Range(1, 100)};{Random.Range(1, 100)}";

                writer.WriteLine($"{uniqueKey},{health},{mana},{attackPower},{attackRange:F1},{moveSpeed:F1},{detectionRange:F1},{chaseRange:F1},{modelNumber},{dropItemsField}.{attackType},{bulletID}");
            }
        }

        Debug.Log($"CSV generated at: {filePath}");
        AssetDatabase.Refresh();
    }
}
