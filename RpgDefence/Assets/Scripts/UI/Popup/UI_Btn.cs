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
        Managers.Resource.Destroy(Managers.GetInstance.gameObject);
        Managers.Scene.LoadScene(Defines.Scene.Main);
    }
}
