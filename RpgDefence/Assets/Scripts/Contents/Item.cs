using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �Һ�, ���, ��Ÿ ...�� ��� �������� �����ϴ� ���� ����
 */
public abstract class Item
{
    protected Defines.ItemCategory ItemCatecory;
    protected string itemName;

    protected abstract void SetCategory();

    public Defines.ItemCategory GetCatecory { get { return ItemCatecory; } }
    public string ItemName { get { return itemName; } }
}
