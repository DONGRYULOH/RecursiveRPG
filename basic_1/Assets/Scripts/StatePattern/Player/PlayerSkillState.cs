using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : MonoBehaviour, PlayerState
{
    private PlayerController _playerController;
    
    public void RockOnOrDeFaultEvent()
    {
        // 락온인 경우 해당 해당 대상을 바라보게 처리
        if (_playerController.LockTarget != null)
        {
            Vector3 dir = _playerController.LockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

        SkillAnimationState(GetComponent<Animator>());
    }

    // 애니메이션에 추가한 이벤트 발동
    void OnHitEvent()
    {        
        if(_playerController.LockTarget != null)
        {
            // Q) 내가 상대방을 공격했을때 내가 직접 상대방의 hp를 가져와서 줄여줄건지? 아니면 상대방쪽에서 hp를 스스로 깎게끔 처리할것인지? 
            // 결론 : 피해를 받는 상대방쪽에서 스스로 hp를 깎는게 좋을 수도 있다고 생각함 
            // 왜냐하면 상대방의 방어력, 공격력 무시 ... 등으로 인해 hp가 깎이는 비율이 줄어들수 있기 때문에 상대방쪽에서 코드를 수정하는게 편함
            Stat targetStat = _playerController.LockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_playerController.Stat);            
        }

        if (_playerController.StopSkill)
        {            
            _playerController.Wait();
        }
        else
        {
            _playerController.PlayerState = Defines.State.Skill;
        }
    }

    public void SkillAnimationState(Animator anim)
    {
        _playerController.PlayerState = Defines.State.Skill;
        anim.Play("ATTACK");
    }

    public void Handle(PlayerController playerController)
    {
        if (playerController == null)
            _playerController = gameObject.GetOrAddComponent<PlayerController>();
        else
            _playerController = playerController;

        RockOnOrDeFaultEvent();
    }
}
