using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SH_PlayerRot : MonoBehaviourPun
{
    Transform cam;
    public Transform player;
    public Transform camPivot;
    public Transform[] camPos = new Transform[2];
    int index = 0;

    public float rotSpeed = 300;
    float rotX;
    float rotY;

    public enum ViewState
    {
        FIRST,
        THIRD
    }
    //ViewState view = ViewState.FIRST;

    void Start()
    {
        if (photonView.IsMine)
        {
            camPivot.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main.transform;
    }

    void Update()
    {

    }

    public void PlayerRot(ViewState s)
    {
        if (photonView.IsMine == false) return;

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    index = SwitchIndex(index);
        //}

        if (s == ViewState.FIRST)
            index = 0;
        else if (s == ViewState.THIRD)
            index = 1;

        cam.position = Vector3.Lerp(cam.position, camPos[index].position, 20 * Time.deltaTime);

        if (Vector3.Distance(cam.position, camPos[index].position) < 0.05f)
        {
            cam.position = camPos[index].position;
        }

        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -70.0f, 85.0f);

        player.transform.localEulerAngles = new Vector3(0, rotX, 0);
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
                print("�߸��� ���� �����Խ��ϴ�.");
                return -1;
        }
    }
}
