using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    private const int maxInvenItemCount = 6; // 인벤토리 아이템 개수 최대치 고정
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
        // 몬스터가 죽었을때 경험치를 주는 것 뿐만 아니라 퀘스트를 완료하거나 어떠한 이벤트를 수행했을때도 경험치를 주기 때문에 경험치가 변경되는 것을 공통으로 만듬
        set
        {
            // 1.경험치가 깎였을때 현재 레벨의 경험치를 충족하는 값 아래로 내려가지 않도록 설정            
            exp = value;
            if (Managers.Data.PlayerStatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.PlayerStatDic[Level];
                if (exp < stat.totalExp) exp = stat.totalExp; // 현재 레벨을 충족하는 최소 EXP 값으로 세팅                
            }

            // 2.레벨업 체크
            // 해당 경험치가 어느 레벨에 도달했는지 체크 후 도달한 레벨로 변경
            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.PlayerStatDic.TryGetValue(level + 1, out stat) == false) // 그 다음 레벨이 없는 경우 (만렙)
                    break;
                if (exp < stat.totalExp) // 그 다음 레벨이 있으면 현재 경험치가 다음 레벨 경험치를 충족하는지 체크
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

    // 플레이어가 들고있는 아이템
    Dictionary<int, Item> item = new Dictionary<int, Item>();
    public Dictionary<int, Item> Item { get { return item; } set { item = value; }}

    // 플레이어가 착용하고 있는 장비상태
    Dictionary<Defines.EquipmentCategory, EquipmentItem> equipmentState = new Dictionary<Defines.EquipmentCategory, EquipmentItem>();
    public Dictionary<Defines.EquipmentCategory, EquipmentItem> EquipmentState { get { return equipmentState; } set { equipmentState = value; }}

    // 플레이어가 착용하려고 하거나 해제하려고 하는 장비종류(무기, 방어구 ..등)
    Defines.EquipmentCategory currentEquipmentCategory;
    public Defines.EquipmentCategory CurrentEquipmentCategory { get { return currentEquipmentCategory; } set { currentEquipmentCategory = value; }}
   
    private void Start()
    {
        job = MainScene.playerJob;
        MainScene.playerJob = Defines.PlayerJob.Unknown; // 멀티작업인 경우 unKnown처리를 해주지 않으면 그 다음 선택하는 유저는 무조건 직업이 선택되어있는 상태?(Static 이므로)                                
        BasicStat(); // 모든 플레이어 공통 스탯
        SetStat(_level); // 직업별 스탯
        EquipmentInvenInit(); // start 장비 세팅
        ItemInvenInit(); // start 아이템 세팅
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

    // 해당 플레이어가 들고있는 장비상태를 초기화
    public void EquipmentInvenInit()
    {
        // 플레이어 장비상태창 개수에 맞춰서 세팅        
        System.Array equipmentCategory = System.Enum.GetValues(typeof(Defines.EquipmentCategory));
        for (int i = 0; i < equipmentCategory.Length; i++)
        {   
            equipmentState.Add((Defines.EquipmentCategory)equipmentCategory.GetValue(i), null);
        }

        // 기본무기 세팅
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

