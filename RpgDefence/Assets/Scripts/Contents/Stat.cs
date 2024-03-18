using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터든, 캐릭터든 공통적으로 가지고 있는 것들을 먼저 구현

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

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

    protected virtual void OnDead(Stat attacker)
    {
        // PlayerStat의 기반(부모) 클래스가 Stat이기 때문에 as 연산자를 이용해서 파생(자식) 클래스인 PlayerStat로 변환
        // as 연산자는 부모 자식간의 형식 변환에 사용되는 문법으로 as [자격]에 해당되지 않으면 null을 리턴한다.
        // "attacker"가 PlayerStat의 자격에 맞으면 그 자격으로 변환
        PlayerStat playerStat = attacker as PlayerStat;
        if(playerStat != null)
        {
            // TODO : 몬스터 별로 주는 Exp가 다르기 때문에 나중에 데이터 시트를 만들어서 체인지 하기
            playerStat.Exp += 5;
        }        

        Managers.Game.DeSpawn(gameObject);
    }
}
