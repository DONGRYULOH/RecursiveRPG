using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{    
    int power;
    int defence;

    Defines.EquipmentCategory equipmentCategory;
    
    public int Power { get { return power; } }
    public int Defence { get { return defence; } }
    public Defines.EquipmentCategory EquipmentCategory { get { return equipmentCategory; } }

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Equipment;
    }

    public EquipmentItem(int itemNumber, string itemName, int power, int defence, Defines.EquipmentCategory equipmentCategory)
    {
        this.itemNumber = itemNumber;
        this.itemName = itemName;
        this.power = power;
        this.defence = defence;
        this.equipmentCategory = equipmentCategory;
        SetCategory();
    }
}
