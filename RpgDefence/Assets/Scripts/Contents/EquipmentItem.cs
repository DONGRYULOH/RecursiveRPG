using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{
    string itemName;
    int power;
    int defence;

    Defines.EquipmentCategory equipmentCategory;

    public string ItemName { get { return itemName; } }
    public int Power { get { return power; } }
    public int Defence { get { return defence; } }

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Equipment;
    }    

    public EquipmentItem() { }

    public EquipmentItem(string itemName, int power, int defence, Defines.EquipmentCategory equipmentCategory)
    {
        this.itemName = itemName;
        this.power = power;
        this.defence = defence;
        this.equipmentCategory = equipmentCategory;
    }
}
