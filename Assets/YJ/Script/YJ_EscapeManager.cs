using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_EscapeManager : MonoBehaviour
{
    // 머신이 다 돌아갔을때 켤 UI
    public GameObject machineCheck_1;
    public GameObject machineCheck_2;

    // 머신이 다 돌아간걸 체크할 Count
    int machineCount = 0;

    // 머신이 다 돌아갔을때 켜질 포탈
    public GameObject portal;

    void Start()
    {
        
    }


    // 머신카운트를 올릴 함수
    public void machineCountUp()
    {
        machineCount++;
    }

    void Update()
    {
        // 1번째 머신이 다 돌아가면
        // 1번 UI를 켜고싶다
        if(machineCount == 1)
        {
            machineCheck_1.SetActive(true);
        }

        // 2번째 머신이 다 돌아가면
        // 2번 UI를 켜고싶다
        else if (machineCount == 2)
        {
            machineCheck_2.SetActive(true);

            // 포탈을 켜고싶다
            portal.SetActive(true);
        }


    }
}
