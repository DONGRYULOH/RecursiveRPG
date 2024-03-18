using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : 타입만 바뀌고 나머지는 공통적으로 사용되는 부분이므로 제네릭을 사용해서 해당 오브젝트(몬스터, 플레이어..등)의 상태에 맞춰서 타입이 바꿔짐 
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
