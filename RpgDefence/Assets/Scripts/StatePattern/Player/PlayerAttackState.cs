using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
        
    void WarriorAttackBeforeAniEvt()
    {             
        GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
        Transform childTransform = go.transform.GetComponentInChildren<Transform>().GetChild(0);        
        if (childTransform != null)
            childTransform.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void WarriorAttackAfterAniEvt()
    {                
        _playerController.PlayerState = Defines.State.Wait;
    }

    void ThiefAttackStartAniEvt()
    {
        GameObject bullet = Managers.Resource.Instantiate("Weapon/Shurikens/ThiefBullet");
        Vector3 direction = new Vector3(_playerController.Joystick.InputDirection.x, 0, _playerController.Joystick.InputDirection.y);
        bullet.GetComponent<Bullet>().Fire(direction, _playerController.transform.position);
    }
    
    void ThiefAttackEndAniEvt()
    {        
        _playerController.PlayerState = Defines.State.Wait;
    }

    void AnimationAttackPlay(Animator anim)
    {
        _playerController.StopAttack = false;
        Defines.PlayerJob job = _playerController.Stat.Job;
        if (job == Defines.PlayerJob.Warrior)
        {            
            anim.Play("WarriorATTACK");
            Managers.Sound.Play("Player/WarriorAttack", Defines.Sound.Effect);
        }
        else if (job == Defines.PlayerJob.Thief)
        {
            anim.Play("ThiefATTACK");
            Managers.Sound.Play("Player/ThiefAttack", Defines.Sound.Effect);
        }        
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        if(_playerController.StopAttack)
            AnimationAttackPlay(GetComponent<Animator>());
    }
}
