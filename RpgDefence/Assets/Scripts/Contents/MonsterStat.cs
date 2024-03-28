using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    int gold;
    int exp;
    int score;

    public int Gold { get { return gold; } set { gold = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int Score { get { return score; } set { score = value; } }

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
            OnDead(attacker);
        }
    }

    protected override void OnDead(Stat attacker)
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
