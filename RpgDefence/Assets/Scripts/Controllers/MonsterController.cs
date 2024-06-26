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
    private bool rangeCheck = true; // 플레이어가 죽거나 플레이어 근처에 도착하면 반경거리를 체크하지 않음

    public MonsterStat Stat { get { return _stat; } set { _stat = value; } }
    public Defines.State State { get { return state; } set { state = value; } }    
    public float ScanRange { get { return scanRange; } set { scanRange = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public bool RangeCheck { get { return rangeCheck; } set { rangeCheck = value; } }             

    public override void Init()
    {
        // Type 설정
        WorldObjectType = Defines.WorldObject.Monster;

        // 스탯
        Stat = gameObject.GetOrAddComponent<MonsterStat>();        

        // HpBar UI 표시
        if (gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

        // 몬스터의 state 패턴 
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

    // -------------- 몬스터 state 패턴 --------------------
    public void StatePattern()
    {
        _monsterStateContext = new MonsterStateContext(this);

        // PlayerController 컴포넌트가 붙어있는 오브젝트에 PlayerMoveState 클래스도 컴포넌트로 붙임
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
