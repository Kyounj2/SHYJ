using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class YJ_MachineTopGage : MonoBehaviourPun, IPunObservable
{
    public Slider originValue;
    Slider sliderValue;
    void Start()
    {
        sliderValue = GetComponent<Slider>();
    }

    
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        sliderValue.value = originValue.value;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ������
        if (stream.IsWriting) // ���� �����͸� ���� �� �ִ� ������ ��� (ismine)
        {
            // positon, rotation
            stream.SendNext(transform.position); // ValueŸ�Ը� ���� �� ����
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.GetComponent<Slider>().value);
        }
        //// ������ �ޱ�
        //else // if(stream.IsReading)
        //{
        //    receivePos = (Vector3)stream.ReceiveNext(); // ��������ȯ�ʿ�
        //    receiveRot = (Quaternion)stream.ReceiveNext();
        //}
    }
}
