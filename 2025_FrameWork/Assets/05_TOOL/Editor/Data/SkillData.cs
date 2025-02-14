using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    private static string filePath = "Assets/03_ASSET_BUNDLE/data/SkillData.csv";

    [MenuItem("Tools/Data/Generate Skill CSV")]
    private static void GenerateCSV()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("SkillIndex,Duration,TotalHitCount,HitFrames,DamagePercent,ArmorPercent,SkillType,ManaCost,Cooldown,IsProjectile,ProjectileID,ActionID");

            for (int i = 0; i < 10; i++)
            {
                int skillIndex = i + 1;
                float duration = Random.Range(0.5f, 5f);
                int totalHitCount = Random.Range(1, 5);
                string hitFrames = $"{Random.Range(5, 30)};{Random.Range(35, 60)};{Random.Range(65, 90)};{Random.Range(95, 120)}";
                float damagePercent = Random.Range(50f, 200f);
                float armorPercent = Random.Range(0f, 100f);
                int skillType = Random.Range(1, 4); // PASSIVE(1) or SINGLE(2) or MULTI(3) or AREA(4)
                int manaCost = Random.Range(10, 100);
                float cooldown = Random.Range(1f, 10f);
                bool isProjectile = Random.value > 0.5f;
                int projectileID = isProjectile ? Random.Range(1, 50) : -1;
                int actionID = Random.Range(1, 10);
                int DisTance = Random.Range(100, 200);

                writer.WriteLine($"{skillIndex},{duration:F1},{totalHitCount},{hitFrames},{damagePercent:F1},{armorPercent:F1},{skillType},{manaCost},{cooldown:F1},{isProjectile},{projectileID},{actionID}.{DisTance}");
            }
        }

        Debug.Log($"Skill CSV generated at: {filePath}");
        AssetDatabase.Refresh();
    }
}
