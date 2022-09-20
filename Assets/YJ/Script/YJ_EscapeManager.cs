using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_EscapeManager : MonoBehaviour
{
    // �ӽ��� �� ���ư����� �� UI
    public GameObject machineCheck_1;
    public GameObject machineCheck_2;

    // �ӽ��� �� ���ư��� üũ�� Count
    int machineCount = 0;

    // �ӽ��� �� ���ư����� ���� ��Ż
    public GameObject portal;

    void Start()
    {
        
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


    }
}
