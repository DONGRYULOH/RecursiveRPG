using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum GameObjects
    {
        HpBar // HpBar라는 이름을 가지고 있는 게임오브젝트에 해당 컴포넌트(UI_HpBar)를 붙여주는것을 자동화 시키기 위한것
    }

    Stat _stat;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {        
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = gameObject.transform.parent; // "UI_HpBar" 컴포넌트가 붙어있는 오브젝트의 부모의 transform.parent; 동일한 의미
        transform.position = parent.position + (Vector3.up * (parent.GetComponent<Collider>().bounds.size.y));

        // 체력 UI와 카메라는 같은 방향을 보게함
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp; // 해당 캐릭터의 최대HP에 따른 현재 hp의 비율
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetGameObject((int)GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }
}
