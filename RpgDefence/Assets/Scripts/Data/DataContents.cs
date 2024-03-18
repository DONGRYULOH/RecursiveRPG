using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이름을 Json 파일에 있는 이름과 똑같이 맞춰줘야지 파싱이 됨
/*
    "StatData.Json"
{
    "stats" : [
        {
            "level" : "1",
            "maxHp" : "100",
            "attack" : "20",
            "totalExp" : "0"
        },
        {
            "level" : "2",
            "maxHp" : "150",
            "attack" : "30",
            "totalExp" : "10"
        },
        {
            "level" : "3",
            "maxHp" : "200",
            "attack" : "60",
            "totalExp" : "20"
        }
    ]
}
*/
namespace Data
{
    #region Stat
    [Serializable] // [Serializable] 키워드를 붙여야지 Json 형식을 해당 형식에 맞춰서 데이터를 넣어줌
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class StatData : DataLoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            // TODO : 해당 오브젝트를 유일하게 식별할 수 있는 값을 key로 설정
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion
}