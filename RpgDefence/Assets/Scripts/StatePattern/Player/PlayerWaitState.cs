using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;

    public void WaitAnimationState(Animator anim)
    {
        _playerController.PlayerState = Defines.State.Wait;
        anim.Play("WAIT");
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        WaitAnimationState(GetComponent<Animator>());
    }
}
