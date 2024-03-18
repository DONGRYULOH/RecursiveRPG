using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Defines.Scene.Login;                
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Defines.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("Login Scene Clear!");
    }
}
