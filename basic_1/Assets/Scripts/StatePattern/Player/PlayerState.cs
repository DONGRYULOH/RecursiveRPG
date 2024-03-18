using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerState
{
    void Handle(PlayerController controller);
}
