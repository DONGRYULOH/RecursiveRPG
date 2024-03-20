using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        UI_Inven_Grid,
        UI_Inven_Close
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        BtnInvenCloseMapping();

        GameObject InvenGrid = Get<GameObject>((int)GameObjects.UI_Inven_Grid);        
        foreach (Transform child in InvenGrid.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // �÷��̰� �����ϰ� �ִ� �������� �����ͼ� �κ��� �߰�
        GameObject player = Managers.Game.GetPlayer();
        PlayerStat stat = player.GetComponent<PlayerStat>();

        foreach(var playerItem in stat.Item)
        {
            // parent : --> [��Ʈ ǥ��] 
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: InvenGrid.transform, playerItem.Key).gameObject;
            item.GetComponent<UI_Inven_Item>().SetInfo(playerItem.Key);
        }
    }

    public void BtnInvenCloseMapping()
    {
        GameObject InvenClose = Get<GameObject>((int)GameObjects.UI_Inven_Close);
        BindEvent(InvenClose, InvenCloseEvent, Defines.UIEvent.Click);
    }

    public void InvenCloseEvent(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
        UI_MyInvenBtn.myInvenOpenCheck = false;
    }
}
