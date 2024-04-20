using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : BaseController
{
    private PlayerStat _stat;

    private PlayerStateContext _playerStateContext;
    private PlayerState dieState, moveState, waitState, attackState, skillState;

    private int _mask = (1 << (int)Defines.Layer.Ground) | (1 << (int)Defines.Layer.Monster1); // Layer 마스킹 처리    

    // 몬스터와 플레이어가 공통으로 갖고 있는 상태(이동, 멈춤)도 있지만 서로 다른 상태(플레이어의 버프 상태 .)도 있을 수 있음
    private Defines.State currentPlayerState = Defines.State.Wait;

    private bool _stopAttack = false;
    private bool isAutoAttack;

    public Defines.State PlayerState { get { return currentPlayerState; } set { currentPlayerState = value; } }
    public PlayerStat Stat { get { return _stat; }}    
    public bool StopAttack { get { return _stopAttack; } set { _stopAttack = value; } }
    public bool IsAutoAttack { get { return isAutoAttack; } set { isAutoAttack = value; } }

    // 조이스틱 이동 정보
    [SerializeField]
    private VirtualJoystick joystick;
    public VirtualJoystick Joystick { get { return joystick; }}

    public override void Init()
    {
        // Type 설정
        WorldObjectType = Defines.WorldObject.Player;

        // 플레이어 스탯(체력, 방어력 ..등) 적용
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        // 플레이어 UpBar 생성
        Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);
        
        // state 패턴 호출
        StatePattern();

        // 조이스틱 정보 가져오기
        joystick = GameObject.FindWithTag("Joystick").GetComponent<VirtualJoystick>();
    }

    private void Update()
    {        
        switch (PlayerState)
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
            case Defines.State.Attack:
                Attack();
                break;
        }
    }    

    // -------------- 플레이어 state 패턴 --------------------
    public void StatePattern()
    {        
        _playerStateContext = new PlayerStateContext(this);        
        moveState = gameObject.AddComponent<PlayerMoveState>();
        waitState = gameObject.AddComponent<PlayerWaitState>();
        dieState = gameObject.AddComponent<PlayerDieState>();
        attackState = gameObject.AddComponent<PlayerAttackState>();
        skillState = gameObject.AddComponent<PlayerSkillState>();        
    }

    public void Move()
    {
        _playerStateContext.Transition(moveState);
    }

    public void Wait()
    {
        _playerStateContext.Transition(waitState);
    }

    public void Die()
    {
        _playerStateContext.Transition(dieState);
    }

    public void Attack()
    {
        _playerStateContext.Transition(attackState);
    }

    public void Skill()
    {
        _playerStateContext.Transition(skillState);
    }

}
