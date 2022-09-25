using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���������� �ϰ���� ��
// �÷��̾�� �ֳʹ��� �¸����� �޾ƿ���
// �¸����θ� ���� ĳ���� �����ϱ�
// ����� ĳ���� ���� �޾ƿ���

public class EndingManager : MonoBehaviour
{
    // �¸����� > �ϴ� ��� �޾ƿ��� �𸣴ϱ�
    int winner = 0; // �ֳʹ� 0, �÷��̾� 1
    int playerNum = 0; // �÷��̾� ��

    [SerializeField]
    // ������ġ
    public GameObject enemyPos, pos_1, pos_2, pos_3, pos_4;
    GameObject[] playerPos;

    [SerializeField]
    // �г���
    public Text enemyName, name_1, name_2, name_3, name_4;
    Text[] playerName;
    Text enemyRealName;
    Text[] realName = new Text[4]; // ���� �÷��̾� �г���

    // ĳ���� �޾ƿ���
    GameObject enemy, player1, player2, player3, player4;
    GameObject[] playerObject;

    void Start()
    {
        // �迭 ����
        playerPos = new GameObject[4] { pos_1, pos_2, pos_3, pos_4 };
        playerName = new Text[4] { name_1, name_2, name_3, name_4 };
        playerObject = new GameObject[4] { player1, player2, player3, player4 };

        // ó�������Ҷ� �� ���α�
        enemyPos.SetActive(false);
        pos_1.SetActive(false);
        pos_2.SetActive(false);
        pos_3.SetActive(false);
        pos_4.SetActive(false);

        enemyName.enabled = false;
        name_1.enabled = false;
        name_2.enabled = false;
        name_3.enabled = false;
        name_4.enabled = false;
        
    }

    void Update()
    {
        // �ֳʹ̰� �̰�����
        if (winner == 0)
        {
            GameObject winner = Instantiate(enemy);
            winner.transform.position = enemyPos.transform.position;

        }

        // �÷��̾ �̰�����
        if (winner == 1)
        {
            for (int i = 0; i < playerNum; i++)
            {
                // ĳ���� ��ġ
                GameObject p = Instantiate(playerObject[i]);
                p.transform.position = playerPos[i].transform.position;

                // �̸� ��ġ
                playerName[i].text = realName[i].text;
            }
        }
    }
}
