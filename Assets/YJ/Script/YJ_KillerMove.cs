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
    public GameObject player_ui;

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

    // �г��� UI
    public Text nicName;

    // ������ġ
    Vector3 receivePos;

    // ȸ���Ǿ� �ϴ� ��
    Quaternion receiveRot;

    // �����ӷ�
    public float lerpSpeed = 100;

    // PlayerState ��������
    State PlayerState;

    // 1��Ī ī�޶���� ����
    Transform cameraOriginPos;

    // ��ų ��Ÿ�� �˷��� UI
    public GameObject canvas;

    // �����
    AudioSource audio;

    // ��ݸ��
    [SerializeField]
    [Header("Sound")]
    public AudioClip Attack_Sound;
    public AudioClip Attack_Hit_Sound;
    public AudioClip Skill_1_Sound;
    public AudioClip Skill_2_Sound;
    public AudioClip Break_propmaghine_Sound;
    public AudioClip Chair_Sound;
    public AudioClip propAttack_Sound;

    public enum State
    {
        //Idle,
        Move,
        Attack,
        Skill_1, // 3�ʵ��� �� �չ������� ���ǵ��
        Skill_2,
        Skill_3,
        Carry,
        Down,
        MachineAttack
    }

    State state;

    void Start()
    {
        if (photonView.IsMine)
        {
            Campos.gameObject.SetActive(true);
            cameraOriginPos = Campos.transform;
            canvas.SetActive(true);
        }

        cc = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        state = State.Move;
        //ChangeState(State.Move);

        enemy_ui = GameObject.Find("EnemyMachineGage");
        enemy_ui.SetActive(false);

        // ������ ui ã��
        blood_1 = GameObject.Find("blood").GetComponent<Image>();
        blood_2 = GameObject.Find("blood (1)").GetComponent<Image>();

        audio = GetComponent<AudioSource>();

    }

    // �������
    public State currState;

    // �ֳʹ� ���¸� �ӽž������� �ٲ��� bool��
    public bool machineAttack = false;


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

            //// ���콺Ŀ�� �����
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            KillerRot();

            switch (state)
            {
                case State.Move:                    
                    KillerMove();
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Skill_1", false);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Skill_2", false);
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
                        // ���տ� ���� �����̱�
                        playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, shoulderPos.transform.position);
                        playerTr.GetComponent<PhotonView>().RPC("RpcPlayerRot", RpcTarget.All, transform.forward);

                    }
                    break;
                case State.Attack:
                    photonView.RPC("RpcSetInteger", RpcTarget.All, "vv", 0);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Skill_1", false);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Attack", true);
                    Attack();
                    break;
                case State.Skill_1: // ���ǵ��
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Skill_1", true);
                    Skill_SpeedUp();
                    break;
                case State.Skill_2: // ���������
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Skill_2", true);
                    Skill_Scream();
                    break;
                case State.Skill_3: // ������ ����
                    break;
                case State.Carry:
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Carry", true);
                    Carry();
                    break;
                case State.Down:
                    Down();
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Carry", false);
                    photonView.RPC("RpcSetBool", RpcTarget.All, "Down", true);
                    break;
                case State.MachineAttack:
                    MachineAttack();
                    break;

            }

        }
        else
        {
            // Lerp�� �̿��ؼ� ������, ����������� �̵� �� ȸ��
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }

    }

    // ���� Ʈ���Ű� Ȱ��ȭ�Ǹ�
    // �÷��̾ ���������ǿ� ��������ʹ�
    public bool triggerChair = false;
    public Transform chairPos;
    float downTime = 0;
    private void Down()
    {
        Campos.transform.position = Vector3.Lerp(Campos.transform.position, camPosOrigin.transform.position, 5 * Time.deltaTime);
        carryTime = 0;
        downTime += Time.deltaTime;

        playerTr.GetComponent<PhotonView>().RPC("RpcPlayerPos", RpcTarget.All, chairPos.transform.position);
        //playerTr.gameObject.transform.forward = -chairPos.right;

        if (downTime > 1.5f)
        {
            photonView.RPC("RpcSetBool", RpcTarget.All, "Down", false);
            playerFSM.ChangeState(SH_PlayerFSM.State.Seated);
            state = State.Move;
            downTime = 0;
        }
    }

    public Transform camPosOrigin;
    public Transform Campos;
    public Transform Campos2;
    public Transform Campos3;
    public bool propmachineFOn = false;
    bool stop = false;
    float stopTime = 0;
    float sinTime = 0;

    // ī�޶� rot����
    void KillerRot()
    {
        if (photonView.IsMine == false) return;

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, 1050, 1120); // ���Ʒ� ����

        print(Vector3.Distance(Campos.transform.position, Campos3.transform.position) < 1);

        // 1��Ī ī�޶�
        if (carryTime <= 0 && !propmachineFOn && !stop)
        {
            if (state == State.Skill_1)
            {
                float rotZ = Mathf.Sin(120 * skill_1Time) * 50f * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, rotX, 0); // �ϴ��¿츸
                Camera.main.transform.eulerAngles = new Vector3(-rotY, rotX, rotZ);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, rotX, 0); // �ϴ��¿츸
                Camera.main.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
            }
        }
        else if (carryTime > 0.1 && !propmachineFOn)
        {
            if (carryTime < 0.2)
            {
                // 3��Ī ī�޶�� ����
                Campos.transform.position = Vector3.Lerp(Campos.transform.position, Campos2.transform.position, Time.deltaTime);
            }
            Campos.transform.position = Campos2.transform.position;
            transform.eulerAngles = new Vector3(0, rotX, 0);
            Campos.transform.forward = transform.forward;
        }
        else if (propmachineFOn && !stop && Input.GetKey(KeyCode.F))
        {
            Campos.transform.position = Vector3.Lerp(Campos.transform.position, Campos3.transform.position, Time.deltaTime * 0.5f);
            if (Vector3.Distance(Campos.transform.position, Campos3.transform.position) < 4)
            {
                //Campos.transform.position = Campos3.transform.position;
                // �����纼��..? 0.5������..?
                stopTime += Time.deltaTime;
                if (stopTime > 0.5)
                {
                    stop = true;
                    stopTime = 0;
                }
            }
        }
        else if ( stop )
        {
            sinTime += Time.deltaTime;
            sinTime = Math.Clamp(sinTime, 0, 1);
            Campos.transform.position = Vector3.Lerp(Campos.transform.position, camPosOrigin.transform.position, Time.deltaTime * 2);
            float rotY = Mathf.Sin((3.14f/0.5f) * sinTime) * 20;
            //transform.eulerAngles = new Vector3(0, rotX, 0); // �ϴ��¿츸
            Camera.main.transform.eulerAngles = new Vector3(-rotY, rotX, 0);
            if (Vector3.Distance(Campos.transform.position, camPosOrigin.transform.position) < 0.2)
            {
                Campos.transform.position = camPosOrigin.transform.position;
                sinTime = 0;
                stop = false;
            }
        }
    }

    RaycastHit player;
    Transform playerTr;
    SH_PlayerFSM playerFSM;
    // �̵�����
    void KillerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        yvel += gravity * Time.deltaTime;

        // ray���ٴϱ�
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 4f, Color.red * 1f);
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
            audio.clip = Attack_Sound;
            audio.Play();
            state = State.Attack;
        }

        // 1��Ű ������ ���ǵ�� ��ų ����
        if (Input.GetKeyDown(KeyCode.Alpha1) && carryTime <= 0 && !canvas.GetComponent<YJ_Skill>().skill_1On)
        {
            audio.clip = Skill_1_Sound;
            audio.Play();

            camPosSave = Camera.main.transform.localPosition;
            goDir = Camera.main.transform.forward;

            state = State.Skill_1;
        }

        // 2��Ű ������ ��� ��ų ����
        if (Input.GetKeyDown(KeyCode.Alpha2) && carryTime <= 0 && !canvas.GetComponent<YJ_Skill>().skill_2On)
        {
            audio.clip = Skill_2_Sound;
            audio.Play(); // ����           
            wave.Play(); // ��ƼŬ
            state = State.Skill_2;
        }

        // ���ڿ� ������ �ٰ�������
        if(playerFSM != null)
        {
            if (triggerChair && playerFSM.state == SH_PlayerFSM.State.Catched)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    audio.clip = Chair_Sound;
                    audio.Play();
                    state = State.Down;
                }
            }
        }

        // �ӽ� �μ����� ������ȯ
        if (machineAttack)
        {
            state = State.MachineAttack;
        }

        cc.Move(dir * speed * Time.deltaTime);
    }

    float attackTime = 0;
    public GameObject hand;
    RaycastHit hit;
    SH_PlayerHP hp;

    void Attack()
    {
        Debug.DrawRay(hand.transform.position, Camera.main.transform.forward * 2.5f, Color.red * 1f);

        Ray ray = new Ray(hand.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            if (hit.transform.gameObject.layer == 29)
            {
                hp = hit.transform.GetComponent<SH_PlayerHP>();
            }
        }

        // �����ϸ鼭 ���������� �ٴ����� ������
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), 10 * Time.deltaTime);

        attackTime += Time.deltaTime;

        if (attackTime > 0.5f)
        {
            if (hp != null)
            {
                audio.clip = Attack_Hit_Sound;
                audio.Play();
                StartCoroutine(OnAttackUI());
                hp.OnDamaged(30); // �����̲� Rpc�� �ٲٱ�
                hp = null;
            }
            //anim.SetBool("Attack", false);
            photonView.RPC("RpcSetBool", RpcTarget.All, "Attack", false);
            state = State.Move;
            attackTime = 0f;
        }
    }

    float skill_1Time = 0;
    Vector3 camPosSave;
    Vector3 goDir;
    // ��ų1��
    void Skill_SpeedUp()
    {
        canvas.GetComponent<YJ_Skill>().skill_1On = true;
        skill_1Time += Time.deltaTime;
        cc.Move(goDir.normalized * (speed * 5) * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            state = State.Attack;
            skill_1Time = 0;
        }

        if (skill_1Time > 1f)
        {
            state = State.Move;
            skill_1Time = 0;
        }
    }


    float skill_2Time = 0;
    public Collider[] colls;
    int dontAgain = 0;

    // ��ų 2 ���� ����� ��
    public ParticleSystem wave;

    void Skill_Scream()
    {
        canvas.GetComponent<YJ_Skill>().skill_2On = true;
        skill_2Time += Time.deltaTime;

        // �ݰ� 5���� ���� �ݶ��̴����� ����
        colls = Physics.OverlapSphere(transform.position, 5f);

        if (skill_2Time > 1)
        {
            Array.Clear(colls, 0, colls.Length); // �迭 ���� ��� ���� ����
            state = State.Move; // ����� �ٽ� ����
            dontAgain = 0;
            skill_2Time = 0;
        }
        else if(dontAgain < 1)
        {
            dontAgain++;
            for (int i = 0; i < colls.Length; i++)
            {
                // �÷��̾ �ִٸ�
                if (colls[i].gameObject.layer == 29)
                {
                    //OnAttackUI();
                    // colls�� ���ӿ�����Ʈ���� ������ �Լ� ����
                    hp = colls[i].gameObject.GetComponent<SH_PlayerHP>();
                    if (hp != null)
                    {
                        StartCoroutine(OnAttackUI());
                        audio.clip = Attack_Hit_Sound;
                        audio.Play();
                        hp.OnDamaged(10);
                    }
                    hp = null;
                }
            }
        }

    }

    Image blood_1;
    Color blood1_C;

    Image blood_2;
    Color blood2_C;

    IEnumerator OnAttackUI()
    {
        blood1_C = blood_1.color;
        blood1_C.a = 0.3f;
        blood_1.color = blood1_C;

        blood2_C = blood_2.color;
        blood2_C.a = 0.3f;
        blood_2.color = blood2_C;

        yield return new WaitForSeconds(0.5f);

        blood1_C.a = 0;
        blood2_C.a = 0;
        blood_1.color = blood1_C;
        blood_2.color = blood2_C;

        //yield break;
    }


    public GameObject shoulderPos;
    float carryTime = 0;
    Vector3 handVec;
    Vector3 shoulderVec;
    void Carry()
    {
        carryTime += Time.deltaTime;
        playerTr.gameObject.GetComponent<CharacterController>().enabled = false;

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
            playerTr.gameObject.transform.forward = transform.forward;
            //photonView.RPC("RpcPlayerPos", RpcTarget.All, shoulderVec);
        }
        else if (carryTime > 0.32)
        {
            state = State.Move;
        }
    }

    float machineAttackTime = 0;
    public ParticleSystem smoke1, smoke2, smoke3;

    void MachineAttack()
    {
        machineAttackTime += Time.deltaTime;
        photonView.RPC("RpcSetBool", RpcTarget.All, "MachineAttack", true);

        if(machineAttackTime > 0.3)
        {
            audio.clip = propAttack_Sound;
            audio.Play();
            photonView.RPC("RpcParticle", RpcTarget.All);
        }

        // machineAttackTime�� �ִϸ��̼� �÷��� �ð����� Ŀ���� ����� �ǵ�����
        if (machineAttackTime > (float)anim.GetCurrentAnimatorStateInfo(2).length)
        {
            photonView.RPC("RpcSetBool", RpcTarget.All, "MachineAttack", false);
            state = State.Move;
            propmachineFOn = false;
            machineAttack = false;
            machineAttackTime = 0;
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ������ ������
        if (stream.IsWriting) // ���� �����͸� ���� �� �ִ� ������ ��� (ismine)
        {
            // positon, rotation
            stream.SendNext(transform.position); // ValueŸ�Ը� ���� �� ����
            stream.SendNext(transform.rotation);
        }
        // ������ �ޱ�
        else // if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext(); // ��������ȯ�ʿ�
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // ��Ʈ��ũ����
    [PunRPC]
    public void RpcSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RpcSetInteger(string s, int i)
    {
        if(anim)
        {
            anim.SetInteger(s, i);
        }
    }

    [PunRPC]
    public void RpcSetBool(string s, bool b)
    {
        anim.SetBool(s, b);
    }

    [PunRPC]
    public void RpcParticle()
    {
        smoke1.Play();
        smoke2.Play();
        smoke3.Play();
    }
}
