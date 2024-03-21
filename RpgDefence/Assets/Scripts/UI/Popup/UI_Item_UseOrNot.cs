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
            Get<Text>((int)Texts.UseOrNotText).text = "�ش� ��� �����Ͻðڽ��ϱ�?";   
        else if (itemClickCategory == Defines.ItemClickCategory.EquipmentUse)
            Get<Text>((int)Texts.UseOrNotText).text = "�ش� ��� �����Ͻðڽ��ϱ�?";
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
