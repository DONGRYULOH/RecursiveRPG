using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private const int maxInvenItemCount = 6; // �κ��丮 ������ ���� �ִ�ġ ����
    public int MaxInvenItemCount { get { return maxInvenItemCount; } }

    private int invenItemCount;
    public int InvenItemCount { get { return invenItemCount; } set { invenItemCount = value; }}

    [SerializeField]
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }

    [SerializeField]
    private int exp;
    public int Exp
    {
        get { return exp; }
        // ���Ͱ� �׾����� ����ġ�� �ִ� �� �Ӹ� �ƴ϶� ����Ʈ�� �Ϸ��ϰų� ��� �̺�Ʈ�� ������������ ����ġ�� �ֱ� ������ ����ġ�� ����Ǵ� ���� �������� ����
        set
        {
            // 1.����ġ�� ������ ���� ������ ����ġ�� �����ϴ� �� �Ʒ��� �������� �ʵ��� ����            
            exp = value;
            if (Managers.Data.StatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.StatDic[Level];
                if (exp < stat.totalExp) exp = stat.totalExp; // ���� ������ �����ϴ� �ּ� EXP ������ ����                
            }

            // 2.������ üũ
            // �ش� ����ġ�� ��� ������ �����ߴ��� üũ �� ������ ������ ����
            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDic.TryGetValue(level + 1, out stat) == false) // �� ���� ������ ���� ��� (����)
                    break;
                if (exp < stat.totalExp) // �� ���� ������ ������ ���� ����ġ�� ���� ���� ����ġ�� �����ϴ��� üũ
                    break;
                level = stat.level;
            }

            if (Level != level)
            {
                Level = level;
                SetStat(Level);
            }
        }
    }
    [SerializeField]
    private int score;
    public int Score { get { return score; } set { score = value; } }

    private int mp;
    public int Mp { get { return mp; } set { mp = value; } }

    // �÷��̾ ����ִ� ������
    Dictionary<int, Item> item = new Dictionary<int, Item>();
    public Dictionary<int, Item> Item { get { return item; } set { item = value; }}

    // �÷��̾ �����ϰ� �ִ� ������
    Dictionary<Defines.EquipmentCategory, EquipmentItem> equipmentState = new Dictionary<Defines.EquipmentCategory, EquipmentItem>();
    public Dictionary<Defines.EquipmentCategory, EquipmentItem> EquipmentState { get { return equipmentState; } set { equipmentState = value; }}

    // �÷��̾ �����Ϸ��� �ϰų� �����Ϸ��� �ϴ� �������(����, �� ..��)
    Defines.EquipmentCategory currentEquipmentCategory;
    public Defines.EquipmentCategory CurrentEquipmentCategory { get { return currentEquipmentCategory; } set { currentEquipmentCategory = value; }}
   
    private void Start()
    {
        _level = 1;
        _defense = 5;
        _moveSpeed = 10.0f;
        exp = 0;
        gold = 10000;
        invenItemCount = 2;
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

        // �⺻���� ����
        if (equipmentState.ContainsKey(Defines.EquipmentCategory.Armor))
        {
            EquipmentItem equipmentItem = new EquipmentItem(103, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon, 100);
            equipmentState[Defines.EquipmentCategory.Weapon] = equipmentItem;
        }
    }
}

