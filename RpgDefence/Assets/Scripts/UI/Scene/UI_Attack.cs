using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Attack : UI_Scene
{
    enum gameObjects
    {
        Attack
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {        
        Bind<GameObject>(typeof(gameObjects));
        GameObject pb = Get<GameObject>((int)gameObjects.Attack).gameObject;
        BindEvent(pb, BtnOnClicked, Defines.UIEvent.Click);
    }

    public void BtnOnClicked(PointerEventData eventData)
    {        
        Managers.Game.GetPlayer().GetComponent<PlayerController>().PlayerState = Defines.State.Attack;
    }
}
