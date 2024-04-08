using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JobSelect : UI_Popup
{
    Defines.PlayerJob selectedJob;
    public Defines.PlayerJob SelectedJob
    {
        set { selectedJob = value; }
    }
    
    enum Buttons
    {
        // ����Ƽ ���� �ִ� Button��� �����ϰ� ����
        Yes,
        No
    }
    
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();        

        Bind<Button>(typeof(Buttons));
        BtnClickYesMapping();
        BtnClickNoMapping();
    }

    public void BtnClickYesMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.Yes).gameObject;
        BindEvent(pb, BtnOnClickedYes, Defines.UIEvent.Click);
    }

    public void BtnClickNoMapping()
    {
        GameObject pb = Get<Button>((int)Buttons.No).gameObject;
        BindEvent(pb, BtnOnClickedNo, Defines.UIEvent.Click);
    }

    public void BtnOnClickedYes(PointerEventData data)
    {        
        // ������ ���� ǥ��(** 3���� ���� �̻��� ��� ����! **)
        if(selectedJob == Defines.PlayerJob.Warrior)
        {
            GameObject.FindWithTag("Thief").GetComponent<Image>().color = new Color32(219, 219, 219, 200);
            GameObject.FindWithTag("Warrior").GetComponent<Image>().color = new Color32(155, 177, 255, 255);            
            MainScene.playerJob = Defines.PlayerJob.Warrior;
        }
        else
        {
            GameObject.FindWithTag("Warrior").GetComponent<Image>().color = new Color32(219, 219, 219, 200);
            GameObject.FindWithTag("Thief").GetComponent<Image>().color = new Color32(155, 177, 255, 255);
            MainScene.playerJob = Defines.PlayerJob.Thief;
        }
                
        Managers.UI.ClosePopupUI();
    }

    public void BtnOnClickedNo(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }


}
