using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    private void Start()
    {
        int level = 0;
        if (Managers.Game.CurrentChpater == 1)
        {
            level = 1;
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            level = 2;
        }
        else if (Managers.Game.CurrentChpater == 3)
        {
            level = 3;
        }

        if (GameObject.FindWithTag("BossMonster") != null)
        {
            Hp = 300;
            MaxHp = 300;
            Attack = 20;
            Defense = 20;
            MoveSpeed = 8;
            Gold = 1000;
            Exp = 1000;
            Score = 100;
        }
        else
        {
            SetStat(level);
        }
    }

    public void SetStat(int level)
    {        
        Dictionary<int, Data.MonsterStat> monsterStat = Managers.Data.MonsterStatDic;
        Hp = monsterStat[level].maxHp;
        MaxHp = monsterStat[level].maxHp;
        Attack = monsterStat[level].attack;
        Defense = monsterStat[level].defense;
        MoveSpeed = monsterStat[level].moveSpeed;
        Gold = monsterStat[level].gold;
        Exp = monsterStat[level].exp;
        Score = monsterStat[level].score;
    }

    public MonsterStat(int gold, int exp, int score)
    {
        this.gold = gold;
        this.exp = exp;
        this.score = score;
    }

    public override void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Managers.Game.GetPlayer().GetComponent<PlayerController>().StopAttack = true;
            OnDead(attacker);
        }
    }

    public override void OnDead(Stat attacker)
    {
        gameObject.GetComponent<MonsterController>().State = Defines.State.Die;
        PlayerStat playerStat = attacker as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Gold += this.Gold;
            playerStat.Exp += this.Exp;
            playerStat.Score += this.Score;            
        }
    }
}
