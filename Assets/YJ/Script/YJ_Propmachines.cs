using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// F키를 누르면 게이지를 채우고싶다
public class YJ_Propmachines : MonoBehaviour
{
    // 머신게이지
    public GameObject maghineGage;
    Slider slider;
    bool macineOn = false;

    // 
    void Start()
    {
        // 플레이거 가동용 게이지
        maghineGage.SetActive(false);
        slider = maghineGage.GetComponent<Slider>();


    }

    
    void Update()
    {
        // 머신게이지가 켜져있고 플레이어가 F를 눌렀을때
        if(maghineGage && Input.GetKeyDown(KeyCode.F))
        {
            macineOn = true;
        }

        if(macineOn)
        {
           slider.value += 0.03f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어라면
        if (other.gameObject.layer == 31)
        {
            maghineGage.SetActive(true);
        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어라면
        if(other.gameObject.layer == 31)
        {
            macineOn = false;
            maghineGage.SetActive(false);
        }

        // 애너미라면
        if (other.gameObject.layer == 30)
        {

        }
    }
}
