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

        // 키보드로 입력을 받는 경우
        // if(Input.anyKey && keyAction != null) keyAction.Invoke();

        // 마우스로 입력을 받는 경우
        if(mouseAction != null)
        {
            // 마우스 버튼이 눌려져 있는지 아닌지 여부를 반환하는데 인자값으로 0이 들어가면 왼쪽 버튼에 한해서 체크
            if (Input.GetMouseButton(0))
            {
                // 최초에 클릭을 하는 경우는 마우스 클릭을 때기까지의 시간을 체크
                if(!pressedCk)
                {
                    mouseAction.Invoke(Defines.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                mouseAction.Invoke(Defines.MouseEvent.Press); // mouseAction 대리자에 등록되있는 함수의 인자에 값을 넣어서 Invoke를 호출시킴
                pressedCk = true;
            }
            else
            {
                if (pressedCk)
                {
                    // 마우스로 클릭하고 바로 땐 상태면 클릭으로 인식
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
