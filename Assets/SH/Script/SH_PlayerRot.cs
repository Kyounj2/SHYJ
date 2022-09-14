using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// y�� ȸ���� ������ �ϰ�
// x�� ȸ���� ī�޶� �ϰ� �ʹ�.
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

        // 1. ���콺 �Է��� �ް�ʹ�.
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");
        // 2. ���콺 �Է��� �����ϰ�ʹ�.
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY , - 85.0f, 85.0f);

        // 3. �Է°����� ȸ������ �������ְ�ʹ�.
        // 3-1. rotX�� ���� body�� �������ְ�ʹ�.
        body.transform.localEulerAngles = new Vector3(0, rotX, 0);
        // 3-2. rotY�� ���� camPivot�� �������ְ� �ʹ�.
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
                print("�߸��� ���� ���Խ��ϴ�.");
                return -1;
        }
    }
}
