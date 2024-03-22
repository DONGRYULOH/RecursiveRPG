using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    소비, 장비, 기타 ...등 모든 아이템을 포함하는 상위 개념
 */
public abstract class Item
{
    protected int itemNumber;
    protected Defines.ItemCategory ItemCatecory;    
    protected string itemName;

    public int ItemNumber { get { return itemNumber; } }
    public Defines.ItemCategory GetCatecory { get { return ItemCatecory; } }    
    public string ItemName { get { return itemName; } }

    protected abstract void SetCategory();    
}
