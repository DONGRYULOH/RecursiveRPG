using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 데이터를 관리하는 방법, 실제로 현장에서는 데이터와 코드를 분리해서 작업함
// ex) int hp = 100 --> 이런식으로 하드코딩해서 넣어주지 않고 데이터 파일에서 불러옴
public class DataManager
{    
    // 캐릭터 공통 스탯
    public Dictionary<int, Data.Stat> PlayerStatDic { get; private set; } = new Dictionary<int, Data.Stat>();

    // 전사 스탯
    public Dictionary<int, Data.PlayerWarriorStat> PlayerWarriorStatDic { get; private set; } = new Dictionary<int, Data.PlayerWarriorStat>();

    // 도적 스탯
    public Dictionary<int, Data.PlayerThiefStat> PlayerThiefStatDic { get; private set; } = new Dictionary<int, Data.PlayerThiefStat>();

    // 몬스터 공통 스탯
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
        return JsonUtility.FromJson<Loader>(textAsset.text); // <"읽어들일 JSON 파일 형식">
    }
}
