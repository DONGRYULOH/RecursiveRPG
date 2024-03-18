using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp { 
        get { return _exp; }
        // 몬스터가 죽었을때 경험치를 주는 것 뿐만 아니라 퀘스트를 완료하거나 어떠한 이벤트를 수행했을때도 경험치를 주기 때문에 경험치가 변경되는 것을 공통으로 만듬
        set
        {
            // 1.경험치가 깎였을때 현재 레벨의 경험치를 충족하는 값 아래로 내려가지 않도록 설정            
            _exp = value;
            if (Managers.Data.StatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.StatDic[Level];
                if (_exp < stat.totalExp) _exp = stat.totalExp; // 현재 레벨을 충족하는 최소 EXP 값으로 세팅                
            }

            // 2.레벨업 체크
            // 해당 경험치가 어느 레벨에 도달했는지 체크 후 도달한 레벨로 변경
            int level = Level; 
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDic.TryGetValue(level + 1, out stat) == false) // 그 다음 레벨이 없는 경우 (만렙)
                    break;
                if (_exp < stat.totalExp) // 그 다음 레벨이 있으면 현재 경험치가 다음 레벨 경험치를 충족하는지 체크
                    break;
                level = stat.level;
            }

            if(Level != level)
            {
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 2;
        _defense = 5;
        _moveSpeed = 10.0f;
        _exp = 10;
        _gold = 0;
        SetStat(_level);
    }

    // 레벨업 할때마다 해당 플레이어의 스텟을 변경
    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> stat = Managers.Data.StatDic;
        _hp = stat[_level].maxHp;
        _maxHp = stat[_level].maxHp;
        _attack = stat[_level].attack;
    }

    protected override void OnDead(Stat attacker)
    {
        if(gameObject != null)
        {
            gameObject.GetComponent<PlayerStat>().Exp -= 10;
        }

        Debug.Log("player 경험치 감소");
    }
}
