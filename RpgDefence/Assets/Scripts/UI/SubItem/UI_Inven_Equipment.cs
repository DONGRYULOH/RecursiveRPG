using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Equipment : UI_Base
{
    // 장비 카테고리별 착용여부(무기 X, 방어구 O, .. 이런식으로 체크)
    Defines.EquipmentCategory equipmentCategory;
    bool equipmentIsUsed = false;

    // GameObject 단위로 관리
    enum GameObjects
    {
        EquipmentIcon,
        EquipmentName,
        ItemName
    }
        
    public string _name
    {
        get; set;
    }

    public Defines.EquipmentCategory EquipmentCategory
    {
        get { return equipmentCategory; }
        set { equipmentCategory = value; }
    }

    public bool EquipmentIsUsed
    {
        get { return equipmentIsUsed; }
        set { equipmentIsUsed = value; }
    }

    void Start()
    {
        Init();   
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GameObject go = Get<GameObject>((int)GameObjects.EquipmentName);
        go.GetComponent<Text>().text = _name;
        
        if (EquipmentIsUsed)
        {
            GameObject itemIcon = Get<GameObject>((int)GameObjects.EquipmentIcon);
            BindEvent(itemIcon, BtnOnClicked, Defines.UIEvent.Click);

            // 플레이어가 착용한 장비템 이미지
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();            
            string imageName = playerStat.EquipmentState[equipmentCategory].ItemName; // 확장자 없이 해야지 불러옴
            itemIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Item/" + imageName);

            // 플레이어가 착용한 장비템 이름
            GameObject itemName = Get<GameObject>((int)GameObjects.ItemName);
            itemName.GetComponent<Text>().text = imageName;
        }
    }

    public void SetInfo(string name)
    {
        _name = name;
    }

    public void BtnOnClicked(PointerEventData data)
    {                
        Managers.Game.ItemClickCategory = Defines.ItemClickCategory.EquipmentRelease;
        PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();
        playerStat.CurrentEquipmentCategory = equipmentCategory;

        if (GameObject.FindWithTag("UI_Item_UseOrNot") == null)
            Managers.UI.ShowPopupUI<UI_Item_UseOrNot>();        
    }

}
