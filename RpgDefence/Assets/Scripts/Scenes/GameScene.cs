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

        // TODO : 타이머가 종료되고 다음챕터로 이동이 가능한 경우만 장비창 띄우기
        GameObject go = Managers.Resource.Instantiate("UI/UI_MyInvenBtn");
        go.GetOrAddComponent<UI_MyInvenBtn>();

        // 임시 : 상점에 아이템 넣어주기(TODO : 데이터 파일로 아이템 추가하기)        
        MakeStoreItem();        
    }

    // 데이터(플레이어, 몬스터 스탯..등등 매핑) 파일
    public void LoadDataFile()
    {
        Managers.Data.Init();
    }    

    public void MakeStoreItem()
    {
        // Managers.Game.CurrentItemNumberIndex 1~100 까지 상점아이템으로 전용으로 사용
        EquipmentItem equipmentItem = new EquipmentItem(Managers.Game.CurrentItemNumberIndex, "대검", 100, 0, Defines.EquipmentCategory.Weapon, 10);
        ConsumeItem consumeItem = new ConsumeItem(Managers.Game.CurrentItemNumberIndex, "파란포션", 10, 0, 10);
        Managers.Game.StoreItem.Add(equipmentItem.ItemNumber, equipmentItem);
        Managers.Game.StoreItem.Add(consumeItem.ItemNumber, consumeItem);
    }

    public void PlayerSpwan()
    {
        GameObject player = Managers.Game.Spawn(Defines.WorldObject.Player, "unitychan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        // 임시 : 게임시작시 플레이어에게 기본 아이템을 일부 넣어주기
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
