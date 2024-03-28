using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers S_Instance;
    public static Managers GetInstance { get { Init(); return S_Instance; } }

    // 게임 컨텐츠 별로 다르게 사용하는 각종 매니저
    #region Contents
    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game { get { return GetInstance._game; } }
    #endregion

    // 공통으로 사용하는 각종 매니저
    #region Core
    InputManager input = new InputManager();
    UiManager ui = new UiManager();
    ResourcesManager resource = new ResourcesManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();

    public static InputManager Input { get { return GetInstance.input; } }    
    public static UiManager UI { get { return GetInstance.ui; } }    
    public static ResourcesManager Resource { get { return GetInstance.resource; } }
    public static SceneManagerEx Scene { get { return GetInstance._scene; } }
    public static SoundManager Sound { get { return GetInstance._sound; } }
    public static PoolManager Pool { get { return GetInstance._pool; } }
    public static DataManager Data { get { return GetInstance._data; } }
    #endregion
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        input.OnUpdate(); // 입력(마우스 클릭 또는 키보드)감지를 매 프레임마다 체크
    }

    static void Init() {
        // Manager 오브젝트가 무조건 존재한다는 보장이 없기 때문에 null 체크를 해줘야 함                 
        if (S_Instance == null) {
            GameObject manager = GameObject.Find("Manager");

            if (manager == null) {
                manager = new GameObject { name = "@Managers" };
                manager.AddComponent<Managers>();
            }

            DontDestroyOnLoad(manager);
            S_Instance = manager.GetComponent<Managers>();

            // 각종 인스턴스 초기화 작업 수행
            S_Instance._data.Init();
            S_Instance._sound.Init(); // 게임시작시 Sound 붙이기
            S_Instance._pool.Init();  // pool 객체를 담을 Root를 만들어줌
            S_Instance._game.Init();  // 상점아이템, 처음 시작시 챕터 1부터 실행
        }
    }    
}
