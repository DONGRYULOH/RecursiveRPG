using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSelectBtn : MonoBehaviour
{
    // 씬이 이동되면 static으로 선언한 데이터는 프로그램이 종료되기 전까지 계속 남아있기 때문에 일회성으로 사용하는 static 변수는 지양해야 함
    static public UI_JobSelect current; 

    // 직업 선택창 팝업을 띄움
    public void JobClickEvent()
    {
        if (Managers.UI.CheckPopupUI(current)) return;
        current = Managers.UI.ShowPopupUI<UI_JobSelect>("UI_Job"); 
        if (gameObject.name == "Warrior")
        {
            current.SelectedJob = Defines.PlayerJob.Warrior;
        }
        else if (gameObject.name == "Thief")
        {
            current.SelectedJob = Defines.PlayerJob.Thief;
        }
    }
}
