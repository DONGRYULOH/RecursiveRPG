using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    // GameObject 단위로 관리
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }
        
    public string _name
    {
        get; set;
    }
    
    void Start()
    {
        Init();   
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GameObject go = Get<GameObject>((int)GameObjects.ItemNameText);
        go.GetComponent<Text>().text = _name;

        GameObject itemIcon = Get<GameObject>((int)GameObjects.ItemIcon);
        BindEvent(itemIcon, BtnOnClicked, Defines.UIEvent.Click);
    }

    public void SetInfo(string name)
    {
        _name = name;
    }

    public void BtnOnClicked(PointerEventData data)
    {
        Debug.Log("아이템 클릭");
    }

}
