using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static partial class AssetServer
{
    public static Lazy<Dictionary<int, MonsterData>> MonsterDataDict = new(() =>
    {
        TotalResourceDict.TryGetValue("data/MonsterData", out UnityEngine.Object Value);
        if (Value == null)
        {
            Debug.LogError("MonsterData.csv not found in TotalResourceDict");
            return new Dictionary<int, MonsterData>();
        }
        TextAsset asset = Value as TextAsset;
        if (asset == null)
        {
            Debug.LogError("Value is Not TextAsset");
            return new Dictionary<int, MonsterData>();
        }

        List <MonsterData> monsterList = new();
        string[] lines = asset.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] fields = lines[i].Split(',');
            int uniqueKey = int.Parse(fields[0]);
            MonsterData monster = new MonsterData(uniqueKey, fields);
            monsterList.Add(monster);
        }

        return monsterList.ToDictionary(x => x.UniqueKey, x => x);
    });

}
