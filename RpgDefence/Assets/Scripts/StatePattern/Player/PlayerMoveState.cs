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

        // TODO : 자동공격을 체크 했을 경우 플레이어 사정거리 안에 몬스터가 들어오면 자동으로 공격처리
        /*if (_playerController.IsAutoAttack)
        {        
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, direction, out hitInfo, 1f, LayerMask.GetMask("Monster1")))
            {
                _playerController.PlayerState = Defines.State.Attack;
                return;
            }
        }*/
        
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
        // 애니메이션 툴과 싱크를 맞추는 작업을 하지않고 애니메이션 제어도 코드에서 관리하도록 설정 
        // 애니메이션이 많아질수록 코드에서 싱크를 맞춰주는 작업도 많아지므로 오히려 코드에서만 제어하도록 하는게 좋을 수도 있음
        anim.Play("RUN");        
    }

    // 애니메이션 이벤트 
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
