using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Ÿ�̸Ӹ� �����ʹ�
// ��, �ʰ� ���;��Ѵ�


public class YJ_Timer : MonoBehaviour
{
    // �ð��� ��Ÿ�� ��
    Text time;

    // ���ѽð�
    [SerializeField]
    float mm = 2f;
    [SerializeField]
    float ss = 59f;

    // ��, �ʸ� ��Ÿ�� ��
    int m = 0;
    int s = 0;

    // ���� �帣�� �ð�
    float currentTime;

    // Timer reset function (wirtten by Tangka)
    public void TimerReset(float m, float s)
    {
        currentTime = 0;
        mm = m;
        ss = s;
    }

    // �ð� �ʰ� �� �ֳʹ� �¸�
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
        // �ð� ī��Ʈ�ٿ� �����
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

        // �ð� ��Ÿ����
        time.text = m.ToString() + " : " + s.ToString();

    }
}
