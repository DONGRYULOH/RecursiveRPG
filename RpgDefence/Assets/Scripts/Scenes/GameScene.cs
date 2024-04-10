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
    GameObject bossMonster;

    protected override void Init()
    {
        base.Init();
        SceneType = Defines.Scene.Game;
        gameObject.GetOrAddComponent<CursorController>();
        CursorController.chapterOrStoreClick = false;
        PlayerSpwan();                        
    }

    private void Start()
    {        
        // 챕터별로 제한시간, 스코어 점수 설정
        if(Managers.Game.CurrentChpater == 1)
        {
            limitSeconds = 1f;
            nextScore = 0;
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            limitSeconds = 90f;
            nextScore = 20;
        }
        else if(Managers.Game.CurrentChpater == 3)
        {
            limitSeconds = 120f;            
        }
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {        
        // 1.게임 시작 전 카운트 다운 UI 
        UI_GameStartAlert startAlert = Managers.UI.ShowPopupUI<UI_GameStartAlert>("UI_GameStartAlert");
        yield return new WaitForSeconds(1);

        while (seconds > 0.0f)
        {
            seconds -= 1f;

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

        // 2.게임시작!
        if (seconds <= 0.0f)
        {            
            Managers.UI.ClosePopupUI(startAlert);
            GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
            go.GetOrAddComponent<UI_MyInvenBtn>();
            MakeMonsterPooling();

            GameObject nextScore = Managers.Resource.Instantiate($"UI/Scene/NextChapterScore");
            GameObject playerScore = Managers.Resource.Instantiate($"UI/Scene/CurrentPlayerScore");
            string scoreForNext = null;            
            // 마지막 챕터면 보스 몬스터 생성
            if (Managers.Game.CurrentChpater == 3)
            {
                MakeBossMonster();                
                scoreForNext = "마지막 스테이지";                
            }
            else
            {                
                scoreForNext = "Next Stage Score : " + this.nextScore;               
            }
            // 다음 스테이지로 가기위한 점수 UI(점수 고정)
            if (nextScore.transform.GetChild(0) != null)
                nextScore.transform.GetChild(0).gameObject.GetComponent<Text>().text = scoreForNext;
            // 플레이어가 획득한 점수 UI(점수 실시간으로 변경)     
            Text playerScoreText = null;
            if (playerScore.transform.GetChild(0) != null)
            {
                playerScoreText = playerScore.transform.GetChild(0).gameObject.GetComponent<Text>();
                playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            }            

            // 타이머 UI 제한시간 체크
            GameObject timer = Managers.Resource.Instantiate($"UI/Scene/Timer");
            Text timerText = null;
            if (timer.transform.GetChild(0) != null)
                timerText = timer.transform.GetChild(0).gameObject.GetComponent<Text>();

            while(seconds <= limitSeconds)
            {
                // 게임도중에 플레이어가 죽거나 제한시간 안에 보스를 죽이면 타이머 중지
                if (Managers.Game.GetPlayer() == null)
                {
                    yield break;
                }
                else
                {
                    if(Managers.Game.CurrentChpater == 3 && bossMonster == null)
                    {
                        // 게임 클리어!                        
                        ShowGameClearPopup();
                        yield break;
                    }

                    seconds += Time.deltaTime;                    
                    playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
                    string minutes = Mathf.Floor(seconds / 60).ToString("00");
                    string secondsTime = (seconds % 60).ToString("00");
                    timerText.text = "경과한 시간 " + string.Format("{0}:{1}", minutes, secondsTime) + " (제한시간 : " + limitSeconds + " 초)";
                    yield return null; // *** 1프레임이 1초가 아니라 엄청 짧은 시간 0.0001초 간격일 수 있음 컴퓨터 성능에 따라 다름
                }                
            }
            
            if(Managers.Game.CurrentChpater == 3)
            {
                ShowGameOverPopup();
            }
            else
            {
                int finalPlayerScore = Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
                if (finalPlayerScore >= this.nextScore)
                    Clear();
                else
                    ShowGameOverPopup();
            }                        
        }
    }
    void ShowGameOverPopup()
    {
        Managers.Resource.Destroy(monsterSpawningPool.gameObject); // 몬스터 풀링 제거
        Managers.Game.MonsterAllRemove(); // 필드에 있는 몬스터 제거
        Managers.UI.ShowPopupUI<UI_GameClearPopup>("UI_GameOverPopup");
    }

    void ShowGameClearPopup()
    {
        Managers.Resource.Destroy(monsterSpawningPool.gameObject); // 몬스터 풀링 제거
        Managers.Game.MonsterAllRemove(); // 필드에 있는 몬스터 제거
        Managers.UI.ShowPopupUI<UI_GameClearPopup>("UI_GameClearPopup");
    }

    public void PlayerSpwan()
    {        
        if (Managers.Game.GetPlayer() == null)
        {
            GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);                        
        }
        else
        {
            Managers.Game.GetPlayer().GetComponent<Transform>().position = new Vector3(0, 0, 0);
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(Managers.Game.GetPlayer());
        }
    }

    public void MakeMonsterPooling()
    {
        GameObject go = new GameObject { name = "SpawningPool" };
        monsterSpawningPool = go.GetOrAddComponent<SpawningPool>();
        monsterSpawningPool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {        
        DontDestroyOnLoad(Managers.Game.GetPlayer());              // 다음 챕터로 이동해도 현재 플레이어의 상태를 계속 유지
        Managers.Resource.Destroy(monsterSpawningPool.gameObject); // 몬스터 풀링 제거
        StartCoroutine("NextStageAlert");
        Managers.Game.MonsterAllRemove(); // 필드에 있는 몬스터 제거
        Managers.Game.OpenDoor();         // 다음챕터이동, 상점으로 가는 문 열어두기        
    }

    IEnumerator NextStageAlert()
    {
        UI_TextPopup alert = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
        yield return null;
        alert.SetText("다음 스테이지 이동 또는 상점 이동하기 위해서 두 개의 문을 이용하세요");
    }

    void MakeBossMonster()
    {                
        bossMonster = Managers.Game.BossMonsterSpawn("Monster/Boss");
    }
}
