using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 공통으로 사용하는 텍스트 팝업창

public class UI_TextPopup : UI_Popup
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

        Bind<Text>(typeof(Texts));
        TextPopup();

        Bind<Button>(typeof(Buttons));
        BtnClickConfirmMapping();
    }

    public void TextPopup()
    {
        Defines.ItemClickCategory ItemClickCategoryText = Managers.Game.ItemClickCategory;

        if (ItemClickCategoryText == Defines.ItemClickCategory.BuyStore || ItemClickCategoryText == Defines.ItemClickCategory.EquipmentRelease)
        {
            Get<Text>((int)Texts.TextPopup).text = Text;
        }
    }

    public void SetText(string text)
    {
        Get<Text>((int)Texts.TextPopup).text = text;
    }

    public void BtnClickConfirmMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.BtnPopup).gameObject;
        BindEvent(pb, BtnOnClickConfirm, Defines.UIEvent.Click);
    }

    public void BtnOnClickConfirm(UnityEngine.EventSystems.PointerEventData data)
    {        
        Managers.UI.CloseParentPopupUI();
    }
}
