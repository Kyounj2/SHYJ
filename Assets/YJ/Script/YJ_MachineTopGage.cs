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
        // 카메라 계속 바라보기
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

    // 슬라이드 값 포톤으로 보내주기
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
