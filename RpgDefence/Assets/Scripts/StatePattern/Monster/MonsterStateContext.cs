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

    // 현재 플레이어의 상태
    public void Transition()
    {
        CurrentState.Handle(this.controller);
    }

    // 현재 플레이어 상태를 업데이트 후 그 상태로 변경 
    public void Transition(MonsterState state)
    {
        CurrentState = state;
        Transition();
    }
}
