using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
        
    void WarriorAttackEvent()
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
    }

    void ThiefAttackEvent()
    {
        
    }    

    void AnimationAttackPlay(Animator anim)
    {        
        Defines.PlayerJob job = _playerController.Stat.Job;
        if (job == Defines.PlayerJob.Warrior)
        {
            anim.Play("WarriorATTACK");            
        }
        else if (job == Defines.PlayerJob.Thief)
        {
            anim.Play("ThiefATTACK");
        }
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        AnimationAttackPlay(GetComponent<Animator>());

        // 공격 애니메이션 이후 몬스터가 죽었거나 자동공격이 false 상태라면 캐릭터를 wait 상태로 변경
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {                        
            if (_playerController.StopAttack || !_playerController.IsAutoAttack)
            {
                _playerController.Wait();                
            }
        }            
    }
}
