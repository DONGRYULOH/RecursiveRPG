using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ Ǯ�� �����ϰ�, Ǯ���� ������Ʈ�� ������ ����ִ� ������ �����Ѵ�.
public class PoolManager
{    
    // �������� Ǯ�� �����ϱ� ���� �����̳�
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

    // ������Ʈ�� �ٽ� Ǯ�� ����
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject); // Pool�� �����ϱ� ������Ʈ�� �׳� �޸𸮿��� ����
            return;
        }

        _pool[name].Push(poolable);
    }  

    // Ǯ���� ������Ʈ�� ������
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
