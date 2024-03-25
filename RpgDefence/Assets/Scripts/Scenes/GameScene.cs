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
        

        // TODO : Ÿ�̸Ӱ� ����ǰ� ����é�ͷ� �̵��� ������ ��츸 ���â ����
        GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
        go.GetOrAddComponent<UI_MyInvenBtn>();
    }

    private void Start()
    {        
        // é�ͺ��� ���ѽð� ����
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
            seconds -= 1f; // �� �����Ӹ��� 1�ʾ� ���ҽ�Ŵ

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
            // ���ӽ���!
            MakeMonsterPooling();
        }
    }

    private void Update()
    {
        
        // 0.ī��Ʈ �ٿ� ����(0, 1, 2 start) �ڷ�ƾ���� 3�ʰ� ������ �׶����� �ð� ����


        // Managers.UI.ShowPopupUI<UI_GameStartAlert>("UI_GameStartAlert");
        // seconds += Time.deltaTime;

        // 1.���ѽð��� �������� Ȯ��
    }

    // ������(�÷��̾�, ���� ����..��� ����) ����
    public void LoadDataFile()
    {
        Managers.Data.Init();
    }

    public void PlayerSpwan()
    {
        GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        // TODO : ���ӽ��۽� ù��° �������������� �÷��̾�� �⺻ �������� �Ϻ� �־��ֱ�
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
