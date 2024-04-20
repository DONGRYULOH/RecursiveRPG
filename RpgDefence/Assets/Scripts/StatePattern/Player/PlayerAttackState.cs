using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    public void RockOnOrDeFaultEvent()
    {
        // ������ ��� �ش� �ش� ����� �ٶ󺸰� ó��
        if (_playerController.LockTarget != null)
        {
            Vector3 dir = _playerController.LockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

        AttackAnimationState(GetComponent<Animator>());
    }

    // �ִϸ��̼ǿ� �߰��� �̺�Ʈ �ߵ�
    void OnHitEvent()
    {
        // ĳ���Ͱ� ��� �ִ� ����κ��� ���Ϳ��� ������ ���������� üũ
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
        // ���� �ִϸ��̼��� �� �������� Wait ���� ����
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (!_playerController.IsAutoAttack)
            {
                _playerController.Wait();
            }
        }            
    }
}
