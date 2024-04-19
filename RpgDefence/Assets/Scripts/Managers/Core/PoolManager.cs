using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각각의 풀을 생성하고, 풀에서 오브젝트를 꺼내고 집어넣는 역할을 수행한다.
public class PoolManager
{    
    // 여러개의 풀을 관리하기 위한 컨테이너
    Dictionary<string, PoolObjectData> _pool = new Dictionary<string, PoolObjectData>();
    
    Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
        }
    }
    
    public void CreatePool(GameObject original)
    {
        int poolObjectCount = 0;
        if (Managers.Game.CurrentChpater == 1)
            poolObjectCount = 10;
        else if (Managers.Game.CurrentChpater == 2)
            poolObjectCount = 15;
        else
            poolObjectCount = 20;

        PoolObjectData pool = new PoolObjectData();
        pool.Init(original, poolObjectCount);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    // 오브젝트를 다시 풀에 넣음
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject); // Pool이 없으니까 오브젝트를 그냥 메모리에서 해제
            return;
        }

        _pool[name].Push(poolable);
    }  

    // 풀에서 오브젝트를 꺼내옴
    public GameObject Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop().gameObject;
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in _root)        
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
