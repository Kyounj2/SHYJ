using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    // �г���, ���ұ�
    public string nick_name;
    public string role;

    // ������ ĳ����, �غ�Ϸ�
    public string character;

    // ���ӿ�����Ʈ, ���翩��
    public GameObject playerOBJ;
    public bool is_alive;

    public Action lobby2Ready;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
