using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Scene 매니저를 따로 만들어주는 이유? 
 * 다크소울 게임을 예시로 들자면 
 * 씬에 플레이어가 무조건 배치되어 있는게 아니라 클라이언트가 게임에 접속하고 그 플레이어로 게임을 시작해야지 동적으로 플레이어를 씬에 배치시킴
*/

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
