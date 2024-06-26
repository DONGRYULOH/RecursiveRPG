using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Defines.CameraMode mode = Defines.CameraMode.QuaterView;

    [SerializeField]
    Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f); // 카메라가 플레이어로부터 얼만큼 떨어져 있는지

    [SerializeField]
    GameObject player;

    void LateUpdate()
    {
        // 플레이어가 이동할때마다 덜덜거리는 현상 발생
        // 캐릭터의 이동을 담당하는 update가 먼저 실행될거라는 보장이 없음 카메라 위치 변경 update 먼저 발생하고 캐릭터 이동 update가 발생하면 카메라는 이동을 했는데 캐릭터는 멈춰있거나 하는 현상이 발생함
        // 해결방법)
        // 카메라 위치 변경되는 기능을 LateUpdate()에 구현한다 (LateUpdate는 Update 이후에 실행되기 때문에 캐릭터가 먼저 이동하고 그 이동한 지점을 카메라로 쏴준다)

        if (mode == Defines.CameraMode.QuaterView)
        {
            // 풀링 대상의 객체인 경우 메모리에서 제거하지 않고 inactive 상태처리로 바꾸기 때문에 null 체크로는 판별 불가능
            if (player == null || !player.activeSelf) return;
            
            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Block")))
            {
                // 벽으로 가려져 있는 경우 카메라를 캐릭터 위치로 이동 ** 
                float dist = (hit.point - player.transform.position).magnitude;
                transform.position = player.transform.position + delta.normalized * dist; 
            }
            else            
            {
                // 플레이어가 이동하는 위치에 맞춰서 카메라 위치 변경
                transform.position = player.transform.position + delta;
                transform.LookAt(player.transform);
            }
        }
        
    }

    public void SetQuaterView(Vector3 delta)
    {
        mode = Defines.CameraMode.QuaterView;
        this.delta = delta;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
