using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<KeyCode> keyAction = null;
    public Action<Defines.MouseEvent> mouseAction = null;

    bool pressedCk = false;    


    public void KeyActionCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {            
            keyAction.Invoke(KeyCode.Q);
        }
    }

    public void MouseActionCheck()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // ���콺�� �Է��� �޴� ���
        if(mouseAction != null)
        {
            // ���콺 ��ư�� Ŭ�������� �� ���� �߻�
            if (Input.GetMouseButtonDown(0))
            {
                // ����é�ͷ� �̵� or �������� �̵��ϴ� ���� Ŭ���� ���
                if (!CursorController.chapterOrStoreClick && (CursorController._cursorType == CursorController.CursorType.NextChapter || CursorController._cursorType == CursorController.CursorType.Store))
                {                    
                    CursorController.chapterOrStoreClick = true;
                    Managers.UI.ShowPopupUI<UI_Choice>();
                    return;
                }
            }

            // ���콺 ��ư�� Ŭ���Ǵ� ���� ��� �߻�(0�� ���� ���콺 �߻��� ���ؼ� üũ)
            if (Input.GetMouseButton(0))
            {                
                if (!pressedCk)
                {                                        
                    mouseAction.Invoke(Defines.MouseEvent.PointerDown);                
                }
                mouseAction.Invoke(Defines.MouseEvent.Press); 
                pressedCk = true;
            }
            else
            {
                pressedCk = false;                
            }
        }
    }
} 
