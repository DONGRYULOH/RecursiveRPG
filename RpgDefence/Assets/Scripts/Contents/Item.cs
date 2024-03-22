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

    public int ItemNumber { get { return itemNumber; } }
    public Defines.ItemCategory GetCatecory { get { return ItemCatecory; } }    
    public string ItemName { get { return itemName; } }

    protected abstract void SetCategory();    
}
