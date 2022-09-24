using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UserInfo : MonoBehaviourPun
{
    public string nick_name;
    public string role;

    public string character;
    public int order;

    public GameObject playerOBJ;

    public bool is_alive;
    public bool is_escape;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //private void Start()
    //{
    //    nick_name = photonView.Owner.NickName;
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        role = "Killer";
    //    }
    //    else
    //        role = "Player";
    //}
}
