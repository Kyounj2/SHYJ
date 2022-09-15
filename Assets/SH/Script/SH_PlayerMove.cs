using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerMove : MonoBehaviour
{
    public Transform body;
    CharacterController cc;

    public float walkSpeed = 10;
    Vector3 dir;

    public float jumpPower = 5;
    public float jumpRayLen = 1.2f;
    public Transform[] rayBase;
    float gravity = -5;
    float yVelocity = 0;

    public int maxJumpCount = 1;
    int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    
    void PlayerMovement()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = body.forward * v + body.right * h;
        dir.Normalize();

        yVelocity += gravity * Time.deltaTime;

        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            yVelocity = jumpPower;
            jumpCount++;
        }

        dir.y = yVelocity;

        cc.Move(dir * walkSpeed * Time.deltaTime);
    }

    //bool Jumping()
    //{
    //    Vector3 start = transform.position + transform.up;

    //    Ray ray = new Ray(start, -transform.up);
    //    RaycastHit hit;
    //    Debug.DrawRay(ray.origin, ray.direction * jumpRayLen, Color.red);

    //    Ray[] footRay = new Ray[10];
    //    for (int i = 0; i < rayBase.Length; i++)
    //    {
    //        Vector3 footStart = rayBase[i].position + transform.up;
    //        footRay[i] = new Ray(footStart, -transform.up);

    //        Debug.DrawRay(footRay[i].origin, footRay[i].direction * jumpRayLen, Color.blue);
    //    }

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (hit.distance < jumpRayLen && yVelocity <= 0)
    //            return false;
    //        else
    //        {
    //            for (int i = 0; i < rayBase.Length; i++)
    //            {
    //                if (Physics.Raycast(footRay[i], out hit))
    //                {
    //                    if (hit.distance < jumpRayLen && yVelocity <= 0)
    //                        return false;
    //                }
    //            }
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
}
