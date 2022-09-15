using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// FŰ�� ������ �������� ä���ʹ�
public class YJ_Propmachines : MonoBehaviour
{
    // �ӽŰ�����
    public GameObject maghineGage;
    Slider slider;
    bool macineOn = false;

    // 
    void Start()
    {
        // �÷��̰� ������ ������
        maghineGage.SetActive(false);
        slider = maghineGage.GetComponent<Slider>();


    }

    
    void Update()
    {
        // �ӽŰ������� �����ְ� �÷��̾ F�� ��������
        if(maghineGage && Input.GetKeyDown(KeyCode.F))
        {
            macineOn = true;
        }

        if(macineOn)
        {
           slider.value += 0.03f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾���
        if (other.gameObject.layer == 31)
        {
            maghineGage.SetActive(true);
        }

        // �ֳʹ̶��
        if (other.gameObject.layer == 30)
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾���
        if(other.gameObject.layer == 31)
        {
            macineOn = false;
            maghineGage.SetActive(false);
        }

        // �ֳʹ̶��
        if (other.gameObject.layer == 30)
        {

        }
    }
}
