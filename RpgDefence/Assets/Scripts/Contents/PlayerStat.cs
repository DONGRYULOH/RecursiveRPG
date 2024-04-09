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

    private Defines.PlayerJob job;
    public Defines.PlayerJob Job { get { return job; } set { job = value; } }

    private float attackRange;
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }

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
            if (Managers.Data.PlayerStatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.PlayerStatDic[Level];
                if (exp < stat.totalExp) exp = stat.totalExp; // ���� ������ �����ϴ� �ּ� EXP ������ ����                
            }

            // 2.������ üũ
            // �ش� ����ġ�� ��� ������ �����ߴ��� üũ �� ������ ������ ����
            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.PlayerStatDic.TryGetValue(level + 1, out stat) == false) // �� ���� ������ ���� ��� (����)
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

    private int maxMp;
    public int MaxMp { get { return maxMp; } set { maxMp = value; } }

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
        job = MainScene.playerJob;
        MainScene.playerJob = Defines.PlayerJob.Unknown; // ��Ƽ�۾��� ��� unKnownó���� ������ ������ �� ���� �����ϴ� ������ ������ ������ ���õǾ��ִ� ����?(Static �̹Ƿ�)                                
        BasicStat(); // ��� �÷��̾� ���� ����
        SetStat(_level); // ������ ����
        EquipmentInvenInit(); // start ��� ����
        ItemInvenInit(); // start ������ ����
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

    void BasicStat()
    {
        Level = 1;
        Defense = 5;
        MoveSpeed = 10.0f;
        Exp = 0;
        Gold = 10000;        
        AttackRange = 2f;
    }
    
    public void SetStat(int level)
    {                
        if (job == Defines.PlayerJob.Warrior)
        {
            Dictionary<int, Data.PlayerWarriorStat> warriorStat = Managers.Data.PlayerWarriorStatDic;
            Hp = warriorStat[_level].maxHp;
            MaxHp = warriorStat[_level].maxHp;
            MaxMp = warriorStat[_level].maxMp;
            Mp = warriorStat[_level].maxMp;
            Attack = warriorStat[_level].attack;
            Defense = warriorStat[_level].defense;
            MoveSpeed = warriorStat[_level].moveSpeed;
        }
        else if (job == Defines.PlayerJob.Thief)
        {
            Dictionary<int, Data.PlayerThiefStat> thiefStat = Managers.Data.PlayerThiefStatDic;
            Hp = thiefStat[_level].maxHp;
            MaxHp = thiefStat[_level].maxHp;
            MaxMp = thiefStat[_level].maxMp;
            Mp = thiefStat[_level].maxMp;
            Attack = thiefStat[_level].attack;
            Defense = thiefStat[_level].defense;
            MoveSpeed = thiefStat[_level].moveSpeed;
        }        
    }

    protected override void OnDead(Stat attacker)
    {
        if(gameObject != null)
        {
            gameObject.GetComponent<PlayerStat>().Exp -= 10;
        }
    }

    // �ش� �÷��̾ ����ִ� �����¸� �ʱ�ȭ
    public void EquipmentInvenInit()
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
            EquipmentItem equipmentItem = new EquipmentItem(102, "BasicSword", 50, 0, Defines.EquipmentCategory.Weapon, 100);
            equipmentState[Defines.EquipmentCategory.Weapon] = equipmentItem;
        }
    }

    public void ItemInvenInit()
    {
        ConsumeItem consumeItem = new ConsumeItem(101, "BasicHpPortion", 50, 0, 20);
        Item.Add(consumeItem.ItemNumber, consumeItem);
        invenItemCount++;
    }
}

