using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 레디씬까지 살아있다가 게임씬에서 없애고싶다
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
