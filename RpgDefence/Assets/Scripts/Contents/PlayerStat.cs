using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{    
    private int _exp;    
    private int _gold;
    private int mp;

    // �÷��̾ ����ִ� ������
    Dictionary<int, Item> item = new Dictionary<int, Item>();

    // �÷��̾ �����ϰ� �ִ� ������
    Dictionary<Defines.EquipmentCategory, EquipmentItem> equipmentState = new Dictionary<Defines.EquipmentCategory, EquipmentItem>();

    // �÷��̾ �����Ϸ��� �ϰų� �����Ϸ��� �ϴ� �������(����, �� ..��)
    Defines.EquipmentCategory currentEquipmentCategory;

    public Defines.EquipmentCategory CurrentEquipmentCategory
    {
        get { return currentEquipmentCategory; }
        set { currentEquipmentCategory = value; }
    }

    public Dictionary<int, Item> Item
    {
        get { return item; }
        set { item = value; }
    }

    public Dictionary<Defines.EquipmentCategory, EquipmentItem> EquipmentState
    {
        get { return equipmentState; }
        set { equipmentState = value; }
    }

    public int Exp { 
        get { return _exp; }
        // ���Ͱ� �׾����� ����ġ�� �ִ� �� �Ӹ� �ƴ϶� ����Ʈ�� �Ϸ��ϰų� ��� �̺�Ʈ�� ������������ ����ġ�� �ֱ� ������ ����ġ�� ����Ǵ� ���� �������� ����
        set
        {
            // 1.����ġ�� ������ ���� ������ ����ġ�� �����ϴ� �� �Ʒ��� �������� �ʵ��� ����            
            _exp = value;
            if (Managers.Data.StatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.StatDic[Level];
                if (_exp < stat.totalExp) _exp = stat.totalExp; // ���� ������ �����ϴ� �ּ� EXP ������ ����                
            }

            // 2.������ üũ
            // �ش� ����ġ�� ��� ������ �����ߴ��� üũ �� ������ ������ ����
            int level = Level; 
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDic.TryGetValue(level + 1, out stat) == false) // �� ���� ������ ���� ��� (����)
                    break;
                if (_exp < stat.totalExp) // �� ���� ������ ������ ���� ����ġ�� ���� ���� ����ġ�� �����ϴ��� üũ
                    break;
                level = stat.level;
            }

            if(Level != level)
            {
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 2;
        _defense = 5;
        _moveSpeed = 10.0f;
        _exp = 10;
        _gold = 0;
        SetStat(_level);

        EquipmentInit();
    }

    public void PlayerStatRelease(Item item)
    {
        if (item is EquipmentItem equipmentItem)
        {
            Attack -= equipmentItem.Power;
            Defense -= equipmentItem.Defence;
        }
    }

    public void PlayerStatUpgrade(Item item)
    {
        if (item is EquipmentItem equipmentItem)
        {
            Attack += equipmentItem.Power;
            Defense += equipmentItem.Defence;
        }
    }

    public void UseConsumeItem(Item item)
    {
        if(item is ConsumeItem ConsumeItem)
        {
            _hp += ConsumeItem.HpIncrement;
            mp += ConsumeItem.MpIncrement;
        }
    }

    // ������ �Ҷ����� �ش� �÷��̾��� ������ ����
    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> stat = Managers.Data.StatDic;
        _hp = stat[_level].maxHp;
        _maxHp = stat[_level].maxHp;
        _attack = stat[_level].attack;
    }

    protected override void OnDead(Stat attacker)
    {
        if(gameObject != null)
        {
            gameObject.GetComponent<PlayerStat>().Exp -= 10;
        }

        Debug.Log("player ����ġ ����");
    }

    // �ش� �÷��̾ ����ִ� �����¸� �ʱ�ȭ
    public void EquipmentInit()
    {
        // �÷��̾� ������â ������ ���缭 ����        
        System.Array equipmentCategory = System.Enum.GetValues(typeof(Defines.EquipmentCategory));
        for (int i = 0; i < equipmentCategory.Length; i++)
        {   
            equipmentState.Add((Defines.EquipmentCategory)equipmentCategory.GetValue(i), null);
        }

        // �ӽ� : ������� �־ ȭ�鿡�� ������� �� �ִ��� Ȯ���ϱ�
        if (equipmentState.ContainsKey(Defines.EquipmentCategory.Armor))
        {
            EquipmentItem equipmentItem = new EquipmentItem(2, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon);
            equipmentState[Defines.EquipmentCategory.Weapon] = equipmentItem;
        }
    }
}
