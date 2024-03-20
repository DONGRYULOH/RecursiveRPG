using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : Item
{
    string itemName;
    int hpIncrement;
    int mpIncrement;

    public string ItemName { get { return itemName; } }
    public int HpIncrement { get { return hpIncrement; } }
    public int MpIncrement { get { return mpIncrement; } }

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Consume;
    }

    public ConsumeItem() { }

    public ConsumeItem(string itemName, int hpIncrement, int mpIncrement) {
        this.itemName = itemName;
        this.hpIncrement = hpIncrement;
        this.mpIncrement = mpIncrement;
    }
}

