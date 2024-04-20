using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    MonsterStat _stat;
    private MonsterStateContext _monsterStateContext;
    private MonsterState dieState, moveState, waitState, skillState;
    private Defines.State state = Defines.State.Wait;    
    private float scanRange = 10;
    private float attackRange = 2;    
    private bool rangeCheck = true; // �÷��̾ �װų� �÷��̾� ��ó�� �����ϸ� �ݰ�Ÿ��� üũ���� ����

    public MonsterStat Stat { get { return _stat; } set { _stat = value; } }
    public Defines.State State { get { return state; } set { state = value; } }    
    public float ScanRange { get { return scanRange; } set { scanRange = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public bool RangeCheck { get { return rangeCheck; } set { rangeCheck = value; } }             

    public override void Init()
    {
        // Type ����
        WorldObjectType = Defines.WorldObject.Monster;

        // ����
        Stat = gameObject.GetOrAddComponent<MonsterStat>();        

        // HpBar UI ǥ��
        if (gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

        // ������ state ���� 
        StatePattern();
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
