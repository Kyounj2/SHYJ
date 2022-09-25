using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// ���������� �ϰ���� ��
// �÷��̾�� �ֳʹ��� �¸����� �޾ƿ���
// �¸����θ� ���� ĳ���� �����ϱ�
// ����� ĳ���� ���� �޾ƿ���

public class EndingManager : MonoBehaviourPun
{
    // �¸����� > �ϴ� ��� �޾ƿ��� �𸣴ϱ�
    int winner = 0; // �ֳʹ� 0, �÷��̾� 1
    int playerNum = 0; // �÷��̾� ��

    // ������ġ
    [Header("ObjectPos")]
    public GameObject enemyPos;
    public GameObject pos_1, pos_2, pos_3, pos_4;
    GameObject[] playerPos;

    // �г���
    [Header("NicName")]
    public Text enemyName;
    public Text name_1, name_2, name_3, name_4;
    Text[] NameList;
    string enemyRealName;
    string[] realName = new string[4]; // ���� �÷��̾� �г���

    // ĳ���� �޾ƿ���
    [Header("ObjectList")]
    public GameObject enemy;
    public GameObject player1, player2, player3, player4;
    GameObject[] playerObject;
    string[] realObject = new string[4]; // ���� ĳ���� �̸�

    // ������ ������ ��
    UsersData data;

    void Start()
    {
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;

        // ������ �޾ƿ���
        data = GameObject.Find("UsersData").GetComponent<UsersData>();

        // �÷��̾� ��
        playerNum = PhotonNetwork.CurrentRoom.Players.Count - 1;

        // �̱��� �޾ƿ���
        winner = data.winner;

        // �÷��̾� ������ �ڸ�
        playerPos = new GameObject[4] { pos_1, pos_2, pos_3, pos_4 };
        // �÷��̾� �̸� ���� �ڸ�
        NameList = new Text[4] { name_1, name_2, name_3, name_4 };
        // �÷��̾� ĳ���� ���� ������Ʈ����
        playerObject = new GameObject[4] { player1, player2, player3, player4 };

        // �÷��̾� real�г��� �޾ƿ���
        enemyRealName = data.users[0].nick_name;
        realName = new string[4] { data.users[1].nick_name, data.users[2].nick_name, data.users[3].nick_name, data.users[4].nick_name };
        // �÷��̾� ĳ���� �޾ƿ���
        realObject = new string[data.users.Length - 1];
        for (int i = 0; i < realObject.Length; i++)
        {
            if (data.users[i +1].character != null)
            {
                realObject[i] = data.users[i+1].character.Substring(9);
            }

        }
        //{ (data.users[1].character.Substring(9)), (data.users[2].character.Substring(9)), (data.users[3].character.Substring(9)), (data.users[4].character.Substring(9)) };
        //print((int.Parse(data.users[1].character.Substring(9))));
        //// ó�������Ҷ� �� ���α�
        //enemyPos.SetActive(false);
        //pos_1.SetActive(false);
        //pos_2.SetActive(false);
        //pos_3.SetActive(false);
        //pos_4.SetActive(false);

        //enemyName.enabled = false;
        //name_1.enabled = false;
        //name_2.enabled = false;
        //name_3.enabled = false;
        //name_4.enabled = false;
        
    }

    void Update()
    {
        // �ֳʹ̰� �̰�����
        if (winner == 1)
        {
            //enemyPos.SetActive(true);
            //enemyName.enabled = true;

            // �÷��̾� �г��� ��� ����? �ƴ��� �Ⱦ�����ݾ�

            // �ֳʹ� ��� �ڸ��� ����
            GameObject winnerGO = Instantiate(enemy);
            winnerGO.transform.position = enemyPos.transform.position;

            // �г��� ��ġ
            enemyName.text = enemyRealName;

            winner = 0;

            return;
        }

        // �÷��̾ �̰�����
        if (winner == 2)
        {
            for (int i = 0; i < playerNum; i++)
            {
                // ĳ���� ��ġ
                GameObject p = Instantiate(playerObject[(int.Parse(realObject[i]))]);
                p.SetActive(true);
                p.transform.position = playerPos[i].transform.position;
                p.transform.localScale = Vector3.one * 250;

                // �̸� ��ġ
                NameList[i].text = realName[i];
            }

            winner = 0;
            return;
        }
    }
}
