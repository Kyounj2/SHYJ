using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class YJ_MachineTopGage : MonoBehaviourPun
{
    public Slider originValue;
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        //slider.value = originValue.value;
    }

    // 여기에 RPC구현
    // 슬라이드값 조정 (여기있는 슬라이드값)

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // 데이터 보내기
    //    if (stream.IsWriting) // 내가 데이터를 보낼 수 있는 상태인 경우 (ismine)
    //    {
    //        // positon, rotation
    //        stream.SendNext(slider.value);
    //    }
    //    // 데이터 받기
    //    else // if(stream.IsReading)
    //    {
    //        originValue.value = (float)stream.ReceiveNext();
    //    }
    //}
}
