using System.IO;

internal class DataManager
{
    public static Dictionary<int, MonsterData> MonsterDataDict = new Dictionary<int, MonsterData>();

    public static void LoadMonsterData()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "MonsterData.csv");

        if (!File.Exists(path))
        {
            Console.WriteLine($"[Error] Monster data file not found: {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)  // 첫 줄(헤더) 건너뛰기
        {
            string[] fields = lines[i].Split(',');

            if (fields.Length < 12)
            {
                Console.WriteLine($"[Warning] Invalid data at line {i}: {lines[i]}");
                continue;
            }

            int uniqueKey = int.Parse(fields[0]);
            MonsterData monsterData = new MonsterData(uniqueKey, fields);
            MonsterDataDict[uniqueKey] = monsterData;
        }

        Console.WriteLine($"[Info] Loaded {MonsterDataDict.Count} monsters from CSV.");
    }

    public static MonsterData? GetMonsterData(int uniqueKey)
    {
        if (MonsterDataDict.TryGetValue(uniqueKey, out MonsterData? monsterData))
        {
            return monsterData;
        }

        Console.WriteLine($"[Warning] MonsterData with UniqueKey {uniqueKey} not found.");

        return null;
    }
}
