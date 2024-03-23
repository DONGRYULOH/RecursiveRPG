using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Store : UI_Popup
{
    enum GameObjects
    {
        UI_Store_Close,
        UI_Store_Grid        
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        StoreItemGridSet();        
        BtnStoreCloseMapping();
    }

    public void StoreItemGridSet()
    {
        GameObject storeGrid = Get<GameObject>((int)GameObjects.UI_Store_Grid);
        foreach (Transform child in storeGrid.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        Dictionary<int, Item> storeItem = Managers.Game.StoreItem;
        foreach (var store in storeItem)
        {                        
            GameObject item = Managers.UI.MakeSubItem<UI_Store_Item>(storeGrid.transform).gameObject;
            item.GetComponent<UI_Store_Item>().ItmeInfo = store.Value;
            item.GetComponent<UI_Store_Item>().Name = store.Value.ItemName;
            item.GetComponent<UI_Store_Item>().Price = store.Value.Price;
        }
    }

    public void BtnStoreCloseMapping()
    {
        GameObject InvenClose = Get<GameObject>((int)GameObjects.UI_Store_Close);
        BindEvent(InvenClose, storeCloseEvent, Defines.UIEvent.Click);
    }

    public void storeCloseEvent(PointerEventData data)
    {
        if (GameObject.FindWithTag("UI_Store") != null)
        {
            Managers.UI.CloseAllPopupUI();
            CursorController.chapterOrStoreClick = false;
        }            
    }
}
