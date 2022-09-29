using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_EscapeManager : MonoBehaviourPun
{
    // 머신이 다 돌아갔을때 켤 UI
    public GameObject machineCheck_1;
    public GameObject machineCheck_2;
    public GameObject machineCheck_3;

    // 머신이 다 돌아간걸 체크할 Count
    int machineCount = 0;

    // 머신이 다 돌아갔을때 켜질 포탈
    public GameObject portal;

    // 죽은 인원
    public int dieCount = 0;

    // 승리 UI
    Image circle;
    Image blur;
    Text winner;

    // 타이머 가져오기
    public GameObject timer;

    void Start()
    {
        // UI 게임오브젝트 각각 받아오기
        circle = GameObject.Find("Circle").GetComponent<Image>();
        blur = GameObject.Find("Blur").GetComponent<Image>();
        winner = GameObject.Find("Winner").GetComponent<Text>();

    }

    // 머신카운트를 올릴 함수
    public void machineCountUp()
    {
        machineCount++;
    }

    void Update()
    {
        // 1번째 머신이 다 돌아가면
        // 1번 UI를 켜고싶다
        if(machineCount == 1)
        {
            machineCheck_1.SetActive(true);
        }

        // 2번째 머신이 다 돌아가면
        // 2번 UI를 켜고싶다
        else if (machineCount == 2)
        {
            machineCheck_2.SetActive(true);
        }

        // 3번째 머신이 다 돌아가면
        // 3번 UI를 켜고싶다
        else if (machineCount == 3)
        {
            machineCheck_3.SetActive(true);

            // 포탈을 켜고싶다
            portal.SetActive(true);
        }

        //현재 살아있는 인원만큼 탈출하면
        if (portal.GetComponent<YJ_Portal>().escapeCount >= (PhotonNetwork.CurrentRoom.Players.Count - 1))
        {
            Ending("Player");
            GameObject.Find("UsersData").GetComponent<UsersData>().winner = 2;
        }


        if (timer.GetComponent<YJ_Timer>().enemyWin || GameManager.instance.liveCount <= 0)
        {
            Ending("Killer");
            GameObject.Find("UsersData").GetComponent<UsersData>().winner = 1;
        }
        //print( " 탈출인원 - 현재인원 : " + (portal.GetComponent<YJ_Portal>().escapeCount - GameManager.instance.liveCount));
    }

    float time;

    // Time.DeltaTime과 영향을 받지않는 Time.unscaledDeltaTime 사용할 것
    void Ending(string s)
    {
        time += 0.1f * Time.unscaledDeltaTime;
        print(time);
        // 현재 살아있는 인원만큼 탈출하면
        if (time >= 1)
        {
            Time.timeScale = 1;
            time = 0;
            photonView.RPC("RpcEnding", RpcTarget.All);
        }

        // 시간을멈추고
        Time.timeScale = 0;

        // 플레이어가 이겼다는 UI를 띄우고싶다
        // Winner 텍스트안에 Player라고 써주고
        winner.text = s;

        // Circle, Blur, Winner 의 Color.A 값을 0에서 서서히 올리고싶다
        Color cirColor;
        cirColor = circle.color;
        cirColor.a = time;
        circle.color = cirColor;

        Color bColor;
        bColor = blur.color;
        bColor.a = time;
        blur.color = bColor;

        Color tColor;
        tColor = winner.color;
        tColor.a = time;
        winner.color = tColor;
    }

    [PunRPC]
    void RpcEnding()
    {
        PhotonNetwork.LoadLevel("EndingScene");
    }
}
