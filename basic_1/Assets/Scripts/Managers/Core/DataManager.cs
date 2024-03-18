using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 데이터를 관리하는 방법, 실제로 현장에서는 데이터와 코드를 분리해서 작업함
// ex) int hp = 100 --> 이런식으로 하드코딩해서 넣어주지 않고 데이터 파일에서 불러옴
public class DataManager
{
    // 스탯, NPC, ... 등등 별로 나눠서 관리

    public Dictionary<int, Data.Stat> StatDic { get; private set; } = new Dictionary<int, Data.Stat>();
    // public Dictionary<string, Data.NPC> NpcDic { get; private set; } = new Dictionary<string, Data.NPC>();

    public void Init()
    {
        // "StatData"라는 Json 파일을 해당 타입(Loader)에 맞춰서 파싱
        Data.StatData loader = LoadJson<Data.StatData, int, Data.Stat>("StatData");
        StatDic = loader.MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : DataLoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text); // json 파일을 해당 Loader 타입(StatData)에 맞춰서 파싱(변환)        
    }
}
