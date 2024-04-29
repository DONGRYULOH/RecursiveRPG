using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
        
    void WarriorAttackBeforeEvent()
    {             
        GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
        Transform childTransform = go.transform.GetComponentInChildren<Transform>().GetChild(0);        
        if (childTransform != null)
            childTransform.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void WarriorAttackAfterEvent()
    {        
        GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
        Transform childTransform = go.transform.GetComponentInChildren<Transform>().GetChild(0);
        if (childTransform != null)
            childTransform.gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }

    void ThiefAttackEvent()
    {
        GameObject bullet = Managers.Resource.Instantiate("Weapon/Shurikens/ThiefBullet");
        Vector3 direction = new Vector3(_playerController.Joystick.InputDirection.x, 0, _playerController.Joystick.InputDirection.y);
        bullet.GetComponent<Bullet>().Fire(direction, _playerController.transform.position);
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

        // ���� �ִϸ��̼� ��� ���� �� ���Ͱ� �׾��ų� �ڵ������� false ���¶�� ĳ���͸� wait ���·� ����
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {                        
            if (_playerController.StopAttack || !_playerController.IsAutoAttack)
            {                
                _playerController.Wait();                
            }
        }            
    }
}
