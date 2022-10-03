using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class SH_PlayerMove : MonoBehaviourPun, IPunObservable
{
    public Transform player;
    public Transform camPivot;
    Animator anim;
    [HideInInspector] public CharacterController cc;

    float speed;
    public float walkSpeed = 10;
    public float runSpeed = 15;
    public float groggySpeed = 5;
    public float transformSpeed = 10;
    Vector3 dir;

    public float jumpPower = 5;
    public float jumpRayLen = 1.2f;
    public Transform[] rayBase;
    float gravity = -5;
    float yVelocity = 0;

    public int maxJumpCount = 1;
    int jumpCount = 0;

    // 닉네임 UI
    public Text nickName;

    // 도착 위치
    Vector3 receivePos;
    // 회전되야 하는 값
    Quaternion receiveRot;
    // 보간 속력
    public float lerpSpeed = 100;

    // PlayerState 컴포넌트
    SH_PlayerFSM fsm;

    public float stamina;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        // 닉네임 설정
        //nickName.text = photonView.Owner.NickName;

        // PlayerState 컴포넌트 가져오기
        fsm = GetComponent<SH_PlayerFSM>();
    }
    
    public void PlayerMovement()
    {
        if (photonView.IsMine)
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

            photonView.RPC("RpcSetWalkFloat", RpcTarget.All, v, h);

            speed = ChangeSpeed(fsm.state);

            cc.Move(dir * speed * Time.deltaTime);
        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    //public void TransformedMovement()
    //{
    //    if (photonView.IsMine)
    //    {
    //        float v = Input.GetAxisRaw("Vertical");
    //        float h = Input.GetAxisRaw("Horizontal");

    //        dir = camPivot.forward * v + camPivot.right * h;
    //        dir = new Vector3(dir.x, 0, dir.z);
    //        dir.Normalize();

    //        yVelocity += gravity * Time.deltaTime;

    //        if (cc.isGrounded)
    //        {
    //            yVelocity = 0;
    //            jumpCount = 0;
    //        }

    //        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
    //        {
    //            yVelocity = jumpPower;
    //            jumpCount++;
    //        }

    //        dir.y = yVelocity;

    //        photonView.RPC("RpcSetWalkFloat", RpcTarget.All, v, h);

    //        speed = ChangeSpeed(fsm.state);

    //        cc.Move(dir * speed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
    //        transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
    //    }
    //}

    private float ChangeSpeed(SH_PlayerFSM.State s)
    {
        switch (s)
        {
            case SH_PlayerFSM.State.Normal:
                if (Input.GetKey(KeyCode.LeftShift))
                    return runSpeed;
                else
                    return walkSpeed;

            case SH_PlayerFSM.State.Transform:
                return transformSpeed;

            case SH_PlayerFSM.State.Groggy:
                return groggySpeed;

            default:
                return 0;
        }
    }

    [PunRPC]
    public void RpcSetWalkFloat(float v, float h)
    {
        anim.SetFloat("Speed", v);
        anim.SetFloat("Direction", h);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 데이터 보내기
        if (stream.IsWriting) // isMine == true
        {
            // position, rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        // 데이터 받기
        else if (stream.IsReading) // isMine == false
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
