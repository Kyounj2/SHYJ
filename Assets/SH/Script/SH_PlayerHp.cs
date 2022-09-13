using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerHP : MonoBehaviour
{
    float hp;
    float maxHP;
    const float defaultHp = 100;

    public float HP
    {
        get { return hp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = defaultHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Mapping()
    {

    }

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
    }

    public void OnHealed(int amount)
    {
        hp += amount;
        print(hp);

        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }

    public void OnDamaged(int amount)
    {
        hp -= amount;
        print(hp);

        if (hp <= 0)
        {
            // 죽거나, 그로기 상태이고 싶다.
        }
    }
}
