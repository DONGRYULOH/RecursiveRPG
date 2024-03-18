using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MonsterState
{
    void Handle(MonsterController controller);
}
