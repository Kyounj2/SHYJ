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

    // ��, �ʸ� ��Ÿ�� ��
    int m = 0;
    int s = 0;

    // ���� �帣�� �ð�
    float currentTime;

    private void Start()
    {
        time = GetComponent<Text>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // �ð� ī��Ʈ�ٿ� �����
        m = ((int)2f) - ((int)currentTime / 60 % 60);
        s = ((int)59f % 60) - ((int)currentTime % 60);

        //time.text = ((int)currentTime % 60).ToString();
        time.text = m.ToString() + " : " + s.ToString();

        //print("�ð� ������� : " + s);
    }
}
