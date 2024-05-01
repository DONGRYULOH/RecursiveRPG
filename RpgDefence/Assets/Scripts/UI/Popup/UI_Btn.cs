using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Btn : UI_Popup
{      
    public void BtnOnClickedGameOver()
    {       
        // 메인화면으로 이동(게임 재시작)
        Managers.UI.CloseAllPopupUI();
        Managers.GetInstance.GameClear();
        Managers.Scene.LoadScene(Defines.Scene.Main);
    }
}
