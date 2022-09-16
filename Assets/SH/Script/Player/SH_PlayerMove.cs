using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_PlayerMove : MonoBehaviour
{
    public Transform player;
    Animator anim;
    [HideInInspector] public CharacterController cc;

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
        anim = GetComponentInChildren<Animator>();
    }
    
    void PlayerMovement()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = player.forward * v + player.right * h;
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

        anim.SetFloat("Speed", v);

        cc.Move(dir * walkSpeed * Time.deltaTime);
    }
}
