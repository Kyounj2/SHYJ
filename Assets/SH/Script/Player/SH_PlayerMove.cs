using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SH_PlayerMove : MonoBehaviourPun, IPunObservable
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

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        // 닉네임 설정
        nickName.text = photonView.Owner.NickName;

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

            photonView.RPC("RpcSetFloat", RpcTarget.All, v);

            cc.Move(dir * walkSpeed * Time.deltaTime);
        }
        else
        {
            // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
    }

    [PunRPC]
    public void RpcSetFloat(float v)
    {
        anim.SetFloat("Speed", v);
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
