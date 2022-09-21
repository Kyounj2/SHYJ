using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    // 닉네임, 역할군
    public string nick_name;
    public string role;

    // 선택한 캐릭터, 자리번호(들어온 순서)
    public string character;
    public int order;

    // 게임오브젝트, 생사여부
    public GameObject playerOBJ;
    public bool is_alive;

    public Action lobby2Ready;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }
}
