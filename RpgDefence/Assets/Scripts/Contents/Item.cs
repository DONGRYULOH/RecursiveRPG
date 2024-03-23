using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �Һ�, ���, ��Ÿ ...�� ��� �������� �����ϴ� ���� ����
 */
public abstract class Item
{
    protected int itemNumber;
    protected Defines.ItemCategory ItemCatecory;    
    protected string itemName;
    protected int price;

    public int ItemNumber { get { return itemNumber; } }
    public Defines.ItemCategory GetCatecory { get { return ItemCatecory; } }    
    public string ItemName { get { return itemName; } }
    public int Price { get { return price; } }

    protected abstract void SetCategory();    
}
