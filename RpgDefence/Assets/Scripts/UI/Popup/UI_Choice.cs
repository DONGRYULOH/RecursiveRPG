using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Choice : UI_Popup
{
    enum Buttons 
    {
        // ����Ƽ ���� �ִ� Button��� �����ϰ� ����
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
            Get<Text>((int)Texts.NextOrStoreText).text = "�������� �̵��Ͻðڽ��ϱ�?";
        else
            Get<Text>((int)Texts.NextOrStoreText).text = "���� é�ͷ� �̵��Ͻðڽ��ϱ�?";
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
        // ���� é�ͷ� �̵� 
        if(CursorController._cursorType == CursorController.CursorType.NextChapter)
        {
            Managers.Game.MoveNextChpater();
        }
        else
        {
            Managers.UI.ClosePopupUI(); // �������� �̵��Ͻðڽ��ϱ�? �˾� ����
            Managers.Game.MoveStore(); // �������� �̵�(�˾�)
        }
    }

    public void BtnOnClickedNo(PointerEventData data)
    {        
        Managers.UI.ClosePopupUI(gameObject.GetComponent<UI_Choice>());        
    }
}
