using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    <������ Ǯ�� ���� ����>
    ex) ����A, ����B�� ���� ������ Ǯ�� ������Ʈ�� ����
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
        // Hierarchy�� ��������� � ���͸� Ǯ���ϰ� �ִ��� ���̹� ó��
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
