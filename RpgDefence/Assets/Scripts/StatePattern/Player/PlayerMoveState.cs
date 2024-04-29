using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    void UpdateMoving()
    {
        Vector3 direction = new Vector3(_playerController.Joystick.InputDirection.x, 0, _playerController.Joystick.InputDirection.y);
        
        // 못가는 지역 체크
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, 1.0f, LayerMask.GetMask("Block")))
        {
            if (Input.GetMouseButton(0) == false)
                _playerController.Wait();
            return;
        }

        // 캐릭터 이동, 회전
        transform.position += direction * _playerController.Stat.MoveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 20 * Time.deltaTime);
        MovingAnimationState(GetComponent<Animator>());        
    }

    public void MovingAnimationState(Animator anim)
    {        
        _playerController.PlayerState = Defines.State.Moving;
        anim.Play("RUN");        
    }             

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        Animator anim = GetComponent<Animator>();
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("WarriorATTACK"))
        {
            MovingAnimationState(GetComponent<Animator>());
            UpdateMoving();
        }
        
    }
}
