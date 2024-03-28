using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers S_Instance;
    public static Managers GetInstance { get { Init(); return S_Instance; } }

    // ���� ������ ���� �ٸ��� ����ϴ� ���� �Ŵ���
    #region Contents
    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game { get { return GetInstance._game; } }
    #endregion

    // �������� ����ϴ� ���� �Ŵ���
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
        input.OnUpdate(); // �Է�(���콺 Ŭ�� �Ǵ� Ű����)������ �� �����Ӹ��� üũ
    }

    static void Init() {
        // Manager ������Ʈ�� ������ �����Ѵٴ� ������ ���� ������ null üũ�� ����� ��                 
        if (S_Instance == null) {
            GameObject manager = GameObject.Find("Manager");

            if (manager == null) {
                manager = new GameObject { name = "@Managers" };
                manager.AddComponent<Managers>();
            }

            DontDestroyOnLoad(manager);
            S_Instance = manager.GetComponent<Managers>();

            // ���� �ν��Ͻ� �ʱ�ȭ �۾� ����
            S_Instance._data.Init();
            S_Instance._sound.Init(); // ���ӽ��۽� Sound ���̱�
            S_Instance._pool.Init();  // pool ��ü�� ���� Root�� �������
            S_Instance._game.Init();  // ����������, ó�� ���۽� é�� 1���� ����
        }
    }    
}
