using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
        
    void WarriorAttackEvent()
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

        // ���� �ִϸ��̼� ���� ���Ͱ� �׾��ų� �ڵ������� false ���¶�� ĳ���͸� wait ���·� ����
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {                        
            if (_playerController.StopAttack || !_playerController.IsAutoAttack)
            {
                _playerController.Wait();                
            }
        }            
    }
}
