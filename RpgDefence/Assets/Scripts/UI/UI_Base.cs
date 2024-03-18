using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {        
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] object1 = new UnityEngine.Object[names.Length];
        //  Bind<T> 형식이 Bind<Button>인 경우 typeof(T)는 typeof(Button)
        objects.Add(typeof(T), object1);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                object1[i] = Util.FindChild(gameObject, names[i], true);
            else
                object1[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] _objects = null;
        if (objects.TryGetValue(typeof(T), out _objects) == false)
            return null;

        return _objects[idx] as T;
    }

    protected Text GetText(int idx)
    {
        return Get<Text>(idx);
    }

    protected GameObject GetGameObject(int idx)
    {
        return Get<GameObject>(idx);
    }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Defines.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }


        // 익명 메소드 형태로 이벤트 핸들러를 만들어서 OnDragHandler 이벤트가 발생시 호출할 이벤트 핸들러로 등록시킴
        // evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }
}
