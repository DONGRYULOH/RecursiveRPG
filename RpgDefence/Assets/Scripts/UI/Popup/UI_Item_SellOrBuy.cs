using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    상점에 해당 아이템을 팔거나 상점에서 해당 아이템을 사는 경우
*/
public class UI_Item_SellOrBuy : UI_Popup
{
    enum Buttons
    {
        // 유니티 툴에 있는 Button명과 동일하게 매핑
        Yes,
        No
    }

    enum Texts
    {
        SellOrBuyText
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

        if (itemClickCategory == Defines.ItemClickCategory.SellStore)
            Get<Text>((int)Texts.SellOrBuyText).text = "판매하시겠습니까?";
        else if (itemClickCategory == Defines.ItemClickCategory.BuyStore)
            Get<Text>((int)Texts.SellOrBuyText).text = "구매하시겠습니까?";        
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
        // 1.판매 or 구매 여부
        PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();

        Defines.ItemClickCategory sellOrBuyCheck = Managers.Game.ItemClickCategory;
        if(sellOrBuyCheck == Defines.ItemClickCategory.SellStore)
        {
            int price = Managers.Game.UseChoiceItem.Price;
            playerStat.Gold += price;
            playerStat.InvenItemCount -= 1;
            playerStat.Item.Remove(Managers.Game.UseChoiceItem.ItemNumber);
        } 
        else if (sellOrBuyCheck == Defines.ItemClickCategory.BuyStore)
        {
            if(playerStat.Gold < Managers.Game.UseChoiceItem.Price)
            {
                Managers.UI.CloseParentPopupUI();
                UI_TextPopup text = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
                text.Text = "골드가 부족합니다!";
                return;
            }

            playerStat.InvenItemCount += 1;
            if (playerStat.InvenItemCount > playerStat.MaxInvenItemCount)
            {
                Managers.UI.CloseParentPopupUI();
                UI_TextPopup text = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");                
                text.Text = "인벤토리 공간이 부족합니다!";                
                playerStat.InvenItemCount -= 1;                
                return;
            }
            
            if(Managers.Game.UseChoiceItem.GetCatecory == Defines.ItemCategory.Consume)
            {
                if (Managers.Game.UseChoiceItem is ConsumeItem item)
                {
                    int itemNumber = Managers.Game.CurrentItemNumberIndex;
                    string name = item.ItemName;
                    int hpIncrement = item.HpIncrement;
                    int mpIncrement = item.MpIncrement;
                    int price = item.Price;
                    ConsumeItem consumeItem = new ConsumeItem(itemNumber, name, hpIncrement, mpIncrement, price);
                    playerStat.Item.Add(itemNumber, consumeItem);
                }                                
            }
            else if (Managers.Game.UseChoiceItem.GetCatecory == Defines.ItemCategory.Equipment)
            {
                if (Managers.Game.UseChoiceItem is EquipmentItem item)
                {
                    int itemNumber = Managers.Game.CurrentItemNumberIndex;
                    string name = item.ItemName;
                    int power = item.Power;
                    Defines.EquipmentCategory equipmentCategory = item.EquipmentCategory;
                    int defence = item.Defence;
                    int price = item.Price;
                    EquipmentItem consumeItem = new EquipmentItem(itemNumber, name, power, defence, equipmentCategory, price);
                    playerStat.Item.Add(itemNumber, consumeItem);
                }
            }
            playerStat.Gold -= Managers.Game.UseChoiceItem.Price;
        }

        // 상점, 플레이어 인벤 UI 팝업 새로고침
        Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_SellOrBuy>(), GameObject.FindWithTag("UI_Item_SellOrBuy").transform.parent.gameObject);
        Managers.UI.CloseAllParentPopupUI();        
        Managers.UI.ShowPopupUI<UI_Store>("UI_Store");        
        UI_Inven InvenItem = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenItemGrid");
        foreach (Transform child in InvenItem.gameObject.transform)
        {
            if (child.gameObject.name == "UI_Inven_Close")
                Managers.Resource.Destroy(child.gameObject);
        }
        InvenItem.InvenGridCategory = Defines.UiInvenGridCategory.ItemGrid;
        InvenItem.StoreSellCheck = true;
    }

    public void BtnOnClickedNo(PointerEventData data)
    {
       Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_SellOrBuy>(), GameObject.FindWithTag("UI_Item_SellOrBuy").transform.parent.gameObject);
    }
}
