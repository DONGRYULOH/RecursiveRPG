using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);        
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        // 인벤에 아이템 추가
        for(int i = 0; i < 8; i++)
        {           
            // parent : --> [힌트 표시] 
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent : gridPanel.transform).gameObject;

            // UI_Inven_Item 이라는 게임오브젝트에 붙어있는 컴포넌트(UI_Inven_Item 스크립트)를 가져옴
            UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item);
            invenItem.SetInfo("바인드 테스트");
        }
    }
}
