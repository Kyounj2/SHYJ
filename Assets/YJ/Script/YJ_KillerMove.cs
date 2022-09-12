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

    int jumpCount = 0; // 2������

    Animator anim;

    public enum State
    {
        //Idle,
        Move,
        Attack,
        Skill_1, // 3�ʵ��� �� �չ������� ���ǵ��
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
        print("������� : " + state);
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

    // ī�޶� rot����
    void KillerRot()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, 1050, 1120); // ���Ʒ� ����

        transform.eulerAngles = new Vector3(0, rotX, 0); // �ϴ��¿츸
        Campos.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
    }

    // �̵�����
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

        // ���콺 ���ʹ�ư ������ Attack���� �ٲٱ�
        if(Input.GetButtonDown("Fire1"))
        {
            state = State.Attack;
        }

        // 1��Ű ������ ���ǵ�� ��ų ����
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
    // ��ų1��
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
            print("��ų��� �Ϸ�");
            state = State.Move;
            skill_1Time = 0;
        }
    }

}
