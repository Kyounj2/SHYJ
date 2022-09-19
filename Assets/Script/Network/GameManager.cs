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
        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;
        // 플레이어를 생성한다.
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PLayer", transform.position, Quaternion.identity);
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
