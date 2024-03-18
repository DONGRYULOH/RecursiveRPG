using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 destPos;

    [SerializeField]
    protected GameObject _lockTarget; // 해당 오브젝트 타켓팅 설정

    public Defines.WorldObject WorldObjectType { get; protected set; } = Defines.WorldObject.Unknown;

    public Vector3 DestPos { get { return destPos; } set { destPos = value; } }
    public GameObject LockTarget { get { return _lockTarget; } set { _lockTarget = value; } }

    private void Start()
    {
        Init(); // 자식에서 오버라이드된 Init()이 있으면 자식의 Init() 실행됨
    }
    
    public abstract void Init();
}
