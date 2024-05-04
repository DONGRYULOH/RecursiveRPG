using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateMachineBehaviour
{
    // �Ȱ��� �ִϸ��̼��� ��� �ݺ������� ȣ��Ǵ� ���� ������� ����
    // A �ִϸ��̼� ������ -> B �ִϸ��̼����� ��ȯ�Ǵ� ��츸 �����
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
