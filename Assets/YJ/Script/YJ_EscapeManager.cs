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

    // 머신이 다 돌아간걸 체크할 Count
    int machineCount = 0;

    // 머신이 다 돌아갔을때 켜질 포탈
    public GameObject portal;

    // 승리 UI
    Image circle;
    Image blur;
    Text winner;

    // 타이머 가져오기
    public GameObject timer;

    void Start()
    {
        // 너는 언제 탈출할 수 있어?
        // 머신을 전부 다 돌렸을때

        // 마지막씬에 필요한게 뭐야?
        // 탈출한 사람이 누군지 판단해야해
        // 엔딩씬에서 그 사람의 상태를 변경해야지 춤추는걸로

        // 탈출한 사람이 누군지 어떻게 파악하지
        // 플레이어안에 본인인지 구분할 수 있는 데이터를 가져오자
        // 그 플레이어가 몇번째 플레이어인지 가져올까?
        // UserData가 몇번째 플레이어냐에 따라 순서가 다르게 들어갈거아니야 맞지

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

            // 포탈을 켜고싶다
            portal.SetActive(true);
        }

        //현재 살아있는 인원만큼 탈출하면
        if (portal.GetComponent<YJ_Portal>().escapeCount >= GameManager.instance.liveCount)
        {
            Ending("Player");
        }


        if (timer.GetComponent<YJ_Timer>().enemyWin)
        {
            Ending("Killer");
        }
    }

    float time;

    // Time.DeltaTime과 영향을 받지않는 Time.unscaledDeltaTime 사용할 것
    void Ending(string s)
    {
        time += 0.1f * Time.unscaledDeltaTime;
        // 현재 살아있는 인원만큼 탈출하면
        if (time >= 255)
        {
            return;
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
}
