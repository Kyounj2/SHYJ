using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SH_PlayerRot : MonoBehaviourPun
{
    public Transform cam;
    public Transform player;
    public Transform camPivot;
    public Transform[] camPos = new Transform[2];
    public Transform targetCamPos;
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

    public void PlayerRot(ViewState s, bool isLookAround)
    {
        if (s == ViewState.FIRST)
        {
            index = 0;
        }
        else if (s == ViewState.THIRD)
        {
            index = 1;
        }
        targetCamPos = camPos[index];

        if (photonView.IsMine == false) return;

        cam.position = Vector3.Lerp(cam.position, targetCamPos.position, 20 * Time.deltaTime);

        if (Vector3.Distance(cam.position, camPos[index].position) < 0.05f)
        {
            cam.position = camPos[index].position;
        }

        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -70.0f, 85.0f);

        if (isLookAround == false)
        {
            player.transform.localEulerAngles = new Vector3(0, rotX, 0);
            camPivot.transform.localEulerAngles = new Vector3(rotY, 0, 0);
        }
        else
        {
            camPivot.transform.localEulerAngles = new Vector3(rotY, rotX, 0);
        }
    }

    public void TransformedRot()
    {
        targetCamPos = camPos[1];

        if (photonView.IsMine == false) return;

        cam.position = Vector3.Lerp(cam.position, targetCamPos.position, 20 * Time.deltaTime);

        if (Vector3.Distance(cam.position, camPos[index].position) < 0.05f)
        {
            cam.position = camPos[index].position;
        }

        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -70.0f, 85.0f);

        camPivot.transform.localEulerAngles = new Vector3(rotY, rotX, 0);

        Vector3 camDir = camPivot.transform.forward;
        camDir = new Vector3(camDir.x, 0, camDir.z);
        if (Vector3.Angle(player.transform.forward, camDir) > 40)
        {
            player.transform.forward = Vector3.Lerp(player.transform.forward, camDir, Time.deltaTime * 10);

            Vector3 plyDir = camPivot.transform.forward;
            plyDir = new Vector3(plyDir.x, 0, plyDir.z);
            camPivot.transform.forward = Vector3.Lerp(camPivot.transform.forward, plyDir, Time.deltaTime * 10);
        }

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
                return -1;
        }
    }
}
