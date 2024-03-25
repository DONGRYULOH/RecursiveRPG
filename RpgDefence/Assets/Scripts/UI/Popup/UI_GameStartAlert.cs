using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameStartAlert : UI_Popup
{
    public string Text
    {
        get; set;
    }

    enum Texts
    {
        AlertText
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));        
    }

    public void CountDownText(string text)
    {        
        Get<Text>((int)Texts.AlertText).text = text;
    }
}
