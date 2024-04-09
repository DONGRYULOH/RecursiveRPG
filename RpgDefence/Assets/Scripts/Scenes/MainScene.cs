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

        Texture2D cursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Arrow");
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 5, 0), CursorMode.Auto);
    }

    public override void Clear()
    {        
    }
}
