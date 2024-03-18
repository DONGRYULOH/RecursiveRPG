using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{    
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }


    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive == false)
        {
            // 찾고자 하는것이 GameObject(부모) 바로 밑에 있는 경우
            // parent 
            // - child 1 (O)            
            // -- child 2  (X)
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            // GameObject의 하위 컴포넌트를 T타입인 것만 가져옴
            foreach (T component in go.GetComponentsInChildren<T>())
            {                
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }
}
