using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    소비, 장비, 기타 ...등 모든 아이템을 포함하는 상위 개념
 */
public abstract class Item
{
    protected Defines.ItemCategory ItemCatecory;

    protected abstract void SetCategory();

    public Defines.ItemCategory GetCatecory { get { return ItemCatecory; } }
}
