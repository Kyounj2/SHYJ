using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersData : MonoBehaviour
{
    Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();
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
