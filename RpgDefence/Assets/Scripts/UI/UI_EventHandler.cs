using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    UI에 어떠한 이벤트가 발생했을 때 어떠한 로직을 수행할 핸들러 이벤트
*/
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler ,IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {        
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData); // 등록된 이벤트 핸들러 호출
    }

    public void OnDrag(PointerEventData eventData)
    {
        // transform.position = eventData.position;
        Debug.Log("드래그 중..");

        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData); // 등록된 이벤트 핸들러 호출
    }

}
