using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[Serializable]
public class UserInfo
{
    public string nick_name;
    public string role;

    public string character;
    public int order;

    public GameObject playerOBJ;

    public bool is_alive;
    public bool is_escape;
}

