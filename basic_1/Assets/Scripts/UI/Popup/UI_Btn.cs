using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Btn : UI_Popup
{  
    // 해당 오브젝트에 컴포넌트로 연결을 자동화 시킬 UI 오브젝트들의 이름들
    enum Buttons
    {
        PointBtn
    }

    enum Texts
    {
        PointTxt,
        ScoreTxt
    }

    enum GameObjects
    {
        TestObject
    }

    enum Images
    {
        ItemIcon
    }
    // -----------------------------------------------------------------------

    int score;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 부모의 Init() 메소드 실행해서 Canvas Sort 작업 수행

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GameObject pb = Get<Button>((int)Buttons.PointBtn).gameObject;
        BindEvent(pb, BtnOnClicked, Defines.UIEvent.Click);

        // 익명 메소드 형태로 이벤트 핸들러를 만들어서 OnDragHandler 이벤트가 발생시 호출할 이벤트 핸들러로 등록시킴
        GameObject go = Get<Image>((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Defines.UIEvent.Drag);
    }


    public void BtnOnClicked(PointerEventData data)
    {
        score++;
        GetText((int)Texts.ScoreTxt).text = $"점수 : {score}";
    }
}
