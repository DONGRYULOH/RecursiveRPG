using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : BaseScene
{
    static public Defines.PlayerJob playerJob;

    protected override void Init()
    {
        base.Init();        
        SceneType = Defines.Scene.Main;
        playerJob = Defines.PlayerJob.Unknown;        
    }

    public override void Clear()
    {        
    }
}
