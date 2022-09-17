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
        PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
