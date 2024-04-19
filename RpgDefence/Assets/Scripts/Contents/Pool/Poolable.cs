using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀링할 대상(이 컴포넌트를 가지고 있으면 메모리 풀링 대상으로 인식)
public class Poolable : MonoBehaviour
{    
    public void InitBeforeInactive()
    {
        // 풀링 대상인 몬스터인 경우 초기화(왜냐하면 다시 스폰이 될때 죽어있는 상태가 되어 있으면 안되므로)
        // 풀링 대상 몬스터 죽음 -> 해당 몬스터를 풀로 이동 -> 풀에서 해당 몬스터를 꺼내옴 -> 몬스터 활성화 처리 -> 몬스터 정보 초기화
        gameObject.transform.position = new Vector3(0, 0, -15); // 스폰 위치
        gameObject.GetComponent<MonsterController>().State = Defines.State.Wait; // 상태
        gameObject.GetComponent<MonsterController>().Stat.SetStat(Managers.Game.CurrentChpater); // 스탯
    }
}
