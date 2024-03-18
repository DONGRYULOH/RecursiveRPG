using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
  
    public override void Init() {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        // 여기서 말하는 this가 뭐지??
        Managers.UI.ClosePopupUI(this);
    }

}
