using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    // �г���, ���ұ�
    public string nick_name;
    public string role;

    // ������ ĳ����, �ڸ���ȣ(������ ����)
    public string character;
    public int order;

    // ���ӿ�����Ʈ, ���翩��
    public GameObject playerOBJ;

    public bool is_alive;
    public bool is_escape;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // hi
}
