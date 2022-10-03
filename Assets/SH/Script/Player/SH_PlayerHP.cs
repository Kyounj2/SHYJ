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
    Animator anim;

    public GameObject normalUI;
    Slider myHPSlider;
    Text txtCurHP;
    Text txtMaxHP;

    Image[] iconArray = new Image[4];
    public Sprite[] CharacterImg = new Sprite[4];
    public Sprite crossIcon;
    public Sprite chairIcon;
    public Sprite skeletonIcon;

    public float HP
    {
        get { return hp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = defaultHp;
        fsm = GetComponent<SH_PlayerFSM>();
        anim = GetComponentInChildren<Animator>();

        Transform myThumb = normalUI.transform.Find("MyTumbnail");
        myHPSlider = myThumb.Find("SliderHP").gameObject.GetComponent<Slider>();
        txtCurHP = myThumb.Find("currentHP").gameObject.GetComponent<Text>();
        txtMaxHP = myThumb.Find("maxHP").gameObject.GetComponent<Text>();

        iconArray[0] = myThumb.gameObject.GetComponent<Image>();
        iconArray[1] = normalUI.transform.Find("OtherTumbnail1").GetComponent<Image>();
        iconArray[2] = normalUI.transform.Find("OtherTumbnail2").GetComponent<Image>();
        iconArray[3] = normalUI.transform.Find("OtherTumbnail3").GetComponent<Image>();

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

        if (photonView.IsMine)
            SetStateIconUI();
    }

    private void SetIconUI()
    {
        int idx = 1;
        for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
        {
            string num = GameManager.instance.usersData.users[i].character.Substring(9);
            int characterNum = int.Parse(num) - 1;

            if (GameManager.instance.userInfo.order == i)
            {
                iconArray[0].sprite = CharacterImg[characterNum];
            }
            else
            {
                iconArray[idx].sprite = CharacterImg[characterNum];
                string playerNickName = GameManager.instance.usersData.users[i].nick_name;
                iconArray[idx].transform.GetChild(1).GetComponent<Text>().text = playerNickName;
                idx++;
            }
        }
    }

    public void SetStateIconUI()
    {
        int idx = 1;
        for (int i = 1; i <= PhotonNetwork.CurrentRoom.PlayerCount - 1; i++)
        {
            bool isGroggy = GameManager.instance.usersData.users[i].is_groggy;
            bool isSeated = GameManager.instance.usersData.users[i].is_seated;
            bool isDead = !GameManager.instance.usersData.users[i].is_alive;

            if (GameManager.instance.userInfo.order == i)
            {
                Image stateIcon = iconArray[0].transform.GetChild(0).GetComponent<Image>();
                if (isGroggy)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = crossIcon;
                }
                else if (isSeated)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = chairIcon;
                }
                else if (isDead)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = skeletonIcon;
                }
                else
                {
                    stateIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                Image stateIcon = iconArray[idx].transform.GetChild(0).GetComponent<Image>();
                if (isGroggy)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = crossIcon;
                }
                else if (isSeated)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = chairIcon;
                }
                else if (isDead)
                {
                    stateIcon.gameObject.SetActive(true);
                    stateIcon.sprite = skeletonIcon;
                }
                else
                {
                    stateIcon.gameObject.SetActive(false);
                }
                idx++;
            }
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
        if (fsm.state == SH_PlayerFSM.State.Seated || fsm.state == SH_PlayerFSM.State.Die)
            return;

        photonView.RPC("RpcOnDamaged", RpcTarget.All, amount);
    }

    [PunRPC]
    public void RpcOnDamaged(int amount)
    {
        hp -= amount;

        hp = Mathf.Clamp(hp, 0, 100);
        print("OnDamaged : " + hp);

        if (hp <= 0)
        {
            fsm.RpcOnChangeState(SH_PlayerFSM.State.Groggy);
        }
        else
        {
            anim.SetTrigger("Hitted");
        }

        SetUI(hp);
    }

    void SetUI(float hp)
    {
        txtCurHP.text = hp.ToString();
        myHPSlider.value = hp;
    }

    const float DEADLINE = 30.0f;
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
