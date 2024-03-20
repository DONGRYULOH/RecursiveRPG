using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        UI_Inven_Grid
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject InvenGrid = Get<GameObject>((int)GameObjects.UI_Inven_Grid);        
        foreach (Transform child in InvenGrid.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // TODO : ������ �÷��̰� ����ִ� �������� �����ͼ� �κ��� �߰�
        GameObject player = Managers.Game.GetPlayer();
        PlayerStat stat = player.GetComponent<PlayerStat>();

        foreach(var playerItem in stat.Item)
        {
            // parent : --> [��Ʈ ǥ��] 
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: InvenGrid.transform).gameObject;
            UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item);

            string setItemName;
            if(playerItem.Value.GetCatecory == Defines.ItemCategory.Equipment)
            {
               // setItemName = playerItem.Value.Power;
            }                        

            // invenItem.SetInfo(playerItem.Value.);
        }
    }
}
