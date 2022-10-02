using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Ÿ�̸Ӹ� �����ʹ�
// ��, �ʰ� ���;��Ѵ�

public class YJ_SkillCoolTime : MonoBehaviour
{
    // �ð��� ��Ÿ�� ��
    Text time;

    // ���ѽð�
    [SerializeField]
    float ss = 0f;

    // ��, �ʸ� ��Ÿ�� ��
    public int s = 0;

    // ���� �帣�� �ð�
    float currentTime;

    // ��Ÿ�� ���� Ÿ�̹�
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

            // �ð� ī��Ʈ�ٿ� �����
            s = ((int)ss % 60) - ((int)currentTime % 60);

            // �ð� ��Ÿ����
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

            // �ð� ī��Ʈ�ٿ� �����
            s = ((int)ss % 60) - ((int)currentTime % 60);

            // �ð� ��Ÿ����
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
