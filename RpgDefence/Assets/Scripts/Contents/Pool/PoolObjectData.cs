using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    <각각의 풀에 대한 정보>
    ex) 몬스터A, 몬스터B에 대한 각각의 풀링 오브젝트를 만듬
 */
public class PoolObjectData
{  
    public int PoolObjectCount { get; private set; }
    public GameObject Original { get; private set; }
    public Transform Root { get; set; }

    Stack<Poolable> _poolStack = new Stack<Poolable>();

    public void Init(GameObject original,int poolObjectCount)
    {
        Original = original;
        PoolObjectCount = poolObjectCount;
        // Hierarchy에 명시적으로 어떤 몬스터를 풀링하고 있는지 네이밍 처리
        Root = new GameObject().transform;
        Root.name = $"{original.name}_Pool";

        for (int i = 0; i < PoolObjectCount; i++)
        {
            Push(Create());
        }
    }

    Poolable Create()
    {
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = Original.name;
        return go.GetOrAddComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        poolable.transform.parent = Root;
        poolable.gameObject.SetActive(false);        

        _poolStack.Push(poolable);
    }

    public Poolable Pop()
    {
        Poolable poolable;

        if (_poolStack.Count > 0)
        {
            poolable = _poolStack.Pop();
        }
        else
        {
            poolable = Create();
        }

        poolable.gameObject.SetActive(true);
        poolable.transform.SetParent(null);

        return poolable;
    }
    
}
