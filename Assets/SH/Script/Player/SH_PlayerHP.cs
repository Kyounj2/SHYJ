using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class SH_PlayerHP : MonoBehaviourPun
{
    float hp;
    float maxHP;
    const float defaultHp = 100;

    SH_PlayerFSM fsm;

    public float HP
    {
        get { return hp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = defaultHp;
        fsm = GetComponent<SH_PlayerFSM>();
    }

    void Mapping()
    {

    }

    public void SetMaxHP(int value)
    {
        maxHP = value;
    }

    public void OnHealed(int amount)
    {
        hp += amount;
        print(hp);

        hp = Mathf.Clamp(hp, 0, 100);
    }

    public void OnDamaged(int amount)
    {
        hp -= amount;

        hp = Mathf.Clamp(hp, 0, 100);
        print(hp);

        if (hp <= 0)
        {
            // 죽거나, 그로기 상태이고 싶다.
            fsm.ChangeState(SH_PlayerFSM.State.Groggy);
        }
    }

    [PunRPC]
    public void RpcOnDamaged(int amount)
    {

    }

}
