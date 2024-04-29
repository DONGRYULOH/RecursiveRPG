using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// 공통적으로 사용되는 매니저가 아니라 해당 컨텐츠에서만 사용되는 특수한 매니저
// ex) 플레이어가 몬스터를 때렸을때 어떠한 처리해줄건지, 챕터 이동

public class GameManagerEx
{
    // 현재 플레이어가 있는 챕터(맵)
    int currentChpater; 
    public int CurrentChpater { get{ return currentChpater; } set{ currentChpater = value;} }

    // 플레이어 정보
    GameObject player;
    public GameObject GetPlayer() { return player; }

    // 생성된 몬스터 정보
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    Defines.ItemClickCategory itemClickCategory = Defines.ItemClickCategory.Unknown;
    public Defines.ItemClickCategory ItemClickCategory { get { return itemClickCategory; } set { itemClickCategory = value; } }

    // 사용을 위해 선택한 아이템
    Item useChoiceItem = null; 
    public Item UseChoiceItem { get { return useChoiceItem; } set { useChoiceItem = value; } }

    // 상점에 있는 아이템 정보
    Dictionary<int, Item> storeItem = new Dictionary<int, Item>();
    public Dictionary<int, Item> StoreItem { get { return storeItem; } set { storeItem = value; } }

    int currentItemNumberIndex;
    public int CurrentItemNumberIndex { get { return currentItemNumberIndex++; } set { currentItemNumberIndex = value; } }    

    public void Init()
    {
        currentChpater = 1;
        MakeStoreItem();
    }

    public void MonsterAllRemove()
    {
        List<GameObject> monsters = new List<GameObject>();

        // hashset에 들어가 있는 몬스터를 꺼내오기 위해 컬렉션을 반복하는 동안 해당 컬렉션을 수정하면 안되기 때문에 List에 넣음
        foreach (GameObject monster in _monsters)
        {
            monsters.Add(monster);            
        }

        for(int i = 0; i < monsters.Count; i++)
        {
            DeSpawn(monsters[i]);
        }        
    }

    public void OpenDoor()
    {     
        GameObject nextChapter = Managers.Resource.Instantiate("NextChapter");
        UI_EventHandler nextChapterOnClickEvent = nextChapter.GetOrAddComponent<UI_EventHandler>();
        nextChapterOnClickEvent.OnClickHandler += ShowSelectPopupUI;

        GameObject store = Managers.Resource.Instantiate("Store");
        UI_EventHandler storeOnClickEvent = store.GetOrAddComponent<UI_EventHandler>();
        storeOnClickEvent.OnClickHandler += ShowSelectPopupUI;     
    }

    public void ShowSelectPopupUI(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerEnter.gameObject;
        if(obj.name == "NextChapter")
            CursorController._cursorType = CursorController.CursorType.NextChapter;
        else
            CursorController._cursorType = CursorController.CursorType.Store;

        Managers.UI.ShowPopupUI<UI_Choice>();
    }

    // 다음 챕터로 넘어가도 상점에 있는 아이템은 그대로 고정이므로 초기화시 딱 한번만 만들어줌
    public void MakeStoreItem()
    {
        // Managers.Game.CurrentItemNumberIndex 1~100 까지 상점아이템으로 전용으로 사용
        EquipmentItem equipmentItem1 = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "LongSword", 150, 0, 0, 0, Defines.EquipmentCategory.Weapon, 5000);
        EquipmentItem equipmentItem2 = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "가죽갑옷", 0, 30, 0, 0, Defines.EquipmentCategory.Armor, 2500);
        EquipmentItem equipmentItem3 = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "기본망토", 0, 0, 50, 0, Defines.EquipmentCategory.Accessory, 2500);
        EquipmentItem equipmentItem4 = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "신속의장화", 0, 0, 0, 5.5f, Defines.EquipmentCategory.Shoes, 2500);
        StoreItem.Add(equipmentItem1.ItemNumber, equipmentItem1);
        StoreItem.Add(equipmentItem2.ItemNumber, equipmentItem2);
        StoreItem.Add(equipmentItem3.ItemNumber, equipmentItem3);
        StoreItem.Add(equipmentItem4.ItemNumber, equipmentItem4);
        ConsumeItem consumeItem = new ConsumeItem(Managers.Game.CurrentItemNumberIndex, "BigHpPortion", 100, 0, 100);
        StoreItem.Add(consumeItem.ItemNumber, consumeItem);
    }

    public void MoveNextChpater()
    {        
        switch (++currentChpater)
        {
            case 1:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterOne);
                break;
            case 2:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterTwo);
                break;
            case 3:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterThree);
                break;
        }
    }

    public void MoveStore()
    {
        if (GameObject.FindWithTag("UI_Store") == null)
        {
            // MyInven(아이템, 장비)이 열려져 있으면 강제로 닫음
            if(GameObject.FindWithTag("UI_InvenEquipmentGrid") != null && GameObject.FindWithTag("UI_InvenItemGrid") != null)
            {
                UI_MyInvenBtn.myInvenOpenCheck = false;
            }                
            Managers.UI.CloseAllParentPopupUI();

            // 상점과 아이템 인벤 팝업 띄우기
            Managers.UI.ShowPopupUI<UI_Store>("UI_Store");
            UI_Inven InvenItem = Managers.UI.ShowPopupUI<UI_Inven>("UI_InvenItemGrid");
            foreach(Transform child in InvenItem.gameObject.transform)
            {
                if (child.gameObject.name == "UI_Inven_Close")                
                    Managers.Resource.Destroy(child.gameObject);                                    
            }            
            InvenItem.InvenGridCategory = Defines.UiInvenGridCategory.ItemGrid;
            InvenItem.StoreSellCheck = true;
        }            
    }

    public GameObject BossMonsterSpawn(string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        _monsters.Add(go);
        return go;
    }

    public GameObject Spawn(Defines.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Defines.WorldObject.Monster:         
                _monsters.Add(go);
                if(OnSpawnEvent != null)                
                    OnSpawnEvent.Invoke(1);                
                break;
            case Defines.WorldObject.Player:             
                player = go;
                break;
        }        
        return go;
    }

    public Defines.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Defines.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void DeSpawn(GameObject go)
    {
        Defines.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Defines.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                        if(go.GetComponent<Poolable>() != null)
                            go.GetComponent<Poolable>().InitBeforeInactive();
                    }                                           
                }
                break;
            case Defines.WorldObject.Player:
                {
                    if (player == go)
                    {
                        GameObject.FindWithTag("UI_MyInvenBtn").SetActive(false); // 장비창 인벤 비활성화                        
                        player = null;
                    }                        
                }                
                break;
        }

        Managers.Resource.Destroy(go);
    }

}
