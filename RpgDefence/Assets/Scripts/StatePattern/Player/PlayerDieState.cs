using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : MonoBehaviour, PlayerState
{
    public void Handle(PlayerController controller)
    {
        Managers.Game.DeSpawn(controller.gameObject);
        Managers.Resource.Instantiate("UI/UI_GameOver");               
    }
}
