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

    // ���� �÷��̾��� ����
    public void Transition()
    {
        CurrentState.Handle(this.controller);
    }

    // ���� �÷��̾� ���¸� ������Ʈ �� �� ���·� ���� 
    public void Transition(PlayerState state)
    {
        CurrentState = state;
        Transition();
    }
}
