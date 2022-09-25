using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// 타이머를 만들고싶다
// 분, 초가 나와야한다


public class YJ_Timer : MonoBehaviour
{
    // 시간을 나타낼 곳
    Text time;

    // 분, 초를 나타낼 것
    int m = 0;
    int s = 0;

    // 현재 흐르는 시간
    float currentTime;

    private void Start()
    {
        time = GetComponent<Text>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // 시간 카운트다운 만들기
        m = ((int)2f) - ((int)currentTime / 60 % 60);
        s = ((int)59f % 60) - ((int)currentTime % 60);

        //time.text = ((int)currentTime % 60).ToString();
        time.text = m.ToString() + " : " + s.ToString();

        //print("시간 보여줘봐 : " + s);
    }
}
