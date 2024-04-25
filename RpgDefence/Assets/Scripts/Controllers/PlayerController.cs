using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : BaseController
{
    private PlayerStat _stat;
    private PlayerStateContext _playerStateContext;
    private PlayerState dieState, moveState, waitState, attackState, skillState;          
    private Defines.State currentPlayerState = Defines.State.Wait;
    private bool _stopAttack = false;
    private bool isAutoAttack;

    public PlayerStat Stat { get { return _stat; } }
    public Defines.State PlayerState { get { return currentPlayerState; } set { currentPlayerState = value; } }     
    public bool StopAttack { get { return _stopAttack; } set { _stopAttack = value; } }
    public bool IsAutoAttack { get { return isAutoAttack; } set { isAutoAttack = value; } }

    // 조이스틱 이동 정보
    [SerializeField]
    private VirtualJoystick joystick;
    public VirtualJoystick Joystick { get { return joystick; } set { joystick = value; } }

    public override void Init()
    {
        // Type 설정
        WorldObjectType = Defines.WorldObject.Player;

        // 플레이어 스탯(체력, 방어력 ..등) 적용
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        // 플레이어 UpBar 생성
        Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

        // 직업에 따른 무기 착용(전사 : 대검, 도적 : 아대)
        WeaponPutOn();
        
        // state 패턴 호출
        StatePattern();
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

    void WeaponPutOn()
    {
        GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
        if (Stat.Job == Defines.PlayerJob.Warrior)
        {
            GameObject obj = Managers.Resource.Instantiate("Weapon/LongSword");            
            obj.transform.parent = go.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.identity;
        }
        else if (Stat.Job == Defines.PlayerJob.Thief)
        {
            GameObject obj = Managers.Resource.Instantiate("Weapon/Chakram");            
            obj.transform.parent = go.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.identity;
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
