using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    public void RockOnOrDeFaultEvent()
    {
        // 락온인 경우 해당 해당 대상을 바라보게 처리
        if (_playerController.LockTarget != null)
        {
            Vector3 dir = _playerController.LockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

        AttackAnimationState(GetComponent<Animator>());
    }

    // 애니메이션에 추가한 이벤트 발동
    void OnHitEvent()
    {
        // 캐릭터가 들고 있는 무기로부터 몬스터에게 접촉이 가해졌는지 체크
        GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
        Transform childTransform = go.transform.GetComponentInChildren<Transform>().GetChild(0);
        
        if (childTransform != null)
        {
            Vector3 direction = new Vector3(_playerController.Joystick.InputDirection.x, 0, _playerController.Joystick.InputDirection.y);
            RaycastHit hitInfo;
            if (Physics.Raycast(childTransform.position, direction, out hitInfo, 1f, LayerMask.GetMask("Monster1")))
            {
                MonsterStat targetStat = hitInfo.transform.GetComponent<MonsterStat>();
                targetStat.OnAttacked(_playerController.Stat);
            }
        }

        if (_playerController.StopAttack)
        {
            _playerController.Wait();
            return;
        }                
        _playerController.PlayerState = Defines.State.Attack;
    }

    public void AttackAnimationState(Animator anim)
    {
        _playerController.PlayerState = Defines.State.Attack;
        anim.Play("ATTACK");
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        RockOnOrDeFaultEvent();
        // 공격 애니메이션이 다 끝났을때 Wait 상태 변경
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (!_playerController.IsAutoAttack)
            {
                _playerController.Wait();
            }
        }            
    }
}
