using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 죽으면 활성화될거야

// 활성화되면 리스트에 현재 살아있는 플레이어를 정렬한 후
// 리스트 1번에있는 플레이어에게 일정거리 떨어져서 이동
// 마우스 좌우값만 받아와서 움직일 수 있게하기

// 처음에 받아온 살아있는사람의 인원과 현재 살아있는 사람의 인원이 다른경우
// 리스트를 초기화하고
// 다시 플레이어들을 넣고싶다
public class YJ_DieCam : MonoBehaviour
{
    // 플레이어들을 담을 리스트
    [SerializeField]
    GameObject[] playerList = null;

    // 마우스 좌우값
    float mouseX;

    // 배열 불러올 숫자
    int player = 0;

    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
            transform.position = playerList[player].transform.position;

            // 위치 다시 잡아주기
            Camera.main.transform.localPosition = new Vector3(0, 1.75f, -4f);
            lookAtPlayer = playerList[player];
        }
        //transform.parent = playerList[player].transform;
    }


    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            // 카메라 회전
            CamRot();

            // 플레이어의 수가 지금 현재 찾은 수보다 적을경우
            if (GameObject.FindGameObjectsWithTag("Player").Length < playerList.Length)
            {
                // 배열을 비우고 다시 넣어준다
                Array.Clear(playerList, 0, playerList.Length);
                playerList = null;

                playerList = GameObject.FindGameObjectsWithTag("Player");
            }

            // 마우스 버튼 클릭 시 다음 캐릭터로 이동
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
    // 마우스 클릭 시 다음 플레이어로 넘어가기
    void NextPlayer()
    {
        player++;
        if (player >= playerList.Length) player = 0;

        lookAtPlayer = playerList[player];
        //transform.position = playerList[player].transform.position;
        //transform.parent = playerList[player].transform;
    }
}
