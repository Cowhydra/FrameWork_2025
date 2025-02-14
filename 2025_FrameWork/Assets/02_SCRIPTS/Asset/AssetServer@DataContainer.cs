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



    public static Lazy<Dictionary<int, StageData>> StageDataDict = new(() =>
    {
        TotalResourceDict.TryGetValue("data/StageData", out UnityEngine.Object Value);
        if (Value == null)
        {
            Debug.LogError("StageData.csv not found in TotalResourceDict");
            return new Dictionary<int, StageData>();
        }
        TextAsset asset = Value as TextAsset;
        if (asset == null)
        {
            Debug.LogError("Value is Not TextAsset");
            return new Dictionary<int, StageData>();
        }

        List<StageData> stageList = new();
        string[] lines = asset.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] fields = lines[i].Split(',');
            int uniqueKey = int.Parse(fields[0]);
            StageData monster = new StageData(uniqueKey, fields);
            stageList.Add(monster);
        }

        return stageList.ToDictionary(x => x.StageIndex, x => x);
    });



    public static Lazy<Dictionary<int, SkillData>> SkillDataDict = new(() =>
    {
        TotalResourceDict.TryGetValue("data/SkillData", out UnityEngine.Object Value);
        if (Value == null)
        {
            Debug.LogError("SkillData.csv not found in TotalResourceDict");
            return new Dictionary<int, SkillData>();
        }

        TextAsset asset = Value as TextAsset;
        if (asset == null)
        {
            Debug.LogError("Value is Not TextAsset");
            return new Dictionary<int, SkillData>();
        }

        List<SkillData> stageList = new();
        string[] lines = asset.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] fields = lines[i].Split(',');
            int uniqueKey = int.Parse(fields[0]);
            SkillData monster = new SkillData(uniqueKey, fields);
            stageList.Add(monster);
        }

        return stageList.ToDictionary(x => x.SkillIndex, x => x);
    });
}
