using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{    
    public Defines.Scene SceneType
    {
        get;
        protected set;
    } = Defines.Scene.Unknown;

    // Awake는 게임 오브젝트가 비활성인 상태에서도 실행됨 
    // Start() 경우는 비활성인 상태에서 실행이 안됨 
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        // 씬에 EventSystem이 없으면 직접 만들어서 붙여줌 (EventSystem이 없으면 UI 이벤트 관련된 것들이 안먹히기 때문)
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
