using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Defines.Layer.Ground) | (1 << (int)Defines.Layer.Monster1); // Layer 마스킹 처리

    Texture2D _attackIcon, _handIcon; // 게임화면의 마우스 커서    

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }
    CursorType _cursorType = CursorType.None;
    
    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }
    
    void Update()
    {
        // 최초에 클릭한 지점에 해당하는 커서로만 표시하고 계속 마우스 버튼이 눌러지고 있는 상태라면 다른 커서로 변경 X
        // ex) 몬스터를 클릭하고 계속 누른 상태에서 커서가 다른쪽으로 이동했을때 바뀌는것을 방지
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Defines.Layer.Monster1)
            {
                // 이미 Attack 커서인 경우(몬스터 커서시에만) SetCursor 작업을 하지 않도록 설정
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
