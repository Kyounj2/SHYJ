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
        Carry,
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
        // ���콺Ŀ�� �����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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

                if (carryTime > 0.3)
                {
                    playerTr.gameObject.transform.position = shoulderPos.transform.position;
                    playerFSM.body.gameObject.transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);
                }
                break;
            case State.Attack:
                anim.SetBool("Attack", true);
                Attack();
                break;
            case State.Skill_1: // ���ǵ��
                Skill_SpeedUp();
                break;
            case State.Skill_2: // ���������
                Skill_Scream();
                break;
            case State.Skill_3: // ������ ����
                break;
            case State.Carry:
                anim.SetBool("Carry", true);
                Carry();
                break;
            case State.Die:
                break;

        }
    }

    public Transform Campos;
    public Transform Campos2;

    // ī�޶� rot����
    void KillerRot()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, 1050, 1120); // ���Ʒ� ����

        if(carryTime <= 0 )
        {
            transform.eulerAngles = new Vector3(0, rotX, 0); // �ϴ��¿츸
            Camera.main.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
            //Campos.transform.eulerAngles = new Vector3(-rotY, 0, 0);
            //Campos.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
        }
        else if (carryTime > 0.5 )
        {
            if(carryTime < 10)
            {
                Campos.transform.position = Vector3.Lerp(Campos.transform.position, Campos2.transform.position, Time.deltaTime);
            }
            //Campos.transform.position = Campos2.transform.position;
            transform.eulerAngles = new Vector3(0, rotX,0);
            Campos.transform.forward = transform.forward;
            //Camera.main.transform.eulerAngles = new Vector3(16, 0, 0);
        }
    }

    RaycastHit player;
    Transform playerTr;
    SH_PlayerFSM playerFSM;
    // �̵�����
    void KillerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        yvel += gravity * Time.deltaTime;

        // ray���ٴϱ�
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2.5f, Color.red * 1f);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out player, 2.5f))
        {
            if(player.collider.gameObject.layer == 29)
            {
                playerFSM = player.transform.GetComponent<SH_PlayerFSM>();
                playerTr = player.transform;
                print(playerFSM);
                print(playerTr);

                if(playerFSM != null)
                {
                    if(playerFSM.state == SH_PlayerFSM.State.Groggy)
                    {
                        if(Input.GetKeyDown(KeyCode.F))
                            {
                            playerFSM.ChangeState(SH_PlayerFSM.State.Catched);
                            state = State.Carry;
                        }
                    }
                    
                }
            }

        }


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
        if (Input.GetButtonDown("Fire1") && carryTime <= 0)
        {
            state = State.Attack;
        }

        // 1��Ű ������ ���ǵ�� ��ų ����
        if (Input.GetKeyDown(KeyCode.Alpha1) && carryTime <= 0)
        {
            state = State.Skill_1;
        }

        // 2��Ű ������ ��� ��ų ����
        if (Input.GetKeyDown(KeyCode.Alpha2) && carryTime <= 0)
        {
            state = State.Skill_2;
        }

        cc.Move(dir * speed * Time.deltaTime);
    }

    float attackTime = 0;
    public GameObject hand;
    RaycastHit hit;
    SH_PlayerHP hp;

    void Attack()
    {
        Debug.DrawRay(hand.transform.position, Camera.main.transform.forward * 1.5f, Color.red * 1f);

        Ray ray = new Ray(hand.transform.position, Camera.main.transform.forward);

        if(Physics.Raycast(ray, out hit, 1.5f))
        {
            if(hit.transform.gameObject.layer == 29)
            {
                hp = hit.transform.GetComponent<SH_PlayerHP>();
            }
        }

        // �����ϸ鼭 ���������� �ٴ����� ������
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,0,transform.position.z), 10 * Time.deltaTime);

        attackTime += Time.deltaTime;

        if(attackTime > 0.5f)
        {
            if(hp != null)
            {
                hp.OnDamaged(30);
                hp = null;
            }
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
                // �Ǵް��� �Լ� ����
            }
        }

        if(skill_2Time > 1.5f)
        {
            Array.Clear(colls, 0, colls.Length); // �迭 ���� ��� ���� ����
            state = State.Move;
            skill_2Time = 0;
        }
    }

    public GameObject shoulderPos;
    float carryTime = 0;
    void Carry()
    {
        carryTime += Time.deltaTime;
        playerTr.gameObject.GetComponent<CharacterController>().enabled = false;
        //playerFSM.body.gameObject.transform.up = transform.forward;
        playerFSM.body.gameObject.transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);

        if(carryTime < 0.29)
        {
            playerTr.gameObject.transform.position = hand.transform.position;
        }
        else if(carryTime > 0.28 && carryTime < 0.32)
        {
            playerTr.gameObject.transform.position = shoulderPos.transform.position;
        }
        else if(carryTime > 0.32)
        {
            state = State.Move;
        }
    }
}
