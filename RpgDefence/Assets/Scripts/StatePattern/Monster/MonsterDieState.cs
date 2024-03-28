using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : MonoBehaviour, MonsterState
{
    public void Handle(MonsterController controller)
    {        
        Managers.Game.DeSpawn(controller.gameObject);
    }
}
