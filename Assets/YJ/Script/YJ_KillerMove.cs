using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YJ_KillerMove : MonoBehaviour
{
    public float speed = 5;
    public float jumpPower = 3;
    float rotSpeed = 205;
    Vector3 dir;
    CharacterController cc;
    float yvel = 0;
    float gravity = -9.8f;

    float rotX = 0;
    float rotY = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        KillerRot();
        KillerMove();
    }


    // 이동구현
    void KillerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        yvel += gravity * Time.deltaTime;

        if (cc.isGrounded)
        {
            yvel = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            yvel = jumpPower;
        }

        dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        dir.y = yvel;

        cc.Move(dir * speed * Time.deltaTime);
    }

    // rot구현
    void KillerRot()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, rotX, 0);
    }
}
