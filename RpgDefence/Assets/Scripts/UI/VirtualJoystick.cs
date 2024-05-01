using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector3 inputDirection;
    public Vector3 InputDirection { get { return inputDirection; } }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {        
        if (Managers.Game.GetPlayer().GetComponent<PlayerController>().StopAttack)
        {
            Managers.Game.GetPlayer().GetComponent<PlayerController>().PlayerState = Defines.State.Moving;
            ControlJoystickLever(eventData);
        }
    }

    // 클릭해서 드래그 하는 중에 발생하는 이벤트    
    public void OnDrag(PointerEventData eventData)
    {
        if (Managers.Game.GetPlayer().GetComponent<PlayerController>().StopAttack)
            ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {            
        Managers.Game.GetPlayer().GetComponent<PlayerController>().PlayerState = Defines.State.Wait;
        lever.anchoredPosition = Vector2.zero; // 조이스틱의 중심부로 레버를 원위치                
    }

    void ControlJoystickLever(PointerEventData eventData)
    {        
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;        
        Vector2 inputVector = inputPos.magnitude <= leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector.normalized;
    }

}
