using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action keyAction = null;
    public Action<Defines.MouseEvent> mouseAction = null;

    bool pressedCk = false;
    float _pressedTime = 0.0f;

    public void OnUpdate()
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
                // ���ʿ� Ŭ���� �ϴ� ���� ���콺 Ŭ���� ��������� �ð��� üũ
                if(!pressedCk)
                {                                        
                    mouseAction.Invoke(Defines.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                mouseAction.Invoke(Defines.MouseEvent.Press); // mouseAction �븮�ڿ� ��ϵ��ִ� �Լ��� ���ڿ� ���� �־ Invoke�� ȣ���Ŵ
                pressedCk = true;
            }
            else
            {
                if (pressedCk)
                {
                    // ���콺�� Ŭ���ϰ� �ٷ� �� ���¸� Ŭ������ �ν�
                    if(Time.time < _pressedTime + 0.2f)
                        mouseAction.Invoke(Defines.MouseEvent.Click);

                    mouseAction.Invoke(Defines.MouseEvent.PointerUp);
                }
                pressedCk = false;
                _pressedTime = 0.0f;
            }
        }
    }

    public void Clear()
    {
        keyAction = null;
        mouseAction = null;
    }
} 
