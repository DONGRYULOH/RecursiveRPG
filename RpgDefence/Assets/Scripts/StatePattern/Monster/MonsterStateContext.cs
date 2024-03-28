using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateContext
{
    public MonsterState CurrentState
    {
        get; set;
    }

    private readonly MonsterController controller;

    public MonsterStateContext(MonsterController controller)
    {
        this.controller = controller;
    }

    // ���� �÷��̾��� ����
    public void Transition()
    {
        CurrentState.Handle(this.controller);
    }

    // ���� �÷��̾� ���¸� ������Ʈ �� �� ���·� ���� 
    public void Transition(MonsterState state)
    {
        CurrentState = state;
        Transition();
    }
}
