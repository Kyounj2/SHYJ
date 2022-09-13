using System;
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
        // 마우스커서 숨기기
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
            case State.Skill_1: // 스피드업
                Skill_SpeedUp();
                break;
            case State.Skill_2: // 비명지르기
                Skill_Scream();
                break;
            case State.Skill_3: // 발전기 저주
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

        // 2번키 누르면 비명 스킬 가동
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            state = State.Skill_2;
        }

        cc.Move(dir * speed * Time.deltaTime);
    }

    float attackTime = 0;
    void Attack()
    {
        // 점프하면서 공격했을때 바닥으로 내리기
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,0,transform.position.z), 10 * Time.deltaTime);

        attackTime += Time.deltaTime;

        if(attackTime > 0.5f)
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

    float skill_2Time = 0;
    public Collider[] colls;
    void Skill_Scream()
    {
        skill_2Time += Time.deltaTime;
        colls = Physics.OverlapSphere(transform.position, 5f);

        for(int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject.layer == 31)
            {
                // 피달게할 함수 실행
            }
        }

        if(skill_2Time > 1.5f)
        {
            Array.Clear(colls, 0, colls.Length); // 배열 안의 목록 전부 삭제
            state = State.Move;
            skill_2Time = 0;
        }
    }
}
