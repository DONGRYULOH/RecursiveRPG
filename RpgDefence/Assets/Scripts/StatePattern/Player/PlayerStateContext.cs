using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContext
{
    public PlayerState CurrentState
    {
        get; set;
    }

    private readonly PlayerController controller;

    public PlayerStateContext(PlayerController controller)
    {
        this.controller = controller;
    }

    // 현재 플레이어의 상태
    public void Transition()
    {
        CurrentState.Handle(this.controller);
    }

    // 현재 플레이어 상태를 업데이트 후 그 상태로 변경 
    public void Transition(PlayerState state)
    {
        CurrentState = state;
        Transition();
    }
}
