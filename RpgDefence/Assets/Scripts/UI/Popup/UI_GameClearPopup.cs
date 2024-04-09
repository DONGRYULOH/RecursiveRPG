using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UI_GameClearPopup : UI_Popup
{
    public string Text
    {
        get; set;
    }

    enum Texts
    {
        TextPopup
    }

    enum Buttons
    {
        BtnPopup
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();        

        Bind<Button>(typeof(Buttons));
        BtnClickConfirmMapping();
    }    

    public void BtnClickConfirmMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.BtnPopup).gameObject;
        BindEvent(pb, BtnOnClickConfirm, Defines.UIEvent.Click);
    }

    public void BtnOnClickConfirm(UnityEngine.EventSystems.PointerEventData data)
    {
        // 메인화면으로 이동(게임 재시작)
        Managers.UI.CloseAllPopupUI();
        Managers.GetInstance.GameClear();
        Managers.Scene.LoadScene(Defines.Scene.Main);
    }
}
