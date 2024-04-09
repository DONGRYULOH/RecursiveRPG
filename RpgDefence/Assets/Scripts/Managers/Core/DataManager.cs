using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����͸� �����ϴ� ���, ������ ���忡���� �����Ϳ� �ڵ带 �и��ؼ� �۾���
// ex) int hp = 100 --> �̷������� �ϵ��ڵ��ؼ� �־����� �ʰ� ������ ���Ͽ��� �ҷ���
public class DataManager
{    
    // ĳ���� ���� ����
    public Dictionary<int, Data.Stat> PlayerStatDic { get; private set; } = new Dictionary<int, Data.Stat>();

    // ���� ����
    public Dictionary<int, Data.PlayerWarriorStat> PlayerWarriorStatDic { get; private set; } = new Dictionary<int, Data.PlayerWarriorStat>();

    // ���� ����
    public Dictionary<int, Data.PlayerThiefStat> PlayerThiefStatDic { get; private set; } = new Dictionary<int, Data.PlayerThiefStat>();

    // ���� ���� ����
    public Dictionary<int, Data.MonsterStat> MonsterStatDic { get; private set; } = new Dictionary<int, Data.MonsterStat>();

    public void Init()
    {        
        Data.StatData loader = LoadJson<Data.StatData, int, Data.Stat>("StatData");
        PlayerStatDic = loader.MakeDict();

        Data.PlayerWarriorStatData loader2 = LoadJson<Data.PlayerWarriorStatData, int, Data.PlayerWarriorStat>("PlayerWarriorStatData");
        PlayerWarriorStatDic = loader2.MakeDict();

        Data.PlayerThiefStatData loader3 = LoadJson<Data.PlayerThiefStatData, int, Data.PlayerThiefStat>("PlayerThiefStatData");
        PlayerThiefStatDic = loader3.MakeDict();

        Data.MonsterStatData loader4 = LoadJson<Data.MonsterStatData, int, Data.MonsterStat>("MonsterStatData");
        MonsterStatDic = loader4.MakeDict();
    }   

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : DataLoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text); // <"�о���� JSON ���� ����">
    }
}
