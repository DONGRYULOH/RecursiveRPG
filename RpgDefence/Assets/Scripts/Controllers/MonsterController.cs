using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    MonsterStat _stat;

    private MonsterStateContext _monsterStateContext;
    private MonsterState dieState, moveState, waitState, skillState;

    private Defines.State state = Defines.State.Wait;

    [SerializeField]
    float _scanRange = 10;

    [SerializeField]
    float _attackRange = 2;

    // �÷��̾ �װų� �÷��̾� ��ó�� �����ϸ� �ݰ�Ÿ��� üũ���� ����
    private bool rangeCheck = true;

    public bool RangeCheck { get { return rangeCheck; } set { rangeCheck = value; } }
    public float ScanRange { get { return _scanRange; } set { _scanRange = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public Defines.State State { get { return state; } set { state = value; } }
    public MonsterStat Stat { get { return _stat; } set { _stat = value; } }    

    public override void Init()
    {
        // Type ����
        WorldObjectType = Defines.WorldObject.Monster;

        // ����
        _stat = gameObject.GetOrAddComponent<MonsterStat>();
        if(Managers.Game.CurrentChpater == 1)
        {
            Stat.Level = 1;            
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            Stat.Level = 2;            
        }
        else if (Managers.Game.CurrentChpater == 3)
        {
            Stat.Level = 3;            
        }

        if(GameObject.FindWithTag("BossMonster") != null)
        {            
            Stat.Hp = 300;
            Stat.MaxHp = 300;
            Stat.Attack = 20;
            Stat.Defense = 20;
            Stat.MoveSpeed = 8;
            Stat.Gold = 1000;
            Stat.Exp = 1000;
            Stat.Score = 100;
        }
        else
        {
            SetStat();
        }        

        // HpBar UI ǥ��
        if (gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

        // ������ state ���� 
        StatePattern();
    }

    void SetStat()
    {
        int level = Stat.Level;
        Dictionary<int, Data.MonsterStat> monsterStat = Managers.Data.MonsterStatDic;
        Stat.Hp = monsterStat[level].maxHp;
        Stat.MaxHp = monsterStat[level].maxHp;
        Stat.Attack = monsterStat[level].attack;
        Stat.Defense = monsterStat[level].defense;
        Stat.MoveSpeed = monsterStat[level].moveSpeed;
        Stat.Gold = monsterStat[level].gold;
        Stat.Exp = monsterStat[level].exp;
        Stat.Score = monsterStat[level].score;
    }

    private void Update()
    {
        switch (state)
        {
            case Defines.State.Die:
                Die();
                break;
            case Defines.State.Moving:
                Move();
                break;
            case Defines.State.Wait:
                Wait();
                break;
            case Defines.State.Skill:
                Skill();
                break;
        }
    }


    // -------------- ���� state ���� --------------------
    public void StatePattern()
    {
        _monsterStateContext = new MonsterStateContext(this);

        // PlayerController ������Ʈ�� �پ��ִ� ������Ʈ�� PlayerMoveState Ŭ������ ������Ʈ�� ����
        moveState = gameObject.AddComponent<MonsterMoveState>();
        waitState = gameObject.AddComponent<MonsterWaitState>();
        dieState = gameObject.AddComponent<MonsterDieState>();
        skillState = gameObject.AddComponent<MonsterSkillState>();
    }

    public void Move()
    {
        _monsterStateContext.Transition(moveState);
    }

    public void Wait()
    {
        _monsterStateContext.Transition(waitState);
    }

    public void Die()
    {
        _monsterStateContext.Transition(dieState);
    }

    public void Skill()
    {
        _monsterStateContext.Transition(skillState);
    }
}
