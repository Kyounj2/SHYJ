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
    bool macineOn = false;

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
        if(maghineGage.activeSelf)
        {
            macineOn = true;
        }
        if(macineOn)
        {
            playerSlider.enabled = true;
            if (Input.GetKey(KeyCode.F))
                playerSlider.value += 0.03f * Time.deltaTime;
        }

        // 힛게이지가 켜져있고 애너미가 F를 눌렀을때
        if(hitGage.activeSelf)
        {
            macineOff = true;
        }
        if(macineOff)
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
                    macineOff = false;
                    hitGage.SetActive(false);
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = true;

        //// 플레이어라면
        //if (other.gameObject.layer == 29)
        //{

        //}

        //// 애너미라면
        //if (other.gameObject.layer == 30)
        //{
        //    hitGage.SetActive(true);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;

        //// 플레이어라면
        //if (other.gameObject.layer == 29)
        //{
        //    macineOn = false;
        //    playerSlider.enabled = false;
        //    maghineGage.SetActive(false);
        //}

        //// 애너미라면
        //if (other.gameObject.layer == 30)
        //{
        //    macineOff = false;
        //    enemySlider.enabled = false;
        //    enemySlider.value = 0f;
        //    hitGage.SetActive(false);
        //}

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
