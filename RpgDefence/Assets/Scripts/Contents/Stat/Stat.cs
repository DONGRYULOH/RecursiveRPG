using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���͵�, ĳ���͵� ���������� ������ �ִ� �͵��� ���� ����

public class Stat : MonoBehaviour
{    
    protected int _level;    
    protected int _hp;    
    protected int _maxHp;    
    protected int _attack;    
    protected int _defense;    
    protected float _moveSpeed;
    protected int gold;
    protected int exp;
    protected int score;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public int Gold { get { return gold; } set { gold = value; } }
    public virtual int Exp { get { return exp; } set { exp = value; } }
    public int Score { get { return score; } set { score = value; } }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    // ** ���Ͱ� �÷��̾ �׿����� Stat���� ��ĳ������ �ؼ� �޾Ҵµ� PlayerStat�� override�� OnDead()�Լ��� �۵��ϴ� ������?
    public virtual void OnDead(Stat attacker)
    {                
        Managers.Game.DeSpawn(gameObject);
    }
}
