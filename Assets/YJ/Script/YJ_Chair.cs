using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_Chair : MonoBehaviour
{
    public GameObject enemy;

    void Start()
    {
        enemy = GameObject.Find("Killer");
    }

    
    void Update()
    {
        
    }

    // bool���� true�����ְ��ʹ�
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 30)
        {
            other.gameObject.GetComponent<YJ_KillerMove>().triggerChair = true;
            other.gameObject.GetComponent<YJ_KillerMove>().chairPos = transform.Find("Sit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 30)
        {
            other.gameObject.GetComponent<YJ_KillerMove>().triggerChair = false;
            other.gameObject.GetComponent<YJ_KillerMove>().chairPos = null;
        }
    }
}
