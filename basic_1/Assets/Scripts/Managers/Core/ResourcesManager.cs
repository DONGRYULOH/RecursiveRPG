using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{

    public T Load<T>(string path) where T : Object {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if(index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null) 
    {
        // 가져오려는 오브젝트가 해당 위치에 실제로 있는지 확인
        // 해당 오브젝트가 풀링 대상이라면 풀안에 있을수도 있으니까 폴더를 뒤지지 않고 풀객체 종류중에서 해당 오브젝트가 있는지 확인
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null) {
            Debug.Log("경로가 잘못됨");
            return null;
        }

        // 풀링 대상인 객체에 한해서만 풀에서 해당 객체를 가져옴        
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // ex) 플레이어 전용 상태창 UI 같이 한번 인스턴스화 하고 계속 사용되는 경우는 풀링을 할 필요 없으므로 그냥 그 오브젝트를 인스턴스화 해줌
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject obj) {
        if (obj == null)
            return;

        // 만약에 풀링이 필요한 대상이라면 해당 오브젝트를 메모리에서 해제하지 않고 풀(대기실)로 이동시킴
        if (obj.GetComponent<Poolable>() != null)
            Managers.Pool.Push(obj.GetComponent<Poolable>());
        else        
            Object.Destroy(obj);
    }

}
