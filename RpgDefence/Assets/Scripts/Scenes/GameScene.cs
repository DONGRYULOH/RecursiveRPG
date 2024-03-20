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

        // 임시 : 게임시작시 플레이어에게 기본 아이템을 일부 넣어주기
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
