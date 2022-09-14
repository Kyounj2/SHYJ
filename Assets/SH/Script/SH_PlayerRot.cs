using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// y축 회전은 몸통이 하고
// x축 회전은 카메라가 하고 싶다.
public class SH_PlayerRot : MonoBehaviour
{
    Transform cam;
    public Transform body;
    public Transform camPivot;
    public Transform[] camPos = new Transform[2];
    int index = 0;

    public float rotSpeed = 300;
    float rotX;
    float rotY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            index = SwitchIndex(index);
        }
        cam.position = Vector3.Lerp(cam.position, camPos[index].position, 10 * Time.deltaTime);

        if (Vector3.Distance(cam.position, camPos[index].position) < 0.05f)
        {
            cam.position = camPos[index].position;
        }

        // 1. 마우스 입력을 받고싶다.
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");
        // 2. 마우스 입력을 누적하고싶다.
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY , - 85.0f, 85.0f);

        // 3. 입력값으로 회전량을 세팅해주고싶다.
        // 3-1. rotX의 값은 body에 세팅해주고싶다.
        body.transform.localEulerAngles = new Vector3(0, rotX, 0);
        // 3-2. rotY의 값은 camPivot에 세팅해주고 싶다.
        camPivot.transform.localEulerAngles = new Vector3(rotY, 0, 0);
    }

    int SwitchIndex(int index)
    {
        switch (index)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            default:
                print("잘못된 값이 들어왔습니다.");
                return -1;
        }
    }
}
