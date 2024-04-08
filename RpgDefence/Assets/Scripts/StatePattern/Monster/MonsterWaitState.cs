 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWaitState : MonoBehaviour, MonsterState
{
    private MonsterController _monsterController;

    public void Handle(MonsterController controller)
    {
        if (_monsterController == null)
            _monsterController = gameObject.GetOrAddComponent<MonsterController>();
        else
            _monsterController = controller;

                
        WaitAnimationState(GetComponent<Animator>());

        if(_monsterController.RangeCheck) RangeCheck();
    }

    // �÷��̾ ���Ͱ� �����ϴ� �ݰ�ȿ� �������� �� �÷��̾ ���� ������
    public void RangeCheck()
    {        
        GameObject player = Managers.Game.GetPlayer();
        if (player == null) return;
        float distance = (player.transform.position - _monsterController.transform.position).magnitude;     
        if (_monsterController.ScanRange >= distance)
        {
            _monsterController.LockTarget = player;
            _monsterController.State = Defines.State.Moving;            
        }           
    }

    public void WaitAnimationState(Animator anim)
    {        
        anim.Play("WAIT");
    }

}
