using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartBtn : MonoBehaviour
{
    UI_TextPopup text;

    public void GameStart()
    {
        if(MainScene.playerJob == Defines.PlayerJob.Unknown)
        {
            if (Managers.UI.CheckPopupUI(text)) return;
            text = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
            text.Text = "������ �������ּ���!";
        }
        else
        {
            Managers.UI.Clear();
            SceneManager.LoadScene("ChapterOne");
        }        
    }
}
