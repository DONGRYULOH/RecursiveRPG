using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateMachineBehaviour
{
    // 똑같은 애니메이션이 계속 반복적으로 호출되는 경우는 실행되지 않음
    // A 애니메이션 실행후 -> B 애니메이션으로 전환되는 경우만 실행됨
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("ThiefATTACK"))
        {            
            animator.gameObject.GetComponent<PlayerController>().StopAttack = true;
        }
        else if (stateInfo.IsName("WarriorATTACK"))
        {
            GameObject go = GameObject.FindGameObjectWithTag("RightHandSocket");
            Transform childTransform = go.transform.GetComponentInChildren<Transform>().GetChild(0);
            if (childTransform != null)
                childTransform.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            animator.gameObject.GetComponent<PlayerController>().StopAttack = true;
        }        
    }
}
