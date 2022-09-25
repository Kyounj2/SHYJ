using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ������ Ȱ��ȭ�ɰž�

// Ȱ��ȭ�Ǹ� ����Ʈ�� ���� ����ִ� �÷��̾ ������ ��
// ����Ʈ 1�����ִ� �÷��̾�� �����Ÿ� �������� �̵�
// ���콺 �¿찪�� �޾ƿͼ� ������ �� �ְ��ϱ�

// ó���� �޾ƿ� ����ִ»���� �ο��� ���� ����ִ� ����� �ο��� �ٸ����
// ����Ʈ�� �ʱ�ȭ�ϰ�
// �ٽ� �÷��̾���� �ְ�ʹ�
public class YJ_DieCam : MonoBehaviour
{
    // �÷��̾���� ���� ����Ʈ
    [SerializeField]
    GameObject[] playerList = null;

    // ���콺 �¿찪
    float mouseX;

    // �迭 �ҷ��� ����
    int player = 0;

    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
            transform.position = playerList[player].transform.position;

            // ��ġ �ٽ� ����ֱ�
            Camera.main.transform.localPosition = new Vector3(0, 1.75f, -4f);
            lookAtPlayer = playerList[player];
        }
        //transform.parent = playerList[player].transform;
    }


    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            // ī�޶� ȸ��
            CamRot();

            // �÷��̾��� ���� ���� ���� ã�� ������ �������
            if (GameObject.FindGameObjectsWithTag("Player").Length < playerList.Length)
            {
                // �迭�� ���� �ٽ� �־��ش�
                Array.Clear(playerList, 0, playerList.Length);
                playerList = null;

                playerList = GameObject.FindGameObjectsWithTag("Player");
            }

            // ���콺 ��ư Ŭ�� �� ���� ĳ���ͷ� �̵�
            if (Input.GetMouseButtonDown(0) || !lookAtPlayer.activeSelf)
            {
                //rPlayer = player;
                NextPlayer();
            }

            if (lookAtPlayer.activeSelf)
                transform.position = playerList[player].transform.position;

        }
    }

    void CamRot()
    {
        float x = Input.GetAxis("Mouse X");

        mouseX += x * 205 * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }

    GameObject lookAtPlayer;
    // ���콺 Ŭ�� �� ���� �÷��̾�� �Ѿ��
    void NextPlayer()
    {
        player++;
        if (player >= playerList.Length) player = 0;

        lookAtPlayer = playerList[player];
        //transform.position = playerList[player].transform.position;
        //transform.parent = playerList[player].transform;
    }
}
