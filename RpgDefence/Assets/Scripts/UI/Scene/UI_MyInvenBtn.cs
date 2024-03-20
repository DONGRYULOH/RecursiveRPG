using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MyInvenBtn : UI_Scene
{
    public static bool myInvenOpenCheck = false;

    enum Buttons
    {
        BtnMyInven
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init(); // 부모의 Init() 메소드 실행해서 Canvas Sort 작업 수행

        Bind<Button>(typeof(Buttons));
        GameObject pb = Get<Button>((int)Buttons.BtnMyInven).gameObject;
        BindEvent(pb, BtnOnClicked, Defines.UIEvent.Click);
    }

    public void BtnOnClicked(PointerEventData eventData)
    {
        // 임시 : 인벤토리 생성        
        if (!myInvenOpenCheck)
        {            
            Managers.UI.ShowPopupUI<UI_Inven>("UI_Inven");
            myInvenOpenCheck = true;
        }        
    }

}
