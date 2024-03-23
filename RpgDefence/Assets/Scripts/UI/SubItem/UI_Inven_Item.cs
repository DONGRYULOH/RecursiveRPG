using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    Defines.ItemCategory itemCategory;

    Defines.EquipmentCategory equipmentCategory;

    Item itmeInfo;

    // GameObject 단위로 관리
    enum GameObjects
    {
        ItemIcon,
        ItemName
    }

    public Item ItmeInfo
    {
        get { return itmeInfo; }
        set { itmeInfo = value; }
    }
    public Defines.ItemCategory ItemCategory
    {
        get { return itemCategory; }
        set { itemCategory = value; }
    }

    public Defines.EquipmentCategory EquipmentCategory
    {
        get { return equipmentCategory; }
        set { equipmentCategory = value; }
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

        GameObject go = Get<GameObject>((int)GameObjects.ItemName);
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
        if(itemCategory == Defines.ItemCategory.Equipment)
        {
            // 장비아이템인 경우 플레이어 장비 인벤(해당 장비아이템이 어떤 종류인지 판단)에 붙이기            
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();
            playerStat.CurrentEquipmentCategory = equipmentCategory;
            Managers.Game.ItemClickCategory = Defines.ItemClickCategory.EquipmentUse;            
        }
        else if (itemCategory == Defines.ItemCategory.Consume)
        {            
            Managers.Game.ItemClickCategory = Defines.ItemClickCategory.ConsumeUse;           
        }
        Managers.Game.UseChoiceItem = itmeInfo;
        
        if(GameObject.FindWithTag("UI_Item_UseOrNot") == null)
            Managers.UI.ShowPopupUI<UI_Item_UseOrNot>();        
    }

}
