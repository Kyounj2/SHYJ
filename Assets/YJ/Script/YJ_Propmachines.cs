using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// F키를 누르면 게이지를 채우고싶다
public class YJ_Propmachines : MonoBehaviourPun
{
    // 전체 머신 게이지
    public GameObject originGage;
    Slider originGageSlider;

    // 플레이어 머신 게이지
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

    // gage 다 찼을때 더이상 가동되지 않게할 bool값
    bool end = false;

    public Animation anim;

    void Start()
    {
        // 플레이거 가동용 게이지
        //maghineGage.SetActive(false);
        playerSlider = maghineGage.GetComponent<Slider>();
        enemySlider = hitGage.GetComponent<Slider>();
        originGageSlider = originGage.GetComponent<Slider>();

        // 애니메이션
        anim = GetComponent<Animation>();
    }

    //[PunRPC]
    //public void SliderValue(float i)
    //{
    //    YJ_MachineTopGage m = originGage.transform.GetComponent<YJ_MachineTopGage>();
    //    m.SliderValue2(i);
    //}

    void Update()
    {
        

        // 슬라이드 꽉차면 작동못하게하기
        if (originGageSlider.value >= 1)
        {
            end = true;
        }


        // 머신게이지가 켜져있고 플레이어가 F를 눌렀을때
        if (player)
        {
            macineOn_p = true;
        }
        if (macineOn_p)
        {
            playerSlider.enabled = true;

            if (Input.GetKey(KeyCode.F))
            {
                anim.Play();
                playerSlider.value += 0.1f * Time.deltaTime;
                // Rpc로 메인값변경 또보내기
                originGage.transform.GetComponent<PhotonView>().RPC("SliderValue", RpcTarget.All, 0.1f * Time.deltaTime);
                if (Input.GetKeyUp(KeyCode.F))
                    anim.Stop();
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
                enemySlider.value += 0.2f * Time.deltaTime;
                //photonView.RPC("RpcEnemyInputF", RpcTarget.All);
            }

            if (enemySlider.value > 0.99)
            {
                //playerSlider.value -= 0.3f; // rpc로 기계 자체를 깎기
                originGage.transform.GetComponent<PhotonView>().RPC("SliderValue", RpcTarget.All, -0.3f);
                enemySlider.value = 1f;
                if (enemySlider.value >= 1f)
                {
                    enemyObject.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
                    enemy = false;
                    macineOn_e = false;
                }

            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기
        if (stream.IsWriting) // 내가 데이터를 보낼 수 있는 상태인 경우 (ismine)
        {
            // positon, rotation
            stream.SendNext(playerSlider.value);
        }
        // 데이터 받기
        //else // if(stream.IsReading)
        //{
        //    playerSlider.value = (float)stream.ReceiveNext();
        //}
    }

    GameObject enemyObject;
    GameObject playerObject;

    private void OnTriggerEnter(Collider other)
    {


        // 플레이어라면
        if (other.gameObject.layer == 29)
        {
            if (end) return;

            playerObject = other.gameObject;
            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = true;
            player = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
            maghineGage.GetComponent<Slider>().value = originGageSlider.GetComponent<Slider>().value;
        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {
            if (end) return;

            enemyObject = other.gameObject;
            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = true;
            enemy = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //// 플레이어라면
        if (other.gameObject.layer == 29)
        {
            playerSlider.value = 0;
            //if (end) return;

            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = false;
            //macineOn_p = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
            player = false;
            macineOn_p = false;

        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {
            //if (end) return;

            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
            //macineOn_e = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
            enemy = false;
            macineOn_e = false;
        }

    }
}
