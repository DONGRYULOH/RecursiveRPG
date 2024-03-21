using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 공통적으로 사용되는 매니저가 아니라 해당 컨텐츠에서만 사용되는 특수한 매니저
// ex) 플레이어가 몬스터를 때렸을때 어떠한 처리해줄건지, 챕터 이동

public class GameManagerEx
{
    int currentChpater; // 현재 플레이어가 있는 챕터(맵)
    public int CurrentChpater { get{ return currentChpater; } set{ currentChpater = value;} }

    GameObject player;
    
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    Defines.ItemClickCategory itemClickCategory = Defines.ItemClickCategory.Unknown;
    public Defines.ItemClickCategory ItemClickCategory { get { return itemClickCategory; } set { itemClickCategory = value; } }

    public GameObject GetPlayer() { return player; }

    public void MoveNextChpater()
    {        
        switch (++currentChpater)
        {
            case 1:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterTwo);
                break;
            case 2:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterTwo);
                break;
            case 3:
                Managers.Scene.LoadChpater(Defines.Chapter.ChapterTwo);
                break;
        }
    }

    public void MoveStore()
    {
        Managers.Scene.LoadScene(Defines.Scene.Store);
    }

    public GameObject Spawn(Defines.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Defines.WorldObject.Monster:
                _monsters.Add(go);
                if(OnSpawnEvent != null)                
                    OnSpawnEvent.Invoke(1);                
                break;
            case Defines.WorldObject.Player:
                player = go;
                break;
        }        
        return go;
    }

    public Defines.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Defines.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void DeSpawn(GameObject go)
    {
        Defines.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Defines.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                    }                                           
                }
                break;
            case Defines.WorldObject.Player:
                {
                    if (player == go)
                        player = null;
                }                
                break;
        }

        Managers.Resource.Destroy(go);
    }

}
