using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Choice : UI_Popup
{
    enum Buttons 
    {
        // 유니티 툴에 있는 Button명과 동일하게 매핑
        Yes,
        No
    }

    enum Texts
    {
        NextOrStoreText
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        ModifyChoiceText();

        Bind<Button>(typeof(Buttons));
        BtnClickYesMapping();
        BtnClickNoMapping();
    }

    public void ModifyChoiceText()
    {
        if (CursorController._cursorType == CursorController.CursorType.Store)
            Get<Text>((int)Texts.NextOrStoreText).text = "상점으로 이동하시겠습니까?";
        else
            Get<Text>((int)Texts.NextOrStoreText).text = "다음 챕터로 이동하시겠습니까?";
    }

    public void BtnClickYesMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.Yes).gameObject;
        BindEvent(pb, BtnOnClickedYes, Defines.UIEvent.Click);
    }

    public void BtnClickNoMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.No).gameObject;
        BindEvent(pb, BtnOnClickedNo, Defines.UIEvent.Click);
    }

    public void BtnOnClickedYes(PointerEventData data)
    {
        // 다음 챕터로 이동 
        if(CursorController._cursorType == CursorController.CursorType.NextChapter)
        {
            Managers.Game.MoveNextChpater();
        }
        else
        {
            Managers.UI.ClosePopupUI(); // 상점으로 이동하시겠습니까? 팝업 닫음
            Managers.Game.MoveStore(); // 상점으로 이동(팝업)
        }
    }

    public void BtnOnClickedNo(PointerEventData data)
    {        
        Managers.UI.ClosePopupUI(gameObject.GetComponent<UI_Choice>());        
    }
}
