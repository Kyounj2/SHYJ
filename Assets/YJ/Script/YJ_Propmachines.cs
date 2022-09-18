using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// F키를 누르면 게이지를 채우고싶다
public class YJ_Propmachines : MonoBehaviourPun
{
    // 머신 전체 게이지
    public GameObject maghineGage;

    // 플레이어용 머신작동
    Slider playerSlider;
    bool macineOn_e = false;
    bool macineOn_p = false;
    bool enemy = false;
    bool player = false;

    // 애너미전용 게이지
    public GameObject hitGage;

    // 애너미용 머신작동
    Slider enemySlider;
    bool macineOff = false;

    void Start()
    {
        // 플레이거 가동용 게이지
        //maghineGage.SetActive(false);
        playerSlider = maghineGage.GetComponent<Slider>();
        enemySlider = hitGage.GetComponent<Slider>();
    }

    
    void Update()
    {
        // 머신게이지가 켜져있고 플레이어가 F를 눌렀을때
        if(player)
        {
            macineOn_p = true;
        }
        if(macineOn_p)
        {
            playerSlider.enabled = true;

            if (Input.GetKey(KeyCode.F))
            {
                playerSlider.value += 0.1f * Time.deltaTime;
                //photonView.RPC("RpcEnemyInputF", RpcTarget.All);
            }
        }

        // 힛게이지가 켜져있고 애너미가 F를 눌렀을때
        if (enemy)
        {
            macineOn_e = true;
        }
        if (macineOn_e)
        {
            enemySlider.enabled = true;

            if (Input.GetKey(KeyCode.F))
            {
                enemySlider.value += 0.1f * Time.deltaTime;
                //photonView.RPC("RpcEnemyInputF", RpcTarget.All);
            }

            if (enemySlider.value > 0.99)
            {
                playerSlider.value -= 0.3f;
                enemySlider.value = 1f;
                if(enemySlider.value >= 1f)
                {
                    enemyObject.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
                    enemy = false;
                    macineOn_e = false;
                }

            }
        }
    }

    GameObject enemyObject;
    GameObject playerObject;

    private void OnTriggerEnter(Collider other)
    {


        // 플레이어라면
        if (other.gameObject.layer == 29)
        {
            playerObject = other.gameObject;
            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = true;
            player = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {
            enemyObject = other.gameObject;
            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = true;
            enemy = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
            //hitGage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //// 플레이어라면
        if (other.gameObject.layer == 29)
        {
            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = false;
            macineOn_p = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
            player = false;
            macineOn_p = false;
        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {
            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
            macineOn_e = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
        }

    }

    // 네트워크
    [PunRPC]
    public void RpcPlayerInputF()
    {
        playerSlider.value += 0.03f * Time.deltaTime;
    }

    [PunRPC]
    public void RpcEnemyInputF()
    {
        enemySlider.value += 0.1f * Time.deltaTime;
    }
}
