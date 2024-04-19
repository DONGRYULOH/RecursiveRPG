using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
           
    void OnHitSkillEvent()
    {
        // ������ �浹������ ĳ���Ͱ� �ֵθ� ������ �浹������ �´�� �Ǹ� ������ �Ǹ� �����
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
