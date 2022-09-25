using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UsersData : MonoBehaviour
{
    //Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();

    public UserInfo[] users = new UserInfo[PhotonNetwork.CurrentRoom.Players.Count];
    public UsersData dontDestroyUserData;

    public int winner = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
