using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AutoAttack : UI_Scene
{
    enum gameObjects
    {
        Auto
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {        
        Bind<GameObject>(typeof(gameObjects));
        GameObject pb = Get<GameObject>((int)gameObjects.Auto).gameObject;
        BindEvent(pb, BtnOnClicked, Defines.UIEvent.Click);
    }

    public void BtnOnClicked(PointerEventData eventData)
    {
        bool autoCheck = Managers.Game.GetPlayer().GetComponent<PlayerController>().IsAutoAttack;
        GameObject auto = Get<GameObject>((int)gameObjects.Auto).gameObject;
        if (!autoCheck)
        {            
            auto.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Consol/Attack/GUI/Elements/Buttons/" + "btn_big");
            // Managers.Game.GetPlayer().GetComponent<PlayerController>().IsAutoAttack = true;
        }
        else
        {
            auto.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Consol/Attack/GUI/Elements/Buttons/" + "btn_round_small");
            // Managers.Game.GetPlayer().GetComponent<PlayerController>().IsAutoAttack = false;
        }        
    }
}
