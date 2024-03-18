using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene
    {
        get
        {
            // BaseScene 타입과 일치하는 첫 번째로 로드된 객체(활성화 상태만)를 리턴 
            // BaseScene을 부모로 가지고 있는 객체를 반환 
            // ex) 첫번째로 로드된 씬이 Login이라면 LoginScene 객체를 반환
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    public void LoadScene(Defines.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Defines.Scene type)
    {
        string name = System.Enum.GetName(typeof(Defines.Scene), type);     
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
