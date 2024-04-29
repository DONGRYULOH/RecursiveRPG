using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{    
    int power;
    int defence;
    int hp;
    float speed;

    Defines.EquipmentCategory equipmentCategory;
    public Defines.EquipmentCategory EquipmentCategory { get { return equipmentCategory; } }

    public int Power { get { return power; } }
    public int Defence { get { return defence; } }
    public int Hp { get { return hp; } }
    public float Speed { get { return speed; } }

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Equipment;
    }

    public EquipmentItem(int itemNumber, string itemName, int power, int defence, int hp, float speed, Defines.EquipmentCategory equipmentCategory, int price)
    {
        this.itemNumber = itemNumber;
        this.itemName = itemName;
        this.power = power;
        this.defence = defence;
        this.hp = hp;
        this.speed = speed;
        this.equipmentCategory = equipmentCategory;
        this.price = price;
        SetCategory();
    }
}
