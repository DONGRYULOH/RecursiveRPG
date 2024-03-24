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

    // GameObject ������ ����
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

        // �׳� ������ �κ��丮 or �������� �Ǹ��ϴ� ������ �κ��丮
        GameObject itemIcon = Get<GameObject>((int)GameObjects.ItemIcon);
        UI_Inven inven = itemIcon.transform.parent.parent.parent.gameObject.GetComponent<UI_Inven>();
        if (!inven.StoreSellCheck)
            BindEvent(itemIcon, BtnOnClickedItem, Defines.UIEvent.Click);
        else
            BindEvent(itemIcon, BtnOnClickedStore, Defines.UIEvent.Click);
    }

    public void SetInfo(string name)
    {
        _name = name;
    }

    public void BtnOnClickedItem(PointerEventData data)
    {        
        if(itemCategory == Defines.ItemCategory.Equipment)
        {
            // ���������� ��� �÷��̾� ��� �κ�(�ش� ���������� � �������� �Ǵ�)�� ���̱�            
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

    public void BtnOnClickedStore(PointerEventData data)
    {
        if (GameObject.FindWithTag("UI_Item_SellOrBuy") == null)
        {
            Managers.Game.ItemClickCategory = Defines.ItemClickCategory.SellStore; // �Ǹſ���        
            Managers.Game.UseChoiceItem = itmeInfo; // �Ǹ��� ������ ����        
            Managers.UI.ShowPopupUI<UI_Item_SellOrBuy>();            
        }                    
    }

}
