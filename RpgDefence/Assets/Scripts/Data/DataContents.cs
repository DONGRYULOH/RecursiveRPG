using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region PlayerStat
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
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion

    #region PlayerWarriorStat
    [Serializable] // [Serializable] 키워드를 붙여야지 Json 형식을 해당 형식에 맞춰서 데이터를 넣어줌
    public class PlayerWarriorStat
    {
        public int level;
        public int maxHp;
        public int maxMp;
        public int attack;
        public int defense;
        public int moveSpeed;
    }

    [Serializable]
    public class PlayerWarriorStatData : DataLoader<int, PlayerWarriorStat>
    {
        // JSON으로 읽어들인 파일 데이터
        public List<PlayerWarriorStat> stats = new List<PlayerWarriorStat>();

        public Dictionary<int, PlayerWarriorStat> MakeDict()
        {
            Dictionary<int, PlayerWarriorStat> dict = new Dictionary<int, PlayerWarriorStat>();

            foreach (PlayerWarriorStat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion

    #region PlayerThiefStat
    [Serializable] // [Serializable] 키워드를 붙여야지 Json 형식을 해당 형식에 맞춰서 데이터를 넣어줌
    public class PlayerThiefStat
    {
        public int level;
        public int maxHp;
        public int maxMp;
        public int attack;
        public int defense;
        public int moveSpeed;
    }

    [Serializable]
    public class PlayerThiefStatData : DataLoader<int, PlayerThiefStat>
    {
        // JSON으로 읽어들인 파일 데이터
        public List<PlayerThiefStat> stats = new List<PlayerThiefStat>();

        public Dictionary<int, PlayerThiefStat> MakeDict()
        {
            Dictionary<int, PlayerThiefStat> dict = new Dictionary<int, PlayerThiefStat>();

            foreach (PlayerThiefStat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion

    #region MonsterStat
    [Serializable] // [Serializable] 키워드를 붙여야지 Json 형식을 해당 형식에 맞춰서 데이터를 넣어줌
    public class MonsterStat
    {
        public int level;
        public int maxHp;        
        public int attack;
        public int defense;
        public int moveSpeed;
        public int gold;
        public int exp;
        public int score;
    }

    [Serializable]
    public class MonsterStatData : DataLoader<int, MonsterStat>
    {        
        public List<MonsterStat> stats = new List<MonsterStat>();

        public Dictionary<int, MonsterStat> MakeDict()
        {
            Dictionary<int, MonsterStat> dict = new Dictionary<int, MonsterStat>();

            foreach (MonsterStat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion
}