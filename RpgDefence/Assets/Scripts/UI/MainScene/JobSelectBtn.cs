using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSelectBtn : MonoBehaviour
{    
    public void JobClickEvent()
    {
        UI_JobSelect obj = Managers.UI.ShowPopupUI<UI_JobSelect>("UI_Job"); // "UI_JOB" ¼±ÅÃÃ¢ ÆË¾÷À» ¶ç¿ò
        if(gameObject.name == "Warrior")
        {
            obj.SelectedJob = Defines.PlayerJob.Warrior;
        }
        else if (gameObject.name == "Thief")
        {
            obj.SelectedJob = Defines.PlayerJob.Thief;
        }
    }
}
