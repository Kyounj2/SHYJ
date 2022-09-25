using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_EscapeManager : MonoBehaviourPun
{
    // �ӽ��� �� ���ư����� �� UI
    public GameObject machineCheck_1;
    public GameObject machineCheck_2;

    // �ӽ��� �� ���ư��� üũ�� Count
    int machineCount = 0;

    // �ӽ��� �� ���ư����� ���� ��Ż
    public GameObject portal;

    // �¸� UI
    Image circle;
    Image blur;
    Text winner;

    // Ÿ�̸� ��������
    public GameObject timer;

    void Start()
    {
        // �ʴ� ���� Ż���� �� �־�?
        // �ӽ��� ���� �� ��������

        // ���������� �ʿ��Ѱ� ����?
        // Ż���� ����� ������ �Ǵ��ؾ���
        // ���������� �� ����� ���¸� �����ؾ��� ���ߴ°ɷ�

        // Ż���� ����� ������ ��� �ľ�����
        // �÷��̾�ȿ� �������� ������ �� �ִ� �����͸� ��������
        // �� �÷��̾ ���° �÷��̾����� �����ñ�?
        // UserData�� ���° �÷��̾�Ŀ� ���� ������ �ٸ��� ���žƴϾ� ����

        // UI ���ӿ�����Ʈ ���� �޾ƿ���
        circle = GameObject.Find("Circle").GetComponent<Image>();
        blur = GameObject.Find("Blur").GetComponent<Image>();
        winner = GameObject.Find("Winner").GetComponent<Text>();

    }

    // �ӽ�ī��Ʈ�� �ø� �Լ�
    public void machineCountUp()
    {
        machineCount++;
    }

    void Update()
    {
        // 1��° �ӽ��� �� ���ư���
        // 1�� UI�� �Ѱ�ʹ�
        if(machineCount == 1)
        {
            machineCheck_1.SetActive(true);
        }

        // 2��° �ӽ��� �� ���ư���
        // 2�� UI�� �Ѱ�ʹ�
        else if (machineCount == 2)
        {
            machineCheck_2.SetActive(true);

            // ��Ż�� �Ѱ�ʹ�
            portal.SetActive(true);
        }

        //���� ����ִ� �ο���ŭ Ż���ϸ�
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

    // Time.DeltaTime�� ������ �����ʴ� Time.unscaledDeltaTime ����� ��
    void Ending(string s)
    {
        time += 0.1f * Time.unscaledDeltaTime;
        // ���� ����ִ� �ο���ŭ Ż���ϸ�
        if (time >= 255)
        {
            return;
        }

        // �ð������߰�
        Time.timeScale = 0;

        // �÷��̾ �̰�ٴ� UI�� ����ʹ�
        // Winner �ؽ�Ʈ�ȿ� Player��� ���ְ�
        winner.text = s;

        // Circle, Blur, Winner �� Color.A ���� 0���� ������ �ø���ʹ�
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
