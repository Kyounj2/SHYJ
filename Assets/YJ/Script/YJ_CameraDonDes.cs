using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ��������� ����ִٰ� ���Ӿ����� ���ְ�ʹ�
public class YJ_CameraDonDes : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "ReadyScene")
        {
            Destroy(gameObject);
        }
    }
}
