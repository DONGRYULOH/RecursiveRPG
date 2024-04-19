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

    public Defines.State PlayerState { get { return currentPlayerState; } set { currentPlayerState = value; } }
    public PlayerStat Stat { get { return _stat; }}    
    public bool StopAttack { get { return _stopAttack; } set { _stopAttack = value; } }

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

        // 입력(마우스) 발생시 플레이어에게 발생할 이벤트
        Managers.Input.mouseAction -= OnMouseEvent;
        Managers.Input.mouseAction += OnMouseEvent;

        // 입력(키보드) 발생시 플레이어에게 발생할 이벤트
        Managers.Input.keyAction -= OnKeyEvent;
        Managers.Input.keyAction += OnKeyEvent;

        // state 패턴 호출
        StatePattern();

        // 조이스틱 정보 가져오기
        joystick = GameObject.FindWithTag("Joystick").GetComponent<VirtualJoystick>();
    }

    private void Update()
    {
        Managers.Input.MouseActionCheck();
        Managers.Input.KeyActionCheck();

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

    void OnKeyEvent(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Q:
                Skill();                
                break;
        }
    }

    // 플레이어가 스킬을 시전중인 상태일때 마우스를 클릭하는 경우 다른 메소드가 실행되면 안되기 때문에 분기처리를 해줬음
    void OnMouseEvent(Defines.MouseEvent evt)
    {        
        switch (PlayerState)
        {
            case Defines.State.Wait:
                OnMouseEvent_IdleRun(evt);
                break;
            case Defines.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Defines.State.Attack: 
                {
                    // 공격 애니메이션이 끝난 경우만 다른 상태로 변환 가능
                    if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)                    
                        OnMouseEvent_IdleRun(evt);                    
                }                
                break;
        }
    }
    
    void OnMouseEvent_IdleRun(Defines.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 50.0f, _mask);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 50.0f, Color.red, 2.0f);

        switch (evt)
        {            
            case Defines.MouseEvent.PointerDown: // 마우스 클릭을 놓은 상태에서 최초로 해당 지점을 마우스로 클릭했을 때
                {
                    if (raycastHit)
                    {                                                
                        if (hit.collider.gameObject.layer == (int)Defines.Layer.Monster1)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;

                        destPos = hit.point;                        
                        Move();
                    }
                }
                break;
            case Defines.MouseEvent.Press: // 마우스를 계속 누르고 있는 상태
                {
                    if (_lockTarget == null && raycastHit)
                        destPos = hit.point;
                }
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
