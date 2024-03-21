using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type���� �����ϴ� Ŭ����

public class Defines
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    // �÷��̾�� ���Ͱ� �������� ����� ���� 
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

    // ���(����, ��, �׼����� .. ��) ī�װ�
    public enum EquipmentCategory
    {
        Weapon,
        Armor,
        Accessory,
        Shoes
    }

    // �÷��̾ ��� ����, ���� or �Ҹ� ������ ����ϴ� ���
    public enum ItemClickCategory
    {
        Unknown,
        EquipmentUse,
        EquipmentRelease,
        ConsumeUse
    }
}
