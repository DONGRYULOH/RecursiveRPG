using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*        
    팝업형태의 UI와 팝업이 아닌 그냥 UI를 관리하는 매니저
*/
public class UiManager 
{
    int _order = 10;

    Stack<UI_Popup> _popStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }        
    }

    // Popup 형태의 UI Canvas간의 우선순위를 세팅
    public void SetCanvas(GameObject go, bool sort = true)
    {
        // 모든 UI 요소는 Canvas 안에 위치함 
        // 모든 UI 요소는 반드시 어떤 Canvas의 자식이어야 한다.
        // Images, Text .. 등 이러한 UI를 만들때 어떤 Canvas의 자식으로 무조건 포함된다.
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        // sort = true 라면 popup canvas 경우고 false라면 일반 UI canvas
        if (sort)
        {
            canvas.sortingOrder = _order++;
        }            
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // ------------- Popup UI --------------------------

    // name : 프리팹 이름
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popStack.Push(popup);       

        // 씬에 생성된 모든 팝업이 하나의 빈 게임오브젝트 하위에 들어가도록 세팅 
        // 팝업이 여러개 생성되는 경우 씬에 오브젝트들이 너무 많아서 복잡해 보이니까 묶어서 보여주는 용도
        go.transform.SetParent(Root.transform);
         
        return popup;
    }

    // 닫을 팝업을 직접 인자로 받아서 사용하는 방식 
    // 중간에 있는 팝업을 강제로 삭제하려는 경우 이러한 방식을 쓰면 무조건 맨 위에 있는 팝업부터 삭제되므로 중간 삭제를 방지할 수 있음
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popStack.Count == 0)
            return;

        // 스택에 젤 위에있는 원소를 인자로 들어온 팝업과 비교
        // 스택은 젤 위에있는 원소부터 pop하기 때문에 인자로 들어온 팝업이 맨 위에 있는것이 아니라면 pop을 수행하지 않음
        if(_popStack.Peek() != popup)
        {
            Debug.Log("Close popup Failed");
            return;
        }

        CloseAllPopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popStack.Count == 0)
            return;

        UI_Popup popup = _popStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while(_popStack.Count > 0)
        {
            ClosePopupUI();
        }           
    }


    // ------------- Popup이 아닌 Scene UI --------------------------

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI; 

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    // ------------- Popup도 아니고 Scene 아닌 sub UI --------------------------

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);        
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 프리팹으로 만든 HpBar를 인스턴스화 시켜서 해당 캐릭터에 붙여줌
        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>();
        // RenderMode.ScreenSpaceCamera; --> 씬 밖에서 2D canvas 형태로 촬영
        canvas.renderMode = RenderMode.WorldSpace; // 씬 안에 있는 어떤 하나의 카메라를 사용해서 렌더링 처리
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    // 씬이 변경되면 기존에 있는 데이터를 없애버림
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }

}
