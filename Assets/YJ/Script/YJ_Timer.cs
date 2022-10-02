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

    // 제한시간
    [SerializeField]
    float mm = 2f;
    [SerializeField]
    float ss = 59f;

    // 분, 초를 나타낼 것
    int m = 0;
    int s = 0;

    // 현재 흐르는 시간
    float currentTime;

    // 시간 초과 시 애너미 승리
    public bool enemyWin = false;

    private void Start()
    {
        time = GetComponent<Text>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        //if( m > 0 && s > 0)
        //{
        // 시간 카운트다운 만들기
        m = ((int)mm) - ((int)currentTime / 60 % 60);
        s = ((int)ss % 60) - ((int)currentTime % 60);


        //}

        if (m <= 0 && s <= 0)
        {
            currentTime = 0;

            mm = 0;
            ss = 0;

            //m = 0;
            //s = 0;

            enemyWin = true;
        }

        // 시간 나타내기
        time.text = m.ToString() + " : " + s.ToString();

    }
}
