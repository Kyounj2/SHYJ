using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// FŰ�� ������ �������� ä���ʹ�
public class YJ_Propmachines : MonoBehaviourPun
{
    // ��ü �ӽ� ������
    public GameObject originGage;
    Slider originGageSlider;

    // �÷��̾� �ӽ� ������
    public GameObject maghineGage;

    // �÷��̾�� �ӽ��۵�
    Slider playerSlider;
    bool macineOn_e = false;
    bool macineOn_p = false;
    bool enemy = false;
    bool player = false;

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
        originGageSlider = originGage.GetComponent<Slider>();
    }

    
    void Update()
    {
        // �ӽŰ������� �����ְ� �÷��̾ F�� ��������
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
                // Rpc�� ���ΰ����� �Ǻ�����
                originGage.transform.GetComponent<PhotonView>().RPC("SliderValue", RpcTarget.All, 0.1f * Time.deltaTime);
                //photonView.RPC("RpcEnemyInputF", RpcTarget.All);
            }
        }

        // ���������� �����ְ� �ֳʹ̰� F�� ��������
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
                //playerSlider.value -= 0.3f; // rpc�� ��� ��ü�� ���
                originGage.transform.GetComponent<PhotonView>().RPC("SliderValue", RpcTarget.All, -0.3f);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ������
        if (stream.IsWriting) // ���� �����͸� ���� �� �ִ� ������ ��� (ismine)
        {
            // positon, rotation
            stream.SendNext(playerSlider.value);
        }
        // ������ �ޱ�
        //else // if(stream.IsReading)
        //{
        //    playerSlider.value = (float)stream.ReceiveNext();
        //}
    }

    GameObject enemyObject;
    GameObject playerObject;

    private void OnTriggerEnter(Collider other)
    {


        // �÷��̾���
        if (other.gameObject.layer == 29)
        {
            playerObject = other.gameObject;
            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = true;
            player = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
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
        if (other.gameObject.layer == 29)
        {
            other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine = false;
            macineOn_p = other.gameObject.GetComponent<SH_PlayerSkill>().isNearPropMachine;
            player = false;
            macineOn_p = false;
        }

        // �ֳʹ̶��
        if (other.gameObject.layer == 30)
        {
            other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine = false;
            macineOn_e = other.gameObject.GetComponent<YJ_KillerMove>().isNearPropMachine;
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
