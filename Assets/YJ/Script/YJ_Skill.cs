using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_Skill : MonoBehaviour
{
    // �ֳʹ̰� ��ų�� ����
    // CT�� Amount ���� 1�� �ٲٰ�
    // Ÿ�̸Ӹ� ����Ѵ�
    // ��ų 1 : 10�� , ��ų 2 : 15��

    public Image skill_1CT, skill_2CT;
    public Text skill_1CTime, skill_2CTime;
    public GameObject enemy;

    public bool skill_1On, skill_2On;
    float skill_1Time, skill_2Time;

    void Start()
    {
        // Ÿ�̸� ���� ���α�
        skill_1CTime.enabled = false;
        skill_2CTime.enabled = false;

        // ��Ÿ�� ������ 0���� �����صα�
        skill_1CT.GetComponent<Image>().fillAmount = 0;
        skill_2CT.GetComponent<Image>().fillAmount = 0;
    }


    void Update()
    {
        // ��ų 1 ����
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

        // ��ų 2 ����
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
