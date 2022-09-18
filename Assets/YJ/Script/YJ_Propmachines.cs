using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// FŰ�� ������ �������� ä���ʹ�
public class YJ_Propmachines : MonoBehaviourPun
{
    // �ӽ� ��ü ������
    public GameObject maghineGage;

    // �÷��̾�� �ӽ��۵�
    Slider playerSlider;
    bool macineOn = false;
    bool enemy = false;

    // �ֳʹ����� ������
    public GameObject hitGage;

    // �ֳʹ̿� �ӽ��۵�
    Slider enemySlider;
    bool macineOff = false;

    void Start()
    {
        // �÷��̰� ������ ������
        //maghineGage.SetActive(false);
        playerSlider = maghineGage.GetComponent<Slider>();
        enemySlider = hitGage.GetComponent<Slider>();
    }

    
    void Update()
    {
        // �ӽŰ������� �����ְ� �÷��̾ F�� ��������
        //if (hitGage.activeSelf)
        //{
        //    macineOff = true;
        //}
        //if (macineOn)
        //{
        //    playerSlider.enabled = true;
        //    if (Input.GetKey(KeyCode.F))
        //        playerSlider.value += 0.03f * Time.deltaTime;
        //}

        // ���������� �����ְ� �ֳʹ̰� F�� ��������
        if (enemy)
        {
            macineOn = true;
        }
        if (macineOn)
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
                    macineOn = false;
                }

            }
        }
    }

    GameObject enemyObject;
    GameObject playerObject;

    private void OnTriggerEnter(Collider other)
    {


        // �÷��̾���
        if (other.gameObject.layer == 29)
        {
            playerObject = other.gameObject;
            //other.gameObject.GetComponent<SH_PlayerMove>(),
        }

        // �ֳʹ̶��
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

        //// �÷��̾���
        //if (other.gameObject.layer == 29)
        //{
        //    macineOn = false;
        //    playerSlider.enabled = false;
        //    maghineGage.SetActive(false);
        //}

        // �ֳʹ̶��
        if (other.gameObject.layer == 30)
        {
            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
            macineOn = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
            //macineOff = false;
            //enemySlider.enabled = false;
            //enemySlider.value = 0f;
            //hitGage.SetActive(false);
        }

    }

    // ��Ʈ��ũ
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
