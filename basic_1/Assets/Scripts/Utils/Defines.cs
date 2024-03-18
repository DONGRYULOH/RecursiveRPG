using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type별로 관리하는 클래스

public class Defines
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    // 플레이어와 몬스터가 공통으로 사용할 상태 
    public enum State
    {
        Die,
        Moving,
        Wait,
        Skill
    }

    public enum Layer
    {
        Monster1 = 6,
        Ground = 7,
        Block = 8,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuaterView,
    }
}
