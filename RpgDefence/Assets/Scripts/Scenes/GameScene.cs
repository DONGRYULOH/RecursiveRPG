using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Defines.Scene.Game;
        Managers.Game.CurrentChpater = 1;
        gameObject.GetOrAddComponent<CursorController>();

        LoadDataFile();      
        PlayerSpwan();

        // TODO : Ÿ�̸Ӱ� ����ǰ� ����é�ͷ� �̵��� ������ ��츸 ���â ����
        GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
        go.GetOrAddComponent<UI_MyInvenBtn>();
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

        // �ӽ� : ���ӽ��۽� �÷��̾�� �⺻ �������� �Ϻ� �־��ֱ�
        EquipmentItem equipmentItem = new EquipmentItem("LongSword", 100, 0, Defines.EquipmentCategory.Weapon);
        ConsumeItem consumeItem = new ConsumeItem("HpPortion", 10, 0);
        PlayerStat stat = player.GetComponent<PlayerStat>();
        stat.Item.Add("LongSword", equipmentItem);
        stat.Item.Add("HpPortion", consumeItem);
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
