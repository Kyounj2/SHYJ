using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerHp : MonoBehaviour
{
    float hp;
    const float defaultHp = 100;

    public float HP
    {
        get { return hp; }

        set
        {
            hp = value;

            if (hp <= 0)
            {
                // 죽거나, 그로기 상태이고 싶다.
            }
        }
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
}
