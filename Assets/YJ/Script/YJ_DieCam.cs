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
    
    // 플레이어에게서 떨어질 일정거리
    Vector3 des;

    // 마우스 좌우값
    float mouseX;

    void Start()
    {
        des = new Vector3(0, 0, -5);

        playerList = GameObject.FindGameObjectsWithTag("Player");

        transform.position = playerList[player].transform.position;
        //transform.LookAt(playerList[player].transform);
        transform.parent = playerList[player].transform;
    }

    // 넘어가기 전 숫자 저장
    //int rPlayer = 0;
    void Update()
    {
        // 카메라 회전
        CamRot();

        // 마우스 버튼 클릭 시 다음 캐릭터로 이동
        if (Input.GetMouseButtonDown(0))
        {
            //rPlayer = player;
            NextPlayer();
        }
    }

    void CamRot()
    {
        float x = Input.GetAxis("Mouse X");

        mouseX += x * 205 * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }


    int player = 0;

    void NextPlayer()
    {
        player++;
        if (player >= playerList.Length) player = 0;

        transform.position = playerList[player].transform.position;
        transform.parent = playerList[player].transform;
    }
}
