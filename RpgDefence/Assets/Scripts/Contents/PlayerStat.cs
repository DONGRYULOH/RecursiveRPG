using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{    
    private int _exp;    
    private int _gold;
    private int mp;

    // 플레이어가 들고있는 아이템
    Dictionary<int, Item> item = new Dictionary<int, Item>();

    // 플레이어가 착용하고 있는 장비상태
    Dictionary<Defines.EquipmentCategory, EquipmentItem> equipmentState = new Dictionary<Defines.EquipmentCategory, EquipmentItem>();

    // 플레이어가 착용하려고 하거나 해제하려고 하는 장비종류(무기, 방어구 ..등)
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
        // 몬스터가 죽었을때 경험치를 주는 것 뿐만 아니라 퀘스트를 완료하거나 어떠한 이벤트를 수행했을때도 경험치를 주기 때문에 경험치가 변경되는 것을 공통으로 만듬
        set
        {
            // 1.경험치가 깎였을때 현재 레벨의 경험치를 충족하는 값 아래로 내려가지 않도록 설정            
            _exp = value;
            if (Managers.Data.StatDic.ContainsKey(Level))
            {
                Data.Stat stat = Managers.Data.StatDic[Level];
                if (_exp < stat.totalExp) _exp = stat.totalExp; // 현재 레벨을 충족하는 최소 EXP 값으로 세팅                
            }

            // 2.레벨업 체크
            // 해당 경험치가 어느 레벨에 도달했는지 체크 후 도달한 레벨로 변경
            int level = Level; 
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDic.TryGetValue(level + 1, out stat) == false) // 그 다음 레벨이 없는 경우 (만렙)
                    break;
                if (_exp < stat.totalExp) // 그 다음 레벨이 있으면 현재 경험치가 다음 레벨 경험치를 충족하는지 체크
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

    // 레벨업 할때마다 해당 플레이어의 스텟을 변경
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

        Debug.Log("player 경험치 감소");
    }

    // 해당 플레이어가 들고있는 장비상태를 초기화
    public void EquipmentInit()
    {
        // 플레이어 장비상태창 개수에 맞춰서 세팅        
        System.Array equipmentCategory = System.Enum.GetValues(typeof(Defines.EquipmentCategory));
        for (int i = 0; i < equipmentCategory.Length; i++)
        {   
            equipmentState.Add((Defines.EquipmentCategory)equipmentCategory.GetValue(i), null);
        }

        // 임시 : 무기장비를 넣어서 화면에서 무기장비가 들어가 있는지 확인하기
        if (equipmentState.ContainsKey(Defines.EquipmentCategory.Armor))
        {
            EquipmentItem equipmentItem = new EquipmentItem(2, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon);
            equipmentState[Defines.EquipmentCategory.Weapon] = equipmentItem;
        }
    }
}
