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

        // 마우스로 입력을 받는 경우
        if(mouseAction != null)
        {
            // 마우스 버튼을 클릭했을때 한 번만 발생
            if (Input.GetMouseButtonDown(0))
            {
                // 다음챕터로 이동 or 상점으로 이동하는 문을 클릭한 경우
                if (!CursorController.chapterOrStoreClick && (CursorController._cursorType == CursorController.CursorType.NextChapter || CursorController._cursorType == CursorController.CursorType.Store))
                {                    
                    CursorController.chapterOrStoreClick = true;
                    Managers.UI.ShowPopupUI<UI_Choice>();
                    return;
                }
            }

            // 마우스 버튼이 클릭되는 동안 계속 발생(0은 왼쪽 마우스 발생에 한해서 체크)
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
