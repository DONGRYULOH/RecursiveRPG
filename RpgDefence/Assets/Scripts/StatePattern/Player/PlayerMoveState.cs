using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    void UpdateMoving()
    {
        // TODO : �ڵ������� üũ ���� ��� �÷��̾� �����Ÿ� �ȿ� ���Ͱ� ������ �ڵ����� ����ó��

        Vector3 direction = new Vector3(_playerController.Joystick.InputDirection.x, 0, _playerController.Joystick.InputDirection.y);

        // ������ ���� üũ
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
        {
            if (Input.GetMouseButton(0) == false)
                _playerController.Wait();
            return;
        }

        // ĳ���� �̵�, ȸ��
        transform.position += direction * _playerController.Stat.MoveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 20 * Time.deltaTime);
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
