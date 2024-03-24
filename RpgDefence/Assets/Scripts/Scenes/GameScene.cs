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

        // �ӽ� : ������ ������ �־��ֱ�(TODO : ������ ���Ϸ� ������ �߰��ϱ�)        
        MakeStoreItem();        
    }

    // ������(�÷��̾�, ���� ����..��� ����) ����
    public void LoadDataFile()
    {
        Managers.Data.Init();
    }    

    public void MakeStoreItem()
    {
        // Managers.Game.CurrentItemNumberIndex 1~100 ���� �������������� �������� ���
        EquipmentItem equipmentItem = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "���", 100, 0, Defines.EquipmentCategory.Weapon, 10);
        ConsumeItem consumeItem = new ConsumeItem(Managers.Game.CurrentItemNumberIndex, "�Ķ�����", 10, 0, 10);
        Managers.Game.StoreItem.Add(equipmentItem.ItemNumber, equipmentItem);
        Managers.Game.StoreItem.Add(consumeItem.ItemNumber, consumeItem);
    }

    public void PlayerSpwan()
    {
        GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        // �ӽ� : ���ӽ��۽� �÷��̾�� �⺻ �������� �Ϻ� �־��ֱ�
        EquipmentItem equipmentItem = new EquipmentItem(101, "LongSword", 100, 0, Defines.EquipmentCategory.Weapon, 100);
        ConsumeItem consumeItem = new ConsumeItem(102, "HpPortion", 10, 0, 20);
        PlayerStat stat = player.GetComponent<PlayerStat>();
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
