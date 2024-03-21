using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    해당 아이템을 착용 또는 사용하거나 해제하는 경우
*/

public class UI_Item_UseOrNot : UI_Popup
{
    enum Buttons
    {
        // 유니티 툴에 있는 Button명과 동일하게 매핑
        Yes,
        No
    }

    enum Texts
    {
        UseOrNotText
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        ModifyChoiceText();

        Bind<Button>(typeof(Buttons));
        BtnClickYesMapping();
        BtnClickNoMapping();
    }

    public void ModifyChoiceText()
    {
        Defines.ItemClickCategory itemClickCategory = Managers.Game.ItemClickCategory;

        if (itemClickCategory == Defines.ItemClickCategory.EquipmentRelease)
            Get<Text>((int)Texts.UseOrNotText).text = "해당 장비를 해제하시겠습니까?";   
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
            Get<Text>((int)Texts.UseOrNotText).text = "해당 장비를 착용하시겠습니까?";
    }

    public void BtnClickYesMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.Yes).gameObject;
        BindEvent(pb, BtnOnClickedYes, Defines.UIEvent.Click);
    }

    public void BtnClickNoMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.No).gameObject;
        BindEvent(pb, BtnOnClickedNo, Defines.UIEvent.Click);
    }

    public void BtnOnClickedYes(PointerEventData data)
    {        
        Defines.ItemClickCategory itemClickCategory = Managers.Game.ItemClickCategory;

        if (itemClickCategory == Defines.ItemClickCategory.EquipmentRelease)
        {            
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();

            // 장비중에서 어떤 장비인지 판단(무기, 방어구 ..등)
            EquipmentItem equipmentItem;
            if(playerStat.EquipmentState.TryGetValue(playerStat.CurrentEquipmentCategory, out equipmentItem))
            {
                if(equipmentItem != null)
                {
                    playerStat.Item.Add(equipmentItem.ItemNumber, equipmentItem); // 아이템 인벤토리에 넣고 
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = null; // 장비 인벤토리에선 삭제
                }
            }
            Managers.UI.CloseAllPopupUI();
            Managers.UI.ShowPopupUI<UI_Inven>("UI_Inven");
        }            
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
        {

        }
    }

    public void BtnOnClickedNo(PointerEventData data)
    {        
        Managers.UI.ClosePopupUI(gameObject.GetComponent<UI_Item_UseOrNot>());
    }
}
