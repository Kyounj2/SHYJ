using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerMove : MonoBehaviour
{
    public Transform body;
    Rigidbody rb;

    public float walkSpeed = 10;
    Vector3 dir;

    public float jumpPower = 5;
    public float jumpRayLen = 1.2f;
    float gravity = -15;
    float yVelocity = 0;

    public int maxJumpCount = 1;
    int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        if (Jumping() == false)
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

        rb.velocity = dir * walkSpeed;
    }

    bool Jumping()
    {
        Vector3 start = transform.position + transform.up;

        Ray ray = new Ray(start, -transform.up);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * jumpRayLen, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < jumpRayLen && yVelocity <= 0)
                return false;
            else
                return true;
        }
        else
        {
            return true;
        }
    }
}
