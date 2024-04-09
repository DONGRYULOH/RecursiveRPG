using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    void UpdateMoving()
    {
        Vector3 dir = _playerController.DestPos - transform.position;
        dir.y = 0; // RayCasting ��ǥ�� y���� �ִٸ� ���� �̵��ϹǷ� y���� �̵����� ���ϵ��� ����

        // 1.������ �Ǿ��ִ� ������Ʈ�� ���� �̵� (�÷��̾� �����Ÿ� �ȿ� ���Ͱ� ������ ���ݻ��·� ����)
        if (_playerController.LockTarget != null)
        {
            float distance = dir.magnitude;
            // �÷��̾��� �����Ÿ��ȿ� ���Ͱ� ������ ����ó��
            if (distance <= _playerController.Stat.AttackRange)
            {
                Managers.Game.GetPlayer().GetComponent<PlayerController>().StopSkill = false;
                _playerController.Skill();
                return;
            }
        }
        else if (dir.magnitude < 0.1f)
        {
            // 2.�ܼ��� �̵����� ���������� �����̴� ���
            _playerController.Wait();
            return;
        }
               
                             
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
        {
            if (Input.GetMouseButton(0) == false)
                _playerController.Wait();
            return;
        }

        float moveDist = Mathf.Clamp(_playerController.Stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
        transform.position += dir.normalized * moveDist;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        MovingAnimationState(GetComponent<Animator>());
        
    }

    public void MovingAnimationState(Animator anim)
    {        
        _playerController.PlayerState = Defines.State.Moving;
        // �ִϸ��̼� ���� ��ũ�� ���ߴ� �۾��� �����ʰ� �ִϸ��̼� ��� �ڵ忡�� �����ϵ��� ���� 
        // �ִϸ��̼��� ���������� �ڵ忡�� ��ũ�� �����ִ� �۾��� �������Ƿ� ������ �ڵ忡���� �����ϵ��� �ϴ°� ���� ���� ����
        anim.Play("RUN");        
    }

    // �ִϸ��̼� �̺�Ʈ 
    public void OnRunEvent() { }                

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        MovingAnimationState(GetComponent<Animator>());
        UpdateMoving();
    }
}
