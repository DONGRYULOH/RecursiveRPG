using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    private float seconds = 4.0f;
    private float limitSeconds;    

    protected override void Init()
    {
        base.Init();
        SceneType = Defines.Scene.Game;
        gameObject.GetOrAddComponent<CursorController>();

        LoadDataFile();      
        PlayerSpwan();
        

        // TODO : 타이머가 종료되고 다음챕터로 이동이 가능한 경우만 장비창 띄우기
        GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
        go.GetOrAddComponent<UI_MyInvenBtn>();
    }

    private void Start()
    {        
        // 챕터별로 제한시간 설정
        if(Managers.Game.CurrentChpater == 1)
        {
            limitSeconds = 60.0f;
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            limitSeconds = 90.0f;
        }
        else
        {
            limitSeconds = 120.0f;
        }


        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        Debug.Log(seconds);
        UI_GameStartAlert startAlert = Managers.UI.ShowPopupUI<UI_GameStartAlert>("UI_GameStartAlert");
        yield return new WaitForSeconds(1);

        while (seconds > 0.0f)
        {
            seconds -= 1f; // 각 프레임마다 1초씩 감소시킴

            if (seconds >= 3.0f)
            {
                Debug.Log(seconds);
                startAlert.CountDownText("3");
                yield return new WaitForSeconds(1);
            }
            else if (seconds >= 2.0f)
            {
                Debug.Log(seconds);
                startAlert.CountDownText("2");
                yield return new WaitForSeconds(1);
            }
            else if (seconds >= 1.0f)
            {
                Debug.Log(seconds);
                startAlert.CountDownText("1");
                yield return new WaitForSeconds(1);
            }
        }

        if (seconds <= 0.0f)
        {
            Managers.UI.ClosePopupUI(startAlert);
            // 게임시작!
            MakeMonsterPooling();
        }
    }

    private void Update()
    {
        
        // 0.카운트 다운 시작(0, 1, 2 start) 코루틴으로 3초가 지나면 그때부터 시간 측정


        // Managers.UI.ShowPopupUI<UI_GameStartAlert>("UI_GameStartAlert");
        // seconds += Time.deltaTime;

        // 1.제한시간이 지났는지 확인
    }

    // 데이터(플레이어, 몬스터 스탯..등등 매핑) 파일
    public void LoadDataFile()
    {
        Managers.Data.Init();
    }

    public void PlayerSpwan()
    {
        GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        // TODO : 게임시작시 첫번째 스테이지에서만 플레이어에게 기본 아이템을 일부 넣어주기
        EquipmentItem equipmentItem = new EquipmentItem(101, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon, 100);
        ConsumeItem consumeItem = new ConsumeItem(102, "HpPortion", 10, 0, 20);
        PlayerStat stat = player.GetOrAddComponent<PlayerStat>();
        stat.Item.Add(equipmentItem.ItemNumber, equipmentItem);
        stat.Item.Add(consumeItem.ItemNumber, consumeItem);                
    }

    public void MakeMonsterPooling()
    {
        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {

    }



}
