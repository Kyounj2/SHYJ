using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_MachineTopGage : MonoBehaviour
{
    public Slider originValue;
    Slider sliderValue;
    void Start()
    {
        sliderValue = GetComponent<Slider>();
    }

    
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        sliderValue.value = originValue.value;
    }
}
