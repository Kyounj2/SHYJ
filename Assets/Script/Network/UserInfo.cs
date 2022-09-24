using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
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
}
