using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Store_Item : UI_Base
{
    Item itmeInfo;

    public Item ItmeInfo
    {
        get { return itmeInfo; }
        set { itmeInfo = value; }
    }

    enum GameObjects
    {
        StoreItemIcon,
        StoreItemName,
        StoreItemPrice
    }

    public string Name
    {
        get; set;
    }

    public int Price
    {
        get; set;
    }

    // GameObject ������ ����
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GameObject name = Get<GameObject>((int)GameObjects.StoreItemName);
        name.GetComponent<Text>().text = Name;

        GameObject price = Get<GameObject>((int)GameObjects.StoreItemPrice);
        price.GetComponent<Text>().text = Price + "$";
        
        GameObject icon = Get<GameObject>((int)GameObjects.StoreItemIcon);
        icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Item/" + Name);
        BindEvent(icon, BtnOnClickedItem, Defines.UIEvent.Click);
    }

    public void BtnOnClickedItem(PointerEventData data)
    {
        if (GameObject.FindWithTag("UI_Item_SellOrBuy") == null)
        {
            Managers.Game.ItemClickCategory = Defines.ItemClickCategory.BuyStore; // ���ſ���        
            Managers.Game.UseChoiceItem = itmeInfo; // ������ ������ ����        
            Managers.UI.ShowPopupUI<UI_Item_SellOrBuy>();
        }
    }
}
