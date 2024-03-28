using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Defines.CameraMode mode = Defines.CameraMode.QuaterView;

    [SerializeField]
    Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f); // ī�޶� �÷��̾�κ��� ��ŭ ������ �ִ���

    [SerializeField]
    GameObject player;

    void LateUpdate()
    {
        // �÷��̾ �̵��Ҷ����� �����Ÿ��� ���� �߻�
        // ĳ������ �̵��� ����ϴ� update�� ���� ����ɰŶ�� ������ ���� ī�޶� ��ġ ���� update ���� �߻��ϰ� ĳ���� �̵� update�� �߻��ϸ� ī�޶�� �̵��� �ߴµ� ĳ���ʹ� �����ְų� �ϴ� ������ �߻���
        // �ذ���)
        // ī�޶� ��ġ ����Ǵ� ����� LateUpdate()�� �����Ѵ� (LateUpdate�� Update ���Ŀ� ����Ǳ� ������ ĳ���Ͱ� ���� �̵��ϰ� �� �̵��� ������ ī�޶�� ���ش�)

        if (mode == Defines.CameraMode.QuaterView)
        {
            // Ǯ�� ����� ��ü�� ��� �޸𸮿��� �������� �ʰ� inactive ����ó���� �ٲٱ� ������ null üũ�δ� �Ǻ� �Ұ���
            if (player == null || !player.activeSelf) return;
            
            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Block")))
            {
                // ������ ������ �ִ� ��� ī�޶� ĳ���� ��ġ�� �̵� ** 
                float dist = (hit.point - player.transform.position).magnitude;
                transform.position = player.transform.position + delta.normalized * dist; 
            }
            else            
            {
                // �÷��̾ �̵��ϴ� ��ġ�� ���缭 ī�޶� ��ġ ����
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
