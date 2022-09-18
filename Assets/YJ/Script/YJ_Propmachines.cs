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

        // ���������� �����ְ� �ֳʹ̰� F�� ��������
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

        //// �÷��̾���
        //if (other.gameObject.layer == 29)
        //{

        //}

        //// �ֳʹ̶��
        //if (other.gameObject.layer == 30)
        //{
        //    hitGage.SetActive(true);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;

        //// �÷��̾���
        //if (other.gameObject.layer == 29)
        //{
        //    macineOn = false;
        //    playerSlider.enabled = false;
        //    maghineGage.SetActive(false);
        //}

        //// �ֳʹ̶��
        //if (other.gameObject.layer == 30)
        //{
        //    macineOff = false;
        //    enemySlider.enabled = false;
        //    enemySlider.value = 0f;
        //    hitGage.SetActive(false);
        //}

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
