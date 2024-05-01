using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSelectBtn : MonoBehaviour
{
    // ���� �̵��Ǹ� static���� ������ �����ʹ� ���α׷��� ����Ǳ� ������ ��� �����ֱ� ������ ��ȸ������ ����ϴ� static ������ �����ؾ� ��
    static public UI_JobSelect current; 

    // ���� ����â �˾��� ���
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
