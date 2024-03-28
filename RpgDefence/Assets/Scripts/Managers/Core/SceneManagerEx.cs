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
            // BaseScene Ÿ�԰� ��ġ�ϴ� ù ��°�� �ε�� ��ü(Ȱ��ȭ ���¸�)�� ���� 
            // BaseScene�� �θ�� ������ �ִ� ��ü�� ��ȯ 
            // ex) ù��°�� �ε�� ���� Login�̶�� LoginScene ��ü�� ��ȯ
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    public void LoadScene(Defines.Scene type)
    {        
        SceneManager.LoadScene(GetSceneName(type));
    }

    public void LoadChpater(Defines.Chapter type)
    {
        Managers.UI.Clear();
        SceneManager.LoadScene(GetChpaterName(type));
    }

    string GetSceneName(Defines.Scene type)
    {
        string name = System.Enum.GetName(typeof(Defines.Scene), type);     
        return name;
    }

    string GetChpaterName(Defines.Chapter type)
    {
        string name = System.Enum.GetName(typeof(Defines.Chapter), type);
        return name;
    }    
}