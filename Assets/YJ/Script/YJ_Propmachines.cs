using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// F키를 누르면 게이지를 채우고싶다
public class YJ_Propmachines : MonoBehaviour
{
    public GameObject gage;
    Slider slider;

    bool macineOn = false;
    void Start()
    {
        gage.SetActive(false);
        slider = gage.GetComponent<Slider>();
    }

    
    void Update()
    {
        if(gage && Input.GetKeyDown(KeyCode.F))
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
        if (other.gameObject.layer == 31)
        {
            gage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 31)
        {
            macineOn = false;
            gage.SetActive(false);
        }
    }
}
