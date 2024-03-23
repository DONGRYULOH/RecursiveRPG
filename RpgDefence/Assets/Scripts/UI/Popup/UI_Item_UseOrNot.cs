using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    �ش� �������� ���� �Ǵ� ����ϰų� �����ϴ� ���
*/

public class UI_Item_UseOrNot : UI_Popup
{
    enum Buttons
    {
        // ����Ƽ ���� �ִ� Button��� �����ϰ� ����
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
            Get<Text>((int)Texts.UseOrNotText).text = "��� �����Ͻðڽ��ϱ�?";   
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
            Get<Text>((int)Texts.UseOrNotText).text = "��� �����Ͻðڽ��ϱ�?";
        else if (itemClickCategory == Defines.ItemClickCategory.ConsumeUse)
            Get<Text>((int)Texts.UseOrNotText).text = "����Ͻðڽ��ϱ�?";
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

            // ����߿��� � ������� �Ǵ�(����, �� ..��)
            EquipmentItem equipmentItem;
            if(playerStat.EquipmentState.TryGetValue(playerStat.CurrentEquipmentCategory, out equipmentItem))
            {
                if(equipmentItem != null)
                {                    
                    playerStat.Item.Add(equipmentItem.ItemNumber, equipmentItem); // ������ �κ��丮�� �ְ� 
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = null; // ��� �κ��丮���� ����                                        
                    playerStat.PlayerStatRelease(equipmentItem); // ������ ��� �ɷ¸�ŭ �÷��̾� ���� ����
                }
            }            
        }            
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
        {
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();

            // ����߿��� � ������� �Ǵ�(����, �� ..��)
            EquipmentItem equipmentItem;
            if (playerStat.EquipmentState.TryGetValue(playerStat.CurrentEquipmentCategory, out equipmentItem))
            {
                // �÷��̾��� ������ �κ��丮 ������ �����ͼ� �� �߿��� Ŭ���� �������� ������                
                if (equipmentItem != null)
                {
                    // ** ���࿡ �÷��̾ ������ ��� ������ �����ϱ�(�Ȱ��� ���ī�װ��� ���ؼ��� ���� ex) �ռҵ�(����) <-> ���ҵ�(����)
                    playerStat.Item.Add(equipmentItem.ItemNumber, equipmentItem); // ��� �κ��丮 ������ -> ������ �κ��丮
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = null; // ��� �κ��丮 ������ ����   
                    if (Managers.Game.UseChoiceItem is EquipmentItem equip)
                    {
                        playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = equip; // ������ �κ��丮 ������ -> ��� �κ��丮
                        playerStat.Item.Remove(Managers.Game.UseChoiceItem.ItemNumber); // ������ �κ��丮�� ����
                        playerStat.PlayerStatRelease(equipmentItem); // ������ ��� �ɷ¸�ŭ �÷��̾� ���� ����
                        playerStat.PlayerStatUpgrade(equip); // ������ ��� �ɷ¸�ŭ �÷��̾� ���� ����
                    }                        
                }
                else if (Managers.Game.UseChoiceItem as EquipmentItem != null)
                {
                    equipmentItem = Managers.Game.UseChoiceItem as EquipmentItem;
                    playerStat.EquipmentState[playerStat.CurrentEquipmentCategory] = equipmentItem; // ��� �κ��丮�� �ְ�    
                    playerStat.Item.Remove(equipmentItem.ItemNumber); // ������ �κ��丮�� ����
                    playerStat.PlayerStatUpgrade(equipmentItem); // ������ ��� �ɷ¸�ŭ �÷��̾� ���� ����
                } 
            }
        
        }
        else if (itemClickCategory == Defines.ItemClickCategory.ConsumeUse)
        {
            // �÷��̾� ���ȿ��� �Һ� ������ ���ȿ�� �����ϱ�
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();
            playerStat.UseConsumeItem(Managers.Game.UseChoiceItem);
            // ������ �κ��丮���� �ش� �Һ� ������ ����
            playerStat.Item.Remove(Managers.Game.UseChoiceItem.ItemNumber);            
        }

        Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_UseOrNot>(), GameObject.FindWithTag("UI_Item_UseOrNot").transform.parent.gameObject);
        Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI<UI_Inven>("UI_Inven");
    }

    public void BtnOnClickedNo(PointerEventData data)
    {        
        Managers.UI.CloseSelectedPopupUI(gameObject.GetComponent<UI_Item_UseOrNot>(), GameObject.FindWithTag("UI_Item_UseOrNot").transform.parent.gameObject);
    }
}
