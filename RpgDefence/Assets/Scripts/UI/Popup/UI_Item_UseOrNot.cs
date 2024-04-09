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
            Get<Text>((int)Texts.UseOrNotText).text = "장비를 해제하시겠습니까?";   
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
            Get<Text>((int)Texts.UseOrNotText).text = "장비를 착용하시겠습니까?";
        else if (itemClickCategory == Defines.ItemClickCategory.ConsumeUse)
            Get<Text>((int)Texts.UseOrNotText).text = "사용하시겠습니까?";
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
        PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();

        if (itemClickCategory == Defines.ItemClickCategory.EquipmentRelease)
        {                        
            // 장비중에서 어떤 장비인지 판단(무기, 방어구 ..등)
            EquipmentItem equipmentItem;
            if(playerStat.EquipmentState.TryGetValue(playerStat.CurrentEquipmentCategory, out equipmentItem))
            {
                if(equipmentItem != null)
                {
                    playerStat.InvenItemCount += 1; // 인벤토리에 아이템을 최대로 넣을 수 있는 개수 업데이트                    
                    if (playerStat.InvenItemCount > playerStat.MaxInvenItemCount)
                    {
                        Managers.UI.CloseParentPopupUI();
                        UI_TextPopup text = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
                        text.Text = "인벤토리 공간이 부족합니다!";
                        playerStat.InvenItemCount -= 1;
                        return;
                    }
                    playerStat.Item.Add(equipmentItem.ItemNumber, equipmentItem); // 아이템 인벤토리에 넣고 
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = null; // 장비 인벤토리에선 삭제                                        
                    playerStat.PlayerStatRelease(equipmentItem); // 해제된 장비 능력만큼 플레이어 스탯 변경                    
                }
            }            
        }            
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
        {            
            // 장비중에서 어떤 장비인지 판단(무기, 방어구 ..등)
            EquipmentItem equipmentItem;
            if (playerStat.EquipmentState.TryGetValue(playerStat.CurrentEquipmentCategory, out equipmentItem))
            {
                // 플레이어의 아이템 인벤토리 정보를 가져와서 그 중에서 클릭한 아이템을 가져옴                
                if (equipmentItem != null)
                {
                    // ** 만약에 플레이어가 장착한 장비가 있으면 변경하기(똑같은 장비카테고리에 한해서만 변경 ex) 롱소드(무기) <-> 숏소드(무기)
                    playerStat.Item.Add(equipmentItem.ItemNumber, equipmentItem); // 장비 인벤토리 아이템 -> 아이템 인벤토리
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = null; // 장비 인벤토리 아이템 삭제   
                    if (Managers.Game.UseChoiceItem is EquipmentItem equip)
                    {
                        playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = equip; // 아이템 인벤토리 아이템 -> 장비 인벤토리
                        playerStat.Item.Remove(Managers.Game.UseChoiceItem.ItemNumber); // 아이템 인벤토리에 삭제
                        playerStat.PlayerStatRelease(equipmentItem); // 해제된 장비 능력만큼 플레이어 스탯 변경
                        playerStat.PlayerStatUpgrade(equip); // 장착된 장비 능력만큼 플레이어 스탯 변경
                    }                        
                }
                else if (Managers.Game.UseChoiceItem as EquipmentItem != null)
                {
                    equipmentItem = Managers.Game.UseChoiceItem as EquipmentItem;
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = equipmentItem; // 장비 인벤토리에 넣고    
                    playerStat.Item.Remove(equipmentItem.ItemNumber); // 아이템 인벤토리에 삭제
                    playerStat.PlayerStatUpgrade(equipmentItem); // 장착된 장비 능력만큼 플레이어 스탯 변경
                    playerStat.InvenItemCount -= 1; // 인벤토리에 아이템을 최대로 넣을 수 있는 개수 업데이트
                } 
            }
        
        }
        else if (itemClickCategory == Defines.ItemClickCategory.ConsumeUse)
        {
            playerStat.UseConsumeItem(Managers.Game.UseChoiceItem);         // 플레이어 스탯에서 소비 아이템 사용효과 적용                       
            playerStat.Item.Remove(Managers.Game.UseChoiceItem.ItemNumber); // 아이템 인벤토리에서 해당 소비 아이템 제거
            playerStat.InvenItemCount -= 1; // 인벤토리에 아이템을 최대로 넣을 수 있는 개수 업데이트
        }

        Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_UseOrNot>(), GameObject.FindWithTag("UI_Item_UseOrNot").transform.parent.gameObject);
        Managers.UI.CloseAllParentPopupUI();

        UI_Inven InvenItem = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenItemGrid");
        InvenItem.InvenGridCategory = Defines.UiInvenGridCategory.ItemGrid;

        UI_Inven InvenEquipment = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenEquipmentGrid");
        InvenEquipment.InvenGridCategory = Defines.UiInvenGridCategory.EquipmentGird;        
    }

    public void BtnOnClickedNo(PointerEventData data)
    {        
        Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_UseOrNot>(), GameObject.FindWithTag("UI_Item_UseOrNot").transform.parent.gameObject);
    }
}
