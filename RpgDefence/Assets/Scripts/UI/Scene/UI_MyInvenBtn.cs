using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MyInvenBtn : UI_Scene
{
    public static bool myInvenOpenCheck = false;

    enum Buttons
    {
        BtnMyInven
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // �θ��� Init() �޼ҵ� �����ؼ� Canvas Sort �۾� ����

        Bind<Button>(typeof(Buttons));
        GameObject pb = Get<Button>((int)Buttons.BtnMyInven).gameObject;
        BindEvent(pb, BtnOnClicked, Defines.UIEvent.Click);
    }

    public void BtnOnClicked(PointerEventData eventData)
    {
        // �ӽ� : �κ��丮 ����        
        if (!myInvenOpenCheck)
        {
            UI_Inven InvenItem = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenItemGrid");
            InvenItem.InvenGridCategory = Defines.UiInvenGridCategory.ItemGrid;

            UI_Inven InvenEquipment = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenEquipmentGrid");
            InvenEquipment.InvenGridCategory = Defines.UiInvenGridCategory.EquipmentGird;
            myInvenOpenCheck = true;
        }        
    }

}
