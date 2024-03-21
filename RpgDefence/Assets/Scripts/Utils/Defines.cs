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
        NextChapter = 9,
        Store = 10
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Store
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

    public enum Chapter
    {
        ChapterOne,
        ChapterTwo,
        ChapterThree
    }

    public enum ItemCategory
    {
        Equipment,
        Consume,
        Etc
    }

    // 장비별(무기, 방어구, 액세서리 .. 등) 카테고리
    public enum EquipmentCategory
    {
        Weapon,
        Armor,
        Accessory,
        Shoes
    }

    // 플레이어가 장비 착용, 해제 or 소모성 아이템 사용하는 경우
    public enum ItemClickCategory
    {
        Unknown,
        EquipmentUse,
        EquipmentRelease,
        ConsumeUse
    }
}
