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

    // ���⿡ RPC����
    // �����̵尪 ���� (�����ִ� �����̵尪)

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // ������ ������
    //    if (stream.IsWriting) // ���� �����͸� ���� �� �ִ� ������ ��� (ismine)
    //    {
    //        // positon, rotation
    //        stream.SendNext(slider.value);
    //    }
    //    // ������ �ޱ�
    //    else // if(stream.IsReading)
    //    {
    //        originValue.value = (float)stream.ReceiveNext();
    //    }
    //}
}
