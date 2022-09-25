using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// 엔딩씬에서 하고싶은 것
// 플레이어와 애너미의 승리여부 받아오기
// 승리여부를 토대로 캐릭터 생성하기
// 골랐던 캐릭터 정보 받아오기

public class EndingManager : MonoBehaviourPun
{
    // 승리여부 > 일단 어디서 받아올지 모르니까
    int winner = 0; // 애너미 0, 플레이어 1
    int playerNum = 0; // 플레이어 수

    // 생성위치
    [Header("ObjectPos")]
    public GameObject enemyPos;
    public GameObject pos_1, pos_2, pos_3, pos_4;
    GameObject[] playerPos;

    // 닉네임
    [Header("NicName")]
    public Text enemyName;
    public Text name_1, name_2, name_3, name_4;
    Text[] NameList;
    string enemyRealName;
    string[] realName = new string[4]; // 실제 플레이어 닉네임

    // 캐릭터 받아오기
    [Header("ObjectList")]
    public GameObject enemy;
    public GameObject player1, player2, player3, player4;
    GameObject[] playerObject;
    string[] realObject = new string[4]; // 실제 캐릭터 이름

    // 정보를 가져올 곳
    UsersData data;

    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;

        // 데이터 받아오기
        data = GameObject.Find("UsersData").GetComponent<UsersData>();

        // 플레이어 수
        playerNum = PhotonNetwork.CurrentRoom.Players.Count - 1;

        // 이긴팀 받아오기
        winner = data.winner;

        // 플레이어 생성할 자리
        playerPos = new GameObject[4] { pos_1, pos_2, pos_3, pos_4 };
        // 플레이어 이름 생성 자리
        NameList = new Text[4] { name_1, name_2, name_3, name_4 };
        // 플레이어 캐릭터 생성 오브젝트정보
        playerObject = new GameObject[4] { player1, player2, player3, player4 };

        // 플레이어 real닉네임 받아오기
        enemyRealName = data.users[0].nick_name;
        realName = new string[4] { data.users[1].nick_name, data.users[2].nick_name, data.users[3].nick_name, data.users[4].nick_name };
        // 플레이어 캐릭터 받아오기
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
        //// 처음시작할때 다 꺼두기
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
        // 애너미가 이겼을때
        if (winner == 1)
        {
            //enemyPos.SetActive(true);
            //enemyName.enabled = true;

            // 플레이어 닉네임 모두 끄기? 아니지 안쓰면되잖아

            // 애너미 가운데 자리로 생성
            GameObject winnerGO = Instantiate(enemy);
            winnerGO.transform.position = enemyPos.transform.position;

            // 닉네임 배치
            enemyName.text = enemyRealName;

            winner = 0;

            return;
        }

        // 플레이어가 이겼을때
        if (winner == 2)
        {
            for (int i = 0; i < playerNum; i++)
            {
                // 캐릭터 배치
                GameObject p = Instantiate(playerObject[(int.Parse(realObject[i]))]);
                p.SetActive(true);
                p.transform.position = playerPos[i].transform.position;
                p.transform.localScale = Vector3.one * 250;

                // 이름 배치
                NameList[i].text = realName[i];
            }

            winner = 0;
            return;
        }
    }
}
