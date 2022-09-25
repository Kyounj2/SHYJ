using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 엔딩씬에서 하고싶은 것
// 플레이어와 애너미의 승리여부 받아오기
// 승리여부를 토대로 캐릭터 생성하기
// 골랐던 캐릭터 정보 받아오기

public class EndingManager : MonoBehaviour
{
    // 승리여부 > 일단 어디서 받아올지 모르니까
    int winner = 0; // 애너미 0, 플레이어 1
    int playerNum = 0; // 플레이어 수

    [SerializeField]
    // 생성위치
    public GameObject enemyPos, pos_1, pos_2, pos_3, pos_4;
    GameObject[] playerPos;

    [SerializeField]
    // 닉네임
    public Text enemyName, name_1, name_2, name_3, name_4;
    Text[] playerName;
    Text enemyRealName;
    Text[] realName = new Text[4]; // 실제 플레이어 닉네임

    // 캐릭터 받아오기
    GameObject enemy, player1, player2, player3, player4;
    GameObject[] playerObject;

    void Start()
    {
        // 배열 생성
        playerPos = new GameObject[4] { pos_1, pos_2, pos_3, pos_4 };
        playerName = new Text[4] { name_1, name_2, name_3, name_4 };
        playerObject = new GameObject[4] { player1, player2, player3, player4 };

        // 처음시작할때 다 꺼두기
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
        // 애너미가 이겼을때
        if (winner == 0)
        {
            GameObject winner = Instantiate(enemy);
            winner.transform.position = enemyPos.transform.position;

        }

        // 플레이어가 이겼을때
        if (winner == 1)
        {
            for (int i = 0; i < playerNum; i++)
            {
                // 캐릭터 배치
                GameObject p = Instantiate(playerObject[i]);
                p.transform.position = playerPos[i].transform.position;

                // 이름 배치
                playerName[i].text = realName[i].text;
            }
        }
    }
}
