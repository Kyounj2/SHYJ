using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class YJ_KillerMove : MonoBehaviourPun, IPunObservable
{
    public bool isNearPropMachine = false;
    public void testUI(bool b)
    {
        enemy_ui.SetActive(b);
    }

    public GameObject enemy_ui;

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

    // 닉네임 UI
    public Text nicName;

    // 도착위치
    Vector3 receivePos;

    // 회전되야 하는 값
    Quaternion receiveRot;

    // 보간속력
    public float lerpSpeed = 100;

    // PlayerState 가져오기
    State PlayerState;

    public enum State
    {
        //Idle,
        Move,
        Attack,
        Skill_1, // 3초동안 내 앞방향으로 스피드업
        Skill_2,
        Skill_3,
        Carry,
        Down,
        Die
    }

    State state;

    void Start()
    {
        if (photonView.IsMine)
            Campos.gameObject.SetActive(true);

        cc = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        state = State.Move;
        //ChangeState(State.Move);

        enemy_ui = GameObject.Find("EnemyMachineGage");

        enemy_ui.SetActive(false);
    }

    // 현재상태
    public State currState;

    // 상태 변경
    //public void ChangeState(State s)
    //{
    //    photonView.RPC("RPCChangeState", RpcTarget.All, s);
    //}

    void Update()
    {

        if (photonView.IsMine)
        {
            if(isNearPropMachine)
            {
                enemy_ui.SetActive(true);
            }
            else
            {
                enemy_ui.SetActive(false);
                enemy_ui.GetComponent<Slider>().value = 0;
            }
            //ChangeState(State.Move); 안됌

            //// 마우스커서 숨기기
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            print("현재상태 : " + state);
            KillerRot();

            switch (state)
            {
                case State.Move:
                    KillerMove();
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                    {
                        //anim.SetInteger("vv", 1);
                        photonView.RPC("RpcSetInteger", RpcTarget.All, "vv", 1);
                    }
                    else
                    {
                        //anim.SetInteger("vv", 0);
                        photonView.RPC("RpcSetInteger", RpcTarget.All, "vv", 0);
                    }

                    if (carryTime > 0.3)
                    {
                        // 3인칭 모드로 바꾸기
                        // 내손에 놓고 움직이기
                        playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, shoulderPos.transform.position);//, new Vector3(100, 0, 180));
                        //playerTr.gameObject.transform.position = shoulderPos.transform.position;
                        //playerFSM.body.gameObject.transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);
                    }
                    break;
                case State.Attack:
                    //anim.SetBool("Attack", true);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Attack", true);
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
                case State.Carry:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Carry", true);
                    //anim.SetBool("Carry", true);
                    Carry();
                    break;
                case State.Down:
                    print("다운상태로 변경하자");
                    Down();
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Carry", false);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Down", true);
                    break;
                case State.Die:
                    break;

            }

        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }

    }

    // 의자 트리거가 활성화되면
    // 플레이어를 의자포지션에 내려놓고싶다
    public bool triggerChair = false;
    public Transform chairPos;
    float downTime = 0;
    private void Down()
    {
        carryTime = 0;
        downTime += Time.deltaTime;

        playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, chairPos.transform.position);

        if(downTime > 1.5f)
        {
            print("드디어 다운하고 넘어가죠");
            photonView.RPC("RpcSetBool", RpcTarget.All, "Down", false);
            downTime = 0;
            playerFSM.ChangeState(SH_PlayerFSM.State.Seated);
            state = State.Move;
        }
    }

    public Transform Campos;
    public Transform Campos2;

    // 카메라 rot구현
    void KillerRot()
    {
        if (photonView.IsMine == false) return;

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, 1050, 1120); // 위아래 고정

        if (carryTime <= 0)
        {
            transform.eulerAngles = new Vector3(0, rotX, 0); // 일단좌우만
            Camera.main.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
            //Campos.transform.eulerAngles = new Vector3(-rotY, 0, 0);
            //Campos.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
        }
        else if (carryTime > 0.1)
        {
            if (carryTime < 0.2)
            {
                // 3인칭 카메라로 변경
                Campos.transform.position = Vector3.Lerp(Campos.transform.position, Campos2.transform.position, Time.deltaTime);
            }
            Campos.transform.position = Campos2.transform.position;
            transform.eulerAngles = new Vector3(0, rotX, 0);
            Campos.transform.forward = transform.forward;
            //Camera.main.transform.eulerAngles = new Vector3(16, 0, 0);
        }
    }

    RaycastHit player;
    Transform playerTr;
    SH_PlayerFSM playerFSM;
    // 이동구현
    void KillerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        yvel += gravity * Time.deltaTime;

        // ray쏘고다니기
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2.5f, Color.red * 1f);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out player, 2.5f))
        {
            if (player.collider.gameObject.layer == 29)
            {
                playerFSM = player.transform.GetComponent<SH_PlayerFSM>();
                playerTr = player.transform;

                if (playerFSM != null)
                {
                    if (playerFSM.state == SH_PlayerFSM.State.Groggy)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            playerFSM.ChangeState(SH_PlayerFSM.State.Catched);
                            state = State.Carry;
                        }
                    }
                }
            }
            //RpcPlayerGroggy();
            //photonView.RPC("RpcPlayerGroggy", RpcTarget.All);
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

        // 마우스 왼쪽버튼 누르면 Attack으로 바꾸기
        if (Input.GetButtonDown("Fire1") && carryTime <= 0)
        {
            state = State.Attack;
        }

        // 1번키 누르면 스피드업 스킬 가동
        if (Input.GetKeyDown(KeyCode.Alpha1) && carryTime <= 0)
        {
            state = State.Skill_1;
        }

        // 2번키 누르면 비명 스킬 가동
        if (Input.GetKeyDown(KeyCode.Alpha2) && carryTime <= 0)
        {
            state = State.Skill_2;
        }

        // 의자에 가까이 다가갔을때
        if (triggerChair)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                print("의자에앉아 짜증나니깐");
                state = State.Down;
            }
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

        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            if (hit.transform.gameObject.layer == 29)
            {
                hp = hit.transform.GetComponent<SH_PlayerHP>();
            }
        }

        // 점프하면서 공격했을때 바닥으로 내리기
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 10 * Time.deltaTime);

        attackTime += Time.deltaTime;

        if (attackTime > 0.5f)
        {
            if (hp != null)
            {
                hp.OnDamaged(30); // 혁신이꺼 Rpc로 바꾸기
                hp = null;
            }
            //anim.SetBool("Attack", false);
            photonView.RPC("RpcSetBool", RpcTarget.All, "Attack", false);
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

        if (Input.GetButtonDown("Fire1"))
        {
            state = State.Attack;
        }

        if (skill_1Time > 1f)
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

        // 반경 5미터 내의 콜라이더들을 수집
        colls = Physics.OverlapSphere(transform.position, 5f);

        for (int i = 0; i < colls.Length; i++)
        {
            // 플레이어가 있다면
            if (colls[i].gameObject.layer == 29)
            {
                // colls의 게임오브젝트에서 데미지 함수 실행
                hp = colls[i].gameObject.GetComponent<SH_PlayerHP>();
                hp.OnDamaged(10);
                break;
            }
        }

        if (skill_2Time > 1.5f)
        {
            Array.Clear(colls, 0, colls.Length); // 배열 안의 목록 전부 삭제
            state = State.Move; // 무브로 다시 변경
            skill_2Time = 0;
        }
    }

    public GameObject shoulderPos;
    float carryTime = 0;
    Vector3 handVec;
    Vector3 shoulderVec;
    void Carry()
    {
        carryTime += Time.deltaTime;
        playerTr.gameObject.GetComponent<CharacterController>().enabled = false;
        //playerFSM.body.gameObject.transform.up = transform.forward;
        //playerFSM.body.gameObject.transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);

        handVec = hand.transform.position;
        shoulderVec = shoulderPos.transform.position;

        if (carryTime < 0.29)
        {
            //playerTr.gameObject.transform.position = hand.transform.position;
            playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, handVec); //, new Vector3(100, 0, 0));
            //photonView.RPC("RpcPlayerPos", RpcTarget.All, handVec);
        }
        else if (carryTime > 0.28 && carryTime < 0.32)
        {
            //playerTr.gameObject.transform.position = shoulderPos.transform.position;
            playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, shoulderVec); //, new Vector3(100, 0, 180));
            //photonView.RPC("RpcPlayerPos", RpcTarget.All, shoulderVec);
        }
        else if (carryTime > 0.32)
        {
            state = State.Move;
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기
        if (stream.IsWriting) // 내가 데이터를 보낼 수 있는 상태인 경우 (ismine)
        {
            // positon, rotation
            stream.SendNext(transform.position); // Value타입만 보낼 수 있음
            stream.SendNext(transform.rotation);
        }
        // 데이터 받기
        else // if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext(); // 강제형변환필요
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // 네트워크모음
    [PunRPC]
    void RpcPlayerGroggy()
    {
        //if (player.collider.gameObject.layer == 29)
        //{
        //    playerFSM = player.transform.GetComponent<SH_PlayerFSM>();
        //    playerTr = player.transform;

        //    if (playerFSM != null)
        //    {
        //        if (playerFSM.state == SH_PlayerFSM.State.Groggy)
        //        {
        //            if (Input.GetKeyDown(KeyCode.F))
        //            {
        //                playerFSM.ChangeState(SH_PlayerFSM.State.Catched);
        //                state = State.Carry;
        //            }
        //        }
        //    }
        //}
    }

    [PunRPC]
    public void RpcSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RpcSetInteger(string s, int i)
    {
        anim.SetInteger(s, i);
    }

    [PunRPC]
    public void RpcSetBool(string s, bool b)
    {
        anim.SetBool(s, b);
    }

    

    //[PunRPC]
    //public void RPCChangeState(State s)
    //{
    //    switch (s)
    //    {
    //        case State.Move:
    //            print("된거야만거야 짜증나게");
    //            //KillerMove();
    //            //photonView.RPC("RPCKillerMove", RpcTarget.All);
    //            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
    //            {
    //                //anim.SetInteger("vv", 1);
    //                photonView.RPC("RpcSetInteger", RpcTarget.All, "vv", 1);
    //            }
    //            else
    //            {
    //                //anim.SetInteger("vv", 0);
    //                photonView.RPC("RpcSetInteger", RpcTarget.All, "vv", 0);
    //            }

    //            if (carryTime > 0.3)
    //            {
    //                playerTr.gameObject.transform.position = shoulderPos.transform.position;
    //                playerFSM.body.gameObject.transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);
    //            }
    //            break;
    //        case State.Attack:
    //            anim.SetBool("Attack", true);
    //            Attack();
    //            break;
    //        case State.Skill_1: // 스피드업
    //            Skill_SpeedUp();
    //            break;
    //        case State.Skill_2: // 비명지르기
    //            Skill_Scream();
    //            break;
    //        case State.Skill_3: // 발전기 저주
    //            break;
    //        case State.Carry:
    //            anim.SetBool("Carry", true);
    //            Carry();
    //            break;
    //        case State.Die:
    //            break;

    //    }
    //}

    //[PunRPC]
    //public void RPCKillerMove()
    //{
    //    float h = Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");

    //    yvel += gravity * Time.deltaTime;

    //    // ray쏘고다니기
    //    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2.5f, Color.red * 1f);
    //    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    //    if (Physics.Raycast(ray, out player, 2.5f))
    //    {
    //        photonView.RPC("RpcPlayerGroggy", RpcTarget.All);
    //    }

    //    if (cc.isGrounded)
    //    {
    //        yvel = 0;
    //        jumpCount = 0;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
    //    {
    //        yvel = jumpPower;
    //        jumpCount++;
    //    }

    //    dir = transform.right * h + transform.forward * v;
    //    dir.Normalize();

    //    dir.y = yvel;

    //    // 마우스 왼쪽버튼 누르면 Attack으로 바꾸기
    //    if (Input.GetButtonDown("Fire1") && carryTime <= 0)
    //    {
    //        state = State.Attack;
    //    }

    //    // 1번키 누르면 스피드업 스킬 가동
    //    if (Input.GetKeyDown(KeyCode.Alpha1) && carryTime <= 0)
    //    {
    //        state = State.Skill_1;
    //    }

    //    // 2번키 누르면 비명 스킬 가동
    //    if (Input.GetKeyDown(KeyCode.Alpha2) && carryTime <= 0)
    //    {
    //        state = State.Skill_2;
    //    }

    //    cc.Move(dir * speed * Time.deltaTime);


    //}
}
