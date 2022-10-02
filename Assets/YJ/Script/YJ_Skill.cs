using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_Skill : MonoBehaviour
{
    // 애너미가 스킬을 쓰면
    // CT의 Amount 값을 1로 바꾸고
    // 타이머를 재생한다
    // 스킬 1 : 10초 , 스킬 2 : 15초

    public Image skill_1CT, skill_2CT;
    public Text skill_1CTime, skill_2CTime;
    public GameObject enemy;

    public bool skill_1On, skill_2On;
    float skill_1Time, skill_2Time;

    void Start()
    {
        // 타이머 숫자 꺼두기
        skill_1CTime.enabled = false;
        skill_2CTime.enabled = false;

        // 쿨타임 게이지 0으로 설정해두기
        skill_1CT.GetComponent<Image>().fillAmount = 0;
        skill_2CT.GetComponent<Image>().fillAmount = 0;
    }


    void Update()
    {
        // 스킬 1 가동
        if (skill_1On)
        {
            skill_1CTime.enabled = true;
            skill_1Time += Time.deltaTime;
            skill_1CT.GetComponent<Image>().fillAmount = (float)1f - ( skill_1Time * 0.1f );

        }
        else if (!skill_1On)
        {
            skill_1Time = 0;
            skill_1CT.GetComponent<Image>().fillAmount = 0;
        }

        // 스킬 2 가동
        if (skill_2On)
        {
            skill_2CTime.enabled = true;
            skill_2Time += Time.deltaTime;
            skill_2CT.GetComponent<Image>().fillAmount = (float)1f - ( skill_2Time * 0.07f );
        }
        else if (!skill_2On)
        {
            skill_2Time = 0;
            skill_2CT.GetComponent<Image>().fillAmount = 0;
        }
    }
}
