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
        CursorController.chapterOrStoreClick = false;
        PlayerSpwan();                        
    }

    private void Start()
    {        
        // é�ͺ��� ���ѽð�, ���ھ� ���� ����
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
            MakeMonsterPooling();
            
            // ���� ���������� �������� ���� UI(���� ����)
            GameObject nextScore = Managers.Resource.Instantiate($"UI/Scene/NextChapterScore");
            if (nextScore.transform.GetChild(0) != null)
                nextScore.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Next Stage Score : " + this.nextScore;

            // �÷��̾ ȹ���� ���� UI(���� �ǽð����� ����) 
            GameObject playerScore = Managers.Resource.Instantiate($"UI/Scene/CurrentPlayerScore");
            Text playerScoreText = null;
            if (playerScore.transform.GetChild(0) != null)
            {
                playerScoreText = playerScore.transform.GetChild(0).gameObject.GetComponent<Text>();
                playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            }                            

            // Ÿ�̸� UI
            GameObject timer = Managers.Resource.Instantiate($"UI/Scene/Timer");
            Text timerText = null;
            if (timer.transform.GetChild(0) != null)
                timerText = timer.transform.GetChild(0).gameObject.GetComponent<Text>();

            while(seconds <= limitSeconds)
            {
                // ���ӵ��߿� �÷��̾ ������ Ÿ�̸� ����
                if (Managers.Game.GetPlayer() == null)                
                    yield break; // �ڷ�ƾ �Լ� ����                
                else
                {
                    seconds += Time.deltaTime;
                    playerScoreText.text = "Player Score : " + Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
                    string minutes = Mathf.Floor(seconds / 60).ToString("00");
                    string secondsTime = (seconds % 60).ToString("00");
                    timerText.text = "����� �ð� " + string.Format("{0}:{1}", minutes, secondsTime) + " (���ѽð� : " + limitSeconds + " ��)";
                    yield return null; // *** 1�������� 1�ʰ� �ƴ϶� ��û ª�� �ð� 0.0001�� ������ �� ���� ��ǻ�� ���ɿ� ���� �ٸ�
                }                
            }

            // Debug.Log(seconds); // 60.00827
            int finalPlayerScore = Managers.Game.GetPlayer().GetComponent<PlayerStat>().Score;
            if (finalPlayerScore >= this.nextScore)            
                Clear();            
            else
            {
                Managers.Resource.Destroy(Managers.GetInstance.gameObject);
                Managers.Scene.LoadScene(Defines.Scene.Main);                
            }
        }
    }

    IEnumerator NextStageAlert()
    {
        UI_TextPopup alert = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");        
        yield return null;
        alert.SetText("���� �������� �̵� �Ǵ� ���� �̵��ϱ� ���ؼ� �� ���� ���� �̿��ϼ���");
    }

    public void PlayerSpwan()
    {
        // ���� ���۽ÿ��� �÷��̾�� �⺻ �������� �־��ֱ�
        if (Managers.Game.GetPlayer() == null)
        {
            GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
            Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);            
            EquipmentItem equipmentItem = new EquipmentItem(101, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon, 100);
            ConsumeItem consumeItem = new ConsumeItem(102, "HpPortion", 100, 0, 20);
            PlayerStat stat = player.GetOrAddComponent<PlayerStat>();
            stat.Item.Add(equipmentItem.ItemNumber, equipmentItem);
            stat.Item.Add(consumeItem.ItemNumber, consumeItem);
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
        DontDestroyOnLoad(Managers.Game.GetPlayer());              // ���� é�ͷ� �̵��ص� ���� �÷��̾��� ���¸� ��� ����
        Managers.Resource.Destroy(monsterSpawningPool.gameObject); // ���� Ǯ�� ����
        StartCoroutine("NextStageAlert");
        Managers.Game.MonsterAllRemove(); // �ʵ忡 �ִ� ���� ����
        Managers.Game.OpenDoor();         // ����é���̵�, �������� ���� �� ����α�        
    }

}
