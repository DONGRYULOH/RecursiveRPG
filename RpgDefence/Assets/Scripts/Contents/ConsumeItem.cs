using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : Item
{
    int itemNumber; // PK    
    int hpIncrement;
    int mpIncrement;

    public int ItemNumber { get { return itemNumber; } }    
    public int HpIncrement { get { return hpIncrement; } }
    public int MpIncrement { get { return mpIncrement; } }

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Consume;
    }

    public ConsumeItem() { }

    public ConsumeItem(int itemNumber, string itemName, int hpIncrement, int mpIncrement) {
        this.itemNumber = itemNumber;
        this.itemName = itemName;
        this.hpIncrement = hpIncrement;
        this.mpIncrement = mpIncrement;
    }
}

