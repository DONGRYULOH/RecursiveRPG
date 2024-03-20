using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : Item
{
    string ItemName;
    int hpIncrement;
    int mpIncrement;

    protected override void SetCategory()
    {
        ItemCatecory = Defines.ItemCategory.Consume;
    }

    public ConsumeItem() { }

    public ConsumeItem(string ItemName, int hpIncrement, int mpIncrement) {
        this.ItemName = ItemName;
        this.hpIncrement = hpIncrement;
        this.mpIncrement = mpIncrement;
    }
}

