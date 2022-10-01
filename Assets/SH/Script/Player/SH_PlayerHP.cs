using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class SH_PlayerHP : MonoBehaviourPun
{
    float hp;
    float maxHP;
    const float defaultHp = 100;

    SH_PlayerFSM fsm;

    public GameObject normalUI;
    Slider myHPSlider;
    Text txtCurHP;
    Text txtMaxHP;

    Image icon0;
    Image icon1;
    Image icon2;
    Image icon3;
    Image[] iconArray = new Image[4];

    public Sprite character1;
    public Sprite character2;
    public Sprite character3;
    public Sprite character4;

    public float HP
    {
        get { return hp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = defaultHp;
        fsm = GetComponent<SH_PlayerFSM>();

        Transform myThumb = normalUI.transform.Find("MyTumbnail");
        myHPSlider = myThumb.Find("SliderHP").gameObject.GetComponent<Slider>();
        txtCurHP = myThumb.Find("currentHP").gameObject.GetComponent<Text>();
        txtMaxHP = myThumb.Find("maxHP").gameObject.GetComponent<Text>();

        icon0 = myThumb.gameObject.GetComponent<Image>();
        icon1 = normalUI.transform.Find("OtherTumbnail1").GetComponent<Image>();
        icon2 = normalUI.transform.Find("OtherTumbnail2").GetComponent<Image>();
        icon3 = normalUI.transform.Find("OtherTumbnail3").GetComponent<Image>();

        //iconArray = { icon0, icon1, icon2, icon3 };

        SetUI(100);
        SetIconUI();

        if (photonView.IsMine)
            normalUI.SetActive(true);
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

    private void SetIconUI()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
        {

        }
    }

    public void SetMaxHP(int value)
    {
        maxHP = value;
        txtMaxHP.text = maxHP.ToString();
    }

    public void OnHealed(int amount)
    {
        photonView.RPC("RpcOnHealed", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RpcOnHealed(int amount)
    {
        hp += amount;
        print("OnHealed : " + hp);

        hp = Mathf.Clamp(hp, 0, 100);
        SetUI(hp);
    }

    public void OnDamaged(int amount)
    {
        photonView.RPC("RpcOnDamaged", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RpcOnDamaged(int amount)
    {
        amount = 1;
        hp -= amount;

        hp = Mathf.Clamp(hp, 0, 100);
        print("OnDamaged : " + hp);

        if (hp <= 0)
        {
            fsm.RpcOnChangeState(SH_PlayerFSM.State.Groggy);
        }
        SetUI(hp);
    }

    void SetUI(float hp)
    {
        txtCurHP.text = hp.ToString();
        myHPSlider.value = hp;
    }

    const float DEADLINE = 100.0f;
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
