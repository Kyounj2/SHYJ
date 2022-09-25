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

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                OnDamaged(1000);
            }
        }
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
        photonView.RPC("RpcOnHealed", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RpcOnHealed(int amount)
    {
        hp += amount;
        print(hp);

        hp = Mathf.Clamp(hp, 0, 100);
    }

    public void OnDamaged(int amount)
    {
        photonView.RPC("RpcOnDamaged", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RpcOnDamaged(int amount)
    {
        hp -= amount;

        hp = Mathf.Clamp(hp, 0, 100);
        print(hp);

        if (hp <= 0)
        {
            fsm.RpcOnChangeState(SH_PlayerFSM.State.Groggy);
        }
    }

    const float DEADLINE = 1000.0f;
    public float seatedTime = 0;
    public void Seated()
    {
        seatedTime += Time.deltaTime;
        //print(seatedTime);
        if (seatedTime > DEADLINE)
        {
            fsm.ChangeState(SH_PlayerFSM.State.Die);
            seatedTime = 0;
        }
    }
}
