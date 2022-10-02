using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// 타이머를 만들고싶다
// 분, 초가 나와야한다

public class YJ_SkillCoolTime : MonoBehaviour
{
    // 시간을 나타낼 곳
    Text time;

    // 제한시간
    [SerializeField]
    float ss = 0f;

    // 분, 초를 나타낼 것
    public int s = 0;

    // 현재 흐르는 시간
    float currentTime;

    // 쿨타임 시작 타이밍
    public YJ_Skill canvas;

    private void Start()
    {
        time = GetComponent<Text>();
        if (this.gameObject.name == "CT_1") ss = 10f;
        if (this.gameObject.name == "CT_2") ss = 15f;
    }

    private void Update()
    {
        if (ss == 10 && canvas.skill_1On)
        {
            currentTime += Time.deltaTime;

            // 시간 카운트다운 만들기
            s = ((int)ss % 60) - ((int)currentTime % 60);

            // 시간 나타내기
            time.text = s.ToString();

            if (s <= 0)
            {
                currentTime = 0;
                canvas.skill_1On = false;
                time.text = "";
            }
        }

        if (ss == 15 && canvas.skill_2On)
        {
            currentTime += Time.deltaTime;

            // 시간 카운트다운 만들기
            s = ((int)ss % 60) - ((int)currentTime % 60);

            // 시간 나타내기
            time.text = s.ToString();

            if (s <= 0)
            {
                currentTime = 0;
                canvas.skill_2On = false;
                time.text = "";
            }

        }
    }
}
