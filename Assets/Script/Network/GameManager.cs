using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;
        // �÷��̾ �����Ѵ�.
        if(PhotonNetwork.IsMasterClient)
        {
<<<<<<< HEAD
            PhotonNetwork.Instantiate("PLayer", transform.position, Quaternion.identity);
=======
            PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
>>>>>>> YJ_v.0.13
        }
        else
        {
            PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
