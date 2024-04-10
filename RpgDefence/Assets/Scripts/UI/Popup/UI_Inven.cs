using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven : UI_Popup
{
    Defines.UiInvenGridCategory invenGridCategory = Defines.UiInvenGridCategory.Unknown;    
    bool storeSellCheck; // ���� �Ǹ� or �׳� ������ �κ��丮�� ���� �̺�Ʈ�� �޶���

    public Defines.UiInvenGridCategory InvenGridCategory
    {
        get { return invenGridCategory; }
        set { invenGridCategory = value; }
    }

    public bool StoreSellCheck
    {
        get { return storeSellCheck; }
        set { storeSellCheck = value; }
    }    

    enum GameObjects
    {
        UI_Inven_Grid,
        UI_Equip_Grid,
        UI_Inven_Close,
        UI_Inven_Gold
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        if (invenGridCategory == Defines.UiInvenGridCategory.ItemGrid)
        {
            PlayerInvenItemGridSet();
            PlayerInvenGoldSet();
        }
        else if (invenGridCategory == Defines.UiInvenGridCategory.EquipmentGird)
        {
            PlayerInvenEquipGridSet();
        }
        BtnInvenCloseMapping();
    }

    public void PlayerInvenGoldSet()
    {
        GameObject playerGold = Get<GameObject>((int)GameObjects.UI_Inven_Gold);
        playerGold.GetComponent<Text>().text = Managers.Game.GetPlayer().GetComponent<PlayerStat>().Gold + "$";
    }

    public void PlayerInvenItemGridSet()
    {
        GameObject InvenGrid = Get<GameObject>((int)GameObjects.UI_Inven_Grid);
        foreach (Transform child in InvenGrid.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // �÷��̰� �����ϰ� �ִ� �������� �����ͼ� �κ��� �߰�
        GameObject player = Managers.Game.GetPlayer();
        PlayerStat stat = player.GetComponent<PlayerStat>();

        foreach (var playerItem in stat.Item)
        {            
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(InvenGrid.transform).gameObject;
            if(playerItem.Value.GetCatecory == Defines.ItemCategory.Equipment)
            {
                if (playerItem.Value is EquipmentItem equipment)
                {                
                    item.GetComponent<UI_Inven_Item>().EquipmentCategory = equipment.EquipmentCategory;
                }                
            }
            
            item.GetComponent<UI_Inven_Item>().ItemCategory = playerItem.Value.GetCatecory;
            item.GetComponent<UI_Inven_Item>().Name = playerItem.Value.ItemName;
            item.GetComponent<UI_Inven_Item>().ItmeInfo = playerItem.Value;
        }
    }

    public void PlayerInvenEquipGridSet()
    {
        GameObject equipGrid = Get<GameObject>((int)GameObjects.UI_Equip_Grid);
        foreach (Transform child in equipGrid.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // �÷��̰� ����ϰ� �ִ� �������� �����ͼ� �����¿� �߰�
        GameObject player = Managers.Game.GetPlayer();
        PlayerStat stat = player.GetComponent<PlayerStat>();

        foreach (var playerEquipmentItem in stat.EquipmentState)
        {
            // parent : --> [��Ʈ ǥ��] 
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Equipment>(parent: equipGrid.transform).gameObject;
            
            string name = null;
            switch (playerEquipmentItem.Key)
            {
                case Defines.EquipmentCategory.Weapon:
                    name = "����";
                    break;
                case Defines.EquipmentCategory.Armor:
                    name = "��";
                    break;
                case Defines.EquipmentCategory.Accessory:
                    name = "�׼�����";
                    break;
                case Defines.EquipmentCategory.Shoes:
                    name = "�Ź�";
                    break;

            }            
            item.GetComponent<UI_Inven_Equipment>().SetInfo(name);
            item.GetComponent<UI_Inven_Equipment>().EquipmentCategory = playerEquipmentItem.Key;

            // �÷��̾ �����ϰ� �ִ� ������� �ִ� ��츸 ȭ�鿡 �׷���
            if (playerEquipmentItem.Value != null)
            {
                item.GetComponent<UI_Inven_Equipment>().EquipmentIsUsed = true;
            }
        }
    }

    public void BtnInvenCloseMapping()
    {
        if (invenGridCategory == Defines.UiInvenGridCategory.ItemGrid)
        {
            GameObject InvenClose = Get<GameObject>((int)GameObjects.UI_Inven_Close);
            BindEvent(InvenClose, InvenCloseEvent, Defines.UIEvent.Click);
        }            
    }

    public void InvenCloseEvent(PointerEventData data)
    {
        if(GameObject.FindWithTag("UI_Item_UseOrNot") != null)        
            Managers.UI.CloseSelectedPopupUI(GameObject.FindWithTag("UI_Item_UseOrNot").GetComponent<UI_Item_UseOrNot>(), GameObject.FindWithTag("UI_Item_UseOrNot").GetComponent<UI_Item_UseOrNot>().transform.parent.gameObject);
        UI_MyInvenBtn.myInvenOpenCheck = false;

        // ���, ������ �κ��丮 �˾� �ݱ�
        Managers.UI.CloseSelectedPopupUI(GameObject.FindWithTag("UI_InvenEquipmentGrid").GetComponent<UI_Inven>(), GameObject.FindWithTag("UI_InvenEquipmentGrid").GetComponent<UI_Inven>().transform.parent.gameObject);
        Managers.UI.CloseSelectedPopupUI(GameObject.FindWithTag("UI_InvenItemGrid").GetComponent<UI_Inven>(), GameObject.FindWithTag("UI_InvenItemGrid").GetComponent<UI_Inven>().transform.parent.gameObject);                
    }
}
