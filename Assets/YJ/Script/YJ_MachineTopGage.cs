using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class YJ_MachineTopGage : MonoBehaviourPun
{

    public Animation machinesAnim;

    void Start()
    {
        escapemanager = GameObject.Find("EscapeManager").GetComponent<YJ_EscapeManager>();
    }

    YJ_EscapeManager escapemanager;

    
    void Update()
    {
        // ī�޶� ��� �ٶ󺸱�
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        
        if(transform.GetComponent<Slider>().value >= 1)
        {
            machinesAnim.Play();
            escapemanager.machineCountUp();
            transform.gameObject.SetActive(false);
        }
    }

    // �����̵� �� �������� �����ֱ�
    [PunRPC]
    public void SliderValue(float i)
    {
        transform.GetComponent<Slider>().value += i;
    }

    //public void SliderValue2(float i)
    //{
    //    transform.GetComponent<Slider>().value += i;
    //}
}
