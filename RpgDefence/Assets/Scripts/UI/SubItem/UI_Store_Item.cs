using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // GameObject 단위로 관리
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
        icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/StoreItem/" + Name);     
    }

}
