using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region PoolClass
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for(int i = 0; i < count; i++)
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
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if(_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            poolable.gameObject.SetActive(true);

            // Don't DestroyOnLoad 해제 용도
            if (parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    // 여러개의 pool을 종류별로 가지고 있음
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    // 트랜스폼은 무조건 GameObject가 가지고 있는 컴포넌트이기 때문에 GameObject가 존재하는것 대신에 Transform이 있는지 체크해도 됨
    Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

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

    // 해당 오브젝트에 관련된 Pool이 없으면 해당 종류의 Pool 객체를 만듬
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    // Q) Pool 객체를 날려버릴지 아니면 계속 들고 있을껀지? 
    // 게임마다 다르기 때문에 상황에 맞춰서 구현해야함 
    // ex) 대규모 MMORPG 게임에서 지역마다 오브젝트가 달라지고 그 오브젝트의 수도 많을 경우 굳이 기존의 Pool을 계속 들고 있을 필요가 없음
    // 다른 씬으로 넘어갔는데 사용을 안하는 Pool 객체가 많이있으면 그만큼 메모리를 많이 잡아먹기 때문에 성능이 저하됨
    // ex) 롤같은 AOS 게임에서는 한 맵에서 계속 플레이되기 때문에 굳이 Pool 객체를 날려줄 필요가 없음
    public void Clear()
    {
        foreach(Transform child in _root)        
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
