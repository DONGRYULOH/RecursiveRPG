using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    private float seconds = 4.0f;
    private float limitSeconds;
    private int nextScore;

    SpawningPool monsterSpawningPool;

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
        // 챕터별로 제한시간, 스코어 점수 설정
        if(Managers.Game.CurrentChpater == 1)
        {
            limitSeconds = 60.0f;
            nextScore = 10;
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            limitSeconds = 90.0f;
            nextScore = 20;
        }
        else
        {
            limitSeconds = 120.0f;
            nextScore = 30;
        }

        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {        
        UI_GameStartAlert startAlert = Managers.UI.ShowPopupUI<UI_GameStartAlert>("UI_GameStartAlert");
        yield return new WaitForSeconds(1);

        while (seconds > 0.0f)
        {
            seconds -= 1f; // 각 프레임마다 1초씩 감소시킴

            if (seconds >= 3.0f)
            {                
                startAlert.CountDownText("3");
                yield return new WaitForSeconds(1);
            }
            else if (seconds >= 2.0f)
            {                
                startAlert.CountDownText("2");
                yield return new WaitForSeconds(1);
            }
            else if (seconds >= 1.0f)
            {                
                startAlert.CountDownText("1");
                yield return new WaitForSeconds(1);
            }
        }

        // 게임시작!
        if (seconds <= 0.0f)
        {            
            Managers.UI.ClosePopupUI(startAlert);            
            MakeMonsterPooling();
            
            // 다음 스테이지로 가기위한 점수 UI(점수 고정)
            GameObject nextScore = Managers.Resource.Instantiate($"UI/Scene/NextChapterScore");
            if (nextScore.transform.GetChild(0) != null)
                nextScore.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Next Stage Score : " + this.nextScore;

            // 플레이어가 획득한 점수 UI(점수 실시간으로 변경) 
            GameObject playerScore = Managers.Resource.Instantiate($"UI/Scene/CurrentPlayerScore");
            Text playerScoreText = null;
            if (playerScore.transform.GetChild(0) != null)
            {
                playerScoreText = playerScore.transform.GetChild(0).gameObject.GetComponent<Text>();
                playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            }                            

            // 타이머 UI
            GameObject timer = Managers.Resource.Instantiate($"UI/Scene/Timer");
            Text timerText = null;
            if (timer.transform.GetChild(0) != null)
                timerText = timer.transform.GetChild(0).gameObject.GetComponent<Text>();

            while(seconds <= limitSeconds)
            {             
                seconds += Time.deltaTime;
                playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
                string minutes = Mathf.Floor(seconds / 60).ToString("00");
                string secondsTime = (seconds % 60).ToString("00");
                timerText.text = "경과한 시간 " + string.Format("{0}:{1}", minutes, secondsTime);
                yield return null; // *** 1프레임이 1초가 아니라 엄청 짧은 시간 0.0001초 간격일 수 있음 컴퓨터 성능에 따라 다름
            }

            // Debug.Log(seconds); // 60.00827
            int finalPlayerScore = Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            if (finalPlayerScore >= this.nextScore)
            {
                Managers.Resource.Destroy(monsterSpawningPool.gameObject); // 몬스터 풀링 객체 제거                
                StartCoroutine("NextStageAlert");                
                Managers.Game.MonsterAllRemove(); // 필드에 있는 몬스터 제거
                Managers.Game.OpenDoor(); // 다음챕터이동, 상점으로 가는 문 열어두기
            }
            else
            {                
                Managers.Scene.LoadScene(Defines.Scene.Main);
            }
        }
    }

    IEnumerator NextStageAlert()
    {
        UI_TextPopup alert = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");        
        yield return null;
        alert.SetText("다음 스테이지 이동 또는 상점 이동하기 위해서 두 개의 문을 이용하세요");
    }

    private void Update()
    {
               
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
        monsterSpawningPool = go.GetOrAddComponent<SpawningPool>();
        monsterSpawningPool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {

    }



}
