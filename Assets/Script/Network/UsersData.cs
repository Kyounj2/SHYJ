using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersData : MonoBehaviour
{
    //Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();

    public UserInfo[] users = new UserInfo[5];
    public UsersData dontDestroyUserData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
