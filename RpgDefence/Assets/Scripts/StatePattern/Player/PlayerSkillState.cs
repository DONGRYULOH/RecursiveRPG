using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
           
    void OnHitSkillEvent()
    {
        // 몬스터의 충돌범위와 캐릭터가 휘두른 무기의 충돌범위가 맞닿게 되면 몬스터의 피를 깎아줌
    }

    void ChangeWaitState()
    {
        _playerController.PlayerState = Defines.State.Wait;
    }

    public void SkillAnimationState()
    {
        _playerController.PlayerState = Defines.State.Skill;
        Animator animation = _playerController.gameObject.GetComponent<Animator>();        
        animation.Play("Skill_Q");
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        SkillAnimationState();
    }
}
