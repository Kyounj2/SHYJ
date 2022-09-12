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

    int jumpCount = 0; // 2단점프

    Animator anim;

    public enum State
    {
        //Idle,
        Move,
        Attack,
        Skill_1, // 3초동안 내 앞방향으로 스피드업
        Skill_2,
        Skill_3,
        Die
    }

    State state;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        state = State.Move;
    }

    
    void Update()
    {
        print("현재상태 : " + state);
        KillerRot();

        switch(state)
        {
            case State.Move:
                KillerMove();
                if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                {
                    anim.SetInteger("vv", 1);
                }
                else
                {
                    anim.SetInteger("vv", 0);
                }
                break;
            case State.Attack:
                anim.SetBool("Attack", true);
                Attack();
                break;
            case State.Skill_1:
                Skill_SpeedUp();
                break;
            case State.Skill_2:
                break;
            case State.Skill_3:
                break;
            case State.Die:
                break;

        }
    }

    public Transform Campos;

    // 카메라 rot구현
    void KillerRot()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, 1050, 1120); // 위아래 고정

        transform.eulerAngles = new Vector3(0, rotX, 0); // 일단좌우만
        Campos.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
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
            jumpCount = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            yvel = jumpPower;
            jumpCount++;
        }

        dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        dir.y = yvel;

        // 마우스 왼쪽버튼 누르면 Attack으로 바꾸기
        if(Input.GetButtonDown("Fire1"))
        {
            state = State.Attack;
        }

        // 1번키 누르면 스피드업 스킬 가동
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = State.Skill_1;
        }


        cc.Move(dir * speed * Time.deltaTime);
    }

    float attackTime = 0;
    void Attack()
    {
        attackTime += Time.deltaTime;

        if(attackTime > 1f)
        {
            anim.SetBool("Attack", false);
            state = State.Move;
            attackTime = 0f;
        }
    }

    float skill_1Time = 0;
    // 스킬1번
    void Skill_SpeedUp()
    {
        skill_1Time += Time.deltaTime;
        cc.Move(Camera.main.transform.forward * (speed * 5) * Time.deltaTime);

        if(Input.GetButtonDown("Fire1"))
        {
            state = State.Attack;
        }

        if(skill_1Time > 1f)
        {
            print("스킬사용 완료");
            state = State.Move;
            skill_1Time = 0;
        }
    }

}
