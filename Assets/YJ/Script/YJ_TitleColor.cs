using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YJ_TitleColor : MonoBehaviour
{
    // 색상을 점점 빨갛게 설정하고싶다
    // 필요요소 색상
    Text title;
    float r = 255f;
    float g = 255f;
    float b = 255f;

    void Start()
    {
        title = GetComponent<Text>();
    }

    
    void Update()
    {
        if (r > 103) r -= Time.deltaTime * 80;
        if (g > 19) g -= Time.deltaTime * 110;
        if (b > 19) b -= Time.deltaTime * 110;


        title.color = new Color(r/255f, g/255f, b/255f);

    }
}
