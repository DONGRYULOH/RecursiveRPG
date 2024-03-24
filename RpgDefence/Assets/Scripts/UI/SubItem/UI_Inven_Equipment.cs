using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Equipment : UI_Base
{
    // ��� ī�װ��� ���뿩��(���� X, �� O, .. �̷������� üũ)
    Defines.EquipmentCategory equipmentCategory;
    bool equipmentIsUsed = false;

    // GameObject ������ ����
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

            // �÷��̾ ������ ����� �̹���
            PlayerStat playerStat = Managers.Game.GetPlayer().GetComponent<PlayerStat>();            
            string imageName = playerStat.EquipmentState[equipmentCategory].ItemName; // Ȯ���� ���� �ؾ��� �ҷ���
            itemIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Item/" + imageName);

            // �÷��̾ ������ ����� �̸�
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
