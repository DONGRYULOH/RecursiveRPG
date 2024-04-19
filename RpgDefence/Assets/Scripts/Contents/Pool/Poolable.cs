using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ǯ���� ���(�� ������Ʈ�� ������ ������ �޸� Ǯ�� ������� �ν�)
public class Poolable : MonoBehaviour
{    
    public void InitBeforeInactive()
    {
        // Ǯ�� ����� ������ ��� �ʱ�ȭ(�ֳ��ϸ� �ٽ� ������ �ɶ� �׾��ִ� ���°� �Ǿ� ������ �ȵǹǷ�)
        // Ǯ�� ��� ���� ���� -> �ش� ���͸� Ǯ�� �̵� -> Ǯ���� �ش� ���͸� ������ -> ���� Ȱ��ȭ ó�� -> ���� ���� �ʱ�ȭ
        gameObject.transform.position = new Vector3(0, 0, -15); // ���� ��ġ
        gameObject.GetComponent<MonsterController>().State = Defines.State.Wait; // ����
        gameObject.GetComponent<MonsterController>().Stat.SetStat(Managers.Game.CurrentChpater); // ����
    }
}
