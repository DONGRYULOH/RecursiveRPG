using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    private float seconds = 4.0f;
    private float limitSeconds;
    private int nextScore;

    MonsterSpawn monsterSpawning;
    GameObject bossMonster;

    protected override void Init()
    {
        base.Init();
        SceneType = Defines.Scene.Game;        
        Managers.Pool.Init();
        PlayerSpwan();
    }

    private void Start()
    {        
        // é�ͺ��� ���ѽð�, ���ھ� ���� ����        
        if (Managers.Game.CurrentChpater == 1)
        {
            limitSeconds = 10f;
            nextScore = 0;           
        }
        else if (Managers.Game.CurrentChpater == 2)
        {
            limitSeconds = 10f;
            nextScore = 0;
            
        }
        else if(Managers.Game.CurrentChpater == 3)
        {
            limitSeconds = 60f;         
        }        
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {        
        // 1.���� ���� �� ī��Ʈ �ٿ� UI 
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

        // 2.���ӽ���!
        if (seconds <= 0.0f)
        {            
            Managers.UI.ClosePopupUI(startAlert);
            GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
            go.GetOrAddComponent<UI_MyInvenBtn>();
            MakeMonsterSpawn();

            GameObject nextScore = Managers.Resource.Instantiate($"UI/Scene/NextChapterScore");
            GameObject playerScore = Managers.Resource.Instantiate($"UI/Scene/CurrentPlayerScore");
            string scoreForNext = null;            
            // ������ é�͸� ���� ���� ����
            if (Managers.Game.CurrentChpater == 3)
            {
                MakeBossMonster();                
                scoreForNext = "������ ��������";                
            }
            else
            {                
                scoreForNext = "Next Stage Score : " + this.nextScore;               
            }
            // ���� ���������� �������� ���� UI(���� ����)
            if (nextScore.transform.GetChild(0) != null)
                nextScore.transform.GetChild(0).gameObject.GetComponent<Text>().text = scoreForNext;
            // �÷��̾ ȹ���� ���� UI(���� �ǽð����� ����)     
            Text playerScoreText = null;
            if (playerScore.transform.GetChild(0) != null)
            {
                playerScoreText = playerScore.transform.GetChild(0).gameObject.GetComponent<Text>();
                playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            }            

            // Ÿ�̸� UI ���ѽð� üũ
            GameObject timer = Managers.Resource.Instantiate($"UI/Scene/Timer");
            Text timerText = null;
            if (timer.transform.GetChild(0) != null)
                timerText = timer.transform.GetChild(0).gameObject.GetComponent<Text>();

            while(seconds <= limitSeconds)
            {
                // ���ӵ��߿� �÷��̾ �װų� ���ѽð� �ȿ� ������ ���̸� Ÿ�̸� ����
                if (Managers.Game.GetPlayer() == null)
                {
                    yield break;
                }
                else
                {
                    if(Managers.Game.CurrentChpater == 3 && bossMonster == null)
                    {
                        // ���� Ŭ����!                        
                        ShowGameClearPopup();
                        yield break;
                    }

                    seconds += Time.deltaTime;                    
                    playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
                    string minutes = Mathf.Floor(seconds / 60).ToString("00");
                    string secondsTime = (seconds % 60).ToString("00");
                    timerText.text = "����� �ð� " + string.Format("{0}:{1}", minutes, secondsTime) + " (���ѽð� : " + limitSeconds + " ��)";
                    yield return null; // *** 1�������� 1�ʰ� �ƴ϶� ��û ª�� �ð� 0.0001�� ������ �� ���� ��ǻ�� ���ɿ� ���� �ٸ�
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
        Managers.Resource.Destroy(monsterSpawning.gameObject); // ���� Ǯ�� ����
        Managers.Game.MonsterAllRemove(); // �ʵ忡 �ִ� ���� ����
        Managers.UI.ShowPopupUI<UI_GameClearPopup>("UI_GameOverPopup");
    }

    void ShowGameClearPopup()
    {
        Managers.Resource.Destroy(monsterSpawning.gameObject); // ���� Ǯ�� ����
        Managers.Game.MonsterAllRemove(); // �ʵ忡 �ִ� ���� ����
        Managers.UI.ShowPopupUI<UI_GameClearPopup>("UI_GameClearPopup");
    }

    public void PlayerSpwan()
    {        
        if (Managers.Game.GetPlayer() == null)
        {
            GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "Player");
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
            player.GetComponent<PlayerController>().Joystick = GameObject.FindWithTag("Joystick").GetComponent<VirtualJoystick>();
        }
        else
        {
            Managers.Game.GetPlayer().GetComponent<Transform>().position = new Vector3(0, 0, 0);
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(Managers.Game.GetPlayer());
            Managers.Game.GetPlayer().GetComponent<PlayerController>().Joystick = GameObject.FindWithTag("Joystick").GetComponent<VirtualJoystick>();
        }        
    }

    public void MakeMonsterSpawn()
    {
        GameObject go = new GameObject { name = "MonsterSpawn" };
        monsterSpawning = go.GetOrAddComponent<MonsterSpawn>();
        if (Managers.Game.CurrentChpater == 1)        
            monsterSpawning.SetKeepMonsterCount(5);
        else if (Managers.Game.CurrentChpater == 2)
            monsterSpawning.SetKeepMonsterCount(15);
        else
            monsterSpawning.SetKeepMonsterCount(20);
    }

    public override void Clear()
    {        
        DontDestroyOnLoad(Managers.Game.GetPlayer());          // ���� é�ͷ� �̵��ص� ���� �÷��̾��� ���¸� ��� ����
        Managers.Pool.Clear();                                 // ���� Ǯ�� ������Ʈ ����
        Managers.Resource.Destroy(monsterSpawning.gameObject); // ���� ���� ������Ʈ ����
        StartCoroutine("NextStageAlert");
        Managers.Game.MonsterAllRemove(); // �ʵ忡 �ִ� ���� ����
        Managers.Game.OpenDoor();         // ����é���̵�, �������� ���� �� ����α�        
    }

    IEnumerator NextStageAlert()
    {
        UI_TextPopup alert = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
        yield return null;
        alert.SetText("���� �������� �̵� �Ǵ� �������� �̵��ϱ� ���ؼ� �� ���� ���� �̿��ϼ���");
    }

    void MakeBossMonster()
    {                
        bossMonster = Managers.Game.BossMonsterSpawn("Monster/Boss");
    }
}
