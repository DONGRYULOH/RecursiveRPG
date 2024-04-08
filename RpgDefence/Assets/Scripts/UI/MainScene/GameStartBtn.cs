using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartBtn : MonoBehaviour
{    
    public void GameStart()
    {
        if(MainScene.playerJob == Defines.PlayerJob.Unknown)
        {
            UI_TextPopup text = Managers.UI.ShowPopupUI<UI_TextPopup>("UI_TextPopup");
            text.Text = "������ �������ּ���!";
        }
        else
        {
            SceneManager.LoadScene("ChapterOne");
        }        
    }
}
